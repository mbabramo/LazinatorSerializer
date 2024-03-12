using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;
using CountedTree.Updating;

namespace CountedTree.PendingChanges
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.PendingChangesOverTime)]
    public interface IPendingChangesOverTime<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        long RootID { get; set; }
        Queue<PendingChangesAtTime<TKey>> SubmittedChanges { get; set; }
        DateTime? NextTimeAvailable { get; set; }
        RedundancyAvoider RedundancyAvoider { get; set; }
    }
}