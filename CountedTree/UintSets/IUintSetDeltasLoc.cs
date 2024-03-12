using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;
using Lazinator.Wrappers;

namespace CountedTree.UintSets
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.UintSetDeltasLoc)]
    public interface IUintSetDeltasLoc
    {
        [SetterAccessibility("private")]
        Dictionary<uint, byte> ToAdd { get; }
        [SetterAccessibility("private")]
        HashSet<WUInt32> IndicesToRemove { get; }
    }
}