using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperDateTime)]
    public interface ILazinatorWrapperDateTime : ILazinatorWrapper<DateTime>
    {
    }
}