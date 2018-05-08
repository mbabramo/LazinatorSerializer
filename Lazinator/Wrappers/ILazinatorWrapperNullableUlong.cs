using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableUlong, -1)]
    interface ILazinatorWrapperNullableUlong : ILazinatorWrapper<ulong?>
    {
    }
}