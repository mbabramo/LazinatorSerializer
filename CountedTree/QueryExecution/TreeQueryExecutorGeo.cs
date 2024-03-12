using R8RUtilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CountedTree.NodeResults;
using CountedTree.Core;
using CountedTree.Queries;
using Lazinator.Wrappers;
using Utility;

namespace CountedTree.QueryExecution
{
    public class TreeQueryExecutorGeo : TreeQueryExecutorBase<WUInt64>
    {

        LeafResultTrackerGeoItems CompletedLeafResultsGeo => (LeafResultTrackerGeoItems)CompletedLeafResults;

        ulong MortonCenterPoint;
        float Miles;

        public TreeQueryExecutorGeo(TreeInfo treeInfo, DateTime asOfTime, uint? take, QueryResultType resultType, ulong mortonCenterPoint, float miles, QueryFilter filter) : base(treeInfo, asOfTime, 0, take, resultType, filter)
        {
            MortonCenterPoint = mortonCenterPoint;
            Miles = miles;
        }

        internal async override Task SetInitialQuery()
        {
            var pendingChanges = await GetPendingChanges();
            InitialQuery = new NodeQueryGeo(TreeInfo.CurrentRootID, true, pendingChanges, MortonCenterPoint, Miles, new ProperMortonRange(0, 0), Filter);
        }

        internal override void InitializeCompletedLeafResults()
        {
            bool useGeoFilter = ResultType == QueryResultType.IDsAsBitSet && Take == null; // we don't use GeoFilter for a nearest neighbors query (i.e., a geo query where we're only taking some items), because we need to then order our results by item.
            if (useGeoFilter)
                CompletedLeafResults = new LeafResultTrackerGeoFilter();
            else
            {
                CompletedLeafResults = new LeafResultTrackerGeoItems();
                CompletedLeafResultsGeo.AddInitialQuery((NodeQueryGeo)InitialQuery);
            }
        }

        public async Task<List<GeoResult>> GetGeoResults(bool waitForAll = true)
        {
            if (ResultType != QueryResultType.KeysIDsAndDistance)
                throw new Exception("Inconsistent query result types.");
            if (waitForAll)
                return await GetAllResults<GeoResult>();
            else
                return await GetSomeResults<GeoResult>();
        }

    }
}
