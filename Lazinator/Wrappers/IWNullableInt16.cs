using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a nullable short. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [SizeOfLength(1)]
    [ExcludeLazinatorVersionByte]
    [NonbinaryHash]
    [Lazinator((int)LazinatorCoreUniqueIDs.IWNullableInt16, -1)]
    interface IWNullableInt16 : IW<short?>
    {
    }
}