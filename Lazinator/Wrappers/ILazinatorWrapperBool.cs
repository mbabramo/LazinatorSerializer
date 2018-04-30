using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperBool)]
    public interface ILazinatorWrapperBool : ILazinatorWrapper<bool>
    {
    }
}