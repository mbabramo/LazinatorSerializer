using CountedTree.Queries;
using CountedTree.UintSets;
using Lazinator.Wrappers;
using R8RUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace CountedTree.NodeResults
{
    public partial class NodeResultGeoFurtherQueries : NodeResultGeoBase, IFurtherQueries<WUInt64>, INodeResultGeoFurtherQueries 
    {

        public NodeResultGeoFurtherQueries(ProperMortonRange nodeMortonRange, List<NodeQueryBase<WUInt64>> furtherQueries, uint filteredMatches, uint supersetMatches) : base(nodeMortonRange, filteredMatches, supersetMatches)
        {
            FurtherQueries = furtherQueries;
        }

        public override IEnumerable<object> GetResults()
        {
            return FurtherQueries.AsEnumerable();
        }

        public override UintSet GetUintSet()
        {
            throw new NotImplementedException();
        }
    }
}
