using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Lazinator.Exceptions;
using Lazinator.Support;
using Lazinator.Buffers; 
using Lazinator.Core;
using static Lazinator.Core.LazinatorUtilities;
using System.Linq;

namespace Lazinator.Collections
{
    public partial class LazinatorList<T> : IList<T>, ILazinatorList<T> where T : ILazinator
    {
        [NonSerialized] private bool FullyDeserialized;
        [NonSerialized] private int CountWhenDeserialized;
        [NonSerialized] private List<T> UnderlyingList;
        [NonSerialized] private List<bool> ItemsAccessedBeforeFullyDeserialized;
        [NonSerialized] private int? FixedID;

        public LazinatorList()
        {
            FixedID = DeserializationFactory.Instance.GetFixedUniqueID(typeof(T));
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
            if (FullyDeserialized || (UnderlyingList != null && ItemsAccessedBeforeFullyDeserialized[index]))
                return ((IList<T>)UnderlyingList)[index].GetBinaryHashCode32();
            
            var byteSpan = GetListMemberSlice(index);
            return FarmhashByteSpans.Hash32(byteSpan.Span);
        }

        private ReadOnlyMemory<byte> GetListMemberSlice(int index)
        {
            // The 1st item (# 0) has index 0 always, so it's not stored in Offsets.
            // If we have three items, we store the offset of the second and the third,
            // plus the location AFTER the third.
            // The offset of the first is then 0 and the next offset is Offsets[0].
            // The offset of the second is then Offsets[0] and next offset is Offsets[1].
            // The offste of the third is Offsets[1] and the next offset is Offsets[2], the position at the end of the third item.
            if (Offsets == null || index >= Offsets.Count)
                return new ReadOnlyMemory<byte>();
            int offset;
            if (index == 0)
                offset = 0;
            else
                offset = Offsets[index - 1];
            int nextOffset = Offsets[index];
            var byteSpan = MainListSerialized.Slice(offset, nextOffset - offset);
            return byteSpan;
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
                if (FullyDeserialized)
                {
                    ((IList<T>)UnderlyingList)[index] = value;
                    return;
                }
                CreateUnderlyingListIfNecessary();
                ((IList<T>) UnderlyingList)[index] = value;
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
                    return CountWhenDeserialized;
                return ((IList<T>) UnderlyingList).Count; 
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            // this is the one change to the list (other than changes to specific indices) that does not require us to fully deserialize
            if (item != null)
                item.LazinatorParentClass = this;
            CreateUnderlyingListIfNecessary();
            ((IList<T>)UnderlyingList).Add(item);
            if (!FullyDeserialized)
                ItemsAccessedBeforeFullyDeserialized.Add(true);
            MarkDirty();
        }

        public void Clear()
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

        public void Insert(int index, T item)
        {
            if (item != null)
                item.LazinatorParentClass = this;
            CreateUnderlyingListIfNecessary();
            ((IList<T>)UnderlyingList).Insert(index, item);
            if (!FullyDeserialized)
                ItemsAccessedBeforeFullyDeserialized.Insert(index, true);
            MarkDirty();
        }

        public bool Remove(T item)
        {
            FullyDeserializeIfNecessary();
            MarkDirty();
            return ((IList<T>)UnderlyingList).Remove(item);
        }

        public void RemoveAt(int index)
        {
            CreateUnderlyingListIfNecessary();
            ((IList<T>)UnderlyingList).RemoveAt(index);
            if (!FullyDeserialized)
                ItemsAccessedBeforeFullyDeserialized.RemoveAt(index);
            MarkDirty();
        }

        public virtual void PreSerialization(bool verifyCleanness, bool updateStoredBuffer)
        {
            if (IsDirty || DescendantIsDirty)
            {
                var mainListSerialized = MainListSerialized; // has side effect of loading _MainListSerialized and setting _MainListSerialized_Accessed to true, thus making sure we call WriteMainList
            }
        }

        private bool ItemHasBeenAccessed(int index)
        {
            return FullyDeserialized || ItemsAccessedBeforeFullyDeserialized[index];
        }

        private void WriteMainList(BinaryBufferWriter writer, ReadOnlyMemory<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (IsDirty || DescendantIsDirty)
            {
                var offsetList = new LazinatorOffsetList();
                int originalStartingPosition = writer.Position;
                LazinatorUtilities.WriteToBinaryWithoutLengthPrefix(writer, w =>
                {
                    int startingPosition = w.Position;
                    for (int i = 0; i < (UnderlyingList?.Count ?? 0); i++)
                    {
                        var itemIndex = i; // avoid closure problem
                        WriteChild(w, UnderlyingList[itemIndex], IncludeChildrenMode.IncludeAllChildren, ItemHasBeenAccessed(itemIndex), () => GetListMemberSlice(itemIndex), verifyCleanness, updateStoredBuffer, false, true /* skip length altogether */, this);
                        var offset = (int)(w.Position - startingPosition);
                        offsetList.AddOffset(offset);
                    }
                });
                MainListSerialized = writer.MemoryInBuffer.FilledMemory.Slice(originalStartingPosition);
                _Offsets_Accessed = true;
                _Offsets = offsetList;
                _Offsets.IsDirty = true;
            }
            else
                ConvertToBytes_ReadOnlyMemory_Gbyte_g(writer, MainListSerialized, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        }

    }
}
