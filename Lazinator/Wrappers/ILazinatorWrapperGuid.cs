using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperGuid, -1)]
    interface ILazinatorWrapperGuid : ILazinatorWrapper<Guid>
    {
    }
}