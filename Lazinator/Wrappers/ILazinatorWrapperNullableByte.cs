using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableByte)]
    public interface ILazinatorWrapperNullableByte : ILazinatorWrapper<byte?>
    {
    }
}