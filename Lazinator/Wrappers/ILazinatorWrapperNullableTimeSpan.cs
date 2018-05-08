using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorNullableTimeSpan, -1)]
    interface ILazinatorWrapperNullableTimeSpan : ILazinatorWrapper<TimeSpan?>
    {
    }
}