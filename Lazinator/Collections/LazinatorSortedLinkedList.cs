using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    public partial class LazinatorSortedLinkedList<T> : LazinatorLinkedList<T>, ILazinatorSortedLinkedList<T>, ILazinatorSorted<T> where T : IComparable<T>, ILazinator
    {
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item) => InsertGetIndex(item, Comparer<T>.Default);
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, MultivalueLocationOptions whichOne) => InsertGetIndex(item, whichOne, Comparer<T>.Default);
        public bool TryRemove(T item) => TryRemove(item, Comparer<T>.Default);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne) => TryRemove(item, whichOne, Comparer<T>.Default);
        public (long index, bool exists) Find(T target) => Find(target, Comparer<T>.Default);
        public (long index, bool exists) Find(T target, MultivalueLocationOptions whichOne) => Find(target, whichOne, Comparer<T>.Default);

        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, IComparer<T> comparer) => InsertGetIndex(item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any, comparer);
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            (long index, bool exists) = Find(item, whichOne, comparer);
            if (AllowDuplicates)
            {
                if (whichOne == MultivalueLocationOptions.InsertBeforeFirst || whichOne == MultivalueLocationOptions.InsertAfterLast)
                {
                    Insert((int)index, item);
                    return (index, true);
                }
                else
                {
                    this[(int)index] = item;
                    return (index, false);
                }
            }
            else
            {
                Insert((int)index, item);
                return (index, true);
            }
        }
        public bool TryRemove(T item, IComparer<T> comparer) => TryRemove(item, MultivalueLocationOptions.Any, comparer);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            (long location, bool exists) = Find(item, whichOne, comparer);
            if (exists)
                RemoveAt(location);
            return exists;
        }

        public (long index, bool exists) Find(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            (long index, bool exists) result = Find(target, comparer);
            if (!result.exists || whichOne == MultivalueLocationOptions.Any)
                return result;
            long firstIndex = result.index, lastIndex = result.index;
            while (firstIndex > 0 && EqualityComparer<T>.Default.Equals(this[(int) (firstIndex - 1)], target))
                firstIndex--;
            while (lastIndex < Count - 1 && EqualityComparer<T>.Default.Equals(this[(int)(lastIndex + 1)], target))
                lastIndex++;
            switch (whichOne)
            {
                case MultivalueLocationOptions.InsertBeforeFirst:
                case MultivalueLocationOptions.First:
                    return (firstIndex, true);
                case MultivalueLocationOptions.Last:
                    return (lastIndex, true);
                case MultivalueLocationOptions.InsertAfterLast:
                    return (lastIndex + 1, true);
                default:
                    throw new NotSupportedException();
            }

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

    }
}
