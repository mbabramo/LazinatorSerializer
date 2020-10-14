using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Lazinator.Attributes;
using LazinatorAnalyzer.AttributeClones;
using LazinatorAnalyzer.Settings;
using LazinatorAnalyzer.Support;
using LazinatorCodeGen.Roslyn;
using Microsoft.CodeAnalysis;

namespace Lazinator.CodeDescription
{
    /// <summary>
    /// A description of a Lazinator object, including information about the properties that it must contain based on its exclusive interface and any nonexclusive interfaces.
    /// </summary>
    public class ObjectDescription
    {
        /* Main properties */
        public INamedTypeSymbol ILazinatorTypeSymbol { get; set; }
        public INamedTypeSymbol InterfaceTypeSymbol { get; set; }
        public LazinatorObjectType ObjectType { get; set; }
        public LazinatorCompilation Compilation { get; set; }
        public LazinatorConfig Config { get; set; }
        public Guid Hash { get; set; }
        public string CodeToInsert { get; set; }

        /* Nullable context */
        public NullableContext NullableContextSetting { get; set; }
        public bool NullableModeEnabled => NullableContextSetting.AnnotationsEnabled();
        public bool NullableModeInherited => NullableContextSetting.AnnotationsEnabled();
        public bool AlwaysSpecifyNullableMode => true;
        public string NullableModeSettingString => NullableModeInherited && !AlwaysSpecifyNullableMode ? "" : (NullableModeEnabled ? $@"
            #nullable enable" : $@"
            #nullable disable");
        public string QuestionMarkIfNullableModeEnabled => NullableModeEnabled ? "?" : "";
        public string ILazinatorStringWithoutQuestionMark => "ILazinator";
        public string ILazinatorString => "ILazinator" + QuestionMarkIfNullableModeEnabled;
        public string ILazinatorStringNonNullableIfPropertyNonNullable(bool nonnullable) => nonnullable ? "ILazinator" : ILazinatorString;

        /* Derivation */
        public ObjectDescription BaseLazinatorObject { get; set; }
        public bool IsDerived => BaseLazinatorObject != null;
        public bool IsDerivedFromNonAbstractLazinator => BaseLazinatorObject != null &&
                        (BaseLazinatorObject.IsDerivedFromNonAbstractLazinator ||
                        !BaseLazinatorObject.IsAbstract) && !GeneratingRefStruct;
        public bool IsDerivedFromAbstractLazinator => BaseLazinatorObject != null &&
                        (BaseLazinatorObject.IsDerivedFromAbstractLazinator || BaseLazinatorObject.IsAbstract) && !GeneratingRefStruct;
        public string DerivationKeyword => (IsDerivedFromNonAbstractLazinator || IsDerivedFromAbstractLazinator) ? "override " : (IsSealedOrStruct ? "" : "virtual ");
        public string BaseObjectName => BaseLazinatorObject.Namespace == Namespace ? BaseLazinatorObject.NameIncludingGenerics : BaseLazinatorObject.FullyQualifiedObjectName;
        public string ProtectedIfApplicable => (ObjectType == LazinatorObjectType.Struct || IsSealed || GeneratingRefStruct) ? "" : "protected ";

        /* Names */
        public string FilePath { get; set; }
        public string Namespace { get; set; }
        public string NameIncludingGenerics { get; set; }
        public string NameIncludingGenerics_RefStruct => IsGeneric ? NameIncludingGenerics.Replace("<", "_RefStruct<") : NameIncludingGenerics + "_RefStruct";
        public string SimpleName { get; set; }
        public string FullyQualifiedObjectName { get; set; }
        public string ObjectNameEncodable => RoslynHelpers.EncodableTypeName(ILazinatorTypeSymbol);

        /* General aspects */
        public Accessibility Accessibility { get; set; }
        public bool IsNonLazinatorBaseClass { get; set; }
        public bool IsRecord { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }
        public string SealedKeyword => IsSealed ? "sealed " : "";
        public string ClassKeyword => IsRecord ? "record" : "class";
        public bool IsClass => ObjectType == LazinatorObjectType.Class && !GeneratingRefStruct;
        public bool IsStruct => !IsClass;
        public bool IsSealedOrStruct => IsSealed || ObjectType != LazinatorObjectType.Class || GeneratingRefStruct;

        /* Interfaces */
        public ExclusiveInterfaceDescription ExclusiveInterface { get; set; }
        public int Version => ExclusiveInterface.Version;
        public int UniqueID => (int)ExclusiveInterface.UniqueID;
        public List<NonexclusiveInterfaceDescription> NonexclusiveInterfaces { get; set; }
        public bool HasNonexclusiveInterfaces => NonexclusiveInterfaces != null && NonexclusiveInterfaces.Any();
        public int TotalNumProperties => ExclusiveInterface.TotalNumProperties;
        internal bool AutoChangeParentAllThisLevel => ExclusiveInterface?.AutoChangeParentAll ?? false;
        public bool AutoChangeParentAll => AutoChangeParentAllThisLevel || GetBaseObjectDescriptions().Any(x => x.AutoChangeParentAllThisLevel);

        /* Implementations */
        public string[] ImplementedMethods { get; set; }
        public bool ImplementsLazinatorObjectVersionUpgrade => ImplementedMethods.Contains("LazinatorObjectVersionUpgrade");
        public bool ImplementsPreSerialization => ImplementedMethods.Contains("PreSerialization");
        public bool ImplementsPostDeserialization => ImplementedMethods.Contains("PostDeserialization");
        public bool ImplementsOnDirty => ImplementedMethods.Contains("OnDirty");
        public bool ImplementsOnDescendantIsDirty => ImplementedMethods.Contains("OnDescendantIsDirty");
        public bool ImplementsOnFreeInMemoryObjects => ImplementedMethods.Contains("OnFreeInMemoryObjects");
        public bool ImplementsOnClone => ImplementedMethods.Contains("OnCompleteClone");
        public bool ImplementsOnForEachLazinator => ImplementedMethods.Contains("OnForEachLazinator");
        public bool ImplementsConvertFromBytesAfterHeader => ImplementedMethods.Contains("ConvertFromBytesAfterHeader");
        public bool ImplementsOnUpdateDeserializedChildren => ImplementedMethods.Contains("OnUpdateDeserializedChildren");
        public bool ImplementsWritePropertiesIntoBuffer => ImplementedMethods.Contains("WritePropertiesIntoBuffer");
        public bool ImplementsOnPropertiesWritten => ImplementedMethods.Contains("OnPropertiesWritten");
        public bool ImplementsEnumerateLazinatorDescendants => ImplementedMethods.Contains("EnumerateLazinatorDescendants");
        public bool ImplementsAssignCloneProperties => ImplementedMethods.Contains("AssignCloneProperties");

        /* Complications */
        public List<string> GenericArgumentNames { get; set; }
        public string GenericArgumentNameTypes => String.Join(", ", GenericArgumentNames.Select(x => "typeof(" + x + ")"));
        public bool IsGeneric => GenericArgumentNames != null && GenericArgumentNames.Any();
        public bool AllGenericsAreNonlazinator { get; set; }
        public bool ContainsOpenGenericParameters => IsGeneric && !AllGenericsAreNonlazinator;
        public List<PropertyDescription> PropertiesToDefineThisLevel => ExclusiveInterface?.PropertiesToDefineThisLevel;
        public List<PropertyDescription> PropertiesIncludingInherited => ExclusiveInterface?.PropertiesIncludingInherited;
        public List<PropertyDescription> PropertiesInherited => ExclusiveInterface?.PropertiesInherited;
        public bool CanNeverHaveChildren => Version == -1 && IsSealedOrStruct && !ExclusiveInterface.PropertiesIncludingInherited.Any(x => x.PropertyType != LazinatorPropertyType.PrimitiveType && x.PropertyType != LazinatorPropertyType.PrimitiveTypeNullable) && !IsGeneric;
        public bool UniqueIDCanBeSkipped => Version == -1 && IsSealedOrStruct && BaseLazinatorObject == null && !HasNonexclusiveInterfaces && !ContainsOpenGenericParameters;
        public bool SuppressDate { get; set; }
        public bool AsyncLazinatorMemory => InterfaceTypeSymbol.HasAttributeOfType<CloneAsyncLazinatorMemoryAttribute>();
        public bool AllowNonlazinatorGenerics => InterfaceTypeSymbol.HasAttributeOfType<CloneAllowNonlazinatorOpenGenericsAttribute>();
        public bool SuppressLazinatorVersionByte => InterfaceTypeSymbol.HasAttributeOfType<CloneExcludeLazinatorVersionByteAttribute>();
        public bool GenerateRefStruct => InterfaceTypeSymbol.HasAttributeOfType<CloneGenerateRefStructAttribute>() && !GeneratingRefStruct;
        public bool GeneratingRefStruct = false;
        public bool IncludeTracingCode => true; // DEBUG Config?.IncludeTracingCode ?? false;
        public bool StepThroughProperties => Config?.StepThroughProperties ?? true;
        public bool NonbinaryHash => InterfaceTypeSymbol.HasAttributeOfType<CloneNonbinaryHashAttribute>();
        private string IIF(bool x, string y) => x ? y : ""; // Include if function
        private string IIF(bool x, Func<string> y) => x ? y() : ""; // Same but with a function to produce the string

        /* Strings to hide fields or properties if applicable */
        internal string HideMainProperty => (Config?.HideMainProperties ?? false) ? $@"[DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    " : "";
        internal string HideBackingField => (Config?.HideBackingFields ?? true) ? $@"[DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    " : "";
        internal string HideILazinatorProperty => (Config?.HideILazinatorProperties ?? true) ? $@"[DebuggerBrowsable(DebuggerBrowsableState.Never)]
                    " : "";

        public ObjectDescription()
        {

        }

        public ObjectDescription(INamedTypeSymbol iLazinatorTypeSymbol, LazinatorCompilation compilation, string filePath, bool suppressDate = false)
        {
            //Debug.WriteLine($"Creating object description for {iLazinatorTypeSymbol}");
            ILazinatorTypeSymbol = iLazinatorTypeSymbol;
            var implementedAttributes = iLazinatorTypeSymbol.GetAttributesIncludingBase<CloneImplementsAttribute>();
            ImplementedMethods = implementedAttributes.SelectMany(x => x.Implemented).ToArray();
            CodeToInsert = iLazinatorTypeSymbol.GetKnownAttribute<CloneInsertCodeAttribute>()?.CodeToInsert;
            FilePath = filePath;
            Compilation = compilation;
            Config = compilation.GetConfigForPath(FilePath);
            SuppressDate = suppressDate;
            Accessibility = compilation.ImplementingTypeAccessibility;
            Namespace = iLazinatorTypeSymbol.GetFullNamespace();
            FullyQualifiedObjectName = iLazinatorTypeSymbol.GetFullyQualifiedNameWithoutGlobal(NullableModeEnabled);
            NameIncludingGenerics = iLazinatorTypeSymbol.Name; // possibly updated later
            SimpleName = iLazinatorTypeSymbol.Name;
            if (iLazinatorTypeSymbol.TypeKind == TypeKind.Class)
            {
                ObjectType = LazinatorObjectType.Class;
                IsRecord = iLazinatorTypeSymbol.IsRecord();
                IsAbstract = iLazinatorTypeSymbol.IsAbstract;
                IsSealed = iLazinatorTypeSymbol.IsSealed;
            }
            else
            { // struct or ref struct
                IsSealed = false;
                // commented out -- we're not currently supporting generating ref structs
                //if (iLazinatorType.IsByRefLike)
                //    ObjectType = LazinatorObjectType.RefStruct;
                //else
                ObjectType = LazinatorObjectType.Struct;
            }

            if (iLazinatorTypeSymbol.TypeKind != TypeKind.Struct)
            {
                var baseILazinatorType = iLazinatorTypeSymbol.BaseType;

                if (baseILazinatorType != null && baseILazinatorType.Name != "Object")
                {
                    BaseLazinatorObject = new ObjectDescription(baseILazinatorType, compilation, baseILazinatorType.Locations.Select(x => x?.SourceTree?.FilePath).FirstOrDefault());
                    if (BaseLazinatorObject.IsNonLazinatorBaseClass)
                        BaseLazinatorObject = null; // ignore base non-lazinator
                }
            }

            if (!Compilation.TypeToExclusiveInterface.ContainsKey(LazinatorCompilation.TypeSymbolToString(iLazinatorTypeSymbol.OriginalDefinition)))
            {
                // This is a nonlazinator base class
                IsNonLazinatorBaseClass = true;
                return;
            }
            INamedTypeSymbol interfaceTypeSymbol = LazinatorCompilation.NameTypedSymbolFromString
                [Compilation.TypeToExclusiveInterface[LazinatorCompilation.TypeSymbolToString(iLazinatorTypeSymbol.OriginalDefinition)]];
            InterfaceTypeSymbol = interfaceTypeSymbol;
            Hash = Compilation.InterfaceTextHash.ContainsKey(LazinatorCompilation.TypeSymbolToString(interfaceTypeSymbol)) ? Compilation.InterfaceTextHash[LazinatorCompilation.TypeSymbolToString(interfaceTypeSymbol)] : default;
            NullableContextSetting = interfaceTypeSymbol.GetNullableContextForSymbol(Compilation.Compilation);
            ExclusiveInterface = new ExclusiveInterfaceDescription(compilation.Compilation, interfaceTypeSymbol, NullableContextSetting, this);
            if (ExclusiveInterface.GenericArgumentNames.Any())
                HandleGenerics(iLazinatorTypeSymbol);
            var nonexclusiveInterfaces = iLazinatorTypeSymbol.AllInterfaces
                                .Where(x => Compilation.ContainsAttributeOfType<CloneNonexclusiveLazinatorAttribute>(x));
            NonexclusiveInterfaces = nonexclusiveInterfaces
                .Select(x => new NonexclusiveInterfaceDescription(Compilation, x, NullableContextSetting, this)).ToList();
        }

        public IEnumerable<ObjectDescription> GetBaseObjectDescriptions()
        {
            var b = BaseLazinatorObject;
            while (b != null)
            {
                yield return b;
                b = b.BaseLazinatorObject;
            }
        }

        /// <summary>
        /// Returns base objects that are abstract if there properties are not implemented by base objects that are concrete.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ObjectDescription> GetAbstractBaseObjectDescriptions()
        {
            foreach (ObjectDescription o in GetBaseObjectDescriptions())
            {
                if (!o.IsAbstract)
                    yield break;
                yield return o;
            }
        }

        public IEnumerable<ObjectDescription> GetAbstractBaseObjectDescriptions(INamedTypeSymbol belowThisLevel)
        {
            bool found = false;
            foreach (ObjectDescription o in GetAbstractBaseObjectDescriptions())
            {
                if (found)
                    yield return o;
                else
                    found = SymbolEqualityComparer.Default.Equals(o.ILazinatorTypeSymbol, belowThisLevel);
            }
        }

        public string GetCodeBehind()
        {
            CodeStringBuilder sb = new CodeStringBuilder();
            AppendCodeBehindFile(sb);
            string result = sb.ToString();
            return result;
        }

        private void AppendCodeBehindFile(CodeStringBuilder sb)
        {
            string partialsuperclasses;
            IEnumerable<ITypeSymbol> supertypes;
            AppendSupertypesInformation(out partialsuperclasses, out supertypes);
            AppendDeclaration(sb, partialsuperclasses);
            AppendPropertyDefinitions(sb);
            AppendGeneralDefinitions(sb);
            AppendMiscMethods(sb);
            if (GenerateRefStruct)
            {
                AppendToRefStruct(sb);
                GeneratingRefStruct = true;
                AppendCodeBehindFile(sb);
                GeneratingRefStruct = false;
            }
            AppendCloseClassSupertypesAndNamespace(sb, supertypes);
        }

        private void AppendMiscMethods(CodeStringBuilder sb)
        {
            if (IsAbstract && !GeneratingRefStruct)
            {
                if (!IsDerivedFromAbstractLazinator && !IsDerivedFromNonAbstractLazinator)
                    AppendAbstractConversions(sb);
                // otherwise, lower level has already defined these methods, so nothing more to do
            }
            else
            {
                AppendEnumerateLazinatorDescendants(sb);
                AppendEnumerateNonLazinatorProperties(sb);
                AppendForEachLazinator(sb);
                AppendResetProperties(sb);
                AppendConversionSectionStart(sb);
                AppendConvertFromBytesAfterHeader(sb);
                AppendSerializeToExistingBuffer(sb);
                AppendWritePropertiesIntoBuffer(sb);
                AppendSupportedConversions(sb);
            }
        }

        private void AppendDeclaration(CodeStringBuilder sb, string partialsuperclasses)
        {
            List<string> namespaces = PropertiesToDefineThisLevel.SelectMany(x => x.PropertyAndInnerProperties().Select(y => y.Namespace)).ToList();
            namespaces.AddRange(ILazinatorTypeSymbol.GetNamespacesOfTypesAndContainedTypes());
            namespaces.AddRange(ILazinatorTypeSymbol.GetNamespacesOfContainingTypes());

            string theBeginning =
                            $@"{GetFileHeader(Hash.ToString(), Namespace, namespaces)}

                    {partialsuperclasses}[Autogenerated]
                    {IIF(GeneratingRefStruct, $@"public ref partial struct { NameIncludingGenerics_RefStruct }")}{IIF(!GeneratingRefStruct, $@"{AccessibilityConverter.Convert(Accessibility)} { SealedKeyword }partial { (ObjectType == LazinatorObjectType.Class ? ClassKeyword : "struct") } { NameIncludingGenerics } : {IIF(IsDerivedFromNonAbstractLazinator, () => BaseObjectName + ", ")}{ILazinatorStringWithoutQuestionMark}")}
                    {{";
            sb.AppendLine(theBeginning);
            if (CodeToInsert != null && CodeToInsert != "")
            {
                sb.AppendLine("");
                sb.AppendLine(CodeToInsert);
            }

            if (BaseLazinatorObject == null)
                sb.AppendLine($@"{HideILazinatorProperty}public bool IsStruct => {(ObjectType == LazinatorObjectType.Struct || GeneratingRefStruct ? "true" : "false")};
                        ");

            AppendFromRefStruct(sb);
        }

        private void AppendSupertypesInformation(out string partialsuperclasses, out IEnumerable<ITypeSymbol> supertypes)
        {
            // we may have nested lazinator classes, in which class we need to nest the partial class definitions.
            partialsuperclasses = "";
            supertypes = null;
            if (ILazinatorTypeSymbol.ContainingType != null && !GeneratingRefStruct)
            {
                supertypes = ILazinatorTypeSymbol.GetContainingTypes();
                foreach (var supertype in supertypes)
                {
                    partialsuperclasses = partialsuperclasses + $@"{AccessibilityConverter.Convert(Accessibility)} {IIF(supertype.IsSealed, "sealed ")}partial {ClassKeyword} {supertype.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}
                        {{
                        ";
                }
            }
        }

        private void AppendToRefStruct(CodeStringBuilder sb)
        {
            if (!GenerateRefStruct)
                return;
            sb.AppendLine($@"
                public {NameIncludingGenerics_RefStruct} ToRefStruct()
                {{
                    UpdateStoredBuffer();
                    var clone = new {NameIncludingGenerics_RefStruct}(LazinatorMemoryStorage, null, OriginalIncludeChildrenMode, null);
                    return clone;
                }}");
        }

        private void AppendFromRefStruct(CodeStringBuilder sb)
        {
            if (!GeneratingRefStruct)
                return;
            sb.AppendLine($@"public {NameIncludingGenerics} FromRefStruct()
                {{
                    UpdateStoredBuffer();
                    var clone = new {NameIncludingGenerics}(LazinatorMemoryStorage);
                    return clone;
                }}
            ");
        }


        private void AppendGeneralDefinitions(CodeStringBuilder sb)
        {
            string additionalDescendantDirtinessChecks = GetDescendantDirtinessChecks(false);
            string additionalDescendantHasChangedChecks = GetDescendantDirtinessChecks(true);
            string constructors = GetConstructors();
            string cloneMethod = GetCloneMethod();

            if (!IsDerivedFromNonAbstractLazinator)
            {
                string boilerplate;
                if (IsAbstract && BaseLazinatorObject?.IsAbstract == true) // abstract class inheriting from abstract class
                    boilerplate = $@""; // everything is inherited from parent abstract class
                else if (IsAbstract && !GeneratingRefStruct)
                    boilerplate = $@"        /* Abstract declarations */
			            public abstract LazinatorParentsCollection LazinatorParents {{ get; set; }}
                    
                        {ProtectedIfApplicable}abstract int Deserialize();
                        
                        public abstract LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
                        
                        public abstract {ILazinatorString} CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers);

                        {IIF(!ImplementsAssignCloneProperties, $@"{ProtectedIfApplicable}abstract {ILazinatorString} AssignCloneProperties({ILazinatorStringWithoutQuestionMark} clone, IncludeChildrenMode includeChildrenMode);

                        ")}{HideILazinatorProperty}public abstract bool HasChanged
                        {{
			                get;
			                set;
                        }}

                        {HideILazinatorProperty}public abstract bool IsDirty
                        {{
			                get;
			                set;
                        }}
                        
                        {HideILazinatorProperty}public abstract bool DescendantHasChanged
                        {{
			                get;
			                set;
                        }}

                        {HideILazinatorProperty}public abstract bool DescendantIsDirty
                        {{
			                get;
			                set;
                        }}

                        public abstract bool NonBinaryHash32
                        {{
                            get;
                        }}
                        public abstract IEnumerable<{ILazinatorString}> EnumerateLazinatorNodes(Func<{ILazinatorString}, bool>{QuestionMarkIfNullableModeEnabled} matchCriterion, bool stopExploringBelowMatch, Func<{ILazinatorString}, bool>{QuestionMarkIfNullableModeEnabled} exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
                        public abstract IEnumerable<(string propertyName, {ILazinatorString} descendant)> EnumerateLazinatorDescendants(Func<{ILazinatorString}, bool>{QuestionMarkIfNullableModeEnabled} matchCriterion, bool stopExploringBelowMatch, Func<{ILazinatorString}, bool>{QuestionMarkIfNullableModeEnabled} exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
                        public abstract IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties();
                        public abstract {ILazinatorString} ForEachLazinator(Func<{ILazinatorString}, {ILazinatorString}>{QuestionMarkIfNullableModeEnabled} changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel);
		                
                        {ProtectedIfApplicable}abstract void DeserializeLazinator(LazinatorMemory serializedBytes);

                        {HideILazinatorProperty}public abstract LazinatorMemory LazinatorMemoryStorage
                        {{
			                get;
			                set;
                        }}

                        {HideILazinatorProperty}public abstract IncludeChildrenMode OriginalIncludeChildrenMode
                        {{
                            get;
                            set;
                        }}

                        public abstract void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren);
                        public abstract void UpdateStoredBuffer();
                        public abstract void FreeInMemoryObjects();
        
                ";
                else
                {
                    string readUniqueID;
                    if (!ContainsOpenGenericParameters && IsSealedOrStruct)
                        readUniqueID = $@"{(UniqueIDCanBeSkipped ? "" : $@"int uniqueID = span.ToDecompressedInt32(ref bytesSoFar);
                            if (uniqueID != LazinatorUniqueID)
                            {{
                                ThrowHelper.ThrowFormatException();
                            }}

                            ")}";
                    else
                        readUniqueID = $@"ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);

                                    ";

                    boilerplate = $@"
                        /* Serialization, deserialization, and object relationships */

                        {constructors}{HideILazinatorProperty}public {DerivationKeyword}LazinatorParentsCollection LazinatorParents {{ get; set; }}

                        {HideILazinatorProperty}public {DerivationKeyword}IncludeChildrenMode OriginalIncludeChildrenMode {{ get; set; }}

                        {ProtectedIfApplicable}{DerivationKeyword}int Deserialize()
                        {{
                            FreeInMemoryObjects();
                            int bytesSoFar = 0;
                            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
                            if (span.Length == 0)
                            {{
                                return 0;
                            }}

                            {readUniqueID}{(SuppressLazinatorVersionByte ? "" : $@"int lazinatorLibraryVersion = span.ToDecompressedInt32(ref bytesSoFar);
                            
                        ")}int serializedVersionNumber = {(Version == -1 ? "-1; /* versioning disabled */" : $@"span.ToDecompressedInt32(ref bytesSoFar);")}

                            OriginalIncludeChildrenMode = {(CanNeverHaveChildren ? "IncludeChildrenMode.IncludeAllChildren; /* cannot have children */" : $@"(IncludeChildrenMode)span.ToByte(ref bytesSoFar);")}

                            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);{
                            IIF(ImplementsLazinatorObjectVersionUpgrade && Version != -1,
                                $@"
                            if (serializedVersionNumber < LazinatorObjectVersion)
                            {{
                                LazinatorObjectVersionUpgrade(serializedVersionNumber);
                            }}")
                        }{  String.Join("", ExclusiveInterface.PropertiesToDefineThisLevel.Where(x => x.HasEagerAttribute && !x.NonNullableThatRequiresInitialization).Select(x => $@"
                                _ = {x.PropertyName};"))
                        }{
                            IIF(ImplementsPostDeserialization,
                                $@"
                            PostDeserialization();")
                        }
                            return bytesSoFar;
                        }}

                        public {DerivationKeyword}LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
                        {{
                            if (LazinatorMemoryStorage.IsEmpty || includeChildrenMode != OriginalIncludeChildrenMode || (verifyCleanness || IsDirty || (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
                            {{
                                return EncodeToNewBuffer(includeChildrenMode, verifyCleanness, updateStoredBuffer);
                            }}
                            BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
                            LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
                            return writer.LazinatorMemory;
                        }}

                        {ProtectedIfApplicable}{DerivationKeyword}LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
                        {{
                            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
                            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
                            SerializeToExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                            return writer.LazinatorMemory;
                        }}

                        {cloneMethod}

                        {HideILazinatorProperty}public {DerivationKeyword}bool HasChanged {{ get; set; }}

                        {HideBackingField}{ProtectedIfApplicable}bool _IsDirty;
                        {HideILazinatorProperty}public {DerivationKeyword}bool IsDirty
                        {{
                            [DebuggerStepThrough]
                            get => _IsDirty{IIF(!(ObjectType == LazinatorObjectType.Struct || GeneratingRefStruct), "|| LazinatorMemoryStorage.Length == 0")};
                            [DebuggerStepThrough]
                            set
                            {{
                                if (_IsDirty != value)
                                {{
                                    _IsDirty = value;
                                    if (_IsDirty)
                                    {{
                                        LazinatorParents.InformParentsOfDirtiness();{IIF(ImplementsOnDirty, $@"
                                        OnDirty();")}
                                        HasChanged = true;
                                    }}
                                }}
                            }}
                        }}

                        {HideBackingField}{ProtectedIfApplicable}bool _DescendantHasChanged;
                        {HideILazinatorProperty}public {DerivationKeyword}bool DescendantHasChanged
                        {{
                            [DebuggerStepThrough]
                            get => _DescendantHasChanged{additionalDescendantHasChangedChecks};
                            [DebuggerStepThrough]
                            set
                            {{
                                _DescendantHasChanged = value;
                            }}
                        }}

                        {HideBackingField}{ProtectedIfApplicable}bool _DescendantIsDirty;
                        {HideILazinatorProperty}public {DerivationKeyword}bool DescendantIsDirty
                        {{
                            [DebuggerStepThrough]
                            get => _DescendantIsDirty{additionalDescendantDirtinessChecks};
                            [DebuggerStepThrough]
                            set
                            {{
                                if (_DescendantIsDirty != value)
                                {{
                                    _DescendantIsDirty = value;
                                    if (_DescendantIsDirty)
                                    {{
                                        LazinatorParents.InformParentsOfDirtiness();{IIF(ImplementsOnDescendantIsDirty, $@"
                                        OnDescendantIsDirty();")}
                                        _DescendantHasChanged = true;
                                    }}
                                }}
                            }}
                        }}
        
                        {ProtectedIfApplicable}{DerivationKeyword}void DeserializeLazinator(LazinatorMemory serializedBytes)
                        {{
                            LazinatorMemoryStorage = serializedBytes;
                            int length = Deserialize();
                            if (length != LazinatorMemoryStorage.Length)
                            {{
                                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
                            }}
                        }}

                        {HideILazinatorProperty}public {DerivationKeyword}LazinatorMemory LazinatorMemoryStorage
                        {{
                            get;
                            set;
                        }}

                        public {DerivationKeyword}void UpdateStoredBuffer()
                        {{
                            if (!IsDirty && !DescendantIsDirty && LazinatorMemoryStorage.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                            {{
                                return;
                            }}
                            var previousBuffer = LazinatorMemoryStorage;
                            if (LazinatorMemoryStorage.IsEmpty || IncludeChildrenMode.IncludeAllChildren != OriginalIncludeChildrenMode || (IsDirty || DescendantIsDirty))
                            {{
                                LazinatorMemoryStorage = EncodeToNewBuffer(IncludeChildrenMode.IncludeAllChildren, false, true);
                            }}
                            else
                            {{
                                BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
                                LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
                                LazinatorMemoryStorage = writer.LazinatorMemory;
                            }}
                            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
                            if (!LazinatorParents.Any())
                            {{
                                previousBuffer.Dispose();
                            }}
                        }}

                        public {DerivationKeyword}bool NonBinaryHash32 => {(NonbinaryHash ? "true" : "false")};
        
                ";
                }


                sb.Append(boilerplate);
            }
            else if (!IsAbstract || GeneratingRefStruct)
            {
                sb.Append($@"        /* Clone overrides */

                        {constructors}{cloneMethod}

                        /* Properties */
");
            }
        }

        private string GetCloneMethod()
        {
            if (GeneratingRefStruct)
                return "";
            string parametersToFirstConstructor = "";
            var allPropertiesRequiringInitialization = ExclusiveInterface.PropertiesIncludingInherited.Where(x => x.NonNullableThatRequiresInitialization).ToList();
            if (allPropertiesRequiringInitialization.Any())
                parametersToFirstConstructor = String.Join(", ", allPropertiesRequiringInitialization.Select(x => x.PropertyName)) + ", ";
            return $@"public {DerivationKeyword}{ILazinatorString} CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
                        {{
                            {NameIncludingGenerics} clone;
                            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
                            {{
                                clone = new {NameIncludingGenerics}({parametersToFirstConstructor}includeChildrenMode);{IIF(Version != -1, $@"
                                clone.LazinatorObjectVersion = LazinatorObjectVersion;")}
                                clone = ({NameIncludingGenerics})AssignCloneProperties(clone, includeChildrenMode){IIF(NullableModeEnabled, "!")};
                            }}
                            else
                            {{
                                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                                clone = new {NameIncludingGenerics}(bytes);
                            }}{IIF(ImplementsOnClone, $@"
            clone.OnCompleteClone(this);")}
                            return clone;
                        }}{IIF(!ImplementsAssignCloneProperties, $@"

                        {ProtectedIfApplicable}{DerivationKeyword}{ILazinatorString} AssignCloneProperties({ILazinatorStringWithoutQuestionMark} clone, IncludeChildrenMode includeChildrenMode)
                        {{
                            {(IsDerivedFromNonAbstractLazinator ? $"base.AssignCloneProperties(clone, includeChildrenMode);" : $"clone.FreeInMemoryObjects();")}
                            {NameIncludingGenerics} typedClone = ({NameIncludingGenerics}) clone;
                            {AppendCloneProperties()}{IIF(ObjectType == LazinatorObjectType.Struct || GeneratingRefStruct, $@"
                            typedClone.IsDirty = false;")}
                            return typedClone;
                        }}")}";

        }


        private string AppendCloneProperties()
        {
            if (GeneratingRefStruct)
                return "";
            CodeStringBuilder sb = new CodeStringBuilder();
            string nameOfCloneVariable = "typedClone";
            foreach (var property in PropertiesToDefineThisLevel.Where(x => x.PlaceholderMemoryWriteMethod == null))
            {
                property.AppendCopyPropertyToClone(sb, nameOfCloneVariable);
            }
            return sb.ToString();
        }

        private void AppendPropertyDefinitions(CodeStringBuilder sb)
        {
            sb.AppendLine($@"/* Property definitions */
");
            var thisLevel = PropertiesToDefineThisLevel;
            var withRecordedIndices = thisLevel
                .Where(property =>
                    property.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface ||
                    property.PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface ||
                    property.PropertyType == LazinatorPropertyType.LazinatorStruct ||
                    property.PropertyType == LazinatorPropertyType.LazinatorStructNullable ||
                    property.PropertyType == LazinatorPropertyType.NonLazinator ||
                    property.PropertyType == LazinatorPropertyType.SupportedCollection ||
                    property.PropertyType == LazinatorPropertyType.SupportedTuple ||
                    property.PropertyType == LazinatorPropertyType.OpenGenericParameter)
                .ToList();

            var lastPropertyToIndex = withRecordedIndices.LastOrDefault();
            for (int i = 0; i < withRecordedIndices.Count(); i++)
                if (withRecordedIndices[i].DerivationKeyword != "override ")
                    sb.AppendLine($"        {ProtectedIfApplicable}int {withRecordedIndices[i].BackingFieldByteIndex};");
            for (int i = 0; i < withRecordedIndices.Count() - 1; i++)
            {
                PropertyDescription propertyDescription = withRecordedIndices[i];
                string derivationKeyword = GetDerivationKeywordForLengthProperty(propertyDescription);
                sb.AppendLine(
                        $"{ProtectedIfApplicable}{derivationKeyword}int {propertyDescription.BackingFieldByteLength} => {withRecordedIndices[i + 1].BackingFieldByteIndex} - {propertyDescription.BackingFieldByteIndex};");
            }
            if (lastPropertyToIndex != null)
            {
                if (IsAbstract)
                {
                    sb.AppendLine(
                            $"{ProtectedIfApplicable}virtual int {lastPropertyToIndex.BackingFieldByteLength} {{ get; }}"); // defined as virtual so that it's not mandatory to override, since it won't be used if an open generic is redefined.
                }
                else
                {
                    string derivationKeyword = GetDerivationKeywordForLengthProperty(lastPropertyToIndex);
                    if ((ObjectType != LazinatorObjectType.Struct && !GeneratingRefStruct) && (lastPropertyToIndex.PropertyType == LazinatorPropertyType.OpenGenericParameter || derivationKeyword == "override "))
                        sb.AppendLine(
                            $"private int _{ObjectNameEncodable}_EndByteIndex = 0;"); // initialization suppresses warning in case the open generic is never closed
                    else sb.AppendLine(
                            $"private int _{ObjectNameEncodable}_EndByteIndex;");
                    sb.AppendLine(
                            $"{ProtectedIfApplicable}{derivationKeyword}int {lastPropertyToIndex.BackingFieldByteLength} => _{ObjectNameEncodable}_EndByteIndex - {lastPropertyToIndex.BackingFieldByteIndex};");
                }
            }
            sb.AppendLine();

            foreach (var property in thisLevel)
            {
                property.AppendPropertyDefinitionString(sb);
            }
        }

        private void AppendAbstractConversions(CodeStringBuilder sb)
        {
            sb.Append($@"{HideILazinatorProperty}public abstract int LazinatorUniqueID {{ get; }}
                        {HideILazinatorProperty}{ProtectedIfApplicable}{DerivationKeyword}bool ContainsOpenGenericParameters => {(ContainsOpenGenericParameters ? "true" : "false")};
                        {HideILazinatorProperty}public abstract LazinatorGenericIDType LazinatorGenericID {{ get; }}
                        {HideILazinatorProperty}public abstract int LazinatorObjectVersion {{ get; set; }}
                        {(ImplementsConvertFromBytesAfterHeader ? skipConvertFromBytesAfterHeaderString : $@"{ProtectedIfApplicable}abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
                            {ProtectedIfApplicable}abstract void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
                            {ProtectedIfApplicable}abstract int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar);")}
                        public abstract void SerializeToExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
                        {ProtectedIfApplicable}abstract LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
                        {ProtectedIfApplicable}abstract void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition);
                        {(ImplementsWritePropertiesIntoBuffer ? skipWritePropertiesIntoBufferString : $@"{ProtectedIfApplicable}abstract void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID);
                            {ProtectedIfApplicable}abstract void WritePrimitivePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID);
                            {ProtectedIfApplicable}abstract void WriteChildrenPropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID, int startOfObjectPosition, Span<byte> lengthsSpan);
")}
");
        }

        private void AppendResetProperties(CodeStringBuilder sb)
        {
            string resetAccessed = "", resetStorage = "";
            foreach (var property in PropertiesToDefineThisLevel.Where(x => !x.IsPrimitive && x.PlaceholderMemoryWriteMethod == null)) // Note that we will free in memory objects even for non-nullables. We can do this because we have nullable backing properties for them. 
            {
                if (property.BackingAccessFieldIncluded)
                {
                    if (!property.NonNullableThatRequiresInitialization)
                        resetStorage += $@"{property.BackingFieldString} = default;
                            ";
                    resetAccessed += $"{property.BackingFieldAccessedString} = ";
                }
            }
            if (resetAccessed != "")
                resetAccessed += $@"false;";

            sb.AppendLine($@"
                public {DerivationKeyword}void FreeInMemoryObjects()
                {{
                    {IIF(IsDerivedFromNonAbstractLazinator, $@"base.FreeInMemoryObjects();
                    ")}{resetStorage}{resetAccessed}
                    IsDirty = false;
                    DescendantIsDirty = false;
                    HasChanged = false;
                    DescendantHasChanged = false;{IIF(ImplementsOnFreeInMemoryObjects, $@"
                                        OnFreeInMemoryObjects();")}
                }}");
        }

        private void AppendEnumerateLazinatorDescendants(CodeStringBuilder sb)
        {
            if (IsAbstract || GeneratingRefStruct)
                return;
            else
            {
                if (IsDerivedFromNonAbstractLazinator)
                {
                    // we've already defined EnumerateLazinatorNodes, so we just need to override the Helper function, calling the base function
                    if (!ImplementsEnumerateLazinatorDescendants)
                    {
                        sb.Append($@"
                            public override IEnumerable<(string propertyName, {ILazinatorString} descendant)> EnumerateLazinatorDescendants(Func<{ILazinatorString}, bool>{QuestionMarkIfNullableModeEnabled} matchCriterion, bool stopExploringBelowMatch, Func<{ILazinatorString}, bool>{QuestionMarkIfNullableModeEnabled} exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
                            {{
                                foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                                {{
                                    yield return inheritedYield;
                                }}
                            ");
                    }
                }
                else
                {
                    string derivationKeyword = IIF(IsDerivedFromAbstractLazinator, "override ");
                    // we need EnumerateLazinatorNodes, plus EnumerateLazinatorDescendants but without a call to a base function
                    sb.AppendLine($@"
                            public {derivationKeyword}IEnumerable<{ILazinatorString}> EnumerateLazinatorNodes(Func<{ILazinatorString}, bool>{QuestionMarkIfNullableModeEnabled} matchCriterion, bool stopExploringBelowMatch, Func<{ILazinatorString}, bool>{QuestionMarkIfNullableModeEnabled} exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
                            {{
                                bool match = (matchCriterion == null) ? true : matchCriterion(this);
                                bool explore = (!match || !stopExploringBelowMatch) && ((exploreCriterion == null) ? true : exploreCriterion(this));
                                if (match)
                                {{
                                    yield return this;
                                }}
                                if (explore)
                                {{
                                    foreach (var item in EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                                    {{
                                        yield return item.descendant;
                                    }}
                                }}
                            }}
                        ");
                    if (!ImplementsEnumerateLazinatorDescendants)
                        sb.AppendLine(
                            $@"public {DerivationKeyword}IEnumerable<(string propertyName, {ILazinatorString} descendant)> EnumerateLazinatorDescendants(Func<{ILazinatorString}, bool>{QuestionMarkIfNullableModeEnabled} matchCriterion, bool stopExploringBelowMatch, Func<{ILazinatorString}, bool>{QuestionMarkIfNullableModeEnabled} exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
                            {{");
                }

                if (!ImplementsEnumerateLazinatorDescendants)
                {
                    foreach (var property in PropertiesToDefineThisLevel.Where(x => x.IsLazinator))
                    {
                        string propertyName = property.PropertyName;
                        string elseConsequent = new ConditionalCodeGenerator(
                                    ConditionsCodeGenerator.OrCombine(
                                        ConditionsCodeGenerator.AndCombine(
                                            $"!exploreOnlyDeserializedChildren",
                                            property.GetNonNullCheck(false)),
                                        property.GetNonNullCheck(true)),
                                   $@"bool isMatch_{propertyName} = matchCriterion == null || matchCriterion({propertyName});
                                    bool shouldExplore_{propertyName} = exploreCriterion == null || exploreCriterion({propertyName});
                                    if (isMatch_{propertyName})
                                    {{
                                        yield return (""{propertyName}"", {propertyName});
                                    }}
                                    if ((!stopExploringBelowMatch || !isMatch_{propertyName}) && shouldExplore_{propertyName})
                                    {{
                                        foreach (var toYield in {propertyName}{property.NullForgiveness}.{IIF(property.PropertyType == LazinatorPropertyType.LazinatorStructNullable || (property.IsDefinitelyStruct && property.Nullable), "Value.")}EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                                        {{
                                            yield return (""{propertyName}"" + ""."" + toYield.propertyName, toYield.descendant);
                                        }}
                                    }}").ToString();
                        string ifThenStatement = property.GetNullCheckPlusPrecedingConditionIfThen(
                                ConditionsCodeGenerator.AndCombine(
                                    "enumerateNulls",
                                    ConditionsCodeGenerator.OrCombine(
                                        "!exploreOnlyDeserializedChildren",
                                        property.BackingFieldAccessedString))
                                , propertyName,
                                // CONSEQUENT
                                $@"yield return (""{propertyName}"", default);",
                                // ELSE CONSEQUENT -- another conditional
                                elseConsequent);
                        sb.AppendLine(ifThenStatement);
                    }
                    sb.Append($@"yield break;
                            }}
                        ");
                }
            }
        }

        private void AppendEnumerateNonLazinatorProperties(CodeStringBuilder sb)
        {
            if (IsAbstract || GeneratingRefStruct)
                return;
            else
            {
                if (IsDerivedFromNonAbstractLazinator)
                {
                    sb.Append($@"

                        public override IEnumerable<(string propertyName, object{QuestionMarkIfNullableModeEnabled} descendant)> EnumerateNonLazinatorProperties()
                        {{
                                foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
                                {{
                                    yield return inheritedYield;
                                }}
                        ");
                }
                else
                {
                    string derivationKeyword = IIF(IsDerivedFromAbstractLazinator, "override ");
                    sb.AppendLine(
                        $@"

                        public {DerivationKeyword}IEnumerable<(string propertyName, object{QuestionMarkIfNullableModeEnabled} descendant)> EnumerateNonLazinatorProperties()
                        {{");
                }

                foreach (var property in PropertiesToDefineThisLevel.Where(x => !x.IsLazinator && x.PlaceholderMemoryWriteMethod == null))
                {
                    if (!property.DoNotEnumerate)
                    {
                        if (property.PropertyType == LazinatorPropertyType.SupportedCollection && property.SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan)
                        {
                            // because ReadOnlySpan is a ref struct, we can't enumerate it.
                            sb.Append($@"yield return (""{property.PropertyName}"", (object{QuestionMarkIfNullableModeEnabled}){property.PropertyName}.ToString());
                                    ");
                        }
                        else
                            sb.Append($@"yield return (""{property.PropertyName}"", (object{QuestionMarkIfNullableModeEnabled}){property.PropertyName});
                                    ");
                    }
                }
                sb.Append($@"yield break;
                        }}
                    ");
            }
        }

        private void AppendForEachLazinator(CodeStringBuilder sb)
        {
            if (IsAbstract || GeneratingRefStruct)
                return;
            sb.Append($@"
                    public {DerivationKeyword}{ILazinatorString} ForEachLazinator(Func<{ILazinatorString}, {ILazinatorString}>{QuestionMarkIfNullableModeEnabled} changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
                    {{
                        {IIF(IsDerivedFromNonAbstractLazinator, $@"base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
                        ")}");
            ConditionCodeGenerator getAntecedent(PropertyDescription property) =>
                ConditionsCodeGenerator.OrCombine(
                    ConditionsCodeGenerator.AndCombine(
                        "!exploreOnlyDeserializedChildren",
                        property.GetNonNullCheck(false)),
                    property.GetNonNullCheck(true));
            bool nonNullCheckDefinitelyTrue(PropertyDescription property) => property.GetNonNullCheck(false).ToString() == "true";
            foreach (var property in PropertiesToDefineThisLevel.Where(x => x.IsLazinator && x.PlaceholderMemoryWriteMethod == null))
            {
                string propertyName = property.PropertyName;
                sb.AppendLine(new ConditionalCodeGenerator(getAntecedent(property),
                        $@"{IIF(nonNullCheckDefinitelyTrue(property), $@"var deserialized_{propertyName} = {propertyName};
                            ")}_{propertyName} = ({property.AppropriatelyQualifiedTypeName}) _{propertyName}{property.NullForgiveness}{IIF(property.PropertyType == LazinatorPropertyType.LazinatorStructNullable || (property.IsDefinitelyStruct && property.Nullable), ".Value")}.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);").ToString());
            }
            foreach (var property in PropertiesToDefineThisLevel.Where(x => x.IsSupportedCollectionOrTupleOrNonLazinatorWithInterchangeType && !x.IsMemoryOrSpan))
            {
                string propertyName = property.PropertyName;

                sb.AppendLine(new ConditionalCodeGenerator(getAntecedent(property),
                        $@"{IIF(nonNullCheckDefinitelyTrue(property), $@"var deserialized_{propertyName} = {propertyName};
                            ")}_{propertyName} = ({property.AppropriatelyQualifiedTypeName}) CloneOrChange_{property.AppropriatelyQualifiedTypeNameEncodable}({property.BackingFieldAccessWithPossibleException}, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true){property.InnerProperties?[0].PossibleUnsetException}, true);").ToString());
            }
            foreach (var property in PropertiesToDefineThisLevel.Where(x => x.IsNonLazinatorTypeWithoutInterchange && x.PlaceholderMemoryWriteMethod == null))
            {
                string propertyName = property.PropertyName;
                sb.AppendLine(new ConditionalCodeGenerator(getAntecedent(property),
                        $@"{IIF(nonNullCheckDefinitelyTrue(property), $@"var deserialized_{propertyName} = {propertyName};
                            ")}_{propertyName} = {property.DirectConverterTypeNamePrefix}CloneOrChange_{property.AppropriatelyQualifiedTypeNameEncodable}(_{propertyName}, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);").ToString());
            }

            foreach (var property in PropertiesToDefineThisLevel.Where(x => ((!x.IsPrimitive && !x.IsLazinator && !x.IsSupportedCollectionOrTupleOrNonLazinatorWithInterchangeType && !x.IsNonLazinatorTypeWithoutInterchange) || x.IsMemoryOrSpan) && x.PlaceholderMemoryWriteMethod == null))
            {
                // we want to deserialize the memory. In case of ReadOnlySpan<byte>, we also want to duplicate the memory if it hasn't been set by the user, since we want to make sure that the property will work even if the buffer is removed (which might be the reason for the ForEachLazinator call)
                sb.Append($@"if (!exploreOnlyDeserializedChildren)
                    {{
                        var deserialized{property.BackingFieldString} = {property.PropertyName};{IIF(property.SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan, $@"
                        if ({property.BackingFieldNotAccessedString})
                        {{
                            {property.PropertyName} = deserialized{property.BackingFieldString};
                        }}")}
                    }}
");
            }

            sb.Append($@"{IIF(ImplementsOnForEachLazinator && (BaseLazinatorObject == null || !BaseLazinatorObject.ImplementsOnForEachLazinator), $@"OnForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, changeThisLevel);
                    ")}if (changeThisLevel && changeFunc != null)
                        {{
                            return changeFunc(this);
                        }}
                        return this;
                    }}
                ");
        }

        private void AppendConversionSectionStart(CodeStringBuilder sb)
        {
            string containsOpenGenericParametersString = $@"{HideILazinatorProperty}{ProtectedIfApplicable}{DerivationKeyword}bool ContainsOpenGenericParameters => {(ContainsOpenGenericParameters ? "true" : "false")};";

            string lazinatorGenericID;
            if (ContainsOpenGenericParameters)
                lazinatorGenericID = $@"{containsOpenGenericParametersString}
                        {HideILazinatorProperty}public {DerivationKeyword}LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<{NameIncludingGenerics}>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType({ UniqueID }, new Type[] {{ {GenericArgumentNameTypes} }}));
                    ";
            else
                lazinatorGenericID = $@"{containsOpenGenericParametersString}
                        {HideILazinatorProperty}public {DerivationKeyword}LazinatorGenericIDType LazinatorGenericID => default;
                    ";

            string selfSerializationVersionString;
            if (Version == -1)
                selfSerializationVersionString = $@"{HideILazinatorProperty}public {DerivationKeyword}int LazinatorObjectVersion
                {{
                    get => -1;
                    set => ThrowHelper.ThrowVersioningDisabledException(""{NameIncludingGenerics}"");
                }}";
            else if (ObjectType == LazinatorObjectType.Class && !GeneratingRefStruct)
                selfSerializationVersionString = $@"{HideILazinatorProperty}public {DerivationKeyword}int LazinatorObjectVersion {{ get; set; }} = {Version};"; // even if versioning is disabled, we still need to implement the interface
            else
            { // can't set default property value in struct, so we have a workaround. If the version has not been changed, we assume that it is still Version. 
                selfSerializationVersionString =
                        $@"{HideBackingField}private bool _LazinatorObjectVersionChanged;
                        {HideBackingField}private int _LazinatorObjectVersionOverride;
                        {HideILazinatorProperty}public int LazinatorObjectVersion
                        {{
                            get => _LazinatorObjectVersionChanged ? _LazinatorObjectVersionOverride : { Version };
                            set
                            {{
                                _LazinatorObjectVersionOverride = value;
                                _LazinatorObjectVersionChanged = true;
                            }}
                        }}";
            }
            sb.AppendLine($@"
                /* Conversion */

                {HideILazinatorProperty}public {DerivationKeyword}int LazinatorUniqueID => { UniqueID };

                {lazinatorGenericID}

                { selfSerializationVersionString }

                ");
        }

        private void AppendCloseClassSupertypesAndNamespace(CodeStringBuilder sb, IEnumerable<ITypeSymbol> supertypes)
        {
            if (supertypes != null)
                foreach (var supertype in supertypes)
                    sb.Append($@"
                                }}
                            ");
            if (GeneratingRefStruct)
                sb.Append($@"
                            }}
                        ");
            else
                sb.Append($@"
                            }}
                        }}
                        ");
        }

        private void AppendSupportedConversions(CodeStringBuilder sb)
        {
            var propertiesSupportedCollections = PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.SupportedCollection).ToList();
            var propertiesSupportedTuples = PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.SupportedTuple).ToList();
            var propertiesNonSerialized = PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.NonLazinator).ToList();

            GetSupportedConversions(sb, propertiesSupportedCollections, propertiesSupportedTuples, propertiesNonSerialized);
        }

        private void AppendSerializeToExistingBuffer(CodeStringBuilder sb)
        {

            if (IsDerivedFromNonAbstractLazinator)
                sb.AppendLine(
                        $@"public override void SerializeToExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                        {{");
            else
                sb.AppendLine(
                        $@"public {DerivationKeyword}void SerializeToExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                        {{");

            if (IncludeTracingCode)
            {
                sb.AppendLine($@"TabbedText.WriteLine($""Initiating serialization of {ILazinatorTypeSymbol} "");");
            }

            sb.AppendLine($@"if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                            {{
                                updateStoredBuffer = false;
                            }}");

            sb.AppendLine($@"{ IIF(ImplementsPreSerialization, $@"PreSerialization(verifyCleanness, updateStoredBuffer);
                            ")}int startPosition = writer.Position;
                            WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, {(UniqueIDCanBeSkipped ? "false" : "true")});");

            sb.AppendLine($@"if (updateStoredBuffer)
                        {{
                            UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);");
            sb.Append($@"}}
");
            sb.Append($@"}}
");
            AppendUpdateStoredBufferMethod(sb);
        }

        private void AppendUpdateStoredBufferMethod(CodeStringBuilder sb)
        {
            sb.AppendLine($@"
            public {DerivationKeyword}void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
            {{");
            GetCodeBeforeBufferIsUpdated(sb);
            sb.AppendLine($@"
                var newBuffer = writer.Slice(startPosition, length);
                LazinatorMemoryStorage = newBuffer;");
            sb.Append($@"}}
");
            AppendUpdateDeserializedChildren(sb);
        }

        private void GetCodeBeforeBufferIsUpdated(CodeStringBuilder sb)
        {
            sb.Append($@"_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {{
                            _DescendantIsDirty = false;");
            sb.AppendLine($@"
                        if (updateDeserializedChildren)
                        {{
                            UpdateDeserializedChildren(ref writer, startPosition);");


            sb.AppendLine($@"{IIF(ImplementsOnFreeInMemoryObjects, $@"OnUpdateDeserializedChildren(ref writer, startPosition);
                                    ")}}}");
            sb.Append(GetStructAndOpenGenericReset()); // needed for dirtiness to be set correctly
            sb.AppendLine($@"
                    }}
                    else
                    {{
                        ThrowHelper.ThrowCannotUpdateStoredBuffer();
                    }}");
        }

        private string GetStructAndOpenGenericReset()
        {
            string reset = "";
            foreach (var property in PropertiesIncludingInherited.Where(x => x.BackingAccessFieldIncluded && (x.PropertyType == LazinatorPropertyType.LazinatorStruct || x.PropertyType == LazinatorPropertyType.LazinatorStructNullable)))
            {
                reset +=
                    $@"
                    {property.BackingFieldAccessedString} = false;"; // force deserialization to make the struct clean
            }
            foreach (var property in PropertiesIncludingInherited
                .Where(x => x.PropertyType == LazinatorPropertyType.OpenGenericParameter))
            {
                if (property.BackingAccessFieldIncluded)
                {
                    if (!property.GenericConstrainedToStruct)
                        reset +=
                            $@"
                            if ({property.BackingFieldAccessedString} && {property.BackingFieldString} != null && {property.BackingFieldString}.IsStruct && ({property.BackingFieldString}.IsDirty || {property.BackingFieldString}.DescendantIsDirty))
                            {{
                                {property.BackingFieldAccessedString} = false;
                            }}";
                    else
                        reset +=
                            $@"
                            if ({property.BackingFieldAccessedString} && {property.BackingFieldString}{IIF(property.Nullable, ".Value")}.IsStruct && ({property.BackingFieldString}{IIF(property.Nullable, ".Value")}.IsDirty || {property.BackingFieldString}{IIF(property.Nullable, ".Value")}.DescendantIsDirty))
                            {{
                                {property.BackingFieldAccessedString} = false;
                            }}";
                }
            }

            return reset;
        }

        private void AppendUpdateDeserializedChildren(CodeStringBuilder sb)
        {
            if (IsDerivedFromNonAbstractLazinator)
                sb.AppendLine(
                    $@"
                        {ProtectedIfApplicable}override void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                        {{
                            base.UpdateDeserializedChildren(ref writer, startPosition);");
            else
                sb.AppendLine(
                    $@"
                        {ProtectedIfApplicable}{DerivationKeyword}void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                        {{");
            foreach (var property in PropertiesToDefineThisLevel.Where(x => !x.IsPrimitive && !x.IsNonLazinatorType && x.PlaceholderMemoryWriteMethod == null))
            {
                sb.AppendLine(new ConditionalCodeGenerator(property.GetNonNullCheck(true),
$@"{property.PropertyName}{property.NullForgiveness}{IIF(property.PropertyType == LazinatorPropertyType.LazinatorStructNullable || (property.IsDefinitelyStruct && property.Nullable), ".Value")}.UpdateStoredBuffer(ref writer, startPosition + {property.BackingFieldByteIndex}{property.IncrementChildStartBySizeOfLength}, {property.BackingFieldByteLength}{property.DecrementTotalLengthBySizeOfLength}, IncludeChildrenMode.IncludeAllChildren, true);").ToString());
            }
            foreach (var property in PropertiesToDefineThisLevel.Where(x => x.IsSupportedCollectionOrTupleOrNonLazinatorWithInterchangeType && x.PlaceholderMemoryWriteMethod == null))
            {
                if (property.IsMemoryOrSpan)
                {
                }
                else
                {
                    string propertyName = property.PropertyName;
                    sb.AppendLine(new ConditionalCodeGenerator(property.GetNonNullCheck(true),
$@"_{propertyName} = ({property.AppropriatelyQualifiedTypeName}) CloneOrChange_{property.AppropriatelyQualifiedTypeNameEncodable}({property.BackingFieldWithPossibleValueDereferenceWithPossibleException}, l => l.RemoveBufferInHierarchy(), true);").ToString());
                }
            }
            sb.AppendLine($@"}}
                    ");
        }

        string skipWritePropertiesIntoBufferString = "// WritePropertiesIntoBuffer defined in main class; thus skipped here";

        private void AppendWritePropertiesIntoBuffer(CodeStringBuilder sb)
        {
            if (ImplementsWritePropertiesIntoBuffer)
            {
                sb.AppendLine($@"
                            {skipWritePropertiesIntoBufferString}");
                return;
            }

            var thisLevel = PropertiesToDefineThisLevel;
            bool hasChildrenThisLevel = thisLevel.Any(x => !x.IsPrimitive);
            string positionInitialization = !hasChildrenThisLevel && !IsDerived ? $@"" : $@"
                    int startPosition = writer.Position;";

            sb.AppendLine(
                        $@"
                        {ProtectedIfApplicable}{DerivationKeyword}void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                        {{{positionInitialization}");


            if (IncludeTracingCode)
            {
                sb.AppendLine($@"TabbedText.WriteLine($""Writing properties for {ILazinatorTypeSymbol} starting at {{writer.Position}}."");");
                sb.AppendLine($@"TabbedText.WriteLine($""Includes? uniqueID {{(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join("""","""",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))}} {{includeUniqueID}}, Lazinator version {{Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion}} {!SuppressLazinatorVersionByte}, Object version {{LazinatorObjectVersion}} {Version != -1}, IncludeChildrenMode {{includeChildrenMode}} {!CanNeverHaveChildren}"");");
                sb.AppendLine($@"TabbedText.WriteLine($""IsDirty {{IsDirty}} DescendantIsDirty {{DescendantIsDirty}} HasParentClass {{LazinatorParents.Any()}}"");");
            }

            if (ContainsOpenGenericParameters || !IsSealedOrStruct)
                sb.AppendLine($@"if (includeUniqueID)
                        {{
                            if (!ContainsOpenGenericParameters)
                            {{
                                CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                            }}
                            else
                            {{
                                WriteLazinatorGenericID(ref writer, LazinatorGenericID);
                            }}
                        }}");
            else
                sb.AppendLine(
                    $@"if (includeUniqueID)
                        {{
                            CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                        }}
                    ");

            sb.AppendLine(
                    $@"{(SuppressLazinatorVersionByte ? "" : $@"CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                    ")}{(Version == -1 ? "" : $@"CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                    ")}{(CanNeverHaveChildren ? "" : $@"writer.Write((byte)includeChildrenMode);")}");

            sb.AppendLine("// write properties");


            IEnumerable<PropertyDescription> primitiveProperties = thisLevel.Where(x => x.IsPrimitive);
            IEnumerable<PropertyDescription> childrenProperties = thisLevel.Where(x => !x.IsPrimitive);
            sb.AppendLine($@"
                            {IIF(primitiveProperties.Any() || IsDerivedFromNonAbstractLazinator, $@"WritePrimitivePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);")}{IIF(childrenProperties.Any() || IsDerivedFromNonAbstractLazinator, $@"
                            {GetLengthsCalculation(false)}Span<byte> lengthsSpan = writer.FreeSpan.Slice(0, lengthForLengths);
                            writer.Skip(lengthForLengths);
                            WriteChildrenPropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID, startPosition, lengthsSpan);")}");

            if (IncludeTracingCode)
            {
                sb.AppendLine($@"TabbedText.WriteLine($""Byte {{writer.Position}} (end of {NameIncludingGenerics}) "");");
            }

            if (ImplementsOnPropertiesWritten)
            {
                sb.AppendLine($@"OnPropertiesWritten(updateStoredBuffer);");
            }
            sb.Append($@"}}
");
            if (primitiveProperties.Any() || !IsSealedOrStruct)
                AppendWritePropertiesHelper(sb, primitiveProperties, true);
            if (childrenProperties.Any() || !IsSealedOrStruct)
                AppendWritePropertiesHelper(sb, childrenProperties, false);

        }

        public string GetLengthsCalculation(bool readVersion)
        {
            var properties = PropertiesIncludingInherited.Where(x => !x.IsPrimitive && x.BytesUsedForLength() > 0).Select(x => (x.InclusionConditionalHelper(readVersion).ToString(), x.BytesUsedForLength()));
            Dictionary<string, int> distinct = new Dictionary<string, int>();
            foreach (var property in properties)
            {
                if (distinct.ContainsKey(property.Item1))
                    distinct[property.Item1] += property.Item2;
                else
                    distinct[property.Item1] = property.Item2;
            }
            string variableInitializationString = "0";
            var alwaysTrue = distinct.FirstOrDefault(x => x.Key == "true");
            if (alwaysTrue.Key != null)
                variableInitializationString = alwaysTrue.Value.ToString();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"int lengthForLengths = {variableInitializationString};");
            IEnumerable<KeyValuePair<string, int>> results = distinct.Where(x => x.Key != "true" && x.Key != "false").OrderBy(x => x.Key);
            // we want to indent these results, so if we have X and then X && Y, we format as 
            // if (X)
            // {
            //   if (Y)
            //   ...
            // So, let's create a stack showing the active prefixes;
            Stack<string> s = new Stack<string>();
            foreach (var result in results)
            {
                while (s.Any() && !result.Key.StartsWith(s.Peek()))
                {
                    s.Pop();
                    sb.AppendLine($"}}");
                }
                string portionOfStringToInclude;
                if (s.Any())
                    portionOfStringToInclude = result.Key.Substring(s.Peek().Length + 4); // include " && "
                else
                    portionOfStringToInclude = result.Key;
                sb.AppendLine($@"if ({portionOfStringToInclude})
                    {{{IIF(result.Value > 0, $@"
                        lengthForLengths += {result.Value};")}");
                s.Push(result.Key);
            } 
            while (s.Any())
            {
                s.Pop();
                sb.AppendLine($"}}");
            }
            return sb.ToString();
        }

        private void AppendWritePropertiesHelper(CodeStringBuilder sb, IEnumerable<PropertyDescription> propertiesToWrite, bool isPrimitive)
        {
            string methodName = isPrimitive ? "WritePrimitivePropertiesIntoBuffer" : "WriteChildrenPropertiesIntoBuffer";

            if (IsDerivedFromNonAbstractLazinator)
                sb.AppendLine(
                        $@"
                        {ProtectedIfApplicable}override void {methodName}(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID{IIF(!isPrimitive, $", int startOfObjectPosition, Span<byte> lengthsSpan")})
                        {{
                            base.{methodName}(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID{IIF(!isPrimitive, ", startOfObjectPosition, lengthsSpan")});");
            else
            {
                sb.AppendLine(
                        $@"
                        {ProtectedIfApplicable}{DerivationKeyword}void {methodName}(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID{IIF(!isPrimitive, $", int startOfObjectPosition, Span<byte> lengthsSpan")})
                        {{");
            }
            if (!isPrimitive && propertiesToWrite.Any())
            {
                sb.AppendLine($@"int startOfChildPosition = 0;");
                if (propertiesToWrite.Any(x => x.UsesLengthValue))
                    sb.AppendLine($@"int lengthValue = 0;");
            }


            foreach (var property in propertiesToWrite)
            {
                AppendPropertyWrite(sb, property);
            }

            if (!isPrimitive)
                AppendEndByteIndex(sb, propertiesToWrite, "writer.Position - startOfObjectPosition", true);

            sb.Append($@"}}
");
        }

        private void AppendPropertyWrite(CodeStringBuilder sb, PropertyDescription property)
        {
            if (IncludeTracingCode)
            {
                if (property.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || (property.PropertyType == LazinatorPropertyType.LazinatorStructNullable) || (property.PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface))
                {
                    sb.AppendLine($@"TabbedText.WriteLine($""Byte {{writer.Position}}, {property.PropertyName} (accessed? {IIF(property.BackingAccessFieldIncluded, $"{{{property.BackingFieldAccessedString}}}")}) (backing var null? {{{property.BackingFieldString} == null}}) "");");
                }
                else if (property.PropertyType == LazinatorPropertyType.LazinatorStruct)
                {
                    sb.AppendLine($@"TabbedText.WriteLine($""Byte {{writer.Position}}, {property.PropertyName} (accessed? {IIF(property.BackingAccessFieldIncluded, $"{{{property.BackingFieldAccessedString}}}")}) "");");
                }
                else if (property.TrackDirtinessNonSerialized)
                    sb.AppendLine($@"TabbedText.WriteLine($""Byte {{writer.Position}}, {property.PropertyName} (accessed? {IIF(property.BackingAccessFieldIncluded, $"{{{property.BackingFieldAccessedString}}}")}) (dirty? {{{property.BackingDirtyFieldString}}})"");");
                else if (property.PropertyType == LazinatorPropertyType.NonLazinator || property.PropertyType == LazinatorPropertyType.SupportedCollection || property.PropertyType == LazinatorPropertyType.SupportedTuple)
                    sb.AppendLine($@"TabbedText.WriteLine($""Byte {{writer.Position}}, {property.PropertyName} (accessed? {IIF(property.BackingAccessFieldIncluded, $"{{{property.BackingFieldAccessedString}}}")})"");");
                else
                    sb.AppendLine($@"TabbedText.WriteLine($""Byte {{writer.Position}}, {property.PropertyName} value {{{property.BackingFieldString}}}"");");
                sb.AppendLine($@"TabbedText.Tabs++;");
            }
            property.AppendPropertyWriteString(sb);
            if (IncludeTracingCode)
            {
                sb.AppendLine($@"TabbedText.Tabs--;");
            }
        }

        string skipConvertFromBytesAfterHeaderString = "// ConvertFromBytesAfterHeader defined in main class; thus skipped here";

        private void AppendConvertFromBytesAfterHeader(CodeStringBuilder sb)
        {
            var thisLevel = PropertiesToDefineThisLevel;
            if (ImplementsConvertFromBytesAfterHeader)
            {
                sb.Append($@"{skipConvertFromBytesAfterHeaderString}

                        ");
                return;
            }

            sb.Append($@"{ProtectedIfApplicable}{DerivationKeyword}void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
                {{
                    ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
                    ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
                    {GetLengthsCalculation(true)}ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
                }}
                    
");


            sb.Append($@"{ProtectedIfApplicable}{DerivationKeyword}void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
                {{
                    {(!IsDerivedFromNonAbstractLazinator ? "" : $@"base.ConvertFromBytesForPrimitiveProperties(span, OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
                    ")}");

            foreach (var property in thisLevel)
            {
                if (property.IsPrimitive)
                    property.AppendPropertyReadString(sb);
            }

            sb.Append($@"        }}

        ");

            sb.Append($@"{ProtectedIfApplicable}{DerivationKeyword}int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
                {{
                    {(!IsDerivedFromNonAbstractLazinator ? "int totalChildrenBytes = 0;" : $@"int totalChildrenBytes = base.ConvertFromBytesForChildProperties(span, OriginalIncludeChildrenMode, serializedVersionNumber, indexOfFirstChild, ref bytesSoFar);")}
                ");

            foreach (var property in thisLevel)
            {
                if (!property.IsPrimitive)
                    property.AppendPropertyReadString(sb);
            }

            AppendEndByteIndex(sb, thisLevel, "indexOfFirstChild + totalChildrenBytes", false);

            sb.Append($@"return totalChildrenBytes;
                }}

        ");
        }

        private void AppendEndByteIndex(CodeStringBuilder sb, IEnumerable<PropertyDescription> thisLevel, string endByteString, bool addConditionalForUpdateStoredBuffer)
        {
            var lastProperty = thisLevel.LastOrDefault();
            if (lastProperty != null && lastProperty.PropertyType != LazinatorPropertyType.PrimitiveType && lastProperty.PropertyType != LazinatorPropertyType.PrimitiveTypeNullable)
            {
                if (addConditionalForUpdateStoredBuffer)
                    sb.AppendLine($@"if (updateStoredBuffer)
                                {{");
                sb.Append($@"_{ObjectNameEncodable}_EndByteIndex = {endByteString};
                    ");
                if (addConditionalForUpdateStoredBuffer)
                    sb.AppendLine($@"}}");
            }
        }

        private string GetConstructors()
        {
            // Our constructor accepts as parameters the original include children mode plus all the properties whose backing fields must be initialized (because they are non-nullable reference types)
            bool inheritFromBaseType = ILazinatorTypeSymbol.BaseType != null && !ILazinatorTypeSymbol.BaseType.IsAbstract && IsDerivedFromNonAbstractLazinator;
            var allPropertiesRequiringInitialization = ExclusiveInterface.PropertiesIncludingInherited.Where(x => x.NonNullableThatRequiresInitialization).ToList();

            string firstConstructor;
            string lazinateInSecondConstructor = "";
            if (allPropertiesRequiringInitialization.Any())
            {
                var propertiesRequiringInitializationInBaseClass = ExclusiveInterface.PropertiesInherited.Where(x => x.NonNullableThatRequiresInitialization).ToList();
                var propertiesRequiringInitializationHere = ExclusiveInterface.PropertiesToDefineThisLevel.Where(x => x.NonNullableThatRequiresInitialization).ToList();
                var parametersString = String.Join(", ", allPropertiesRequiringInitialization.Select(x => x.PropertyNameWithTypeNameForConstructorParameter));
                var parametersForBaseClassString = String.Join(", ", propertiesRequiringInitializationInBaseClass.Select(x => x.PropertyNameForConstructorParameter));
                var initializationString = String.Join("", propertiesRequiringInitializationHere.Select(x => x.AssignParameterToBackingField));
                var throwIfNullString = String.Join("", propertiesRequiringInitializationHere.Where(x => !x.IsNonNullableRecordLikeTypeInNullableEnabledContext).Select(x => $@"
                    if ({x.PropertyNameForConstructorParameter} == null)
                    {{
                        throw new ArgumentNullException(""{x.PropertyNameForConstructorParameter}"");
                    }}"));
                lazinateInSecondConstructor = $@"LazinatorMemory childData;
                            " + String.Join("", allPropertiesRequiringInitialization.Select(x => x.GetLazinateContentsForConstructor()));
                firstConstructor = $@"public {SimpleName}{IIF(GeneratingRefStruct, "_RefStruct")}({parametersString}, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren){IIF(inheritFromBaseType, " : base({parametersForBaseClassString}), originalIncludeChildrenMode")}{IIF(IsStruct, " : this()")}
                        {{
                            {initializationString}{throwIfNullString}{IIF(!inheritFromBaseType, $@"
                            OriginalIncludeChildrenMode = originalIncludeChildrenMode;")}
                        }}";
            }
            else
            {
                firstConstructor = firstConstructor = $@"public {SimpleName}{IIF(GeneratingRefStruct, "_RefStruct")}(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren){IIF(inheritFromBaseType, " : base(originalIncludeChildrenMode)")}{IIF(IsStruct, " : this()")}
                        {{{IIF(!inheritFromBaseType, $@"
                            OriginalIncludeChildrenMode = originalIncludeChildrenMode;")}
                        }}";
            }
            string constructors =
                        $@"{firstConstructor}

                        public {SimpleName}{IIF(GeneratingRefStruct, "_RefStruct")}(LazinatorMemory serializedBytes, ILazinator{IIF(NullableModeEnabled, "?")} parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null){IIF(inheritFromBaseType, " : base(serializedBytes, parent, originalIncludeChildrenMode, lazinatorObjectVersion)")}{IIF(IsStruct, " : this()")}
                        {{{IIF(!inheritFromBaseType, $@"
                            if (lazinatorObjectVersion != null)
                            {{
                                LazinatorObjectVersion = (int) lazinatorObjectVersion;
                            }}
                            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
                            LazinatorParents = new LazinatorParentsCollection(parent);
                            DeserializeLazinator(serializedBytes);
                            HasChanged = false;
                            DescendantHasChanged = false;")}{lazinateInSecondConstructor}
                        }}

                        ";
            //if (Compilation.ImplementingTypeRequiresParameterlessConstructor) constructor +=
            //        $@"public {SimpleName}(){IIF(ILazinatorTypeSymbol.BaseType != null, " : base()")}
            //            {{
            //            }}

            //            ";
            return constructors;
        }

        private string GetDescendantDirtinessChecks(bool usePastTense)
        {
            string additionalDescendantDirtinessChecks;
            // we need a way of determining descendant dirtiness manually. We build a set of checks, each beginning with "||" (which, for the first entry, we strip out for one scenario below).
            string manualDescendantDirtinessChecks = "";
            string tense = usePastTense ? "HasChanged" : "IsDirty";
            foreach (var property in PropertiesIncludingInherited.Where(x => x.IsLazinator))
            {
                if (property.PropertyType == LazinatorPropertyType.LazinatorStruct || property.PropertyType == LazinatorPropertyType.LazinatorStructNullable)
                {
                    manualDescendantDirtinessChecks += $" || ({IIF(property.BackingAccessFieldIncluded, $"{property.BackingFieldAccessedString} && ")}({property.PropertyName}{property.NullableStructValueAccessor}.{tense} || {property.PropertyName}{property.NullableStructValueAccessor}.Descendant{tense}))";
                }
                else
                {
                    string nonNullCheck = property.GetNonNullCheck(true);
                    manualDescendantDirtinessChecks += $" || ({nonNullCheck} && ({property.PropertyName}.{tense} || {property.PropertyName}.Descendant{tense}))";
                }
            }
            // The following is not necessary, because manual _Dirty properties automatically lead to _IsDirty being set to true. Because non-Lazinators are not considered "children," nothing needs to happen to DescendantIsDirty; this also means that when encoding, non-Lazinators are encoded if dirty regardless of the include child setting.
            //foreach (var property in PropertiesToDefineThisLevel.Where(x => x.TrackDirtinessNonSerialized))
            //    additionalDirtinessChecks += $" || ({property.BackingFieldAccessedString} && {property.PropertyName}_Dirty)";

            // For a class, we don't need to use the manual descendant dirtiness checks routinely, since DescendantIsDirty will automatically be sent.
            // But a struct's descendants (whether classes or structs) have no way of informing the struct that they are dirty. A struct cannot pass to the descendant a delegate so that the descendant can inform the struct that it is dirty. (When a struct passes a delegate, the delegate actually operates on a copy of the struct, not the original.) Thus, the only way to check for descendant dirtiness is to check each child Lazinator property.
            additionalDescendantDirtinessChecks = "";
            if (ObjectType == LazinatorObjectType.Struct || GeneratingRefStruct)
            {
                additionalDescendantDirtinessChecks = manualDescendantDirtinessChecks;
            }

            return additionalDescendantDirtinessChecks;
        }

        private static void GetSupportedConversions(CodeStringBuilder sb, List<PropertyDescription> propertiesSupportedCollections, List<PropertyDescription> propertiesSupportedTuples, List<PropertyDescription> propertiesNonSerialized)
        {
            if (propertiesSupportedCollections.Any() || propertiesSupportedTuples.Any())
                sb.Append($@"
                            /* Conversion of supported collections and tuples */
");

            HashSet<string> alreadyGenerated = new HashSet<string>(); // avoid duplicate additions
            foreach (var property in propertiesSupportedCollections)
                property.AppendSupportedCollectionConversionMethods(sb, alreadyGenerated);

            foreach (var property in propertiesSupportedTuples)
                property.AppendSupportedTupleConversionMethods(sb, alreadyGenerated);

            foreach (var property in propertiesNonSerialized)
                property.AppendInterchangeTypes(sb, alreadyGenerated);
        }

        private string GetFileHeader(string hash, string primaryNamespace, List<string> otherNamespaces)
        {
            if (GeneratingRefStruct)
                return $@"
                        ";
            otherNamespaces.AddRange(new string[] { "System", "System.Buffers", "System.Collections.Generic", "System.Diagnostics", "System.IO", "System.Linq", "System.Runtime.InteropServices", "Lazinator.Attributes", "Lazinator.Buffers", "Lazinator.Core", "Lazinator.Exceptions", "Lazinator.Support", "static Lazinator.Buffers.WriteUncompressedPrimitives" });
            if (AsyncLazinatorMemory)
                otherNamespaces.Add("System.Threading.Tasks");
            otherNamespaces.RemoveAll(x => x == primaryNamespace);
            otherNamespaces = otherNamespaces.Where(x => x != null && x != "").OrderBy(x => x).Distinct().ToList();
            CodeStringBuilder header = new CodeStringBuilder();
            header.Append($@"//{@hash}
                //------------------------------------------------------------------------------
                // <auto-generated>
                //     This code was generated by the Lazinator tool, version {LazinatorVersionInfo.LazinatorVersionString}{(SuppressDate ? "" : $", on {DateTime.Now}")}
                //
                //     Changes to this file may cause incorrect behavior and will be lost if
                //     the code is regenerated.
                // </auto-generated>
                //------------------------------------------------------------------------------
                {NullableModeSettingString}
                namespace { primaryNamespace }
                {{");
            foreach (string other in otherNamespaces)
                header.Append($@"
                        using {other};");
            header.Append($@"
                        using static Lazinator.Core.LazinatorUtilities;");
            return header.ToString();
        }

        private string GetDerivationKeywordForLengthProperty(PropertyDescription propertyDescription)
        {
            string derivationKeyword;
            if (ObjectType == LazinatorObjectType.Struct || GeneratingRefStruct)
                derivationKeyword = "";
            else if (propertyDescription.DerivationKeyword == "override ")
                derivationKeyword = "override ";
            else if (IsSealed)
                derivationKeyword = "";
            else
                derivationKeyword = "virtual ";
            return derivationKeyword;
        }

        public void HandleGenerics(INamedTypeSymbol iLazinatorType)
        {
            var genericArguments = iLazinatorType.TypeArguments;
            if (!iLazinatorType.IsAbstract
                && genericArguments.Any(x => IsNonlazinatorGeneric(x))
                )
            {
                if (AllowNonlazinatorGenerics)
                {
                    if (genericArguments.All(x => IsNonlazinatorGeneric(x)))
                        AllGenericsAreNonlazinator = true;
                }
                else
                    throw new LazinatorCodeGenException("Open generic parameter in non-abstract type must be constrained to type ILazinator. Add a clause like 'where T : ILazinator' to both the main class and the interface definition");
            }
            NameIncludingGenerics = iLazinatorType.GetMinimallyQualifiedName(NullableModeEnabled);
            GenericArgumentNames = genericArguments.Select(x => x.Name).ToList();
            FullyQualifiedObjectName = iLazinatorType.GetFullyQualifiedNameWithoutGlobal(NullableModeEnabled);
        }

        private bool IsNonlazinatorGeneric(ITypeSymbol typeSymbol)
        {
            return !(
                        ( // each generic argument must implement ILazinator or be constrained to ILazinator
                            typeSymbol.Interfaces.Any(y => y.Name == "ILazinator" || y.Name == "ILazinator?")
                            ||
                            (((typeSymbol as ITypeParameterSymbol)?.ConstraintTypes.Any(y => y.Name == "ILazinator" || y.Name == "ILazinator?") ?? false))
                        )
                    )
                    && // but if this is a Lazinator interface type, that's fine too.
                    !Compilation.ContainsAttributeOfType<CloneLazinatorAttribute>(typeSymbol);
        }
    }
}
