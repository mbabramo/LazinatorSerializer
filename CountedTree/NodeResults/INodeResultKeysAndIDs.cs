using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.Core;

namespace CountedTree.NodeResults
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeResultKeysAndIDs)]
    public interface INodeResultKeysAndIDs<TKey> : INodeResultLinearBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        KeyAndID<TKey>[] Values { get; set; }
    }
}