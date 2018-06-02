using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperLong, -1)]
    interface ILazinatorWrapperLong : ILazinatorWrapper<long>
    {
    }
}