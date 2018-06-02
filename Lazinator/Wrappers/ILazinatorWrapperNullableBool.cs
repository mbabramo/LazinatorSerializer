using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableBool, -1)]
    interface ILazinatorWrapperNullableBool : ILazinatorWrapper<bool?>
    {
    }
}