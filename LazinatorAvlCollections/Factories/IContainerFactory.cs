using Lazinator.Core;
using Lazinator.Attributes;
using System;
using LazinatorCollections.Interfaces;

namespace LazinatorAvlCollections.Factories
{
    [Lazinator((int)LazinatorAvlCollectionUniqueIDs.IContainerFactory)]
    public interface IContainerFactory 
    {
        ContainerLevel ThisLevel { get; set; }
        ContainerFactory InnerFactory { get; set; }
    }
}