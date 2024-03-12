using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility
{
    public static class EnumerableExtensionMethods
    {
        public static IEnumerable<T> SkipItemsInMiddle<T>(this IEnumerable<T> list, int startSkippingIndex, int numToSkip)
        {
            var enumerator = list.GetEnumerator();
            int i = -1;
            while (enumerator.MoveNext())
            {
                i++;
                if (i < startSkippingIndex || i >= startSkippingIndex + numToSkip)
                    yield return enumerator.Current;
            }
        }

        public static bool IsSorted<T>(this IEnumerable<T> list, bool allowRepeats = false) where T : IComparable
        {
            var y = list.FirstOrDefault();
            if (y == null)
                return true;
            return list.Skip(1).All(x =>
            {
                bool b = allowRepeats ? y.CompareTo(x) < 0 : y.CompareTo(x) <= 0;
                y = x;
                return b;
            });
        }

        public static bool ContainsAdjacentRepeats<T>(this IEnumerable<T> list) where T : IComparable
        {
            var y = list.FirstOrDefault();
            if (y == null)
                return true;
            return list.Skip(1).Any(x =>
            {
                bool b = y.CompareTo(x) == 0;
                y = x;
                return b;
            });
        }

        public static IEnumerable<T> WithoutAdjacentRepeats<T>(this IEnumerable<T> list) where T : IComparable
        {
            var y = list.FirstOrDefault();
            if (y == null)
                yield break;
            bool isFirst = true;
            T lastValue = default(T);
            foreach (var item in list)
            {
                if (isFirst || item.CompareTo(lastValue) != 0)
                    yield return item;
                isFirst = false;
                lastValue = item;
            }
        }

        public static IEnumerable<T> MergeOrderedEnumerables<T>(this IEnumerable<T> firstEnumerable, IEnumerable<T> secondEnumerable) where T : IComparable
        {
            return MergeOrderedEnumerables(new IEnumerable<T>[] { firstEnumerable, secondEnumerable });
        }

        public static IEnumerable<T> MergeOrderedEnumerables<T>(this IEnumerable<IEnumerable<T>> orderedEnumerables, bool ascending = true) where T : IComparable
        {
            var enumerators = orderedEnumerables.Select(y => new EnumeratorPlus<T>(y.GetEnumerator())).ToArray();
            int? bestYetIndex;
            do
            {
                bestYetIndex = null;
                T bestYetItem = default(T);
                for (int i = 0; i < enumerators.Length; i++) // foreach buffer
                {
                    var nextItem = enumerators[i].GetNextOrDefault();
                    if (nextItem != null && 
                        (
                            bestYetItem == null || 
                            ((ascending && nextItem.CompareTo(bestYetItem) < 0) || (!ascending && nextItem.CompareTo(bestYetItem) > 0))
                        )
                        )
                    {
                        bestYetIndex = i;
                        bestYetItem = nextItem;
                    }
                }
                if (bestYetIndex != null)
                {
                    yield return bestYetItem;
                    enumerators[(int)bestYetIndex].MoveNext();
                }
            }
            while (bestYetIndex != null);
        }
    }
}
