using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlAggregatedTree)]
    public interface IAvlAggregatedTree<T> where T : ILazinator, ICountableContainer
    {
    }
}