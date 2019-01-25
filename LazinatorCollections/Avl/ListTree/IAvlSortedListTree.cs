using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorCollections.Avl.ListTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedListTree)]
    public interface IAvlSortedListTree<T> where T : ILazinator
    {
    }
}