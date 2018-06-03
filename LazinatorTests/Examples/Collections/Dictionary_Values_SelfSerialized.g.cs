//c98d38ec-9d75-73a7-42dc-57556d06757d
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
    using LazinatorTests.Examples;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Dictionary_Values_SelfSerialized : ILazinator
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
            var clone = new Dictionary_Values_SelfSerialized()
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
        
        protected int _MyDictionary_ByteIndex;
        protected int _MySortedDictionary_ByteIndex;
        protected int _MySortedList_ByteIndex;
        protected virtual int _MyDictionary_ByteLength => _MySortedDictionary_ByteIndex - _MyDictionary_ByteIndex;
        protected virtual int _MySortedDictionary_ByteLength => _MySortedList_ByteIndex - _MySortedDictionary_ByteIndex;
        private int _Dictionary_Values_SelfSerialized_EndByteIndex;
        protected virtual int _MySortedList_ByteLength => _Dictionary_Values_SelfSerialized_EndByteIndex - _MySortedList_ByteIndex;
        
        private Dictionary<int, ExampleChild> _MyDictionary;
        public Dictionary<int, ExampleChild> MyDictionary
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MyDictionary_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyDictionary = default(Dictionary<int, ExampleChild>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyDictionary_ByteIndex, _MyDictionary_ByteLength, false, false, null);
                        _MyDictionary = ConvertFromBytes_Dictionary_Gint_c_C32ExampleChild_g(childData, DeserializationFactory, null);
                    }
                    _MyDictionary_Accessed = true;
                    IsDirty = true;
                }
                return _MyDictionary;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MyDictionary = value;
                _MyDictionary_Accessed = true;
            }
        }
        protected bool _MyDictionary_Accessed;
        private SortedDictionary<int, ExampleChild> _MySortedDictionary;
        public SortedDictionary<int, ExampleChild> MySortedDictionary
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MySortedDictionary_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MySortedDictionary = default(SortedDictionary<int, ExampleChild>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MySortedDictionary_ByteIndex, _MySortedDictionary_ByteLength, false, false, null);
                        _MySortedDictionary = ConvertFromBytes_SortedDictionary_Gint_c_C32ExampleChild_g(childData, DeserializationFactory, null);
                    }
                    _MySortedDictionary_Accessed = true;
                    IsDirty = true;
                }
                return _MySortedDictionary;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MySortedDictionary = value;
                _MySortedDictionary_Accessed = true;
            }
        }
        protected bool _MySortedDictionary_Accessed;
        private SortedList<int, ExampleChild> _MySortedList;
        public SortedList<int, ExampleChild> MySortedList
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MySortedList_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MySortedList = default(SortedList<int, ExampleChild>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MySortedList_ByteIndex, _MySortedList_ByteLength, false, false, null);
                        _MySortedList = ConvertFromBytes_SortedList_Gint_c_C32ExampleChild_g(childData, DeserializationFactory, null);
                    }
                    _MySortedList_Accessed = true;
                    IsDirty = true;
                }
                return _MySortedList;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _MySortedList = value;
                _MySortedList_Accessed = true;
            }
        }
        protected bool _MySortedList_Accessed;
        
        protected virtual void ResetAccessedProperties()
        {
            _MyDictionary_Accessed = _MySortedDictionary_Accessed = _MySortedList_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 204;
        
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
            _MyDictionary_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MySortedDictionary_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MySortedList_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _Dictionary_Values_SelfSerialized_EndByteIndex = bytesSoFar;
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
            nonLazinatorObject: _MyDictionary, isBelievedDirty: _MyDictionary_Accessed,
            isAccessed: _MyDictionary_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyDictionary_ByteIndex, _MyDictionary_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_Dictionary_Gint_c_C32ExampleChild_g(w, MyDictionary,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MySortedDictionary, isBelievedDirty: _MySortedDictionary_Accessed,
            isAccessed: _MySortedDictionary_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MySortedDictionary_ByteIndex, _MySortedDictionary_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_SortedDictionary_Gint_c_C32ExampleChild_g(w, MySortedDictionary,
            includeChildrenMode, v));
            WriteNonLazinatorObject(
            nonLazinatorObject: _MySortedList, isBelievedDirty: _MySortedList_Accessed,
            isAccessed: _MySortedList_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MySortedList_ByteIndex, _MySortedList_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_SortedList_Gint_c_C32ExampleChild_g(w, MySortedList,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Dictionary<int, ExampleChild> ConvertFromBytes_Dictionary_Gint_c_C32ExampleChild_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(Dictionary<int, ExampleChild>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            Dictionary<int, ExampleChild> collection = new Dictionary<int, ExampleChild>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes_KeyValuePair_Gint_c_C32ExampleChild_g(childData, deserializationFactory, informParentOfDirtinessDelegate);
                collection.Add(item.Key, item.Value);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_Dictionary_Gint_c_C32ExampleChild_g(BinaryBufferWriter writer, Dictionary<int, ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(Dictionary<int, ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            foreach (var item in itemToConvert)
            {
                void action(BinaryBufferWriter w) => ConvertToBytes_KeyValuePair_Gint_c_C32ExampleChild_g(writer, item, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, action);
            }
        }
        
        private static KeyValuePair<int, ExampleChild> ConvertFromBytes_KeyValuePair_Gint_c_C32ExampleChild_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            ExampleChild item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                if (deserializationFactory == null)
                {
                    deserializationFactory = DeserializationFactory.GetInstance();
                }
                item2 = deserializationFactory.CreateBasedOnTypeSpecifyingDelegate<ExampleChild>(childData, informParentOfDirtinessDelegate);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var tupleType = new KeyValuePair<int, ExampleChild>(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_KeyValuePair_Gint_c_C32ExampleChild_g(BinaryBufferWriter writer, KeyValuePair<int, ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Key);
            
            if (itemToConvert.Value == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionValue(BinaryBufferWriter w) => itemToConvert.Value.SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, actionValue);
            };
        }
        
        private static SortedDictionary<int, ExampleChild> ConvertFromBytes_SortedDictionary_Gint_c_C32ExampleChild_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(SortedDictionary<int, ExampleChild>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            SortedDictionary<int, ExampleChild> collection = new SortedDictionary<int, ExampleChild>();
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes_KeyValuePair_Gint_c_C32ExampleChild_g(childData, deserializationFactory, informParentOfDirtinessDelegate);
                collection.Add(item.Key, item.Value);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_SortedDictionary_Gint_c_C32ExampleChild_g(BinaryBufferWriter writer, SortedDictionary<int, ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(SortedDictionary<int, ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            foreach (var item in itemToConvert)
            {
                void action(BinaryBufferWriter w) => ConvertToBytes_KeyValuePair_Gint_c_C32ExampleChild_g(writer, item, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, action);
            }
        }
        
        private static SortedList<int, ExampleChild> ConvertFromBytes_SortedList_Gint_c_C32ExampleChild_g(ReadOnlyMemory<byte> storage, DeserializationFactory deserializationFactory, InformParentOfDirtinessDelegate informParentOfDirtinessDelegate)
        {
            if (storage.Length == 0)
            {
                return default(SortedList<int, ExampleChild>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            SortedList<int, ExampleChild> collection = new SortedList<int, ExampleChild>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes_KeyValuePair_Gint_c_C32ExampleChild_g(childData, deserializationFactory, informParentOfDirtinessDelegate);
                collection.Add(item.Key, item.Value);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_SortedList_Gint_c_C32ExampleChild_g(BinaryBufferWriter writer, SortedList<int, ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            if (itemToConvert == default(SortedList<int, ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            foreach (var item in itemToConvert)
            {
                void action(BinaryBufferWriter w) => ConvertToBytes_KeyValuePair_Gint_c_C32ExampleChild_g(writer, item, includeChildrenMode, verifyCleanness);
                WriteToBinaryWithIntLengthPrefix(writer, action);
            }
        }
        
    }
}
