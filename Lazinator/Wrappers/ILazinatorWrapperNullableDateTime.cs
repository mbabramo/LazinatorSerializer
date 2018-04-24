using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableDateTime)]
    public interface ILazinatorWrapperNullableDateTime : ILazinatorWrapper<DateTime?>
    {
    }
}