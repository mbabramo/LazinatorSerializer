using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Immutable;
using Lazinator.CodeDescription;
using LazinatorAnalyzer.AttributeClones;
using LazinatorAnalyzer.Analyzer;
using LazinatorAnalyzer.Roslyn;
using LazinatorAnalyzer.Settings;

namespace LazinatorCodeGen.Roslyn
{
    public class LazinatorCompilation
    {

        // We must be able to determine whether a type implements various methods, but we want to exclude methods implemented in code behind
        static readonly string[] _methodNamesToLookFor = new string[] { "LazinatorObjectVersionUpgrade", "PreSerialization", "PostDeserialization", "OnDirty", "ConvertFromBytesAfterHeader", "WritePropertiesIntoBuffer", "EnumerateLazinatorDescendants" };

        public Compilation Compilation;
        public LazinatorConfig Config { get; private set; }
        public INamedTypeSymbol ImplementingTypeSymbol { get; private set; }
        public Accessibility ImplementingTypeAccessibility { get; private set; }
        internal List<TypeDeclarationSyntax> TypeDeclarations { get; private set; }
        internal HashSet<ISymbol> RelevantSymbols = new HashSet<ISymbol>();
        internal HashSet<INamedTypeSymbol> ExclusiveInterfaces = new HashSet<INamedTypeSymbol>();
        internal HashSet<INamedTypeSymbol> NonexclusiveInterfaces = new HashSet<INamedTypeSymbol>();
        internal Dictionary<INamedTypeSymbol, HashSet<INamedTypeSymbol>> InterfaceToClasses = new Dictionary<INamedTypeSymbol, HashSet<INamedTypeSymbol>>();
        public Dictionary<INamedTypeSymbol, List<PropertyWithDefinitionInfo>> PropertiesForType { get; internal set; } = new Dictionary<INamedTypeSymbol, List<PropertyWithDefinitionInfo>>();
        internal Dictionary<INamedTypeSymbol, INamedTypeSymbol> TypeToExclusiveInterface = new Dictionary<INamedTypeSymbol, INamedTypeSymbol>();
        internal HashSet<(INamedTypeSymbol type, string methodName)> TypeImplementsMethod = new HashSet<(INamedTypeSymbol type, string methodName)>();
        internal bool ImplementingTypeRequiresParameterlessConstructor { get; set; }
        internal Dictionary<ISymbol, HashSet<Attribute>> KnownAttributes = new Dictionary<ISymbol, HashSet<Attribute>>();
        internal Dictionary<INamedTypeSymbol, List<(IParameterSymbol parameterSymbol, IPropertySymbol property)>> RecordLikeTypes = new Dictionary<INamedTypeSymbol, List<(IParameterSymbol parameterSymbol, IPropertySymbol property)>>();
        internal HashSet<INamedTypeSymbol> NonRecordLikeTypes = new HashSet<INamedTypeSymbol>();
        internal Dictionary<INamedTypeSymbol, Guid> InterfaceTextHash = new Dictionary<INamedTypeSymbol, Guid>();

        public LazinatorCompilation(Compilation compilation, Type type, LazinatorConfig config) : this(compilation, RoslynHelpers.GetNameWithoutGenericArity(type), type.FullName, config)
        {
        }

        public LazinatorCompilation(Compilation compilation, string implementingTypeName, string fullImplementingTypeName, LazinatorConfig config)
        {
            Config = config;
            Initialize(compilation, implementingTypeName, fullImplementingTypeName);
        }

        public void Initialize(Compilation compilation, string implementingTypeName, string fullImplementingTypeName)
        {
            Compilation = compilation;

            ImplementingTypeSymbol = Compilation.GetTypeByMetadataName(fullImplementingTypeName);

            TypeDeclarations = GetTypeDeclarationsForNamedType(ImplementingTypeSymbol).ToList();
            if (TypeDeclarations == null || !TypeDeclarations.Any())
                throw new LazinatorCodeGenException($"Internal Lazinator error. Implementing type declaration for {implementingTypeName} not found.");

            ImplementingTypeAccessibility = TypeDeclarations.First().GetAccessibility();
            INamedTypeSymbol lazinatorTypeAttribute = Compilation.GetTypeByMetadataName(LazinatorCompilationAnalyzer.LazinatorAttributeName);
            INamedTypeSymbol exclusiveInterfaceTypeSymbol = ImplementingTypeSymbol.GetTopLevelInterfaceImplementingAttribute(lazinatorTypeAttribute);
            if (exclusiveInterfaceTypeSymbol == null)
                throw new LazinatorCodeGenException($"Type {ImplementingTypeSymbol.Name} should implement exactly one Lazinator interface (plus any Lazinator interfaces implemented by that interface, plus any number of nonexclusive interfaces).");
            Initialize(TypeDeclarations, ImplementingTypeSymbol, exclusiveInterfaceTypeSymbol);
        }

        public void AnalyzeAnotherType(string implementingTypeName, string fullImplementingTypeName)
        {

        }

        private void Initialize(List<TypeDeclarationSyntax> implementingTypeDeclarations, INamedTypeSymbol implementingTypeSymbol, INamedTypeSymbol exclusiveInterfaceTypeSymbol)
        {
            // Record information about the type, its interfaces, and inner types.
            RecordInformationAboutTypeAndRelatedTypes(implementingTypeSymbol);

            // Then deal with any implementing classes -- in particular, checking whether each implements methods.
            TypeImplementsMethod = GetMethodImplementations(implementingTypeDeclarations, implementingTypeSymbol);
            ImplementingTypeRequiresParameterlessConstructor = RequiresParameterlessConstructor(implementingTypeSymbol, implementingTypeDeclarations);

            // Now, record the interface text hash
            RecordInterfaceTextHash(exclusiveInterfaceTypeSymbol, implementingTypeSymbol);

            // And save the pair
            TypeToExclusiveInterface[implementingTypeSymbol] = exclusiveInterfaceTypeSymbol;
        }

        private HashSet<IPropertySymbol> ILazinatorProperties = null; 
        private void RecordILazinatorProperties()
        {
            INamedTypeSymbol iLazinatorSymbol = Compilation.GetTypeByMetadataName("Lazinator.Core.ILazinator");
            ILazinatorProperties = new HashSet<IPropertySymbol>(iLazinatorSymbol.GetPropertySymbols());
        }
        
        private void RecordPropertiesForInterface(INamedTypeSymbol @interface)
        {
            if (ILazinatorProperties == null)
                RecordILazinatorProperties();
            List<PropertyWithDefinitionInfo> propertiesInInterfaceWithLevel = new List<PropertyWithDefinitionInfo>();
            foreach (var propertyWithLevelInfo in GetPropertyWithDefinitionInfo(@interface))
            {
                if (!ILazinatorProperties.Contains(propertyWithLevelInfo.Property))
                { // ignore a property that is actually an ILazinator property rather than a property we are looking for
                    propertiesInInterfaceWithLevel.Add(propertyWithLevelInfo);
                    RelevantSymbols.Add(propertyWithLevelInfo.Property);
                }
            }
            PropertiesForType[@interface] = propertiesInInterfaceWithLevel;
            foreach (var propertyWithLevel in propertiesInInterfaceWithLevel)
            {
                IPropertySymbol property = propertyWithLevel.Property;
                AddKnownAttributesForSymbol(property);
                propertyWithLevel.SpecifyDerivationKeyword(GetDerivationKeyword(property));
                RecordInformationAboutTypeAndRelatedTypes(property.Type);
            }
        }

        public List<PropertyWithDefinitionInfo> GetPropertyWithDefinitionInfo(
            INamedTypeSymbol namedTypeSymbol)
        {
            return DistinctBy(GetPropertiesWithDefinitionInfoHelper(namedTypeSymbol).ToList(), x => x.Property.Name).ToList(); // ordinarily, we're not getting duplicate items. But sometimes we are.
        }

        private static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public IEnumerable<PropertyWithDefinitionInfo> GetPropertiesWithDefinitionInfoHelper(INamedTypeSymbol namedTypeSymbol)
        {
            // check whether there are lower level abstract or open generic types 
            Dictionary<INamedTypeSymbol, ImmutableList<IPropertySymbol>> lowerLevelInterfaces = null;
            if (namedTypeSymbol.TypeKind == TypeKind.Interface && namedTypeSymbol.HasAttributeOfType<CloneLazinatorAttribute>())
            {
                lowerLevelInterfaces = namedTypeSymbol.GetInterfacesWithAttributeOfType<CloneLazinatorAttribute>()
                    .Select(x => new KeyValuePair<INamedTypeSymbol, ImmutableList<IPropertySymbol>>(x, x.GetPropertySymbols()))
                    .ToDictionary(x => x.Key, x => x.Value);
            }

            namedTypeSymbol.GetPropertiesForType(out ImmutableList<IPropertySymbol> propertiesThisLevel, out ImmutableList<IPropertySymbol> propertiesLowerLevels);
            foreach (var p in propertiesThisLevel.OrderBy(x => x.Name).Where(x => !x.HasAttributeOfType<CloneDoNotAutogenerateAttribute>()))
                yield return new PropertyWithDefinitionInfo(p, PropertyWithDefinitionInfo.Level.IsDefinedThisLevel);
            foreach (var p in propertiesLowerLevels.OrderBy(x => x.Name).Where(x => !x.HasAttributeOfType<CloneDoNotAutogenerateAttribute>()))
            {
                if (lowerLevelInterfaces != null && lowerLevelInterfaces.Any(x => x.Value.Any(y => y.Name == p.Name)))
                    yield return
                        new PropertyWithDefinitionInfo(p,
                            PropertyWithDefinitionInfo.Level.IsDefinedInLowerLevelInterface);
                else
                    yield return
                        new PropertyWithDefinitionInfo(p,
                            PropertyWithDefinitionInfo.Level.IsDefinedLowerLevelButNotInInterface);
            }
        }


        public string GetDerivationKeyword(IPropertySymbol symbol)
        {
            var attribute = GetFirstAttributeOfType<CloneDerivationKeywordAttribute>(symbol);
            if (attribute == null)
                return null;
            if (attribute.Choice != "virtual" && attribute.Choice != "override" && attribute.Choice != "new")
                throw new LazinatorCodeGenException($"Property {symbol.Name}'s DerivationKeyWordAttribution must have choice of 'virtual', 'override', or 'new'.");
            return attribute.Choice + " ";
        }

        private void RecordInformationAboutTypeAndRelatedTypes(ITypeSymbol type)
        {
            foreach (ITypeSymbol t in GetTypeAndRelatedTypes(type))
                RecordInformationAboutType(t);
        }
        
        private void RecordInformationAboutType(ITypeSymbol type)
        {
            if (type.ContainingNamespace != null &&  type.ContainingNamespace.Name == "System" && type.ContainingNamespace.ContainingNamespace.Name == "")
                return;
            if (RelevantSymbols.Contains(type))
                return;
            RelevantSymbols.Add(type);
            if (type is INamedTypeSymbol namedTypeSymbol)
            {
                AddKnownAttributesForSymbol(type);
                if (namedTypeSymbol.TypeKind == TypeKind.Interface && GetFirstAttributeOfType<CloneLazinatorAttribute>(namedTypeSymbol) == null && GetFirstAttributeOfType<CloneNonexclusiveLazinatorAttribute>(namedTypeSymbol) == null)
                    return; // don't worry about IEnumerable etc.
                
                List<INamedTypeSymbol> allInterfaces = namedTypeSymbol.AllInterfaces.Where(x => x != type && x.Name != "ILazinator")
                    .OrderByDescending(x => namedTypeSymbol.Interfaces.Contains(x))
                    .ThenByDescending(x => x.AllInterfaces.Count())
                    .ToList();
                foreach (var @interface in allInterfaces)
                {
                    if (@interface.GetFullNamespace().StartsWith("System.Collections"))
                        continue;
                    RecordInformationAboutTypeAndRelatedTypes(@interface);
                    AddLinkFromTypeToInterface(namedTypeSymbol, @interface);
                }
                if (namedTypeSymbol.TypeKind == TypeKind.Class || namedTypeSymbol.TypeKind == TypeKind.Struct)
                {
                    foreach (var @interface in allInterfaces)
                    {
                        LazinatorAnalyzer.AttributeClones.CloneLazinatorAttribute attribute = GetFirstAttributeOfType<LazinatorAnalyzer.AttributeClones.CloneLazinatorAttribute>(@interface);
                        if (attribute != null)
                        {
                            AddLinkFromTypeToInterface(namedTypeSymbol, @interface);
                            break;
                        }
                    }
                    ConsiderAddingAsRecordLikeType(namedTypeSymbol);
                    if (namedTypeSymbol.BaseType != null)
                        RecordInformationAboutTypeAndRelatedTypes(namedTypeSymbol.BaseType);
                }
                else if (namedTypeSymbol.TypeKind == TypeKind.Interface)
                {
                    if (GetFirstAttributeOfType<LazinatorAnalyzer.AttributeClones.CloneLazinatorAttribute>(type) != null)
                    {
                        ExclusiveInterfaces.Add(namedTypeSymbol);
                        RecordPropertiesForInterface(namedTypeSymbol);
                    }
                    else if (GetFirstAttributeOfType<LazinatorAnalyzer.AttributeClones.CloneNonexclusiveLazinatorAttribute>(type) != null)
                        NonexclusiveInterfaces.Add(namedTypeSymbol);
                }
            }
        }

        private void AddLinkFromTypeToInterface(INamedTypeSymbol namedTypeSymbol, INamedTypeSymbol @interface)
        {
            if (namedTypeSymbol.TypeKind != TypeKind.Interface && ExclusiveInterfaces.Contains(@interface) && !TypeToExclusiveInterface.ContainsKey(namedTypeSymbol))
                TypeToExclusiveInterface[namedTypeSymbol.OriginalDefinition] = @interface.OriginalDefinition;
        }

        HashSet<ITypeSymbol> alreadyProduced = new HashSet<ITypeSymbol>();
        private IEnumerable<ITypeSymbol> GetTypeAndRelatedTypes(ITypeSymbol type)
        {
            if (alreadyProduced.Contains(type))
                yield break;
            alreadyProduced.Add(type);
            yield return type;
            if (type is INamedTypeSymbol namedType)
            {
                foreach (ITypeSymbol t in namedType.TypeArguments)
                    foreach (ITypeSymbol t2 in GetTypeAndRelatedTypes(t))
                    {
                        alreadyProduced.Add(t2);
                        yield return t2;
                    }
                if (namedType.TupleUnderlyingType != null)
                    foreach (ITypeSymbol t in GetTypeAndRelatedTypes(namedType.TupleUnderlyingType))
                    {
                        alreadyProduced.Add(t);
                        yield return t;
                    }
            }
            else if (type is IArrayTypeSymbol arrayType)
            {
                foreach (ITypeSymbol t in GetTypeAndRelatedTypes(arrayType.ElementType))
                {
                    alreadyProduced.Add(t);
                    yield return t;
                }
            }
            if (type != type.OriginalDefinition)
                foreach (ITypeSymbol t in GetTypeAndRelatedTypes(type.OriginalDefinition))
                {
                    alreadyProduced.Add(t);
                    yield return t;
                }
            var attributes = (GetAttributesOfType<CloneUnofficiallyIncorporateInterfaceAttribute>(type)).ToList();
            foreach (CloneUnofficiallyIncorporateInterfaceAttribute attribute in attributes)
            {
                foreach (ITypeSymbol t in GetTypeAndRelatedTypes(
                    Compilation.GetTypeByMetadataName(attribute.OtherInterfaceFullyQualifiedTypeName)))
                {
                    if (t == null)
                        throw new LazinatorCodeGenException($"Unofficial type {attribute.OtherInterfaceFullyQualifiedTypeName} must exist and have a Lazinator attribute.");
                    alreadyProduced.Add(t);
                    yield return t;
                }
            }
            foreach (var @interface in type.AllInterfaces)
                foreach (ITypeSymbol t in GetTypeAndRelatedTypes(@interface))
                {
                    alreadyProduced.Add(t);
                    yield return t;
                }
            if (type.BaseType != null)
            {
                foreach (ITypeSymbol t in GetTypeAndRelatedTypes(type.BaseType))
                {
                    alreadyProduced.Add(t);
                    yield return t;
                }
            }
        }

        private void RecordInterfaceTextHash(INamedTypeSymbol @interface, INamedTypeSymbol implementingType)
        {
            var hash = GetHashForInterface(@interface, implementingType, TypeImplementsMethod, ImplementingTypeRequiresParameterlessConstructor);
            InterfaceTextHash[@interface] = hash;
        }

        private static List<SyntaxNode> GetSyntaxNodesForNamedType(INamedTypeSymbol namedType)
        {
            // Sometimes Roslyn returns the same syntax reference twice, with the only difference being that slashes in the path are represented differently (e.g., / vs. \\). So we want to get distinct syntax nodes.

            string OmitSlashes(string x)
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in x)
                    if (c != '/' && c != '\\')
                        sb.Append(c);
                return sb.ToString();
            }

            var syntaxReferences = namedType.DeclaringSyntaxReferences;
            var syntaxNodes = syntaxReferences.Select(x => x.GetSyntax());
            var syntaxNodesInDistinctLocations = DistinctBy(syntaxNodes, x => x.GetLocation().GetLineSpan().Span.ToString() + OmitSlashes(x.GetLocation().GetLineSpan().Path)).ToList();
            return syntaxNodesInDistinctLocations;
        }

        private static List<TypeDeclarationSyntax> GetTypeDeclarationsForNamedType(INamedTypeSymbol namedType)
        {
            var syntaxNodes = GetSyntaxNodesForNamedType(namedType);
            List<TypeDeclarationSyntax> typeDeclarations = syntaxNodes
                .SelectMany(x => x.DescendantNodesAndSelf()
                .OfType<TypeDeclarationSyntax>())
                .ToList();
            List<TypeDeclarationSyntax> filtered =
                typeDeclarations
                .Where(x => !x.TypeDeclarationIncludesAttribute("Autogenerated"))
                .Distinct()
                .ToList();
            return filtered;
        }

        public static Guid GetHashForInterface(INamedTypeSymbol @interface, INamedTypeSymbol implementingType)
        {
            IEnumerable<TypeDeclarationSyntax> typeDeclarations = GetTypeDeclarationsForNamedType(implementingType);
            HashSet<(INamedTypeSymbol type, string methodName)> typeImplementsMethodHashSet =
                GetMethodImplementations(typeDeclarations, implementingType);
            bool typeRequiresParameterlessConstructor = RequiresParameterlessConstructor(implementingType, typeDeclarations);
            return GetHashForInterface(@interface, implementingType, typeImplementsMethodHashSet, typeRequiresParameterlessConstructor);
        }

        private static bool RequiresParameterlessConstructor(INamedTypeSymbol implementingType, IEnumerable<TypeDeclarationSyntax> typeDeclarations)
        {
            return implementingType.IsNonAbstractTypeWithConstructor() && !typeDeclarations.Any(x => x.TypeDeclarationIncludesParameterlessConstructor());
        }

        public static Guid GetHashForInterface(INamedTypeSymbol @interface, INamedTypeSymbol implementingType, HashSet<(INamedTypeSymbol type, string methodName)> typeImplementsMethodHashSet, bool implementingTypeRequiresParameterlessConstructor)
        {
            var syntaxNodes = GetSyntaxNodesForNamedType(@interface);
            if (syntaxNodes.Count() > 1)
                throw new LazinatorCodeGenException("Lazinator interface must be contained in a single file.");
            Guid hash = GetHashForInterface(syntaxNodes.Single(), implementingType, typeImplementsMethodHashSet, implementingTypeRequiresParameterlessConstructor);
            return hash;
        }

        public static Guid GetHashForInterface(SyntaxNode interfaceSyntaxNode, INamedTypeSymbol implementingType, HashSet<(INamedTypeSymbol type, string methodName)> typeImplementsMethodHashSet, bool implementingTypeRequiresParameterlessConstructor)
        {
            // The advantage of the approach here is that we can determine whether we need an update extremely quickly, thus
            // avoiding delaying compilation. The disadvantage is that it might miss an unusual scenario: where the interface has not
            // changed but the types it has referenced have changed -- for example, because a Lazinator class has become non-Lazinator.
            // The workaround for now is to regenerate the code (for example, by making a trivial change to the interface file or by deleting
            // the code behind file).
            var bitFieldRepresentingImplementedMethods = GetBitFieldRepresentingImplementedMethods(implementingType, typeImplementsMethodHashSet);
            var bytes = interfaceSyntaxNode.GetText().GetChecksum().ToList();
            bytes.Add(bitFieldRepresentingImplementedMethods);
            bytes.AddRange(LazinatorCodeGen.Roslyn.LazinatorVersionInfo.LazinatorVersionBytes); // thus, if we change the version, all code behind will need to be regenerated
            var md5 = System.Security.Cryptography.MD5.Create();
            Guid hash = new Guid(md5.ComputeHash(bytes.ToArray()));
            return hash;
        }

        private static byte GetBitFieldRepresentingImplementedMethods(INamedTypeSymbol implementingType, HashSet<(INamedTypeSymbol type, string methodName)> typeImplementsMethodHashSet)
        {
            int byteForImplementedMethods = 0;
            for (int methodIndex = 0; methodIndex < _methodNamesToLookFor.Length; methodIndex++)
            {
                if (typeImplementsMethodHashSet.Contains((implementingType, _methodNamesToLookFor[methodIndex])))
                    byteForImplementedMethods |= 1 << methodIndex;
                methodIndex++;
            }

            return (byte) byteForImplementedMethods;
        }

        private void ConsiderAddingAsRecordLikeType(INamedTypeSymbol type)
        {
            if (RecordLikeTypes.ContainsKey(type) || NonRecordLikeTypes.Contains(type))
                return;
            // Consider whether to add this as a record-like type
            if (type.IsAbstract)
                return; 
            if (!IsAllowedAsRecordLikeTypeIfProperlyFormed(type))
            {
                NonRecordLikeTypes.Add(type);
                return;
            }

            var constructorCandidates = type.Constructors.OrderByDescending(x => x.Parameters.Count()).ToList();
            if (!constructorCandidates.Any())
                NonRecordLikeTypes.Add(type);
            else
            {
                foreach (var candidate in constructorCandidates)
                {
                    var parameters = candidate.Parameters.ToList();
                    var properties = GetPropertyWithDefinitionInfo(type)
                        .Where(x => (x?.Property?.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() as PropertyDeclarationSyntax)?.AccessorList?.Accessors
                            .Any(y => y.Body == null)
                            ?? false)
                        .ToList();
                    if (parameters.Count() < properties.Count() || !parameters.Any())
                    { // better constructor candidates have already been rejected, so reject this too.
                        if (!IsAllowedAsRecordLikeTypeIfMismatched(type))
                        {
                            NonRecordLikeTypes.Add(type);
                            return;
                        }
                    }
                    if (parameters.All(x => properties.Any(y => y.Property.Name.ToUpper() == x.Name.ToUpper())))
                    {
                        List<(IParameterSymbol parameterSymbol, IPropertySymbol property)> parametersAndProperties = parameters.Select(x => (x, properties.FirstOrDefault(y => y.Property.Name.ToUpper() == x.Name.ToUpper()).Property)).ToList();
                        if (parametersAndProperties.Any(x => !x.parameterSymbol.Type.Equals(x.property.Type)))
                            continue;
                        // we have found the constructor for our record like type
                        PropertiesForType[type] = properties.ToList();
                        foreach (var property in properties)
                            RecordInformationAboutTypeAndRelatedTypes(property.Property.Type);
                        RecordLikeTypes[type] = parametersAndProperties;
                        return;
                    }
                }
            }
            NonRecordLikeTypes.Add(type);
        }

        private bool IsAllowedAsRecordLikeTypeIfProperlyFormed(INamedTypeSymbol type)
        {
            bool defaultAllowRecordLikeClasses = false, defaultAllowRecordLikeRegularStructs = false, defaultAllowRecordLikeReadOnlyStructs = true;
            if (Config != null)
            {
                string appropriatelyQualifiedName = Config.UseFullyQualifiedNames ? type.GetFullyQualifiedNameWithoutGlobal() : type.GetMinimallyQualifiedName();
                if (Config.IncludeRecordLikeTypes.Contains(appropriatelyQualifiedName))
                    return true;
                if (Config.IgnoreRecordLikeTypes.Contains(appropriatelyQualifiedName))
                    return false;
                defaultAllowRecordLikeClasses = Config.DefaultAllowRecordLikeClasses;
                defaultAllowRecordLikeRegularStructs = Config.DefaultAllowRecordLikeRegularStructs;
                defaultAllowRecordLikeReadOnlyStructs = Config.DefaultAllowRecordLikeReadOnlyStructs;
            }
            if (type.TypeKind == TypeKind.Class)
                return defaultAllowRecordLikeClasses;
            if (type.IsReadOnlyStruct())
                return defaultAllowRecordLikeReadOnlyStructs;
            return defaultAllowRecordLikeRegularStructs;
        }

        private bool IsAllowedAsRecordLikeTypeIfMismatched(INamedTypeSymbol type)
        {
            if (Config != null)
            {
                string appropriatelyQualifiedName = Config.UseFullyQualifiedNames ? type.GetFullyQualifiedNameWithoutGlobal() : type.GetMinimallyQualifiedName();
                if (Config.IncludeRecordLikeTypes.Contains(appropriatelyQualifiedName))
                    return true;
            }
            return false;
        }

        string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public IPropertySymbol GetPropertyByName(INamedTypeSymbol type, string name, bool tryCapitalizingProperty)
        {

            if (PropertiesForType.ContainsKey(type))
            {
                var properties = PropertiesForType[type];
                var match = properties.FirstOrDefault(x => x.Property.Name == name).Property;
                if (match != null)
                    return match;
                if (tryCapitalizingProperty)
                    return GetPropertyByName(type, FirstCharToUpper(name), false);
            }
            return null;
        }



        public IEnumerable<Attribute> GetAttributes(ISymbol symbol)
        {
            if (symbol == null || !KnownAttributes.ContainsKey(symbol))
                yield break;
            foreach (var attribute in KnownAttributes[symbol])
                yield return attribute;
        }

        public T GetFirstAttributeOfType<T>(ISymbol symbol) where T : Attribute
        {
            return GetAttributesOfType<T>(symbol).FirstOrDefault();
        }

        public IEnumerable<T> GetAttributesOfType<T>(ISymbol symbol) where T : Attribute
        {
            return GetAttributes(symbol).Where(x => x.GetType() == typeof(T)).Cast<T>();
        }

        public bool ContainsAttributeOfType<T>(ISymbol symbol) where T : Attribute
        {
            return GetAttributes(symbol).Any(x => x.GetType() == typeof(T));
        }

        private void AddKnownAttributesForSymbol(ISymbol symbol)
        {
            if (KnownAttributes.ContainsKey(symbol))
                return;
            AddKnownAttributesForSymbol(symbol, RoslynHelpers.GetKnownAttributes(symbol));
        }

        private void AddKnownAttributesForSymbol(ISymbol symbol, IEnumerable<Attribute> newlyKnownAttributes)
        {
            HashSet<Attribute> alreadyKnownAttributes;
            if (KnownAttributes.ContainsKey(symbol))
                alreadyKnownAttributes = KnownAttributes[symbol];
            else
                alreadyKnownAttributes = new HashSet<Attribute>();
            foreach (var newlyKnownAttribute in newlyKnownAttributes)
                alreadyKnownAttributes.Add(newlyKnownAttribute);
            KnownAttributes[symbol] = alreadyKnownAttributes;
        }

        public INamedTypeSymbol LookupSymbol(string name)
        {
            foreach (var symbol in RelevantSymbols)
            {
                if (symbol is INamedTypeSymbol namedSymbol)
                {
                    string fullyQualifiedName = RoslynHelpers.GetFullyQualifiedNameWithoutGlobal(namedSymbol);
                    string fullyQualifiedMetadataName = RoslynHelpers.GetFullyQualifiedMetadataName(namedSymbol);
                    if (namedSymbol.Name == name || fullyQualifiedName == name || fullyQualifiedName == RoslynHelpers.GetNameWithoutGenericArity(name) || namedSymbol.MetadataName == name || fullyQualifiedMetadataName == name)
                        return namedSymbol;
                }
            }

            return null;
        }

        private static HashSet<(INamedTypeSymbol type, string methodName)> GetMethodImplementations(IEnumerable<TypeDeclarationSyntax> typeDeclarations, INamedTypeSymbol typeSymbol)
        {
            HashSet<(INamedTypeSymbol type, string methodName)> typeImplementsMethod =
                new HashSet<(INamedTypeSymbol type, string methodName)>();
            foreach (var typeDeclaration in typeDeclarations)
                GetMethodImplementationsHelper(typeDeclaration, typeSymbol, typeImplementsMethod);
            return typeImplementsMethod;
        }

        private static void GetMethodImplementationsHelper(TypeDeclarationSyntax typeDeclaration,
            INamedTypeSymbol typeSymbol, HashSet<(INamedTypeSymbol type, string methodName)> typeImplementsMethod)
        {
            HashSet<MethodDeclarationSyntax> methodDeclarations = typeDeclaration.GetMethodDeclarations();
            HashSet<TypeDeclarationSyntax> subtypeDeclarations = typeDeclaration.GetSubtypeDeclarations();
            if (subtypeDeclarations.Any())
            { // remove methods defined only in subclass
                var containedMethodDeclarations = subtypeDeclarations.SelectMany(x => x.GetMethodDeclarations());
                foreach (var methodDeclaration in containedMethodDeclarations)
                    methodDeclarations.Remove(methodDeclaration);
            }
            foreach (string methodName in _methodNamesToLookFor)
                if (methodDeclarations.Any(x => x.Identifier.Text == methodName))
                    typeImplementsMethod.Add((typeSymbol, methodName));
        }
    }
}
