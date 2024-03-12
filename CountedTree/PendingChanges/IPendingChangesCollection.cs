using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.PendingChanges
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.PendingChangesCollection)]
    public interface IPendingChangesCollection<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        [SetterAccessibility("private")]
        PendingChange<TKey>[] PendingChanges { get;  }
    }
}