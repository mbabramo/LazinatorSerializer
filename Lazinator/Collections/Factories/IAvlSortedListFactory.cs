using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedListFactory)]
    public interface IAvlSortedListFactory<T> : ISortedIndexableContainerFactory<T> where T : ILazinator, IComparable<T>
    {
        ISortedIndexableContainerFactory<T> SortedIndexableContainerFactory { get; set; }
        bool AllowDuplicates { get; set; }
    }
}