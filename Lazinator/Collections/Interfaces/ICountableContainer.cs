using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int) LazinatorCollectionUniqueIDs.ILazinatorCountable)]
    public interface ICountableContainer: ILazinator
    {
        long LongCount { get; }
    }
}
