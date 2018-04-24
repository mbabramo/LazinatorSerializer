using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableGuid)]
    public interface ILazinatorWrapperNullableGuid : ILazinatorWrapper<Guid?>
    {
    }
}