using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCollectionUniqueIDs.IWUlong, -1)]
    interface IWUlong : IW<ulong>
    {
    }
}