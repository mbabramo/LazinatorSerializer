using Utility;
using Lazinator.Core;
using System;
using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace CountedTree.Queries
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeQueryGeo)]
    public interface INodeQueryGeo : INodeQueryBase<WUInt64>
    {
        ulong QueryMortonCenter { get; set; }
        float QueryMaxDistanceFromCenter { get; set; }
        LatLonPoint QueryLatLonCenter { get; set; }
        ProperMortonRange NodeMortonRange { get; set; }
        float ClosestPossibleResult { get; set; }

    }
}