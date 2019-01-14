using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Factories
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ISortedIndexableMultivalueContainerFactory)]
    public interface ISortedIndexableMultivalueContainerFactory<T> : ILazinator where T : ILazinator, IComparable<T>
    {
        bool AllowDuplicates { get; set; }
        bool Unbalanced { get; set; }
        ISortedIndexableMultivalueContainer<T> CreateSortedIndexableMultivalueContainer();
    }
}
