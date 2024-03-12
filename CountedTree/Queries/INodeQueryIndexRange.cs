using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.Queries
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeQueryIndexRange)]
    
    public interface INodeQueryIndexRange<TKey> : INodeQueryLinearBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
    }
}