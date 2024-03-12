using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility
{
    public static class ListUtils
    {
        public static IEnumerable<T> ExceptLast<T>(this List<T> values)
        {
            return values.Take(values.Count() - 1);
        }

        public static List<List<T>> SplitValues<T>(this IEnumerable<T> values, IEnumerable<T> maximumPoints) where T : IComparable
        {
            List<List<T>> masterList = new List<List<T>>();
            var valueEnumerator = values.GetEnumerator();
            bool moreValues = valueEnumerator.MoveNext();
            foreach (var splitPoint in maximumPoints)
            {
                List<T> subList = new List<T>();
                while (moreValues && valueEnumerator.Current.CompareTo(splitPoint) <= 0)
                {
                    subList.Add(valueEnumerator.Current);
                    moreValues = valueEnumerator.MoveNext();
                }
                masterList.Add(subList);
            }
            return masterList;
        }

        public static void SeparateOriginalAndReplacementListItems<T>(List<T> original, List<T> replacement, out List<T> originalOnly, out List<T> replacementOnly, out List<T> both)
        {
            originalOnly = original.Where(x => !replacement.Any(y => y.Equals(x))).ToList();
            replacementOnly = replacement.Where(x => !original.Any(y => y.Equals(x))).ToList();
            both = original.Where(x => replacement.Any(y => y.Equals(x))).ToList();
        }


        public static IEnumerable<List<T>> Split<T>(this List<T> list, int parts)
        {
            int c = list.Count();
            if (parts > c)
                throw new NotImplementedException("Cannot split list into more parts than elements.");
            int numItemsPerPart = c / parts;
            int extraItems = c - parts * numItemsPerPart;
            int lastIndex = -1;
            for (int p = 0; p < parts; p++)
            {
                int firstIndex = lastIndex + 1;
                lastIndex = firstIndex + numItemsPerPart - 1;
                if (p + 1 <= extraItems)
                    lastIndex++;
                yield return list.GetRange(firstIndex, lastIndex - firstIndex + 1).ToList();
            }
        }
    }
}
