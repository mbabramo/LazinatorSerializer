using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorNullableTimeSpan, -1)]
    public interface ILazinatorWrapperNullableTimeSpan : ILazinatorWrapper<TimeSpan?>
    {
    }
}