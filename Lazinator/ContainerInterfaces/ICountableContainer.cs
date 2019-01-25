using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.ContainerInterfaces
{
    [NonexclusiveLazinator((int) LazinatorCoreUniqueIDs.ICountableContainer)]
    public interface ICountableContainer: ILazinator
    {
        long LongCount { get; }
    }
}
