using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Factories
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IAvlListTreeInteriorCollectionFactory)]
    public interface IAvlListTreeInteriorCollectionFactory<T> : IMultivalueContainerFactory<T>, ILazinator where T : ILazinator
    {
        bool RequiresSplitting(IMultivalueContainer<T> container);
        bool FirstIsShorter(IMultivalueContainer<T> first, IMultivalueContainer<T> second);
    }
}
