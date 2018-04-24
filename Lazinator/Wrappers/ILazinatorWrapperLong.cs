using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperLong)]
    public interface ILazinatorWrapperLong : ILazinatorWrapper<long>
    {
    }
}