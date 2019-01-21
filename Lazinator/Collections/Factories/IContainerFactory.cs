using Lazinator.Core;
using Lazinator.Attributes;
using System;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IContainerFactory)]
    public interface IContainerFactory 
    {
        ContainerLevel ThisLevel { get; set; }
        ContainerFactory InnerFactory { get; set; }
    }
}