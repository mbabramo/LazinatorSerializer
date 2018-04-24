using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableUint)]
    public interface ILazinatorWrapperNullableUint : ILazinatorWrapper<uint?>
    {
    }
}