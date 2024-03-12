using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.ByteUtilities;

namespace CountedTree.UintSets
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.UintSetLoc)]
    public interface IUintSetLoc
    {
        bool NoMoreThan16ChildrenPerNode { get; set; }
        HalfBytesStorage Storage { get; set; }
    }
}