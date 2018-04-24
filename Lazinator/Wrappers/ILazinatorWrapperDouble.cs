using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperDouble)]
    public interface ILazinatorWrapperDouble : ILazinatorWrapper<double>
    {
    }
}