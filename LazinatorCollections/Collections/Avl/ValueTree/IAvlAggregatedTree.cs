using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlAggregatedTree)]
    public interface IAvlAggregatedTree<T> where T : ILazinator, ICountableContainer
    {
    }
}