using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IValueContainerFactory)]
    public interface IValueContainerFactory<T> where T : ILazinator
    {
        ValueContainerLevel ThisLevel { get; set; }
        ValueContainerFactory<T> InteriorFactory { get; set; }

        IValueContainer<T> CreateContainer();
        IValueContainer<T> CreateInteriorContainer();
        bool RequiresSplitting(IValueContainer<T> container);
        bool FirstIsShorter(IValueContainer<T> first, IValueContainer<T> second);
    }
}