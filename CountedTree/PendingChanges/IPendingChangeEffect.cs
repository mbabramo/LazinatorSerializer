using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.PendingChanges
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.PendingChangeEffect)]
    public interface IPendingChangeEffect
    {
        [SetterAccessibility("private")]
        uint ID { get; }
        [SetterAccessibility("private")]
        byte ChildIndex { get; }
        [SetterAccessibility("private")]
        bool Delete { get; }
    }
}