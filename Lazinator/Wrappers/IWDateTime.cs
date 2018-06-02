using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.WDateTime, -1)]
    interface IWDateTime : IW<DateTime>
    {
    }
}