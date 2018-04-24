using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorNullableTimeSpan)]
    public interface ILazinatorWrapperNullableTimeSpan : ILazinatorWrapper<TimeSpan?>
    {
    }
}