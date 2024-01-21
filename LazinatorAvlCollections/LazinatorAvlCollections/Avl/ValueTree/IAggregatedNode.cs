using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Collections.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    [NonexclusiveLazinator((int)LazinatorAvlCollectionUniqueIDs.IAggregatedNode)]
    public interface IAggregatedNode<T, N> : IIndexableNode<T, N> where T : ILazinator where N : class, ILazinator, new()
    {
        [DoNotAutogenerate]
        long FirstAggregatedIndex { get; set; }
        [DoNotAutogenerate]
        long LastAggregatedIndex { get; }
        [DoNotAutogenerate]
        long LongAggregatedCount { get; }
        [DoNotAutogenerate]
        long LeftAggregatedCount { get; set; }
        [DoNotAutogenerate]
        long SelfAggregatedCount { get; set; }
        [DoNotAutogenerate]
        long RightAggregatedCount { get; set; }

        bool ContainsAggregatedIndex(long aggregatedIndex);
    }
}
