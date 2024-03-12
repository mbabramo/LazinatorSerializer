using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility
{
    /// <summary>
    /// Split a range defined by a generic parameter that is a float or a double. This overcomes a limitation of C#, that we can't do arithmetic operations with generic parameters.
    /// </summary>
    public static class GetRangeSplitter
    {
        public static SplitGenericRange<T> GetSplitter<T>()
        {
            Type t = typeof(T);
            if (t == typeof(float))
                return new RangeSplitterFloat() as SplitGenericRange<T>;
            if (t == typeof(double))
                return new RangeSplitterDouble() as SplitGenericRange<T>;
            throw new NotImplementedException();
        }
    }

    public abstract class SplitGenericRange<T>
    {
        /// <summary>
        /// Splits a range represented by a generic value, returning numRanges + 1 values.
        /// </summary>
        /// <param name="startRange"></param>
        /// <param name="endRange"></param>
        /// <param name="numRanges"></param>
        /// <returns></returns>
        public abstract IEnumerable<T> Split(T startRange, T endRange, int numRanges);

        /// <summary>
        /// Splits a range represented by a generic value, returning numRanges - 1 values.
        /// </summary>
        /// <param name="startRange"></param>
        /// <param name="endRange"></param>
        /// <param name="numRanges"></param>
        /// <returns></returns>
        public IEnumerable<T> GetInteriorSplits(T startRange, T endRange, int numRanges)
        {
            var omittingFirst = Split(startRange, endRange, numRanges).Skip(1).ToList();
            var omittingLast = omittingFirst.Take(omittingFirst.Count() - 1);
            return omittingLast;
        }
    }

    public class RangeSplitterFloat : SplitGenericRange<WFloat>
    {
        public override IEnumerable<WFloat> Split(WFloat startRange, WFloat endRange, int numRanges)
        {
            float spacing = (endRange - startRange) / (float)numRanges;
            return Enumerable.Range(0, numRanges + 1).Select(x => x == numRanges ? (WFloat) endRange /* avoid rounding errors */ : (WFloat) (startRange + spacing * x));
        }
    }

    public class RangeSplitterDouble : SplitGenericRange<double>
    {
        public override IEnumerable<double> Split(double startRange, double endRange, int numRanges)
        {
            double spacing = (endRange - startRange) / (double)numRanges;
            return Enumerable.Range(0, numRanges + 1).Select(x => x == numRanges ? endRange /* avoid rounding errors */ : startRange + spacing * x);
        }
    }
}
