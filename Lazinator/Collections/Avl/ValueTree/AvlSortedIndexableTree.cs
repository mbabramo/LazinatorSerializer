using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ValueTree
{
    public partial class AvlSortedIndexableTree<T> : AvlIndexableTree<T>, IAvlSortedIndexableTree<T>, ISortedIndexableContainer<T>, ISortedIndexableMultivalueContainer<T> where T : ILazinator, IComparable<T>
    {
        public AvlSortedIndexableTree(bool allowDuplicates, bool unbalanced) : base(allowDuplicates, unbalanced)
        {
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlSortedIndexableTree<T>() { AllowDuplicates = AllowDuplicates };
        }

        private MultivalueLocationOptions MultivalueLocationOptionForInsertion => AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any;
        private MultivalueLocationOptions MultivalueLocationOptionForRemoval => MultivalueLocationOptions.Any; // because all items are same, it doesn't matter.

        public (long index, bool exists) FindIndex(T target) => FindIndex(target, MultivalueLocationOptions.First);
        public (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne) => FindIndex(target, whichOne, Comparer<T>.Default);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item) => InsertOrReplace(item, MultivalueLocationOptionForInsertion);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne) => InsertOrReplace(item, whichOne, Comparer<T>.Default);
        public bool RemoveSorted(T item) => TryRemove(item, MultivalueLocationOptionForRemoval);

        
        public bool TryRemove(T item, MultivalueLocationOptions whichOne) => TryRemove(item, whichOne, Comparer<T>.Default);
        public bool TryRemove(T item) => TryRemove(item, MultivalueLocationOptionForRemoval);

        public bool Contains(T item) => Contains(item, Comparer<T>.Default);

        public long Count(T item) => Count(item, Comparer<T>.Default);
    }
}
