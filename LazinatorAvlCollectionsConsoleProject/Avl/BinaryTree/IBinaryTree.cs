using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IBinaryTree)]
    public interface IBinaryTree<T> : IBinaryTreeWithNodeType<T, BinaryNode<T>> where T : ILazinator 
    {
    }
}