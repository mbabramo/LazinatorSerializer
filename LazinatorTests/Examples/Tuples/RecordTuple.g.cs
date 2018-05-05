//f12e5b7d-69d2-e2c3-8bef-1392505203ec
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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

namespace LazinatorTests.Examples.Tuples
{
    public partial class RecordTuple : ILazinator
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
            var clone = new RecordTuple()
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
            return Farmhash.Hash32(LazinatorObjectBytes.Span);
        }
        
        public virtual ulong GetBinaryHashCode64()
        {
            LazinatorConvertToBytes();
            return Farmhash.Hash64(LazinatorObjectBytes.Span);
        }
        
        /* Field boilerplate */
        
        internal int _MyRecordLikeClass_ByteIndex;
        internal int _MyRecordLikeType_ByteIndex;
        internal int _MyRecordLikeClass_ByteLength => _MyRecordLikeType_ByteIndex - _MyRecordLikeClass_ByteIndex;
        internal int _MyRecordLikeType_ByteLength => LazinatorObjectBytes.Length - _MyRecordLikeType_ByteIndex;
        
        private LazinatorTests.Examples.RecordLikeClass _MyRecordLikeClass;
        public LazinatorTests.Examples.RecordLikeClass MyRecordLikeClass
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyRecordLikeClass_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyRecordLikeClass = default(LazinatorTests.Examples.RecordLikeClass);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyRecordLikeClass_ByteIndex, _MyRecordLikeClass_ByteLength);
                        _MyRecordLikeClass = ConvertFromBytes_LazinatorTests_Examples_RecordLikeClass(childData, DeserializationFactory, null);
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
        internal bool _MyRecordLikeClass_Accessed;
        private LazinatorTests.Examples.RecordLikeType _MyRecordLikeType;
        public LazinatorTests.Examples.RecordLikeType MyRecordLikeType
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyRecordLikeType_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyRecordLikeType = default(LazinatorTests.Examples.RecordLikeType);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyRecordLikeType_ByteIndex, _MyRecordLikeType_ByteLength);
                        _MyRecordLikeType = ConvertFromBytes_LazinatorTests_Examples_RecordLikeType(childData, DeserializationFactory, null);
                    }
                    _MyRecordLikeType_Accessed = true;
                    IsDirty = true;
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
        internal bool _MyRecordLikeType_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 226;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyRecordLikeClass_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyRecordLikeType_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
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
            nonLazinatorObject: _MyRecordLikeClass, isBelievedDirty: _MyRecordLikeClass_Accessed,
            isAccessed: _MyRecordLikeClass_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyRecordLikeClass_ByteIndex, _MyRecordLikeClass_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_LazinatorTests_Examples_RecordLikeClass(w, MyRecordLikeClass,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyRecordLikeType, isBelievedDirty: _MyRecordLikeType_Accessed,
            isAccessed: _MyRecordLikeType_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyRecordLikeType_ByteIndex, _MyRecordLikeType_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_LazinatorTests_Examples_RecordLikeType(w, MyRecordLikeType,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static LazinatorTests.Examples.RecordLikeClass ConvertFromBytes_LazinatorTests_Examples_RecordLikeClass(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            LazinatorTests.Examples.Example item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                if (deserializationFactory == null)
                {
                    throw new MissingDeserializationFactoryException();
                }
                item2 = (LazinatorTests.Examples.Example)deserializationFactory.FactoryCreate(childData, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var tupleType = new LazinatorTests.Examples.RecordLikeClass(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_LazinatorTests_Examples_RecordLikeClass(BinaryBufferWriter writer, LazinatorTests.Examples.RecordLikeClass itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Age);
            
            if (itemToConvert.Example == null)
            {
                writer.Write((uint) 0);
            }
            else
            {
                void actionExample(BinaryBufferWriter w) => itemToConvert.Example.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithUintLengthPrefix(writer, actionExample);
            };
        }
        
        private static LazinatorTests.Examples.RecordLikeType ConvertFromBytes_LazinatorTests_Examples_RecordLikeType(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            string item2 = span.ToString_VarIntLength(ref bytesSoFar);
            
            var tupleType = new LazinatorTests.Examples.RecordLikeType(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_LazinatorTests_Examples_RecordLikeType(BinaryBufferWriter writer, LazinatorTests.Examples.RecordLikeType itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Age);
            
            EncodeCharAndString.WriteStringWithVarIntPrefix(writer, itemToConvert.Name);
        }
        
    }
}
