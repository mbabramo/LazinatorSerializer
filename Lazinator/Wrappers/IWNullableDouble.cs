using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.WNullableDouble, -1)]
    interface IWNullableDouble : IW<double?>
    {
    }
}