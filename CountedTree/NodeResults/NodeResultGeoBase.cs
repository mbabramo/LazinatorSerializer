using Lazinator.Wrappers;
using R8RUtilities;
using Utility;

namespace CountedTree.NodeResults
{
    public abstract partial class NodeResultGeoBase : NodeResultBase<WUInt64>, INodeResultGeoBase
    {

        public NodeResultGeoBase() : base(0,0)
        {
        }

        public NodeResultGeoBase(ProperMortonRange nodeMortonRange, uint filteredMatches, uint supersetMatches) : base(filteredMatches, supersetMatches)
        {
            NodeMortonRange = nodeMortonRange;
        }
    }
}
