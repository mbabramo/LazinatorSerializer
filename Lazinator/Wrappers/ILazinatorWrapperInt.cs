using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperInt, -1)]
    public interface ILazinatorWrapperInt : ILazinatorWrapper<int>
    {
    }
}