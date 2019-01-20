using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Interfaces
{
    public interface IContainerLocation
    {
        IContainerLocation GetNextLocation();
        IContainerLocation GetPreviousLocation();
    }
}
