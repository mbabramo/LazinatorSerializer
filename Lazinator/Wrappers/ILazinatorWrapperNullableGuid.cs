using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableGuid, -1)]
    interface ILazinatorWrapperNullableGuid : ILazinatorWrapper<Guid?>
    {
    }
}