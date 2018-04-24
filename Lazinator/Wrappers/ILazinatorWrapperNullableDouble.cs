using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableDouble)]
    public interface ILazinatorWrapperNullableDouble : ILazinatorWrapper<double?>
    {
    }
}