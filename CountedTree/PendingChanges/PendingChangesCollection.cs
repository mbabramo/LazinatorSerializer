using CountedTree.ByteUtilities;
using CountedTree.Core;
using CountedTree.UintSets;
using Lazinator.Core;
using Lazinator.Collections;
using Lazinator.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountedTree.PendingChanges
{
    public partial class PendingChangesCollection<TKey> : IPendingChangesCollection<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public PendingChange<TKey> this[int index] => PendingChanges[index];

        public int Count => Any() ? PendingChanges.Length : 0;

        public bool Any() => PendingChanges.Any();

        public int ExpectedNetPendingChanges => Count - 2 * PendingChanges.Where(x => x.Delete).Count();

        public uint NumValuesInTreeAfterPendingChanges(uint originalNumValuesInTree)
        {
            long numValuesInTree = (long)originalNumValuesInTree + (long)ExpectedNetPendingChanges;
            if (numValuesInTree < 0)
                numValuesInTree = 0;
            return (uint)numValuesInTree;
        }

        public PendingChangesCollection()
        {
            PendingChanges = new PendingChange<TKey>[0];
        }

        public PendingChangesCollection(IEnumerable<PendingChange<TKey>> pendingChanges, bool alreadyOrderedByDateOfChange)
        {
            if (!alreadyOrderedByDateOfChange)
                pendingChanges = pendingChanges.OrderByDateOfChange();
            PendingChanges = pendingChanges.ToArray().Simplify();
        }

        public PendingChangesCollection(IEnumerable<IEnumerable<PendingChange<TKey>>> pendingChangesSets)
        {
            PendingChanges = new PendingChange<TKey>[0];
            AddPendingChangesSets(pendingChangesSets, false);
        }

        public PendingChangesCollection(IEnumerable<PendingChangesCollection<TKey>> pendingChangesCollections)
        {
            PendingChanges = new PendingChange<TKey>[0];
            AddPendingChangesCollections(pendingChangesCollections, false);
        }

        public PendingChangesCollection(PendingChangesCollection<TKey> existingCollection, UintSet filter)
        {
            PendingChanges = existingCollection.AsEnumerable().Where(x => filter.Contains(x.Item.ID)).ToArray();
        }

        public override bool Equals(object obj)
        {
            PendingChangesCollection<TKey> other = obj as PendingChangesCollection<TKey>;
            if (other == null)
                return false;
            if (PendingChanges == null && other.PendingChanges == null)
                return true;
            if (PendingChanges == null || other.PendingChanges == null)
                return false;
            return (PendingChanges.SequenceEqual(other.PendingChanges));
        }

        public override int GetHashCode()
        {
            // if non-null array then go into unchecked block to avoid overflow
            if (PendingChanges != null)
            {
                unchecked
                {
                    int hash = 17;

                    // get hash code for all items in array
                    foreach (var item in PendingChanges)
                    {
                        hash = hash * 23 + ((item != null) ? item.GetHashCode() : 0);
                    }

                    return hash;
                }
            }

            // if null, hash code is zero
            return 0;
        }

        public void AddPendingChanges(IEnumerable<PendingChange<TKey>> pendingChanges, bool alreadyOrderedByDateofChange)
        {
            AddPendingChangesCollections(new List<PendingChangesCollection<TKey>> { new PendingChangesCollection<TKey>(pendingChanges, alreadyOrderedByDateofChange) }, alreadyOrderedByDateofChange);
        }

        public void AddPendingChangesSets(IEnumerable<IEnumerable<PendingChange<TKey>>> pendingChangesSets, bool alreadyOrderedByDateofChange)
        {
            var collections = pendingChangesSets.Select(x => new PendingChangesCollection<TKey>(x, false)).ToList();
            AddPendingChangesCollections(collections, false);
        }

        public void AddPendingChangesCollections(IEnumerable<PendingChangesCollection<TKey>> pendingChangesCollections, bool alreadyOrderedByDateofChange)
        {
            // We only add collections, since we know they've been simplified.
            IEnumerable<PendingChange<TKey>> temp = PendingChanges.OrderByDateOfChange().ToList();
            foreach (PendingChangesCollection<TKey> collection in pendingChangesCollections)
                temp = temp.Concat(alreadyOrderedByDateofChange ? collection.AsEnumerable().ToList() : collection.AsEnumerable().OrderByDateOfChange().ToList());
            PendingChanges = temp.Simplify();
        }

        public List<KeyAndID<TKey>> ApplyToCreateNewList(IEnumerable<KeyAndID<TKey>> original)
        {
            var copy = original.Select(x => x.Clone()).ToList();
            foreach (var p in PendingChanges)
            {
                if (p.Delete)
                    copy.RemoveAll(x => x.Equals(p.Item));
                else
                {
                    copy.RemoveAll(x => x.ID.Equals(p.Item.ID)); // make sure that we don't have any redundant or inconsistent items; should not be necessary, but it's an extra check
                    copy.Add(p.Item);
                }
            }
            return copy.OrderBy(x => x).ToList();
        }

        internal IEnumerable<PendingChange<TKey>> AsEnumerable()
        {
            return PendingChanges.AsEnumerable();
        }

        public override string ToString()
        {
            return String.Join(", ", AsEnumerable());
        }

        public IEnumerable<KeyAndID<TKey>> Integrate(LazinatorList<KeyAndID<TKey>> items, bool ascending, Func<KeyAndID<TKey>, bool> itemIsPotentialMatch)
        {
            int itemsCount, pendingChangesCount, additions, subtractions, expectedNumberItems;
            CountPendingEffects(items, out itemsCount, out pendingChangesCount, out additions, out subtractions, out expectedNumberItems);
            int itemsIndex = 0, pendingChangesIndex = 0;
            if (ascending == false)
            {
                itemsIndex = itemsCount - 1;
                pendingChangesIndex = pendingChangesCount - 1;
            }
            Func<bool> itemsIndexIsValid = () => itemsIndex >= 0 && itemsIndex < itemsCount;
            Func<bool> pendingChangesIndexIsValid = () => pendingChangesIndex >= 0 && pendingChangesIndex < pendingChangesCount;
            Action advanceItemsIndex = () => { itemsIndex = ascending ? itemsIndex + 1 : itemsIndex - 1; };
            Action advancePendingChangesIndex = () => { pendingChangesIndex = ascending ? pendingChangesIndex + 1 : pendingChangesIndex - 1; };

            while (itemsIndexIsValid() && pendingChangesIndexIsValid())
            {
                var item = items[itemsIndex];
                if (!itemIsPotentialMatch(item))
                {
                    advanceItemsIndex();
                    continue;
                }
                var pendingChange = PendingChanges[pendingChangesIndex];
                if (!itemIsPotentialMatch(pendingChange.Item))
                {
                    advancePendingChangesIndex();
                    continue;
                }
                int compareResult = item.CompareTo(pendingChange.Item);
                if ((ascending && compareResult < 0) || (!ascending && compareResult > 0))
                {
                    yield return items[itemsIndex];
                    advanceItemsIndex();
                }
                else if ((ascending && compareResult >= 0) || (!ascending && compareResult <= 0))
                {
                    if (pendingChange.Delete == false)
                    {
                        yield return pendingChange.Item; // otherwise don't yield anything
                    }
                    advancePendingChangesIndex();
                    if (compareResult == 0)
                        advanceItemsIndex(); // the items are the same, so we advance both of them. This will usually happen because we're deleting something that already exists. But it could also happen because we're trying to insert something that already exists.
                }
            }
            while (itemsIndexIsValid())
            {
                yield return items[itemsIndex];
                advanceItemsIndex();
            }
            while (pendingChangesIndexIsValid())
            {
                var pendingChange = PendingChanges[pendingChangesIndex];
                if (pendingChange.Delete == false)
                {
                    yield return pendingChange.Item;
                }
                advancePendingChangesIndex();
            }
        }

        private void CountPendingEffects(LazinatorList<KeyAndID<TKey>> items, out int itemsCount, out int pendingChangesCount, out int additions, out int subtractions, out int expectedNumberItems)
        {
            itemsCount = items.Count;
            pendingChangesCount = PendingChanges.Length;
            additions = PendingChanges.Where(x => !x.Delete).Count();
            subtractions = pendingChangesCount - additions;
            expectedNumberItems = itemsCount + additions - subtractions;
        }
    }
}
