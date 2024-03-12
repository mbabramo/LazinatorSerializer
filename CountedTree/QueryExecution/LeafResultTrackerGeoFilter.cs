using System;
using System.Collections.Generic;
using CountedTree.NodeResults;
using CountedTree.UintSets;
using Lazinator.Wrappers;

namespace CountedTree.QueryExecution
{
    public class LeafResultTrackerGeoFilter : LeafResultTrackerBase<WUInt64>
    {
        UintSet GeoFilter;

        public LeafResultTrackerGeoFilter()
        {
            GeoFilter = new UintSet();
        }

        public override void AddLeafResult(NodeResultBase<WUInt64> result)
        {
            GeoFilter = GeoFilter.Union(result.GetUintSet());
        }

        public override uint CumulativeAvailableResultCount()
        {
            return 0; // this will result in completion when there are no more pending queries
        }

        public override UintSet GetResultsAsUintSet()
        {
            return GeoFilter;
        }

        public override IEnumerable<object> TakeResultsAvailable(int? maxToTakeNow)
        {
            throw new NotImplementedException();
        }

        public override void UpdateExpectations(IFurtherQueries<WUInt64> result)
        {
        }
    }
}
