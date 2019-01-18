using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Factories
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IMultivalueContainerFactory)]
    public interface IMultivalueContainerFactory<T> : ILazinator where T : ILazinator
    {
        bool AllowDuplicates { get; set; }
        bool Unbalanced { get; set; }
        IMultivalueContainer<T> CreateMultivalueContainer();
    }
}
