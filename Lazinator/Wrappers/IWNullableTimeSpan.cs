using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorNullableTimeSpan, -1)]
    interface IWNullableTimeSpan : IW<TimeSpan?>
    {
    }
}