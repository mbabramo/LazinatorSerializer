using Lazinator.Core;
using Lazinator.Attributes;
using System;
using CountedTree.Core;

namespace CountedTree.Rebuild
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.RebuildFromTreeSource)]
    public interface IRebuildFromTreeSource<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        TreeInfo SourceTree { get; set; }
        DateTime AsOfTime { get; set; }
        bool LoadedRootInfo { get; set; }
        uint NumberItemsToGet { get; set; }
        uint NumberItemsGot { get; set; }
        KeyAndID<TKey>? FirstExclusive { get; set; }
        KeyAndID<TKey>? LastInclusive { get; set; }
    }
}