//2bef0d08-aaa4-14de-6ea7-9c300a3f3e3d
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
    public partial class RecordLikeContainer : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public RecordLikeContainer() : base()
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
            
            GetGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
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
            var clone = new RecordLikeContainer()
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
        
        protected int _MyMismatchedRecordLikeType_ByteIndex;
        protected int _MyRecordLikeClass_ByteIndex;
        protected int _MyRecordLikeType_ByteIndex;
        protected virtual int _MyMismatchedRecordLikeType_ByteLength => _MyRecordLikeClass_ByteIndex - _MyMismatchedRecordLikeType_ByteIndex;
        protected virtual int _MyRecordLikeClass_ByteLength => _MyRecordLikeType_ByteIndex - _MyRecordLikeClass_ByteIndex;
        private int _RecordLikeContainer_EndByteIndex;
        protected virtual int _MyRecordLikeType_ByteLength => _RecordLikeContainer_EndByteIndex - _MyRecordLikeType_ByteIndex;
        
        private MismatchedRecordLikeType _MyMismatchedRecordLikeType;
        public MismatchedRecordLikeType MyMismatchedRecordLikeType
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyMismatchedRecordLikeType_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyMismatchedRecordLikeType = default(MismatchedRecordLikeType);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyMismatchedRecordLikeType_ByteIndex, _MyMismatchedRecordLikeType_ByteLength, false, false, null);
                        _MyMismatchedRecordLikeType = ConvertFromBytes_MismatchedRecordLikeType(childData, null);
                    }
                    _MyMismatchedRecordLikeType_Accessed = true;
                }
                return _MyMismatchedRecordLikeType;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyMismatchedRecordLikeType = value;
                _MyMismatchedRecordLikeType_Accessed = true;
            }
        }
        protected bool _MyMismatchedRecordLikeType_Accessed;
        private RecordLikeClass _MyRecordLikeClass;
        public RecordLikeClass MyRecordLikeClass
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyRecordLikeClass_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyRecordLikeClass = default(RecordLikeClass);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyRecordLikeClass_ByteIndex, _MyRecordLikeClass_ByteLength, false, false, null);
                        _MyRecordLikeClass = ConvertFromBytes_RecordLikeClass(childData, null);
                    }
                    _MyRecordLikeClass_Accessed = true;
                    IsDirty = true;
                }
                return _MyRecordLikeClass;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyRecordLikeClass = value;
                _MyRecordLikeClass_Accessed = true;
            }
        }
        protected bool _MyRecordLikeClass_Accessed;
        private RecordLikeType _MyRecordLikeType;
        public RecordLikeType MyRecordLikeType
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyRecordLikeType_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyRecordLikeType = default(RecordLikeType);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyRecordLikeType_ByteIndex, _MyRecordLikeType_ByteLength, false, false, null);
                        _MyRecordLikeType = ConvertFromBytes_RecordLikeType(childData, null);
                    }
                    _MyRecordLikeType_Accessed = true;
                }
                return _MyRecordLikeType;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyRecordLikeType = value;
                _MyRecordLikeType_Accessed = true;
            }
        }
        protected bool _MyRecordLikeType_Accessed;
        
        protected virtual void ResetAccessedProperties()
        {
            _MyMismatchedRecordLikeType_Accessed = _MyRecordLikeClass_Accessed = _MyRecordLikeType_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 226;
        
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
            _MyMismatchedRecordLikeType_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyRecordLikeClass_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyRecordLikeType_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _RecordLikeContainer_EndByteIndex = bytesSoFar;
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
            nonLazinatorObject: _MyMismatchedRecordLikeType, isBelievedDirty: _MyMismatchedRecordLikeType_Accessed,
            isAccessed: _MyMismatchedRecordLikeType_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyMismatchedRecordLikeType_ByteIndex, _MyMismatchedRecordLikeType_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_MismatchedRecordLikeType(w, MyMismatchedRecordLikeType,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyRecordLikeClass, isBelievedDirty: _MyRecordLikeClass_Accessed,
            isAccessed: _MyRecordLikeClass_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyRecordLikeClass_ByteIndex, _MyRecordLikeClass_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_RecordLikeClass(w, MyRecordLikeClass,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyRecordLikeType, isBelievedDirty: _MyRecordLikeType_Accessed,
            isAccessed: _MyRecordLikeType_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyRecordLikeType_ByteIndex, _MyRecordLikeType_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_RecordLikeType(w, MyRecordLikeType,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static MismatchedRecordLikeType ConvertFromBytes_MismatchedRecordLikeType(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            string item2 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            
            var tupleType = new MismatchedRecordLikeType(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_MismatchedRecordLikeType(BinaryBufferWriter writer, MismatchedRecordLikeType itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Age);
            
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, itemToConvert.Name);
        }
        
        private static RecordLikeClass ConvertFromBytes_RecordLikeClass(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            Example item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = DeserializationFactory.Instance.CreateBasedOnTypeSpecifyingDelegate<Example>(childData, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var tupleType = new RecordLikeClass(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_RecordLikeClass(BinaryBufferWriter writer, RecordLikeClass itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Age);
            
            if (itemToConvert.Example == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionExample(BinaryBufferWriter w) => itemToConvert.Example.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionExample);
            };
        }
        
        private static RecordLikeType ConvertFromBytes_RecordLikeType(ReadOnlyMemory<byte> storage, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            string item2 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            
            var tupleType = new RecordLikeType(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_RecordLikeType(BinaryBufferWriter writer, RecordLikeType itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Age);
            
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, itemToConvert.Name);
        }
        
    }
}
