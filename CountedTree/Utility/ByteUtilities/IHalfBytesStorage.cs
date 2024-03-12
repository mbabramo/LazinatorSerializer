using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.ByteUtilities
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.HalfBytesStorage)]
    public interface IHalfBytesStorage
    {
        [SetterAccessibility("private")]
        bool TwoPerByte { get; }
        [SetterAccessibility("private")]
        bool EvenNumber { get; }
        [SetterAccessibility("private")]
        ReadOnlyMemory<byte> Storage { get; }
    }
}