using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableInt, -1)]
    interface ILazinatorWrapperNullableInt : ILazinatorWrapper<int?>
    {
    }
}