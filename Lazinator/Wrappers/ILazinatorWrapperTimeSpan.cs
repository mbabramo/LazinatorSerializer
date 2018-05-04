using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperTimeSpan, -1)]
    public interface ILazinatorWrapperTimeSpan : ILazinatorWrapper<TimeSpan>
    {
    }
}