using System;
using System.Collections.Generic;
using CountedTree.Core;
using CountedTree.PendingChanges;
using CountedTree.NodeResults;
using CountedTree.Node;
using Lazinator.Core;

namespace CountedTree.Queries
{
    public abstract partial class NodeQueryLinearBase<TKey> : NodeQueryBase<TKey>, INodeQueryLinearBase<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        public NodeQueryLinearBase()
        {

        }

        public NodeQueryLinearBase(long nodeID, bool nodeExists, bool ascending, uint skip, uint? take, IncludedIndices includedIndices, PendingChangesCollection<TKey> pendingChanges, QueryResultType nodeResultType, QueryFilter filter) : base(nodeID, nodeExists, pendingChanges, nodeResultType, take, filter)
        {
            Ascending = ascending;
            Skip = skip;
            IncludedIndices = includedIndices;
        }

        public override bool Equals(object obj)
        {
            NodeQueryLinearBase<TKey> other = obj as NodeQueryLinearBase<TKey>;
            if (other == null)
                return false;
            return base.Equals(obj) && Ascending == other.Ascending && Skip == other.Skip && IncludedIndices.Equals(other.IncludedIndices);
        }

        public override int GetHashCode()
        {
            return new Tuple<bool, uint, IncludedIndices, int>(Ascending, Skip, IncludedIndices, base.GetHashCode()).GetHashCode();
        }

        public abstract NodeQueryLinearBase<TKey> GenerateChildQuery(long nodeID, bool nodeExists, IncludedIndices includedIndices, uint skip, uint? take, PendingChangesCollection<TKey> pendingChangesForChildNode);

        /// <summary>
        /// The range of items that we will skip, take, and ultimately return for the node that is receiving this query.
        /// </summary>
        /// <param name="expectedFirstIndex"></param>
        /// <param name="expectedLastIndex"></param>
        /// <returns></returns>
        public void RangeOfNumberExpectedToReturn(KeyAndID<TKey>? nodeLowerBound, KeyAndID<TKey>? nodeUpperBound, out uint minNumberSkipped, out uint maxNumberSkipped, out uint minNumberAvailableAfterSkipping, out uint maxNumberAvailableAfterSkipping, out uint minNumberReturned, out uint maxNumberReturned)
        {
            RangeOfNumberExpectedToReturn(IncludedIndices.FirstIndexInFilteredSet, IncludedIndices.LastIndexInFilteredSet, nodeLowerBound, nodeUpperBound, Skip, Take, out minNumberSkipped, out maxNumberSkipped, out minNumberAvailableAfterSkipping, out maxNumberAvailableAfterSkipping, out minNumberReturned, out maxNumberReturned);
        }

        /// <summary>
        /// The range of items that we will skip, take, and ultimately return for a specific node.
        /// </summary>
        /// <param name="expectedFirstIndex"></param>
        /// <param name="expectedLastIndex"></param>
        /// <returns></returns>
        public virtual void RangeOfNumberExpectedToReturn(uint expectedFirstIndex, uint expectedLastIndex, KeyAndID<TKey>? nodeLowerBound, KeyAndID<TKey>? nodeUpperBound, uint skip, uint? take, out uint minNumberSkipped, out uint maxNumberSkipped, out uint minNumberAvailableAfterSkipping, out uint maxNumberAvailableAfterSkipping, out uint minNumberReturned, out uint maxNumberReturned)
        {
            uint expectedNumberItemsInNode = (expectedLastIndex - expectedFirstIndex + 1); // since we are not using a value range, we know exactly how many items will match. 
            CalculateSkippedAndReturnedRanges(expectedNumberItemsInNode, expectedNumberItemsInNode, skip, take, out minNumberSkipped, out maxNumberSkipped, out minNumberAvailableAfterSkipping, out maxNumberAvailableAfterSkipping, out minNumberReturned, out maxNumberReturned);
        }

        internal void CalculateSkippedAndReturnedRanges(uint minNumberMatching, uint maxNumberMatching, uint skip, uint? take, out uint minNumberSkipped, out uint maxNumberSkipped, out uint minNumberAvailableAfterSkipping, out uint maxNumberAvailableAfterSkipping, out uint minNumberReturned, out uint maxNumberReturned)
        {
            minNumberSkipped = Math.Min(skip, minNumberMatching);
            maxNumberSkipped = Math.Min(skip, maxNumberMatching);
            minNumberAvailableAfterSkipping = minNumberMatching - minNumberSkipped;
            maxNumberAvailableAfterSkipping = maxNumberMatching - maxNumberSkipped;
            minNumberReturned = Math.Min(minNumberAvailableAfterSkipping, take ?? minNumberAvailableAfterSkipping);
            maxNumberReturned = Math.Min(maxNumberAvailableAfterSkipping, take ?? maxNumberAvailableAfterSkipping);
        }

        public override NodeResultBase<TKey> GetResultFromMatches(List<RankKeyAndID<TKey>> matches, uint filteredMatches, uint supersetMatches)
        {
            NodeResultLinearBase<TKey> r;
            switch (NodeResultType)
            {
                case QueryResultType.IDsOnly:
                case QueryResultType.IDsAsBitSet: // Since we are setting back individual results (i.e., from leaf node), we'll just do it by listing them individually.
                    r = new NodeResultIDs<TKey>(IncludedIndices, filteredMatches, supersetMatches);
                    break;
                case QueryResultType.KeysOnly:
                    r = new NodeResultKeys<TKey>(IncludedIndices, filteredMatches, supersetMatches);
                    break;
                case QueryResultType.KeysAndIDs:
                    r = new NodeResultKeysAndIDs<TKey>(IncludedIndices, filteredMatches, supersetMatches);
                    break;
                case QueryResultType.IDsAndRanks:
                    r = new NodeResultRanksAndIDs<TKey>(IncludedIndices, filteredMatches, supersetMatches);
                    break;
                case QueryResultType.KeysIDsAndDistance:
                    throw new NotImplementedException("Linear query should not request Geo result.");
                default:
                    throw new NotImplementedException("Unknown NodeRequestType.");
            }
            r.SetResults(matches);
            return r;
        }

    }
}
