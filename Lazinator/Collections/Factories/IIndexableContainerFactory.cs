using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Factories
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IIndexableContainerFactory)]
    public interface IIndexableContainerFactory<T> : ILazinator where T : ILazinator
    {
        IIndexableContainer<T> CreateIndexableContainer();
    }
}
