using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    public partial class SortedLazinatorList<T> : LazinatorList<T>, ISortedLazinatorList<T>, ILazinatorSortable<T> where T : ILazinator, IComparable<T>
    {
        public (long location, bool rejectedAsDuplicate) InsertSorted(T item, IComparer<T> comparer = null)
        {
            (long location, bool exists) = FindSorted(item, comparer);
            if (exists && !AllowDuplicates)
                return (location, true);
            Insert((int) location, item);
            return (location, false);
        }

        public (long priorLocation, bool existed) RemoveSorted(T item, IComparer<T> comparer = null)
        {
            (long location, bool exists) = FindSorted(item, comparer);
            if (exists)
                RemoveAt(location);
            return (location, exists);
        }

        public (long location, bool exists) FindSorted(T target, IComparer<T> comparer = null)
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

        public (long location, bool rejectedAsDuplicate) InsertSorted(T item)
        {
            return InsertSorted(item, null);
        }

        public (long priorLocation, bool existed) RemoveSorted(T item)
        {
            return RemoveSorted(item, null);
        }

        public (long location, bool exists) FindSorted(T target)
        {
            return FindSorted(target, null);
        }
    }
}
