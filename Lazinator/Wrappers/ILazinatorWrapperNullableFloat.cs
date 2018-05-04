using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableFloat, -1)]
    public interface ILazinatorWrapperNullableFloat : ILazinatorWrapper<float?>
    {
    }
}