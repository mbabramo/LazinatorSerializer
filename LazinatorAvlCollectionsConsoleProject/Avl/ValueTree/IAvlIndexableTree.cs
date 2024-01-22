using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlIndexableTree)]
    public interface IAvlIndexableTree<T> where T : ILazinator
    {
    }
}