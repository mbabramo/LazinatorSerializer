using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using CountedTree.Core;
using Lazinator.Collections;

namespace CountedTree.Rebuild
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.InMemoryRebuildSource)]
    public interface IInMemoryRebuildSource<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        LazinatorList<KeyAndID<TKey>> Items { get; set; }
        int NumProcessed { get; set; }
        KeyAndID<TKey>? FirstExclusive { get; set; }
        KeyAndID<TKey>? LastInclusive { get; set; }
    }
}