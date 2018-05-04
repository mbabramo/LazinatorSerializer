using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperInt, -1)]
    public interface ILazinatorWrapperInt : ILazinatorWrapper<int>
    {
    }
}