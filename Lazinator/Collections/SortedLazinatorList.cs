using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    public partial class SortedLazinatorList<T> : LazinatorList<T>, ISortedLazinatorList<T>, ILazinatorSorted<T> where T : ILazinator, IComparable<T>
    {
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item) => InsertGetIndex(item, Comparer<T>.Default);
        public bool TryRemove(T item) => TryRemove(item, Comparer<T>.Default);
        public (long index, bool exists) Find(T target) => Find(target, Comparer<T>.Default);

        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, IComparer<T> comparer)
        {
            (long location, bool exists) = Find(item, comparer);
            if (exists && !AllowDuplicates)
                return (location, true);
            Insert((int) location, item);
            return (location, false);
        }

        public bool TryRemove(T item, IComparer<T> comparer)
        {
            (long location, bool exists) = Find(item);
            if (exists)
                RemoveAt(location);
            return exists;
        }

        public (long index, bool exists) Find(T target, IComparer<T> comparer)
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
