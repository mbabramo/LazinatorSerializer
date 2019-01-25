using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlAggregatedNode)]
    public interface IAvlAggregatedNode<T> where T : ILazinator
    {
        long LeftAggregatedCount { get; set; }
        long SelfAggregatedCount { get; set; }
        long RightAggregatedCount { get; set; }
    }
}