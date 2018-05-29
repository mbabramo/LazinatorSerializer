//ed4d7889-1e00-3b3a-bbda-1838b3fb20ba
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.61
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
    using LazinatorTests.Examples;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class DotNetList_NonSelfSerializable : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
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
            var clone = new DotNetList_NonSelfSerializable()
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
            _MyListNonLazinatorType_Dirty = false;
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
        
        public virtual Guid GetBinaryHashCode128()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Field definitions */
        
        protected int _MyListNonLazinatorType_ByteIndex;
        protected int _MyListNonLazinatorType2_ByteIndex;
        protected virtual int _MyListNonLazinatorType_ByteLength => _MyListNonLazinatorType2_ByteIndex - _MyListNonLazinatorType_ByteIndex;
        private int _DotNetList_NonSelfSerializable_EndByteIndex;
        protected virtual int _MyListNonLazinatorType2_ByteLength => _DotNetList_NonSelfSerializable_EndByteIndex - _MyListNonLazinatorType2_ByteIndex;
        
        private List<NonLazinatorClass> _MyListNonLazinatorType;
        public List<NonLazinatorClass> MyListNonLazinatorType
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListNonLazinatorType_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListNonLazinatorType = default(List<NonLazinatorClass>);
                        _MyListNonLazinatorType_Dirty = true;
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListNonLazinatorType_ByteIndex, _MyListNonLazinatorType_ByteLength);
                        _MyListNonLazinatorType = ConvertFromBytes_List_GNonLazinatorClass_g(childData, DeserializationFactory, () => { MyListNonLazinatorType_Dirty = true; });
                    }
                    _MyListNonLazinatorType_Accessed = true;
                }
                return _MyListNonLazinatorType;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListNonLazinatorType = value;
                _MyListNonLazinatorType_Dirty = true;
                _MyListNonLazinatorType_Accessed = true;
            }
        }
        protected bool _MyListNonLazinatorType_Accessed;
        
        private bool _MyListNonLazinatorType_Dirty;
        public bool MyListNonLazinatorType_Dirty
        {
            [DebuggerStepThrough]
            get => _MyListNonLazinatorType_Dirty;
            [DebuggerStepThrough]
            set
            {
                if (_MyListNonLazinatorType_Dirty != value)
                {
                    _MyListNonLazinatorType_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        private List<NonLazinatorClass> _MyListNonLazinatorType2;
        public List<NonLazinatorClass> MyListNonLazinatorType2
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListNonLazinatorType2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListNonLazinatorType2 = default(List<NonLazinatorClass>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListNonLazinatorType2_ByteIndex, _MyListNonLazinatorType2_ByteLength);
                        _MyListNonLazinatorType2 = ConvertFromBytes_List_GNonLazinatorClass_g(childData, DeserializationFactory, null);
                    }
                    _MyListNonLazinatorType2_Accessed = true;
                    IsDirty = true;
                }
                return _MyListNonLazinatorType2;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListNonLazinatorType2 = value;
                _MyListNonLazinatorType2_Accessed = true;
            }
        }
        protected bool _MyListNonLazinatorType2_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 207;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyListNonLazinatorType_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListNonLazinatorType2_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _DotNetList_NonSelfSerializable_EndByteIndex = bytesSoFar;
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
            nonLazinatorObject: _MyListNonLazinatorType, isBelievedDirty: MyListNonLazinatorType_Dirty,
            isAccessed: _MyListNonLazinatorType_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListNonLazinatorType_ByteIndex, _MyListNonLazinatorType_ByteLength),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GNonLazinatorClass_g(w, MyListNonLazinatorType,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListNonLazinatorType2, isBelievedDirty: _MyListNonLazinatorType2_Accessed,
            isAccessed: _MyListNonLazinatorType2_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListNonLazinatorType2_ByteIndex, _MyListNonLazinatorType2_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GNonLazinatorClass_g(w, MyListNonLazinatorType2,
            includeChildrenMode, v));
            
            _IsDirty = false;
            _DescendantIsDirty = false;
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<NonLazinatorClass> ConvertFromBytes_List_GNonLazinatorClass_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(List<NonLazinatorClass>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<NonLazinatorClass> collection = new List<NonLazinatorClass>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(NonLazinatorClass));
                }
                else
                {
                    ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = NonLazinatorDirectConverter.ConvertFromBytes_NonLazinatorClass(childData, deserializationFactory, informParentOfDirtinessDelegate);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GNonLazinatorClass_g(BinaryBufferWriter writer, List<NonLazinatorClass> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(List<NonLazinatorClass>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(NonLazinatorClass))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(BinaryBufferWriter w) => NonLazinatorDirectConverter.ConvertToBytes_NonLazinatorClass(writer, itemToConvert[itemIndex], includeChildrenMode, verifyCleanness);
                    WriteToBinaryWithIntLengthPrefix(writer, action);
                }
                
            }
        }
        
    }
}
