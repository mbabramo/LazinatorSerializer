using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorCollections.Interfaces
{
    /// <summary>
    /// A Lazinator interface for multivalue containers where the number of each item is recorded.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IAggregatedMultivalueContainer)]
    public interface IAggregatedMultivalueContainer<T> : IAggregatedValueContainer<T>, IIndexableMultivalueContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
    }
}
