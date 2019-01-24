using Lazinator.Collections.Enumerators;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Extensions
{
    public static class LazinatorListableMultivalueExtensions
    {
        public static IEnumerator<T> MultivalueGetEnumerator<L, T>(this L list, bool reverse, T startValue, IComparer<T> comparer) where L : ILazinatorListable<T>, IIndexableMultivalueContainer<T> where T : ILazinator
        {
            var result = list.FindIndex(startValue, reverse ? MultivalueLocationOptions.Last : MultivalueLocationOptions.First, comparer);
            long skipAdjusted = reverse ? list.LongCount - result.index - 1 : result.index;
            return new ListableEnumerator<T>(list, reverse, skipAdjusted);
        }

        public static IEnumerable<T> MultivalueAsEnumerable<L, T>(this L list, bool reverse, T startValue, IComparer<T> comparer) where L : ILazinatorListable<T>, IIndexableMultivalueContainer<T> where T : ILazinator
        {
            var result = list.FindIndex(startValue, reverse ? MultivalueLocationOptions.Last : MultivalueLocationOptions.First, comparer);
            long skipAdjusted = reverse ? list.LongCount - result.index - 1 : result.index;
            return ((ILazinatorListable<T>)list).AsEnumerable(reverse, skipAdjusted);
        }

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
