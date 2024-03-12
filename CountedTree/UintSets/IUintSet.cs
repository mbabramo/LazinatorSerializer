using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;
using Lazinator.Wrappers;
using Lazinator.Collections.BitArray;

namespace CountedTree.UintSets
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.UintSet)]
    public interface IUintSet
    {
        HashSet<WUInt32> HashedItems { get; set; }
        LazinatorBitArray Bits { get; set; }
        int? _BitsCount { get; set; }
    }
}