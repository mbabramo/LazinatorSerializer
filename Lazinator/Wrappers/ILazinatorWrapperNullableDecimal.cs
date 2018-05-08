using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableDecimal, -1)]
    interface ILazinatorWrapperNullableDecimal : ILazinatorWrapper<decimal?>
    {
    }
}