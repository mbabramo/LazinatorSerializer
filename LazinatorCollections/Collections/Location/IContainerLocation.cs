using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Location
{
    public interface IContainerLocation
    {
        bool IsBeforeContainer { get; }
        bool IsAfterContainer { get; }
        IContainerLocation GetNextLocation();
        IContainerLocation GetPreviousLocation();
    }
}
