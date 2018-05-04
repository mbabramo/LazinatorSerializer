using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableLong, -1)]
    public interface ILazinatorWrapperNullableLong : ILazinatorWrapper<long?>
    {
    }
}