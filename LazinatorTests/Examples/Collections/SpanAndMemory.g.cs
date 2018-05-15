//6b423365-108f-814a-fc20-f7a218638a53
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.25
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Collections
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
    
    
    public partial class SpanAndMemory : ILazinator
    {
        /* Boilerplate for every non-abstract ILazinator object */
        
        public virtual ILazinator LazinatorParentClass { get; set; }
        
        protected internal IncludeChildrenMode OriginalIncludeChildrenMode;
        
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
        
        protected internal virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new SpanAndMemory()
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
            InformParentOfDirtinessDelegate();
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
        
        internal int _MyMemoryInt_ByteIndex;
        internal int _MyNullableMemoryInt_ByteIndex;
        internal int _MyReadOnlySpanByte_ByteIndex;
        internal int _MyReadOnlySpanChar_ByteIndex;
        internal int _MyReadOnlySpanDateTime_ByteIndex;
        internal int _MyReadOnlySpanLong_ByteIndex;
        internal virtual int _MyMemoryInt_ByteLength => _MyNullableMemoryInt_ByteIndex - _MyMemoryInt_ByteIndex;
        internal virtual int _MyNullableMemoryInt_ByteLength => _MyReadOnlySpanByte_ByteIndex - _MyNullableMemoryInt_ByteIndex;
        internal virtual int _MyReadOnlySpanByte_ByteLength => _MyReadOnlySpanChar_ByteIndex - _MyReadOnlySpanByte_ByteIndex;
        internal virtual int _MyReadOnlySpanChar_ByteLength => _MyReadOnlySpanDateTime_ByteIndex - _MyReadOnlySpanChar_ByteIndex;
        internal virtual int _MyReadOnlySpanDateTime_ByteLength => _MyReadOnlySpanLong_ByteIndex - _MyReadOnlySpanDateTime_ByteIndex;
        private int _SpanAndMemory_EndByteIndex;
        internal virtual int _MyReadOnlySpanLong_ByteLength => _SpanAndMemory_EndByteIndex - _MyReadOnlySpanLong_ByteIndex;
        
        private Memory<int> _MyMemoryInt;
        public Memory<int> MyMemoryInt
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyMemoryInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyMemoryInt = default(Memory<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyMemoryInt_ByteIndex, _MyMemoryInt_ByteLength);
                        _MyMemoryInt = ConvertFromBytes_Memory_int(childData, DeserializationFactory, null);
                    }
                    _MyMemoryInt_Accessed = true;
                    IsDirty = true;
                }
                return _MyMemoryInt;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyMemoryInt = value;
                _MyMemoryInt_Accessed = true;
            }
        }
        internal bool _MyMemoryInt_Accessed;
        private Memory<int>? _MyNullableMemoryInt;
        public Memory<int>? MyNullableMemoryInt
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyNullableMemoryInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyNullableMemoryInt = default(Memory<int>?);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyNullableMemoryInt_ByteIndex, _MyNullableMemoryInt_ByteLength);
                        _MyNullableMemoryInt = ConvertFromBytes_Nullable_Memory_int(childData, DeserializationFactory, null);
                    }
                    _MyNullableMemoryInt_Accessed = true;
                    IsDirty = true;
                }
                return _MyNullableMemoryInt;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyNullableMemoryInt = value;
                _MyNullableMemoryInt_Accessed = true;
            }
        }
        internal bool _MyNullableMemoryInt_Accessed;
        private ReadOnlyMemory<byte> _MyReadOnlySpanByte;
        public ReadOnlySpan<byte> MyReadOnlySpanByte
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyReadOnlySpanByte_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanByte_ByteIndex, _MyReadOnlySpanByte_ByteLength);
                    _MyReadOnlySpanByte = childData;
                    _MyReadOnlySpanByte_Accessed = true;
                }
                return _MyReadOnlySpanByte.Span;
            }
            [DebuggerStepThrough]
            set
            {
                
                IsDirty = true;
                _MyReadOnlySpanByte = new ReadOnlyMemory<byte>(MemoryMarshal.Cast<byte, byte>(value).ToArray());
                _MyReadOnlySpanByte_Accessed = true;
            }
        }
        internal bool _MyReadOnlySpanByte_Accessed;
        private ReadOnlyMemory<byte> _MyReadOnlySpanChar;
        public ReadOnlySpan<char> MyReadOnlySpanChar
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyReadOnlySpanChar_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanChar_ByteIndex, _MyReadOnlySpanChar_ByteLength);
                    _MyReadOnlySpanChar = childData;
                    _MyReadOnlySpanChar_Accessed = true;
                }
                return MemoryMarshal.Cast<byte, char>(_MyReadOnlySpanChar.Span);
            }
            [DebuggerStepThrough]
            set
            {
                
                IsDirty = true;
                _MyReadOnlySpanChar = new ReadOnlyMemory<byte>(MemoryMarshal.Cast<char, byte>(value).ToArray());
                _MyReadOnlySpanChar_Accessed = true;
            }
        }
        internal bool _MyReadOnlySpanChar_Accessed;
        private ReadOnlyMemory<byte> _MyReadOnlySpanDateTime;
        public ReadOnlySpan<DateTime> MyReadOnlySpanDateTime
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyReadOnlySpanDateTime_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanDateTime_ByteIndex, _MyReadOnlySpanDateTime_ByteLength);
                    _MyReadOnlySpanDateTime = childData;
                    _MyReadOnlySpanDateTime_Accessed = true;
                }
                return MemoryMarshal.Cast<byte, DateTime>(_MyReadOnlySpanDateTime.Span);
            }
            [DebuggerStepThrough]
            set
            {
                
                IsDirty = true;
                _MyReadOnlySpanDateTime = new ReadOnlyMemory<byte>(MemoryMarshal.Cast<DateTime, byte>(value).ToArray());
                _MyReadOnlySpanDateTime_Accessed = true;
            }
        }
        internal bool _MyReadOnlySpanDateTime_Accessed;
        private ReadOnlyMemory<byte> _MyReadOnlySpanLong;
        public ReadOnlySpan<long> MyReadOnlySpanLong
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyReadOnlySpanLong_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanLong_ByteIndex, _MyReadOnlySpanLong_ByteLength);
                    _MyReadOnlySpanLong = childData;
                    _MyReadOnlySpanLong_Accessed = true;
                }
                return MemoryMarshal.Cast<byte, long>(_MyReadOnlySpanLong.Span);
            }
            [DebuggerStepThrough]
            set
            {
                
                IsDirty = true;
                _MyReadOnlySpanLong = new ReadOnlyMemory<byte>(MemoryMarshal.Cast<long, byte>(value).ToArray());
                _MyReadOnlySpanLong_Accessed = true;
            }
        }
        internal bool _MyReadOnlySpanLong_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 228;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyMemoryInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyNullableMemoryInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyReadOnlySpanByte_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyReadOnlySpanChar_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyReadOnlySpanDateTime_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyReadOnlySpanLong_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _SpanAndMemory_EndByteIndex = bytesSoFar;
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
            nonLazinatorObject: _MyMemoryInt, isBelievedDirty: _MyMemoryInt_Accessed,
            isAccessed: _MyMemoryInt_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyMemoryInt_ByteIndex, _MyMemoryInt_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Memory_int(w, MyMemoryInt,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyNullableMemoryInt, isBelievedDirty: _MyNullableMemoryInt_Accessed,
            isAccessed: _MyNullableMemoryInt_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyNullableMemoryInt_ByteIndex, _MyNullableMemoryInt_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Nullable_Memory_int(w, MyNullableMemoryInt,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyReadOnlySpanByte, isBelievedDirty: _MyReadOnlySpanByte_Accessed,
            isAccessed: _MyReadOnlySpanByte_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanByte_ByteIndex, _MyReadOnlySpanByte_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlySpan_byte(w, MyReadOnlySpanByte,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyReadOnlySpanChar, isBelievedDirty: _MyReadOnlySpanChar_Accessed,
            isAccessed: _MyReadOnlySpanChar_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanChar_ByteIndex, _MyReadOnlySpanChar_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlySpan_char(w, MyReadOnlySpanChar,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyReadOnlySpanDateTime, isBelievedDirty: _MyReadOnlySpanDateTime_Accessed,
            isAccessed: _MyReadOnlySpanDateTime_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanDateTime_ByteIndex, _MyReadOnlySpanDateTime_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlySpan_DateTime(w, MyReadOnlySpanDateTime,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyReadOnlySpanLong, isBelievedDirty: _MyReadOnlySpanLong_Accessed,
            isAccessed: _MyReadOnlySpanLong_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanLong_ByteIndex, _MyReadOnlySpanLong_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlySpan_long(w, MyReadOnlySpanLong,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Memory<int> ConvertFromBytes_Memory_int(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(Memory<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            Memory<int> collection = new Memory<int>(new int[collectionLength]);
            var collectionAsSpan = collection.Span;
            for (int i = 0; i < collectionLength; i++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collectionAsSpan[i] = item;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_Memory_int(BinaryBufferWriter writer, Memory<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Length);
            var itemToConvertSpan = itemToConvert.Span;
            int itemToConvertCount = itemToConvertSpan.Length;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvertSpan[itemIndex]);
            }
        }
        
        private static Memory<int>? ConvertFromBytes_Nullable_Memory_int(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(Memory<int>?);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            Memory<int> collection = new Memory<int>(new int[collectionLength]);
            var collectionAsSpan = collection.Span;
            for (int i = 0; i < collectionLength; i++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collectionAsSpan[i] = item;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_Nullable_Memory_int(BinaryBufferWriter writer, Memory<int>? itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                ConvertToBytes_Memory_int(writer, itemToConvert.Value, includeChildrenMode, verifyCleanness);
            }
        }
        
        private static void ConvertToBytes_ReadOnlySpan_byte(BinaryBufferWriter writer, ReadOnlySpan<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<byte, byte>(itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        
        private static void ConvertToBytes_ReadOnlySpan_char(BinaryBufferWriter writer, ReadOnlySpan<char> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<char, byte>(itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        
        private static void ConvertToBytes_ReadOnlySpan_DateTime(BinaryBufferWriter writer, ReadOnlySpan<DateTime> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<DateTime, byte>(itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        
        private static void ConvertToBytes_ReadOnlySpan_long(BinaryBufferWriter writer, ReadOnlySpan<long> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<long, byte>(itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        
    }
}
