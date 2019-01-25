using Lazinator.Core;
using Lazinator.Attributes;
using LazinatorCollections;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IAvlAggregatedTree)]
    public interface IAvlAggregatedTree<T> where T : ILazinator, ICountableContainer
    {
    }
}