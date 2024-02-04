using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Collections;
using LazinatorAvlCollections.Avl.BinaryTree;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlTreeWithNodeType)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IAvlTreeWithNodeType<T, N> : IBinaryTreeWithNodeType<T, N> where T : ILazinator where N : class, ILazinator, IUpdatableNode<T, N>, new()
    {
    }
}