using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.UintSets
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.UintSetDelta)]
    public interface IUintSetDelta
    {
        [SetterAccessibility("private")]
        uint Index { get; }
        [SetterAccessibility("private")]
        bool Delete { get; }
    }
}