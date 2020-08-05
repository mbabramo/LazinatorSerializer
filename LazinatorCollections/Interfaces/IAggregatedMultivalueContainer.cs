using Lazinator.Attributes;
using Lazinator.Core;

namespace LazinatorCollections.Interfaces
{
    /// <summary>
    /// A Lazinator nonexclusive interface for multivalue containers where each item is contained along with an indication of the number of that item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IAggregatedMultivalueContainer)]
    public interface IAggregatedMultivalueContainer<T> : IAggregatedValueContainer<T>, IIndexableMultivalueContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
    }
}
