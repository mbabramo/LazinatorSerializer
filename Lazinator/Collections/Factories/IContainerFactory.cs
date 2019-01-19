using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IContainerFactory)]
    public interface IContainerFactory<T> where T : ILazinator
    {
        ContainerLevel ThisLevel { get; set; }
        ContainerFactory<T> InteriorFactory { get; set; }
    }
}