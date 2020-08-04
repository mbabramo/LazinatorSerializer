using System;
using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator interface for a Lazinator wrapper for a read only span of characters. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    [Lazinator((int)LazinatorCoreUniqueIDs.IWReadOnlySpanChar, -1)]
    interface IWReadOnlySpanChar
    {
        ReadOnlySpan<char> Value { get; set; }
    }
}