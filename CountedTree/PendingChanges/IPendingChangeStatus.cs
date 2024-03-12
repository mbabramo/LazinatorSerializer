using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;

namespace CountedTree.PendingChanges
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.PendingChangeStatus)]
    public interface IPendingChangeStatus<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        HashSet<TKey> DeletionValues { get; set; } // we can delete multiple values at the same ID. (1) A list of pending changes may make a single deletion to an item that was previously added. (2) An internal node may include a list of changes that includes two deletions, because there has been an intervening addition. 
        TKey? AdditionValue { get; set; }
    }
}