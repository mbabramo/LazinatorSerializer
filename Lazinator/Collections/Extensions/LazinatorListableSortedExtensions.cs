using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    public static class LazinatorListableSortedExtensions 
    {
        public static (long index, bool insertedNotReplaced) SortedInsertOrReplace<L, T>(this L list, bool allowDuplicates, T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) where L : ILazinatorListable<T> where T : ILazinator
        {
            (long index, bool exists) = list.SortedFind(allowDuplicates, item, whichOne, comparer);
            if (allowDuplicates && (whichOne == MultivalueLocationOptions.InsertBeforeFirst || whichOne == MultivalueLocationOptions.InsertAfterLast))
            {
                list.Insert((int)index, item);
                return (index, true);
            }
            if (!allowDuplicates && whichOne != MultivalueLocationOptions.Any)
                throw new Exception("Allowing potential duplicates is forbidden. Use MultivalueLocationOptions.Any");

            if (index < list.LongCount)
            {
                var existing = list.GetAt(index);
                if (comparer.Compare(existing, item) == 0)
                {
                    list.SetAt(index, item);
                    return (index, false);
                }
            }
            list.Insert((int)index, item);
            return (index, true);
        }

        public static bool SortedTryRemove<L, T>(this L list, bool allowDuplicates, T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) where L : ILazinatorListable<T> where T : ILazinator
        {
            (long location, bool exists) = list.SortedFind(allowDuplicates, item, whichOne, comparer);
            if (exists)
                list.RemoveAt(location);
            return exists;
        }

        public static (long index, bool exists) SortedFind<L, T>(this L list, bool allowDuplicates, T target, MultivalueLocationOptions whichOne, IComparer<T> comparer) where L : ILazinatorListable<T> where T : ILazinator
        {
            (long index, bool exists) result = list.SortedFind(allowDuplicates, target, comparer);
            if (!result.exists || whichOne == MultivalueLocationOptions.Any)
                return result;
            long firstIndex = result.index, lastIndex = result.index;
            while (firstIndex > 0 && comparer.Compare(list.GetAt((firstIndex - 1)), target) == 0)
                firstIndex--;
            while (lastIndex < list.LongCount - 1 && comparer.Compare(list.GetAt(lastIndex + 1), target) == 0)
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

        public static (long index, bool exists) SortedFind<L, T>(this L list, bool allowDuplicates, T target, IComparer<T> comparer) where L : ILazinatorListable<T> where T : ILazinator
        {
            bool found = false;
            if (list.LongCount == 0)
                return (0, false);
            long first = 0, last = list.LongCount - 1;
            long mid = 0;

            //for a sorted array with descending values
            while (!found && first <= last)
            {
                mid = (first + last) / 2;

                int comparison = comparer.Compare(target, list.GetAt(mid));
                if (comparison == 0)
                {
                    if (allowDuplicates)
                    { // return first match
                        bool matches = true;
                        while (matches && mid > 0)
                        {
                            var previous = list.GetAt(mid - 1);
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
