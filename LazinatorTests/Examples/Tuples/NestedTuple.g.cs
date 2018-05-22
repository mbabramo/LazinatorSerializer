//e1119312-2758-2893-1888-707c5612604f
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.33
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Tuples
{
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
    
    public partial class NestedTuple : ILazinator
    {
        /* Boilerplate for every non-abstract ILazinator object */
        
        public virtual ILazinator LazinatorParentClass { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual void Deserialize()
        {
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return;
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
            var clone = new NestedTuple()
            {
                DeserializationFactory = DeserializationFactory,
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            return clone;
        }
        
        private bool _IsDirty;
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
        
        private bool _DescendantIsDirty;
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
            get => _HierarchyBytes;
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        private ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                Deserialize();
            }
        }
        
        public virtual void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _IsDirty = false;
            LazinatorObjectBytes = bytes.FilledMemory;
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
        
        /* Field boilerplate */
        
        protected int _MyNestedTuple_ByteIndex;
        private int _NestedTuple_EndByteIndex;
        protected virtual int _MyNestedTuple_ByteLength => _NestedTuple_EndByteIndex - _MyNestedTuple_ByteIndex;
        
        private Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass> _MyNestedTuple;
        public Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass> MyNestedTuple
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyNestedTuple_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyNestedTuple = default(Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyNestedTuple_ByteIndex, _MyNestedTuple_ByteLength);
                        _MyNestedTuple = ConvertFromBytes_Tuple_Guint_C63_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(childData, DeserializationFactory, null);
                    }
                    _MyNestedTuple_Accessed = true;
                    IsDirty = true;
                }
                return _MyNestedTuple;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyNestedTuple = value;
                _MyNestedTuple_Accessed = true;
            }
        }
        protected bool _MyNestedTuple_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 224;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyNestedTuple_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _NestedTuple_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyNestedTuple, isBelievedDirty: _MyNestedTuple_Accessed,
            isAccessed: _MyNestedTuple_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyNestedTuple_ByteIndex, _MyNestedTuple_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Tuple_Guint_C63_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(w, MyNestedTuple,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass> ConvertFromBytes_Tuple_Guint_C63_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            uint? item1 = span.ToDecompressedNullableUint(ref bytesSoFar);
            
            (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)) item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = ConvertFromBytes__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p(childData, deserializationFactory, informParentOfDirtinessDelegate);
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
            
            var tupleType = new Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass>(item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_Tuple_Guint_C63_c_C32_PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p_c_C32NonLazinatorClass_g(BinaryBufferWriter writer, Tuple<uint?, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)), NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedNullableUint(writer, itemToConvert.Item1);
            
            void actionItem2(BinaryBufferWriter w) => ConvertToBytes__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p(writer, itemToConvert.Item2, includeChildrenMode, verifyCleanness);
            WriteToBinaryWithIntLengthPrefix(writer, actionItem2);
            
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
        
        private static (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)) ConvertFromBytes__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            ExampleChild item1 = default;
            int lengthCollectionMember_item1 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item1 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item1);
                if (deserializationFactory == null)
                {
                    throw new MissingDeserializationFactoryException();
                }
                item1 = (ExampleChild)deserializationFactory.FactoryCreate(childData, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item1;
            
            (uint, (int a, string b)?, Tuple<short, long>) item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = ConvertFromBytes__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p(childData, deserializationFactory, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var tupleType = (item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__PExampleChild_c_C32_Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p_p(BinaryBufferWriter writer, (ExampleChild, (uint, (int a, string b)?, Tuple<short, long>)) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            if (itemToConvert.Item1 == null)
            {
                writer.Write((int) 0);
            }
            else
            {
                void actionItem1(BinaryBufferWriter w) => itemToConvert.Item1.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem1);
            };
            
            void actionItem2(BinaryBufferWriter w) => ConvertToBytes__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p(writer, itemToConvert.Item2, includeChildrenMode, verifyCleanness);
            WriteToBinaryWithIntLengthPrefix(writer, actionItem2);
        }
        
        private static (uint, (int a, string b)?, Tuple<short, long>) ConvertFromBytes__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            uint item1 = span.ToDecompressedUint(ref bytesSoFar);
            
            (int a, string b)? item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = ConvertFromBytes__Pint_C32a_c_C32string_C32b_p_C63(childData, deserializationFactory, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            Tuple<short, long> item3 = default;
            int lengthCollectionMember_item3 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = ConvertFromBytes_Tuple_Gshort_c_C32long_g(childData, deserializationFactory, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            var tupleType = (item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__Puint_c_C32_Pint_C32a_c_C32string_C32b_p_C63_c_C32Tuple_Gshort_c_C32long_g_p(BinaryBufferWriter writer, (uint, (int a, string b)?, Tuple<short, long>) itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            CompressedIntegralTypes.WriteCompressedUint(writer, itemToConvert.Item1);
            
            if (itemToConvert.Item2 == null)
            {
                writer.Write((uint) 0);
            }
            else
            {
                void actionItem2(BinaryBufferWriter w) => ConvertToBytes__Pint_C32a_c_C32string_C32b_p_C63(writer, itemToConvert.Item2, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem2);
            }
            
            if (itemToConvert.Item3 == null)
            {
                writer.Write((uint) 0);
            }
            else
            {
                void actionItem3(BinaryBufferWriter w) => ConvertToBytes_Tuple_Gshort_c_C32long_g(writer, itemToConvert.Item3, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem3);
            }
        }
        
        private static (int a, string b)? ConvertFromBytes__Pint_C32a_c_C32string_C32b_p_C63(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            string item2 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            
            var tupleType = (item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes__Pint_C32a_c_C32string_C32b_p_C63(BinaryBufferWriter writer, (int a, string b)? itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Value.Item1);
            
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, itemToConvert.Value.Item2);
        }
        
        private static Tuple<short, long> ConvertFromBytes_Tuple_Gshort_c_C32long_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            short item1 = span.ToDecompressedShort(ref bytesSoFar);
            
            long item2 = span.ToDecompressedLong(ref bytesSoFar);
            
            var tupleType = new Tuple<short, long>(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_Tuple_Gshort_c_C32long_g(BinaryBufferWriter writer, Tuple<short, long> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == null)
            {
                return;
            }
            
            CompressedIntegralTypes.WriteCompressedShort(writer, itemToConvert.Item1);
            
            CompressedIntegralTypes.WriteCompressedLong(writer, itemToConvert.Item2);
        }
        
    }
}
