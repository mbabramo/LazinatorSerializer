using CountedTree.ByteUtilities;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace CountedTree.UintSets
{
    /// <summary>
    /// Stores information about the location of every element within a UintSet (i.e., within which child a particular item in a UintSet is located)
    /// </summary>
    public partial class UintSetLoc : IUintSetLoc
    {
        public int NumItems => Storage.Count;
        public byte this[int index]
        {
            get
            {
                return Storage[index];
            }
        }
        public ReadOnlySpan<byte> UncompressedLocations
        {
            get
            {
                return Storage.AsUncompressed();
            }
            set
            {
                Storage = new HalfBytesStorage(NoMoreThan16ChildrenPerNode, value.ToArray());
            }
        }
        public IEnumerator<byte> GetLocationsEnumerator() => Storage.GetEnumerator();

        public UintSetLoc(bool noMoreThan16ChildrenPerNode) : this(noMoreThan16ChildrenPerNode, new byte[0])
        {
        }

        public UintSetLoc(bool noMoreThan16ChildrenPerNode, ReadOnlySpan<byte> locations)
        {
            NoMoreThan16ChildrenPerNode = noMoreThan16ChildrenPerNode;
            UncompressedLocations = locations.ToArray();
        }

        public UintSetLoc Clone()
        {
            return new UintSetLoc(NoMoreThan16ChildrenPerNode, UncompressedLocations);
        }

        /// <summary>
        /// Gets the location for an item at a particular index within the UintSet
        /// </summary>
        /// <param name="u"></param>
        /// <param name="idOfItemInUintSet"></param>
        /// <returns></returns>
        public byte GetLocationOfItem(UintSet u, uint idOfItemInUintSet)
        {
            int indexWithinPresentItems = u.GetIndexWithinPresentItems(idOfItemInUintSet);
            return this[indexWithinPresentItems];
        }

        public UintSetLoc AddItemsAtUintIndices(UintSet previousUintSet, List<WUInt32> indicesToAdd, List<byte> locationsToAdd)
        {
            // Example: UintSet is 0010100 and Loc is 2,4. We must store now for 1100010 the locations 5,6,9. So we should end up with 5,6,2,4,9.
            // So, we go through all of the indices of the UintSet. When we come to an index to add, we add it. When we come to an index that is already set, we take from the existing locations.
            SortIfNecessary(ref indicesToAdd, ref locationsToAdd);
            RemoveAdjacentRepeatsIfNecessary(ref indicesToAdd, ref locationsToAdd);
            uint uintSetSize = previousUintSet.Count;
            int numCurrentlyStored = UncompressedLocations.Length;
            if ((long)uintSetSize != (long)numCurrentlyStored)
                throw new Exception("Internal error. Initial version of UintSetLoc does not correspond to previousUintSet.");
            var newItemsToStore = indicesToAdd.Count(x => !previousUintSet.Contains(x));
            int revisedNumberToStore = numCurrentlyStored + newItemsToStore;
            var oldIndicesEnumerator = (previousUintSet.AsEnumerable()).GetEnumerator();
            IEnumerator<WUInt32> newIndicesEnumerator = indicesToAdd.GetEnumerator();
            IEnumerator<byte> oldLocationsEnumerator = GetLocationsEnumerator();
            IEnumerator<byte> newLocationsEnumerator = locationsToAdd.GetEnumerator();
            bool moreOldIndices = oldIndicesEnumerator.MoveNext();
            bool moreNewIndices = newIndicesEnumerator.MoveNext();
            bool moreOldLocations = oldLocationsEnumerator.MoveNext();
            bool moreNewLocations = newLocationsEnumerator.MoveNext();
            BinaryWriter bw = new BinaryWriter(revisedNumberToStore);
            while (moreOldIndices || moreNewIndices)
            {
                uint i = Math.Min(moreOldIndices ? (uint) oldIndicesEnumerator.Current : uint.MaxValue, moreNewIndices ? (uint) newIndicesEnumerator.Current : uint.MaxValue);
                bool isNewIndex = moreNewIndices && newIndicesEnumerator.Current == i;
                bool isOldIndex = moreOldIndices && oldIndicesEnumerator.Current == i;
                if (isNewIndex)
                {
                    if (!moreNewLocations)
                        throw new Exception("Internal error. New location doesn't exist.");
                    bw.WriteByte(newLocationsEnumerator.Current);
                    moreNewLocations = newLocationsEnumerator.MoveNext();
                    moreNewIndices = newIndicesEnumerator.MoveNext();
                    if (isOldIndex)
                    { // overwriting this location, so move to next one
                        moreOldLocations = oldLocationsEnumerator.MoveNext();
                        moreOldIndices = oldIndicesEnumerator.MoveNext();
                    }
                }
                else if (isOldIndex)
                {
                    if (!moreOldLocations)
                        throw new Exception("Internal error. Old location doesn't exist.");
                    bw.WriteByte(oldLocationsEnumerator.Current);
                    moreOldLocations = oldLocationsEnumerator.MoveNext();
                    moreOldIndices = oldIndicesEnumerator.MoveNext();
                }
            }
            bw.ShortenIfNecessary();
            return new UintSetLoc(NoMoreThan16ChildrenPerNode, bw.Bytes);
        }

        private static void RemoveAdjacentRepeatsIfNecessary(ref List<WUInt32> indicesToAdd, ref List<byte> locationsToAdd)
        {
            if (indicesToAdd.ContainsAdjacentRepeats())
            {
                var zipped = indicesToAdd.Zip(locationsToAdd, (x, y) => new Tuple<uint, byte>(x, y));
                indicesToAdd = new List<WUInt32>();
                locationsToAdd = new List<byte>();
                bool isFirst = true;
                uint lastIndex = 0;
                foreach (var item in zipped)
                {
                    if (isFirst || item.Item1 != lastIndex)
                    {
                        indicesToAdd.Add(item.Item1);
                        locationsToAdd.Add(item.Item2);
                    }
                    else
                        locationsToAdd[locationsToAdd.Count() - 1] = item.Item2;
                    isFirst = false;
                    lastIndex = item.Item1;
                }
            }
        }

        private static void SortIfNecessary(ref List<WUInt32> indicesToAdd, ref List<byte> locationsToAdd)
        {
            if (!indicesToAdd.IsSorted())
            {
                var zipped = indicesToAdd.Zip(locationsToAdd, (x, y) => new Tuple<uint, byte>(x, y));
                var ordered = zipped.OrderBy(x => x.Item1);
                indicesToAdd = ordered.Select(x => (WUInt32) x.Item1).ToList();
                locationsToAdd = ordered.Select(x => x.Item2).ToList();
            }
        }

        public UintSetLoc RemoveItemsAtUintIndices(UintSet previousUintSet, IEnumerable<WUInt32> indicesToRemove)
        {
            uint uintSetSize = previousUintSet.Count;
            int numCurrentlyStored = UncompressedLocations.Length;
            if (indicesToRemove.Any(x => !previousUintSet.Contains(x)))
                indicesToRemove = indicesToRemove.Where(x => previousUintSet.Contains(x)).ToList();
            if (!indicesToRemove.IsSorted())
                indicesToRemove = indicesToRemove.OrderBy(x => x).ToList();
            if (indicesToRemove.ContainsAdjacentRepeats())
                indicesToRemove = indicesToRemove.Distinct().ToList();
            var numIndicesToRemove = indicesToRemove.Count();
            int revisedNumberToStore = numCurrentlyStored - numIndicesToRemove;
            IEnumerator<WUInt32> indicesToRemoveEnumerator = indicesToRemove.GetEnumerator();
            IEnumerator<WUInt32> oldIndicesEnumerator = (previousUintSet.AsEnumerable()).GetEnumerator();
            IEnumerator<byte> oldLocationsEnumerator = GetLocationsEnumerator();
            bool moreIndicesToRemove = indicesToRemoveEnumerator.MoveNext();
            bool moreOldIndices = oldIndicesEnumerator.MoveNext();
            bool moreOldLocations = oldLocationsEnumerator.MoveNext();
            BinaryWriter bw = new BinaryWriter(revisedNumberToStore);
            while (moreOldIndices)
            {
                // Skip any indices to remove that are before the next old index (i.e., that aren't really in the set)
                while (moreIndicesToRemove && moreOldIndices && indicesToRemoveEnumerator.Current < oldIndicesEnumerator.Current)
                    moreIndicesToRemove = indicesToRemoveEnumerator.MoveNext();
                // Write the old location if this isn't one to be removed
                if (!moreIndicesToRemove || oldIndicesEnumerator.Current != indicesToRemoveEnumerator.Current)
                    bw.WriteByte(oldLocationsEnumerator.Current);
                moreOldIndices = oldIndicesEnumerator.MoveNext();
                moreOldLocations = oldLocationsEnumerator.MoveNext();
            }
            bw.ShortenIfNecessary();
            return new UintSetLoc(NoMoreThan16ChildrenPerNode, bw.Bytes);
        }
    }
}
