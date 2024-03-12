using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using CountedTree.Core;
using CountedTree.Queries;

namespace CountedTree.Node
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.CountedLeafNodeMatcher)]
    public interface ICountedLeafNodeMatcher<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        bool Done { get; set; }
        List<RankKeyAndID<TKey>> Matches { get; set; }
        uint FilteredMatches { get; set; }
        uint SupersetMatches { get; set; }
        int IndexNum { get; set; }
        int SkipsProcessed { get; set; }
        uint RankInSuperset { get; set; }
        NodeQueryLinearBase<TKey> Request { get; set; }
    }
}