using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableInt, -1)]
    public interface ILazinatorWrapperNullableInt : ILazinatorWrapper<int?>
    {
    }
}