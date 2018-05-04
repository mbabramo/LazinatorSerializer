using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperDecimal, -1)]
    public interface ILazinatorWrapperDecimal : ILazinatorWrapper<decimal>
    {
    }
}