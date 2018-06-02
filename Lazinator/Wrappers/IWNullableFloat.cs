using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [Lazinator((int)LazinatorCollectionUniqueIDs.WNullableFloat, -1)]
    interface IWNullableFloat : IW<float?>
    {
    }
}