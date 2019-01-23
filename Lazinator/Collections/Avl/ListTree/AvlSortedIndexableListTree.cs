﻿using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ListTree
{
    public partial class AvlSortedIndexableListTree<T> : AvlIndexableListTree<T>, IAvlSortedIndexableListTree<T>, ISortedIndexableMultivalueContainer<T> where T : ILazinator, IComparable<T>
    {
        public AvlSortedIndexableListTree(bool allowDuplicates, bool unbalanced, ContainerFactory innerContainerFactory) : base(allowDuplicates, unbalanced, innerContainerFactory)
        {
        }

        protected override void CreateUnderlyingTree(ContainerFactory innerContainerFactory)
        {
            UnderlyingTree = (AvlAggregatedTree<IIndexableMultivalueContainer<T>>)innerContainerFactory.CreateValueContainer<ISortedMultivalueContainer<T>>();
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlSortedListTree<T>(AllowDuplicates, Unbalanced, InnerContainerFactory);
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

        public bool TryRemoveAll(T item)
        {
            bool any = TryRemove(item);
            if (any)
            {
                do
                {
                } while (TryRemove(item));
            }
            return any;
        }

        public bool Contains(T item) => Contains(item, Comparer<T>.Default);

        public long Count(T item) => Count(item, Comparer<T>.Default);

    }
}
