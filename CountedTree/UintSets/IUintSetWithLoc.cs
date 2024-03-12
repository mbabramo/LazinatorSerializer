using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.UintSets
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.UintSetWithLoc)]    
    public interface IUintSetWithLoc
    {
        UintSet Set { get; set; }
        UintSetLoc Loc { get; set; }
    }
}