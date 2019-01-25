using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.ILazinatorNullableTimeSpan, -1)]
    interface IWNullableTimeSpan : IW<TimeSpan?>
    {
    }
}