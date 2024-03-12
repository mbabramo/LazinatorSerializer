using Lazinator.Wrappers;
using Lazinator.Collections.BitArray;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CountedTree.UintSets
{
    public partial class UintSet : IUintSet
    {

        public uint Count => (UsingHashedItems) ? (uint) HashedItems.Count : (uint) UpdateCardinality(); 

        bool UsingHashedItems => HashedItems != null; // we'll start by using a list, but will change if the list gets long

        public UintSet()
        {
            HashedItems = new HashSet<WUInt32>();
        }

        public UintSet(HashSet<WUInt32> items)
        {
            SetHashedItems(items);
        }

        private void SetHashedItems(HashSet<WUInt32> items)
        {
            HashedItems = items;
            if (!UseHashedItems())
                ConvertToBitArray();
        }

        public UintSet(string binaryString)
        {
            var items = new HashSet<WUInt32>();
            for (uint i = 0; i < binaryString.Length; i++)
            {
                char c = binaryString[(int) i];
                if (c != '0' && c != '1')
                    throw new Exception("UintSet string must include only 0s and 1s");
                if (c == '1')
                    items.Add(i);
            }
            SetHashedItems(items);
        }

        public UintSet(LazinatorBitArray items)
        {
            Bits = items;
            UpdateCardinality();
        }

        public override bool Equals(object obj)
        {
            UintSet other = obj as UintSet;
            if (other.Equals(null))
                return false;
            return Length() == other.Length() && AsEnumerable().SequenceEqual(other.AsEnumerable());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;

                // get hash code for all items in array
                foreach (var item in AsEnumerable())
                {
                    hash = hash * 37 + item.GetHashCode();
                }

                return hash;
            }
        }

        private uint Length()
        {
            if (UsingHashedItems)
                return (MaxHashedItem() ?? 0) + 1;
            else
                return (uint) Bits.Count;
        }

        private uint? MaxHashedItem()
        {
            if (HashedItems.Any())
                return HashedItems.Max();
            return null;
        }

        private uint FutureMaxHashedItem(uint newItem)
        {
            uint? maxHashed = MaxHashedItem();
            if (maxHashed == null)
                return newItem;
            return (uint) Math.Max((int) newItem, (int) maxHashed);
        }

        public Int32 UpdateCardinality()
        {
            if (_BitsCount == null)
                _BitsCount = GetCardinality(Bits);
            return (int)_BitsCount;
        }

        public static Int32 GetCardinality(LazinatorBitArray bitArray)
        {

            Int32[] ints = new Int32[(bitArray.Count >> 5) + 1];

            bitArray.CopyTo(ints, 0);

            Int32 count = 0;

            // fix for not truncated bits in last integer that may have been set to true with SetAll()
            ints[ints.Length - 1] &= ~(-1 << (bitArray.Count % 32));

            for (Int32 i = 0; i < ints.Length; i++)
            {

                Int32 c = ints[i];

                // magic (http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel)
                unchecked
                {
                    c = c - ((c >> 1) & 0x55555555);
                    c = (c & 0x33333333) + ((c >> 2) & 0x33333333);
                    c = ((c + (c >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
                }

                count += c;

            }

            return count;

        }

        private bool UseHashedItems()
        {
            if (!UsingHashedItems)
                return false; // for simplicity, once we go to bit array, we stick with it.
            int numItems = HashedItems.Count();
            if (numItems <= 25)
                return true; // to avoid premature bitarray, we stick with hashed set initially
            int sizeOfHashSet = numItems * 16; // 4 bytes for int plus 12 bytes per slot
            int sizeOfBitArray = ((int?) MaxHashedItem() ?? 0) / 8;
            return sizeOfHashSet < sizeOfBitArray;
        }

        private bool UseHashedItemsWhenAdding(uint newItem)
        {
            if (!UsingHashedItems)
                return false; // for simplicity, once we go to bit array, we stick with it.
            int numItems = HashedItems.Count() + 1;
            if (numItems <= 25)
                return true; // to avoid premature bitarray, we stick with hashed set initially
            int sizeOfHashSet = numItems * 16; // 4 bytes for int plus 12 bytes per slot
            int sizeOfBitArray = (int) FutureMaxHashedItem(newItem) / 8;
            return sizeOfHashSet < sizeOfBitArray;
        }

        public LazinatorBitArray AsBitArray(int? length = null)
        {
            if (length == null)
            {
                if (UsingHashedItems)
                {
                    if (HashedItems.Any())
                        length = (int)(uint)HashedItems.Max() + 1;
                    else
                        length = 0;
                }
                else
                    length = Bits.Length;
            }
            LazinatorBitArray bits;
            if (UsingHashedItems)
                bits = GetBitArrayFromHashSet((int)length);
            else
            {
                if (Bits.Length == length)
                    return Bits;
                bits = new LazinatorBitArray(Bits);
                bits.Length = (int) length;
            }
            return bits;
        }

        private LazinatorBitArray GetBitArrayFromHashSet(int length)
        {
            if (!UsingHashedItems)
                throw new Exception("Internal error. Can only get bit array if using a bit array.");
            var bits = new LazinatorBitArray(length);
            foreach (uint i in HashedItems)
                bits.Set((int)i, true);
            return bits;
        }

        private void ConvertToBitArray(uint? newItem = null)
        {
            if (newItem != null)
                HashedItems.Add((uint) newItem);
            Bits = GetBitArrayFromHashSet((int)(uint) HashedItems.Max() + 1);
            UpdateCardinality();
            HashedItems = null;
        }

        public UintSet Clone()
        {
            if (UsingHashedItems)
                return new UintSet(new HashSet<WUInt32>(HashedItems.AsEnumerable()));
            else
                return new UintSet((LazinatorBitArray) Bits.Clone());
        }

        private void AddToBitArray(uint newItem)
        {
            while (Bits.Length < newItem + 1)
                Bits.Length = (int) (Bits.Length * 1.5);
            Bits.Set((int)newItem, true);
            if (_BitsCount == null)
                UpdateCardinality();
            _BitsCount++;
        }

        private void RemoveFromBitArray(uint newItem)
        {
            Bits.Set((int)newItem, false);
            if (_BitsCount == null)
                UpdateCardinality();
            _BitsCount--;
        }

        //private void ConfirmBitsCount()
        //{
        //    if (_BitsCount != GetCardinality(Bits))
        //        throw new Exception();
        //}

        private bool IsInBitArray(uint index)
        {
            if (index > Bits.Length - 1)
                return false;
            return Bits.Get((int) index);
        }

        public void AddUints(IEnumerable<WUInt32> items)
        {
            foreach (var item in items)
                AddUint(item);
        }

        public void AddUint(uint i)
        {
            if (!Contains(i))
            {
                if (UseHashedItemsWhenAdding(i))
                    HashedItems.Add(i);
                else
                {
                    if (UsingHashedItems)
                        ConvertToBitArray(i);
                    else
                        AddToBitArray(i);
                }
            }
        }

        public void RemoveUints(IEnumerable<WUInt32> items)
        {
            foreach (var item in items)
                RemoveUint(item);
        }

        public void RemoveUint(uint i)
        {
            if (Contains(i))
            {
                if (UsingHashedItems)
                    HashedItems.Remove(i);
                else
                    RemoveFromBitArray(i);
            }
        }

        public bool Contains(uint i)
        {
            if (UsingHashedItems)
                return HashedItems.Contains(i);
            else
                return IsInBitArray(i);
        }

        public IEnumerable<WUInt32> AsEnumerable()
        {
            if (UsingHashedItems)
            {
                foreach (uint i in HashedItems.OrderBy(x => x))
                    yield return i;
            }
            else
            {
                for (int i = 0; i < Bits.Length; i++)
                    if (Bits.Get(i))
                        yield return (uint) i;
            }
        }

        private void GetAsBitArrays(UintSet other, out LazinatorBitArray b1, out LazinatorBitArray b2)
        {
            int length = (int) Math.Max(Length(), other.Length());
            b1 = AsBitArray(length);
            b2 = other.AsBitArray(length);
        }

        // NOTE: Somewhat counterintuitively, the BitArray And, Or etc. methods mutate the BitArray and then return the mutated bitarray. So, we need to clone them first.

        public UintSet Union(UintSet other)
        {
            if (UsingHashedItems && other.UsingHashedItems)
                return new UintSet(new HashSet<WUInt32>(HashedItems.Union(other.HashedItems)));
            else
            {
                LazinatorBitArray b1, b2;
                GetAsBitArrays(other, out b1, out b2);
                LazinatorBitArray combined = ((LazinatorBitArray) b1.Clone()).Or(b2);
                return new UintSet(combined);
            }
        }

        public UintSet Except(UintSet other)
        {
            if (UsingHashedItems && other.UsingHashedItems)
                return new UintSet(new HashSet<WUInt32>(HashedItems.Except(other.HashedItems)));
            else
            {
                LazinatorBitArray b1, b2;
                GetAsBitArrays(other, out b1, out b2);
                LazinatorBitArray combined = ((LazinatorBitArray)b1.Clone()).And(((LazinatorBitArray)b2.Clone()).Not());
                return new UintSet(combined);
            }
        }


        public UintSet Intersect(UintSet other)
        {
            if (UsingHashedItems && other.UsingHashedItems)
                return new UintSet(new HashSet<WUInt32>(HashedItems.Intersect(other.HashedItems)));
            else
            {
                LazinatorBitArray b1, b2;
                GetAsBitArrays(other, out b1, out b2);
                LazinatorBitArray combined = ((LazinatorBitArray)b1.Clone()).And(b2);
                return new UintSet(combined);
            }
        }

        public bool IsEquivalentTo(UintSet other)
        {
            return Count == other.Count && Except(other).Count == 0;
        }

        /// <summary>
        /// Modify a filter so that it can be applied to the subset of items indicated by this UintSet. In other words, remove (not just clear) bits that are missing here from the filter and return the revised filter. 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public UintSet RemoveUnsetBitsFromFilter(UintSet filter)
        {
            // Equivalently, for each bit set in the filter UintSet with index i, if there is a corresponding bit set in index i in this UintSet, then set a bit in the output corresponding to the index of the bit in this UintSet among those bits set in this UintSet (so, for example, index 0 if this is the first item set in this UintSet.

            UintSet output = new UintSet();
            uint j = 0;
            foreach (uint i in AsEnumerable())
            { // i.e., for each item in this UintSet
                if (filter.Contains(i))
                    output.AddUint(j);
                j++;
            }
            return output;
        }

        // TODO: Can we optimize this and the reverse operation by keeping track of how many items are from 0 to 999, etc.?

        /// <summary>
        /// Gets the index within those items present in the UintSet. For example, if the UintSet contains (4, 6, 8), then requesting this for 4 would return 0, 6 would return 1, and 8 would return 2; other items would trigger an exception.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="absoluteIndex"></param>
        /// <returns></returns>
        public int GetIndexWithinPresentItems(uint absoluteIndex)
        {
            if (!Contains(absoluteIndex))
                throw new Exception("Item was not present in UintSet.");
            int indexWithinPresentItems = 0;
            var enumerator = AsEnumerable().GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current == absoluteIndex)
                    break;
                indexWithinPresentItems++;
            }

            return indexWithinPresentItems;
        }


    }
}
