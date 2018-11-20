using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Support;
using Lazinator.Buffers;
using Lazinator.Core;
using static Lazinator.Core.LazinatorUtilities;
using Lazinator.Attributes;
using System.Buffers;
using System.Linq;

namespace Lazinator.Collections
{
    [Implements(new string[] { "PreSerialization", "EnumerateLazinatorDescendants", "OnFreeInMemoryObjects", "AssignCloneProperties", "OnUpdateDeserializedChildren", "OnPropertiesWritten", "OnForEachLazinator" })]
    public partial class LazinatorList<T> : IList<T>, ILazinatorList<T>, ILazinatorList where T : ILazinator
    {
        [NonSerialized] private bool _FullyDeserialized;
        [NonSerialized] private int _CountWhenDeserialized;
        [NonSerialized] private List<T> _UnderlyingList;
        [NonSerialized] private List<bool> _ItemsAccessedBeforeFullyDeserialized;
        [NonSerialized] private int? _FixedID;
        [NonSerialized] private bool _TypeRequiresNonBinaryHashing;
        [NonSerialized] private int _NumRemovedFromStart;
        [NonSerialized] private Memory<byte> _PreviousMainListSerialized;
        [NonSerialized] private LazinatorOffsetList _PreviousOffsets;

        public LazinatorList()
        {
            _FixedID = DeserializationFactory.Instance.GetFixedUniqueID(typeof(T));
            _TypeRequiresNonBinaryHashing = DeserializationFactory.Instance.HasNonBinaryHashAttribute(typeof(T));
        }

        public LazinatorList(int numItems)
        {
            for (int i = 0; i < numItems; i++)
                Add(default);
        }

        public LazinatorList(IEnumerable<T> items)
        {
            foreach (T item in items)
                Add(item);
        }

        private void CreateUnderlyingListIfNecessary()
        {
            if (_UnderlyingList == null)
            {
                _CountWhenDeserialized = Offsets?.Count ?? 0;

                _UnderlyingList = new List<T>(_CountWhenDeserialized);
                _ItemsAccessedBeforeFullyDeserialized = new List<bool>(_CountWhenDeserialized);
                for (int i = 0; i < _CountWhenDeserialized; i++)
                {
                    _UnderlyingList.Add(default);
                    _ItemsAccessedBeforeFullyDeserialized.Add(false);
                }
            }
        }

        private void FullyDeserialize()
        {
            if (!_FullyDeserialized)
            {
                CreateUnderlyingListIfNecessary();
                int count = Count;
                for (int i = 0; i < count; i++)
                {
                    if (!_ItemsAccessedBeforeFullyDeserialized[i])
                        _UnderlyingList[i] = GetSerializedContents(i);
                }
                _FullyDeserialized = true;
                _NumRemovedFromStart = 0;
            }
        }

        private T GetSerializedContents(int index)
        {
            var byteSpan = GetListMemberSlice(index);
            if (byteSpan.Length == 0)
                return default;
            T n2;
            if (_FixedID == null)
                n2 = (T)DeserializationFactory.Instance.CreateFromBytesIncludingID(byteSpan, this);
            else
                n2 = (T)DeserializationFactory.Instance.CreateKnownID((int)_FixedID, byteSpan, this);
            return n2;
        }

        public uint GetListMemberHash32(int index)
        {
            if (_TypeRequiresNonBinaryHashing)
                return (uint) this[index].GetHashCode();

            if (_FullyDeserialized || (_UnderlyingList != null && _ItemsAccessedBeforeFullyDeserialized[index]))
                return ((IList<T>)_UnderlyingList)[index].GetBinaryHashCode32();
            
            var byteSpan = GetListMemberSlice(index);
            return FarmhashByteSpans.Hash32(byteSpan.Span);
        }

        private LazinatorMemory GetListMemberSlice(int index)
        {
            if (_FullyDeserialized)
            { // we can't rely on original offsets, because an insertion/removal may have occurred
                T underlyingItem = _UnderlyingList[index];
                if (underlyingItem == null)
                    return LazinatorUtilities.EmptyLazinatorMemory;
                return underlyingItem.LazinatorMemoryStorage;
            }
            // The 1st item (# 0) has index 0 always, so it's not stored in Offsets.
            // If we have three items, we store the offset of the second and the third,
            // plus the location AFTER the third.
            // The offset of the first is then 0 and the next offset is Offsets[0].
            // The offset of the second is then Offsets[0] and next offset is Offsets[1].
            // The offste of the third is Offsets[1] and the next offset is Offsets[2], the position at the end of the third item.
            if (Offsets == null || index + _NumRemovedFromStart >= Offsets.Count)
                return LazinatorUtilities.EmptyLazinatorMemory;
            int offset = GetOffset(index);
            int nextOffset = Offsets[_NumRemovedFromStart + index];

            var mainListSerializedStorage = GetChildSlice(LazinatorMemoryStorage, _MainListSerialized_ByteIndex, _MainListSerialized_ByteLength, false, false, null); // this is equivalent to MainListSerialized (omitting the length, containing the bytes). We don't use MainListSerialized itself because it's not slicesable
            return mainListSerializedStorage.Slice(offset, nextOffset - offset);
        }

        private int GetOffset(int index)
        {
            index += _NumRemovedFromStart; // e.g., if index is 0 in the list now, but two were removed from start since we last updated offsets, then we need to look at slot 2
            int offset;
            if (index == 0)
                offset = 0;
            else
                offset = Offsets[index - 1];
            return offset;
        }

        public T this[int index]
        {
            get
            {
                if (_FullyDeserialized)
                    return ((IList<T>) _UnderlyingList)[index];
                CreateUnderlyingListIfNecessary();
                var current = ((IList<T>) _UnderlyingList)[index];
                if (current == null || (current.Equals(default(T))))
                {
                    if (_ItemsAccessedBeforeFullyDeserialized[index])
                        return default;
                    current = GetSerializedContents(index);
                    _UnderlyingList[index] = current;
                    _ItemsAccessedBeforeFullyDeserialized[index] = true;
                }
                return current;
            }
            set
            {
                var currentOccupant = this[index];
                if (currentOccupant != null)
                    currentOccupant.LazinatorParents = currentOccupant.LazinatorParents.WithRemoved(this);
                if (_FullyDeserialized)
                {
                    ((IList<T>)_UnderlyingList)[index] = value;
                    if (value != null)
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    MarkDirty();
                    return;
                }
                CreateUnderlyingListIfNecessary();
                ((IList<T>) _UnderlyingList)[index] = value;
                if (value != null)
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                MarkDirty();
                _ItemsAccessedBeforeFullyDeserialized[index] = true;
            }
        }

        private void MarkDirty()
        {
            IsDirty = true;
            DescendantIsDirty = true;
            //if (Offsets != null)
            //    Offsets.IsDirty = true;
        }

        public int Count
        {
            get
            {
                if (_UnderlyingList == null)
                {
                    _CountWhenDeserialized = Offsets?.Count ?? 0;
                    return _CountWhenDeserialized;
                }
                return ((IList<T>) _UnderlyingList).Count; 
            }
        }

        public bool IsReadOnly => false;

        public virtual void Add(T item)
        {
            CompleteAdd(item);
        }

        // extracted so that we can call this from lazinatorarray, even though add is overriden
        protected void CompleteAdd(T item)
        {
            // this does not require us to fully deserialize,
            // because it doesn't change anything earlier in the list
            if (item != null)
                item.LazinatorParents = item.LazinatorParents.WithAdded(this);
            CreateUnderlyingListIfNecessary();
            ((IList<T>)_UnderlyingList).Add(item);
            if (!_FullyDeserialized)
                _ItemsAccessedBeforeFullyDeserialized.Add(true);
            MarkDirty();
        }

        public virtual void Clear()
        {
            FullyDeserialize();
            ((IList<T>)_UnderlyingList).Clear();
            MarkDirty();
        }

        public bool Contains(T item)
        {
            FullyDeserialize();
            return ((IList<T>)_UnderlyingList).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            FullyDeserialize();
            ((IList<T>)_UnderlyingList).CopyTo(array, arrayIndex);
        }

        public bool Any()
        {
            CreateUnderlyingListIfNecessary();
            return _UnderlyingList.Any();
        }

        public IEnumerator<T> GetEnumerator()
        {
            FullyDeserialize();
            return ((IList<T>)_UnderlyingList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            FullyDeserialize();
            return ((IList<T>)_UnderlyingList).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            FullyDeserialize();
            return ((IList<T>)_UnderlyingList).IndexOf(item);
        }

        // Note: Instead of fully deserializing on inserts and removes, we might keep track of what has been inserted and what has been removed. But if high insert/removes are expected, or especially big lists, we can use AvlList<T> instead.

        public virtual void Insert(int index, T item)
        {
            FullyDeserialize();
            if (item != null)
                item.LazinatorParents = item.LazinatorParents.WithAdded(this);
            CreateUnderlyingListIfNecessary();
            ((IList<T>)_UnderlyingList).Insert(index, item);
            MarkDirty();
        }

        public virtual bool Remove(T item)
        {
            FullyDeserialize();
            var success = ((IList<T>)_UnderlyingList).Remove(item);
            if (success)
                MarkDirty();
            return success;
        }

        public virtual int RemoveAll(Predicate<T> match)
        {
            FullyDeserialize();
            int matches = 0;
            for (int i = Count - 1; i >= 0; i--) // iterate backwards so that indices stay same during loop
            { 
                var item = _UnderlyingList[i];
                if (match(item))
                {
                    _UnderlyingList.RemoveAt(i);
                    matches++;
                }
            }
            if (matches > 0)
                MarkDirty();
            return matches;
        }

        public virtual void RemoveAt(int index)
        {
            if (index >= Count || index < 0)
                throw new Exception("Invalid removal index.");
            if (!_FullyDeserialized && index == 0)
            {
                CreateUnderlyingListIfNecessary();
                ((IList<T>)_UnderlyingList).RemoveAt(0);
                _ItemsAccessedBeforeFullyDeserialized.RemoveAt(0);
                _NumRemovedFromStart++;
            }
            else if (!_FullyDeserialized && index == Count - 1)
            {
                CreateUnderlyingListIfNecessary();
                ((IList<T>)_UnderlyingList).RemoveAt(index);
                _ItemsAccessedBeforeFullyDeserialized.RemoveAt(index);
            }
            else
            {
                FullyDeserialize();
                ((IList<T>)_UnderlyingList).RemoveAt(index);
            }
            MarkDirty();
        }

        public virtual void PreSerialization(bool verifyCleanness, bool updateStoredBuffer)
        {
            if (IsDirty || DescendantIsDirty)
            {
                MainListSerialized_Dirty = true;
                _PreviousMainListSerialized = MainListSerialized; // has side effect of loading _MainListSerialized and setting _MainListSerialized_Accessed to true, thus making sure we call WriteMainList
                _PreviousOffsets = Offsets;
            }
        }


        // How do we ensure that after serialization occurs, the child items get updated?
        // If the list is clean, then the entire storage of the LazinatorList is written in one fell swoop. But UpdateStoredBuffer will be called if the list is instantiated. Then, OnUpdateDeserializedChildren will be called, and the active BinaryBufferWriter can be used.
        // If the list is dirty or has a dirty descendant, then the writing of properties will work as follows: 
        // If updateStoredBuffer is true, then after writing properties into buffer, we call UpdateStoredBuffer with updateDeserializedChildren = false. The expectation is that we'll update each child when writing the properties, so we do this in WriteMainList.  The WriteChild method there will do this assuming that the child is in memory, and the child will get the new buffer. 
        // But what happens if updateStoredBuffer is false? If that is so, WriteMainList still updates MainListSerialized and Offsets. But it then immediately switches them back after we update with updateStoredBuffer = false. This ensures that Offsets refers to the original LazinatorMemoryStorage. 

        public void OnUpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_UnderlyingList == null)
                return;
            for (int index = 0; index < _UnderlyingList.Count; index++)
            {
                if (_FullyDeserialized || _ItemsAccessedBeforeFullyDeserialized[index])
                {
                    var current = ((IList<T>)_UnderlyingList)[index];
                    if (current != null)
                    {
                        current.UpdateStoredBuffer(ref writer, startPosition + _MainListSerialized_ByteIndex + sizeof(int) + GetOffset(index), GetOffset(index + 1) - GetOffset(index), IncludeChildrenMode.IncludeAllChildren, true);
                        if (current.IsStruct)
                        {
                            _UnderlyingList[index] = current;
                        }
                    }
                }
            }
        }

        public void OnPropertiesWritten(bool updateStoredBuffer)
        {
            if (updateStoredBuffer)
            {
                // MainListSerialized and Offsets have been updated, and this will match the updated LazinatorMemoryStorage.
                _PreviousMainListSerialized = null;
                _PreviousOffsets = null;
                _NumRemovedFromStart = 0;
            }
            else
            {
                // Because LazinatorMemoryStorage is the same, we need to return MainListSerialized and especially Offsets to their previous values.
                MainListSerialized = _PreviousMainListSerialized;
                Offsets = _PreviousOffsets;
            }
        }

        private bool ItemHasBeenAccessed(int index)
        {
            CreateUnderlyingListIfNecessary();
            return _FullyDeserialized || _ItemsAccessedBeforeFullyDeserialized[index];
        }

        private void WriteMainList(ref BinaryBufferWriter writer, ReadOnlyMemory<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (IsDirty || DescendantIsDirty|| includeChildrenMode != OriginalIncludeChildrenMode)
            {
                var offsetList = new LazinatorOffsetList();
                int originalStartingPosition = writer.Position;
                LazinatorUtilities.WriteToBinaryWithoutLengthPrefix(ref writer, (ref BinaryBufferWriter w) =>
                {
                    int startingPosition = w.Position;
                    if (_UnderlyingList == null && _CountWhenDeserialized > 0)
                        CreateUnderlyingListIfNecessary();
                    for (int i = 0; i < (_UnderlyingList?.Count ?? 0); i++)
                    {
                        var itemIndex = i; // avoid closure problem
                        var underlyingItem = _UnderlyingList[itemIndex];
                        WriteChild(ref w, ref underlyingItem, includeChildrenMode, ItemHasBeenAccessed(itemIndex), () => GetListMemberSlice(itemIndex), verifyCleanness, updateStoredBuffer, false, true /* skip length altogether */, this);
                        if (underlyingItem != null && underlyingItem.IsStruct)
                        { // the struct that was just written may be noted as dirty, but it's really clean. Cloning is the only safe way to get a clean hierarchy.
                            underlyingItem = underlyingItem.CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.NoBuffer);
                            _UnderlyingList[itemIndex] = underlyingItem;
                        }
                        var offset = (int)(w.Position - startingPosition);
                        offsetList.AddOffset(offset);
                    }
                });
                MainListSerialized = writer.LazinatorMemory.Memory.Slice(originalStartingPosition);
                _Offsets_Accessed = true;
                _Offsets = offsetList;
                _Offsets.IsDirty = true;
            }
            else
                ConvertToBytes_Memory_Gbyte_g(ref writer, MainListSerialized, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        }


        public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            // Do not enumerate offsets. Just enumerate items.
            for (int i = 0; i < Count; i++)
            {
                if (!exploreOnlyDeserializedChildren || ItemHasBeenAccessed(i))
                {
                    var item = this[i];
                    if (enumerateNulls && (item == null || item.Equals(default(T))))
                    {
                        yield return (i.ToString(), null);
                    }
                    else if (item != null && !(item.Equals(default(T))))
                    {
                        foreach (ILazinator toYield in item.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return (i.ToString(), toYield);
                        }
                    }
                }
            }
            yield break;
        }

        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren || includeChildrenMode == IncludeChildrenMode.ExcludeOnlyExcludableChildren)
            {
                LazinatorList<T> typedClone = (LazinatorList<T>)clone;
                foreach (T member in this)
                {
                    if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(member, default(T)))
                        typedClone.Add(default(T));
                    else
                        typedClone.Add(member.CloneLazinatorTyped(includeChildrenMode, CloneBufferOptions.NoBuffer));
                }
            }

            return this;
        }

        public void OnFreeInMemoryObjects()
        {
            _FullyDeserialized = false;
            _UnderlyingList = null;
            _ItemsAccessedBeforeFullyDeserialized = null;
            _CountWhenDeserialized = -1;
        }

        public void OnForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if (!exploreOnlyDeserializedChildren)
                FullyDeserialize();
            if (_UnderlyingList == null)
                return;
            for (int index = 0; index < _UnderlyingList.Count; index++)
            {
                if (_FullyDeserialized || _ItemsAccessedBeforeFullyDeserialized[index])
                {
                    var current = ((IList<T>)_UnderlyingList)[index];
                    if (current != null)
                    {
                        _UnderlyingList[index] = (T) changeFunc(current);
                    }
                }
            }
        }

    }
}
