using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperFloat, -1)]
    public interface ILazinatorWrapperFloat : ILazinatorWrapper<float>
    {
    }
}