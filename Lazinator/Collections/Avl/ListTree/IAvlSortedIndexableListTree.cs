using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Avl.ListTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedIndexableListTree)]
    public interface IAvlSortedIndexableListTree<T> where T : ILazinator
    {
    }
}