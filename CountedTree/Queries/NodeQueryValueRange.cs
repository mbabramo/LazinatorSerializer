using System;
using CountedTree.Core;
using CountedTree.PendingChanges;
using CountedTree.NodeResults;
using CountedTree.Node;
using Lazinator.Core;

namespace CountedTree.Queries
{
    /// <summary>
    /// A query to output values within some range in order. This type of query cannot be used in conjunction with additional filters.
    /// An important application is to produce a filter based on a particular tree. That filter can then be used to query from the same or a different tree using NodeQueryIndexRange.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public partial class NodeQueryValueRange<TKey> : NodeQueryLinearBase<TKey>, INodeQueryValueRange<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public override bool Equals(object obj)
        {
            NodeQueryValueRange<TKey> other = obj as NodeQueryValueRange<TKey>;
            if (other == null)
                return false;
            return base.Equals(obj) && ((StartingValue.HasValue == false && other.StartingValue.HasValue == false) || (StartingValue.HasValue && other.StartingValue.HasValue && Equals(StartingValue.Value, other.StartingValue.Value))) && ((EndingValue.HasValue == false && other.EndingValue.HasValue == false) || (EndingValue.HasValue && other.EndingValue.HasValue && Equals(EndingValue.Value, other.EndingValue.Value)));
        }

        public override int GetHashCode()
        {
            return new Tuple<KeyAndID<TKey>?, KeyAndID<TKey>?, int>(StartingValue, EndingValue, base.GetHashCode()).GetHashCode();
        }


        public override NodeQueryBase<TKey> WithRevisedPendingChanges(uint numSubtreeValues, PendingChangesCollection<TKey> revised)
        {
            var includedIndices = ModifyIncludedIndices(numSubtreeValues, IncludedIndices, revised);
            return new NodeQueryValueRange<TKey>(NodeID, NodeHasStorage, Ascending, Skip, Take, includedIndices, revised, NodeResultType, StartingValue, EndingValue);
        }

        public NodeQueryValueRange()
        {

        }

        public NodeQueryValueRange(long nodeID, bool nodeExists, bool ascending, uint skip, uint? take, IncludedIndices includedIndices, PendingChangesCollection<TKey> pendingChanges, QueryResultType nodeResultType, KeyAndID<TKey>? startingValue, KeyAndID<TKey>? endingValue) : base(nodeID, nodeExists, ascending, skip, take, includedIndices, pendingChanges, nodeResultType, null)
        {
            StartingValue = startingValue;
            EndingValue = endingValue;
        }

        public override NodeQueryLinearBase<TKey> GenerateChildQuery(long nodeID, bool nodeExists, IncludedIndices includedIndices, uint skip, uint? take, PendingChangesCollection<TKey> pendingChangesForChildNode)
        {
            return new NodeQueryValueRange<TKey>(nodeID, nodeExists, Ascending, skip, take, includedIndices, pendingChangesForChildNode, NodeResultType, StartingValue, EndingValue);
        }


        public override void RangeOfNumberExpectedToReturn(uint expectedFirstIndex, uint expectedLastIndex, KeyAndID<TKey>? nodeLowerBound, KeyAndID<TKey>? nodeUpperBound, uint skip, uint? take, out uint minNumberSkipped, out uint maxNumberSkipped, out uint minNumberAvailableAfterSkipping, out uint maxNumberAvailableAfterSkipping, out uint minNumberReturned, out uint maxNumberReturned)
        {
            PortionOfNodeInSearchRange overlap = GetPortionOfNodeInSearchRange(nodeLowerBound, nodeUpperBound);
            switch (overlap)
            {
                case PortionOfNodeInSearchRange.EntireNodeIsWithinSearchRange:
                    base.RangeOfNumberExpectedToReturn(expectedFirstIndex, expectedLastIndex, nodeLowerBound, nodeUpperBound, skip, take, out minNumberSkipped, out maxNumberSkipped, out minNumberAvailableAfterSkipping, out maxNumberAvailableAfterSkipping, out minNumberReturned, out maxNumberReturned);
                    return;
                case PortionOfNodeInSearchRange.EntireNodeIsOutsideOfSearchRange:
                    minNumberSkipped = 0;
                    maxNumberSkipped = 0;
                    minNumberAvailableAfterSkipping = 0;
                    maxNumberAvailableAfterSkipping = 0;
                    minNumberReturned = 0;
                    maxNumberReturned = 0;
                    return;
                case PortionOfNodeInSearchRange.SomeOfNodeIsWithinSearchRange:
                    uint minToReturn = 0;
                    uint maxToReturn = expectedLastIndex - expectedFirstIndex + 1;
                    CalculateSkippedAndReturnedRanges(minToReturn, maxToReturn, skip, take, out minNumberSkipped, out maxNumberSkipped, out minNumberAvailableAfterSkipping, out maxNumberAvailableAfterSkipping, out minNumberReturned, out maxNumberReturned);
                    return;
                default:
                    throw new NotImplementedException("Unknown overlap type.");
            }
        }

        public enum PortionOfNodeInSearchRange
        {
            EntireNodeIsOutsideOfSearchRange,
            SomeOfNodeIsWithinSearchRange,
            EntireNodeIsWithinSearchRange
        }

        public PortionOfNodeInSearchRange GetPortionOfNodeInSearchRange(KeyAndID<TKey>? nodeLowerBound, KeyAndID<TKey>? nodeUpperBound)
        {
            // We can determine the overlap by asking four questions:
            // Is nodeLowerBound in the search area? Is nodeUpperBound in the search area? Is search lower bound within the node? Is search upper bound within the node?
            // If the first two are yes, then the node is entirely in the search area. 
            // If one of the first two is yes, then the node is partly in the search area.
            // If the first two are no, then either the node is to the left of the search area, to the right of the search area, or completely encloses the search area.
            // The nodeLowerBound is exclusive, while the nodeUpperBound is inclusive. But the StartingValue and EndingValue are both inclusive. This makes a difference when comparing the NodeLowerBound and the search upper bound.
            // If all four are no, then the node is entirely out of the search area.
            // Otherwise, the node is partly in the search area.
            // The tricky one is the search upper bound. Consider search [0, 1]. Node is (1, 2]. Then the search upper bound is NOT within the node.
            int compareNodeLowerBoundToStartingValue = CompareRangeExtremes(nodeLowerBound, StartingValue, true, true);
            int compareNodeLowerBoundToEndingValue = CompareRangeExtremes(nodeLowerBound, EndingValue, true, false);
            int compareNodeUpperBoundToStartingValue = CompareRangeExtremes(nodeUpperBound, StartingValue, false, true);
            int compareNodeUpperBoundToEndingValue = CompareRangeExtremes(nodeUpperBound, EndingValue, false, false);
            bool nodeLowerBoundInSearchArea = compareNodeLowerBoundToStartingValue >= 0 && compareNodeLowerBoundToEndingValue < 0; // Note that the node lower bound is inclusive, thus when comparing it to the search upper bound, we look for < 0. We don't need to worry about the inclusivity in the lower comparison.
            bool nodeUpperBoundInSearchArea = compareNodeUpperBoundToStartingValue >= 0 && compareNodeUpperBoundToEndingValue <= 0;
            if (nodeLowerBoundInSearchArea && nodeUpperBoundInSearchArea)
                return PortionOfNodeInSearchRange.EntireNodeIsWithinSearchRange;
            if (nodeLowerBoundInSearchArea || nodeUpperBoundInSearchArea)
                return PortionOfNodeInSearchRange.SomeOfNodeIsWithinSearchRange;
            bool searchAreaIsEntirelyWithinNode = compareNodeLowerBoundToStartingValue <= 0 && compareNodeUpperBoundToEndingValue >= 0;
            if (searchAreaIsEntirelyWithinNode)
                return PortionOfNodeInSearchRange.SomeOfNodeIsWithinSearchRange;
            else // the node is either entirely to the left or to the right of the search area 
                return PortionOfNodeInSearchRange.EntireNodeIsOutsideOfSearchRange;
        }

        /// <summary>
        /// Determines whether one range extreme is less than, equal to, or greater than another, taking into account null conditions and whether the extremes are lower bounds or upper bounds.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item1IsLowerBound"></param>
        /// <param name="item2IsLowerBound"></param>
        /// <returns></returns>
        private int CompareRangeExtremes(KeyAndID<TKey>? item1, KeyAndID<TKey>? item2, bool item1IsLowerBound, bool item2IsLowerBound)
        {
            if (item1 != null && item2 != null)
                return ((KeyAndID<TKey>)item1).CompareTo((KeyAndID<TKey>)item2);
            if (item1 == null && item2 == null)
            {
                if (item1IsLowerBound == item2IsLowerBound)
                    return 0; // equal
                if (item1IsLowerBound)
                    return -1; // item2 is greater
                else
                    return 1; // item1 is greater
            }
            if (item1 == null)
            {
                if (item1IsLowerBound)
                    return -1; // item2 is greater
                else
                    return 1; // item1 is greater
            }
            if (item2 == null)
            {
                if (item2IsLowerBound)
                    return 1; // item1 is greater
                else
                    return -1; // item2 is greater
            }
            throw new NotImplementedException();
        }

        public override bool ItemIsPotentialMatch(KeyAndID<TKey> item)
        {
            return ItemMatches(item, 0, false);
        }

        public override bool ItemMatches(KeyAndID<TKey> item, uint index, bool filtered)
        {
            // filtered is ignored, since we have no filters for value range queries
                return ((StartingValue == null || item >= StartingValue) && (EndingValue == null || item <= EndingValue));
        }

        public override bool AllItemsAreInRange(KeyAndID<TKey>? nodeLowerBound, KeyAndID<TKey>? nodeUpperBound)
        {
            var overlap = GetPortionOfNodeInSearchRange(nodeLowerBound, nodeUpperBound);
            return overlap == PortionOfNodeInSearchRange.EntireNodeIsWithinSearchRange;
        }

    }
}
