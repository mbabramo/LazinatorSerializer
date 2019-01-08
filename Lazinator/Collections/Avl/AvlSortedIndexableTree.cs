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

        private MultivalueLocationOptions MultivalueLocationOptionForInsertion => AllowDuplicates ? MultivalueLocationOptions.AfterLast : MultivalueLocationOptions.Any;
        private MultivalueLocationOptions MultivalueLocationOptionForRemoval => MultivalueLocationOptions.Any; // because all items are same, it doesn't matter.

        public (long index, bool exists) Find(T target) => Find(target, MultivalueLocationOptions.First);
        public (long index, bool exists) Find(T target, MultivalueLocationOptions whichOne) => Find(target, whichOne, Comparer<T>.Default);
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item) => InsertGetIndex(item, MultivalueLocationOptionForInsertion);
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, MultivalueLocationOptions whichOne) => InsertGetIndex(item, whichOne, Comparer<T>.Default);
        public bool RemoveSorted(T item) => TryRemove(item, MultivalueLocationOptionForRemoval);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne) => TryRemove(item, whichOne, Comparer<T>.Default);


        public bool TryInsertSorted(T item) => TryInsertSorted(item, MultivalueLocationOptionForInsertion);
        public bool TryInsertSorted(T item, MultivalueLocationOptions whichOne) => TryInsert(item, whichOne, Comparer<T>.Default);
        public bool TryRemoveSorted(T item) => TryRemoveSorted(item, MultivalueLocationOptionForRemoval);
        public bool TryRemoveSorted(T item, MultivalueLocationOptions whichOne) => TryRemove(item, Comparer<T>.Default);

        public bool Contains(T item) => Contains(item, Comparer<T>.Default);


    }
}
