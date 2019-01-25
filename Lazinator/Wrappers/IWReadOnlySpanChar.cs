using System;
using Lazinator.Attributes;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IWReadOnlySpanChar, -1)]
    interface IWReadOnlySpanChar
    {
        ReadOnlySpan<char> Value { get; set; }
    }
}