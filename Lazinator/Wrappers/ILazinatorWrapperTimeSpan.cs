using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperTimeSpan)]
    public interface ILazinatorWrapperTimeSpan : ILazinatorWrapper<TimeSpan>
    {
    }
}