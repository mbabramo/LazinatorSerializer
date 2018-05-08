using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperByte, -1)]
    interface ILazinatorWrapperByte : ILazinatorWrapper<byte>
    {
    }
}