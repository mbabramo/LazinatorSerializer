using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;
using CountedTree.Queries;
using Lazinator.Wrappers;

namespace CountedTree.NodeResults
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeResultGeoFurtherQueries)]
    public interface INodeResultGeoFurtherQueries : INodeResultGeoBase
    {
        List<NodeQueryBase<WUInt64>> FurtherQueries { get; set; }
    }
}