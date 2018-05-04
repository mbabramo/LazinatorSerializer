using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableByte, -1)]
    public interface ILazinatorWrapperNullableByte : ILazinatorWrapper<byte?>
    {
    }
}