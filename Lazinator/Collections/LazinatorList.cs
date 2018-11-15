using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Support;
using Lazinator.Buffers;
using Lazinator.Core;
using static Lazinator.Core.LazinatorUtilities;
using Lazinator.Attributes;
using System.Buffers;

namespace Lazinator.Collections
{
    [Implements(new string[] { "PreSerialization", "EnumerateLazinatorDescendants", "OnFreeInMemoryObjects", "AssignCloneProperties", "OnUpdateDeserializedChildren", "OnPropertiesWritten", "OnForEachLazinator" })]
    public partial class LazinatorList<T> : IList<T>, ILazinatorList<T>, ILazinatorList where T : ILazinator
    {
        [NonSerialized] private bool FullyDeserialized;
        [NonSerialized] private int CountWhenDeserialized;
        [NonSerialized] private List<T> UnderlyingList;
        [NonSerialized] private List<bool> ItemsAccessedBeforeFullyDeserialized;
        [NonSerialized] private int? FixedID;
        [NonSerialized] private bool TypeRequiresNonBinaryHashing;

        public LazinatorList()
        {
            FixedID = DeserializationFactory.Instance.GetFixedUniqueID(typeof(T));
            TypeRequiresNonBinaryHashing = DeserializationFactory.Instance.HasNonBinaryHashAttribute(typeof(T));
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
            if (UnderlyingList == null)
            {
                CountWhenDeserialized = Offsets?.Count ?? 0;

                UnderlyingList = new List<T>(CountWhenDeserialized);
                ItemsAccessedBeforeFullyDeserialized = new List<bool>(CountWhenDeserialized);
                for (int i = 0; i < CountWhenDeserialized; i++)
                {
                    UnderlyingList.Add(default);
                    ItemsAccessedBeforeFullyDeserialized.Add(false);
                }
            }
        }

        private void FullyDeserializeIfNecessary()
        {
            if (!FullyDeserialized)
            {
                CreateUnderlyingListIfNecessary();
                int count = Count;
                for (int i = 0; i < count; i++)
                {
                    if (!ItemsAccessedBeforeFullyDeserialized[i])
                        UnderlyingList[i] = GetSerializedContents(i);
                }
                FullyDeserialized = true;
            }
        }

        private T GetSerializedContents(int index)
        {
            var byteSpan = GetListMemberSlice(index);
            if (byteSpan.Length == 0)
                return default;
            T n2;
            if (FixedID == null)
                n2 = (T)DeserializationFactory.Instance.CreateFromBytesIncludingID(byteSpan, this);
            else
                n2 = (T)DeserializationFactory.Instance.CreateKnownID((int)FixedID, byteSpan, this);
            return n2;
        }

        public uint GetListMemberHash32(int index)
        {
            if (TypeRequiresNonBinaryHashing)
                return (uint) this[index].GetHashCode();

            if (FullyDeserialized || (UnderlyingList != null && ItemsAccessedBeforeFullyDeserialized[index]))
                return ((IList<T>)UnderlyingList)[index].GetBinaryHashCode32();
            
            var byteSpan = GetListMemberSlice(index);
            return FarmhashByteSpans.Hash32(byteSpan.Span);
        }

        private LazinatorMemory GetListMemberSlice(int index)
        {
            if (FullyDeserialized)
            { // we can't rely on original offsets, because an insertion/removal may have occurred
                T underlyingItem = UnderlyingList[index];
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
            if (Offsets == null || index >= Offsets.Count)
                return LazinatorUtilities.EmptyLazinatorMemory;
            int offset = GetOffset(index);
            int nextOffset = Offsets[index];

            var mainListSerializedStorage = GetChildSlice(LazinatorMemoryStorage, _MainListSerialized_ByteIndex, _MainListSerialized_ByteLength, false, false, null); // this is equivalent to MainListSerialized (omitting the length, containing the bytes). We don't use MainListSerialized itself because it's not slicesable
            return mainListSerializedStorage.Slice(offset, nextOffset - offset);
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

        public T this[int index]
        {
            get
            {
                if (FullyDeserialized)
                    return ((IList<T>) UnderlyingList)[index];
                CreateUnderlyingListIfNecessary();
                var current = ((IList<T>) UnderlyingList)[index];
                if (current == null || (current.Equals(default(T))))
                {
                    if (ItemsAccessedBeforeFullyDeserialized[index])
                        return default;
                    current = GetSerializedContents(index);
                    UnderlyingList[index] = current;
                    ItemsAccessedBeforeFullyDeserialized[index] = true;
                }
                return current;
            }
            set
            {
                var currentOccupant = this[index];
                if (currentOccupant != null)
                    currentOccupant.LazinatorParents = currentOccupant.LazinatorParents.WithRemoved(this);
                if (FullyDeserialized)
                {
                    ((IList<T>)UnderlyingList)[index] = value;
                    if (value != null)
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    MarkDirty();
                    return;
                }
                CreateUnderlyingListIfNecessary();
                ((IList<T>) UnderlyingList)[index] = value;
                if (value != null)
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                MarkDirty();
                ItemsAccessedBeforeFullyDeserialized[index] = true;
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
                if (UnderlyingList == null)
                {
                    CountWhenDeserialized = Offsets?.Count ?? 0;
                    return CountWhenDeserialized;
                }
                return ((IList<T>) UnderlyingList).Count; 
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
            // this is the one change to the list (other than changes to specific indices) that does not require us to fully deserialize,
            // because it doesn't change anything up to this point
            if (item != null)
                item.LazinatorParents = item.LazinatorParents.WithAdded(this);
            CreateUnderlyingListIfNecessary();
            ((IList<T>)UnderlyingList).Add(item);
            if (!FullyDeserialized)
                ItemsAccessedBeforeFullyDeserialized.Add(true);
            MarkDirty();
        }

        public virtual void Clear()
        {
            FullyDeserializeIfNecessary();
            ((IList<T>)UnderlyingList).Clear();
            MarkDirty();
        }

        public bool Contains(T item)
        {
            FullyDeserializeIfNecessary();
            return ((IList<T>)UnderlyingList).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            FullyDeserializeIfNecessary();
            ((IList<T>)UnderlyingList).CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            FullyDeserializeIfNecessary();
            return ((IList<T>)UnderlyingList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            FullyDeserializeIfNecessary();
            return ((IList<T>)UnderlyingList).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            FullyDeserializeIfNecessary();
            return ((IList<T>)UnderlyingList).IndexOf(item);
        }

        // Note: Instead of fully deserializing on inserts and removes, we might keep track of what has been inserted and what has been removed. But if high insert/removes are expected, or especially big lists, we can use AvlList<T> instead.

        public virtual void Insert(int index, T item)
        {
            FullyDeserializeIfNecessary();
            if (item != null)
                item.LazinatorParents = item.LazinatorParents.WithAdded(this);
            CreateUnderlyingListIfNecessary();
            ((IList<T>)UnderlyingList).Insert(index, item);
            MarkDirty();
        }

        public virtual bool Remove(T item)
        {
            FullyDeserializeIfNecessary();
            var success = ((IList<T>)UnderlyingList).Remove(item);
            if (success)
                MarkDirty();
            return success;
        }

        public virtual int RemoveAll(Predicate<T> match)
        {
            FullyDeserializeIfNecessary();
            int matches = 0;
            for (int i = Count - 1; i >= 0; i--) // iterate backwards so that indices stay same during loop
            { 
                var item = UnderlyingList[i];
                if (match(item))
                {
                    UnderlyingList.RemoveAt(i);
                    matches++;
                }
            }
            if (matches > 0)
                MarkDirty();
            return matches;
        }

        public virtual void RemoveAt(int index)
        {
            FullyDeserializeIfNecessary();
            ((IList<T>)UnderlyingList).RemoveAt(index);
            if (!FullyDeserialized)
                ItemsAccessedBeforeFullyDeserialized.RemoveAt(index);
            MarkDirty();
        }

        Memory<byte> _PreviousMainListSerialized;
        LazinatorOffsetList _PreviousOffsets;

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
            if (UnderlyingList == null)
                return;
            for (int index = 0; index < UnderlyingList.Count; index++)
            {
                if (FullyDeserialized || ItemsAccessedBeforeFullyDeserialized[index])
                {
                    var current = ((IList<T>)UnderlyingList)[index];
                    if (current != null)
                    {
                        current.UpdateStoredBuffer(ref writer, startPosition + _MainListSerialized_ByteIndex + sizeof(int) + GetOffset(index), GetOffset(index + 1) - GetOffset(index), IncludeChildrenMode.IncludeAllChildren, true);
                        if (current.IsStruct)
                        {
                            UnderlyingList[index] = current;
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
            return FullyDeserialized || ItemsAccessedBeforeFullyDeserialized[index];
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
                    if (UnderlyingList == null && CountWhenDeserialized > 0)
                        CreateUnderlyingListIfNecessary();
                    for (int i = 0; i < (UnderlyingList?.Count ?? 0); i++)
                    {
                        var itemIndex = i; // avoid closure problem
                        var underlyingItem = UnderlyingList[itemIndex];
                        WriteChild(ref w, ref underlyingItem, includeChildrenMode, ItemHasBeenAccessed(itemIndex), () => GetListMemberSlice(itemIndex), verifyCleanness, updateStoredBuffer, false, true /* skip length altogether */, this);
                        if (underlyingItem != null && underlyingItem.IsStruct)
                        { // the struct that was just written may be noted as dirty, but it's really clean. Cloning is the only safe way to get a clean hierarchy.
                            underlyingItem = underlyingItem.CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.NoBuffer);
                            UnderlyingList[itemIndex] = underlyingItem;
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

        protected virtual void AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
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
        }

        public void OnFreeInMemoryObjects()
        {
            FullyDeserialized = false;
            UnderlyingList = null;
            ItemsAccessedBeforeFullyDeserialized = null;
            CountWhenDeserialized = -1;
        }

        public void OnForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if (!exploreOnlyDeserializedChildren)
                FullyDeserializeIfNecessary();
            if (UnderlyingList == null)
                return;
            for (int index = 0; index < UnderlyingList.Count; index++)
            {
                if (FullyDeserialized || ItemsAccessedBeforeFullyDeserialized[index])
                {
                    var current = ((IList<T>)UnderlyingList)[index];
                    if (current != null)
                    {
                        UnderlyingList[index] = (T) changeFunc(current);
                    }
                }
            }
        }

    }
}
