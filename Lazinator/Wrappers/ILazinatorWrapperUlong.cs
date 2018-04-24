using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperUlong)]
    public interface ILazinatorWrapperUlong : ILazinatorWrapper<ulong>
    {
    }
}