using System;
using System.Collections.Generic;
using System.Linq;
using LazinatorCodeGen.AttributeClones;
using LazinatorCodeGen.Roslyn;
using LazinatorCodeGen.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Lazinator.CodeDescription
{
    public class ObjectDescription
    {
        /* Main properties */
        public INamedTypeSymbol ILazinatorTypeSymbol { get; set; }
        public INamedTypeSymbol InterfaceTypeSymbol { get; set; }
        public LazinatorObjectType ObjectType { get; set; }
        public LazinatorCompilation Compilation { get; set; }
        public Guid Hash { get; set; }

        /* Derivation */
        public ObjectDescription BaseLazinatorObject { get; set; }
        public bool IsDerivedFromNonAbstractLazinator => BaseLazinatorObject != null &&
                        (BaseLazinatorObject.IsDerivedFromNonAbstractLazinator ||
                        !BaseLazinatorObject.IsAbstract);
        public bool IsDerivedFromAbstractLazinator => BaseLazinatorObject != null &&
                        (BaseLazinatorObject.IsDerivedFromAbstractLazinator || BaseLazinatorObject.IsAbstract);
        public string DerivationKeyword => (IsDerivedFromNonAbstractLazinator || IsDerivedFromAbstractLazinator) ? "override " : (IsSealedOrStruct ? "" : "virtual ");
        public string BaseObjectName => BaseLazinatorObject.Namespace == Namespace ? BaseLazinatorObject.NameIncludingGenerics : BaseLazinatorObject.FullyQualifiedObjectName;
        public string ProtectedIfApplicable => (ObjectType == LazinatorObjectType.Struct || IsSealed) ? "" : "protected ";

        /* Names */
        public string Namespace { get; set; }
        public string NameIncludingGenerics { get; set; }
        public string SimpleName { get; set; }
        public string FullyQualifiedObjectName { get; set; }
        public string ObjectNameEncodable => RoslynHelpers.EncodableTypeName(ILazinatorTypeSymbol);

        /* General aspects */
        public Accessibility Accessibility { get; set; }
        public bool IsNonLazinatorBaseClass { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }
        public string SealedKeyword => IsSealed ? "sealed " : "";
        public bool IsSealedOrStruct => IsSealed || ObjectType != LazinatorObjectType.Class;

        /* Interfaces */
        public ExclusiveInterfaceDescription ExclusiveInterface { get; set; }
        public int Version => ExclusiveInterface.Version;
        public int UniqueID => (int)ExclusiveInterface.UniqueID;
        public List<NonexclusiveInterfaceDescription> NonexclusiveInterfaces { get; set; }
        public bool HasNonexclusiveInterfaces => NonexclusiveInterfaces != null && NonexclusiveInterfaces.Any();
        public int TotalNumProperties => ExclusiveInterface.TotalNumProperties;

        /* Implementations */
        public bool ImplementsLazinatorObjectVersionUpgrade { get; set; }
        public bool ImplementsPreSerialization { get; set; }
        public bool ImplementsPostDeserialization { get; set; }
        public bool ImplementsOnDirty { get; set; }
        public bool ImplementsMarkHierarchyClean { get; set; }
        public bool ImplementsConvertFromBytesAfterHeader { get; set; }
        public bool ImplementsWritePropertiesIntoBuffer { get; set; }

        /* Complications */
        public List<string> GenericArgumentNames { get; set; }
        public string GenericArgumentNameTypes => String.Join(", ", GenericArgumentNames.Select(x => "typeof(" + x + ")"));
        public bool IsGeneric => GenericArgumentNames != null && GenericArgumentNames.Any();
        public List<PropertyDescription> PropertiesToDefineThisLevel => ExclusiveInterface.PropertiesToDefineThisLevel;
        public bool CanNeverHaveChildren => Version == -1 && IsSealedOrStruct && !ExclusiveInterface.PropertiesIncludingInherited.Any(x => x.PropertyType != LazinatorPropertyType.PrimitiveType && x.PropertyType != LazinatorPropertyType.PrimitiveTypeNullable) && !IsGeneric;
        public bool UniqueIDCanBeSkipped => Version == -1 && IsSealedOrStruct && BaseLazinatorObject == null && !HasNonexclusiveInterfaces && !IsGeneric;
        public bool SuppressDate { get; set; }
        public bool SuppressLazinatorVersionByte => InterfaceTypeSymbol.HasAttributeOfType<CloneExcludeLazinatorVersionByteAttribute>();
        public bool IncludeTracingCode => Compilation.Config?.IncludeTracingCode ?? false;

        public ObjectDescription()
        {

        }

        public ObjectDescription(INamedTypeSymbol iLazinatorTypeSymbol, LazinatorCompilation compilation, bool suppressDate = false)
        {
            ILazinatorTypeSymbol = iLazinatorTypeSymbol;
            Compilation = compilation;
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
                    BaseLazinatorObject = new ObjectDescription(baseILazinatorType, compilation);
                    if (BaseLazinatorObject.IsNonLazinatorBaseClass)
                        BaseLazinatorObject = null; // ignore base non-lazinator
                }
            }

            if (!Compilation.TypeToExclusiveInterface.ContainsKey(iLazinatorTypeSymbol.OriginalDefinition))
            {
                // This is a nonlazinator base class
                IsNonLazinatorBaseClass = true;
                return;
            }
            INamedTypeSymbol interfaceTypeSymbol = Compilation.TypeToExclusiveInterface[iLazinatorTypeSymbol.OriginalDefinition];
            InterfaceTypeSymbol = interfaceTypeSymbol;
            Hash = Compilation.InterfaceTextHash.ContainsKey(interfaceTypeSymbol) ? Compilation.InterfaceTextHash[interfaceTypeSymbol] : default;
            ExclusiveInterface = new ExclusiveInterfaceDescription(interfaceTypeSymbol, this);
            if (ExclusiveInterface.GenericArgumentNames.Any())
                HandleGenerics(iLazinatorTypeSymbol);
            var nonexclusiveInterfaces = iLazinatorTypeSymbol.AllInterfaces
                                .Where(x => Compilation.ContainsAttributeOfType<CloneNonexclusiveLazinatorAttribute>(x));
            NonexclusiveInterfaces = nonexclusiveInterfaces
                .Select(x => new NonexclusiveInterfaceDescription(Compilation, x, this)).ToList();

            ImplementsLazinatorObjectVersionUpgrade = Compilation.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "LazinatorObjectVersionUpgrade"));
            ImplementsPreSerialization = Compilation.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "PreSerialization"));
            ImplementsPostDeserialization = Compilation.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "PostDeserialization"));
            ImplementsOnDirty = Compilation.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "OnDirty"));
            ImplementsMarkHierarchyClean = Compilation.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "MarkHierarchyClean"));
            ImplementsConvertFromBytesAfterHeader = Compilation.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "ConvertFromBytesAfterHeader"));
            ImplementsWritePropertiesIntoBuffer = Compilation.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "WritePropertiesIntoBuffer"));
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
            AppendGeneralDefinitions(sb);
            AppendPropertyDefinitions(sb);
            AppendConversions(sb);
            AppendCloseClassSupertypesAndNamespace(sb, supertypes);
        }

        private void AppendConversions(CodeStringBuilder sb)
        {
            if (IsAbstract)
            {
                if (!IsDerivedFromAbstractLazinator && !IsDerivedFromNonAbstractLazinator)
                    AppendAbstractConversions(sb);
                // otherwise, lower level has already defined these methods, so nothing more to do
            }
            else
            {
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
                    {AccessibilityConverter.Convert(Accessibility)} { SealedKeyword }partial { (ObjectType == LazinatorObjectType.Class ? "class" : "struct") } { NameIncludingGenerics } : {(IsDerivedFromNonAbstractLazinator ? BaseObjectName + ", " : "")}ILazinator
                    {{";
            sb.AppendLine(theBeginning);
        }

        private void AppendSupertypesInformation(out string partialsuperclasses, out IEnumerable<ITypeSymbol> supertypes)
        {
            // we may have nested lazinator classes, in which class we need to nest the partial class definitions. We assume that the 
            partialsuperclasses = "";
            supertypes = null;
            if (ILazinatorTypeSymbol.ContainingType != null)
            {
                supertypes = ILazinatorTypeSymbol.GetContainingTypes();
                foreach (var supertype in supertypes)
                {
                    partialsuperclasses = partialsuperclasses + $@"{AccessibilityConverter.Convert(Accessibility)} {(supertype.IsSealed ? "sealed " : "")}partial class {supertype.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}
                        {{
                        ";
                }
            }
        }

        private void AppendGeneralDefinitions(CodeStringBuilder sb)
        {
            string additionalDescendantDirtinessChecks, postEncodingDirtinessCheck;
            GetDescendantDirtinessChecks(out additionalDescendantDirtinessChecks, out postEncodingDirtinessCheck);
            string classContainingStructContainingClassError = GetClassContainingStructContainingClassError();
            string constructor = GetConstructor();
            string markHierarchyCleanMethod = GetMarkHierarchyCleanMethod();

            if (!IsDerivedFromNonAbstractLazinator)
            {
                string boilerplate;
                if (IsAbstract && BaseLazinatorObject?.IsAbstract == true) // abstract class inheriting from abstract class
                    boilerplate = $@""; // everything is inherited from parent abstract class
                else if (IsAbstract)
                    boilerplate = $@"        /* Abstract declarations */
			            public abstract ILazinator LazinatorParentClass {{ get; set; }}
                    
                        public abstract int Deserialize();
                        
                        public abstract MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
                        
                        protected abstract MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
                        
                        public abstract ILazinator CloneLazinator();
                        
                        public abstract ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode);
                        
                        public abstract bool IsDirty
                        {{
			                get;
			                set;
                        }}
                        
                        public abstract InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate {{ get; set; }}
                        public abstract void InformParentOfDirtiness();
                        
                        public abstract bool DescendantIsDirty
                        {{
			                get;
			                set;
                        }}

                        {(ImplementsMarkHierarchyClean ? skipMarkHierarchyClean : "public abstract void MarkHierarchyClean();")}
		                
                        public abstract MemoryInBuffer HierarchyBytes
                        {{
			                set;
                        }}
                        
                        public abstract ReadOnlyMemory<byte> LazinatorObjectBytes
                        {{
			                get;
			                set;
                        }}

                        public abstract void LazinatorConvertToBytes();
                        public abstract int GetByteLength();
                        public abstract uint GetBinaryHashCode32();
                        public abstract ulong GetBinaryHashCode64();
                        public abstract Guid GetBinaryHashCode128();

                        /* Field definitions */
        
                ";
                else
                {
                    string readUniqueID;
                    if (!IsGeneric && IsSealedOrStruct)
                        readUniqueID = $@"{(UniqueIDCanBeSkipped ? "" : $@"int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
                            if (uniqueID != LazinatorUniqueID)
                            {{
                                throw new FormatException(""Wrong self-serialized type initialized."");
                            }}

                            ")}";
                    else
                        readUniqueID = $@"LazinatorGenericID = GetGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);

                                    ";

                    boilerplate = $@"        /* Serialization, deserialization, and object relationships */

                        {constructor}public {DerivationKeyword}ILazinator LazinatorParentClass {{ get; set; }}

                        {ProtectedIfApplicable}IncludeChildrenMode OriginalIncludeChildrenMode;

                        public {DerivationKeyword}int Deserialize()
                        {{
                            ResetAccessedProperties();
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
                            (ImplementsLazinatorObjectVersionUpgrade && Version != -1
                                ? $@"
                            if (serializedVersionNumber < LazinatorObjectVersion)
                            {{
                                LazinatorObjectVersionUpgrade(serializedVersionNumber);
                            }}"
                                : "")
                        }{
                            (ImplementsPostDeserialization
                                ? $@"
                            PostDeserialization();"
                                : "")
                        }
                            return bytesSoFar;
                        }}

                        public {DerivationKeyword}MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
                        {{
                            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
                        }}

                        {ProtectedIfApplicable}{DerivationKeyword}MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);

                        public {DerivationKeyword}ILazinator CloneLazinator()
                        {{
                            return CloneLazinator(OriginalIncludeChildrenMode);
                        }}

                        public {DerivationKeyword}ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
                        {{
                            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
                            var clone = new {NameIncludingGenerics}()
                            {{
                                LazinatorParentClass = LazinatorParentClass,
                                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                                OriginalIncludeChildrenMode = includeChildrenMode,
                                HierarchyBytes = bytes
                            }};
                            return clone;
                        }}

                        {ProtectedIfApplicable}bool _IsDirty;
                        public {DerivationKeyword}bool IsDirty
                        {{
                            [DebuggerStepThrough]
                            get => _IsDirty;
                            [DebuggerStepThrough]
                            set
                            {{
                                if (_IsDirty != value)
                                {{
                                    _IsDirty = value;
                                    if (_IsDirty)
                                    {{
                                        InformParentOfDirtiness();{(ImplementsOnDirty ? $@"
                                        OnDirty();" : "")}
                                    }}
                                }}
                            }}
                        }}

                        public {DerivationKeyword}InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate {{ get; set; }}
                        public {DerivationKeyword}void InformParentOfDirtiness()
                        {{
                            if (InformParentOfDirtinessDelegate == null)
                            {{
                                if (LazinatorParentClass != null)
                                {{
                                    LazinatorParentClass.DescendantIsDirty = true;
                                }}
                            }}
                            else
                            {{
                                InformParentOfDirtinessDelegate();
                            }}
                        }}

                        {ProtectedIfApplicable}bool _DescendantIsDirty;
                        public {DerivationKeyword}bool DescendantIsDirty
                        {{
                            [DebuggerStepThrough]
                            get => _DescendantIsDirty{additionalDescendantDirtinessChecks};
                            [DebuggerStepThrough]
                            set
                            {{
                                if (_DescendantIsDirty != value)
                                {{
                                    _DescendantIsDirty = value;
                                    if (_DescendantIsDirty && LazinatorParentClass != null)
                                    {{
                                        LazinatorParentClass.DescendantIsDirty = true;
                                    }}
                                }}
                            }}
                        }}

                        {markHierarchyCleanMethod}
        
                        private MemoryInBuffer _HierarchyBytes;
                        public {DerivationKeyword}MemoryInBuffer HierarchyBytes
                        {{
                            set
                            {{
                                _HierarchyBytes = value;
                                LazinatorObjectBytes = value.FilledMemory;
                            }}
                        }}

                        {ProtectedIfApplicable}ReadOnlyMemory<byte> _LazinatorObjectBytes;
                        public {DerivationKeyword}ReadOnlyMemory<byte> LazinatorObjectBytes
                        {{
                            get => _LazinatorObjectBytes;
                            set
                            {{
                                _LazinatorObjectBytes = value;
                                int length = Deserialize();
                                _LazinatorObjectBytes = _LazinatorObjectBytes.Slice(0, length);
                            }}
                        }}

                        public {DerivationKeyword}void LazinatorConvertToBytes()
                        {{
                            if (!IsDirty && !DescendantIsDirty && _LazinatorObjectBytes.Length > 0)
                            {{
                                return;
                            }}
                            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
                            _LazinatorObjectBytes = bytes.FilledMemory;
                        }}

                        public {DerivationKeyword}int GetByteLength()
                        {{
                            LazinatorConvertToBytes();
                            return _LazinatorObjectBytes.Length;
                        }}

                        public {DerivationKeyword}uint GetBinaryHashCode32()
                        {{
                            LazinatorConvertToBytes();
                            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
                        }}

                        public {DerivationKeyword}ulong GetBinaryHashCode64()
                        {{
                            LazinatorConvertToBytes();
                            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
                        }}

                        public {DerivationKeyword}Guid GetBinaryHashCode128()
                        {{
                            LazinatorConvertToBytes();
                            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
                        }}

                        /* Field definitions */
        
                ";
                }


                sb.Append(boilerplate);
            }
            else if (!IsAbstract)
            {
                sb.Append($@"        /* Clone overrides */

                        {constructor}public override ILazinator CloneLazinator()
                        {{
                            return CloneLazinator(OriginalIncludeChildrenMode);
                        }}

                        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
                        {{
                            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
                            var clone = new {NameIncludingGenerics}()
                            {{
                                LazinatorParentClass = LazinatorParentClass,
                                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                                OriginalIncludeChildrenMode = includeChildrenMode,
                                HierarchyBytes = bytes
                            }};
                            return clone;
                        }}

                        /* Properties */
");
            }
        }

        private void AppendPropertyDefinitions(CodeStringBuilder sb)
        {
            var thisLevel = PropertiesToDefineThisLevel;
            var withRecordedIndices = thisLevel
                .Where(property =>
                    property.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface ||
                    property.PropertyType == LazinatorPropertyType.LazinatorStruct ||
                    property.PropertyType == LazinatorPropertyType.NonSelfSerializingType ||
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
                    if (ObjectType != LazinatorObjectType.Struct && (lastPropertyToIndex.PropertyType == LazinatorPropertyType.OpenGenericParameter || derivationKeyword == "override "))
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
            sb.Append($@"public abstract int LazinatorUniqueID {{ get; }}
                        {ProtectedIfApplicable}abstract System.Collections.Generic.List<int> _LazinatorGenericID {{ get; set; }}
                        {ProtectedIfApplicable}{DerivationKeyword}bool ContainsOpenGenericParameters => {(IsGeneric ? "true" : "false")};
                        public abstract System.Collections.Generic.List<int> LazinatorGenericID {{ get; set; }}
                        public abstract int LazinatorObjectVersion {{ get; set; }}
                        {(ImplementsConvertFromBytesAfterHeader ? skipConvertFromBytesAfterHeaderString : $@"public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);")}
                        public abstract void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
                        {(ImplementsWritePropertiesIntoBuffer ? skipWritePropertiesIntoBufferString : $@"{ProtectedIfApplicable}abstract void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID);")}
                        {ProtectedIfApplicable}abstract void ResetAccessedProperties();
");
        }

        private void AppendResetProperties(CodeStringBuilder sb)
        {
            string resetAccessed = "";
            foreach (var property in PropertiesToDefineThisLevel.Where(x => x.PropertyType != LazinatorPropertyType.OpenGenericParameter && x.PropertyType != LazinatorPropertyType.PrimitiveType && x.PropertyType != LazinatorPropertyType.PrimitiveTypeNullable))
            {
                resetAccessed += $"_{property.PropertyName}_Accessed = ";
            }
            if (resetAccessed != "")
                resetAccessed += $@"false;";

            sb.AppendLine($@"
                {ProtectedIfApplicable}{DerivationKeyword}void ResetAccessedProperties()
                {{
                    {(IsDerivedFromNonAbstractLazinator ? $@"base.ResetAccessedProperties();
                    " : "")}{resetAccessed}
                }}");
        }

        private void AppendConversionSectionStart(CodeStringBuilder sb)
        {
            string containsOpenGenericParametersString = $@"{ProtectedIfApplicable}{DerivationKeyword}bool ContainsOpenGenericParameters => {(IsGeneric ? "true" : "false")};";

            string lazinatorGenericBackingID = "";
            if (!IsDerivedFromNonAbstractLazinator && (IsGeneric || !IsSealedOrStruct))
                lazinatorGenericBackingID = $@"{ProtectedIfApplicable}{DerivationKeyword}System.Collections.Generic.List<int> _LazinatorGenericID {{ get; set; }}
                        ";

            string lazinatorGenericID;
            if (IsGeneric)
                lazinatorGenericID = $@"{containsOpenGenericParametersString}
                        {lazinatorGenericBackingID}public {DerivationKeyword}System.Collections.Generic.List<int> LazinatorGenericID
                        {{
                            get
                            {{
                                if (_LazinatorGenericID == null)
                                {{
                                    _LazinatorGenericID = DeserializationFactory.Instance.GetUniqueIDListForGenericType({ UniqueID }, new Type[] {{ {GenericArgumentNameTypes} }});
                                }}
                                return _LazinatorGenericID;
                            }}
                            set
                            {{
                                _LazinatorGenericID = value;
                            }}
                        }}";
            else
                lazinatorGenericID = $@"{containsOpenGenericParametersString}
                        {lazinatorGenericBackingID}public {DerivationKeyword}System.Collections.Generic.List<int> LazinatorGenericID
                        {{
                            get => null;
                            set {{ }}
                        }}";

            string selfSerializationVersionString;
            if (Version == -1)
                selfSerializationVersionString = $@"public int LazinatorObjectVersion
                {{
                    get => -1;
                    set => throw new LazinatorSerializationException(""Lazinator versioning disabled for {NameIncludingGenerics}."");
                }}";
            else if (ObjectType == LazinatorObjectType.Class)
                selfSerializationVersionString = $@"public {DerivationKeyword}int LazinatorObjectVersion {{ get; set; }} = {Version};"; // even if versioning is disabled, we still need to implement the interface
            else
            { // can't set default property value in struct, so we have a workaround. If the version has not been changed, we assume that it is still Version. 
                selfSerializationVersionString =
                        $@"private bool _LazinatorObjectVersionChanged;
                        private int _LazinatorObjectVersionOverride;
                        public int LazinatorObjectVersion
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

                public {DerivationKeyword}int LazinatorUniqueID => { UniqueID };

                {lazinatorGenericID}

                { selfSerializationVersionString }

                ");
        }

        private static void AppendCloseClassSupertypesAndNamespace(CodeStringBuilder sb, IEnumerable<ITypeSymbol> supertypes)
        {
            if (supertypes != null)
                foreach (var supertype in supertypes)
                    sb.Append($@"
                                }}
                            ");

            sb.Append($@"
                            }}
                        }}
                        ");
        }

        private void AppendSupportedConversions(CodeStringBuilder sb)
        {
            var propertiesSupportedCollections = PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.SupportedCollection).ToList();
            var propertiesSupportedTuples = PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.SupportedTuple).ToList();
            var propertiesNonSerialized = PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.NonSelfSerializingType).ToList();

            GetSupportedConversions(sb, propertiesSupportedCollections, propertiesSupportedTuples, propertiesNonSerialized);
        }

        private void AppendSerializeExistingBuffer(CodeStringBuilder sb)
        {

            if (IsDerivedFromNonAbstractLazinator)
                sb.AppendLine(
                        $@"public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                        {{");
            else
                sb.AppendLine(
                        $@"public {DerivationKeyword}void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
                        {{");

            if (IncludeTracingCode)
            {
                sb.AppendLine($@"TabbedText.WriteLine($""Initiating serialization of {ILazinatorTypeSymbol} "");");
            }

            sb.AppendLine($@"{ (ImplementsPreSerialization ? $@"PreSerialization(verifyCleanness, updateStoredBuffer);
                            " : "")}int startPosition = writer.Position;
                            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, {(UniqueIDCanBeSkipped ? "false" : "true")});");

            string additionalDescendantDirtinessChecks, postEncodingDirtinessCheck;
            GetDescendantDirtinessChecks(out additionalDescendantDirtinessChecks, out postEncodingDirtinessCheck);
            sb.AppendLine($@"if (updateStoredBuffer)
                        {{
                        ");
            sb.AppendLine(postEncodingDirtinessCheck);
            sb.AppendLine($@"
                _LazinatorObjectBytes = writer.Slice(startPosition);");
            sb.Append($@"}}
");
            sb.Append($@"}}
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
                        {ProtectedIfApplicable}override void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                        {{{positionInitialization}
                            base.WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);");
            else
            {
                sb.AppendLine(
                        $@"{ProtectedIfApplicable}{DerivationKeyword}void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                        {{{positionInitialization}
                            // header information");


                if (IncludeTracingCode)
                {
                    sb.AppendLine($@"TabbedText.WriteLine($""Writing properties for {ILazinatorTypeSymbol} starting at {{writer.Position}}."");");
                    sb.AppendLine($@"TabbedText.WriteLine($""Includes? uniqueID {{(LazinatorGenericID == null ? LazinatorUniqueID.ToString() : String.Join("""","""",LazinatorGenericID.ToArray()))}} {{includeUniqueID}}, Lazinator version {{Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion}} {!SuppressLazinatorVersionByte}, Object version {{LazinatorObjectVersion}} {Version != -1}, IncludeChildrenMode {{includeChildrenMode}} {!CanNeverHaveChildren}"");");
                    sb.AppendLine($@"TabbedText.WriteLine($""IsDirty {{IsDirty}} DescendantIsDirty {{DescendantIsDirty}} HasParentClass {{LazinatorParentClass != null}}"");");
                }

                if (IsGeneric || !IsSealedOrStruct)
                    sb.AppendLine($@"if (includeUniqueID)
                            {{
                                if (LazinatorGenericID == null)
                                {{
                                    CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
                                }}
                                else
                                {{
                                    WriteLazinatorGenericID(writer, LazinatorGenericID);
                                }}
                            }}");
                else
                    sb.AppendLine(
                       $@"if (includeUniqueID)
                            {{
                                CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
                            }}
                        ");

                sb.AppendLine(
                        $@"{(SuppressLazinatorVersionByte ? "" : $@"CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                        ")}{(Version == -1 ? "" : $@"CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
                        ")}{(CanNeverHaveChildren ? "" : $@"writer.Write((byte)includeChildrenMode);")}");
            }

            sb.AppendLine("// write properties");

            foreach (var property in thisLevel)
            {
                if (IncludeTracingCode)
                {
                    if (property.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || (property.PropertyType == LazinatorPropertyType.LazinatorStruct && property.Nullable))
                    {
                        sb.AppendLine($@"TabbedText.WriteLine($""Now at {{writer.Position}}, before {property.PropertyName} (accessed? {{_{property.PropertyName}_Accessed}}) (backing var null? {{_{property.PropertyName} == null}}) "");");
                    }
                    else if (property.PropertyType == LazinatorPropertyType.LazinatorStruct)
                    {
                        sb.AppendLine($@"TabbedText.WriteLine($""Now at {{writer.Position}}, before {property.PropertyName} (accessed? {{_{property.PropertyName}_Accessed}}) "");");
                    }
                    else if (property.TrackDirtinessNonSerialized)
                        sb.AppendLine($@"TabbedText.WriteLine($""Now at {{writer.Position}}, before {property.PropertyName} (accessed? {{_{property.PropertyName}_Accessed}}) (dirty? {{_{property.PropertyName}_Dirty}})"");");
                    else if (property.PropertyType == LazinatorPropertyType.NonSelfSerializingType || property.PropertyType == LazinatorPropertyType.SupportedCollection || property.PropertyType == LazinatorPropertyType.SupportedTuple)
                        sb.AppendLine($@"TabbedText.WriteLine($""Now at {{writer.Position}}, before {property.PropertyName} (accessed? {{_{property.PropertyName}_Accessed}}"");");
                    else
                        sb.AppendLine($@"TabbedText.WriteLine($""Now at {{writer.Position}}, before {property.PropertyName} value {{{property.PropertyName}}}"");");
                    sb.AppendLine($@"TabbedText.Tabs++;");
                }
                property.AppendPropertyWriteString(sb);
                if (IncludeTracingCode)
                {
                    sb.AppendLine($@"TabbedText.Tabs--;");
                }
            }
            AppendEndByteIndex(sb, thisLevel, "writer.Position");
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
            AppendEndByteIndex(sb, thisLevel, "bytesSoFar");

            sb.Append($@"        }}

        ");
        }

        private void AppendEndByteIndex(CodeStringBuilder sb, List<PropertyDescription> thisLevel, string endByteString)
        {
            var lastProperty = thisLevel.LastOrDefault();
            if (lastProperty != null && lastProperty.PropertyType != LazinatorPropertyType.PrimitiveType && lastProperty.PropertyType != LazinatorPropertyType.PrimitiveTypeNullable)
            {
                sb.Append($@"_{ObjectNameEncodable}_EndByteIndex = {endByteString};
                    ");
            }
        }

        private string GetConstructor()
        {
            string constructor = "";
            if (Compilation.ImplementingTypeRequiresParameterlessConstructor) constructor =
                    $@"public {SimpleName}(){(ILazinatorTypeSymbol.BaseType != null ? " : base()" : "")}
                        {{
                        }}
                        
                        ";
            return constructor;
        }

        private string GetClassContainingStructContainingClassError()
        {
            string classContainingStructContainingClassError = "";
            if (ObjectType == LazinatorObjectType.Struct)
            {
                if (PropertiesToDefineThisLevel.Any(x => x.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface))
                    classContainingStructContainingClassError = $@"

                        if (LazinatorParentClass != null)
                        {{
                            throw new LazinatorDeserializationException(""A Lazinator struct may include a Lazinator class or interface as a property only when the Lazinator struct has no parent class."");
                        }}"; //  Otherwise, when a child is deserialized, the struct's parent will not automatically be affected, because the deserialization will take place in a copy of the struct. Though it is possible to handle this scenario, the risk of error is too great. 
            }

            return classContainingStructContainingClassError;
        }

        const string skipMarkHierarchyClean = "// MarkHierarchyClean defined in main class; thus skipped here";

        private string GetMarkHierarchyCleanMethod()
        {
            if (ImplementsMarkHierarchyClean)
                return skipMarkHierarchyClean;
            string markHierarchyCleanMethod = "";
            if (!IsAbstract)
            {
                markHierarchyCleanMethod = $@"public {DerivationKeyword}void MarkHierarchyClean()
                            {{";
                if (IsDerivedFromNonAbstractLazinator)
                    markHierarchyCleanMethod += $@"
                        base.MarkHierarchyClean();";
                else
                    markHierarchyCleanMethod += $@"
                            _IsDirty = false;
                            _DescendantIsDirty = false;";
                foreach (var property in PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || x.PropertyType == LazinatorPropertyType.LazinatorStruct))
                {
                    if (property.PropertyType == LazinatorPropertyType.LazinatorStruct)
                        markHierarchyCleanMethod += $@"
                            if (_{property.PropertyName}_Accessed)
                            {{
                                {property.PropertyName}{property.NullableStructValueAccessor}.MarkHierarchyClean();
                            }}";
                    else
                        markHierarchyCleanMethod += $@"
                            if (_{property.PropertyName}_Accessed)
                            {{
                                {property.PropertyName}.MarkHierarchyClean();
                            }}";
                }
                foreach (var property in PropertiesToDefineThisLevel.Where(x => x.TrackDirtinessNonSerialized))
                    markHierarchyCleanMethod += $@"
                            _{property.PropertyName}_Dirty = false;";
                markHierarchyCleanMethod += $@"
                    }}";
            }

            return markHierarchyCleanMethod;
        }

        private void GetDescendantDirtinessChecks(out string additionalDescendantDirtinessChecks, out string postEncodingDirtinessCheck)
        {
            // we need a way of determining descendant dirtiness manually. We build a set of checks, each beginning with "||" (which, for the first entry, we strip out for one scenario below).
            string manualDescendantDirtinessChecks = "";
            foreach (var property in PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || x.PropertyType == LazinatorPropertyType.LazinatorStruct))
            {
                if (property.PropertyType == LazinatorPropertyType.LazinatorStruct)
                    manualDescendantDirtinessChecks += $" || (_{property.PropertyName}_Accessed && ({property.PropertyName}{property.NullableStructValueAccessor}.IsDirty || {property.PropertyName}{property.NullableStructValueAccessor}.DescendantIsDirty))";
                else
                    manualDescendantDirtinessChecks += $" || (_{property.PropertyName}_Accessed && {property.PropertyName} != null && ({property.PropertyName}.IsDirty || {property.PropertyName}.DescendantIsDirty))";
            }
            // The following is not necessary, because manual _Dirty properties automatically lead to _IsDirty being set to true. Because non-Lazinators are not considered "children," nothing needs to happen to DescendantIsDirty; this also means that when encoding, non-Lazinators are encoded if dirty regardless of the include child setting.
            //foreach (var property in PropertiesToDefineThisLevel.Where(x => x.TrackDirtinessNonSerialized))
            //    additionalDirtinessChecks += $" || (_{property.PropertyName}_Accessed && {property.PropertyName}_Dirty)";

            // For a class, we don't need to use the manual descendant dirtiness checks routinely, since DescendantIsDirty will automatically be sent.
            // But a struct's descendants (whether classes or structs) have no way of informing the struct that they are dirty. A struct cannot pass to the descendant a delegate so that the descendant can inform the struct that it is dirty. (When a struct passes a delegate, the delegate actually operates on a copy of the struct, not the original.) Thus, the only way to check for descendant dirtiness is to check each child self-serialized property.
            additionalDescendantDirtinessChecks = "";
            if (ObjectType == LazinatorObjectType.Struct)
            {
                additionalDescendantDirtinessChecks = manualDescendantDirtinessChecks;
            }

            // After encoding in a mode in which we don't encode all children, we will need to do a check.
            
            if (manualDescendantDirtinessChecks == "")
                postEncodingDirtinessCheck =
                    $@"
                        _IsDirty = false;
                        _DescendantIsDirty = false;";
            else
                postEncodingDirtinessCheck =
                    $@"
                        _IsDirty = false;
                        _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && (" + manualDescendantDirtinessChecks.Substring(4) + ");";
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
            otherNamespaces.AddRange(new string[] { "System", "System.Buffers", "System.Diagnostics", "System.IO", "System.Runtime.InteropServices", "Lazinator.Attributes", "Lazinator.Buffers", "Lazinator.Core", "Lazinator.Exceptions", "Lazinator.Support" });
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
            if (ObjectType == LazinatorObjectType.Struct)
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
            if (!iLazinatorType.IsAbstract && genericArguments.Any(x => 
                    !(
                        ( // each generic argument must implement ILazinator or be constrained to ILazinator
                            x.Interfaces.Any(y => y.Name == "ILazinator") 
                            ||
                            (((x as ITypeParameterSymbol)?.ConstraintTypes.Any(y => y.Name == "ILazinator") ?? false))
                        )
                    )
                    && // but if this is a Lazinator interface type, that's fine too.
                    !Compilation.ContainsAttributeOfType<CloneLazinatorAttribute>(x)
                    )
                )
                throw new LazinatorCodeGenException("Open generic parameter in non-abstract type must be constrained to type ILazinator and should generally implement new() if not a struct. Add a clause like 'where T : ILazinator, new()'");
            GenericArgumentNames = genericArguments.Select(x => x.Name).ToList();
            if (GenericArgumentNames.Any())
                NameIncludingGenerics = iLazinatorType.Name + "<" + string.Join(", ", GenericArgumentNames) + ">";
            else
                NameIncludingGenerics = iLazinatorType.Name;
            FullyQualifiedObjectName = iLazinatorType.GetFullNamespace() + "." + NameIncludingGenerics;
        }
    }
}
