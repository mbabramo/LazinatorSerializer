using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree;

namespace Utility
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.ProperMortonRange)]
    public interface IProperMortonRange
    {
        [SetterAccessibility("private")]
        ulong StartValue { get; }
        [SetterAccessibility("private")]
        byte Depth { get; }
    }
}