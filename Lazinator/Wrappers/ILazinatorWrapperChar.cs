using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperChar)]
    public interface ILazinatorWrapperChar : ILazinatorWrapper<char>
    {
    }
}