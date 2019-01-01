using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCollectionUniqueIDs.IWNullableDouble, -1)]
    interface IWNullableDouble : IW<double?>
    {
    }
}