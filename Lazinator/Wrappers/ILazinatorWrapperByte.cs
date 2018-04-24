using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperByte)]
    public interface ILazinatorWrapperByte : ILazinatorWrapper<byte>
    {
    }
}