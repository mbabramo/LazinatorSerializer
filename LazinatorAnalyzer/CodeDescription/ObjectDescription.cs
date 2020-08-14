using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
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
        public bool NullableModeEnabled => NullableContextSetting.WarningsEnabled(); // TODO && NullableContextSetting.AnnotationsEnabled();
        public bool NullableModeInherited => NullableContextSetting.WarningsInherited(); // TODO annotations
        public bool AlwaysSpecifyNullableMode => true; // the WarningsInherited() setting seems not always to be correct
        public string NullableModeSettingString => NullableModeInherited && !AlwaysSpecifyNullableMode ? "" : (NullableModeEnabled ? $@"
            #nullable enable" : $@"
            #nullable disable"); 
        public string QuestionMarkIfNullableModeEnabled => NullableModeEnabled ? "?" : "";
        public string ILazinatorStringWithoutQuestionMark => "ILazinator";
        public string ILazinatorString => "ILazinator" + QuestionMarkIfNullableModeEnabled;
        public string ILazinatorStringNonNullableIfPropertyNonNullable(bool nonnullable) => nonnullable ? "ILazinator" : ILazinatorString;

        /* Derivation */
        public ObjectDescription BaseLazinatorObject { get; set; }
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
        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }
        public string SealedKeyword => IsSealed ? "sealed " : "";
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
        public bool CanNeverHaveChildren => Version == -1 && IsSealedOrStruct && !ExclusiveInterface.PropertiesIncludingInherited.Any(x => x.PropertyType != LazinatorPropertyType.PrimitiveType && x.PropertyType != LazinatorPropertyType.PrimitiveTypeNullable) && !IsGeneric;
        public bool UniqueIDCanBeSkipped => Version == -1 && IsSealedOrStruct && BaseLazinatorObject == null && !HasNonexclusiveInterfaces && !ContainsOpenGenericParameters;
        public bool SuppressDate { get; set; }
        public bool AllowNonlazinatorGenerics => InterfaceTypeSymbol.HasAttributeOfType<CloneAllowNonlazinatorOpenGenericsAttribute>();
        public bool SuppressLazinatorVersionByte => InterfaceTypeSymbol.HasAttributeOfType<CloneExcludeLazinatorVersionByteAttribute>();
        public bool GenerateRefStruct => InterfaceTypeSymbol.HasAttributeOfType<CloneGenerateRefStructAttribute>() && !GeneratingRefStruct;
        public bool GeneratingRefStruct = false;
        public bool IncludeTracingCode => Config?.IncludeTracingCode ?? false;
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
            FullyQualifiedObjectName = iLazinatorTypeSymbol.GetFullyQualifiedNameWithoutGlobal();
            NameIncludingGenerics = iLazinatorTypeSymbol.Name; // possibly updated later
            SimpleName = iLazinatorTypeSymbol.Name;
            if (iLazinatorTypeSymbol.TypeKind == TypeKind.Class)
            {
                ObjectType = LazinatorObjectType.Class;
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
                AppendSerializeExistingBuffer(sb);
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
                    {IIF(GeneratingRefStruct, $@"public ref partial struct { NameIncludingGenerics_RefStruct }")}{IIF(!GeneratingRefStruct, $@"{AccessibilityConverter.Convert(Accessibility)} { SealedKeyword }partial { (ObjectType == LazinatorObjectType.Class ? "class" : "struct") } { NameIncludingGenerics } : {IIF(IsDerivedFromNonAbstractLazinator, () => BaseObjectName + ", ")}{ILazinatorStringWithoutQuestionMark}")}
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
                    partialsuperclasses = partialsuperclasses + $@"{AccessibilityConverter.Convert(Accessibility)} {IIF(supertype.IsSealed, "sealed ")}partial class {supertype.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}
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
                    var clone = new {NameIncludingGenerics_RefStruct}()
                    {{
                        OriginalIncludeChildrenMode = OriginalIncludeChildrenMode,
                        LazinatorMemoryStorage = LazinatorMemoryStorage
                    }};
                    clone.Deserialize();
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
                    var clone = new {NameIncludingGenerics}({IIF(!IsStruct, "LazinatorConstructorEnum.LazinatorConstructor")})
                    {{
                        OriginalIncludeChildrenMode = OriginalIncludeChildrenMode,
                        LazinatorMemoryStorage = LazinatorMemoryStorage
                    }};
                    clone.Deserialize();
                    return clone;
                }}
            ");
        }


        private void AppendGeneralDefinitions(CodeStringBuilder sb)
        {
            string additionalDescendantDirtinessChecks = GetDescendantDirtinessChecks(false);
            string additionalDescendantHasChangedChecks = GetDescendantDirtinessChecks(true);
            string classContainingStructContainingClassError = GetClassContainingStructContainingClassError();
            string constructor = GetConstructors();
            string cloneMethod = GetCloneMethod();

            if (!IsDerivedFromNonAbstractLazinator)
            {
                string boilerplate;
                if (IsAbstract && BaseLazinatorObject?.IsAbstract == true) // abstract class inheriting from abstract class
                    boilerplate = $@""; // everything is inherited from parent abstract class
                else if (IsAbstract && !GeneratingRefStruct)
                    boilerplate = $@"        /* Abstract declarations */
			            public abstract LazinatorParentsCollection LazinatorParents {{ get; set; }}
                    
                        public abstract int Deserialize();
                        
                        public abstract LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
                        
                        public abstract {ILazinatorString} CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers);

                        {IIF(!ImplementsAssignCloneProperties, $@"public abstract {ILazinatorString} AssignCloneProperties({ILazinatorStringWithoutQuestionMark} clone, IncludeChildrenMode includeChildrenMode);

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
		                
                        public abstract void DeserializeLazinator(LazinatorMemory serializedBytes);

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

                        {HideILazinatorProperty}{ProtectedIfApplicable}abstract ReadOnlyMemory<byte> LazinatorObjectBytes
                        {{
			                get;
                        }}

                        public abstract void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren);
                        public abstract void UpdateStoredBuffer();
                        public abstract void FreeInMemoryObjects();
                        public abstract int GetByteLength();
        
                ";
                else
                {
                    string readUniqueID;
                    if (!ContainsOpenGenericParameters && IsSealedOrStruct)
                        readUniqueID = $@"{(UniqueIDCanBeSkipped ? "" : $@"int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
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

                        {constructor}{HideILazinatorProperty}public {DerivationKeyword}LazinatorParentsCollection LazinatorParents {{ get; set; }}

                        {HideILazinatorProperty}public {DerivationKeyword}IncludeChildrenMode OriginalIncludeChildrenMode {{ get; set; }}

                        public {DerivationKeyword}int Deserialize()
                        {{
                            FreeInMemoryObjects();
                            int bytesSoFar = 0;
                            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
                            if (span.Length == 0)
                            {{
                                return 0;
                            }}{classContainingStructContainingClassError}

                            {readUniqueID}{(SuppressLazinatorVersionByte ? "" : $@"int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
                            
                        ")}int serializedVersionNumber = {(Version == -1 ? "-1; /* versioning disabled */" : $@"span.ToDecompressedInt(ref bytesSoFar);")}

                            OriginalIncludeChildrenMode = {(CanNeverHaveChildren ? "IncludeChildrenMode.IncludeAllChildren; /* cannot have children */" : $@"(IncludeChildrenMode)span.ToByte(ref bytesSoFar);")}

                            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);{
                            IIF(ImplementsLazinatorObjectVersionUpgrade && Version != -1,
                                $@"
                            if (serializedVersionNumber < LazinatorObjectVersion)
                            {{
                                LazinatorObjectVersionUpgrade(serializedVersionNumber);
                            }}")
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
                            writer.Write(LazinatorMemoryStorage.Span);
                            return writer.LazinatorMemory;
                        }}

                        {ProtectedIfApplicable}{DerivationKeyword}LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
                        {{
                            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
                            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
                            SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                            return writer.LazinatorMemory;
                        }}

                        {cloneMethod}

                        {HideILazinatorProperty}public {DerivationKeyword}bool HasChanged {{ get; set; }}

                        {HideBackingField}{ProtectedIfApplicable}bool _IsDirty;
                        {HideILazinatorProperty}public {DerivationKeyword}bool IsDirty
                        {{
                            [DebuggerStepThrough]
                            get => _IsDirty{IIF(!(ObjectType == LazinatorObjectType.Struct || GeneratingRefStruct), "|| LazinatorObjectBytes.Length == 0")};
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
        
                        public {DerivationKeyword}void DeserializeLazinator(LazinatorMemory serializedBytes)
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
                        {HideILazinatorProperty}{ProtectedIfApplicable}{DerivationKeyword}ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage.IsEmpty ? LazinatorMemory.EmptyReadOnlyMemory : LazinatorMemoryStorage.Memory;

                        public {DerivationKeyword}void UpdateStoredBuffer()
                        {{
                            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
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
                                writer.Write(LazinatorMemoryStorage.Span);
                                LazinatorMemoryStorage = writer.LazinatorMemory;
                            }}
                            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
                            if (!LazinatorParents.Any())
                            {{
                                previousBuffer.Dispose();
                            }}
                        }}

                        public {DerivationKeyword}int GetByteLength()
                        {{
                            UpdateStoredBuffer();
                            return LazinatorObjectBytes.Length;
                        }}

                        public {DerivationKeyword}bool NonBinaryHash32 => {(NonbinaryHash ? "true" : "false")};
        
                ";
                }


                sb.Append(boilerplate);
            }
            else if (!IsAbstract || GeneratingRefStruct)
            {
                sb.Append($@"        /* Clone overrides */

                        {constructor}{cloneMethod}

                        /* Properties */
");
            }
        }

        private string GetCloneMethod()
        {
            if (GeneratingRefStruct)
                return "";
            return $@"public {DerivationKeyword}{ILazinatorString} CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
                        {{
                            var clone = new {NameIncludingGenerics}({IIF(!IsStruct, "LazinatorConstructorEnum.LazinatorConstructor")})
                            {{
                                OriginalIncludeChildrenMode = includeChildrenMode
                            }};
                            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);{IIF(ImplementsOnClone, $@"
            clone.OnCompleteClone(this);")}
                            return clone;
                        }}{IIF(!ImplementsAssignCloneProperties, $@"

                        public {DerivationKeyword}{ILazinatorString} AssignCloneProperties({ILazinatorStringWithoutQuestionMark} clone, IncludeChildrenMode includeChildrenMode)
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
                    sb.AppendLine($"        {ProtectedIfApplicable}int _{withRecordedIndices[i].PropertyName}_ByteIndex;");
            for (int i = 0; i < withRecordedIndices.Count() - 1; i++)
            {
                PropertyDescription propertyDescription = withRecordedIndices[i];
                string derivationKeyword = GetDerivationKeywordForLengthProperty(propertyDescription);
                sb.AppendLine(
                        $"{ProtectedIfApplicable}{derivationKeyword}int _{propertyDescription.PropertyName}_ByteLength => _{withRecordedIndices[i + 1].PropertyName}_ByteIndex - _{propertyDescription.PropertyName}_ByteIndex;");
            }
            if (lastPropertyToIndex != null)
            {
                if (IsAbstract)
                {
                    sb.AppendLine(
                            $"{ProtectedIfApplicable}virtual int _{lastPropertyToIndex.PropertyName}_ByteLength {{ get; }}"); // defined as virtual so that it's not mandatory to override, since it won't be used if an open generic is redefined.
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
                            $"{ProtectedIfApplicable}{derivationKeyword}int _{lastPropertyToIndex.PropertyName}_ByteLength => _{ObjectNameEncodable}_EndByteIndex - _{lastPropertyToIndex.PropertyName}_ByteIndex;");
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
                        {(ImplementsConvertFromBytesAfterHeader ? skipConvertFromBytesAfterHeaderString : $@"public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);")}
                        public abstract void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
                        {ProtectedIfApplicable}abstract LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
                        {ProtectedIfApplicable}abstract void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition);
                        {(ImplementsWritePropertiesIntoBuffer ? skipWritePropertiesIntoBufferString : $@"{ProtectedIfApplicable}abstract void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID);")}
");
        }

        private void AppendResetProperties(CodeStringBuilder sb)
        {
            string resetAccessed = "", resetStorage = "";
            foreach (var property in PropertiesToDefineThisLevel.Where(x => !x.IsPrimitive && x.PlaceholderMemoryWriteMethod == null)) // Note that we will free in memory objects even for non-nullables. We can do this because we have nullable backing properties for them. 
            {
                if (!property.NonNullableThatRequiresInitialization)
                    resetStorage += $@"_{property.PropertyName} = default;
                        ";
                resetAccessed += $"_{property.PropertyName}_Accessed = ";
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
                        string ifThenStatement = property.GetNullCheckPlusPrecedingConditionIfThen(
                                ConditionsCodeGenerator.AndCombine(
                                    "enumerateNulls", 
                                    ConditionsCodeGenerator.OrCombine(
                                        "!exploreOnlyDeserializedChildren", 
                                        $"_{propertyName}_Accessed"))
                                , propertyName, 
                                // CONSEQUENT
                                $@"yield return (""{propertyName}"", default);", 
                                // ELSE CONSEQUENT -- another conditional
                                new ConditionalCodeGenerator(
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
                                        foreach (var toYield in {propertyName}{property.NullForgiveness}.{IIF(property.PropertyType == LazinatorPropertyType.LazinatorStructNullable, "Value.")}EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                                        {{
                                            yield return (""{propertyName}"" + ""."" + toYield.propertyName, toYield.descendant);
                                        }}
                                    }}").ToString());
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
                sb.Append(new ConditionalCodeGenerator(getAntecedent(property),
                        $@"{IIF(nonNullCheckDefinitelyTrue(property), $@"var deserialized_{propertyName} = {propertyName};
                            ")}_{propertyName} = ({property.AppropriatelyQualifiedTypeName}) _{propertyName}{property.NullForgiveness}{IIF(property.PropertyType == LazinatorPropertyType.LazinatorStructNullable, ".Value")}.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);").ToString());
            }
            foreach (var property in PropertiesToDefineThisLevel.Where(x => x.IsSupportedCollectionOrTupleOrNonLazinatorWithInterchangeType && !x.IsMemoryOrSpan))
            {
                string propertyName = property.PropertyName;

                sb.Append(new ConditionalCodeGenerator(getAntecedent(property),
                        $@"{IIF(nonNullCheckDefinitelyTrue(property), $@"var deserialized_{propertyName} = {propertyName};
                            ")}_{propertyName} = ({property.AppropriatelyQualifiedTypeName}) CloneOrChange_{property.AppropriatelyQualifiedTypeNameEncodable}({property.BackingFieldAccessWithPossibleException}, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true){property.InnerProperties?[0].PossibleUnsetException}, true);").ToString());
            }
            foreach (var property in PropertiesToDefineThisLevel.Where(x => x.IsNonLazinatorTypeWithoutInterchange && x.PlaceholderMemoryWriteMethod == null))
            {
                string propertyName = property.PropertyName;
                sb.Append(new ConditionalCodeGenerator(getAntecedent(property),
                        $@"{IIF(nonNullCheckDefinitelyTrue(property), $@"var deserialized_{propertyName} = {propertyName};
                            ")}_{propertyName} = {property.DirectConverterTypeNamePrefix}CloneOrChange_{property.AppropriatelyQualifiedTypeNameEncodable}(_{propertyName}, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);").ToString());
            }

            foreach (var property in PropertiesToDefineThisLevel.Where(x => ((!x.IsPrimitive && !x.IsLazinator && !x.IsSupportedCollectionOrTupleOrNonLazinatorWithInterchangeType && !x.IsNonLazinatorTypeWithoutInterchange) || x.IsMemoryOrSpan) && x.PlaceholderMemoryWriteMethod == null))
            {
                // we want to deserialize the memory. In case of ReadOnlySpan<byte>, we also want to duplicate the memory if it hasn't been set by the user, since we want to make sure that the property will work even if the buffer is removed (which might be the reason for the ForEachLazinator call)
                sb.Append($@"if (!exploreOnlyDeserializedChildren)
                    {{
                        var deserialized_{property.PropertyName} = {property.PropertyName};{IIF(property.SupportedCollectionType == LazinatorSupportedCollectionType.ReadOnlySpan, $@"
                        if (!_{property.PropertyName}_Accessed)
                        {{
                            {property.PropertyName} = deserialized_{property.PropertyName};
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

        private void AppendSerializeExistingBuffer(CodeStringBuilder sb)
        {

            if (IsDerivedFromNonAbstractLazinator)
                sb.AppendLine(
                        $@"public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                        {{");
            else
                sb.AppendLine(
                        $@"public {DerivationKeyword}void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
            foreach (var property in PropertiesIncludingInherited.Where(x => x.PropertyType == LazinatorPropertyType.LazinatorStruct || x.PropertyType == LazinatorPropertyType.LazinatorStructNullable))
            {
                reset +=
                    $@"
                    _{property.PropertyName}_Accessed = false;"; // force deserialization to make the struct clean
            }
            foreach (var property in PropertiesIncludingInherited
                .Where(x => x.PropertyType == LazinatorPropertyType.OpenGenericParameter))
            {
                if (!property.GenericConstrainedToStruct)
                    reset +=
                        $@"
                        if (_{property.PropertyName}_Accessed && _{property.PropertyName} != null && _{property.PropertyName}.IsStruct && (_{property.PropertyName}.IsDirty || _{property.PropertyName}.DescendantIsDirty))
                        {{
                            _{property.PropertyName}_Accessed = false;
                        }}";
                else
                    reset +=
                        $@"
                        if (_{property.PropertyName}_Accessed && _{property.PropertyName}.IsStruct && (_{property.PropertyName}.IsDirty || _{property.PropertyName}.DescendantIsDirty))
                        {{
                            _{property.PropertyName}_Accessed = false;
                        }}";
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
$@"{property.PropertyName}{property.NullForgiveness}{IIF(property.PropertyType == LazinatorPropertyType.LazinatorStructNullable, ".Value")}.UpdateStoredBuffer(ref writer, startPosition + _{property.PropertyName}_ByteIndex{property.IncrementChildStartBySizeOfLength}, _{property.PropertyName}_ByteLength{property.DecrementTotalLengthBySizeOfLength}, IncludeChildrenMode.IncludeAllChildren, true);").ToString());
            }
            foreach (var property in PropertiesToDefineThisLevel.Where(x => x.IsSupportedCollectionOrTupleOrNonLazinatorWithInterchangeType && x.PlaceholderMemoryWriteMethod == null))
            {
                if (property.IsMemoryOrSpan)
                {
                }
                else
                {
                    string propertyName = property.PropertyName;
                    sb.Append(new ConditionalCodeGenerator(property.GetNonNullCheck(true),
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
            string positionInitialization = !hasChildrenThisLevel ? $@"" : $@"
                    int startPosition = writer.Position;
                    int startOfObjectPosition = 0;";

            if (IsDerivedFromNonAbstractLazinator)
                sb.AppendLine(
                        $@"
                        {ProtectedIfApplicable}override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                        {{{positionInitialization}
                            base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);");
            else
            {
                sb.AppendLine(
                        $@"
                        {ProtectedIfApplicable}{DerivationKeyword}void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                        {{{positionInitialization}
                            // header information");


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
            }

            sb.AppendLine("// write properties");

            foreach (var property in thisLevel)
            {
                if (IncludeTracingCode)
                {
                    if (property.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || (property.PropertyType == LazinatorPropertyType.LazinatorStructNullable) || (property.PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface) )
                    {
                        sb.AppendLine($@"TabbedText.WriteLine($""Byte {{writer.Position}}, {property.PropertyName} (accessed? {IIF(property.BackingAccessFieldIncluded, $"{{_{property.PropertyName}_Accessed}}")}) (backing var null? {{_{property.PropertyName} == null}}) "");");
                    }
                    else if (property.PropertyType == LazinatorPropertyType.LazinatorStruct)
                    {
                        sb.AppendLine($@"TabbedText.WriteLine($""Byte {{writer.Position}}, {property.PropertyName} (accessed? {IIF(property.BackingAccessFieldIncluded, $"{{_{property.PropertyName}_Accessed}}")}) "");");
                    }
                    else if (property.TrackDirtinessNonSerialized)
                        sb.AppendLine($@"TabbedText.WriteLine($""Byte {{writer.Position}}, {property.PropertyName} (accessed? {IIF(property.BackingAccessFieldIncluded, $"{{_{property.PropertyName}_Accessed}}")}) (dirty? {{_{property.PropertyName}_Dirty}})"");");
                    else if (property.PropertyType == LazinatorPropertyType.NonLazinator || property.PropertyType == LazinatorPropertyType.SupportedCollection || property.PropertyType == LazinatorPropertyType.SupportedTuple)
                        sb.AppendLine($@"TabbedText.WriteLine($""Byte {{writer.Position}}, {property.PropertyName} (accessed? {IIF(property.BackingAccessFieldIncluded, $"{{_{property.PropertyName}_Accessed}}")})"");");
                    else
                        sb.AppendLine($@"TabbedText.WriteLine($""Byte {{writer.Position}}, {property.PropertyName} value {{_{property.PropertyName}}}"");");
                    sb.AppendLine($@"TabbedText.Tabs++;");
                }
                property.AppendPropertyWriteString(sb);
                if (IncludeTracingCode)
                {
                    sb.AppendLine($@"TabbedText.Tabs--;");
                }
            }
            AppendEndByteIndex(sb, thisLevel, "writer.Position - startPosition", true);
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

            sb.Append($@"public {DerivationKeyword}void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
                {{
                    {(!IsDerivedFromNonAbstractLazinator ? "" : $@"base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
                    ")}ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
                        ");

            foreach (var property in thisLevel)
            {
                property.AppendPropertyReadString(sb);
            }
            AppendEndByteIndex(sb, thisLevel, "bytesSoFar", false);

            sb.Append($@"        }}

        ");
        }

        private void AppendEndByteIndex(CodeStringBuilder sb, List<PropertyDescription> thisLevel, string endByteString, bool addConditionalForUpdateStoredBuffer)
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
            // We have a special constructor with the LazinatorConstructorEnum for all classes. Our factory will call this constructor, so as not to trigger the default constructor. 
            string constructors = IsStruct ? "" : $@"public {SimpleName}(LazinatorConstructorEnum constructorEnum){IIF(ILazinatorTypeSymbol.BaseType != null && !ILazinatorTypeSymbol.BaseType.IsAbstract && IsDerivedFromNonAbstractLazinator, " : base(constructorEnum)")}
                        {{
                        }}

                        public {SimpleName}(LazinatorMemory serializedBytes, ILazinator{IIF(NullableModeEnabled, "?")} parent = null){IIF(ILazinatorTypeSymbol.BaseType != null && !ILazinatorTypeSymbol.BaseType.IsAbstract && IsDerivedFromNonAbstractLazinator, " : base(LazinatorConstructorEnum.LazinatorConstructor)")}
                        {{
                            LazinatorParents = new LazinatorParentsCollection(parent);
                            DeserializeLazinator(serializedBytes);
                            HasChanged = false;
                            DescendantHasChanged = false;
                        }}

                        ";
            //if (Compilation.ImplementingTypeRequiresParameterlessConstructor) constructor +=
            //        $@"public {SimpleName}(){IIF(ILazinatorTypeSymbol.BaseType != null, " : base()")}
            //            {{
            //            }}
                        
            //            ";
            return constructors;
        }

        public static bool AllowClassContainingStructContainingClass = true; // for now, let's allow this scenario
        private string GetClassContainingStructContainingClassError()
        {
            if (AllowClassContainingStructContainingClass)
                return "";
            string classContainingStructContainingClassError = "";
            if (ObjectType == LazinatorObjectType.Struct || GeneratingRefStruct)
            {
                if (PropertiesToDefineThisLevel.Any(x => x.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || x.PropertyType == LazinatorPropertyType.LazinatorNonnullableClassOrInterface))
                    classContainingStructContainingClassError = $@"

                        if (LazinatorParents.Any())
                        {{
                            throw new LazinatorDeserializationException(""A Lazinator struct may include a Lazinator class or interface as a property only when the Lazinator struct has no parent class."");
                        }}"; //  Otherwise, when a child is deserialized, the struct's parent will not automatically be affected, because the deserialization will take place in a copy of the struct. Though it is possible to handle this scenario by setting the deserialized property immediately after mutating it, the risk of error is great, and so we do not allow it.
            }

            return classContainingStructContainingClassError;
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
                    manualDescendantDirtinessChecks += $" || (_{property.PropertyName}_Accessed && ({property.PropertyName}{property.NullableStructValueAccessor}.{tense} || {property.PropertyName}{property.NullableStructValueAccessor}.Descendant{tense}))";
                else
                {
                    string nonNullCheck = property.GetNonNullCheck(true);
                    manualDescendantDirtinessChecks += $" || ({nonNullCheck} && ({property.PropertyName}.{tense} || {property.PropertyName}.Descendant{tense}))";
                }
            }
            // The following is not necessary, because manual _Dirty properties automatically lead to _IsDirty being set to true. Because non-Lazinators are not considered "children," nothing needs to happen to DescendantIsDirty; this also means that when encoding, non-Lazinators are encoded if dirty regardless of the include child setting.
            //foreach (var property in PropertiesToDefineThisLevel.Where(x => x.TrackDirtinessNonSerialized))
            //    additionalDirtinessChecks += $" || (_{property.PropertyName}_Accessed && {property.PropertyName}_Dirty)";

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
            otherNamespaces.AddRange(new string[] { "System", "System.Buffers", "System.Collections.Generic", "System.Diagnostics", "System.IO", "System.Linq", "System.Runtime.InteropServices", "Lazinator.Attributes", "Lazinator.Buffers", "Lazinator.Core", "Lazinator.Exceptions", "Lazinator.Support" });
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
            NameIncludingGenerics = iLazinatorType.GetMinimallyQualifiedName();
            GenericArgumentNames = genericArguments.Select(x => x.Name).ToList();
            FullyQualifiedObjectName = iLazinatorType.GetFullyQualifiedNameWithoutGlobal();
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
