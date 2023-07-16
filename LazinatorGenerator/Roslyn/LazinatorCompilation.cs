using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Immutable;
using Lazinator.CodeDescription;
using LazinatorGenerator.AttributeClones;
using LazinatorGenerator.Generator;
using LazinatorGenerator.Roslyn;
using LazinatorGenerator.Settings;
using System.Collections.Concurrent;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;

namespace LazinatorCodeGen.Roslyn
{
    public class LazinatorCompilation
    {

        // We must be able to determine whether a type implements various methods, but we want to exclude methods implemented in code behind
        static readonly string[] _methodNamesToLookFor = new string[] { "LazinatorObjectVersionUpgrade", "PreSerialization", "PostDeserialization", "OnDirty", "ConvertFromBytesAfterHeader", "WritePropertiesIntoBuffer", "EnumerateLazinatorDescendants" };

        public Compilation Compilation;
        public LazinatorConfig DefaultConfig { get; private set; }
        public INamedTypeSymbol ImplementingTypeSymbol { get; private set; }
        public Accessibility ImplementingTypeAccessibility { get; private set; }
        internal List<TypeDeclarationSyntax> TypeDeclarations { get; private set; }
        internal Dictionary<string, ISymbol> RelevantSymbols = new Dictionary<string, ISymbol>();
        internal HashSet<string> ExclusiveInterfaces = new HashSet<string>();
        internal HashSet<string> NonexclusiveInterfaces = new HashSet<string>();
        internal Dictionary<string, HashSet<string>> InterfaceToClasses = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, List<PropertyWithDefinitionInfo>> PropertiesForType { get; internal set; } = new Dictionary<string, List<PropertyWithDefinitionInfo>>();
        internal Dictionary<string, string> TypeToExclusiveInterface = new Dictionary<string, string>();
        internal HashSet<(string typeName, string methodName)> TypeImplementsMethod = new HashSet<(string typeName, string methodName)>();
        internal bool ImplementingTypeRequiresParameterlessConstructor { get; set; }
        internal Dictionary<string, HashSet<Attribute>> KnownAttributes = new Dictionary<string, HashSet<Attribute>>();
        internal Dictionary<string, List<(IParameterSymbol parameterSymbol, IPropertySymbol property)>> RecordLikeTypes = new Dictionary<string, List<(IParameterSymbol parameterSymbol, IPropertySymbol property)>>();
        internal HashSet<string> RecordLikeTypesExclusions = new HashSet<string>();
        internal Dictionary<string, Guid> InterfaceTextHash = new Dictionary<string, Guid>();
        internal static ConcurrentDictionary<string, INamedTypeSymbol> NameTypedSymbolFromString = new ConcurrentDictionary<string, INamedTypeSymbol>();
        internal static ConcurrentDictionary<string, LazinatorConfig?> AdditionalConfigFiles = new ConcurrentDictionary<string, LazinatorConfig?>();
        static object LockObj = new object();

        public LazinatorCompilation(Compilation compilation, Type type, LazinatorConfig? config) : this(compilation, RoslynHelpers.GetNameWithoutGenericArity(type), type.FullName, config)
        {
        }

        public LazinatorCompilation(Compilation compilation, string implementingTypeName, string fullImplementingTypeName, LazinatorConfig? defaultConfig)
        {
            DefaultConfig = defaultConfig ?? new LazinatorConfig();
            Initialize(compilation, implementingTypeName, fullImplementingTypeName);
        }

        public LazinatorCompilation(Compilation compilation, INamedTypeSymbol implementingTypeSymbol, LazinatorConfig? defaultConfig)
        {
            DefaultConfig = defaultConfig ?? new LazinatorConfig();
            Initialize(compilation, implementingTypeSymbol);
        }

        public void Initialize(Compilation compilation, INamedTypeSymbol implementingTypeSymbol)
        {
            Compilation = compilation;
            ImplementingTypeSymbol = implementingTypeSymbol;
            ContinueInitialization();
        }
        
        public void Initialize(Compilation compilation, string implementingTypeName, string fullImplementingTypeName)
        {
            Compilation = compilation;

            ImplementingTypeSymbol = Compilation.GetTypeByMetadataName(fullImplementingTypeName);
            ContinueInitialization();
        }

        private void ContinueInitialization()
        {
            TypeDeclarations = GetTypeDeclarationsForNamedType(ImplementingTypeSymbol).ToList();
            if (TypeDeclarations == null || !TypeDeclarations.Any())
                throw new LazinatorCodeGenException($"Internal Lazinator error. Implementing type declaration for {ImplementingTypeSymbol.GetFullMetadataName()} not found.");

            ImplementingTypeAccessibility = TypeDeclarations.First().GetAccessibility();
            INamedTypeSymbol lazinatorTypeAttribute = Compilation.GetTypeByMetadataName(LazinatorPairFinder.LazinatorAttributeName);
            INamedTypeSymbol exclusiveInterfaceTypeSymbol = ImplementingTypeSymbol.GetTopLevelInterfaceImplementingAttribute(lazinatorTypeAttribute);
            if (exclusiveInterfaceTypeSymbol == null)
                throw new LazinatorCodeGenException($"Type {ImplementingTypeSymbol.Name} should implement exactly one Lazinator interface (plus any Lazinator interfaces implemented by that interface, plus any number of nonexclusive interfaces).");
            FinishInitialization(TypeDeclarations, ImplementingTypeSymbol, exclusiveInterfaceTypeSymbol);
        }

        /// <summary>
        /// Gets the Config file for the path by checking the specified path and all higher-level paths, caching if necessary. If none is found, then the default config file is used.
        /// </summary>
        /// <param name="pathNameOfCodeFile"></param>
        /// <returns></returns>
        public LazinatorConfig? GetConfigForPath(string pathNameOfCodeFile)
        {
            if (pathNameOfCodeFile == null)
                return DefaultConfig;
            DirectoryInfo directory = Directory.GetParent(pathNameOfCodeFile);
            pathNameOfCodeFile = directory?.FullName;
            while (pathNameOfCodeFile != null)
            {
                string possibleFilePath = Path.Combine(pathNameOfCodeFile, "LazinatorConfig.json");
                if (AdditionalConfigFiles.ContainsKey(possibleFilePath))
                {
                    LazinatorConfig? configFileIfAny = AdditionalConfigFiles[possibleFilePath];
                    if (configFileIfAny == null)
                        continue; // there is no config file in this directory
                }
                bool exists = File.Exists(possibleFilePath);
                if (exists)
                {
                    string fileText = File.ReadAllText(possibleFilePath, Encoding.UTF8);
                    LazinatorConfig configFile = new LazinatorConfig(possibleFilePath, fileText);
                    AdditionalConfigFiles[possibleFilePath] = configFile;
                    return configFile;
                }
                else
                {
                    directory = Directory.GetParent(pathNameOfCodeFile);
                    pathNameOfCodeFile = directory?.FullName;
                }
            }

            return DefaultConfig;
        }

        public void AnalyzeAnotherType(string implementingTypeName, string fullImplementingTypeName)
        {

        }

        private void FinishInitialization(List<TypeDeclarationSyntax> implementingTypeDeclarations, INamedTypeSymbol implementingTypeSymbol, INamedTypeSymbol exclusiveInterfaceTypeSymbol)
        {
            // Record information about the type, its interfaces, and inner types.
            RecordInformationAboutTypeAndRelatedTypes(implementingTypeSymbol);

            // Then deal with any implementing classes -- in particular, checking whether each implements methods.
            TypeImplementsMethod = GetMethodImplementations(implementingTypeDeclarations, implementingTypeSymbol);
            ImplementingTypeRequiresParameterlessConstructor = RequiresParameterlessConstructor(implementingTypeSymbol, implementingTypeDeclarations);

            // Now, record the interface text hash
            RecordInterfaceTextHash(exclusiveInterfaceTypeSymbol, implementingTypeSymbol);

            // And save the pair
            TypeToExclusiveInterface[TypeSymbolToString(implementingTypeSymbol)] = TypeSymbolToString(exclusiveInterfaceTypeSymbol);
        }

        private HashSet<string> ILazinatorProperties = null; 
        private void RecordILazinatorProperties()
        {
            INamedTypeSymbol iLazinatorSymbol = Compilation.GetTypeByMetadataName("Lazinator.Core.ILazinator");
            ILazinatorProperties = new HashSet<string>(iLazinatorSymbol.GetPropertySymbols().Select(x => x.GetFullyQualifiedName(true)));
        }
        
        public ImmutableArray<string> GetInterfaceSymbolNames()
        {
            HashSet<string> h = new HashSet<string>();
            foreach (var interfaceSymbol in RelevantSymbols.Where(s => s.Value is ITypeSymbol typeSymbol && typeSymbol.TypeKind == TypeKind.Interface))
            {
                h.Add(interfaceSymbol.Value.GetFullyQualifiedName(true));
            }
            return ImmutableArray.Create<string>(h.OrderBy(x => x).ToArray());
        }

        internal LazinatorDependencyInfo GetDependencyInfo()
        {
            return new LazinatorDependencyInfo(GetInterfaceSymbolNames());
        }
        
        private void RecordPropertiesForInterface(INamedTypeSymbol @interface)
        {
            if (ILazinatorProperties == null)
                RecordILazinatorProperties();
            List<PropertyWithDefinitionInfo> propertiesInInterfaceWithLevel = new List<PropertyWithDefinitionInfo>();
            foreach (var propertyWithLevelInfo in GetPropertyWithDefinitionInfo(@interface, true))
            {
                IPropertySymbol property1 = propertyWithLevelInfo.Property;
                string property1Name = property1.GetFullyQualifiedName(true);
                if (property1Name.Contains("Numerics"))
                {
                    var DEBUG = 0;
                }
                if (!ILazinatorProperties.Contains(property1Name))
                { // ignore a property that is actually an ILazinator property rather than a property we are looking for
                    propertiesInInterfaceWithLevel.Add(propertyWithLevelInfo);
                    if (!RelevantSymbols.ContainsKey(property1Name))
                    {
                        RelevantSymbols.Add(property1Name, property1);
                    }
                }
            }
            PropertiesForType[TypeSymbolToString(@interface)] = propertiesInInterfaceWithLevel;
            foreach (var propertyWithLevel in propertiesInInterfaceWithLevel)
            {
                IPropertySymbol property = propertyWithLevel.Property;
                AddKnownAttributesForSymbol(property);
                propertyWithLevel.SpecifyDerivationKeyword(GetDerivationKeyword(property));
                RecordInformationAboutTypeAndRelatedTypes(property.Type);
            }
        }

        public List<PropertyWithDefinitionInfo> GetPropertyWithDefinitionInfo(
            INamedTypeSymbol namedTypeSymbol,
            bool includeOnlyLowerLevelPropertiesFromInterfaces)
        {
            var results = DistinctBy(GetPropertiesWithDefinitionInfoHelper(namedTypeSymbol, includeOnlyLowerLevelPropertiesFromInterfaces).ToList(), x => x.Property.Name).ToList(); // ordinarily, we're not getting duplicate items. But sometimes we are.
            return results;
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

        public IEnumerable<PropertyWithDefinitionInfo> GetPropertiesWithDefinitionInfoHelper(INamedTypeSymbol namedTypeSymbol, bool includeOnlyLowerLevelPropertiesFromInterfaces)
        {
            // check whether there are lower level abstract or open generic types 
            Dictionary<INamedTypeSymbol, ImmutableList<IPropertySymbol>> lowerLevelInterfaces = null;
            if (namedTypeSymbol.TypeKind == TypeKind.Interface && namedTypeSymbol.HasAttributeOfType<CloneLazinatorAttribute>())
            {
                lowerLevelInterfaces = namedTypeSymbol.GetInterfacesWithAttributeOfType<CloneLazinatorAttribute>()
                    .Select(x => new KeyValuePair<INamedTypeSymbol, ImmutableList<IPropertySymbol>>(x, x.GetPropertySymbols()))
                    .ToDictionary(x => x.Key, x => x.Value);
            }
            namedTypeSymbol.GetPropertiesForType(includeOnlyLowerLevelPropertiesFromInterfaces, out ImmutableList <IPropertySymbol> propertiesThisLevel, out ImmutableList<IPropertySymbol> propertiesLowerLevels);
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
                throw new LazinatorCodeGenException($"Property {symbol.Name}'s DerivationKeywordAttribution must have choice of 'virtual', 'override', or 'new'.");
            return attribute.Choice + " ";
        }

        private void RecordInformationAboutTypeAndRelatedTypes(ITypeSymbol type)
        {
            var typesAndRelatedTypes = GetTypeAndRelatedTypes(type, alreadyProducedDuringCompilation); // NOTE: It's important that we not enumerate here. Otherwise, we may prematurely process a type and run into an error in ExclusiveInterfaceDescription.
            foreach (ITypeSymbol t in typesAndRelatedTypes)
                RecordInformationAboutType(t);
        }
        
        private void RecordInformationAboutType(ITypeSymbol type)
        {
            if (type.ContainingNamespace != null &&  type.ContainingNamespace.Name == "System" && type.ContainingNamespace.ContainingNamespace.Name == "")
                return;
            string typeName = type.GetFullyQualifiedName(true);
            if (typeName.Contains("Numerics"))
            {
                var DEBUG = 0;
            }
            if (RelevantSymbols.ContainsKey(typeName))
                return;
            RelevantSymbols.Add(typeName, type);
            if (type is INamedTypeSymbol namedTypeSymbol)
            {
                AddKnownAttributesForSymbol(type);
                if (namedTypeSymbol.TypeKind == TypeKind.Interface && GetFirstAttributeOfType<CloneLazinatorAttribute>(namedTypeSymbol) == null && GetFirstAttributeOfType<CloneNonexclusiveLazinatorAttribute>(namedTypeSymbol) == null)
                    return; // don't worry about IEnumerable etc.
                
                List<INamedTypeSymbol> allInterfaces = namedTypeSymbol.AllInterfaces.Where(x => !SymbolEqualityComparer.Default.Equals(x, type) && x.Name != "ILazinator")
                    .OrderByDescending(x => namedTypeSymbol.Interfaces.Contains(x))
                    .ThenByDescending(x => x.AllInterfaces.Count())
                    .ToList();
                foreach (var @interface in allInterfaces)
                {
                    if (@interface.GetFullNamespace().StartsWith("System.Collections"))
                        continue;
                    RecordInformationAboutTypeAndRelatedTypes(@interface);
                    AddLinkFromTypeToInt32erface(namedTypeSymbol, @interface);
                }
                if (namedTypeSymbol.TypeKind == TypeKind.Class || namedTypeSymbol.TypeKind == TypeKind.Struct)
                {
                    bool includesInterfaceWithLazinatorAttribute = false;
                    foreach (var @interface in allInterfaces)
                    {
                        LazinatorGenerator.AttributeClones.CloneLazinatorAttribute attribute = GetFirstAttributeOfType<LazinatorGenerator.AttributeClones.CloneLazinatorAttribute>(@interface);
                        if (attribute != null)
                        {
                            AddLinkFromTypeToInt32erface(namedTypeSymbol, @interface);
                            includesInterfaceWithLazinatorAttribute = true;
                            break;
                        }
                    }
                    if (!includesInterfaceWithLazinatorAttribute)
                        ConsiderAddingAsRecordLikeType(namedTypeSymbol);
                    if (namedTypeSymbol.BaseType != null)
                        RecordInformationAboutTypeAndRelatedTypes(namedTypeSymbol.BaseType);
                }
                else if (namedTypeSymbol.TypeKind == TypeKind.Interface)
                {
                    if (GetFirstAttributeOfType<LazinatorGenerator.AttributeClones.CloneLazinatorAttribute>(type) != null)
                    {
                        ExclusiveInterfaces.Add(TypeSymbolToString(namedTypeSymbol));
                        RecordPropertiesForInterface(namedTypeSymbol);
                    }
                    else if (GetFirstAttributeOfType<LazinatorGenerator.AttributeClones.CloneNonexclusiveLazinatorAttribute>(type) != null)
                        NonexclusiveInterfaces.Add(TypeSymbolToString(namedTypeSymbol));
                }
            }
        }

        private void AddLinkFromTypeToInt32erface(INamedTypeSymbol namedTypeSymbol, INamedTypeSymbol @interface)
        {
            if (namedTypeSymbol.TypeKind != TypeKind.Interface && ExclusiveInterfaces.Contains(TypeSymbolToString(@interface)) && !TypeToExclusiveInterface.ContainsKey(TypeSymbolToString(namedTypeSymbol)))
                TypeToExclusiveInterface[TypeSymbolToString(namedTypeSymbol.OriginalDefinition)] = TypeSymbolToString(@interface.OriginalDefinition);
        }

        HashSet<ITypeSymbol> alreadyProducedDuringCompilation = new HashSet<ITypeSymbol>();
        IEnumerable<ITypeSymbol> GetTypeAndRelatedTypes(ITypeSymbol type, HashSet<ITypeSymbol> alreadyProduced)
        {
            if (alreadyProduced.Contains(type))
                yield break;
            alreadyProduced.Add(type);
            yield return type;
            if (type is INamedTypeSymbol namedType)
            {
                foreach (ITypeSymbol t in namedType.TypeArguments)
                    foreach (ITypeSymbol t2 in GetTypeAndRelatedTypes(t, alreadyProduced))
                    {
                        alreadyProduced.Add(t2);
                        yield return t2;
                    }
                if (namedType.TupleUnderlyingType != null)
                    foreach (ITypeSymbol t in GetTypeAndRelatedTypes(namedType.TupleUnderlyingType, alreadyProduced))
                    {
                        alreadyProduced.Add(t);
                        yield return t;
                    }
            }
            else if (type is IArrayTypeSymbol arrayType)
            {
                foreach (ITypeSymbol t in GetTypeAndRelatedTypes(arrayType.ElementType, alreadyProduced))
                {
                    alreadyProduced.Add(t);
                    yield return t;
                }
            }
            if (!SymbolEqualityComparer.Default.Equals(type, type.OriginalDefinition))
                foreach (ITypeSymbol t in GetTypeAndRelatedTypes(type.OriginalDefinition, alreadyProduced))
                {
                    alreadyProduced.Add(t);
                    yield return t;
                }
            var attributes = (GetAttributesOfType<CloneUnofficiallyIncorporateInterfaceAttribute>(type)).ToList();
            foreach (CloneUnofficiallyIncorporateInterfaceAttribute attribute in attributes)
            {
                foreach (ITypeSymbol t in GetTypeAndRelatedTypes(
                    Compilation.GetTypeByMetadataName(attribute.OtherInterfaceFullyQualifiedTypeName), alreadyProducedDuringCompilation))
                {
                    if (t == null)
                        throw new LazinatorCodeGenException($"Unofficial type {attribute.OtherInterfaceFullyQualifiedTypeName} must exist and have a Lazinator attribute.");
                    alreadyProduced.Add(t);
                    yield return t;
                }
            }
            foreach (var @interface in type.AllInterfaces)
                foreach (ITypeSymbol t in GetTypeAndRelatedTypes(@interface, alreadyProduced))
                {
                    alreadyProduced.Add(t);
                    yield return t;
                }
            if (type.BaseType != null)
            {
                foreach (ITypeSymbol t in GetTypeAndRelatedTypes(type.BaseType, alreadyProduced))
                {
                    alreadyProduced.Add(t);
                    yield return t;
                }
            }
        }

        private void RecordInterfaceTextHash(INamedTypeSymbol @interface, INamedTypeSymbol implementingType)
        {
            var hash = GetHashForInterface(@interface, implementingType, TypeImplementsMethod, ImplementingTypeRequiresParameterlessConstructor);
            InterfaceTextHash[TypeSymbolToString(@interface)] = hash;
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
            HashSet<(string typeName, string methodName)> typeImplementsMethodHashSet =
                GetMethodImplementations(typeDeclarations, implementingType);
            bool typeRequiresParameterlessConstructor = RequiresParameterlessConstructor(implementingType, typeDeclarations);
            return GetHashForInterface(@interface, implementingType, typeImplementsMethodHashSet, typeRequiresParameterlessConstructor);
        }

        private static bool RequiresParameterlessConstructor(INamedTypeSymbol implementingType, IEnumerable<TypeDeclarationSyntax> typeDeclarations)
        {
            return implementingType.IsNonAbstractTypeWithConstructor() && !typeDeclarations.Any(x => x.TypeDeclarationIncludesParameterlessConstructor()); // TODO: If typeDeclarations includes a non-Lazinator subclass, this can return false when the correct answer is true. Current workaround is to include parameterless constructor in hand-coded portion of partial class.
        }

        public static Guid GetHashForInterface(INamedTypeSymbol @interface, INamedTypeSymbol implementingType, HashSet<(string typeName, string methodName)> typeImplementsMethodHashSet, bool implementingTypeRequiresParameterlessConstructor)
        {
            var syntaxNodes = GetSyntaxNodesForNamedType(@interface);
            if (syntaxNodes.Count() > 1)
                throw new LazinatorCodeGenException("Lazinator interface must be contained in a single file.");
            Guid hash = GetHashForInterface(syntaxNodes.Single(), implementingType, typeImplementsMethodHashSet, implementingTypeRequiresParameterlessConstructor);
            return hash;
        }

        public static Guid GetHashForInterface(SyntaxNode interfaceSyntaxNode, INamedTypeSymbol implementingType, HashSet<(string typeName, string methodName)> typeImplementsMethodHashSet, bool implementingTypeRequiresParameterlessConstructor)
        {
            // The advantage of the approach here is that we can determine whether we need an update extremely quickly, thus
            // avoiding delaying compilation. The disadvantage is that it might miss an unusual scenario: where the interface has not
            // changed but the types it has referenced have changed -- for example, because a Lazinator class has become non-Lazinator.
            // The workaround for now is to regenerate the code (for example, by making a trivial change to the interface file or by deleting
            // the code behind file).
            var bitFieldRepresentingImplementedMethods = GetBitFieldRepresentingImplementedMethods(implementingType, typeImplementsMethodHashSet);
            var bytes = System.Text.Encoding.UTF8.GetBytes(interfaceSyntaxNode.GetText().ToString().Where(x => x != '\r' && x != '\n').ToArray()).ToList(); // ignore the line endings b/c this differs across computers (even on same platform)
            bytes.Add(bitFieldRepresentingImplementedMethods);
            bytes.AddRange(LazinatorCodeGen.Roslyn.LazinatorVersionInfo.LazinatorVersionBytes); // thus, if we change the version, all code behind will need to be regenerated
            var md5 = System.Security.Cryptography.MD5.Create();
            Guid hash = new Guid(md5.ComputeHash(bytes.ToArray()));
            return hash;
        }

        private static byte GetBitFieldRepresentingImplementedMethods(INamedTypeSymbol implementingType, HashSet<(string typeName, string methodName)> typeImplementsMethodHashSet)
        {
            int byteForImplementedMethods = 0;
            for (int methodIndex = 0; methodIndex < _methodNamesToLookFor.Length; methodIndex++)
            {
                if (typeImplementsMethodHashSet.Contains((TypeSymbolToString(implementingType), _methodNamesToLookFor[methodIndex])))
                    byteForImplementedMethods |= 1 << methodIndex;
                methodIndex++;
            }

            return (byte) byteForImplementedMethods;
        }

        private void ConsiderAddingAsRecordLikeType(INamedTypeSymbol type)
        {
            if (type.ToString().Contains("Point"))
            {
                var DEBUG2 = 0;
            }
            string typeName = TypeSymbolToString(type);
            if (RecordLikeTypes.ContainsKey(typeName) || RecordLikeTypesExclusions.Contains(typeName))
                return;
            // Consider whether to add this as a record-like type
            if (type.IsAbstract)
                return; 
            if (!IsAllowedAsRecordLikeTypeIfProperlyFormed(type))
            {
                RecordLikeTypesExclusions.Add(typeName);
                return;
            }

            bool isActualRecord = type.IsRecord();

            var constructorCandidates = type.Constructors.OrderByDescending(x => x.Parameters.Count()).ToList();
            if (!constructorCandidates.Any() && !isActualRecord)
                RecordLikeTypesExclusions.Add(typeName);
            else
            {
                List<PropertyWithDefinitionInfo> GetProperties(bool forConstructor)
                {
                    static bool IsAutoProperty(IPropertySymbol propertySymbol)
                    {
                        // Get fields declared in the same type as the property
                        var fields = propertySymbol.ContainingType.GetMembers().OfType<IFieldSymbol>();
                        if (!fields.Any())
                            return false;

                        // Check if one field is associated to
                        string propertyNameUppercase = propertySymbol.Name.ToUpper();
                        return fields.Any(field => SymbolEqualityComparer.Default.Equals(field.AssociatedSymbol, propertySymbol) || field.Name.ToUpper() == propertyNameUppercase);
                    }
                    bool IsQualifyingProperty(IPropertySymbol propertySymbol)
                    {
                        if (propertySymbol.IsImplicitlyDeclared || !IsAutoProperty(propertySymbol))
                            return false;
                        bool includesGet = propertySymbol.GetMethod != null;
                        if (forConstructor)
                            return includesGet;
                        bool includesSetOrInit = propertySymbol.SetMethod != null;
                        return includesGet || includesSetOrInit;
                    }
                    List<PropertyWithDefinitionInfo> qualifyingProperties = GetPropertyWithDefinitionInfo(type, false)
                        .Where(x => IsQualifyingProperty(x.Property))
                        .ToList();
                    return qualifyingProperties;
                }
                List<PropertyWithDefinitionInfo> propertiesToMatchWithConstructor = GetProperties(true);
                foreach (var candidate in constructorCandidates)
                {
                    if (candidate.ToString().Contains("Point"))
                    {
                        var DEBUG = 0;
                    }
                    var parameters = candidate.Parameters.ToList();
                    if (parameters.Count() > propertiesToMatchWithConstructor.Count())
                        continue;
                    if (!isActualRecord && (parameters.Count() < propertiesToMatchWithConstructor.Count() || !parameters.Any()))
                    { // there aren't enough parameters to set all properties (or this is parameterless, even if there aren't any properties)
                        if (!IsSpecificallyIncludedRecordLikeType(type))
                        {
                            RecordLikeTypesExclusions.Add(typeName);
                            return;
                        }
                    }
                    if (parameters.Any() && parameters.All(x => propertiesToMatchWithConstructor.Any(y => y.Property.Name.ToUpper() == x.Name.ToUpper())))
                    {
                        List<(IParameterSymbol parameterSymbol, IPropertySymbol property)> parametersAndProperties = parameters.Select(x => (x, propertiesToMatchWithConstructor.FirstOrDefault(y => y.Property.Name.ToUpper() == x.Name.ToUpper()).Property)).ToList();
                        if (parametersAndProperties.Any(x => !SymbolEqualityComparer.Default.Equals(x.parameterSymbol.Type, x.property.Type)))
                            continue;
                        // we have found the constructor for our record like type
                        PropertiesForType[typeName] = propertiesToMatchWithConstructor.ToList();
                        foreach (var property in propertiesToMatchWithConstructor)
                            RecordInformationAboutTypeAndRelatedTypes(property.Property.Type);
                        RecordLikeTypes[typeName] = parametersAndProperties;
                        return;
                    }
                }
                if (isActualRecord || IsSpecificallyIncludedRecordLikeType(type))
                {
                    var propertiesToSetDirectly = GetProperties(false);
                    // store the properties only, omitting the parameters. This will be our signal that we need to use the init-only context.
                    PropertiesForType[typeName] = propertiesToMatchWithConstructor.ToList();
                    foreach (var property in propertiesToMatchWithConstructor)
                        RecordInformationAboutTypeAndRelatedTypes(property.Property.Type);
                    List<(IParameterSymbol parameterSymbol, IPropertySymbol property)> parametersAndProperties = propertiesToMatchWithConstructor.Select(x => ((IParameterSymbol)null, x.Property)).ToList();
                    RecordLikeTypes[typeName] = parametersAndProperties;
                    return;
                }
            }
            RecordLikeTypesExclusions.Add(typeName);
        }

        private bool IsAllowedAsRecordLikeTypeIfProperlyFormed(INamedTypeSymbol type)
        {
            bool defaultAllowRecordLikeClasses = false, defaultAllowRecordLikeRegularStructs = false, defaultAllowRecordLikeReadOnlyStructs = true;
            string appropriatelyQualifiedName = DefaultConfig.UseFullyQualifiedNames ? type.GetFullyQualifiedNameWithoutGlobal(false) : type.GetMinimallyQualifiedName(false);
            if (DefaultConfig.IncludeRecordLikeTypes != null && DefaultConfig.IncludeRecordLikeTypes.Contains(appropriatelyQualifiedName))
                return true;
            if (DefaultConfig.IgnoreRecordLikeTypes != null && DefaultConfig.IgnoreRecordLikeTypes.Contains(appropriatelyQualifiedName))
                return false;
            if (DefaultConfig.GetInterchangeConverterTypeName(type) != null || DefaultConfig.GetDirectConverterTypeName(type) != null)
                return false;
            defaultAllowRecordLikeClasses = DefaultConfig.DefaultAllowRecordLikeClasses;
            defaultAllowRecordLikeRegularStructs = DefaultConfig.DefaultAllowRecordLikeRegularStructs;
            defaultAllowRecordLikeReadOnlyStructs = DefaultConfig.DefaultAllowRecordLikeReadOnlyStructs;
            
            if (type.TypeKind == TypeKind.Class)
                return defaultAllowRecordLikeClasses;
            if (type.IsReadOnlyStruct())
                return defaultAllowRecordLikeReadOnlyStructs;
            return defaultAllowRecordLikeRegularStructs;
        }

        private bool IsSpecificallyIncludedRecordLikeType(INamedTypeSymbol type)
        {
            string appropriatelyQualifiedName = DefaultConfig.UseFullyQualifiedNames ? type.GetFullyQualifiedNameWithoutGlobal(false) : type.GetMinimallyQualifiedName(false);
            string appropriatelyQualifiedNameWithNullableIndicators = DefaultConfig.UseFullyQualifiedNames ? type.GetFullyQualifiedNameWithoutGlobal(true) : type.GetMinimallyQualifiedName(true);
            if (DefaultConfig.IncludeRecordLikeTypes.Contains(appropriatelyQualifiedName) || DefaultConfig.IncludeRecordLikeTypes.Contains(appropriatelyQualifiedNameWithNullableIndicators))
                return true;
            
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

            string typeSymbol = TypeSymbolToString(type);
            if (PropertiesForType.ContainsKey(typeSymbol))
            {
                var properties = PropertiesForType[typeSymbol];
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
            string symbolName = symbol?.GetFullyQualifiedName(true);
            if (symbol == null || !KnownAttributes.ContainsKey(symbolName))
                yield break;
            foreach (var attribute in KnownAttributes[symbolName])
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
            string symbolName = symbol.GetFullyQualifiedName(true);
            if (KnownAttributes.ContainsKey(symbolName))
                return;
            AddKnownAttributesForSymbol(symbol, RoslynHelpers.GetKnownAttributes(symbol));
        }

        private void AddKnownAttributesForSymbol(ISymbol symbol, IEnumerable<Attribute> newlyKnownAttributes)
        {
            string symbolName = symbol.GetFullyQualifiedName(true);
            HashSet<Attribute> alreadyKnownAttributes;
            if (KnownAttributes.ContainsKey(symbolName))
                alreadyKnownAttributes = KnownAttributes[symbolName];
            else
                alreadyKnownAttributes = new HashSet<Attribute>();
            foreach (var newlyKnownAttribute in newlyKnownAttributes)
                alreadyKnownAttributes.Add(newlyKnownAttribute);
            KnownAttributes[symbolName] = alreadyKnownAttributes;
        }

        public INamedTypeSymbol LookupSymbol(string name)
        {
            foreach (KeyValuePair<string, ISymbol> keyValuePair in RelevantSymbols)
            {
                if (keyValuePair.Value is INamedTypeSymbol namedSymbol)
                {
                    string fullyQualifiedName = RoslynHelpers.GetFullyQualifiedNameWithoutGlobal(namedSymbol, true);
                    string fullyQualifiedMetadataName = RoslynHelpers.GetFullyQualifiedMetadataName(namedSymbol);
                    if (namedSymbol.Name == name || fullyQualifiedName == name || fullyQualifiedName == RoslynHelpers.GetNameWithoutGenericArity(name) || namedSymbol.MetadataName == name || fullyQualifiedMetadataName == name)
                        return namedSymbol;
                }
            }

            return null;
        }

        private static HashSet<(string typeName, string methodName)> GetMethodImplementations(IEnumerable<TypeDeclarationSyntax> typeDeclarations, INamedTypeSymbol typeSymbol)
        {
            HashSet<(string typeName, string methodName)> typeImplementsMethod =
                new HashSet<(string typeName, string methodName)>();
            foreach (var typeDeclaration in typeDeclarations)
                GetMethodImplementationsHelper(typeDeclaration, typeSymbol, typeImplementsMethod);
            return typeImplementsMethod;
        }

        private static void GetMethodImplementationsHelper(TypeDeclarationSyntax typeDeclaration,
            INamedTypeSymbol typeSymbol, HashSet<(string typeName, string methodName)> typeImplementsMethod)
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
                    typeImplementsMethod.Add((typeSymbol.GetFullyQualifiedName(true), methodName));
        }

        public static string TypeSymbolToString(INamedTypeSymbol typeSymbol)
        {
            string name = typeSymbol.GetFullyQualifiedName(true);
            if (!NameTypedSymbolFromString.ContainsKey(name))
                NameTypedSymbolFromString[name] = typeSymbol;
            return name;
        }

        public INamedTypeSymbol GetExclusiveInterface(INamedTypeSymbol iLazinatorTypeSymbol)
        {
            string typeSymbolString = LazinatorCompilation.TypeSymbolToString(iLazinatorTypeSymbol.OriginalDefinition);
            string exclusiveInterfaceString = TypeToExclusiveInterface[typeSymbolString];
            INamedTypeSymbol interfaceTypeSymbol = LazinatorCompilation.NameTypedSymbolFromString
                [exclusiveInterfaceString];
            return interfaceTypeSymbol;
        }
    }
}
