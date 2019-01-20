using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IContainerFactory)]
    public interface IContainerFactory<T> : ILazinator where T : ILazinator
    {
        ContainerLevel ThisLevel { get; set; }
        IContainerFactory InnerFactory { get; set; }
    }

    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IContainerFactoryNonGeneric)]
    public interface IContainerFactory : ILazinator
    {

    }
}