using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperUint, -1)]
    public interface ILazinatorWrapperUint : ILazinatorWrapper<uint>
    {
    }
}