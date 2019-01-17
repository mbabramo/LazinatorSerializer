using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedFactory)]
    public interface ILazinatorSortedFactory<T> : ILazinator, ILazinatorListableFactory<T> where T : ILazinator, IComparable<T>
    {
        ILazinatorSorted<T> CreateSorted();
        bool AllowDuplicates { get; set;  }
    }
}
