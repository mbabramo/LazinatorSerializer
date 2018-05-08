using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableBool, -1)]
    interface ILazinatorWrapperNullableBool : ILazinatorWrapper<bool?>
    {
    }
}