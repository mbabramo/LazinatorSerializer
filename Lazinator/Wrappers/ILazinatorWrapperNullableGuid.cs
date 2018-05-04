using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableGuid, -1)]
    public interface ILazinatorWrapperNullableGuid : ILazinatorWrapper<Guid?>
    {
    }
}