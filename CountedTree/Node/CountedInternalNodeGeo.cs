using CountedTree.Core;
using CountedTree.PendingChanges;
using CountedTree.NodeBuffers;
using CountedTree.NodeResults;
using CountedTree.Queries;
using R8RUtilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lazinator.Wrappers;
using Utility;

namespace CountedTree.Node
{
    public partial class CountedInternalNodeGeo : CountedInternalNode<WUInt64>, ICountedInternalNodeGeo
    {
        public CountedInternalNodeGeo()
        {

        }

        public CountedInternalNodeGeo(NodeInfo<WUInt64>[] childNodeInfos, PendingChangesCollection<WUInt64> pendingChanges, long nodeID, byte depth, TreeStructure treeStructure, IBlob<Guid> uintSetStorage) : base(childNodeInfos, pendingChanges, nodeID, depth, treeStructure)
        {
            if (TreeStructure.NumChildrenPerInternalNode != 4 && TreeStructure.NumChildrenPerInternalNode != 16 && TreeStructure.NumChildrenPerInternalNode != 64 && TreeStructure.NumChildrenPerInternalNode != 256)
                throw new Exception("Invalid number of children for a geo node"); // must be a power of 4
        }

        public CountedInternalNodeGeo(byte depth, NodeInfo<WUInt64> thisNodeInfo, NodeInfo<WUInt64>[] childNodeInfos, INodeBufferBaseMethods<WUInt64> mainBuffer, TreeStructure treeStructure) : base(depth, thisNodeInfo, childNodeInfos, mainBuffer, treeStructure)
        {
        }

        public CountedInternalNodeGeo(CountedInternalNode<WUInt64> node) : base(node.NodeInfo, node.ChildNodeInfos, node.Cumulator, node.MainBuffer, node.InheritedMainSetNodeID, node.InheritedDeltaSetsNodeID, node.InheritMainSetContext, node.InheritDeltaSetsContext, node.UintSetInitialized, node.Depth, node.TreeStructure)
        {
        }

        public override async Task<NodeResultBase<WUInt64>> ProcessQuery(NodeQueryBase<WUInt64> request)
        {
            NodeQueryGeo geoRequest = (NodeQueryGeo) request;

            CombinedInternalNodeBuffer<WUInt64> combinedBuffer = new CombinedInternalNodeBuffer<WUInt64>(this, geoRequest.PendingChanges);
            List<NodeQueryBase<WUInt64>> furtherQueries = new List<NodeQueryBase<WUInt64>>();
            int absoluteNodeIndex = 0;
            long totalFurtherItemsToQuery = 0;
            var queryRectangle = geoRequest.QueryRectangle();
            if (geoRequest.NodeResultType == QueryResultType.IDsAsBitSet && TreeStructure.StoreUintSets && geoRequest.AllItemsAreInRange(null, null))
            {
                await SetUintSetStorage(StorageFactory.GetUintSetStorage());
                var uintSet = await GetUintSetWithChanges(geoRequest.PendingChanges);
                if (geoRequest.Filter == null || (geoRequest.Filter.SearchWithin == null && geoRequest.Filter.Superset == null))
                    return new NodeResultGeoFilter(geoRequest.NodeMortonRange, uintSet, uintSet.Count, uintSet.Count);
                var superset = uintSet;
                if (geoRequest.Filter.Superset != null)
                    superset = superset.Intersect(geoRequest.Filter.Superset);
                var filteredSet = uintSet.Intersect(geoRequest.Filter.SearchWithin ?? geoRequest.Filter.Superset);
                return new NodeResultGeoFilter(geoRequest.NodeMortonRange, filteredSet, filteredSet.Count, superset.Count);
            }
            foreach (NodeInfo<WUInt64> childInfo in ChildNodeInfos)
            {
                var itemsInSubtree = Cumulator.NumItemsInSubtreeIncludingBuffer(combinedBuffer, absoluteNodeIndex);
                if (itemsInSubtree > 0)
                {
                    ProperMortonRange mr = GetProperMortonRangeForChild(absoluteNodeIndex);
                    bool queryOverlaps = mr.Intersects(queryRectangle);
                    if (queryOverlaps)
                    {
                        totalFurtherItemsToQuery += itemsInSubtree;
                        var combinedPendingChangesAtNode = combinedBuffer.PendingChangesAtNodeIndex(absoluteNodeIndex);
                        NodeQueryGeo q = new NodeQueryGeo(childInfo.NodeID, childInfo.Created, combinedPendingChangesAtNode, geoRequest.QueryMortonCenter, geoRequest.QueryMaxDistanceFromCenter, mr, geoRequest.Filter); 
                        // It's still possible that this is outside our query circle. Even though the query rectangle intersects the Morton rectangle, the Morton rectangle may not intersect our query circle. Maybe it intersects just a corner. In addition, near the poles, the query circle may be substantially smaller than the query rectangle. (A query from a point just under 1.0 miles south of the pole with a 1.0 mile radius produces a rectangle that includes all points up to 2.0 miles south of the pole, because once we reach the pole, we have to include the full range of longitude values) 
                        if (q.ClosestPossibleResult <= geoRequest.QueryMaxDistanceFromCenter)
                            furtherQueries.Add(q);
                    }
                }
                absoluteNodeIndex++;
            }
            return new NodeResultGeoFurtherQueries(geoRequest.NodeMortonRange, furtherQueries, 0, 0); // We don't have any way of knowing how many items will match the query overall in a geo search, so we just return 0 for filtered and superset matches
        }

        internal ProperMortonRange GetProperMortonRangeForChild(int absoluteNodeIndex)
        {
            byte childTreeDepth = (byte)(Depth + 1); // this will be converted to Morton depth by the last call in this method
            ulong precedingOrStartValue;
            if (absoluteNodeIndex == 0)
                precedingOrStartValue = NodeInfo.FirstExclusive?.Key ?? ulong.MinValue;
            else
                precedingOrStartValue = SplitValue(absoluteNodeIndex - 1)?.Key ?? 0;
            return MortonEncoding.GetProperMortonRangeFollowingValue(precedingOrStartValue, childTreeDepth, TreeStructure.NumChildrenPerInternalNode);
        }
    }
}
