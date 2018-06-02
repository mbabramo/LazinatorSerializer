using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableByte, -1)]
    interface ILazinatorWrapperNullableByte : ILazinatorWrapper<byte?>
    {
    }
}