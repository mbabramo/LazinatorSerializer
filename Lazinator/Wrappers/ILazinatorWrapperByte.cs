using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperByte, -1)]
    public interface ILazinatorWrapperByte : ILazinatorWrapper<byte>
    {
    }
}