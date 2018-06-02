using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperBool, -1)]
    interface ILazinatorWrapperBool : ILazinatorWrapper<bool>
    {
    }
}