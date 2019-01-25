using Lazinator.Attributes;
using Lazinator.ContainerLocation;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IAggregatedValueContainer)]
    public interface IAggregatedValueContainer<T> : IIndexableValueContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
        long LongAggregatedCount { get; }
        (long firstAggregatedIndex, long lastAggregatedIndex) GetAggregatedIndexRange(IContainerLocation locationOfInnerContainer);
        (long firstAggregatedIndex, long lastAggregatedIndex) GetAggregatedIndexRange(long aggregatedIndex);
        long GetNonaggregatedIndex(long aggregatedIndex);
        (long nonaggregatedIndex, long firstAggregatedIndex, long lastAggregatedIndex) GetAggregatedIndexInfo(long aggregatedIndex);
        // DEBUG
        //T GetAtAggregatedIndex(long aggregatedIndex);
        //void SetAtAggregatedIndex(long aggregatedIndex, T value);
        //void InsertAtAggregatedIndex(long aggregatedIndex, T value);
        //void RemoveAtAggregated(long aggregatedIndex);
    }
}