using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableSByte, -1)]
    public interface ILazinatorWrapperNullableSByte : ILazinatorWrapper<sbyte?>
    {
    }
}