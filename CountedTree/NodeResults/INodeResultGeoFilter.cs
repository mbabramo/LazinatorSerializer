using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.UintSets;

namespace CountedTree.NodeResults
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeResultGeoFilter)]
    public interface INodeResultGeoFilter : INodeResultGeoBase
    { 
            public UintSet GeoFilter { get; set; }
    }
}