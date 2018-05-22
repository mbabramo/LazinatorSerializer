using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Lazinator.Exceptions;
using Lazinator.Support;
using Lazinator.Buffers; 
using Lazinator.Core;
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Collections
{
    public partial class LazinatorList<T> : IList<T>, ILazinatorList<T> where T : ILazinator
    {
        [NonSerialized] private bool FullyDeserialized;
        [NonSerialized] private int CountWhenDeserialized;
        [NonSerialized] private List<T> UnderlyingList;
        [NonSerialized] private List<bool> ItemsAccessedBeforeFullyDeserialized;
        [NonSerialized] private ReadOnlyMemory<byte> SerializedMainList;

        public LazinatorList()
        {

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
            if (DeserializationFactory == null)
                throw new MissingDeserializationFactoryException();
            T n2 = (T) DeserializationFactory.FactoryCreate(byteSpan, this);
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
                if (current == null)
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

        // Structure of LazinatorObjectBytes (after length header, if a child, and other bytes of header)
        // uint: byte length of main list (excluding this, including the items themselves, excluding offsets). If list is null, then this is 0, and everything else is omitted.
        // *SerializedMainList
        // *foreach item
        // *  the item. This is the range returned by GetListMemberSlice. Length bytes are excluded, because they are included separately in offsets below.
        // offsets. this is serialized based on SelfSerializingOffsetList (including size information etc.), but we don't need to worry about that here
        //   second item: distance from first foreach (the number of bytes reference)
        //   ...
        //   after last item: where next thing after offset would start (but not used by this list)

        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;

            // we place the list first, before the offsets, because we won't know the offsets until after we've created the list

            // deserialize the list (but not individual items within it)
            int byteLengthOfMainList = span.ToInt32(ref bytesSoFar);
            if (byteLengthOfMainList == 0)
            {
                SerializedMainList = new Memory<byte>();
            }
            else
            {
                SerializedMainList = LazinatorObjectBytes.Slice(bytesSoFar, byteLengthOfMainList);
                bytesSoFar += byteLengthOfMainList;
            }

            // deserialize the length offsets (ignoring includeChildren)
            _Offsets_ByteIndex = bytesSoFar;
            if (byteLengthOfMainList != 0)
                bytesSoFar += span.ToInt32(ref bytesSoFar) + bytesSoFar;
        }

        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            includeChildrenMode = IncludeChildrenMode.IncludeAllChildren; // always include offset list (and its children)

            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);

            if (UnderlyingList == null)
            { 
                if (SerializedMainList.Length == 0)
                    writer.Write((uint) 0); // indicates empty list
                else
                { // Not necessarily null -- nothing has changed, so just write back the bytes (including the byte length, the number of items, the serialized main list, and the offset list, if they exist)
                    LazinatorObjectBytes.Span.Write(writer);
                }
                return;
            }
            
            // we need to start with the byte length of the entire list
            _Offsets_Accessed = true;
            _Offsets = new LazinatorOffsetList();
            _Offsets.IsDirty = true;
            LazinatorUtilities.WriteToBinaryWithIntLengthPrefix(writer, w =>
            {
                int startingPosition = w.Position;
                for (int i = 0; i < UnderlyingList.Count; i++)
                {
                    var item = i; // avoid closure problem
                    LazinatorUtilities.WriteChildWithoutLength(w, UnderlyingList[item], includeChildrenMode, FullyDeserialized || ItemsAccessedBeforeFullyDeserialized[item], () => GetListMemberSlice(item), verifyCleanness);
                    var offset = (int) (w.Position - startingPosition);
                    _Offsets.AddOffset(offset);
                }
            });
            // Write the offsets (including size information). Do this regardless of whether there is anything in the list.
            LazinatorUtilities.WriteChildWithLength(writer, _Offsets, includeChildrenMode, _Offsets_Accessed, () => GetChildSlice(_Offsets_ByteIndex, _Offsets_ByteLength), verifyCleanness, false);
        }

    }
}
