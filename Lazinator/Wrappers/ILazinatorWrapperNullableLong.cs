using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableLong, -1)]
    interface ILazinatorWrapperNullableLong : ILazinatorWrapper<long?>
    {
    }
}