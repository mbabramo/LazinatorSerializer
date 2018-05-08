using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableSByte, -1)]
    interface ILazinatorWrapperNullableSByte : ILazinatorWrapper<sbyte?>
    {
    }
}