using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ValueTree
{
    public partial class AvlSortedTree<T> : AvlTree<T>, IAvlSortedTree<T>, ISortedMultivalueContainer<T> where T : ILazinator , IComparable<T>
    {
        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlSortedTree<T>() { AllowDuplicates = AllowDuplicates };
        }

        public bool Contains(T item) => Contains(item, Comparer<T>.Default);

        public bool TryInsert(T item) => TryInsert(item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any);
        public bool TryInsert(T item, MultivalueLocationOptions whichOne) => TryInsert(item, whichOne, Comparer<T>.Default);

        public bool TryRemove(T item) => TryRemove(item, MultivalueLocationOptions.Any);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne) => TryRemove(item, Comparer<T>.Default);

        public bool TryRemoveAll(T item) => TryRemoveAll(item, Comparer<T>.Default);

        public long Count(T item) => Count(item, Comparer<T>.Default);
    }
}
