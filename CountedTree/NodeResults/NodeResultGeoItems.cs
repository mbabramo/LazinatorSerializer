using System.Collections.Generic;
using System.Linq;
using CountedTree.UintSets;
using Lazinator.Wrappers;
using R8RUtilities;
using Utility;

namespace CountedTree.NodeResults
{
    public partial class NodeResultGeoItems : NodeResultGeoBase, INodeResultGeoItems
    {

        public NodeResultGeoItems(ProperMortonRange nodeMortonRange, uint filteredMatches, uint supersetMatches) : base(nodeMortonRange, filteredMatches, supersetMatches)
        {
        }

        public void SetResults(List<GeoResult> matches)
        {
            Values = matches.ToArray();
            ResultsCount = (uint) Values.Length;
        }

        public override IEnumerable<object> GetResults()
        {
            return Values.AsEnumerable();
        }

        public override UintSet GetUintSet()
        {
            return new UintSet(new HashSet<WUInt32>(Values.Select(x => (WUInt32) x.KeyAndID.ID)));
        }
    }
}
