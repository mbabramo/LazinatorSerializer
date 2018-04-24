using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperGuid)]
    public interface ILazinatorWrapperGuid : ILazinatorWrapper<Guid>
    {
    }
}