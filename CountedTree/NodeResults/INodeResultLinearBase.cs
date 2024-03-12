using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.Node;

namespace CountedTree.NodeResults
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeResultLinearBase)]
    public interface INodeResultLinearBase<TKey> : INodeResultBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        IncludedIndices IncludedIndices { get; set; }
    }
}