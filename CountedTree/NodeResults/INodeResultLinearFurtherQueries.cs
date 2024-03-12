using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;
using CountedTree.Queries;

namespace CountedTree.NodeResults
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeResultLinearFurtherQueries)]
    public interface INodeResultLinearFurtherQueries<TKey> : INodeResultLinearBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        List<NodeQueryBase<TKey>> FurtherQueries { get; set; }
    }
}