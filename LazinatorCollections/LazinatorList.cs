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
using LazinatorCollections.OffsetList;
using LazinatorCollections.Interfaces;
using LazinatorCollections.Extensions;
using LazinatorCollections.Enumerators;
using LazinatorCollections.Location;
using Lazinator.Exceptions;

namespace LazinatorCollections
{
    /// <summary>
    /// A list of Lazinator objects. The list makes it possible to read or change some items without deserializing other previously serialized items.
    /// </summary>
    /// <typeparam name="T">The type of item stored</typeparam>
    [Implements(new string[] { "PreSerialization", "PostDeserialization", "EnumerateLazinatorDescendants", "OnFreeInMemoryObjects", "AssignCloneProperties", "OnUpdateDeserializedChildren", "OnPropertiesWritten", "OnForEachLazinator" })]
    public partial class LazinatorList<T> : IList, IList<T>, IEnumerable, ILazinatorList<T>, ILazinatorList, ILazinatorListable<T>, IIndexableMultivalueContainer<T>, IMultilevelReporter where T : ILazinator
    {
        #region Construction 

        public LazinatorList() : this(false)
        {
        }

        public virtual IValueContainer<T> CreateNewWithSameSettings()
        {
            return new LazinatorList<T>(AllowDuplicates);
        }

        public LazinatorList(bool allowDuplicates = true)
        {
            AllowDuplicates = allowDuplicates;
            PostDeserialization();
        }

        public LazinatorList(int numItems, bool allowDuplicates = true) : this(allowDuplicates)
        {
            for (int i = 0; i < numItems; i++)
                Add(default);
        }

        public LazinatorList(IEnumerable<T> items, bool allowDuplicates = true) : this(allowDuplicates)
        {
            foreach (T item in items)
                Add(item);
        }

        public void PostDeserialization()
        {
            _FixedID = DeserializationFactory.Instance.GetFixedUniqueID(typeof(T));
            _TypeRequiresNonBinaryHashing = DeserializationFactory.Instance.HasNonBinaryHashAttribute(typeof(T));
        }

        #endregion

        #region Item access and status

        // The status of an item currently in the list. To avoid unnecessary deserialization, we keep track of this in memory. 
        struct ItemStatus
        {
            public int OriginalIndex;
            public int DeserializedIndex;
            public bool IsInOriginalItems => OriginalIndex != -1;
            public bool IsDeserialized => DeserializedIndex != -1;

            public ItemStatus(int? originalIndex, int? indexInDeserializedItems)
            {
                OriginalIndex = originalIndex ?? -1;
                DeserializedIndex = indexInDeserializedItems ?? -1;
            }
        }

        [NonSerialized] private int _CountWhenDeserialized;
        [NonSerialized] private List<T> _DeserializedItems;
        [NonSerialized] private List<ItemStatus> _ItemsTracker; // if isDeserialized is true, then index is to _DeserializedItems; otherwise, it is an index to the original items
        [NonSerialized] private int? _FixedID;
        [NonSerialized] private bool _TypeRequiresNonBinaryHashing;
        [NonSerialized] private LazinatorOffsetList _PreviousOffsets;

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
        private bool ItemHasBeenAccessed(int currentIndex)
        {
            if (_ItemsTracker == null)
                return false;
            if (currentIndex >= _ItemsTracker.Count)
                throw new IndexOutOfRangeException();
            return _ItemsTracker[currentIndex].IsDeserialized;
        }

        /// <summary>
        /// Returns a string containing the items in the list, but no more than 10 items.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int maxNumItems = 10;
            return ToString(maxNumItems);
        }

        public string ToString(int maxNumItemsBeforeEllipsis)
        {
            var firstItems = this.Take(maxNumItemsBeforeEllipsis + 1).ToArray();
            bool moreThanMax = false;
            if (firstItems.Length == maxNumItemsBeforeEllipsis + 1)
            {
                moreThanMax = true;
                firstItems = this.Take(maxNumItemsBeforeEllipsis).ToArray();
            }
            return $"[{String.Join(", ", firstItems)}{(moreThanMax ? ", ..." : "")}]";
        }

        public T GetReadOnly(int index)
        {
            if (ItemHasBeenAccessed(index))
                return this[index].CloneLazinatorTyped();
            return GetSerializedContents(index);
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
                return ((IList<T>)_DeserializedItems)[status.DeserializedIndex].GetBinaryHashCode32();
            var byteSpan = GetListMemberSlice(status.OriginalIndex);
            return FarmhashByteSpans.Hash32(byteSpan.OnlyMemory.Span);
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
            return GetChildSlice(LazinatorMemoryStorage, _MainListSerialized_ByteIndex, _MainListSerialized_ByteLength, true, false, null);
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
                    return ((IList<T>) _DeserializedItems)[status.DeserializedIndex];
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
                ((IList<T>) _DeserializedItems)[status.DeserializedIndex] = value;
                if (value != null)
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                MarkDirty();
                ConsiderMultilevelReport(currentIndex);
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

        public virtual bool Contains(T item)
        {
            return (this.Any(x => System.Collections.Generic.EqualityComparer<T>.Default.Equals(x, item)));
        }

        public bool Contains(T item, IComparer<T> comparer)
        {
            var result = FindContainerLocation(item, comparer);
            return result.found;
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

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 0; i < Count; i++)
                array[arrayIndex + i] = this[i];
        }

        public bool Any()
        {
            return Count > 0;
        }

        #endregion

        #region Enumeration

        public IEnumerator<T> GetEnumerator()
        {
            return new ListableEnumerator<T>(this, false, 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListableEnumerator<T>(this, false, 0);
        }

        public IEnumerator<T> GetEnumerator(bool reverse = false, long skip = 0)
        {
            return new ListableEnumerator<T>(this, reverse, skip);
        }

        public IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            if (skip > Count || skip < 0)
                throw new ArgumentException();
            if (reverse)
            {
                for (int i = Count - 1 - (int)skip; i >= 0; i--)
                {
                    yield return this[i];
                }
            }
            else
            {
                for (int i = (int)skip; i < Count; i++)
                {
                    yield return this[i];
                }
            }
        }

        public IEnumerator<T> GetEnumerator(bool reverse, T startValue, IComparer<T> comparer) => this.MultivalueAsEnumerable<LazinatorList<T>, T>(reverse, startValue, comparer).GetEnumerator();

        public IEnumerable<T> AsEnumerable(bool reverse, T startValue, IComparer<T> comparer) => this.MultivalueAsEnumerable<LazinatorList<T>, T>(reverse, startValue, comparer);

        #endregion

        #region Insertion

        public virtual void Add(T item)
        {
            CompleteAdd(item);
        }

        // Completes the addition. Allows subclasses to override Add to throw NotImplementedException.
        protected internal void CompleteAdd(T item)
        {
            CompleteInsert(Count, item);
        }

        public virtual void InsertAt(IContainerLocation location, T item) => InsertAtIndex(location.IsAfterContainer ? Count : (int)((IndexLocation)location).Index, item);

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
            ConsiderMultilevelReport(index);
        }

        #endregion

        #region Removal

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

        public void RemoveAt(IContainerLocation location) => RemoveAt((int) ((IndexLocation)location).Index);

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
                _DeserializedItems[status.DeserializedIndex] = default;
            }
            MarkDirty();
            ConsiderMultilevelReport(currentIndex);
        }

        #endregion

        #region Serialization

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
                    var current = ((IList<T>)_DeserializedItems)[status.DeserializedIndex];
                    if (current != null)
                    {
                        current.UpdateStoredBuffer(ref writer, startPosition + _MainListSerialized_ByteIndex + GetOffset(index), GetOffset(index + 1) - GetOffset(index), IncludeChildrenMode.IncludeAllChildren, true);
                        if (current.IsStruct)
                        {
                            _DeserializedItems[status.DeserializedIndex] = current;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Writes the main list to a binary buffer, using serialized data where possible so that items do not need to be deserialized unnecessarily.
        /// </summary>
        private void WriteMainList(ref BinaryBufferWriter writer, ReadOnlyMemory<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            int originalStartingPosition = writer.ActiveMemoryPosition;
            if (IsDirty || DescendantIsDirty || includeChildrenMode != OriginalIncludeChildrenMode || LazinatorMemoryStorage.IsEmpty)
            {
                var offsetList = new LazinatorOffsetList();
                LazinatorUtilities.WriteToBinaryWithoutLengthPrefix(ref writer, (ref BinaryBufferWriter w) =>
                {
                    int startingPosition = w.ActiveMemoryPosition;
                    if (_DeserializedItems == null && _CountWhenDeserialized > 0)
                        SetupItemsTracker();
                    for (int i = 0; i < (_ItemsTracker?.Count ?? 0); i++)
                    {
                        var itemIndex = i; // avoid closure problem
                        var status = _ItemsTracker[itemIndex];
                        if (status.IsDeserialized)
                        {
                            var underlyingItem = _DeserializedItems[status.DeserializedIndex];
                            WriteChild(ref w, ref underlyingItem, includeChildrenMode, true, () => status.IsInOriginalItems ? GetListMemberSlice(status.OriginalIndex) : LazinatorMemory.EmptyLazinatorMemory, verifyCleanness, updateStoredBuffer, false, true /* skip length altogether */, this);
                            if (underlyingItem != null && underlyingItem.IsStruct)
                            { // the struct that was just written may be noted as dirty, but it's really clean. Cloning is the only safe way to get a clean hierarchy.
                                underlyingItem = underlyingItem.CloneNoBuffer();
                                _DeserializedItems[status.DeserializedIndex] = underlyingItem;
                            }
                        }
                        else
                            WriteExistingChildStorage(ref w, () => GetListMemberSlice(status.OriginalIndex), false, true, LazinatorMemory.EmptyLazinatorMemory);
                        var offset = (int)(w.ActiveMemoryPosition - startingPosition);
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
                mainListSerializedStorage.WriteToBinaryBuffer(ref writer);
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
                    if (this[i] != null)
                    {
                        this[i] = (T)current.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
                    }
                }
            }
        }

        #endregion

        #region Interface implementations 

        public long LongCount => Count;
        
        public bool Unbalanced { get => false; set => throw new NotSupportedException(); }

        public void InsertAtIndex(long index, T item)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            Insert((int) index, item);
        }

        public void RemoveAtIndex(long index)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            RemoveAt((int)index);
        }

        public T GetAtIndex(long index)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            return this[(int) index];
        }

        public void SetAtIndex(long index, T value)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            this[(int)index] = value;
        }

        public bool ShouldSplit(long splitThreshold)
        {
            return Count > splitThreshold;
        }

        public bool IsShorterThan(IValueContainer<T> second)
        {
            return Count < ((LazinatorList<T>)second).Count;
        }

        public IValueContainer<T> SplitOff()
        {
            LazinatorList<T> partSplitOff = (LazinatorList<T>)CreateNewWithSameSettings();
            int numToMove = Count / 2;
            for (int i = 0; i < numToMove; i++)
            {
                partSplitOff.Add(this[0]);
                RemoveAt(0);
            }
            return partSplitOff;
        }

        public T First()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return this[0];
        }

        public T FirstOrDefault()
        {
            if (Any())
                return this[0];
            return default(T);
        }

        public T Last()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return this[Count - 1];
        }

        public T LastOrDefault()
        {
            if (Any())
                return this[Count - 1];
            return default(T);
        }

        public IContainerLocation FirstLocation() => new IndexLocation(0, LongCount);
        public IContainerLocation LastLocation() => new IndexLocation(LongCount - 1, LongCount);
        public T GetAt(IContainerLocation location) => GetAtIndex(((IndexLocation)location).Index);
        public void SetAt(IContainerLocation location, T value) => SetAtIndex(((IndexLocation)location).Index, value);

        public (IContainerLocation location, bool found) FindContainerLocation(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer) => this.MultivalueFindMatchOrNext(AllowDuplicates, value, whichOne, comparer);
        public bool GetValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, out T match) => this.MultivalueGetValue(AllowDuplicates, item, whichOne, comparer, out match);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => this.SortedInsertOrReplace(AllowDuplicates, item, whichOne, comparer);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => this.MultivalueTryRemove(AllowDuplicates, item, whichOne, comparer);
        public bool TryRemoveAll(T item, IComparer<T> comparer) => this.MultivalueTryRemoveAll(AllowDuplicates, item, comparer);
        long IMultivalueContainer<T>.Count(T item, IComparer<T> comparer) => this.MultivalueCount(AllowDuplicates, item, comparer);
        public (IContainerLocation location, bool found) FindContainerLocation(T value, IComparer<T> comparer) => this.MultivalueFindMatchOrNext(AllowDuplicates, value, comparer);
        public bool GetValue(T item, IComparer<T> comparer, out T match) => this.MultivalueGetValue(AllowDuplicates, item, comparer, out match);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, IComparer<T> comparer) => this.SortedInsertOrReplace(AllowDuplicates, item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any, comparer);
        public bool TryRemove(T item, IComparer<T> comparer) => this.MultivalueTryRemove(AllowDuplicates, item, comparer);
        
        #endregion

        #region IIndexable

        public (long index, bool exists) FindIndex(T target, IComparer<T> comparer) => FindIndex(target, MultivalueLocationOptions.Any, comparer);

        public (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var result = FindContainerLocation(target, whichOne, comparer);
            return (((IndexLocation)result.location).Index, result.found);

        }

        public void RemoveAt(long index)
        {
            RemoveAt((int)index);
        }

        #endregion

        #region IList

        public bool IsFixedSize => false;

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        object IList.this[int index] { get => this[index]; set => this[index] = (T) value; }

        public int Add(object value)
        {
            Add((T)value);
            return Count;
        }

        public bool Contains(object value)
        {
            if (value is T t)
                return Contains(t);
            return false;
        }

        public int IndexOf(object value)
        {
            if (value is T t)
                return IndexOf(t);
            return -1;
        }

        public void Insert(int index, object value)
        {
            if (value is T t)
                Insert(index, t);
        }

        public void Remove(object value)
        {
            if (value is T t)
                Remove(t);
        }

        public void CopyTo(Array array, int index)
        {
            for (int i = 0; i < Count; i++)
                array.SetValue(this[i], index + i);
        }

        #endregion

        #region IMultilevelReporter
        
        public IMultilevelReportReceiver MultilevelReporterParent { get; set; }

        protected void ConsiderMultilevelReport(long index)
        {
            if (index == 0)
                ReportFirstChanged();
            if (index >= LongCount - 1)
                ReportLastChanged();
        }

        protected void ReportFirstChanged()
        {
            if (Any())
                MultilevelReporterParent?.EndItemChanged(true, First(), this);
            else
                MultilevelReporterParent?.EndItemRemoved(true, this);
        }

        protected void ReportLastChanged()
        {
            if (Any())
                MultilevelReporterParent?.EndItemChanged(false, Last(), this);
            else
                MultilevelReporterParent?.EndItemRemoved(false, this);
        }

        #endregion
    }
}
