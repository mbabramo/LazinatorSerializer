using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorCollections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorBinaryTree)]
    public interface ILazinatorBinaryTree<T> where T : ILazinator
    {
        T Item { get; set; }
        LazinatorBinaryTree<T> Left { get; set; }
        LazinatorBinaryTree<T> Right { get; set; }
    }
}