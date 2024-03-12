using System;
using System.Collections.Generic;
using System.Linq;
using CountedTree.NodeResults;
using CountedTree.Queries;
using CountedTree.UintSets;
using Lazinator.Wrappers;

namespace CountedTree.QueryExecution
{
    public class LeafResultTrackerGeoItems : LeafResultTrackerBase<WUInt64>
    {
        /// <summary>
        /// The individual leaf results, sorted by distance.
        /// </summary>
        SortedSet<GeoResult> LeafResults;
        /// <summary>
        /// The pending queries, sorted by the closest possible distance.
        /// </summary>
        /// <param name="result"></param>
        SortedSet<NodeQueryGeo> PendingQueries;

        public float? ClosestPossiblePending => PendingQueries.Any() ? (float?) PendingQueries.First().ClosestPossibleResult : null;

        public LeafResultTrackerGeoItems()
        {
            Comparison<GeoResult> comparison = (x, y) =>
            {
                if (x.Distance != y.Distance)
                    return x.Distance.CompareTo(y.Distance);
                return x.KeyAndID.ID.CompareTo(y.KeyAndID.ID);
            };
            IComparer<GeoResult> comparer = System.Collections.Generic.Comparer<GeoResult>.Create(comparison);
            LeafResults = new SortedSet<GeoResult>(comparer);

            Comparison<NodeQueryGeo> comparison2 = (x, y) =>
            {
                if (x.ClosestPossibleResult != y.ClosestPossibleResult)
                    return x.ClosestPossibleResult.CompareTo(y.ClosestPossibleResult);
                return x.GetHashCode().CompareTo(y.GetHashCode());
            };
            IComparer<NodeQueryGeo> comparer2 = System.Collections.Generic.Comparer<NodeQueryGeo>.Create(comparison2);
            PendingQueries = new SortedSet<NodeQueryGeo>(comparer2);
        }

        public void AddInitialQuery(NodeQueryGeo initialQuery)
        {
            PendingQueries.Add(initialQuery);
        }

        public override void AddLeafResult(NodeResultBase<WUInt64> result)
        {
            NodeResultGeoItems geoItemsResult = (NodeResultGeoItems)result;
            float? closestPossible = ClosestPossiblePending;
            //if (geoItemsResult.NodeMortonRange.ApproximateMilesToPoint(InitialGeoQuery.QueryLatLonCenter) < closestPossible)
            //    throw new Exception("Internal error. Unexpected close result set.");
            RemovePendingQuery(geoItemsResult.NodeMortonRange.StartValue);
            foreach (var geoResult in geoItemsResult.Values)
            {
                //if (geoResult.Location.ApproximateMilesTo(InitialGeoQuery.QueryLatLonCenter) < closestPossible)
                //    throw new Exception("Internal error. Unexpected close result set."); 
                lock (LeafResults)
                    LeafResults.Add(geoResult);
                if (CachedReadyResultsCount != null && geoResult.Distance < closestPossible)
                    CachedReadyResultsCount++;
            }
        }

        public void RemovePendingQuery(ulong startOfRange)
        {
            // note that there can be only 1 pending query starting at a particular point, since that query will be replaced by others once it is returned.
            NodeQueryGeo completedQuery = null;
            foreach (var p in PendingQueries.AsEnumerable())
            {
                if (p.NodeMortonRange.StartValue == startOfRange)
                {
                    completedQuery = p;
                    break;
                }
            }
            if (completedQuery == null)
                throw new Exception("Tried to remove non-pending query.");
            if (completedQuery.ClosestPossibleResult == ClosestPossiblePending)
                CachedReadyResultsCount = null;
            lock (PendingQueries)
                PendingQueries.Remove(completedQuery);
        }

        public override uint CumulativeAvailableResultCount()
        {
            if (CachedReadyResultsCount == null)
                CachedReadyResultsCount = (uint?)(GetAvailableResultsWithoutTaking().Count());
            return (uint)CachedReadyResultsCount + NumItemsAlreadyReturned;
        }

        public IEnumerable<object> GetAvailableResultsWithoutTaking()
        {
            float? closestPending = ClosestPossiblePending;
            foreach (var geoResult in LeafResults.Where(x => closestPending == null || x.Distance < closestPending).ToList())
                yield return geoResult;
        }

        public override IEnumerable<object> TakeResultsAvailable(int? maxToTakeNow)
        {
            var availableResults = GetAvailableResultsWithoutTaking();
            if (maxToTakeNow != null)
                availableResults = availableResults.Take((int)maxToTakeNow);
            foreach (var geoResult in availableResults)
            {
                if (CachedReadyResultsCount != null)
                    CachedReadyResultsCount--;
                NumItemsAlreadyReturned++;
                yield return geoResult;
                lock (LeafResults)
                    LeafResults.Remove((GeoResult) geoResult);
            }
        }

        public override UintSet GetResultsAsUintSet()
        {
            UintSet u = new UintSet();
            u.AddUints(LeafResults.AsEnumerable().Select(x => (WUInt32)x.KeyAndID.ID));
            return u;
        }

        public override void UpdateExpectations(IFurtherQueries<WUInt64> result)
        {
            RemovePendingQuery(((NodeResultGeoFurtherQueries)result).NodeMortonRange.StartValue);
            float? closestPending = ClosestPossiblePending;
            foreach (var geoQuery in result.FurtherQueries.Select(x => x as NodeQueryGeo))
            {
                if (geoQuery.ClosestPossibleResult < closestPending)
                    CachedReadyResultsCount = null;
                lock (PendingQueries)
                    PendingQueries.Add(geoQuery);
            }
        }
    }
}
