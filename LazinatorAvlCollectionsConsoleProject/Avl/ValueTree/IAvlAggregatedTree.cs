using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlAggregatedTree)]
    [SingleParent]
    [AsyncLazinatorMemory]
    internal interface IAvlAggregatedTree<T> : IAvlAggregatedTreeWithNodeType<T, AvlAggregatedNode<T>> where T : ILazinator, ICountableContainer
    {
    }
}