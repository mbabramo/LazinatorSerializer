using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableChar, -1)]
    interface ILazinatorWrapperNullableChar : ILazinatorWrapper<char?>
    {
    }
}