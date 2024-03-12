using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.Core;

namespace CountedTree.NodeResults
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeResultRanksAndIDs)]
    public interface INodeResultRanksAndIDs<TKey> : INodeResultLinearBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        RankAndID[] Values { get; set; }
    }
}