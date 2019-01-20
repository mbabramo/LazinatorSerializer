using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IContainerLocation)]
    public interface IContainerLocation : ILazinator
    {
        IContainerLocation GetNextLocation();
        IContainerLocation GetPreviousLocation();
    }
}
