using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using CountedTree.Core;

namespace CountedTree.Rebuild
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.CombinedRebuildSource)]
    public interface ICombinedRebuildSource<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        List<IRebuildSource<TKey>> RebuildSources { get; set; }
        Queue<KeyAndID<TKey>>[] NextItemsInSources { get; set; }
        bool[] SourceComplete { get; set; }
        KeyAndID<TKey>?[] LowestItemInEachSubsource { get; set; }
        bool Initialized { get; set; }
        bool AllComplete { get; set; }
        KeyAndID<TKey>? FirstExclusive { get; set; }
        KeyAndID<TKey>? LastInclusive { get; set; }
    }
}