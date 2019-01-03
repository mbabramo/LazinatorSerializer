﻿using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Support;
using Lazinator.Buffers;
using Lazinator.Core;
using static Lazinator.Core.LazinatorUtilities;
using Lazinator.Attributes;
using System.Buffers;
using System.Linq;
using Lazinator.Collections.OffsetList;

namespace Lazinator.Collections
{
    [Implements(new string[] { "PreSerialization", "EnumerateLazinatorDescendants", "OnFreeInMemoryObjects", "AssignCloneProperties", "OnUpdateDeserializedChildren", "OnPropertiesWritten", "OnForEachLazinator" })]
    public partial class LazinatorList<T> : IList<T>, IEnumerable, ILazinatorList<T>, ILazinatorList, ILazinatorCountableListable<T> where T : ILazinator
    {
        // The status of an item currently in the list.
        struct ItemStatus
        {
            public int OriginalIndex;
            public int IndexInDeserializedItems;
            public bool IsInOriginalItems => OriginalIndex != -1;
            public bool IsDeserialized => IndexInDeserializedItems != -1;

            public ItemStatus(int? originalIndex, int? indexInDeserializedItems)
            {
                OriginalIndex = originalIndex ?? -1;
                IndexInDeserializedItems = indexInDeserializedItems ?? -1;
            }
        }

        [NonSerialized] private int _CountWhenDeserialized;
        [NonSerialized] private List<T> _DeserializedItems;
        [NonSerialized] private List<ItemStatus> _ItemsTracker; // if isDeserialized is true, then index is to _DeserializedItems; otherwise, it is an index to the original items
        [NonSerialized] private int? _FixedID;
        [NonSerialized] private bool _TypeRequiresNonBinaryHashing;
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

        private void SetupItemsTracker()
        {
            if (_DeserializedItems == null)
            {
                _CountWhenDeserialized = Offsets?.Count ?? 0;
                _DeserializedItems = new List<T>();
                _ItemsTracker = new List<ItemStatus>(_CountWhenDeserialized);
                for (int i = 0; i < _CountWhenDeserialized; i++)
                {
                    _ItemsTracker.Add(new ItemStatus(i, null)); // these items have not yet been deserialized, so index is reference to original list
                }
            }
        }

        private T GetSerializedContents(int originalIndex)
        {
            var byteSpan = GetListMemberSlice(originalIndex);
            if (byteSpan.Length == 0)
                return default;
            T n2;
            if (_FixedID == null)
                n2 = (T)DeserializationFactory.Instance.CreateFromBytesIncludingID(byteSpan, this);
            else
                n2 = (T)DeserializationFactory.Instance.CreateKnownID((int)_FixedID, byteSpan, this);
            return n2;
        }

        public uint GetListMemberHash32(int currentIndex)
        {
            SetupItemsTracker();
            if (_TypeRequiresNonBinaryHashing)
                return (uint) this[currentIndex].GetHashCode();
            ItemStatus status = _ItemsTracker[currentIndex];
            if (status.IsDeserialized)
                return ((IList<T>)_DeserializedItems)[status.IndexInDeserializedItems].GetBinaryHashCode32();
            var byteSpan = GetListMemberSlice(status.OriginalIndex);
            return FarmhashByteSpans.Hash32(byteSpan.Span);
        }

        private LazinatorMemory GetListMemberSlice(int originalIndex)
        {
            // The 1st item (# 0) has index 0 always, so it's not stored in Offsets.
            // If we have three items, we store the offset of the second and the third,
            // plus the location AFTER the third.
            // The offset of the first is then 0 and the next offset is Offsets[0].
            // The offset of the second is then Offsets[0] and next offset is Offsets[1].
            // The offste of the third is Offsets[1] and the next offset is Offsets[2], the position at the end of the third item.
            if (Offsets == null || originalIndex >= Offsets.Count)
                return LazinatorMemory.EmptyLazinatorMemory;
            int offset = GetOffset(originalIndex);
            int nextOffset = Offsets[originalIndex];
            LazinatorMemory mainListSerializedStorage = GetMainListSerializedWithoutDeserializing();
            // this is equivalent to MainListSerialized (omitting the length, containing the bytes). We don't use MainListSerialized itself because it's not sliceable
            return mainListSerializedStorage.Slice(offset, nextOffset - offset);
        }

        private LazinatorMemory GetMainListSerializedWithoutDeserializing()
        {
            return GetChildSlice(LazinatorMemoryStorage, _MainListSerialized_ByteIndex, _MainListSerialized_ByteLength, false, false, null);
        }

        private int GetOffset(int index)
        {
            int offset;
            if (index == 0)
                offset = 0;
            else
                offset = Offsets[index - 1];
            return offset;
        }

        public T this[int currentIndex]
        {
            get
            {
                SetupItemsTracker();
                ItemStatus status = _ItemsTracker[currentIndex];
                if (status.IsDeserialized)
                    return ((IList<T>) _DeserializedItems)[status.IndexInDeserializedItems];
                var deserialized = GetSerializedContents(status.OriginalIndex);
                _DeserializedItems.Add(deserialized);
                _ItemsTracker[currentIndex] = new ItemStatus(null, _DeserializedItems.Count - 1);
                return deserialized;
            }
            set
            {
                var currentOccupant = this[currentIndex]; // will deserialize
                if (currentOccupant != null)
                    currentOccupant.LazinatorParents = currentOccupant.LazinatorParents.WithRemoved(this);
                ItemStatus status = _ItemsTracker[currentIndex];
                ((IList<T>) _DeserializedItems)[status.IndexInDeserializedItems] = value;
                if (value != null)
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                MarkDirty();
            }
        }

        private void MarkDirty()
        {
            IsDirty = true;
            DescendantIsDirty = true;
        }

        public int Count
        {
            get
            {
                if (_DeserializedItems == null)
                {
                    _CountWhenDeserialized = Offsets?.Count ?? 0;
                    return _CountWhenDeserialized;
                }
                return _ItemsTracker.Count; 
            }
        }

        public bool IsReadOnly => false;

        public virtual void Clear()
        {
            _DeserializedItems = new List<T>();
            _ItemsTracker = new List<ItemStatus>();
            MarkDirty();
        }

        struct Enumerator : IEnumerator<T>
        {
            LazinatorList<T> List;
            int Index;

            public Enumerator(LazinatorList<T> list)
            {
                Index = -1;
                List = list;
            }

            public T Current
            {
                get
                {
                    if (Index == -1)
                        throw new ArgumentException();
                    return List[Index];
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (Index == List.Count - 1)
                    return false;
                Index++;
                return true;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }

        public bool Contains(T item)
        {
            return (this.Any(x => System.Collections.Generic.EqualityComparer<T>.Default.Equals(x, item)));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < Count; i++)
                array[arrayIndex + i] = this[i];
        }

        public bool Any()
        {
            if (_ItemsTracker != null)
                return _ItemsTracker.Any();
            return _CountWhenDeserialized > 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LazinatorList<T>.Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LazinatorList<T>.Enumerator(this);
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
                if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(this[i], item))
                    return i;
            return -1;
        }

        public int IndexOf(Predicate<T> match)
        {
            for (int i = 0; i < Count; i++)
                if (match(this[i]))
                    return i;
            return -1;
        }

        public virtual void Add(T item)
        {
            CompleteAdd(item);
        }

        // Completes the addition. Allows subclasses to override Add to throw NotImplementedException.
        protected internal void CompleteAdd(T item)
        {
            CompleteInsert(Count, item);
        }

        public virtual void Insert(int index, T item)
        {
            CompleteInsert(index, item);
        }

        protected internal void CompleteInsert(int index, T item)
        {
            if (item != null)
                item.LazinatorParents = item.LazinatorParents.WithAdded(this);
            SetupItemsTracker();
            _DeserializedItems.Add(item);
            _ItemsTracker.Insert(index, new ItemStatus(null, _DeserializedItems.Count - 1));
            MarkDirty();
        }

        public virtual bool Remove(T item)
        {
            int i = IndexOf(item);
            if (i == -1)
                return false;
            RemoveAt(i);
            return true;
        }

        public virtual int RemoveAll(Predicate<T> match)
        {
            int matches = 0;
            int matchIndex = 0;
            do
            {
                matchIndex = IndexOf(match);
                if (matchIndex != -1)
                {
                    matches++;
                    RemoveAt(matchIndex);
                }
            }
            while (matchIndex != -1);
            return matches;
        }

        public virtual void RemoveAt(int currentIndex)
        {
            SetupItemsTracker();
            if (currentIndex >= Count || currentIndex < 0)
                throw new Exception("Invalid removal index.");
            var status = _ItemsTracker[currentIndex];
            _ItemsTracker.RemoveAt(currentIndex);
            if (status.IsDeserialized)
            {
                // Note: We don't remove from DeserializedItems if it's there, because then we 
                // would need to update every other index in the items tracker. However, we set it
                // to null to reduce memory burden.
                _DeserializedItems[status.IndexInDeserializedItems] = default;
            }
            MarkDirty();
        }

        public virtual void PreSerialization(bool verifyCleanness, bool updateStoredBuffer)
        {
            _PreviousOffsets = Offsets;
        }

        public void OnPropertiesWritten(bool updateStoredBuffer)
        {
            // either we have fully deserialized, or we have LazinatorMemoryStorage. Either way, we don't need MainListSerialized to be loaded.
            if (updateStoredBuffer)
            {
                // MainListSerialized and Offsets have been updated, and this will match the updated LazinatorMemoryStorage.
                _PreviousOffsets = null;
            }
            else
            {
                // Because LazinatorMemoryStorage is the same, we need to return MainListSerialized and Offsets to their previous values. We don't want to have the updated LazinatorMemoryStorage
                Offsets = _PreviousOffsets;
            }
        }

        // How do we ensure that after serialization occurs, the child items get updated?
        // If the list is clean, then the entire storage of the LazinatorList is written in one fell swoop. But UpdateStoredBuffer will be called if the list is instantiated. Then, OnUpdateDeserializedChildren will be called, and the active BinaryBufferWriter can be used.
        // If the list is dirty or has a dirty descendant, then the writing of properties will work as follows: 
        // If updateStoredBuffer is true, then after writing properties into buffer, we call UpdateStoredBuffer with updateDeserializedChildren = false. The expectation is that we'll update each child when writing the properties, so we do this in WriteMainList.  The WriteChild method there will do this assuming that the child is in memory, and the child will get the new buffer. 
        // But what happens if updateStoredBuffer is false? If that is so, WriteMainList still updates MainListSerialized and Offsets. But it then immediately switches them back after we update with updateStoredBuffer = false. This ensures that Offsets refers to the original LazinatorMemoryStorage. 

        public void OnUpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_DeserializedItems == null)
                return;
            for (int index = 0; index < _ItemsTracker.Count; index++)
            {
                var status = _ItemsTracker[index];
                if (status.IsDeserialized)
                {
                    var current = ((IList<T>)_DeserializedItems)[status.IndexInDeserializedItems];
                    if (current != null)
                    {
                        current.UpdateStoredBuffer(ref writer, startPosition + _MainListSerialized_ByteIndex + sizeof(int) + GetOffset(index), GetOffset(index + 1) - GetOffset(index), IncludeChildrenMode.IncludeAllChildren, true);
                        if (current.IsStruct)
                        {
                            _DeserializedItems[status.IndexInDeserializedItems] = current;
                        }
                    }
                }
            }
        }

        private bool ItemHasBeenAccessed(int currentIndex)
        {
            if (_ItemsTracker == null)
                return false;
            return _ItemsTracker[currentIndex].IsDeserialized;
        }

        private void WriteMainList(ref BinaryBufferWriter writer, ReadOnlyMemory<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            int originalStartingPosition = writer.Position;
            if (IsDirty || DescendantIsDirty || includeChildrenMode != OriginalIncludeChildrenMode || LazinatorMemoryStorage.IsEmpty)
            {
                var offsetList = new LazinatorOffsetList();
                LazinatorUtilities.WriteToBinaryWithoutLengthPrefix(ref writer, (ref BinaryBufferWriter w) =>
                {
                    int startingPosition = w.Position;
                    if (_DeserializedItems == null && _CountWhenDeserialized > 0)
                        SetupItemsTracker();
                    for (int i = 0; i < (_ItemsTracker?.Count ?? 0); i++)
                    {
                        var itemIndex = i; // avoid closure problem
                        var status = _ItemsTracker[itemIndex];
                        if (status.IsDeserialized)
                        {
                            var underlyingItem = _DeserializedItems[status.IndexInDeserializedItems];
                            WriteChild(ref w, ref underlyingItem, includeChildrenMode, true, () => status.IsInOriginalItems ? GetListMemberSlice(status.OriginalIndex) : LazinatorMemory.EmptyLazinatorMemory, verifyCleanness, updateStoredBuffer, false, true /* skip length altogether */, this);
                            if (underlyingItem != null && underlyingItem.IsStruct)
                            { // the struct that was just written may be noted as dirty, but it's really clean. Cloning is the only safe way to get a clean hierarchy.
                                underlyingItem = underlyingItem.CloneNoBuffer();
                                _DeserializedItems[status.IndexInDeserializedItems] = underlyingItem;
                            }
                        }
                        else
                            WriteExistingChildStorage(ref w, () => GetListMemberSlice(status.OriginalIndex), false, true, LazinatorMemory.EmptyLazinatorMemory);
                        var offset = (int)(w.Position - startingPosition);
                        offsetList.AddOffset(offset);
                    }
                });
                _Offsets_Accessed = true;
                _Offsets = offsetList;
                _Offsets.IsDirty = true;
            }
            else
            {
                LazinatorMemory mainListSerializedStorage = GetMainListSerializedWithoutDeserializing();
                ConvertToBytes_ReadOnlyMemory_Gbyte_g(ref writer, mainListSerializedStorage.ReadOnlyMemory, includeChildrenMode, verifyCleanness,
                    updateStoredBuffer);
            }
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
                    if (EqualityComparer<T>.Default.Equals(member, default))
                        typedClone.Add(default);
                    else
                        typedClone.Add(member.CloneLazinatorTyped(includeChildrenMode, CloneBufferOptions.NoBuffer));
                }
            }

            return clone;
        }

        public void OnFreeInMemoryObjects()
        {
            _DeserializedItems = null;
            _ItemsTracker = null;
            _CountWhenDeserialized = -1;
        }

        public void OnForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if (!exploreOnlyDeserializedChildren)
                SetupItemsTracker();
            if (_ItemsTracker == null && exploreOnlyDeserializedChildren)
                return;
            for (int i = 0; i < _ItemsTracker.Count; i++)
            {
                ItemStatus status = _ItemsTracker[i];
                if (!exploreOnlyDeserializedChildren || status.IsDeserialized)
                {
                    var current = this[i];
                    this[i] = (T) current.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
                }
            }
        }

        #region ILazinatorCountableListableFactory 

        public long LongCount => Count;

        public void InsertAt(long index, T item)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            Insert((int) index, item);
        }

        public void RemoveAt(long index)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            RemoveAt((int)index);
        }

        public IEnumerable<T> EnumerateFrom(long index)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            foreach (T t in this.Skip((int)index))
                yield return t;
        }

        public T GetAt(long index)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            return this[(int) index];
        }

        public void SetAt(long index, T value)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            this[(int)index] = value;
        }

        #endregion
    }
}
