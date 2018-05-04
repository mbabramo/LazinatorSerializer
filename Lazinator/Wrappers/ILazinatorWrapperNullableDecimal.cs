using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableDecimal, -1)]
    public interface ILazinatorWrapperNullableDecimal : ILazinatorWrapper<decimal?>
    {
    }
}