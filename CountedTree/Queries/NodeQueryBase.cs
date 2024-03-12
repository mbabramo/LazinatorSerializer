using CountedTree.PendingChanges;
using CountedTree.NodeResults;
using CountedTree.Core;
using System;
using System.Collections.Generic;
using CountedTree.Node;
using System.Threading.Tasks;
using CountedTree.UintSets;
using Lazinator.Core;

namespace CountedTree.Queries
{
    public abstract partial class NodeQueryBase<TKey> : INodeQueryBase<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public NodeQueryBase()
        {

        }

        public NodeQueryBase(long nodeID, bool nodeExists, PendingChangesCollection<TKey> pendingChanges, QueryResultType nodeResultType, uint? take, QueryFilter filter)
        {
            NodeID = nodeID;
            NodeHasStorage = nodeExists;
            PendingChanges = pendingChanges;
            NodeResultType = nodeResultType;
            Take = take;
            Filter = filter;
        }


        public override bool Equals(object obj)
        {
            NodeQueryBase<TKey> other = obj as NodeQueryBase<TKey>;
            if (other == null)
                return false;
            return NodeID == other.NodeID && NodeHasStorage == other.NodeHasStorage && PendingChanges.Equals(other.PendingChanges) && Equals(Filter, other.Filter) && Take == other.Take && NodeResultType == other.NodeResultType;
        }

        public override int GetHashCode()
        {
            int x = new Tuple<long, bool, PendingChangesCollection<TKey>>(NodeID, NodeHasStorage, PendingChanges).GetHashCode();
            int y = new Tuple<QueryFilter, uint?, QueryResultType>(Filter, Take, NodeResultType).GetHashCode();
            return new Tuple<int, int>(x, y).GetHashCode();
        }

        public abstract NodeQueryBase<TKey> WithRevisedPendingChanges(uint numSubtreeValues, PendingChangesCollection<TKey> revised);

        public abstract bool AllItemsAreInRange(KeyAndID<TKey>? nodeLowerBound, KeyAndID<TKey>? nodeUpperBound);

        public abstract bool ItemMatches(KeyAndID<TKey> item, uint index, bool filtered);

        public abstract NodeResultBase<TKey> GetResultFromMatches(List<RankKeyAndID<TKey>> matches, uint filteredMatches, uint supersetMatches);

        public IncludedIndices ModifyIncludedIndices(uint numSubtreeValues, IncludedIndices includedIndices, PendingChangesCollection<TKey> revised)
        {
            // We only need to modify included indices on the root node. After that, we take into account the changes for the child nodes. We may still CorrectPendingChanges on those nodes because of local moves, but the expected indices won't change.
            if (includedIndices != null && (includedIndices.FirstIndexInFilteredSet != 0 || includedIndices.FirstIndexInSuperset != 0))
                return includedIndices;
            var numInSuperset = revised.NumValuesInTreeAfterPendingChanges(numSubtreeValues);
            var numInFiltered = revised.NumValuesInTreeAfterPendingChanges(numSubtreeValues); // this isn't really correct, but it doesn't matter; what matters from the root node are the results -- see note in SetInitialQuery
            return new IncludedIndices(0, numInSuperset - 1, 0, numInFiltered - 1);
        }

        internal virtual async Task<NodeResultBase<TKey>> ProcessOnEmptyNode()
        {
            CountedLeafNode<TKey> tempNode = new CountedLeafNode<TKey>(new List<KeyAndID<TKey>>(), 0, 0, null, null, null); /* fill with dummy data */
            NodeResultBase<TKey> result = await tempNode.ProcessQuery(this);
            return result;
        }

        public virtual bool ItemIsPotentialMatch(KeyAndID<TKey> item)
        {
            if (Filter?.Superset == null)
                return true;
            return (Filter?.Superset).Contains(item.ID);
        }

        internal bool ItemIsInFilter(KeyAndID<TKey> item, bool filtered)
        {
            UintSet applicableFilter = GetApplicableFilter(filtered);
            if (applicableFilter == null)
                return true;
            return applicableFilter.Contains(item.ID);
        }

        internal UintSet GetApplicableFilter(bool filtered)
        {
            return filtered ? (Filter?.SearchWithin ?? Filter?.Superset) : Filter?.Superset;
        }
    }
}
