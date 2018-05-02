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
        public string Namespace { get; set; }
        public string ObjectName { get; set; }
        public LazinatorObjectType ObjectType { get; set; }
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
        public string DeriveKeyword => (IsDerivedFromNonAbstractLazinator || IsDerivedFromAbstractLazinator) ? "override " : (IsSealed || ObjectType != LazinatorObjectType.Class ? "" : "virtual ");
        public string BaseObjectName => BaseLazinatorObject?.ObjectName;
        public int TotalNumProperties => ExclusiveInterface.TotalNumProperties;
        public bool ImplementsLazinatorObjectVersionUpgrade { get; set; }
        public bool ImplementsPreSerialization { get; set; }
        public bool ImplementsPostDeserialization { get; set; }
        public List<string> GenericArgumentNames { get; set; }
        public List<PropertyDescription> PropertiesToDefineThisLevel => ExclusiveInterface.PropertiesToDefineThisLevel;
        public LazinatorCompilation CodeFiles;
        public Guid Hash;

        public ObjectDescription()
        {

        }

        public ObjectDescription(INamedTypeSymbol iLazinatorTypeSymbol, LazinatorCompilation codeFiles)
        {
            CodeFiles = codeFiles;
            Namespace = iLazinatorTypeSymbol.GetFullNamespace();
            ObjectName = iLazinatorTypeSymbol.Name;
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
                    BaseLazinatorObject = new ObjectDescription(baseILazinatorType, codeFiles);
            }

            INamedTypeSymbol interfaceType = CodeFiles.TypeToExclusiveInterface[iLazinatorTypeSymbol.OriginalDefinition];
            Hash = CodeFiles.InterfaceTextHash.ContainsKey(interfaceType) ? CodeFiles.InterfaceTextHash[interfaceType] : default;
            ExclusiveInterface = new ExclusiveInterfaceDescription(interfaceType, this);
            if (ExclusiveInterface.GenericArgumentNames.Any())
                HandleGenerics(iLazinatorTypeSymbol);
            var nonexclusiveInterfaces = iLazinatorTypeSymbol.AllInterfaces
                                .Where(x => CodeFiles.ContainsAttributeOfType<CloneNonexclusiveLazinatorAttribute>(x));
            NonexclusiveInterfaces = nonexclusiveInterfaces
                .Select(x => new NonexclusiveInterfaceDescription(CodeFiles, x, this)).ToList();

            ImplementsLazinatorObjectVersionUpgrade = CodeFiles.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "LazinatorObjectVersionUpgrade"));
            ImplementsPreSerialization = CodeFiles.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "PreSerialization"));
            ImplementsPostDeserialization = CodeFiles.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "PostDeserialization"));
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

        public string GetCodeBehind()
        {
            CodeStringBuilder sb = new CodeStringBuilder();
            AppendCodeBehindFile(sb);
            string result = sb.ToString();
            return result;
        }

        private void AppendCodeBehindFile(CodeStringBuilder sb)
        {
            string theBeginning =
                $@"//{Hash}
                //------------------------------------------------------------------------------
                // <auto-generated>
                //     This code was generated by the Lazinator tool.
                //
                //     Changes to this file may cause incorrect behavior and will be lost if
                //     the code is regenerated.
                // </auto-generated>
                //------------------------------------------------------------------------------

                using System;
                using System.Buffers;
                using System.Diagnostics;
                using System.IO;
                using System.Runtime.InteropServices;
                using Lazinator.Buffers; 
                using Lazinator.Collections;
                using Lazinator.Core; 
                using static Lazinator.Core.LazinatorUtilities;
                using Lazinator.Exceptions;
                using Lazinator.Support;
                using Lazinator.Wrappers;

                namespace { Namespace }
                {{
                    public { SealedKeyword }partial { (ObjectType == LazinatorObjectType.Class ? "class" : "struct") } { ObjectName } : {(IsDerivedFromNonAbstractLazinator ? BaseObjectName + ", " : "")}ILazinator
                    {{";
            sb.AppendLine(theBeginning);


            string additionalDirtinessChecks = "";
            if (ObjectType != LazinatorObjectType.Class)
            {
                // a struct's descendants (whether classes or structs) have no way of informing the struct that they are dirty. A struct cannot pass to the descendant a delegate so that the descendant can inform the struct that it is dirty. (When a struct passes a delegate, the delegate actually operates on a copy of the struct, not the original.) Thus, the only way to check for descendant dirtiness is to check each child self-serialized property.
                foreach (var property in PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || x.PropertyType == LazinatorPropertyType.LazinatorStruct))
                {
                    if (property.PropertyType == LazinatorPropertyType.LazinatorStruct)
                        additionalDirtinessChecks += $" || ({property.PropertyName}{property.NullableStructValueAccessor}.IsDirty || {property.PropertyName}{property.NullableStructValueAccessor}.DescendantIsDirty)";
                    else
                        additionalDirtinessChecks += $" || ({property.PropertyName} != null && ({property.PropertyName}.IsDirty || {property.PropertyName}.DescendantIsDirty))";
                }
            }

            if (!IsDerivedFromNonAbstractLazinator)
            {
                string boilerplate;
                if (IsAbstract && BaseLazinatorObject?.IsAbstract == true) // abstract class inheriting from abstract class
                    boilerplate = $@""; // everything is inherited from parent abstract class
                else if (IsAbstract)
                    boilerplate = $@"        /* Boilerplate for abstract ILazinator object */
			            public abstract ILazinator LazinatorParentClass {{ get; set; }}
                    
                        public abstract void Deserialize();
                        
                        public abstract MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
                        
                        internal abstract MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
                        
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

                        /* Field boilerplate */
        
                ";
                else
                {
                    string overrideKeyword;
                    if (BaseLazinatorObject?.IsAbstract == true)
                        overrideKeyword = "override ";
                    else
                        overrideKeyword = "";
                    boilerplate = $@"        /* Boilerplate for every non-abstract ILazinator object */

                        public {overrideKeyword}ILazinator LazinatorParentClass {{ get; set; }}

                        {
                            (ObjectType == LazinatorObjectType.Struct || IsSealed ? "" : "protected ")
                        }internal IncludeChildrenMode OriginalIncludeChildrenMode;

                        public {overrideKeyword}void Deserialize()
                        {{
                            int bytesSoFar = 0;
                            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
                            if (span.Length == 0)
                            {{
                                return;
                            }}

                            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
                            if (uniqueID != LazinatorUniqueID)
                            {{
                                throw new FormatException(""Wrong self-serialized type initialized."");
                            }}

                            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);

                            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);

                            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);

                            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);{
                            (ImplementsLazinatorObjectVersionUpgrade
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

                        public {overrideKeyword}MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
                        {{
                            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
                        }}

                        internal {overrideKeyword}MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBuffer(this, includeChildrenMode, verifyCleanness);

                        public {overrideKeyword}ILazinator CloneLazinator()
                        {{
                            return CloneLazinator(OriginalIncludeChildrenMode);
                        }}

                        public {overrideKeyword}ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
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
                        public {overrideKeyword}bool IsDirty
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
                                        InformParentOfDirtiness();
                                    }}
                                }}
                            }}
                        }}

                        public {overrideKeyword}InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate {{ get; set; }}
                        public {overrideKeyword}void InformParentOfDirtiness()
                        {{
                            if (InformParentOfDirtinessDelegate == null)
                            {{
                                if (LazinatorParentClass != null)
                                {{
                                    LazinatorParentClass.DescendantIsDirty = true;
                                }}
                            }}
                            else
                                InformParentOfDirtinessDelegate();
                        }}

                        private bool _DescendantIsDirty;
                        public {overrideKeyword}bool DescendantIsDirty
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

                        public {overrideKeyword}DeserializationFactory DeserializationFactory {{ get; set; }}
        
                        private MemoryInBuffer _HierarchyBytes;
                        public {overrideKeyword}MemoryInBuffer HierarchyBytes
                        {{
                            get => _HierarchyBytes;
                            set
                            {{
                                _HierarchyBytes = value;
                                LazinatorObjectBytes = value.FilledMemory;
                            }}
                        }}

                        private ReadOnlyMemory<byte> _LazinatorObjectBytes;
                        public {overrideKeyword}ReadOnlyMemory<byte> LazinatorObjectBytes
                        {{
                            get => _LazinatorObjectBytes;
                            set
                            {{
                                _LazinatorObjectBytes = value;
                                Deserialize();
                            }}
                        }}

                        /* Field boilerplate */
        
                ";
                }

                sb.Append(boilerplate);
            }

            var thisLevel = PropertiesToDefineThisLevel;
            var withRecordedIndices = thisLevel.Where(property =>
                property.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface ||
                property.PropertyType == LazinatorPropertyType.LazinatorStruct ||
                property.PropertyType == LazinatorPropertyType.NonSelfSerializingType ||
                property.PropertyType == LazinatorPropertyType.SupportedCollection ||
                property.PropertyType == LazinatorPropertyType.SupportedTuple ||
                property.PropertyType == LazinatorPropertyType.OpenGenericParameter).ToList();

            if (!IsAbstract)
            {
                for (int i = 0; i < withRecordedIndices.Count(); i++)
                    sb.AppendLine($"        internal int _{withRecordedIndices[i].PropertyName}_ByteIndex;");
                for (int i = 0; i < withRecordedIndices.Count(); i++)
                    sb.AppendLine(
                        $"internal int _{withRecordedIndices[i].PropertyName}_ByteLength => {(i == withRecordedIndices.Count() - 1 ? "LazinatorObjectBytes.Length" : $"_{withRecordedIndices[i + 1].PropertyName}_ByteIndex")} - _{withRecordedIndices[i].PropertyName}_ByteIndex;");
                sb.AppendLine();
            }

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
            if (ObjectType == LazinatorObjectType.Class)
                selfSerializationVersionString = $@"public {DeriveKeyword}int LazinatorObjectVersion {{ get; set; }} = {Version};";
            else
            {
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

                public {DeriveKeyword}int LazinatorUniqueID => { UniqueID };

                { selfSerializationVersionString }

                public {DeriveKeyword}void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
                {{
                    {(!IsDerivedFromNonAbstractLazinator ? "" : $@"base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
                    ")}ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;");

            foreach (var property in thisLevel)
            {
                property.AppendPropertyReadInSupportedCollectionString(sb);
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
                        $@"public {DeriveKeyword}void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
                        {{
                            {(ImplementsPreSerialization ? $@"PreSerialization();
                            " : "")}// header information
                            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
                            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
                            writer.Write((byte)includeChildrenMode);");

            sb.AppendLine("// write properties");

            foreach (var property in thisLevel)
            {
                property.AppendPropertyWriteInSupportedCollectionString(sb);
            }

            sb.Append($@"}}
");
            var propertiesSupportedCollections = PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.SupportedCollection).ToList();
            var propertiesSupportedTuples = PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.SupportedTuple).ToList();
            var propertiesNonSerialized = PropertiesToDefineThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.NonSelfSerializingType).ToList();
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


            sb.Append($@"
                            }}
                        }}
                        ");
        }

        public void HandleGenerics(INamedTypeSymbol iLazinatorType)
        {
            var genericArguments = iLazinatorType.TypeArguments;
            if (!iLazinatorType.IsAbstract && genericArguments.Any(x => 
                    !((x as ITypeParameterSymbol)?.ConstraintTypes.Any(y => y.Name == "ILazinator") ?? false) 
                    && 
                    !CodeFiles.ContainsAttributeOfType<CloneLazinatorAttribute>(x)
                    )
                )
                throw new Exception("Open generic parameter in non-abstract type must be constrained to type ILazinator.");
            GenericArgumentNames = genericArguments.Select(x => x.Name).ToList();
            ObjectName = iLazinatorType.Name + "<" + string.Join(", ", GenericArgumentNames) + ">";
        }
    }
}
