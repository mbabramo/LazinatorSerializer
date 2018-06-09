//f8a80318-edd8-757b-e313-4d4a30dbf424
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.81
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Collections
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class SpanAndMemory : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public SpanAndMemory() : base()
        {
        }
        
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
            var clone = new SpanAndMemory()
            {
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
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
        
        /* Property definitions */
        
        protected int _MyMemoryInt_ByteIndex;
        protected int _MyNullableMemoryInt_ByteIndex;
        protected int _MyReadOnlyMemoryByte_ByteIndex;
        protected int _MyReadOnlySpanByte_ByteIndex;
        protected int _MyReadOnlySpanChar_ByteIndex;
        protected int _MyReadOnlySpanDateTime_ByteIndex;
        protected int _MyReadOnlySpanLong_ByteIndex;
        protected virtual int _MyMemoryInt_ByteLength => _MyNullableMemoryInt_ByteIndex - _MyMemoryInt_ByteIndex;
        protected virtual int _MyNullableMemoryInt_ByteLength => _MyReadOnlyMemoryByte_ByteIndex - _MyNullableMemoryInt_ByteIndex;
        protected virtual int _MyReadOnlyMemoryByte_ByteLength => _MyReadOnlySpanByte_ByteIndex - _MyReadOnlyMemoryByte_ByteIndex;
        protected virtual int _MyReadOnlySpanByte_ByteLength => _MyReadOnlySpanChar_ByteIndex - _MyReadOnlySpanByte_ByteIndex;
        protected virtual int _MyReadOnlySpanChar_ByteLength => _MyReadOnlySpanDateTime_ByteIndex - _MyReadOnlySpanChar_ByteIndex;
        protected virtual int _MyReadOnlySpanDateTime_ByteLength => _MyReadOnlySpanLong_ByteIndex - _MyReadOnlySpanDateTime_ByteIndex;
        private int _SpanAndMemory_EndByteIndex;
        protected virtual int _MyReadOnlySpanLong_ByteLength => _SpanAndMemory_EndByteIndex - _MyReadOnlySpanLong_ByteIndex;
        
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyMemoryInt_ByteIndex, _MyMemoryInt_ByteLength, false, false, null);
                        _MyMemoryInt = ConvertFromBytes_Memory_Gint_g(childData, null);
                    }
                    _MyMemoryInt_Accessed = true;
                }
                IsDirty = true;
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
        protected bool _MyMemoryInt_Accessed;
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyNullableMemoryInt_ByteIndex, _MyNullableMemoryInt_ByteLength, false, false, null);
                        _MyNullableMemoryInt = ConvertFromBytes_Memory_Gint_g_C63(childData, null);
                    }
                    _MyNullableMemoryInt_Accessed = true;
                }
                IsDirty = true;
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
        protected bool _MyNullableMemoryInt_Accessed;
        private ReadOnlyMemory<byte> _MyReadOnlyMemoryByte;
        public ReadOnlyMemory<byte> MyReadOnlyMemoryByte
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyReadOnlyMemoryByte_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyReadOnlyMemoryByte_ByteIndex, _MyReadOnlyMemoryByte_ByteLength, false, false, null);
                    _MyReadOnlyMemoryByte = childData;
                    _MyReadOnlyMemoryByte_Accessed = true;
                }
                return _MyReadOnlyMemoryByte;
            }
            [DebuggerStepThrough]
            set
            {
                
                IsDirty = true;
                _MyReadOnlyMemoryByte = value;
                _MyReadOnlyMemoryByte_Accessed = true;
            }
        }
        protected bool _MyReadOnlyMemoryByte_Accessed;
        private ReadOnlyMemory<byte> _MyReadOnlySpanByte;
        public ReadOnlySpan<byte> MyReadOnlySpanByte
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyReadOnlySpanByte_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanByte_ByteIndex, _MyReadOnlySpanByte_ByteLength, false, false, null);
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
        protected bool _MyReadOnlySpanByte_Accessed;
        private ReadOnlyMemory<byte> _MyReadOnlySpanChar;
        public ReadOnlySpan<char> MyReadOnlySpanChar
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyReadOnlySpanChar_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanChar_ByteIndex, _MyReadOnlySpanChar_ByteLength, false, false, null);
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
        protected bool _MyReadOnlySpanChar_Accessed;
        private ReadOnlyMemory<byte> _MyReadOnlySpanDateTime;
        public ReadOnlySpan<DateTime> MyReadOnlySpanDateTime
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyReadOnlySpanDateTime_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanDateTime_ByteIndex, _MyReadOnlySpanDateTime_ByteLength, false, false, null);
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
        protected bool _MyReadOnlySpanDateTime_Accessed;
        private ReadOnlyMemory<byte> _MyReadOnlySpanLong;
        public ReadOnlySpan<long> MyReadOnlySpanLong
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyReadOnlySpanLong_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanLong_ByteIndex, _MyReadOnlySpanLong_ByteLength, false, false, null);
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
        protected bool _MyReadOnlySpanLong_Accessed;
        
        protected virtual void ResetAccessedProperties()
        {
            _MyMemoryInt_Accessed = _MyNullableMemoryInt_Accessed = _MyReadOnlyMemoryByte_Accessed = _MyReadOnlySpanByte_Accessed = _MyReadOnlySpanChar_Accessed = _MyReadOnlySpanDateTime_Accessed = _MyReadOnlySpanLong_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 228;
        
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
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyMemoryInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyNullableMemoryInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyReadOnlyMemoryByte_ByteIndex = bytesSoFar;
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
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                
                _IsDirty = false;
                _DescendantIsDirty = false;
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
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
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyMemoryInt, isBelievedDirty: _MyMemoryInt_Accessed,
            isAccessed: _MyMemoryInt_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyMemoryInt_ByteIndex, _MyMemoryInt_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Memory_Gint_g(w, MyMemoryInt,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyMemoryInt_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyNullableMemoryInt, isBelievedDirty: _MyNullableMemoryInt_Accessed,
            isAccessed: _MyNullableMemoryInt_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyNullableMemoryInt_ByteIndex, _MyNullableMemoryInt_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Memory_Gint_g_C63(w, MyNullableMemoryInt,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyNullableMemoryInt_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyReadOnlyMemoryByte, isBelievedDirty: _MyReadOnlyMemoryByte_Accessed,
            isAccessed: _MyReadOnlyMemoryByte_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyReadOnlyMemoryByte_ByteIndex, _MyReadOnlyMemoryByte_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlyMemory_Gbyte_g(w, MyReadOnlyMemoryByte,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyReadOnlyMemoryByte_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyReadOnlySpanByte, isBelievedDirty: _MyReadOnlySpanByte_Accessed,
            isAccessed: _MyReadOnlySpanByte_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanByte_ByteIndex, _MyReadOnlySpanByte_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlySpan_Gbyte_g(w, MyReadOnlySpanByte,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyReadOnlySpanByte_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyReadOnlySpanChar, isBelievedDirty: _MyReadOnlySpanChar_Accessed,
            isAccessed: _MyReadOnlySpanChar_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanChar_ByteIndex, _MyReadOnlySpanChar_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlySpan_Gchar_g(w, MyReadOnlySpanChar,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyReadOnlySpanChar_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyReadOnlySpanDateTime, isBelievedDirty: _MyReadOnlySpanDateTime_Accessed,
            isAccessed: _MyReadOnlySpanDateTime_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanDateTime_ByteIndex, _MyReadOnlySpanDateTime_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlySpan_GDateTime_g(w, MyReadOnlySpanDateTime,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyReadOnlySpanDateTime_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyReadOnlySpanLong, isBelievedDirty: _MyReadOnlySpanLong_Accessed,
            isAccessed: _MyReadOnlySpanLong_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyReadOnlySpanLong_ByteIndex, _MyReadOnlySpanLong_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlySpan_Glong_g(w, MyReadOnlySpanLong,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyReadOnlySpanLong_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _SpanAndMemory_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Memory<int> ConvertFromBytes_Memory_Gint_g(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
        
        private static void ConvertToBytes_Memory_Gint_g(BinaryBufferWriter writer, Memory<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Length);
            var itemToConvertSpan = itemToConvert.Span;
            int itemToConvertCount = itemToConvertSpan.Length;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvertSpan[itemIndex]);
            }
        }
        
        private static Memory<int>? ConvertFromBytes_Memory_Gint_g_C63(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
        
        private static void ConvertToBytes_Memory_Gint_g_C63(BinaryBufferWriter writer, Memory<int>? itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                ConvertToBytes_Memory_Gint_g(writer, itemToConvert.Value, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            }
        }
        
        private static void ConvertToBytes_ReadOnlyMemory_Gbyte_g(BinaryBufferWriter writer, ReadOnlyMemory<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<byte, byte>(itemToConvert.Span);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        
        private static void ConvertToBytes_ReadOnlySpan_Gbyte_g(BinaryBufferWriter writer, ReadOnlySpan<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<byte, byte>(itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        
        private static void ConvertToBytes_ReadOnlySpan_Gchar_g(BinaryBufferWriter writer, ReadOnlySpan<char> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<char, byte>(itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        
        private static void ConvertToBytes_ReadOnlySpan_GDateTime_g(BinaryBufferWriter writer, ReadOnlySpan<DateTime> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<DateTime, byte>(itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        
        private static void ConvertToBytes_ReadOnlySpan_Glong_g(BinaryBufferWriter writer, ReadOnlySpan<long> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<long, byte>(itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        
    }
}
