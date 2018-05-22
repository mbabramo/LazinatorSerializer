//6dce65cf-d436-c6b0-80a3-51a35bd88df1
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
    
    public partial class RecordLikeContainer : ILazinator
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
            var clone = new RecordLikeContainer()
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyMismatchedRecordLikeType_ByteIndex, _MyMismatchedRecordLikeType_ByteLength);
                        _MyMismatchedRecordLikeType = ConvertFromBytes_MismatchedRecordLikeType(childData, DeserializationFactory, null);
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyRecordLikeClass_ByteIndex, _MyRecordLikeClass_ByteLength);
                        _MyRecordLikeClass = ConvertFromBytes_RecordLikeClass(childData, DeserializationFactory, null);
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyRecordLikeType_ByteIndex, _MyRecordLikeType_ByteLength);
                        _MyRecordLikeType = ConvertFromBytes_RecordLikeType(childData, DeserializationFactory, null);
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
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 226;
        
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
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyMismatchedRecordLikeType, isBelievedDirty: _MyMismatchedRecordLikeType_Accessed,
            isAccessed: _MyMismatchedRecordLikeType_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyMismatchedRecordLikeType_ByteIndex, _MyMismatchedRecordLikeType_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_MismatchedRecordLikeType(w, MyMismatchedRecordLikeType,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyRecordLikeClass, isBelievedDirty: _MyRecordLikeClass_Accessed,
            isAccessed: _MyRecordLikeClass_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyRecordLikeClass_ByteIndex, _MyRecordLikeClass_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_RecordLikeClass(w, MyRecordLikeClass,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyRecordLikeType, isBelievedDirty: _MyRecordLikeType_Accessed,
            isAccessed: _MyRecordLikeType_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyRecordLikeType_ByteIndex, _MyRecordLikeType_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_RecordLikeType(w, MyRecordLikeType,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static MismatchedRecordLikeType ConvertFromBytes_MismatchedRecordLikeType(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
        
        private static RecordLikeClass ConvertFromBytes_RecordLikeClass(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
                if (deserializationFactory == null)
                {
                    throw new MissingDeserializationFactoryException();
                }
                item2 = (Example)deserializationFactory.FactoryCreate(childData, informParentOfDirtinessDelegate);
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
                writer.Write((int) 0);
            }
            else
            {
                void actionExample(BinaryBufferWriter w) => itemToConvert.Example.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionExample);
            };
        }
        
        private static RecordLikeType ConvertFromBytes_RecordLikeType(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
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
