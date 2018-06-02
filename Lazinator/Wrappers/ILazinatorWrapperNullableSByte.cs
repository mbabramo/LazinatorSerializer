using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableSByte, -1)]
    interface ILazinatorWrapperNullableSByte : ILazinatorWrapper<sbyte?>
    {
    }
}