using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedListFactory)]
    public interface IAvlSortedListFactory<T> : ILazinatorSortableFactory<T> where T : ILazinator, IComparable<T>
    {
        ISortedIndexableMultivalueContainerFactory<T> SortedIndexableContainerFactory { get; set; }
    }
}