using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [Lazinator((int)LazinatorCollectionUniqueIDs.WUshort, -1)]
    interface IWUshort : IW<ushort>
    {
    }
}