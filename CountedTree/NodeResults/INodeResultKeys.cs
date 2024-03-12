using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.NodeResults
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeResultKeys)]
    public interface INodeResultKeys<TKey> : INodeResultLinearBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        TKey[] Keys { get; set; }
    }
}