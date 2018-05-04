using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperUlong, -1)]
    public interface ILazinatorWrapperUlong : ILazinatorWrapper<ulong>
    {
    }
}