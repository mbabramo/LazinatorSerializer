using Lazinator.Core;
using Lazinator.Attributes;
using System;
using LazinatorCollections.Interfaces;

namespace LazinatorCollections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IContainerFactory)]
    public interface IContainerFactory 
    {
        ContainerLevel ThisLevel { get; set; }
        ContainerFactory InnerFactory { get; set; }
    }
}