using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.Node;

namespace CountedTree.Queries
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeQueryLinearBase)]
    
    public interface INodeQueryLinearBase<TKey> : INodeQueryBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        bool Ascending { get; set; }
        uint Skip { get; set; }
        IncludedIndices IncludedIndices { get; set; }

    }
}