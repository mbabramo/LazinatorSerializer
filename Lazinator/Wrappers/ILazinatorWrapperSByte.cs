using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperSByte, -1)]
    public interface ILazinatorWrapperSByte : ILazinatorWrapper<sbyte>
    {
    }
}