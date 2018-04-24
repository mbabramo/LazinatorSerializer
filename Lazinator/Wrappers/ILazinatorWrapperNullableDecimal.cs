using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableDecimal)]
    public interface ILazinatorWrapperNullableDecimal : ILazinatorWrapper<decimal?>
    {
    }
}