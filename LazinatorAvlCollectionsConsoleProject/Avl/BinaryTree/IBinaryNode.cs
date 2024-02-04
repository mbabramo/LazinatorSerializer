using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IBinaryNode)]
    [SingleParent]
    [AsyncLazinatorMemory]
    public interface IBinaryNode<T> where T : ILazinator
    {
        T Value { get; set; }
        BinaryNode<T> Left { get; set; }
        BinaryNode<T> Right { get; set; }
        [DoNotAutogenerate]
        BinaryNode<T> Parent { get; set; }
    }
}