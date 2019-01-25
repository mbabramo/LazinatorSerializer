using LazinatorCollections.Interfaces;
using Lazinator.Core;
using Lazinator.Support;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Avl.ListTree
{
    public static class ListTreeComparisons<T> where T : ILazinator
    {
        public static int CompareBasedOnEndItems(IMultivalueContainer<T> container, T item, IComparer<T> comparer)
        {
            T last = container.Last();
            var lastComparison = comparer.Compare(item, last);
            if (lastComparison >= 0)
                return lastComparison; // item is last or after
            T first = container.First();
            var firstComparison = comparer.Compare(item, first);
            if (firstComparison <= 0)
                return firstComparison; // item is first or before
            return 0; // item is between first and last
        }

        /// <summary>
        /// Returns a comparer to compare an item to an inner collection. Usually, the custom comparer can only compare like objects, so null should be passed as the inner collection being compared; this custom comparer then substitutes the item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static CustomComparer<IMultivalueContainer<T>> GetItemToInnerContainerComparer(T item, IComparer<T> comparer)
        {
            return new CustomComparer<IMultivalueContainer<T>>((a, b) =>
            {
                if (a == null)
                    return CompareBasedOnEndItems(b, item, comparer);
                return 0 - CompareBasedOnEndItems(a, item, comparer);
            });
        }

        public static CustomComparer<IMultivalueContainer<T>> GetInnerContainersComparer(IComparer<T> comparer)
        {
            return new CustomComparer<IMultivalueContainer<T>>((a, b) =>
            {
                // compare based on firsts
                var aFirst = a.First();
                var bFirst = b.First();
                int firstComparison = comparer.Compare(aFirst, bFirst);
                if (firstComparison != 0)
                    return firstComparison;
                // compare based on lasts
                var aLast = a.Last();
                var bLast = b.Last();
                int lastComparison = comparer.Compare(aLast, bLast);
                return lastComparison;
            });
        }
    }
}
