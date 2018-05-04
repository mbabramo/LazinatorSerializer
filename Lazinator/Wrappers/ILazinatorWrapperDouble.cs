using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperDouble, -1)]
    public interface ILazinatorWrapperDouble : ILazinatorWrapper<double>
    {
    }
}