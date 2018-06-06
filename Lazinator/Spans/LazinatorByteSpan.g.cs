//ef20f21d-4d01-9623-34a7-aad6eff6a44b
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.76
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Spans
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
    public partial class LazinatorByteSpan : ILazinator
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
            PostDeserialization();
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
            var clone = new LazinatorByteSpan()
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
        
        protected int _ReadOnly_ByteIndex;
        protected int _ReadOrWrite_ByteIndex;
        protected virtual int _ReadOnly_ByteLength => _ReadOrWrite_ByteIndex - _ReadOnly_ByteIndex;
        private int _LazinatorByteSpan_EndByteIndex;
        protected virtual int _ReadOrWrite_ByteLength => _LazinatorByteSpan_EndByteIndex - _ReadOrWrite_ByteIndex;
        
        private ReadOnlyMemory<byte> _ReadOnly;
        internal ReadOnlySpan<byte> ReadOnly
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ReadOnly_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ReadOnly_ByteIndex, _ReadOnly_ByteLength, false, false, null);
                    _ReadOnly = childData;
                    _ReadOnly_Accessed = true;
                }
                return _ReadOnly.Span;
            }
            [DebuggerStepThrough]
            set
            {
                
                IsDirty = true;
                _ReadOnly = new ReadOnlyMemory<byte>(MemoryMarshal.Cast<byte, byte>(value).ToArray());
                _ReadOnly_Accessed = true;
            }
        }
        protected bool _ReadOnly_Accessed;
        private Memory<byte> _ReadOrWrite;
        internal Memory<byte> ReadOrWrite
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ReadOrWrite_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ReadOrWrite = default(Memory<byte>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ReadOrWrite_ByteIndex, _ReadOrWrite_ByteLength, false, false, null);
                        _ReadOrWrite = ConvertFromBytes_Memory_Gbyte_g(childData, null);
                    }
                    _ReadOrWrite_Accessed = true;
                }
                IsDirty = true;
                return _ReadOrWrite;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _ReadOrWrite = value;
                _ReadOrWrite_Accessed = true;
            }
        }
        protected bool _ReadOrWrite_Accessed;
        
        protected virtual void ResetAccessedProperties()
        {
            _ReadOnly_Accessed = _ReadOrWrite_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 89;
        
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
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;_ReadOnly_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _ReadOrWrite_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _LazinatorByteSpan_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            PreSerialization();
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
            nonLazinatorObject: _ReadOnly, isBelievedDirty: _ReadOnly_Accessed,
            isAccessed: _ReadOnly_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _ReadOnly_ByteIndex, _ReadOnly_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlySpan_Gbyte_g(w, ReadOnly,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _ReadOrWrite, isBelievedDirty: _ReadOrWrite_Accessed,
            isAccessed: _ReadOrWrite_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _ReadOrWrite_ByteIndex, _ReadOrWrite_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Memory_Gbyte_g(w, ReadOrWrite,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static void ConvertToBytes_ReadOnlySpan_Gbyte_g(BinaryBufferWriter writer, ReadOnlySpan<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<byte, byte>(itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        
        private static Memory<byte> ConvertFromBytes_Memory_Gbyte_g(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(Memory<byte>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            Memory<byte> collection = new Memory<byte>(new byte[collectionLength]);
            var collectionAsSpan = collection.Span;
            for (int i = 0; i < collectionLength; i++)
            {
                byte item = span.ToByte(ref bytesSoFar);
                collectionAsSpan[i] = item;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_Memory_Gbyte_g(BinaryBufferWriter writer, Memory<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Length);
            var itemToConvertSpan = itemToConvert.Span;
            int itemToConvertCount = itemToConvertSpan.Length;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                WriteUncompressedPrimitives.WriteByte(writer, itemToConvertSpan[itemIndex]);
            }
        }
        
    }
}
