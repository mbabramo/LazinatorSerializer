using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableDateTime, -1)]
    interface ILazinatorWrapperNullableDateTime : ILazinatorWrapper<DateTime?>
    {
    }
}