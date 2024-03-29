﻿using LazinatorAvlCollections.Avl.ValueTree;
using LazinatorAvlCollections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl.ListTree
{
    public partial class AvlSortedListTree<T> : AvlListTree<T>, IAvlSortedListTree<T>, ISortedMultivalueContainer<T> where T : ILazinator, IComparable<T>
    { 
        public AvlSortedListTree(bool allowDuplicates, bool unbalanced, ContainerFactory innerContainerFactory) : base(allowDuplicates, unbalanced, innerContainerFactory)
        {
        }

        protected override void CreateUnderlyingTree(ContainerFactory innerContainerFactory)
        {
            UnderlyingTree = new AvlTree<IMultivalueContainer<T>>(AllowDuplicates, Unbalanced, false);
            UnderlyingTree.MultilevelReporterParent = this;
        }

        protected override IMultivalueContainer<T> CreateInnerContainer()
        {
            var result = (IMultivalueContainer<T>)InnerContainerFactory.CreateSortedValueContainer<T>();
            ((IMultilevelReporter)result).MultilevelReporterParent = UnderlyingTree;
            return result;
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlSortedListTree<T>(AllowDuplicates, Unbalanced, InnerContainerFactory);
        }

        public bool Contains(T item) => Contains(item, Comparer<T>.Default);

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item) => InsertOrReplace(item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne) => InsertOrReplace(item, whichOne, Comparer<T>.Default);

        public bool TryRemove(T item) => TryRemove(item, MultivalueLocationOptions.Any);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne) => TryRemove(item, Comparer<T>.Default);

        public bool TryRemoveAll(T item) => TryRemoveAll(item, Comparer<T>.Default);

        public long Count(T item) => Count(item, Comparer<T>.Default);

        public IEnumerable<T> AsEnumerable(bool reverse, T startValue) => AsEnumerable(reverse, startValue, Comparer<T>.Default);
        public IEnumerator<T> GetEnumerator(bool reverse, T startValue) => GetEnumerator(reverse, startValue, Comparer<T>.Default);
    }
}
