using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.Core;

namespace CountedTree.Queries
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeQueryValueRange)]
    public interface INodeQueryValueRange<TKey> : INodeQueryLinearBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        KeyAndID<TKey>? StartingValue { get; set; }
        KeyAndID<TKey>? EndingValue { get; set; }

    }
}