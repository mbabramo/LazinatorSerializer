using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableShort, -1)]
    interface ILazinatorWrapperNullableShort : ILazinatorWrapper<short?>
    {
    }
}