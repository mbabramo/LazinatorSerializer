using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperSByte, -1)]
    interface ILazinatorWrapperSByte : ILazinatorWrapper<sbyte>
    {
    }
}