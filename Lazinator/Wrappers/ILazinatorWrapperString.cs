using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperString)]
    public interface ILazinatorWrapperString : ILazinatorWrapper<string>
    {
    }
}