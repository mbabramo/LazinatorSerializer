using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    public partial class SortedLazinatorList<T> : LazinatorList<T>, ISortedLazinatorList<T> where T : ILazinator
    {
        public (int location, bool rejectedAsDuplicate) Insert(T item, IComparer<T> comparer = null)
        {
            (int location, bool exists) = Find(item, comparer);
            if (exists && !AllowDuplicates)
                return (location, true);
            Insert(location, item);
            return (location, false);
        }

        public (int priorLocation, bool existed) RemoveSorted(T item, IComparer<T> comparer = null)
        {
            (int location, bool exists) = Find(item, comparer);
            if (exists)
                RemoveAt(location);
            return (location, exists);
        }

        public (int location, bool exists) Find(T target, IComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;
            bool found = false;
            if (Count == 0)
                return (0, false);
            int first = 0, last = Count - 1;
            int mid = 0;

            //for a sorted array with descending values
            while (!found && first <= last)
            {
                mid = (first + last) / 2;

                int comparison = comparer.Compare(target, this[mid]);
                if (comparison == 0)
                    return (mid, true);
                if (first == last)
                {
                    if (comparison > 0)
                        return (mid + 1, false);
                    return (mid, false);
                }
                if (comparison > 0)
                {
                    first = mid + 1;
                }
                else
                {
                    last = mid - 1;
                }
            }
            return (mid, false);
        }
    }
}
