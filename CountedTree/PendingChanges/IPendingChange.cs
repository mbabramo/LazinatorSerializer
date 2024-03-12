using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.Core;

namespace CountedTree.PendingChanges
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.PendingChange)]
    public interface IPendingChange<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        KeyAndID<TKey> Item { get; set; }
        PendingChangeOperation Operation { get; set; }
    }
}