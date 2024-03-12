using System.Collections.Generic;
using System.Linq;
using CountedTree.UintSets;
using R8RUtilities;
using Utility;

namespace CountedTree.NodeResults
{
    public partial class NodeResultGeoFilter : NodeResultGeoBase, INodeResultGeoFilter
    {

        public NodeResultGeoFilter(ProperMortonRange nodeMortonRange, UintSet filter, uint filteredMatches, uint supersetMatches) : base(nodeMortonRange, filteredMatches, supersetMatches)
        {
            GeoFilter = filter;
        }

        public override IEnumerable<object> GetResults()
        {
            return GeoFilter.AsEnumerable().Select(x => (object) x);
        }

        public override UintSet GetUintSet()
        {
            return GeoFilter;
        }
    }
}
