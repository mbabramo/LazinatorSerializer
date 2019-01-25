using Lazinator.Core;
using Lazinator.Attributes;
using LazinatorCollections;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlTree)]
    public interface IAvlTree<T> where T : ILazinator
    {
        string ToTreeString();
    }
}