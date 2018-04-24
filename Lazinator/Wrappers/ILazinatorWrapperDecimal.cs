using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperDecimal)]
    public interface ILazinatorWrapperDecimal : ILazinatorWrapper<decimal>
    {
    }
}