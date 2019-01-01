using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [FixedLengthLazinator(1)]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCollectionUniqueIDs.IWByte, -1)]
    interface IWByte : IW<byte>
    {
    }
}