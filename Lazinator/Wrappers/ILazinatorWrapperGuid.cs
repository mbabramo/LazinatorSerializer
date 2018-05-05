using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperGuid, -1)]
    public interface ILazinatorWrapperGuid : ILazinatorWrapper<Guid>
    {
    }
}