using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Extensions
{
    public static class LazinatorListableMultivalueExtensions
    {
        public static (IContainerLocation location, bool found) MultivalueFindMatchOrNext<L, T>(this L list, bool allowDuplicates, T value, IComparer<T> comparer) where L : ILazinatorListable<T> where T : ILazinator => MultivalueFindMatchOrNext(list, allowDuplicates, value, MultivalueLocationOptions.Any, comparer);
        public static (IContainerLocation location, bool found) MultivalueFindMatchOrNext<L, T>(this L list, bool allowDuplicates, T value, MultivalueLocationOptions whichOne, IComparer<T> comparer) where L : ILazinatorListable<T> where T : ILazinator
        {
            (long index, bool exists) = list.SortedFind(allowDuplicates, value, whichOne, comparer);
            return (new IndexLocation(index, list.LongCount), exists);
        }

        public static bool MultivalueGetValue<L, T>(this L list, bool allowDuplicates, T item, IComparer<T> comparer, out T match) where L : ILazinatorListable<T> where T : ILazinator => MultivalueGetValue(list, allowDuplicates, item, MultivalueLocationOptions.Any, comparer, out match);
        public static bool MultivalueGetValue<L, T>(this L list, bool allowDuplicates, T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, out T match) where L : ILazinatorListable<T> where T : ILazinator
        {
            if (whichOne == MultivalueLocationOptions.InsertBeforeFirst || whichOne == MultivalueLocationOptions.InsertAfterLast)
            {
                throw new ArgumentException();
            }
            (long index, bool exists) = list.SortedFind(allowDuplicates, item, whichOne, comparer);
            if (exists)
                match = list.GetAtIndex(index);
            else
                match = default;
            return exists;
        }

        public static bool MultivalueTryInsert<L, T>(this L list, bool allowDuplicates, T item, IComparer<T> comparer) where L : ILazinatorListable<T> where T : ILazinator => MultivalueTryInsert(list, allowDuplicates, item, MultivalueLocationOptions.Any, comparer);
        public static bool MultivalueTryInsert<L, T>(this L list, bool allowDuplicates, T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) where L : ILazinatorListable<T> where T : ILazinator
        {
            var result = list.SortedInsertOrReplace(allowDuplicates, item, whichOne, comparer);
            return result.insertedNotReplaced;
        }

        public static bool MultivalueTryRemove<L, T>(this L list, bool allowDuplicates, T item, IComparer<T> comparer) where L : ILazinatorListable<T> where T : ILazinator => MultivalueTryRemove(list, allowDuplicates, item, MultivalueLocationOptions.Any, comparer);
        public static bool MultivalueTryRemove<L, T>(this L list, bool allowDuplicates, T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) where L : ILazinatorListable<T> where T : ILazinator
        {
            var result = list.SortedTryRemove(allowDuplicates, item, whichOne, comparer);
            return result;
        }

        public static bool MultivalueTryRemoveAll<L, T>(this L list, bool allowDuplicates, T item, IComparer<T> comparer) where L : ILazinatorListable<T> where T : ILazinator
        {
            bool found = false;
            bool foundAny = false;
            do
            {
                found = MultivalueTryRemove(list, allowDuplicates, item, MultivalueLocationOptions.Any, comparer);
                if (found)
                    foundAny = true;
            } while (found);
            return foundAny;
        }

        public static long MultivalueCount<L, T>(this L list, bool allowDuplicates, T item, IComparer<T> comparer) where L : ILazinatorListable<T> where T : ILazinator
        {
            (long firstIndex, bool exists) = list.SortedFind(allowDuplicates, item, MultivalueLocationOptions.First, comparer);
            if (!exists)
                return 0;
            (long lastIndex, _) = list.SortedFind(allowDuplicates, item, MultivalueLocationOptions.Last, comparer);
            return lastIndex - firstIndex + 1;
        }
    }
}
