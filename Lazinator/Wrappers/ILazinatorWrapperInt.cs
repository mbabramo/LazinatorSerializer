using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperInt)]
    public interface ILazinatorWrapperInt : ILazinatorWrapper<int>
    {
    }
}