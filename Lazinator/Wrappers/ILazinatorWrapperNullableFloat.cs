using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableFloat, -1)]
    interface ILazinatorWrapperNullableFloat : ILazinatorWrapper<float?>
    {
    }
}