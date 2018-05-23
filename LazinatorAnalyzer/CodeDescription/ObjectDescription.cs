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
        public INamedTypeSymbol ILazinatorTypeSymbol { get; set; }
        public INamedTypeSymbol InterfaceTypeSymbol { get; set; }
        public Accessibility Accessibility { get; set; }
        public string Namespace { get; set; }
        public string ObjectName { get; set; }
        public string MetadataName { get; set; }
        public string FullyQualifiedObjectName { get; set; }
        public string ObjectNameEncodable => RoslynHelpers.EncodableTypeName(ILazinatorTypeSymbol);
        public LazinatorObjectType ObjectType { get; set; }
        public bool IsNonLazinatorBaseClass { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }
        public string SealedKeyword => IsSealed ? "sealed " : "";
        public ExclusiveInterfaceDescription ExclusiveInterface { get; set; }
        public int Version => ExclusiveInterface.Version;
        public int UniqueID => (int)ExclusiveInterface.UniqueID;
        public List<NonexclusiveInterfaceDescription> NonexclusiveInterfaces { get; set; }
        public ObjectDescription BaseLazinatorObject { get; set; }
        public bool IsDerivedFromNonAbstractLazinator => BaseLazinatorObject != null &&
                                      (BaseLazinatorObject.IsDerivedFromNonAbstractLazinator ||
                                       !BaseLazinatorObject.IsAbstract);
        public bool IsDerivedFromAbstractLazinator => BaseLazinatorObject != null &&
                                                         (BaseLazinatorObject.IsDerivedFromAbstractLazinator ||
                                                          BaseLazinatorObject.IsAbstract);
        public string DerivationKeyword => (IsDerivedFromNonAbstractLazinator || IsDerivedFromAbstractLazinator) ? "override " : (IsSealed || ObjectType != LazinatorObjectType.Class ? "" : "virtual ");
        public string BaseObjectName => BaseLazinatorObject.Namespace == Namespace ? BaseLazinatorObject.ObjectName : BaseLazinatorObject.FullyQualifiedObjectName;
        public int TotalNumProperties => ExclusiveInterface.TotalNumProperties;
        public bool ImplementsLazinatorObjectVersionUpgrade { get; set; }
        public bool ImplementsPreSerialization { get; set; }
        public bool ImplementsPostDeserialization { get; set; }
        public bool ImplementsOnDirty { get; set; }
        public List<string> GenericArgumentNames { get; set; }
        public List<PropertyDescription> PropertiesToDefineThisLevel => ExclusiveInterface.PropertiesToDefineThisLevel;
        public LazinatorCompilation Compilation;
        public Guid Hash;
        public bool SuppressDate { get; set; }
        public string ProtectedIfApplicable => (ObjectType == LazinatorObjectType.Struct || IsSealed) ? "" : "protected ";

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
            ObjectName = iLazinatorTypeSymbol.Name;
            MetadataName = iLazinatorTypeSymbol.MetadataName;
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
            List<string> namespaces = PropertiesToDefineThisLevel.SelectMany(x => x.PropertyAndInnerProperties().Select(y => y.Namespace)).ToList();
            namespaces.AddRange(ILazinatorTypeSymbol.GetNamespacesOfTypesAndContainedTypes());
            string theBeginning =
                $@"{GetFileHeader(Hash.ToString(), Namespace, namespaces)}

                    [Autogenerated]
                    {AccessibilityConverter.Convert(Accessibility)} { SealedKeyword }partial { (ObjectType == LazinatorObjectType.Class ? "class" : "struct") } { ObjectName } : {(IsDerivedFromNonAbstractLazinator ? BaseObjectName + ", " : "")}ILazinator
                    {{";
            sb.AppendLine(theBeginning);

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

                string additionalDirtinessChecks = "";
            if (ObjectType == LazinatorObjectType.Struct)
            {
                // a struct's descendants (whether classes or structs) have no way of informing the struct that they are dirty. A struct cannot pass to the descendant a delegate so that the descendant can inform the struct that it is dirty. (When a struct passes a delegate, the delegate actually operates on a copy of the struct, not the original.) Thus, the only way to check for descendant dirtiness is to check each child self-serialized property.
                foreach (var property in PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || x.PropertyType == LazinatorPropertyType.LazinatorStruct))
                {
                    if (property.PropertyType == LazinatorPropertyType.LazinatorStruct)
                        additionalDirtinessChecks += $" || (_{property.PropertyName}_Accessed && ({property.PropertyName}{property.NullableStructValueAccessor}.IsDirty || {property.PropertyName}{property.NullableStructValueAccessor}.DescendantIsDirty))";
                    else
                        additionalDirtinessChecks += $" || (_{property.PropertyName}_Accessed && {property.PropertyName} != null && ({property.PropertyName}.IsDirty || {property.PropertyName}.DescendantIsDirty))";
                }
                // The following is not necessary, because manual _Dirty properties automatically lead to _IsDirty being set to true. Because non-Lazinators are not considered "children," nothing needs to happen to DescendantIsDirty.
                //foreach (var property in PropertiesToDefineThisLevel.Where(x => x.TrackDirtinessNonSerialized))
                //    additionalDirtinessChecks += $" || (_{property.PropertyName}_Accessed && {property.PropertyName}_Dirty)";
            }

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

            string resetAccessed = "";
            foreach (var property in PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || x.PropertyType == LazinatorPropertyType.LazinatorStruct))
            {
                resetAccessed += $"_{property.PropertyName}_Accessed = ";
            }
            if (resetAccessed != "")
                resetAccessed += "false;";

            if (!IsDerivedFromNonAbstractLazinator)
            {
                string boilerplate;
                if (IsAbstract && BaseLazinatorObject?.IsAbstract == true) // abstract class inheriting from abstract class
                    boilerplate = $@""; // everything is inherited from parent abstract class
                else if (IsAbstract)
                    boilerplate = $@"        /* Abstract declarations */
			            public abstract ILazinator LazinatorParentClass {{ get; set; }}
                    
                        public abstract void Deserialize();
                        
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

                        public abstract void MarkHierarchyClean();
                        
                        public abstract DeserializationFactory DeserializationFactory {{ get; set; }}
		                
                        public abstract MemoryInBuffer HierarchyBytes
                        {{
			                get;
			                set;
                        }}
                        
                        public abstract ReadOnlyMemory<byte> LazinatorObjectBytes
                        {{
			                get;
			                set;
                        }}

                        public abstract void LazinatorConvertToBytes();
                        public abstract uint GetBinaryHashCode32();
                        public abstract ulong GetBinaryHashCode64();

                        /* Field boilerplate */
        
                ";
                else
                {
                    if (MetadataName.Contains("AvlMultiset"))
                    {
                        var DEBUG = 0;
                    }
                    boilerplate = $@"        /* Serialization, deserialization, and object relationships */

                        {(Compilation.ImplementingTypeRequiresParameterlessConstructor ? $@"public {MetadataName}()
                        {{
                        }}
                        
                        " : "")}public {DerivationKeyword}ILazinator LazinatorParentClass {{ get; set; }}

                        {ProtectedIfApplicable}IncludeChildrenMode OriginalIncludeChildrenMode;

                        public {DerivationKeyword}void Deserialize()
                        {{
                            int bytesSoFar = 0;
                            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
                            if (span.Length == 0)
                            {{
                                return;
                            }}{classContainingStructContainingClassError}

                            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
                            if (uniqueID != LazinatorUniqueID)
                            {{
                                throw new FormatException(""Wrong self-serialized type initialized."");
                            }}

                            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
                            
                            int serializedVersionNumber = {(Version == -1 ? "-1; /* versioning disabled */" : $@"span.ToDecompressedInt(ref bytesSoFar);")}

                            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);

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
                            var clone = new {ObjectName}()
                            {{
                                DeserializationFactory = DeserializationFactory,
                                LazinatorParentClass = LazinatorParentClass,
                                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                                OriginalIncludeChildrenMode = includeChildrenMode,
                                HierarchyBytes = bytes
                            }};
                            return clone;
                        }}

                        private bool _IsDirty;
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

                        private bool _DescendantIsDirty;
                        public {DerivationKeyword}bool DescendantIsDirty
                        {{
                            [DebuggerStepThrough]
                            get => _DescendantIsDirty{additionalDirtinessChecks};
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

                        public {DerivationKeyword}DeserializationFactory DeserializationFactory {{ get; set; }}
        
                        private MemoryInBuffer _HierarchyBytes;
                        public {DerivationKeyword}MemoryInBuffer HierarchyBytes
                        {{
                            get => _HierarchyBytes;
                            set
                            {{
                                _HierarchyBytes = value;
                                LazinatorObjectBytes = value.FilledMemory;
                            }}
                        }}

                        private ReadOnlyMemory<byte> _LazinatorObjectBytes;
                        public {DerivationKeyword}ReadOnlyMemory<byte> LazinatorObjectBytes
                        {{
                            get => _LazinatorObjectBytes;
                            set
                            {{
                                _LazinatorObjectBytes = value;
                                Deserialize();
                            }}
                        }}

                        public {DerivationKeyword}void LazinatorConvertToBytes()
                        {{
                            if (!IsDirty && !DescendantIsDirty)
                            {{
                                return;
                            }}
                            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
                            _IsDirty = false;
                            LazinatorObjectBytes = bytes.FilledMemory;{(resetAccessed != "" ? $@"
                            {resetAccessed}" : "")}
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

                        /* Field definitions */
        
                ";
                }


                sb.Append(boilerplate);
            }
            else if (!IsAbstract)
            {
                sb.Append($@"        /* Clone overrides */

                        public override ILazinator CloneLazinator()
                        {{
                            return CloneLazinator(OriginalIncludeChildrenMode);
                        }}

                        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
                        {{
                            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
                            var clone = new {ObjectName}()
                            {{
                                DeserializationFactory = DeserializationFactory,
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

            if (IsAbstract)
            {
                if (IsDerivedFromAbstractLazinator || IsDerivedFromNonAbstractLazinator)
                { // lowere level has already defined these methods, so nothing more to do
                    sb.AppendLine($@"}}
                            }}");
                    return;
                }

                sb.Append($@"public abstract int LazinatorUniqueID {{ get; }}
                        public abstract int LazinatorObjectVersion {{ get; set; }}
                        public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
                        public abstract void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
                }}
            }}
");
                return;
            }

            string selfSerializationVersionString;
            if (Version == -1)
                selfSerializationVersionString = $@"public int LazinatorObjectVersion
                {{
                    get => -1;
                    set => throw new Exception(""Lazinator versioning disabled for {ObjectName}."");
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

                { selfSerializationVersionString }

                public {DerivationKeyword}void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
                {{
                    {(!IsDerivedFromNonAbstractLazinator ? "" : $@"base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
                    ")}ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;");

            foreach (var property in thisLevel)
            {
                property.AppendPropertyReadString(sb);
            }
            var lastProperty = thisLevel.LastOrDefault();
            if (lastProperty != null && lastProperty.PropertyType != LazinatorPropertyType.PrimitiveType && lastProperty.PropertyType != LazinatorPropertyType.PrimitiveTypeNullable)
            {
                sb.Append($@"_{ObjectNameEncodable}_EndByteIndex = bytesSoFar;
                    ");
            }

            sb.Append($@"        }}

        ");

            if (IsDerivedFromNonAbstractLazinator)
                sb.AppendLine(
                        $@"public override void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
                        {{
                            {(ImplementsPreSerialization ? $@"PreSerialization();
                            " : "")}base.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);");
            else
                sb.AppendLine(
                        $@"public {DerivationKeyword}void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
                        {{
                            {(ImplementsPreSerialization ? $@"PreSerialization();
                            " : "")}// header information
                            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
                            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                            {(Version == -1 ? "" : $@"CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
                        ")}writer.Write((byte)includeChildrenMode);");

            sb.AppendLine("// write properties");

            foreach (var property in thisLevel)
            {
                property.AppendPropertyWriteString(sb);
            }

            sb.Append($@"}}
");
            var propertiesSupportedCollections = PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.SupportedCollection).ToList();
            var propertiesSupportedTuples = PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.SupportedTuple).ToList();
            var propertiesNonSerialized = PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.NonSelfSerializingType).ToList();

            GetSupportedConversions(sb, propertiesSupportedCollections, propertiesSupportedTuples, propertiesNonSerialized);

            sb.Append($@"
                            }}
                        }}
                        ");
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
            otherNamespaces.AddRange(new string[] { "System", "System.Buffers", "System.Diagnostics", "System.IO", "System.Runtime.InteropServices", "Lazinator.Buffers", "Lazinator.Core", "Lazinator.Exceptions", "Lazinator.Support" });
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
                ObjectName = iLazinatorType.Name + "<" + string.Join(", ", GenericArgumentNames) + ">";
            else
                ObjectName = iLazinatorType.Name;
            FullyQualifiedObjectName = iLazinatorType.GetFullNamespace() + "." + ObjectName;
        }
    }
}
