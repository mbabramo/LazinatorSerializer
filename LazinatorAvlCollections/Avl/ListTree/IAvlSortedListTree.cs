using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorAvlCollections.Avl.ListTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlSortedListTree)]
    public interface IAvlSortedListTree<T> where T : ILazinator
    {
    }
}