using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// An empty Lazinator object that takes the minimum possible amount of space.
    /// </summary>
    [SizeOfLength(1)]
    [FixedLengthLazinator(0)]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IPlaceholder, -1)]
    interface IPlaceholder
    {
    }
}