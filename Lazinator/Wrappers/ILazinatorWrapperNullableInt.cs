using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableInt)]
    public interface ILazinatorWrapperNullableInt : ILazinatorWrapper<int?>
    {
    }
}