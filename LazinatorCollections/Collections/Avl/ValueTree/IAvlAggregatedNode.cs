using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlAggregatedNode)]
    public interface IAvlAggregatedNode<T> where T : ILazinator
    {
        long LeftAggregatedCount { get; set; }
        long SelfAggregatedCount { get; set; }
        long RightAggregatedCount { get; set; }
    }
}