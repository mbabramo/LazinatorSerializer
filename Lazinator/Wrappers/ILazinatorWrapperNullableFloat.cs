using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableFloat)]
    public interface ILazinatorWrapperNullableFloat : ILazinatorWrapper<float?>
    {
    }
}