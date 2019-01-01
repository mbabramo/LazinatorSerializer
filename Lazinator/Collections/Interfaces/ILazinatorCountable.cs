using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int) LazinatorCollectionUniqueIDs.LazinatorCountable)]
    public interface ILazinatorCountable: ILazinator
    {
        long LongCount { get; }
    }
}
