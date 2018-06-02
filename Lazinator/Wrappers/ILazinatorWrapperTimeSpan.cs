using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperTimeSpan, -1)]
    interface ILazinatorWrapperTimeSpan : ILazinatorWrapper<TimeSpan>
    {
    }
}