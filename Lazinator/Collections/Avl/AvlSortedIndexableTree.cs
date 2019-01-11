using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlSortedIndexableTree<T> : AvlIndexableTree<T>, IAvlSortedIndexableTree<T>, ISortedIndexableContainer<T>, ISortedIndexableMultivalueContainer<T> where T : ILazinator, IComparable<T>
    {
        public AvlSortedIndexableTree(bool allowDuplicates)
        {
            AllowDuplicates = allowDuplicates;
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlSortedIndexableTree<T>() { AllowDuplicates = AllowDuplicates };
        }

        private MultivalueLocationOptions MultivalueLocationOptionForInsertion => AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any;
        private MultivalueLocationOptions MultivalueLocationOptionForRemoval => MultivalueLocationOptions.Any; // because all items are same, it doesn't matter.

        public (long index, bool exists) Find(T target) => Find(target, MultivalueLocationOptions.First);
        public (long index, bool exists) Find(T target, MultivalueLocationOptions whichOne) => Find(target, whichOne, Comparer<T>.Default);
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item) => InsertGetIndex(item, MultivalueLocationOptionForInsertion);
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, MultivalueLocationOptions whichOne) => InsertGetIndex(item, whichOne, Comparer<T>.Default);
        public bool RemoveSorted(T item) => TryRemove(item, MultivalueLocationOptionForRemoval);


        public bool TryInsert(T item) => TryInsert(item, MultivalueLocationOptionForInsertion);
        public bool TryInsert(T item, MultivalueLocationOptions whichOne) => TryInsert(item, whichOne, Comparer<T>.Default);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne) => TryRemove(item, whichOne, Comparer<T>.Default);
        public bool TryRemove(T item) => TryRemove(item, MultivalueLocationOptionForRemoval);

        public bool Contains(T item) => Contains(item, Comparer<T>.Default);

        public int Count(T item)
        {
            var node = GetMatchingNode(item, MultivalueLocationOptions.First, Comparer<T>.Default);
            if (node == null)
                return 0;
            int count = 0;
            while (node != null)
            {
                count++;
                node = node.GetNextNode();
                if (!node.Value.Equals(item))
                    node = null;
            }
            return count;
        }

        public bool TryRemoveAll(T item)
        {
            bool found = false;
            bool foundAny = false;
            do
            {
                found = TryRemove(item);
                if (found)
                    foundAny = true;
            } while (found);
            return foundAny;
        }
    }
}
