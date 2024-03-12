using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;

namespace CountedTree.UintSets
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.UintSetDeltas)]
    public interface IUintSetDeltas
    {
        [SetterAccessibility("private")]
        List<UintSetDelta> Changes { get; }
    }
}