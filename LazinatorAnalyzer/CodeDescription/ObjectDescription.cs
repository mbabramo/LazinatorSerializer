﻿using System;
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
        public bool IsSealed { get; set; }
        public string SealedKeyword => IsSealed ? "sealed " : "";
        public ExclusiveInterfaceDescription ExclusiveInterface { get; set; }
        public int Version => ExclusiveInterface.Version;
        public int UniqueID => (int)ExclusiveInterface.UniqueID;
        public List<NonexclusiveInterfaceDescription> NonexclusiveInterfaces { get; set; }
        public ObjectDescription BaseLazinatorObject { get; set; }
        public bool IsDerived => BaseLazinatorObject != null;
        public string DeriveKeyword => IsDerived ? "override " : (IsSealed || ObjectType != LazinatorObjectType.Class ? "" : "virtual ");
        public string BaseObjectName => BaseLazinatorObject?.ObjectName;
        public int TotalNumProperties => ExclusiveInterface.TotalNumProperties;
        public bool ImplementsLazinatorObjectVersionUpgrade { get; set; }
        public bool ImplementsPreSerialization { get; set; }
        public bool ImplementsPostDeserialization { get; set; }
        public List<string> GenericArgumentNames { get; set; }
        public List<PropertyDescription> PropertiesThisLevel => ExclusiveInterface.PropertiesThisLevel;
        public List<PropertyDescription> PropertiesIncludingInherited => ExclusiveInterface.PropertiesIncludingInherited;
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
            Hash = CodeFiles.InterfaceTextHash.ContainsKey(interfaceType) ? CodeFiles.InterfaceTextHash[interfaceType] : default(Guid);
            ExclusiveInterface = new ExclusiveInterfaceDescription(interfaceType, this);
            if (ExclusiveInterface.GenericArgumentNames.Any())
                HandleGenerics(iLazinatorTypeSymbol);
            var nonexclusiveInterfaces = iLazinatorTypeSymbol.AllInterfaces
                                .Where(x => CodeFiles.ContainsAttributeOfType<NonexclusiveLazinatorAttribute>(x));
            NonexclusiveInterfaces = nonexclusiveInterfaces
                .Select(x => new NonexclusiveInterfaceDescription(CodeFiles, x, this)).ToList();

            ImplementsLazinatorObjectVersionUpgrade = CodeFiles.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "LazinatorObjectVersionUpgrade"));
            ImplementsPreSerialization = CodeFiles.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "PreSerialization"));
            ImplementsPostDeserialization = CodeFiles.TypeImplementsMethod.Contains((iLazinatorTypeSymbol, "PostDeserialization"));
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
                using System.Collections.Generic;
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
                {GetNeededNamespaces()}
                namespace { Namespace }
                {{
                    public { SealedKeyword }partial { StringEnum.GetStringValue(ObjectType) } { ObjectName } : {(IsDerived ? BaseObjectName + ", " : "")}ILazinator
                    {{";
            sb.AppendLine(theBeginning);

            if (!IsDerived)
            {
                string additionalDirtinessChecks = "";
                if (ObjectType != LazinatorObjectType.Class)
                {
                    // a struct's descendants (whether classes or structs) have no way of informing the struct that they are dirty. A struct cannot pass to the descendant a delegate so that the descendant can inform the struct that it is dirty. (When a struct passes a delegate, the delegate actually operates on a copy of the struct, not the original.) Thus, the only way to check for descendant dirtiness is to check each child self-serialized property.
                    foreach (var property in PropertiesThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface || x.PropertyType == LazinatorPropertyType.LazinatorStruct))
                    {
                        if (property.PropertyType == LazinatorPropertyType.LazinatorStruct)
                            additionalDirtinessChecks += $" || ({property.PropertyName}{property.NullableStructValueAccessor}.IsDirty || {property.PropertyName}{property.NullableStructValueAccessor}.DescendantIsDirty)";
                        else
                            additionalDirtinessChecks += $" || ({property.PropertyName} != null && ({property.PropertyName}.IsDirty || {property.PropertyName}.DescendantIsDirty))";
                    }
                }

                string boilerplate = $@"        /* Boilerplate for every base class implementing ILazinator */

                        public ILazinator LazinatorParentClass {{ get; set; }}

                        {(ObjectType == LazinatorObjectType.Struct || IsSealed ? "" : "protected ")}internal IncludeChildrenMode OriginalIncludeChildrenMode;

                        public void Deserialize()
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

                            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);{(ImplementsLazinatorObjectVersionUpgrade ? $@"
                            if (serializedVersionNumber < LazinatorObjectVersion)
                            {{
                                LazinatorObjectVersionUpgrade(serializedVersionNumber);
                            }}" : "")}{(ImplementsPostDeserialization ? $@"
                            PostDeserialization();" : "")}
                        }}

                        public MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
                        {{
                            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
                        }}

                        internal MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBuffer(this, includeChildrenMode, verifyCleanness);

                        public ILazinator Clone()
                        {{
                            return Clone(OriginalIncludeChildrenMode);
                        }}

                        public ILazinator Clone(IncludeChildrenMode includeChildrenMode)
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
                        public bool IsDirty
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

                        public InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate {{ get; set; }}
                        public void InformParentOfDirtiness()
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
                        public bool DescendantIsDirty
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

                        public DeserializationFactory DeserializationFactory {{ get; set; }}
        
                        private MemoryInBuffer _HierarchyBytes;
                        public MemoryInBuffer HierarchyBytes
                        {{
                            get => _HierarchyBytes;
                            set
                            {{
                                _HierarchyBytes = value;
                                LazinatorObjectBytes = value.FilledMemory;
                            }}
                        }}

                        private ReadOnlyMemory<byte> _LazinatorObjectBytes;
                        public ReadOnlyMemory<byte> LazinatorObjectBytes
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
                sb.Append(boilerplate);
            }

            var thisLevel = PropertiesThisLevel;
            var withRecordedIndices = thisLevel.Where(property =>
                property.PropertyType == LazinatorPropertyType.LazinatorClassOrInterface ||
                property.PropertyType == LazinatorPropertyType.LazinatorStruct ||
                property.PropertyType == LazinatorPropertyType.NonSelfSerializingType ||
                property.PropertyType == LazinatorPropertyType.SupportedCollection ||
                property.PropertyType == LazinatorPropertyType.SupportedTuple ||
                property.PropertyType == LazinatorPropertyType.OpenGenericParameter).ToList();
            for (int i = 0; i < withRecordedIndices.Count(); i++)
                sb.AppendLine($"        internal int _{withRecordedIndices[i].PropertyName}_ByteIndex;");
            for (int i = 0; i < withRecordedIndices.Count(); i++)
                sb.AppendLine(
                    $"internal int _{withRecordedIndices[i].PropertyName}_ByteLength => {(i == withRecordedIndices.Count() - 1 ? "LazinatorObjectBytes.Length" : $"_{withRecordedIndices[i + 1].PropertyName}_ByteIndex")} - _{withRecordedIndices[i].PropertyName}_ByteIndex;");

            sb.AppendLine();

            foreach (var property in thisLevel)
            {
                property.AppendPropertyDefinitionString(sb);
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
                    {(!IsDerived ? "" : $@"base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
                    ")}ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;");

            foreach (var property in thisLevel)
            {
                property.AppendPropertyReadInSupportedCollectionString(sb);
            }

            sb.Append($@"        }}

        ");

            if (IsDerived)
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
            var propertiesSupportedCollections = PropertiesThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.SupportedCollection).ToList();
            var propertiesSupportedTuples = PropertiesThisLevel.Where(x => x.PropertyType == LazinatorPropertyType.SupportedTuple).ToList();
            if (propertiesSupportedCollections.Any() || propertiesSupportedTuples.Any())
                sb.Append($@"
                            /* Conversion of supported collections and tuples */
");

            HashSet<string> alreadyGenerated = new HashSet<string>(); // avoid duplicate additions
            foreach (var property in propertiesSupportedCollections)
                property.AppendSupportedCollectionConversionMethods(sb, alreadyGenerated);

            foreach (var property in propertiesSupportedTuples)
                property.AppendSupportedTupleConversionMethods(sb, alreadyGenerated);


            sb.Append($@"
                            }}
                        }}
                        ");
        }

        public string GetNeededNamespaces()
        {
            HashSet<string> namespacesNeeded = null;
            foreach (var property in PropertiesThisLevel)
            {
                if (property.Namespace != "System" && property.Namespace != "System.Collections.Generic" &&
                    property.Namespace != Namespace && property.Namespace != null && property.Namespace != "")
                {
                    bool isSubset = Namespace.StartsWith(property.Namespace) &&
                                    Namespace[property.Namespace.Length] == '.';
                    if (isSubset)
                        continue;
                    if (namespacesNeeded == null)
                        namespacesNeeded = new HashSet<string>();
                    if (!property.Namespace.StartsWith("Lazinator."))
                        namespacesNeeded.Add($"using {property.Namespace};");
                }
            }

            if (namespacesNeeded == null)
                return "";
            return String.Join("\r\n", namespacesNeeded) + "\r\n";
        }

        public void HandleGenerics(INamedTypeSymbol iLazinatorType)
        {
            var genericArguments = iLazinatorType.TypeArguments;
            if (genericArguments.Any(x => 
                    !((x as ITypeParameterSymbol)?.ConstraintTypes.Any(y => y.Name == "ILazinator") ?? false) 
                    && 
                    !CodeFiles.ContainsAttributeOfType<LazinatorAttribute>(x)
                    )
                )
                throw new Exception("Open generic parameter must be constrained to type ILazinator.");
            GenericArgumentNames = genericArguments.Select(x => x.Name).ToList();
            ObjectName = iLazinatorType.Name + "<" + string.Join(", ", GenericArgumentNames) + ">";
        }
    }
}
