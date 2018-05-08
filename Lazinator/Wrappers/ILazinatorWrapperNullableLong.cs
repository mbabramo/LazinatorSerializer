using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableLong, -1)]
    interface ILazinatorWrapperNullableLong : ILazinatorWrapper<long?>
    {
    }
}