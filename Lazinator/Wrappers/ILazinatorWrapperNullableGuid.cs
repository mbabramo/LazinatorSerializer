using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableGuid, -1)]
    interface ILazinatorWrapperNullableGuid : ILazinatorWrapper<Guid?>
    {
    }
}