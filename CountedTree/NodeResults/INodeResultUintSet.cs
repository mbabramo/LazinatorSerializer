using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.UintSets;

namespace CountedTree.NodeResults
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeResultUintSet)]
    public interface INodeResultUintSet<TKey> : INodeResultLinearBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        UintSet UintSet { get; set; }
    }
}