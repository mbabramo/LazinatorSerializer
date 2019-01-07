using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Factories
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedIndexableContainerFactory)]
    public interface ISortedIndexableContainerFactory<T> : ILazinator where T : ILazinator, IComparable<T>
    {
        ISortedIndexableContainer<T> CreateSortedIndexableContainer();
    }
}
