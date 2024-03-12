using CountedTree.PendingChanges;
using R8RUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using CountedTree.Core;
using CountedTree.NodeResults;
using Lazinator.Wrappers;
using Utility;

namespace CountedTree.Queries
{
    public partial class NodeQueryGeo : NodeQueryBase<WUInt64>, INodeQueryGeo
    {
        public NodeQueryGeo(long nodeID, bool nodeExists, PendingChangesCollection<WUInt64> pendingChanges, ulong queryMortonCenter, float queryMaxDistanceFromCenter, ProperMortonRange nodeRange, QueryFilter filter) : base(nodeID, nodeExists, pendingChanges, NodeResults.QueryResultType.KeysIDsAndDistance, null, filter)
        {
            QueryMortonCenter = queryMortonCenter;
            QueryLatLonCenter = MortonEncoding.morton2latlonpoint(queryMortonCenter);
            QueryMaxDistanceFromCenter = queryMaxDistanceFromCenter;
            NodeMortonRange = nodeRange;
            AssignClosestPossibleResult(); // points in center will count as 0
        }

        public override NodeQueryBase<WUInt64> WithRevisedPendingChanges(uint numSubtreeValues, PendingChangesCollection<WUInt64> revised)
        {
            return new NodeQueryGeo(NodeID, NodeHasStorage, revised, QueryMortonCenter, QueryMaxDistanceFromCenter, NodeMortonRange, Filter);
        }

        private void AssignClosestPossibleResult()
        {
            LatLonRectangle rect = NodeMortonRange.ToLatLonRectangle();
            ClosestPossibleResult = NodeMortonRange.ApproximateMilesToPoint(QueryLatLonCenter);

            // We need to be conservative, because the ApproximtaeMilesToPoint is only an estimate. This is as a result of complications in calculating distance between a point and a line when the best strategy may be to go near the North or South pole. By being conservative, we ensure that we wait for results that may be better than the ones we're returning.

            if (rect.Top >= 60F || rect.Bottom <= -60F)
                ClosestPossibleResult *= 0.8F;
            else if (rect.Top >= 50F || rect.Bottom <= -50F)
                ClosestPossibleResult *= 0.9F;
            else
                ClosestPossibleResult *= 0.98F;
            if (QueryMaxDistanceFromCenter > 1000F)
                ClosestPossibleResult *= 0.9F;
        }

        public LatLonRectangle QueryRectangle()
        {
            float miles = QueryMaxDistanceFromCenter;

            // The GetBoundingBox method is not perfect, particularly near the poles, so we will be conservative once again.

            if (QueryLatLonCenter.Latitude >= 60F || QueryLatLonCenter.Latitude <= -60F || miles > 300F)
                miles *= 1.2F;
            else if (QueryLatLonCenter.Latitude >= 50F || QueryLatLonCenter.Latitude <= -50F || miles > 100F)
                miles *= 1.05F;
            else
                miles *= 1.02F;
            if (miles > 1000)
                miles *= 1.1F;

            return BoundingBox.GetBoundingBox(QueryLatLonCenter, miles);
        }

        public override bool Equals(object obj)
        {
            NodeQueryGeo other = obj as NodeQueryGeo;
            if (other == null)
                return false;
            return QueryMortonCenter == other.QueryMortonCenter && QueryMaxDistanceFromCenter == other.QueryMaxDistanceFromCenter && NodeMortonRange.Equals(other.NodeMortonRange);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(QueryMortonCenter, QueryMaxDistanceFromCenter, NodeMortonRange).GetHashCode();
        }

        public override bool ItemIsPotentialMatch(KeyAndID<WUInt64> item)
        {
            return MortonEncoding.morton2latlonpoint(item.Key).ApproximateMilesTo(QueryLatLonCenter) < QueryMaxDistanceFromCenter;
        }

        public override bool ItemMatches(KeyAndID<WUInt64> item, uint index, bool filtered)
        {
            var applicableFilter = GetApplicableFilter(filtered);
            if (applicableFilter != null && !applicableFilter.Contains(item.ID))
                return false;
            return ItemIsPotentialMatch(item);
        }

        public override bool AllItemsAreInRange(KeyAndID<WUInt64>? nodeLowerBound, KeyAndID<WUInt64>? nodeUpperBound)
        {
            // Note that because we have NodeMortonRange saved here, we ignore the parameters.
            return NodeMortonRange.ToLatLonRectangle().EntirelyContains(QueryRectangle());
        }

        public override NodeResultBase<WUInt64> GetResultFromMatches(List<RankKeyAndID<WUInt64>> matches, uint filteredMatches, uint supersetMatches)
        {
            var result = new NodeResultGeoItems(NodeMortonRange, filteredMatches, supersetMatches);
            result.SetResults(matches.Select(x => new GeoResult(x.GetKeyAndID(), MortonEncoding.morton2latlonpoint(x.Key).ApproximateMilesTo(QueryLatLonCenter))).ToList());
            return result;
        }
        
    }
}
