using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperDateTime, -1)]
    interface ILazinatorWrapperDateTime : ILazinatorWrapper<DateTime>
    {
    }
}