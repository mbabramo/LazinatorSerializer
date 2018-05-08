using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableByte, -1)]
    interface ILazinatorWrapperNullableByte : ILazinatorWrapper<byte?>
    {
    }
}