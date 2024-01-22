using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Collections;
using LazinatorAvlCollections.Avl.BinaryTree;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlAggregatedTreeWithNodeType)]
    public interface IAvlAggregatedTreeWithNodeType<T, N> : IAvlIndexableTreeWithNodeType<T, N> where T : ILazinator, ICountableContainer where N : class, ILazinator, IAggregatedNode<T, N>, new()
    {
    }
}