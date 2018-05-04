using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperDateTime, -1)]
    public interface ILazinatorWrapperDateTime : ILazinatorWrapper<DateTime>
    {
    }
}