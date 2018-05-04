using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableDateTime, -1)]
    public interface ILazinatorWrapperNullableDateTime : ILazinatorWrapper<DateTime?>
    {
    }
}