using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableInt, -1)]
    public interface ILazinatorWrapperNullableInt : ILazinatorWrapper<int?>
    {
    }
}