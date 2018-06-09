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
            // If we have three items, we store the offset of the first, the second, and after the third.
            if (Offsets == null || index >= Offsets.Count)
                return new ReadOnlyMemory<byte>();
            int offset;
            if (index == 0)
                offset = 0;
            else
                offset = Offsets[index - 1];
            int nextOffset = Offsets[index];
            var byteSpan = SerializedMainList.Slice(offset, nextOffset - offset);
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

        public virtual void MarkHierarchyClean()
        {
            _IsDirty = false;
            _DescendantIsDirty = false;
            if (FullyDeserialized)
                foreach (var item in UnderlyingList)
                    if (item != null)
                        item.MarkHierarchyClean();
            else
            {
                for (int i = 0; i < ItemsAccessedBeforeFullyDeserialized.Count; i++)
                {
                    if (ItemsAccessedBeforeFullyDeserialized[i])
                    {
                        var item2 = ((IList<T>)UnderlyingList)[i];
                        if (item2 != null)
                           item2.MarkHierarchyClean();
                    }
                }
            }
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
            if (item?.LazinatorParentClass != null)
                throw new MovedLazinatorException();
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
            if (item?.LazinatorParentClass != null)
                throw new MovedLazinatorException();
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
                BinaryBufferWriter writer = new BinaryBufferWriter(Math.Max(100, (int)(SerializedMainList.Length * 1.2))); // try to allocate enough space initially to minimize the risk of buffer recopies, but don't go overboard.
                _Offsets_Accessed = true;
                _Offsets = new LazinatorOffsetList();
                _Offsets.IsDirty = true;
                LazinatorUtilities.WriteToBinaryWithoutLengthPrefix(writer, w =>
                {
                    int startingPosition = w.Position;
                    for (int i = 0; i < (UnderlyingList?.Count ?? 0); i++)
                    {
                        var item = i; // avoid closure problem
                        WriteChild(w, UnderlyingList[item], IncludeChildrenMode.IncludeAllChildren, FullyDeserialized || ItemsAccessedBeforeFullyDeserialized[item], () => GetListMemberSlice(item), verifyCleanness, updateStoredBuffer, false, true /* skip length altogether */, this);
                        var offset = (int)(w.Position - startingPosition);
                        _Offsets.AddOffset(offset);
                    }
                });
                SerializedMainList = writer.MemoryInBuffer.FilledMemory;
            }
        }
        
    }
}
