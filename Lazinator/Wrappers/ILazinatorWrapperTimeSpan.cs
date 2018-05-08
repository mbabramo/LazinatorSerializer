using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperTimeSpan, -1)]
    interface ILazinatorWrapperTimeSpan : ILazinatorWrapper<TimeSpan>
    {
    }
}