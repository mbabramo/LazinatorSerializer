using System;
using CountedTree.PendingChanges;
using CountedTree.Node;
using CountedTree.NodeResults;
using CountedTree.Core;
using Lazinator.Core;

namespace CountedTree.Queries
{
    public partial class NodeQueryIndexRange<TKey> : NodeQueryLinearBase<TKey>, INodeQueryIndexRange<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        public NodeQueryIndexRange()
        {

        }

        public NodeQueryIndexRange(long nodeID, bool nodeExists, bool ascending, uint skip, uint? take, IncludedIndices includedIndices, PendingChangesCollection<TKey> pendingChanges, QueryResultType nodeResultType, QueryFilter filter) : base(nodeID, nodeExists, ascending, skip, take, includedIndices, pendingChanges, nodeResultType, filter)
        {

        }

        public override NodeQueryBase<TKey> WithRevisedPendingChanges(uint numSubtreeValues, PendingChangesCollection<TKey> revised)
        {
            var includedIndices = ModifyIncludedIndices(numSubtreeValues, IncludedIndices, revised);
            return new NodeQueryIndexRange<TKey>(NodeID, NodeHasStorage, Ascending, Skip, Take, includedIndices, revised, NodeResultType, Filter);
        }

        public override NodeQueryLinearBase<TKey> GenerateChildQuery(long nodeID, bool nodeExists, IncludedIndices includedIndices, uint skip, uint? take, PendingChangesCollection<TKey> pendingChangesForChildNode)
        {
            return new NodeQueryIndexRange<TKey>(nodeID, nodeExists, Ascending, skip, take, includedIndices, pendingChangesForChildNode, NodeResultType, Filter);
        }

        public override bool ItemMatches(KeyAndID<TKey> item, uint index, bool filtered)
        {
            return ItemIsInFilter(item, filtered); // we use the ID, not the index of the item. Note that for index range queries, the item will always be in range absent a filter.
        }

        public override bool AllItemsAreInRange(KeyAndID<TKey>? nodeLowerBound, KeyAndID<TKey>? nodeUpperBound)
        {
            return true; // the point here is that all items match the ultimate query (even if all items may not be included in the skip/take)
        }
    }
}
