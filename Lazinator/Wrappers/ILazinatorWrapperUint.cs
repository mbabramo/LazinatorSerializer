using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperUint, -1)]
    public interface ILazinatorWrapperUint : ILazinatorWrapper<uint>
    {
    }
}