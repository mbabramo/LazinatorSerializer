//596f2bf1-2b65-12bb-789a-65733aef22f4
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.207
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
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Dictionary_Values_SelfSerialized : ILazinator
    {
        public bool IsStruct => false;
        
        /* Serialization, deserialization, and object relationships */
        
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
        
        public virtual LazinatorMemory SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode, bool updateStoredBuffer = false)
        {
            LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate)EncodeToNewBuffer, updateStoredBuffer);
            var clone = new Dictionary_Values_SelfSerialized()
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
            get => _IsDirty || LazinatorObjectBytes.Length == 0;
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
        
        private LazinatorMemory _HierarchyBytes;
        public virtual LazinatorMemory HierarchyBytes
        {
            set
            {
                _HierarchyBytes = value;
                LazinatorMemoryStorage = value;
            }
        }
        
        protected LazinatorMemory _LazinatorMemoryStorage; // DEBUG -- use only one memory storage
        public virtual LazinatorMemory LazinatorMemoryStorage
        {
            get => _LazinatorMemoryStorage;
            set
            {
                _LazinatorMemoryStorage = value;
                int length = Deserialize();
            }
        }
        
        public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => LazinatorMemoryStorage.Memory;
        }
        
        public virtual void EnsureLazinatorMemoryUpToDate()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
        }
        
        public virtual int GetByteLength()
        {
            EnsureLazinatorMemoryUpToDate();
            return LazinatorObjectBytes.Length;
        }
        
        public virtual uint GetBinaryHashCode32()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public virtual ulong GetBinaryHashCode64()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public virtual Guid GetBinaryHashCode128()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Property definitions */
        
        protected int _MyDictionary_ByteIndex;
        protected int _MySortedDictionary_ByteIndex;
        protected int _MySortedList_ByteIndex;
        protected virtual int _MyDictionary_ByteLength => _MySortedDictionary_ByteIndex - _MyDictionary_ByteIndex;
        protected virtual int _MySortedDictionary_ByteLength => _MySortedList_ByteIndex - _MySortedDictionary_ByteIndex;
        private int _Dictionary_Values_SelfSerialized_EndByteIndex;
        protected virtual int _MySortedList_ByteLength => _Dictionary_Values_SelfSerialized_EndByteIndex - _MySortedList_ByteIndex;
        
        protected Dictionary<int, ExampleChild> _MyDictionary;
        public Dictionary<int, ExampleChild> MyDictionary
        {
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
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyDictionary_ByteIndex, _MyDictionary_ByteLength, false, false, null);
                        _MyDictionary = ConvertFromBytes_Dictionary_Gint_c_C32ExampleChild_g(childData);
                    }
                    _MyDictionary_Accessed = true;
                }
                IsDirty = true; 
                return _MyDictionary;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyDictionary = value;
                _MyDictionary_Accessed = true;
            }
        }
        protected bool _MyDictionary_Accessed;
        protected SortedDictionary<int, ExampleChild> _MySortedDictionary;
        public SortedDictionary<int, ExampleChild> MySortedDictionary
        {
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
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MySortedDictionary_ByteIndex, _MySortedDictionary_ByteLength, false, false, null);
                        _MySortedDictionary = ConvertFromBytes_SortedDictionary_Gint_c_C32ExampleChild_g(childData);
                    }
                    _MySortedDictionary_Accessed = true;
                }
                IsDirty = true; 
                return _MySortedDictionary;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MySortedDictionary = value;
                _MySortedDictionary_Accessed = true;
            }
        }
        protected bool _MySortedDictionary_Accessed;
        protected SortedList<int, ExampleChild> _MySortedList;
        public SortedList<int, ExampleChild> MySortedList
        {
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
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MySortedList_ByteIndex, _MySortedList_ByteLength, false, false, null);
                        _MySortedList = ConvertFromBytes_SortedList_Gint_c_C32ExampleChild_g(childData);
                    }
                    _MySortedList_Accessed = true;
                }
                IsDirty = true; 
                return _MySortedList;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MySortedList = value;
                _MySortedList_Accessed = true;
            }
        }
        protected bool _MySortedList_Accessed;
        
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
            yield return ("MyDictionary", (object)MyDictionary);
            yield return ("MySortedDictionary", (object)MySortedDictionary);
            yield return ("MySortedList", (object)MySortedList);
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            _MyDictionary_Accessed = _MySortedDictionary_Accessed = _MySortedList_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 204;
        
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
            _MyDictionary_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MySortedDictionary_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MySortedList_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _Dictionary_Values_SelfSerialized_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
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
                
                LazinatorMemoryStorage = writer.LazinatorMemorySlice(startPosition);
            }
        }
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                if (LazinatorGenericID.IsEmpty)
                {
                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(ref writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MyDictionary_Accessed)
            {
                var deserialized = MyDictionary;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyDictionary, isBelievedDirty: _MyDictionary_Accessed,
            isAccessed: _MyDictionary_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyDictionary_ByteIndex, _MyDictionary_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_Dictionary_Gint_c_C32ExampleChild_g(ref w, _MyDictionary,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyDictionary_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MySortedDictionary_Accessed)
            {
                var deserialized = MySortedDictionary;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MySortedDictionary, isBelievedDirty: _MySortedDictionary_Accessed,
            isAccessed: _MySortedDictionary_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MySortedDictionary_ByteIndex, _MySortedDictionary_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_SortedDictionary_Gint_c_C32ExampleChild_g(ref w, _MySortedDictionary,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MySortedDictionary_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_MySortedList_Accessed)
            {
                var deserialized = MySortedList;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MySortedList, isBelievedDirty: _MySortedList_Accessed,
            isAccessed: _MySortedList_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MySortedList_ByteIndex, _MySortedList_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_SortedList_Gint_c_C32ExampleChild_g(ref w, _MySortedList,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MySortedList_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _Dictionary_Values_SelfSerialized_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Dictionary<int, ExampleChild> ConvertFromBytes_Dictionary_Gint_c_C32ExampleChild_g(LazinatorMemory storage)
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
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes_KeyValuePair_Gint_c_C32ExampleChild_g(childData);
                collection.Add(item.Key, item.Value);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_Dictionary_Gint_c_C32ExampleChild_g(ref BinaryBufferWriter writer, Dictionary<int, ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(Dictionary<int, ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            foreach (var item in itemToConvert)
            {
                void action(ref BinaryBufferWriter w) => ConvertToBytes_KeyValuePair_Gint_c_C32ExampleChild_g(ref w, item, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, action);
            }
        }
        
        private static KeyValuePair<int, ExampleChild> ConvertFromBytes_KeyValuePair_Gint_c_C32ExampleChild_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.ReadOnlySpan;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            ExampleChild item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = DeserializationFactory.Instance.CreateBasedOnType<ExampleChild>(childData);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var tupleType = new KeyValuePair<int, ExampleChild>(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_KeyValuePair_Gint_c_C32ExampleChild_g(ref BinaryBufferWriter writer, KeyValuePair<int, ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Key);
            
            if (itemToConvert.Value == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionValue(ref BinaryBufferWriter w) => itemToConvert.Value.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, actionValue);
            };
        }
        
        private static SortedDictionary<int, ExampleChild> ConvertFromBytes_SortedDictionary_Gint_c_C32ExampleChild_g(LazinatorMemory storage)
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
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes_KeyValuePair_Gint_c_C32ExampleChild_g(childData);
                collection.Add(item.Key, item.Value);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_SortedDictionary_Gint_c_C32ExampleChild_g(ref BinaryBufferWriter writer, SortedDictionary<int, ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(SortedDictionary<int, ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            foreach (var item in itemToConvert)
            {
                void action(ref BinaryBufferWriter w) => ConvertToBytes_KeyValuePair_Gint_c_C32ExampleChild_g(ref w, item, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, action);
            }
        }
        
        private static SortedList<int, ExampleChild> ConvertFromBytes_SortedList_Gint_c_C32ExampleChild_g(LazinatorMemory storage)
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
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes_KeyValuePair_Gint_c_C32ExampleChild_g(childData);
                collection.Add(item.Key, item.Value);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_SortedList_Gint_c_C32ExampleChild_g(ref BinaryBufferWriter writer, SortedList<int, ExampleChild> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(SortedList<int, ExampleChild>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            foreach (var item in itemToConvert)
            {
                void action(ref BinaryBufferWriter w) => ConvertToBytes_KeyValuePair_Gint_c_C32ExampleChild_g(ref w, item, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, action);
            }
        }
        
    }
}
