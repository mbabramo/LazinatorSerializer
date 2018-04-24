using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableLong)]
    public interface ILazinatorWrapperNullableLong : ILazinatorWrapper<long?>
    {
    }
}