using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Collections.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    [NonexclusiveLazinator((int)LazinatorAvlCollectionUniqueIDs.IIndexableNode)]
    public interface IIndexableNode<T, N> : IUpdatableNode<T, N> where T : ILazinator where N : class, ILazinator, new()
    {
        [DoNotAutogenerate]
        long Index { get; set; }
        [DoNotAutogenerate]
        long LongCount { get; }
        [DoNotAutogenerate]
        long LeftCount { get; set; }
        [DoNotAutogenerate]
        long SelfCount { get; }
        [DoNotAutogenerate]
        long RightCount { get; set; }
    }
}
