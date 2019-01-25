using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [SmallLazinator]
    [FixedLengthLazinator(0)]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IPlaceholder, -1)]
    interface IPlaceholder
    {
    }
}