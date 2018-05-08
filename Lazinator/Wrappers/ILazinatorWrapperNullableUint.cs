using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableUint, -1)]
    interface ILazinatorWrapperNullableUint : ILazinatorWrapper<uint?>
    {
    }
}