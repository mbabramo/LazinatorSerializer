using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlSortedIndexableTree<T> : AvlIndexableTree<T>, IAvlSortedIndexableTree<T>, ISortedIndexableContainer<T> where T : ILazinator, IComparable<T>
    {
        public override IOrderableContainer<T> CreateNewWithSameSettings()
        {
            return new AvlSortedIndexableTree<T>() { AllowDuplicates = AllowDuplicates };
        }

        private MultivalueLocationOptions MultivalueLocationOptionForInsertion => AllowDuplicates ? MultivalueLocationOptions.AfterLast : MultivalueLocationOptions.Any;
        private MultivalueLocationOptions MultivalueLocationOptionForRemoval => MultivalueLocationOptions.Any; // because all items are same, it doesn't matter.

        public (long index, bool exists) FindSorted(T target) => FindSorted(target, Comparer<T>.Default);
        public (long index, bool exists) FindSorted(T target, MultivalueLocationOptions whichOne) => FindSorted(target, whichOne, Comparer<T>.Default);
        public (long index, bool insertedNotReplaced) InsertSorted(T item) => InsertSorted(item, MultivalueLocationOptionForInsertion);
        public (long index, bool insertedNotReplaced) InsertSorted(T item, MultivalueLocationOptions whichOne) => InsertSorted(item, whichOne, Comparer<T>.Default);
        public (long priorIndex, bool existed) RemoveSorted(T item) => RemoveSorted(item, MultivalueLocationOptionForRemoval);
        public (long priorIndex, bool existed) RemoveSorted(T item, MultivalueLocationOptions whichOne) => RemoveSorted(item, whichOne, Comparer<T>.Default);


        public bool TryInsertSorted(T item) => TryInsertSorted(item, MultivalueLocationOptionForInsertion);
        public bool TryInsertSorted(T item, MultivalueLocationOptions whichOne) => TryInsertSorted(item, whichOne, Comparer<T>.Default);
        public bool TryRemoveSorted(T item) => TryRemoveSorted(item, MultivalueLocationOptionForRemoval);
        public bool TryRemoveSorted(T item, MultivalueLocationOptions whichOne) => TryRemoveSorted(item, Comparer<T>.Default);



        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Contains(T item) => Contains(item, Comparer<T>.Default);


    }
}
