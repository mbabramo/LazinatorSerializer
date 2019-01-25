using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections
{
    [NonexclusiveLazinator((int) LazinatorCollectionUniqueIDs.ICountableContainer)]
    public interface ICountableContainer: ILazinator
    {
        long LongCount { get; }
    }
}
