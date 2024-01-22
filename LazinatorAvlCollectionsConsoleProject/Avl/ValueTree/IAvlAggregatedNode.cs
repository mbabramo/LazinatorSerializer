using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlAggregatedNode)]
    public interface IAvlAggregatedNode<T> where T : ILazinator, ICountableContainer
    {
        long LeftAggregatedCount { get; set; }
        long SelfAggregatedCount { get; set; }
        long RightAggregatedCount { get; set; }
        T Value { get; set; }
        long LeftCount { get; set; }
        long RightCount { get; set; }
        [OnPropertyAccessed("OnChildNodeAccessed(_Left, true);")]
        AvlAggregatedNode<T> Left { get; set; }
        [OnPropertyAccessed("OnChildNodeAccessed(_Right, false);")]
        AvlAggregatedNode<T> Right { get; set; }
        [DoNotAutogenerate]
        AvlAggregatedNode<T> Parent { get; set; }

        int Balance { get; set; }

    }
}