using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for an array of decimals. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [Lazinator((int)LazinatorCoreUniqueIDs.IWDecimalArray, -1)]
    interface IWDecimalArray : IW<decimal[]>
    {
    }
}