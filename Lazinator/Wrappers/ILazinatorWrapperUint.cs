using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperUint)]
    public interface ILazinatorWrapperUint : ILazinatorWrapper<uint>
    {
    }
}