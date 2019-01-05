using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortableFactory)]
    public interface ILazinatorSortableFactory<T>  where T : ILazinator, IComparable<T>
    {
        ILazinatorSortable<T> CreateSortable();
        bool AllowDuplicates { get; }
    }
}
