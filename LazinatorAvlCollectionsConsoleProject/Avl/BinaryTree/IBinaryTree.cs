using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IBinaryTree)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IBinaryTree<T> : IBinaryTreeWithNodeType<T, BinaryNode<T>> where T : ILazinator 
    {
    }
}