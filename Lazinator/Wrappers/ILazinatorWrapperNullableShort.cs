using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableShort, -1)]
    interface ILazinatorWrapperNullableShort : ILazinatorWrapper<short?>
    {
    }
}