using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.UintSets
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.UintSetDeltasWithLoc)]
    public interface IUintSetDeltasWithLoc
    {
        UintSetDeltas Items { get; set; }
        UintSetDeltasLoc Locations { get; set; }
    }
}