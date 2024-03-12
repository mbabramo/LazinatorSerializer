using CountedTree.Core;
using CountedTree.PendingChanges;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace CountedTree.UintSets
{
    public partial class UintSetWithLoc : IUintSetWithLoc
    {

        public UintSetWithLoc()
        {

        }

        public UintSetWithLoc(UintSet set, UintSetLoc loc)
        {
            Set = set;
            Loc = loc;
        }

        public static UintSetWithLoc ConstructFromItems<TKey>(List<KeyAndID<TKey>> itemsToIncludeInUintSet, IEnumerable<KeyAndID<TKey>?> splitValues, bool skipLocations, bool noMoreThan16ChildrenPerNode) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
        {
            var splitValuesEnumerator = splitValues.GetEnumerator();
            bool moreSpots = splitValuesEnumerator.MoveNext();
            if (!itemsToIncludeInUintSet.IsSorted())
                itemsToIncludeInUintSet = itemsToIncludeInUintSet.OrderBy(x => x).ToList();
            var itemsInBitSet = new HashSet<WUInt32>(itemsToIncludeInUintSet.Select(x => (WUInt32)x.ID));
            if (skipLocations)
                return new UintSetWithLoc(new UintSet(itemsInBitSet), null);
            var locationsOfItems = new UintSetLoc(noMoreThan16ChildrenPerNode);
            List<WUInt32> indices = new List<WUInt32>();
            List<byte> locations = new List<byte>();
            byte childIndex = 0;
            foreach (var item in itemsToIncludeInUintSet)
            {
                while (moreSpots && item > splitValuesEnumerator.Current && splitValuesEnumerator.Current != null)
                {
                    moreSpots = splitValuesEnumerator.MoveNext();
                    childIndex++;
                }
                indices.Add(item.ID);
                locations.Add(childIndex);
            }
            locationsOfItems = locationsOfItems.AddItemsAtUintIndices(new UintSet(), indices, locations);
            var uintSetWithLoc = new UintSetWithLoc(new UintSet(itemsInBitSet), locationsOfItems);
            return uintSetWithLoc;
        }

        public IEnumerable<Tuple<uint, byte>> GetIndicesAndLocations()
        {
            // TODO: In C# 7, replace this with ValueTuples by using C# patterns.
            IEnumerator<WUInt32> indicesEnumerator = Set.AsEnumerable().GetEnumerator();
            IEnumerator<byte> locationsEnumerator = Loc.GetLocationsEnumerator();
            bool anotherIndex = indicesEnumerator.MoveNext();
            bool anotherLocation = locationsEnumerator.MoveNext();
            while (anotherIndex && anotherLocation)
            {
                yield return new Tuple<uint, byte>(indicesEnumerator.Current, locationsEnumerator.Current);
                anotherIndex = indicesEnumerator.MoveNext();
                anotherLocation = locationsEnumerator.MoveNext();
            }
            if (anotherIndex != anotherLocation)
                throw new Exception("Internal error. Number of indices does not equal number of locations.");
        }

        public IEnumerable<Tuple<uint, byte>> GetAmendedIndicesAndLocations(IEnumerable<PendingChangeEffect> changes)
        {
            var existing = GetIndicesAndLocations().GetEnumerator();
            bool moreExisting = existing.MoveNext();
            foreach (PendingChangeEffect change in changes)
            {
                if (moreExisting)
                {
                    if (change.ID < existing.Current.Item1)
                    {
                        if (!change.Delete)
                            yield return new Tuple<uint, byte>(change.ID, change.ChildIndex);
                    }
                    else if (change.ID == existing.Current.Item1)
                    {
                        if (change.Delete)
                        {
                            if (change.ChildIndex == existing.Current.Item2)
                                moreExisting = existing.MoveNext();
                            // else ignore this -- can't delete something that's not there
                        }
                        else
                            yield return new Tuple<uint, byte>(change.ID, change.ChildIndex);
                    }
                    else
                    {
                        while (moreExisting && existing.Current.Item1 < change.ID)
                        {
                            yield return existing.Current;
                            moreExisting = existing.MoveNext();
                        }
                    }
                }
            }
            while (moreExisting)
            {
                yield return existing.Current;
                moreExisting = existing.MoveNext();
            }
        }

        public uint[] CountForEachChild(byte numChildren, UintSet furtherFilter = null)
        {
            uint[] numAtChildren = new uint[numChildren];
            uint uintSetSize = Set.Count;
            foreach (Tuple<uint, byte> indexAndLocation in GetIndicesAndLocations())
            {
                if (furtherFilter == null || furtherFilter.Contains(indexAndLocation.Item1))
                    numAtChildren[indexAndLocation.Item2]++;
            }
            return numAtChildren;
        }
    }
}
