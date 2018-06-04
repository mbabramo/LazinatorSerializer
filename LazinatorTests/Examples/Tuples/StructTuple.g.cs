//d759f316-3aaa-24f9-72c6-5d8545e2bed5
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.72
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Tuples
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples;
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class StructTuple : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public virtual ILazinator LazinatorParentClass { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual int Deserialize()
        {
            ResetAccessedProperties();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            LazinatorGenericID = GetGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public virtual MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new StructTuple()
            {
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            return clone;
        }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty;
            [DebuggerStepThrough]
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    if (_IsDirty)
                    {
                        InformParentOfDirtiness();
                    }
                }
            }
        }
        
        public virtual InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public virtual void InformParentOfDirtiness()
        {
            if (InformParentOfDirtinessDelegate == null)
            {
                if (LazinatorParentClass != null)
                {
                    LazinatorParentClass.DescendantIsDirty = true;
                }
            }
            else
            {
                InformParentOfDirtinessDelegate();
            }
        }
        
        protected bool _DescendantIsDirty;
        public virtual bool DescendantIsDirty
        {
            [DebuggerStepThrough]
            get => _DescendantIsDirty;
            [DebuggerStepThrough]
            set
            {
                if (_DescendantIsDirty != value)
                {
                    _DescendantIsDirty = value;
                    if (_DescendantIsDirty && LazinatorParentClass != null)
                    {
                        LazinatorParentClass.DescendantIsDirty = true;
                    }
                }
            }
        }
        
        public virtual void MarkHierarchyClean()
        {
            _IsDirty = false;
            _DescendantIsDirty = false;
        }
        
        private MemoryInBuffer _HierarchyBytes;
        public virtual MemoryInBuffer HierarchyBytes
        {
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        protected ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                int length = Deserialize();
                _LazinatorObjectBytes = _LazinatorObjectBytes.Slice(0, length);
            }
        }
        
        public virtual void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty && _LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _LazinatorObjectBytes = bytes.FilledMemory;
        }
        
        public virtual int GetByteLength()
        {
            LazinatorConvertToBytes();
            return _LazinatorObjectBytes.Length;
        }
        
        public virtual uint GetBinaryHashCode32()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public virtual ulong GetBinaryHashCode64()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public virtual Guid GetBinaryHashCode128()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Field definitions */
        
        protected int _EnumTuple_ByteIndex;
        protected int _MyNamedTuple_ByteIndex;
        protected int _MyNullableTuple_ByteIndex;
        protected int _MyValueTupleSerialized_ByteIndex;
        protected virtual int _EnumTuple_ByteLength => _MyNamedTuple_ByteIndex - _EnumTuple_ByteIndex;
        protected virtual int _MyNamedTuple_ByteLength => _MyNullableTuple_ByteIndex - _MyNamedTuple_ByteIndex;
        protected virtual int _MyNullableTuple_ByteLength => _MyValueTupleSerialized_ByteIndex - _MyNullableTuple_ByteIndex;
        private int _StructTuple_EndByteIndex;
        protected virtual int _MyValueTupleSerialized_ByteLength => _StructTuple_EndByteIndex - _MyValueTupleSerialized_ByteIndex;
        
        private (TestEnum firstEnum, TestEnum anotherEnum) _EnumTuple;
        public (TestEnum firstEnum, TestEnum anotherEnum) EnumTuple
        {
            [DebuggerStepThrough]
            get
            {
                if (!_EnumTuple_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _EnumTuple = default((TestEnum firstEnum, TestEnum anotherEnum));
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _EnumTuple_ByteIndex, _EnumTuple_ByteLength, false, false, null);
                        _EnumTuple = ConvertFromBytes__PTestEnum_C32firstEnum_c_C32TestEnum_C32anotherEnum_p(childData, null);
                    }
                    _EnumTuple_Accessed = true;
                    IsDirty = true;
                }
                return _EnumTuple;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _EnumTuple = value;
                _EnumTuple_Accessed = true;
            }
        }
        protected bool _EnumTuple_Accessed;
        private (int MyFirstItem, double MySecondItem) _MyNamedTuple;
        public (int MyFirstItem, double MySecondItem) MyNamedTuple
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyNamedTuple_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyNamedTuple = default((int MyFirstItem, double MySecondItem));
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyNamedTuple_ByteIndex, _MyNamedTuple_ByteLength, false, false, null);
                        _MyNamedTuple = ConvertFromBytes__Pint_C32MyFirstItem_c_C32double_C32MySecondItem_p(childData, null);
                    }
                    _MyNamedTuple_Accessed = true;
                    IsDirty = true;
                }
                return _MyNamedTuple;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyNamedTuple = value;
                _MyNamedTuple_Accessed = true;
            }
        }
        protected bool _MyNamedTuple_Accessed;
        private (int, double)? _MyNullableTuple;
        public (int, double)? MyNullableTuple
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyNullableTuple_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyNullableTuple = default((int, double)?);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyNullableTuple_ByteIndex, _MyNullableTuple_ByteLength, false, false, null);
                        _MyNullableTuple = ConvertFromBytes__Pint_c_C32double_p_C63(childData, null);
                    }
                    _MyNullableTuple_Accessed = true;
                    IsDirty = true;
                }
                return _MyNullableTuple;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyNullableTuple = value;
                _MyNullableTuple_Accessed = true;
            }
        }
        protected bool _MyNullableTuple_Accessed;
        private (uint, ExampleChild, NonLazinatorClass) _MyValueTupleSerialized;
        public (uint, ExampleChild, NonLazinatorClass) MyValueTupleSerialized
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyValueTupleSerialized_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyValueTupleSerialized = default((uint, ExampleChild, NonLazinatorClass));
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyValueTupleSerialized_ByteIndex, _MyValueTupleSerialized_ByteLength, false, false, null);
                        _MyValueTupleSerialized = ConvertFromBytes__Puint_c_C32ExampleChild_c_C32NonLazinatorClass_p(childData, null);
                    }
                    _MyValueTupleSerialized_Accessed = true;
                    IsDirty = true;
                }
                return _MyValueTupleSerialized;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyValueTupleSerialized = value;
                _MyValueTupleSerialized_Accessed = true;
            }
        }
        protected bool _MyValueTupleSerialized_Accessed;
        
        protected virtual void ResetAccessedProperties()
        {
            _EnumTuple_Accessed = _MyNamedTuple_Accessed = _MyNullableTuple_Accessed = _MyValueTupleSerialized_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 229;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        protected virtual System.Collections.Generic.List<int> _LazinatorGenericID { get; set; }
        public virtual System.Collections.Generic.List<int> LazinatorGenericID
        {
            get => null;
            set { }
        }
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;_EnumTuple_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyNamedTuple_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyNullableTuple_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyValueTupleSerialized_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _StructTuple_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, true);
            
            _IsDirty = false;
            _DescendantIsDirty = false;
            
            _LazinatorObjectBytes = writer.Slice(startPosition);
        }
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool includeUniqueID)
        {
            // header information
            if (includeUniqueID)
            {
                if (LazinatorGenericID == null)
                {
                    CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteNonLazinatorObject(
            nonLazinatorObject: _EnumTuple, isBelievedDirty: _EnumTuple_Accessed,
            isAccessed: _EnumTuple_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _EnumTuple_ByteIndex, _EnumTuple_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes__PTestEnum_C32firstEnum_c_C32TestEnum_C32anotherEnum_p(w, EnumTuple,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyNamedTuple, isBelievedDirty: _MyNamedTuple_Accessed,
            isAccessed: _MyNamedTuple_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyNamedTuple_ByteIndex, _MyNamedTuple_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes__Pint_C32MyFirstItem_c_C32double_C32MySecondItem_p(w, MyNamedTuple,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyNullableTuple, isBelievedDirty: _MyNullableTuple_Accessed,
            isAccessed: _MyNullableTuple_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyNullableTuple_ByteIndex, _MyNullableTuple_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes__Pint_c_C32double_p_C63(w, MyNullableTuple,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyValueTupleSerialized, isBelievedDirty: _MyValueTupleSerialized_Accessed,
            isAccessed: _MyValueTupleSerialized_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyValueTupleSerialized_ByteIndex, _MyValueTupleSerialized_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes__Puint_c_C32ExampleChild_c_C32NonLazinatorClass_p(w, MyValueTupleSerialized,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static (TestEnum firstEnum, TestEnum anotherEnum) ConvertFromBytes__PTestEnum_C32firstEnum_c_C32TestEnum_C32anotherEnum_p(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            TestEnum item1 = (TestEnum)span.ToDecompressedInt(ref bytesSoFar);
            
            TestEnum item2 = (TestEnum)span.ToDecompressedInt(ref bytesSoFar);
            
            var tupleType = (item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__PTestEnum_C32firstEnum_c_C32TestEnum_C32anotherEnum_p(BinaryBufferWriter writer, (TestEnum firstEnum, TestEnum anotherEnum) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(writer, (int) itemToConvert.Item1);
            
            CompressedIntegralTypes.WriteCompressedInt(writer, (int) itemToConvert.Item2);
        }
        
        private static (int MyFirstItem, double MySecondItem) ConvertFromBytes__Pint_C32MyFirstItem_c_C32double_C32MySecondItem_p(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            double item2 = span.ToDouble(ref bytesSoFar);
            
            var tupleType = (item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__Pint_C32MyFirstItem_c_C32double_C32MySecondItem_p(BinaryBufferWriter writer, (int MyFirstItem, double MySecondItem) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Item1);
            
            WriteUncompressedPrimitives.WriteDouble(writer, itemToConvert.Item2);
        }
        
        private static (int, double)? ConvertFromBytes__Pint_c_C32double_p_C63(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            double item2 = span.ToDouble(ref bytesSoFar);
            
            var tupleType = (item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__Pint_c_C32double_p_C63(BinaryBufferWriter writer, (int, double)? itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Value.Item1);
            
            WriteUncompressedPrimitives.WriteDouble(writer, itemToConvert.Value.Item2);
        }
        
        private static (uint, ExampleChild, NonLazinatorClass) ConvertFromBytes__Puint_c_C32ExampleChild_c_C32NonLazinatorClass_p(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            uint item1 = span.ToDecompressedUint(ref bytesSoFar);
            
            ExampleChild item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = DeserializationFactory.Instance.CreateBasedOnTypeSpecifyingDelegate<ExampleChild>(childData, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            NonLazinatorClass item3 = default;
            int lengthCollectionMember_item3 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            var tupleType = (item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__Puint_c_C32ExampleChild_c_C32NonLazinatorClass_p(BinaryBufferWriter writer, (uint, ExampleChild, NonLazinatorClass) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            CompressedIntegralTypes.WriteCompressedUint(writer, itemToConvert.Item1);
            
            if (itemToConvert.Item2 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem2(BinaryBufferWriter w) => itemToConvert.Item2.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem2);
            };
            
            if (itemToConvert.Item3 == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionItem3(BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(writer, itemToConvert.Item3, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem3);
            }
        }
        
    }
}
