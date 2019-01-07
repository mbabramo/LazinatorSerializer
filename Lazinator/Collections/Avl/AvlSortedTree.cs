using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public class AvlSortedTree<T> : AvlTree<T>, IAvlSortedTree<T>, ISortedContainer<T> where T : ILazinator , IComparable<T>
    {
        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Contains(T item) => Contains(item, Comparer<T>.Default);

        public bool TryInsertSorted(T item) => TryInsertSorted(item, AllowDuplicates ? MultivalueLocationOptions.AfterLast : MultivalueLocationOptions.Any);
        public bool TryInsertSorted(T item, MultivalueLocationOptions whichOne) => TryInsertSorted(item, whichOne, Comparer<T>.Default);

        public bool TryRemoveSorted(T item) => TryRemoveSorted(item, Comparer<T>.Default);
        public bool TryRemoveSorted(T item, MultivalueLocationOptions whichOne) => TryRemoveSorted(item, Comparer<T>.Default);
    }
}
