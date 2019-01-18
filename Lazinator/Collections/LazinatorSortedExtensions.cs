using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    public static class LazinatorSortedExtensions 
    {
        public static (long index, bool insertedNotReplaced) SortedInsertGetIndex<L, T>(this L list, T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) where L : ILazinatorSortable<T> where T : ILazinator, IComparable<T>
        {
            (long index, bool exists) = list.SortedFind(item, whichOne, comparer);
            if (list.AllowDuplicates && (whichOne == MultivalueLocationOptions.InsertBeforeFirst || whichOne == MultivalueLocationOptions.InsertAfterLast))
            {
                list.Insert((int)index, item);
                return (index, true);
            }
            if (!list.AllowDuplicates && whichOne != MultivalueLocationOptions.Any)
                throw new Exception("Allowing potential duplicates is forbidden. Use MultivalueLocationOptions.Any");

            if (index < list.LongCount)
            {
                var existing = list.GetAt(index);
                if (comparer.Compare(existing, item) == 0)
                {
                    list[(int)index] = item;
                    return (index, false);
                }
            }
            list.Insert((int)index, item);
            return (index, true);
        }

        public static bool SortedTryRemove<L, T>(this L list, T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) where L : ILazinatorSortable<T> where T : ILazinator, IComparable<T>
        {
            (long location, bool exists) = list.SortedFind(item, whichOne, comparer);
            if (exists)
                list.RemoveAt(location);
            return exists;
        }

        public static (long index, bool exists) SortedFind<L, T>(this L list, T target, MultivalueLocationOptions whichOne, IComparer<T> comparer) where L : ILazinatorSortable<T> where T : ILazinator, IComparable<T>
        {
            (long index, bool exists) result = list.SortedFind(target, comparer);
            if (!result.exists || whichOne == MultivalueLocationOptions.Any)
                return result;
            long firstIndex = result.index, lastIndex = result.index;
            while (firstIndex > 0 && EqualityComparer<T>.Default.Equals(list[(int)(firstIndex - 1)], target))
                firstIndex--;
            while (lastIndex < list.Count - 1 && EqualityComparer<T>.Default.Equals(list[(int)(lastIndex + 1)], target))
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

        public static (long index, bool exists) SortedFind<L, T>(this L list, T target, IComparer<T> comparer) where L : ILazinatorSortable<T> where T : ILazinator, IComparable<T>
        {
            bool found = false;
            if (list.Count == 0)
                return (0, false);
            int first = 0, last = list.Count - 1;
            int mid = 0;

            //for a sorted array with descending values
            while (!found && first <= last)
            {
                mid = (first + last) / 2;

                int comparison = comparer.Compare(target, list[mid]);
                if (comparison == 0)
                {
                    if (list.AllowDuplicates)
                    { // return first match
                        bool matches = true;
                        while (matches && mid > 0)
                        {
                            var previous = list[mid - 1];
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
