//0ec55d1b-7528-d8b8-4609-d74aeadab768
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.72
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
    using Lazinator.Wrappers;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class DotNetList_Wrapper : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public DotNetList_Wrapper() : base()
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
            
            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
            if (uniqueID != LazinatorUniqueID)
            {
                throw new FormatException("Wrong self-serialized type initialized.");
            }
            
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
            var clone = new DotNetList_Wrapper()
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
            _MyListInt_Dirty = false;
        }
        
        public virtual DeserializationFactory DeserializationFactory { get; set; }
        
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
        
        protected int _MyListInt_ByteIndex;
        protected int _MyListNullableByte_ByteIndex;
        protected int _MyListNullableInt_ByteIndex;
        protected virtual int _MyListInt_ByteLength => _MyListNullableByte_ByteIndex - _MyListInt_ByteIndex;
        protected virtual int _MyListNullableByte_ByteLength => _MyListNullableInt_ByteIndex - _MyListNullableByte_ByteIndex;
        private int _DotNetList_Wrapper_EndByteIndex;
        protected virtual int _MyListNullableInt_ByteLength => _DotNetList_Wrapper_EndByteIndex - _MyListNullableInt_ByteIndex;
        
        private List<WInt> _MyListInt;
        public List<WInt> MyListInt
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListInt = default(List<WInt>);
                        _MyListInt_Dirty = true;
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListInt_ByteIndex, _MyListInt_ByteLength, false, false, null);
                        _MyListInt = ConvertFromBytes_List_GWInt_g(childData, DeserializationFactory, () => { MyListInt_Dirty = true; });
                    }
                    _MyListInt_Accessed = true;
                }
                return _MyListInt;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListInt = value;
                _MyListInt_Dirty = true;
                _MyListInt_Accessed = true;
            }
        }
        protected bool _MyListInt_Accessed;
        
        private bool _MyListInt_Dirty;
        public bool MyListInt_Dirty
        {
            [DebuggerStepThrough]
            get => _MyListInt_Dirty;
            [DebuggerStepThrough]
            set
            {
                if (_MyListInt_Dirty != value)
                {
                    _MyListInt_Dirty = value;
                    if (value && !IsDirty)
                    {
                        IsDirty = true;
                    }
                }
            }
        }
        private List<WNullableByte> _MyListNullableByte;
        public List<WNullableByte> MyListNullableByte
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListNullableByte_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListNullableByte = default(List<WNullableByte>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListNullableByte_ByteIndex, _MyListNullableByte_ByteLength, false, false, null);
                        _MyListNullableByte = ConvertFromBytes_List_GWNullableByte_g(childData, DeserializationFactory, null);
                    }
                    _MyListNullableByte_Accessed = true;
                    IsDirty = true;
                }
                return _MyListNullableByte;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListNullableByte = value;
                _MyListNullableByte_Accessed = true;
            }
        }
        protected bool _MyListNullableByte_Accessed;
        private List<WNullableInt> _MyListNullableInt;
        public List<WNullableInt> MyListNullableInt
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyListNullableInt_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListNullableInt = default(List<WNullableInt>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListNullableInt_ByteIndex, _MyListNullableInt_ByteLength, false, false, null);
                        _MyListNullableInt = ConvertFromBytes_List_GWNullableInt_g(childData, DeserializationFactory, null);
                    }
                    _MyListNullableInt_Accessed = true;
                    IsDirty = true;
                }
                return _MyListNullableInt;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyListNullableInt = value;
                _MyListNullableInt_Accessed = true;
            }
        }
        protected bool _MyListNullableInt_Accessed;
        
        protected virtual void ResetAccessedProperties()
        {
            _MyListInt_Accessed = _MyListNullableByte_Accessed = _MyListNullableInt_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 263;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        protected virtual System.Collections.Generic.List<int> _LazinatorGenericID { get; set; }
        public virtual System.Collections.Generic.List<int> LazinatorGenericID
        {
            get => null;
            set => throw new NotSupportedException();
        }
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyListInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListNullableByte_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListNullableInt_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _DotNetList_Wrapper_EndByteIndex = bytesSoFar;
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
                CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            }
            
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListInt, isBelievedDirty: MyListInt_Dirty,
            isAccessed: _MyListInt_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListInt_ByteIndex, _MyListInt_ByteLength, false, false, null),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GWInt_g(w, MyListInt,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListNullableByte, isBelievedDirty: _MyListNullableByte_Accessed,
            isAccessed: _MyListNullableByte_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListNullableByte_ByteIndex, _MyListNullableByte_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GWNullableByte_g(w, MyListNullableByte,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListNullableInt, isBelievedDirty: _MyListNullableInt_Accessed,
            isAccessed: _MyListNullableInt_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListNullableInt_ByteIndex, _MyListNullableInt_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GWNullableInt_g(w, MyListNullableInt,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<WInt> ConvertFromBytes_List_GWInt_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(List<WInt>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<WInt> collection = new List<WInt>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToByte(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = new WInt()
                {
                    DeserializationFactory = deserializationFactory,
                    InformParentOfDirtinessDelegate = informParentOfDirtinessDelegate,
                    LazinatorObjectBytes = childData,
                };
                collection.Add(item);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GWInt_g(BinaryBufferWriter writer, List<WInt> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(List<WInt>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                void action(BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithByteLengthPrefix(writer, action);
            }
        }
        
        private static List<WNullableByte> ConvertFromBytes_List_GWNullableByte_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(List<WNullableByte>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<WNullableByte> collection = new List<WNullableByte>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToByte(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = new WNullableByte()
                {
                    DeserializationFactory = deserializationFactory,
                    InformParentOfDirtinessDelegate = informParentOfDirtinessDelegate,
                    LazinatorObjectBytes = childData,
                };
                collection.Add(item);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GWNullableByte_g(BinaryBufferWriter writer, List<WNullableByte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(List<WNullableByte>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                void action(BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithByteLengthPrefix(writer, action);
            }
        }
        
        private static List<WNullableInt> ConvertFromBytes_List_GWNullableInt_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(List<WNullableInt>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<WNullableInt> collection = new List<WNullableInt>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToByte(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = new WNullableInt()
                {
                    DeserializationFactory = deserializationFactory,
                    InformParentOfDirtinessDelegate = informParentOfDirtinessDelegate,
                    LazinatorObjectBytes = childData,
                };
                collection.Add(item);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GWNullableInt_g(BinaryBufferWriter writer, List<WNullableInt> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(List<WNullableInt>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                void action(BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithByteLengthPrefix(writer, action);
            }
        }
        
    }
}
