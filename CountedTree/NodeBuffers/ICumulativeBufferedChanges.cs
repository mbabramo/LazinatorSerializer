using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.NodeBuffers
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.CumulativeBufferedChanges)]
    public interface ICumulativeBufferedChanges
    {
        [SetterAccessibility("private")]
        int[] CumulativePendingChangesAtIndexAscending { get; }
        [SetterAccessibility("private")]
        int[] CumulativePendingChangesAtIndexDescending { get; }
        [SetterAccessibility("private")]
        int[] CumulativeNetItemChangeAtIndexAscending { get; }
        [SetterAccessibility("private")]
        int[] CumulativeNetItemChangeAtIndexDescending { get; }
        [SetterAccessibility("private")]
        int MaxPendingChanges { get; }
    }
}