using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.PendingChanges
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.PendingChangesAtTime)]
    public interface IPendingChangesAtTime<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        [SetterAccessibility("private")]
        DateTime SubmissionTime { get; }
        [SetterAccessibility("private")]
        PendingChangesCollection<TKey> PendingChanges { get; }
    }
}