using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.NodeResults
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeResultGeoItems)]
    public interface INodeResultGeoItems : INodeResultGeoBase
    {
        GeoResult[] Values { get; set; }
    }
}