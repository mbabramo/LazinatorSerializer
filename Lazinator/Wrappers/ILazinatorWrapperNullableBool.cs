using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableBool)]
    public interface ILazinatorWrapperNullableBool : ILazinatorWrapper<bool?>
    {
    }
}