using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utility
{
    public enum RangeOverlapPossibilities
    {
        Item1IsIdenticalToItem2,
        Item1WithinItem2,
        Item2WithinItem1,
        Item1IsToLeftOfItem2,
        Item1IsToRightOfItem2,
        Item1RightAlignsWithItem2Left,
        Item1LeftAlignsWithItem2Right,
        Item1PartlyOverlapsWithItem2,
    }

    public static class RangeOverlap<T> where T : IComparable<T>
    {
        public static RangeOverlapPossibilities GetRangeOverlap(T item1Left, T item1Right, T item2Left, T item2Right)
        {
            if (item1Left.CompareTo(item2Left) == 0 && item1Right.CompareTo(item2Right) == 0)
                return RangeOverlapPossibilities.Item1IsIdenticalToItem2;
            if (item1Left.CompareTo(item2Left) >= 0 && item1Right.CompareTo(item2Right) <= 0)
                return RangeOverlapPossibilities.Item1WithinItem2;
            if (item2Left.CompareTo(item1Left) >= 0 && item2Right.CompareTo(item1Right) <= 0)
                return RangeOverlapPossibilities.Item2WithinItem1;
            if (item1Right.CompareTo(item2Left) < 0)
                return RangeOverlapPossibilities.Item1IsToLeftOfItem2;
            if (item1Left.CompareTo(item2Right) > 0)
                return RangeOverlapPossibilities.Item1IsToRightOfItem2;
            if (item1Right.CompareTo(item2Left) == 0)
                return RangeOverlapPossibilities.Item1RightAlignsWithItem2Left;
            if (item1Left.CompareTo(item2Right) == 0)
                return RangeOverlapPossibilities.Item1LeftAlignsWithItem2Right;
            return RangeOverlapPossibilities.Item1PartlyOverlapsWithItem2;
        }

        public static bool SomeOverlapExists(T item1Left, T item1Right, T item2Left, T item2Right, bool item1LeftExclusive, bool item1RightExclusive, bool item2LeftExclusive, bool item2RightExclusive)
        {
            var overlapType = GetRangeOverlap(item1Left, item1Right, item2Left, item2Right);
            switch (overlapType)
            {
                case RangeOverlapPossibilities.Item1IsIdenticalToItem2:
                case RangeOverlapPossibilities.Item1WithinItem2:
                case RangeOverlapPossibilities.Item2WithinItem1:
                case RangeOverlapPossibilities.Item1PartlyOverlapsWithItem2:
                    return true;
                case RangeOverlapPossibilities.Item1IsToLeftOfItem2:
                case RangeOverlapPossibilities.Item1IsToRightOfItem2:
                    return false;
                // If they align only at a point, both ranges must be inclusive for there to be any overlap.
                case RangeOverlapPossibilities.Item1RightAlignsWithItem2Left:
                    return !item1RightExclusive && !item2LeftExclusive;
                case RangeOverlapPossibilities.Item1LeftAlignsWithItem2Right:
                    return !item1LeftExclusive && !item2RightExclusive;
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool Item1IsCompletelyInItem2(T item1Left, T item1Right, T item2Left, T item2Right, bool item1LeftExclusive, bool item1RightExclusive, bool item2LeftExclusive, bool item2RightExclusive)
        {
            var overlapType = GetRangeOverlap(item1Left, item1Right, item2Left, item2Right);
            switch (overlapType)
            {
                case RangeOverlapPossibilities.Item1IsIdenticalToItem2:
                    if (!item1LeftExclusive && item2LeftExclusive)
                        return false; // the left of item 1 is outside the item 2 range
                    if (!item1RightExclusive && item2RightExclusive)
                        return false; // the right of item 1 is outside the item 2 range
                    return true;
                case RangeOverlapPossibilities.Item1WithinItem2:
                    return true;
                case RangeOverlapPossibilities.Item2WithinItem1:
                case RangeOverlapPossibilities.Item1PartlyOverlapsWithItem2:
                case RangeOverlapPossibilities.Item1IsToLeftOfItem2:
                case RangeOverlapPossibilities.Item1IsToRightOfItem2:
                case RangeOverlapPossibilities.Item1RightAlignsWithItem2Left:
                case RangeOverlapPossibilities.Item1LeftAlignsWithItem2Right:
                    return false;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
