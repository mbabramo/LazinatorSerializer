using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using Lazinator.Wrappers;
using LazinatorAvlCollections.Avl.ValueTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceProfiling
{
    /// <summary>
    /// An Avl tree that is sorted based on the default comparer and indexable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class AvlSortedIndexableTree_WDouble : AvlIndexableTreeWithNodeType<WDouble, AvlCountedNode<WDouble>>, IAvlSortedIndexableTree_WDouble, ISortedIndexableContainer<WDouble>, ISortedIndexableMultivalueContainer<WDouble> 
    {
        public AvlSortedIndexableTree_WDouble(bool allowDuplicates, bool unbalanced, bool cacheEnds) : base(allowDuplicates, unbalanced, cacheEnds)
        {
        }

        public override IValueContainer<WDouble> CreateNewWithSameSettings()
        {
            return new AvlSortedIndexableTree_WDouble(AllowDuplicates, Unbalanced, CacheEnds);
        }

        private MultivalueLocationOptions MultivalueLocationOptionForInsertion => AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any;
        private MultivalueLocationOptions MultivalueLocationOptionForRemoval => MultivalueLocationOptions.Any; // because all items are same, it doesn't matter.

        public (long index, bool exists) FindIndex(WDouble target) => FindIndex(target, MultivalueLocationOptions.First);
        public (long index, bool exists) FindIndex(WDouble target, MultivalueLocationOptions whichOne) => FindIndex(target, whichOne, Comparer<WDouble>.Default);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(WDouble item) => InsertOrReplace(item, MultivalueLocationOptionForInsertion);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(WDouble item, MultivalueLocationOptions whichOne) => InsertOrReplace(item, whichOne, Comparer<WDouble>.Default);
        public bool RemoveSorted(WDouble item) => TryRemove(item, MultivalueLocationOptionForRemoval);


        public bool TryRemove(WDouble item, MultivalueLocationOptions whichOne) => TryRemove(item, whichOne, Comparer<WDouble>.Default);
        public bool TryRemove(WDouble item) => TryRemove(item, MultivalueLocationOptionForRemoval);

        public bool Contains(WDouble item) => Contains(item, Comparer<WDouble>.Default);

        public long Count(WDouble item) => Count(item, Comparer<WDouble>.Default);

        public IEnumerable<WDouble> AsEnumerable(bool reverse, WDouble startValue) => AsEnumerable(reverse, startValue, Comparer<WDouble>.Default);
        public IEnumerator<WDouble> GetEnumerator(bool reverse, WDouble startValue) => GetEnumerator(reverse, startValue, Comparer<WDouble>.Default);
    }
}
