using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperByte, -1)]
    public interface ILazinatorWrapperByte : ILazinatorWrapper<byte>
    {
    }
}