//2fdfd85a-0359-71f6-1d04-ee636b14967b
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.31
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Tuples
{
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
    
    
    public partial class RegularTuple : ILazinator
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
        
        protected int _MyTupleSerialized_ByteIndex;
        protected int _MyTupleSerialized2_ByteIndex;
        protected int _MyTupleSerialized3_ByteIndex;
        protected int _MyTupleSerialized4_ByteIndex;
        protected virtual int _MyTupleSerialized_ByteLength => _MyTupleSerialized2_ByteIndex - _MyTupleSerialized_ByteIndex;
        protected virtual int _MyTupleSerialized2_ByteLength => _MyTupleSerialized3_ByteIndex - _MyTupleSerialized2_ByteIndex;
        protected virtual int _MyTupleSerialized3_ByteLength => _MyTupleSerialized4_ByteIndex - _MyTupleSerialized3_ByteIndex;
        private int _RegularTuple_EndByteIndex;
        protected virtual int _MyTupleSerialized4_ByteLength => _RegularTuple_EndByteIndex - _MyTupleSerialized4_ByteIndex;
        
        private global::System.Tuple<uint, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass> _MyTupleSerialized;
        public global::System.Tuple<uint, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass> MyTupleSerialized
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyTupleSerialized_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyTupleSerialized = default(global::System.Tuple<uint, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized_ByteIndex, _MyTupleSerialized_ByteLength);
                        _MyTupleSerialized = ConvertFromBytes_System__Tuple_Guint_c_C32global_C58_C58LazinatorTests__Examples__ExampleChild_c_C32global_C58_C58LazinatorTests__Examples__NonLazinatorClass_g(childData, DeserializationFactory, null);
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
        private global::System.Tuple<uint, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass> _MyTupleSerialized2;
        public global::System.Tuple<uint, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass> MyTupleSerialized2
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyTupleSerialized2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyTupleSerialized2 = default(global::System.Tuple<uint, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized2_ByteIndex, _MyTupleSerialized2_ByteLength);
                        _MyTupleSerialized2 = ConvertFromBytes_System__Tuple_Guint_c_C32global_C58_C58LazinatorTests__Examples__ExampleChild_c_C32global_C58_C58LazinatorTests__Examples__NonLazinatorClass_g(childData, DeserializationFactory, null);
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
        private global::System.Tuple<uint?, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass> _MyTupleSerialized3;
        public global::System.Tuple<uint?, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass> MyTupleSerialized3
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyTupleSerialized3_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyTupleSerialized3 = default(global::System.Tuple<uint?, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized3_ByteIndex, _MyTupleSerialized3_ByteLength);
                        _MyTupleSerialized3 = ConvertFromBytes_System__Tuple_Guint_C63_c_C32global_C58_C58LazinatorTests__Examples__ExampleChild_c_C32global_C58_C58LazinatorTests__Examples__NonLazinatorClass_g(childData, DeserializationFactory, null);
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
        private global::System.Tuple<int, global::LazinatorTests.Examples.ExampleStruct> _MyTupleSerialized4;
        public global::System.Tuple<int, global::LazinatorTests.Examples.ExampleStruct> MyTupleSerialized4
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyTupleSerialized4_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyTupleSerialized4 = default(global::System.Tuple<int, global::LazinatorTests.Examples.ExampleStruct>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized4_ByteIndex, _MyTupleSerialized4_ByteLength);
                        _MyTupleSerialized4 = ConvertFromBytes_System__Tuple_Gint_c_C32global_C58_C58LazinatorTests__Examples__ExampleStruct_g(childData, DeserializationFactory, null);
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
            ConvertToBytes_System__Tuple_Guint_c_C32global_C58_C58LazinatorTests__Examples__ExampleChild_c_C32global_C58_C58LazinatorTests__Examples__NonLazinatorClass_g(w, MyTupleSerialized,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTupleSerialized2, isBelievedDirty: _MyTupleSerialized2_Accessed,
            isAccessed: _MyTupleSerialized2_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized2_ByteIndex, _MyTupleSerialized2_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System__Tuple_Guint_c_C32global_C58_C58LazinatorTests__Examples__ExampleChild_c_C32global_C58_C58LazinatorTests__Examples__NonLazinatorClass_g(w, MyTupleSerialized2,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTupleSerialized3, isBelievedDirty: _MyTupleSerialized3_Accessed,
            isAccessed: _MyTupleSerialized3_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized3_ByteIndex, _MyTupleSerialized3_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System__Tuple_Guint_C63_c_C32global_C58_C58LazinatorTests__Examples__ExampleChild_c_C32global_C58_C58LazinatorTests__Examples__NonLazinatorClass_g(w, MyTupleSerialized3,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyTupleSerialized4, isBelievedDirty: _MyTupleSerialized4_Accessed,
            isAccessed: _MyTupleSerialized4_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyTupleSerialized4_ByteIndex, _MyTupleSerialized4_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_System__Tuple_Gint_c_C32global_C58_C58LazinatorTests__Examples__ExampleStruct_g(w, MyTupleSerialized4,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static global::System.Tuple<uint, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass> ConvertFromBytes_System__Tuple_Guint_c_C32global_C58_C58LazinatorTests__Examples__ExampleChild_c_C32global_C58_C58LazinatorTests__Examples__NonLazinatorClass_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            uint item1 = span.ToDecompressedUint(ref bytesSoFar);
            
            global::LazinatorTests.Examples.ExampleChild item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                if (deserializationFactory == null)
                {
                    throw new MissingDeserializationFactoryException();
                }
                item2 = (global::LazinatorTests.Examples.ExampleChild)deserializationFactory.FactoryCreate(childData, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            global::LazinatorTests.Examples.NonLazinatorClass item3 = default;
            int lengthCollectionMember_item3 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = LazinatorTests.Examples.NonLazinatorDirectConverter.ConvertFromBytes_LazinatorTests__Examples__NonLazinatorClass(childData, deserializationFactory, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            var tupleType = new global::System.Tuple<uint, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass>(item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_System__Tuple_Guint_c_C32global_C58_C58LazinatorTests__Examples__ExampleChild_c_C32global_C58_C58LazinatorTests__Examples__NonLazinatorClass_g(BinaryBufferWriter writer, global::System.Tuple<uint, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
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
                void actionItem3(BinaryBufferWriter w) => LazinatorTests.Examples.NonLazinatorDirectConverter.ConvertToBytes_LazinatorTests__Examples__NonLazinatorClass(writer, itemToConvert.Item3, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem3);
            }
        }
        
        private static global::System.Tuple<uint?, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass> ConvertFromBytes_System__Tuple_Guint_C63_c_C32global_C58_C58LazinatorTests__Examples__ExampleChild_c_C32global_C58_C58LazinatorTests__Examples__NonLazinatorClass_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            uint? item1 = span.ToDecompressedNullableUint(ref bytesSoFar);
            
            global::LazinatorTests.Examples.ExampleChild item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                if (deserializationFactory == null)
                {
                    throw new MissingDeserializationFactoryException();
                }
                item2 = (global::LazinatorTests.Examples.ExampleChild)deserializationFactory.FactoryCreate(childData, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            global::LazinatorTests.Examples.NonLazinatorClass item3 = default;
            int lengthCollectionMember_item3 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = LazinatorTests.Examples.NonLazinatorDirectConverter.ConvertFromBytes_LazinatorTests__Examples__NonLazinatorClass(childData, deserializationFactory, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            var tupleType = new global::System.Tuple<uint?, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass>(item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_System__Tuple_Guint_C63_c_C32global_C58_C58LazinatorTests__Examples__ExampleChild_c_C32global_C58_C58LazinatorTests__Examples__NonLazinatorClass_g(BinaryBufferWriter writer, global::System.Tuple<uint?, global::LazinatorTests.Examples.ExampleChild, global::LazinatorTests.Examples.NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
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
                void actionItem3(BinaryBufferWriter w) => LazinatorTests.Examples.NonLazinatorDirectConverter.ConvertToBytes_LazinatorTests__Examples__NonLazinatorClass(writer, itemToConvert.Item3, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionItem3);
            }
        }
        
        private static global::System.Tuple<int, global::LazinatorTests.Examples.ExampleStruct> ConvertFromBytes_System__Tuple_Gint_c_C32global_C58_C58LazinatorTests__Examples__ExampleStruct_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            global::LazinatorTests.Examples.ExampleStruct item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = new global::LazinatorTests.Examples.ExampleStruct()
                {
                    DeserializationFactory = deserializationFactory,
                    InformParentOfDirtinessDelegate = informParentOfDirtinessDelegate,
                    LazinatorObjectBytes = childData,
                };
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var tupleType = new global::System.Tuple<int, global::LazinatorTests.Examples.ExampleStruct>(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_System__Tuple_Gint_c_C32global_C58_C58LazinatorTests__Examples__ExampleStruct_g(BinaryBufferWriter writer, global::System.Tuple<int, global::LazinatorTests.Examples.ExampleStruct> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
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
