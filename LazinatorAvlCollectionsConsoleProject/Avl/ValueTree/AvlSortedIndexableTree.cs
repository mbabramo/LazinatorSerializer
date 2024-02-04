using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    /// <summary>
    /// An Avl tree that is sorted based on the default comparer and indexable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class AvlSortedIndexableTree<T> : AvlIndexableTreeWithNodeType<T, AvlCountedNode<T>>, IAvlSortedIndexableTree<T>, ISortedIndexableContainer<T>, ISortedIndexableMultivalueContainer<T> where T : ILazinator, IComparable<T>
    {
        public AvlSortedIndexableTree(bool allowDuplicates, bool unbalanced, bool cacheEnds) : base(allowDuplicates, unbalanced, cacheEnds)
        {
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlSortedIndexableTree<T>(AllowDuplicates, Unbalanced, CacheEnds);
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

        public IEnumerable<T> AsEnumerable(bool reverse, T startValue) => AsEnumerable(reverse, startValue, Comparer<T>.Default);
        public IEnumerator<T> GetEnumerator(bool reverse, T startValue) => GetEnumerator(reverse, startValue, Comparer<T>.Default);
    }
}
