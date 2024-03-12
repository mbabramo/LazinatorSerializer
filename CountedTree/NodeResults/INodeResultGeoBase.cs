using Lazinator.Core;
using System;
using Lazinator.Attributes;
using Utility;
using Lazinator.Wrappers;

namespace CountedTree.NodeResults
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeResultGeoBase)]
    public interface INodeResultGeoBase : INodeResultBase<WUInt64>
    {
        ProperMortonRange NodeMortonRange { get; set; }
    }
}