//2e7ac0d0-7901-cf31-c1f0-c4ed43f278bc
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.69
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
    public partial class RegularTuple : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public virtual ILazinator LazinatorParentClass { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual int Deserialize()
        {
            _MyTupleSerialized_Accessed = _MyTupleSerialized2_Accessed = _MyTupleSerialized3_Accessed = _MyTupleSerialized4_Accessed = false;
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
            if (uniqueID != LazinatorUniqueID)
            {
                throw new FormatException("Wrong self-serialized type initialized.");
            }
            
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
            var clone = new RegularTuple()
            {
                DeserializationFactory = DeserializationFactory,
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
        
        public virtual DeserializationFactory DeserializationFactory { get; set; }
        
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
            if (!IsDirty && !DescendantIsDirty)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            LazinatorObjectBytes = bytes.FilledMemory;
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
        
        protected int _MyTupleSerialized_ByteIndex;
        protected int _MyTupleSerialized2_ByteIndex;
        protected int _MyTupleSerialized3_ByteIndex;
        protected int _MyTupleSerialized4_ByteIndex;
        protected virtual int _MyTupleSerialized_ByteLength => _MyTupleSerialized2_ByteIndex - _MyTupleSerialized_ByteIndex;
        protected virtual int _MyTupleSerialized2_ByteLength => _MyTupleSerialized3_ByteIndex - _MyTupleSerialized2_ByteIndex;
        protected virtual int _MyTupleSerialized3_ByteLength => _MyTupleSerialized4_ByteIndex - _MyTupleSerialized3_ByteIndex;
        private int _RegularTuple_EndByteIndex;
        protected virtual int _MyTupleSerialized4_ByteLength => _RegularTuple_EndByteIndex - _MyTupleSerialized4_ByteIndex;
        
        private Tuple<uint, ExampleChild, NonLazinatorClass> _MyTupleSerialized;
        public Tuple<uint, ExampleChild, NonLazinatorClass> MyTupleSerialized
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyTupleSerialized_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyTupleSerialized = default(Tuple<uint, ExampleChild, NonLazinatorClass>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized_ByteIndex, _MyTupleSerialized_ByteLength);
                        _MyTupleSerialized = ConvertFromBytes_Tuple_Guint_c_C32ExampleChild_c_C32NonLazinatorClass_g(childData, DeserializationFactory, null);
                    }
                    _MyTupleSerialized_Accessed = true;
                    IsDirty = true;
                }
                return _MyTupleSerialized;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyTupleSerialized = value;
                _MyTupleSerialized_Accessed = true;
            }
        }
        protected bool _MyTupleSerialized_Accessed;
        private Tuple<uint, ExampleChild, NonLazinatorClass> _MyTupleSerialized2;
        public Tuple<uint, ExampleChild, NonLazinatorClass> MyTupleSerialized2
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyTupleSerialized2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyTupleSerialized2 = default(Tuple<uint, ExampleChild, NonLazinatorClass>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized2_ByteIndex, _MyTupleSerialized2_ByteLength);
                        _MyTupleSerialized2 = ConvertFromBytes_Tuple_Guint_c_C32ExampleChild_c_C32NonLazinatorClass_g(childData, DeserializationFactory, null);
                    }
                    _MyTupleSerialized2_Accessed = true;
                    IsDirty = true;
                }
                return _MyTupleSerialized2;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyTupleSerialized2 = value;
                _MyTupleSerialized2_Accessed = true;
            }
        }
        protected bool _MyTupleSerialized2_Accessed;
        private Tuple<uint?, ExampleChild, NonLazinatorClass> _MyTupleSerialized3;
        public Tuple<uint?, ExampleChild, NonLazinatorClass> MyTupleSerialized3
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyTupleSerialized3_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyTupleSerialized3 = default(Tuple<uint?, ExampleChild, NonLazinatorClass>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized3_ByteIndex, _MyTupleSerialized3_ByteLength);
                        _MyTupleSerialized3 = ConvertFromBytes_Tuple_Guint_C63_c_C32ExampleChild_c_C32NonLazinatorClass_g(childData, DeserializationFactory, null);
                    }
                    _MyTupleSerialized3_Accessed = true;
                    IsDirty = true;
                }
                return _MyTupleSerialized3;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyTupleSerialized3 = value;
                _MyTupleSerialized3_Accessed = true;
            }
        }
        protected bool _MyTupleSerialized3_Accessed;
        private Tuple<int, ExampleStruct> _MyTupleSerialized4;
        public Tuple<int, ExampleStruct> MyTupleSerialized4
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyTupleSerialized4_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyTupleSerialized4 = default(Tuple<int, ExampleStruct>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized4_ByteIndex, _MyTupleSerialized4_ByteLength);
                        _MyTupleSerialized4 = ConvertFromBytes_Tuple_Gint_c_C32ExampleStruct_g(childData, DeserializationFactory, null);
                    }
                    _MyTupleSerialized4_Accessed = true;
                    IsDirty = true;
                }
                return _MyTupleSerialized4;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyTupleSerialized4 = value;
                _MyTupleSerialized4_Accessed = true;
            }
        }
        protected bool _MyTupleSerialized4_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 227;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyTupleSerialized_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyTupleSerialized2_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyTupleSerialized3_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyTupleSerialized4_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _RegularTuple_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            int startPosition = writer.Position;
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTupleSerialized, isBelievedDirty: _MyTupleSerialized_Accessed,
            isAccessed: _MyTupleSerialized_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized_ByteIndex, _MyTupleSerialized_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Tuple_Guint_c_C32ExampleChild_c_C32NonLazinatorClass_g(w, MyTupleSerialized,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTupleSerialized2, isBelievedDirty: _MyTupleSerialized2_Accessed,
            isAccessed: _MyTupleSerialized2_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized2_ByteIndex, _MyTupleSerialized2_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Tuple_Guint_c_C32ExampleChild_c_C32NonLazinatorClass_g(w, MyTupleSerialized2,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTupleSerialized3, isBelievedDirty: _MyTupleSerialized3_Accessed,
            isAccessed: _MyTupleSerialized3_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized3_ByteIndex, _MyTupleSerialized3_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Tuple_Guint_C63_c_C32ExampleChild_c_C32NonLazinatorClass_g(w, MyTupleSerialized3,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTupleSerialized4, isBelievedDirty: _MyTupleSerialized4_Accessed,
            isAccessed: _MyTupleSerialized4_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized4_ByteIndex, _MyTupleSerialized4_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Tuple_Gint_c_C32ExampleStruct_g(w, MyTupleSerialized4,
            includeChildrenMode, v));
            
            _IsDirty = false;
            _DescendantIsDirty = false;
            
            _LazinatorObjectBytes = writer.Slice(startPosition);
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Tuple<uint, ExampleChild, NonLazinatorClass> ConvertFromBytes_Tuple_Guint_c_C32ExampleChild_c_C32NonLazinatorClass_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
                if (deserializationFactory == null)
                {
                    deserializationFactory = DeserializationFactory.GetInstance();
                }
                item2 = (ExampleChild)deserializationFactory.FactoryCreate(childData, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            NonLazinatorClass item3 = default;
            int lengthCollectionMember_item3 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData, deserializationFactory, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            var tupleType = new Tuple<uint, ExampleChild, NonLazinatorClass>(item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_Tuple_Guint_c_C32ExampleChild_c_C32NonLazinatorClass_g(BinaryBufferWriter writer, Tuple<uint, ExampleChild, NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedUint(writer, itemToConvert.Item1);
            
            if (itemToConvert.Item2 == null)
            {
                writer.Write((int) 0);
            }
            else
            {
                void actionItem2(BinaryBufferWriter w) => itemToConvert.Item2.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem2);
            };
            
            if (itemToConvert.Item3 == null)
            {
                writer.Write((uint) 0);
            }
            else
            {
                void actionItem3(BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(writer, itemToConvert.Item3, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem3);
            }
        }
        
        private static Tuple<uint?, ExampleChild, NonLazinatorClass> ConvertFromBytes_Tuple_Guint_C63_c_C32ExampleChild_c_C32NonLazinatorClass_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            uint? item1 = span.ToDecompressedNullableUint(ref bytesSoFar);
            
            ExampleChild item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                if (deserializationFactory == null)
                {
                    deserializationFactory = DeserializationFactory.GetInstance();
                }
                item2 = (ExampleChild)deserializationFactory.FactoryCreate(childData, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            NonLazinatorClass item3 = default;
            int lengthCollectionMember_item3 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData, deserializationFactory, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            var tupleType = new Tuple<uint?, ExampleChild, NonLazinatorClass>(item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_Tuple_Guint_C63_c_C32ExampleChild_c_C32NonLazinatorClass_g(BinaryBufferWriter writer, Tuple<uint?, ExampleChild, NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedNullableUint(writer, itemToConvert.Item1);
            
            if (itemToConvert.Item2 == null)
            {
                writer.Write((int) 0);
            }
            else
            {
                void actionItem2(BinaryBufferWriter w) => itemToConvert.Item2.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem2);
            };
            
            if (itemToConvert.Item3 == null)
            {
                writer.Write((uint) 0);
            }
            else
            {
                void actionItem3(BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(writer, itemToConvert.Item3, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem3);
            }
        }
        
        private static Tuple<int, ExampleStruct> ConvertFromBytes_Tuple_Gint_c_C32ExampleStruct_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            ExampleStruct item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = new ExampleStruct()
                {
                    DeserializationFactory = deserializationFactory,
                    InformParentOfDirtinessDelegate = informParentOfDirtinessDelegate,
                    LazinatorObjectBytes = childData,
                };
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var tupleType = new Tuple<int, ExampleStruct>(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_Tuple_Gint_c_C32ExampleStruct_g(BinaryBufferWriter writer, Tuple<int, ExampleStruct> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Item1);
            
            void actionItem2(BinaryBufferWriter w) => itemToConvert.Item2.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
            WriteToBinaryWithIntLengthPrefix(writer, actionItem2);
        }
        
    }
}
