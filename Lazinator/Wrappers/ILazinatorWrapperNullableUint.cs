using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableUint, -1)]
    public interface ILazinatorWrapperNullableUint : ILazinatorWrapper<uint?>
    {
    }
}