using Lazinator.Core;
using System;
using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace CountedTree.NodeResults
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeResultIDs)]
    public interface INodeResultIDs<TKey> : INodeResultLinearBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        WUInt32[] IDs { get; set; }
    }
}