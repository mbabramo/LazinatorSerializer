//26524464-b46c-8820-acfa-f5095f8ea16d
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.170
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class SpanAndMemory : ILazinator
    {
        public bool IsStruct => false;
        
        /* Serialization, deserialization, and object relationships */
        
        public SpanAndMemory() : base()
        {
        }
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
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
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new SpanAndMemory()
            {
                LazinatorParents = LazinatorParents,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParents = default;
            return clone;
        }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty || _LazinatorObjectBytes.Length == 0;
            [DebuggerStepThrough]
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    if (_IsDirty)
                    {
                        LazinatorParents.InformParentsOfDirtiness();
                        HasChanged = true;
                    }
                }
            }
        }
        
        protected bool _DescendantHasChanged;
        public virtual bool DescendantHasChanged
        {
            [DebuggerStepThrough]
            get => _DescendantHasChanged;
            [DebuggerStepThrough]
            set
            {
                _DescendantHasChanged = value;
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
                    if (_DescendantIsDirty)
                    {
                        LazinatorParents.InformParentsOfDirtiness();
                        _DescendantHasChanged = true;
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
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
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
        
        protected Memory<int> _MyMemoryInt;
        public Memory<int> MyMemoryInt
        {
            get
            {
                if (!_MyMemoryInt_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MyMemoryInt = default(Memory<int>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyMemoryInt_ByteIndex, _MyMemoryInt_ByteLength, false, false, null);
                        _MyMemoryInt = ConvertFromBytes_Memory_Gint_g(childData);
                    }
                    _MyMemoryInt_Accessed = true;
                }
                IsDirty = true; 
                return _MyMemoryInt;
            }
            set
            {IsDirty = true;
                DescendantIsDirty = true;
                _MyMemoryInt = value;
                _MyMemoryInt_Accessed = true;
            }
        }
        protected bool _MyMemoryInt_Accessed;
        protected Memory<int>? _MyNullableMemoryInt;
        public Memory<int>? MyNullableMemoryInt
        {
            get
            {
                if (!_MyNullableMemoryInt_Accessed)
                {
                    if (_LazinatorObjectBytes.Length == 0)
                    {
                        _MyNullableMemoryInt = default(Memory<int>?);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyNullableMemoryInt_ByteIndex, _MyNullableMemoryInt_ByteLength, false, false, null);
                        _MyNullableMemoryInt = ConvertFromBytes_Memory_Gint_g_C63(childData);
                    }
                    _MyNullableMemoryInt_Accessed = true;
                }
                IsDirty = true; 
                return _MyNullableMemoryInt;
            }
            set
            {IsDirty = true;
                DescendantIsDirty = true;
                _MyNullableMemoryInt = value;
                _MyNullableMemoryInt_Accessed = true;
            }
        }
        protected bool _MyNullableMemoryInt_Accessed;
        private ReadOnlyMemory<byte> _MyReadOnlyMemoryByte;
        public ReadOnlyMemory<byte> MyReadOnlyMemoryByte
        {
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
            set
            {
                IsDirty = true;
                _MyReadOnlySpanLong = new ReadOnlyMemory<byte>(MemoryMarshal.Cast<long, byte>(value).ToArray());
                _MyReadOnlySpanLong_Accessed = true;
            }
        }
        protected bool _MyReadOnlySpanLong_Accessed;
        
        public IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            bool match = (matchCriterion == null) ? true : matchCriterion(this);
            bool explore = (!match || !stopExploringBelowMatch) && ((exploreCriterion == null) ? true : exploreCriterion(this));
            if (match)
            {
                yield return this;
            }
            if (explore)
            {
                foreach (var item in EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return item.descendant;
                }
            }
        }
        
        public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyMemoryInt", (object)MyMemoryInt);
            yield return ("MyNullableMemoryInt", (object)MyNullableMemoryInt);
            yield return ("MyReadOnlyMemoryByte", (object)MyReadOnlyMemoryByte);
            yield return ("MyReadOnlySpanByte", (object)MyReadOnlySpanByte.ToString());
            yield return ("MyReadOnlySpanChar", (object)MyReadOnlySpanChar.ToString());
            yield return ("MyReadOnlySpanDateTime", (object)MyReadOnlySpanDateTime.ToString());
            yield return ("MyReadOnlySpanLong", (object)MyReadOnlySpanLong.ToString());
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            _MyMemoryInt_Accessed = _MyNullableMemoryInt_Accessed = _MyReadOnlyMemoryByte_Accessed = _MyReadOnlySpanByte_Accessed = _MyReadOnlySpanChar_Accessed = _MyReadOnlySpanDateTime_Accessed = _MyReadOnlySpanLong_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 228;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        protected virtual LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public virtual LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
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
                if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    _DescendantIsDirty = false;
                }
                else
                {
                    throw new Exception("Cannot update stored buffer when serializing only some children.");
                }
                
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
                if (LazinatorGenericID.IsEmpty)
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
            ConvertToBytes_Memory_Gint_g(w, _MyMemoryInt,
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
            ConvertToBytes_Memory_Gint_g_C63(w, _MyNullableMemoryInt,
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
            ConvertToBytes_ReadOnlyMemory_Gbyte_g(w, _MyReadOnlyMemoryByte,
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
            ConvertToBytes_ReadOnlySpan_Gbyte_g(w, _MyReadOnlySpanByte.Span,
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
            ConvertToBytes_ReadOnlySpan_Gchar_g(w, MemoryMarshal.Cast<byte, char>(_MyReadOnlySpanChar.Span),
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
            ConvertToBytes_ReadOnlySpan_GDateTime_g(w, MemoryMarshal.Cast<byte, DateTime>(_MyReadOnlySpanDateTime.Span),
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
            ConvertToBytes_ReadOnlySpan_Glong_g(w, MemoryMarshal.Cast<byte, long>(_MyReadOnlySpanLong.Span),
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
        
        private static Memory<int> ConvertFromBytes_Memory_Gint_g(ReadOnlyMemory<byte> storage)
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
        
        private static Memory<int>? ConvertFromBytes_Memory_Gint_g_C63(ReadOnlyMemory<byte> storage)
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
