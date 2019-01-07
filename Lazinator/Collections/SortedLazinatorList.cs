using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    public partial class SortedLazinatorList<T> : LazinatorList<T>, ISortedLazinatorList<T>, ILazinatorSortable<T> where T : ILazinator, IComparable<T>
    {
        public (long index, bool insertedNotReplaced) InsertSorted(T item) => InsertSorted(item, Comparer<T>.Default);
        public (long priorIndex, bool existed) RemoveSorted(T item) => RemoveSorted(item, Comparer<T>.Default);
        public (long index, bool exists) FindSorted(T target) => FindSorted(target, Comparer<T>.Default);

        public (long index, bool insertedNotReplaced) InsertSorted(T item, IComparer<T> comparer)
        {
            (long location, bool exists) = FindSorted(item, comparer);
            if (exists && !AllowDuplicates)
                return (location, true);
            Insert((int) location, item);
            return (location, false);
        }

        public (long priorIndex, bool existed) RemoveSorted(T item, IComparer<T> comparer)
        {
            (long location, bool exists) = FindSorted(item);
            if (exists)
                RemoveAt(location);
            return (location, exists);
        }

        public (long index, bool exists) FindSorted(T target, IComparer<T> comparer)
        {
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
                {
                    if (AllowDuplicates)
                    { // return first match
                        bool matches = true;
                        while (matches && mid > 0)
                        {
                            var previous = this[mid - 1];
                            matches = previous.Equals(target);
                            if (matches)
                            {
                                mid--;
                            }
                        }
                    }
                    return (mid, true);
                }
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

        protected override ILazinatorListable<T> CreateEmptyList()
        {
            return new SortedLazinatorList<T>();
        }

    }
}
