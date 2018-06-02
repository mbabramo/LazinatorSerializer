using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableUlong, -1)]
    interface ILazinatorWrapperNullableUlong : ILazinatorWrapper<ulong?>
    {
    }
}