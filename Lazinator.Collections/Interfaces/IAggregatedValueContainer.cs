using Lazinator.Attributes;
using Lazinator.Collections.Location;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    /// <summary>
    /// A container for items accessible by index, where the number of copies of each item is recorded (so that the item does not need to be repeated).
    /// </summary>
    /// <typeparam name="T">The type of item stored in the container</typeparam>
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IAggregatedValueContainer)]
    public interface IAggregatedValueContainer<T> : IIndexableValueContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
        long LongAggregatedCount { get; }
        (long firstAggregatedIndex, long lastAggregatedIndex) GetAggregatedIndexRange(IContainerLocation locationOfInnerContainer);
        (long firstAggregatedIndex, long lastAggregatedIndex) GetAggregatedIndexRange(long aggregatedIndex);
        long GetNonaggregatedIndex(long aggregatedIndex);
        (long nonaggregatedIndex, long firstAggregatedIndex, long lastAggregatedIndex) GetAggregatedIndexInfo(long aggregatedIndex);

        // TODO
        //T GetAtAggregatedIndex(long aggregatedIndex);
        //void SetAtAggregatedIndex(long aggregatedIndex, T value);
        //void InsertAtAggregatedIndex(long aggregatedIndex, T value);
        //void RemoveAtAggregated(long aggregatedIndex);
    }
}