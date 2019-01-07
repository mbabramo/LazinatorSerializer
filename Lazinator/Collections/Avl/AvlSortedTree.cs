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

        public bool TryInsertSorted(T item) => TryInsertSorted(item, MultivalueLocationOptions.Last);

        public bool TryInsertSorted(T item, MultivalueLocationOptions whichOne)
        {
            if (AllowDuplicates)
            {
                // If the location option is BeforeFirst/AfterLast, then we'll insert a new item even if the item is already present. Otherwise, we'll replace the item. By default, we'll insert after the last item.
                if (whichOne == MultivalueLocationOptions.First)
                    whichOne = MultivalueLocationOptions.BeforeFirst;
                else
                    whichOne = MultivalueLocationOptions.AfterLast;
            }
            return TryInsertSorted(item, whichOne, Comparer<T>.Default);
        }

        public bool TryRemoveSorted(T item) => TryRemoveSorted(item, Comparer<T>.Default);
        public bool TryRemoveSorted(T item, MultivalueLocationOptions whichOne) => TryRemoveSorted(item, Comparer<T>.Default);
    }
}
