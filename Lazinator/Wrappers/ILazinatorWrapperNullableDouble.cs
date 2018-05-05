using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperNullableDouble, -1)]
    public interface ILazinatorWrapperNullableDouble : ILazinatorWrapper<double?>
    {
    }
}