using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IWReadOnlySpanChar, -1)]
    interface IWReadOnlySpanChar
    {
        ReadOnlySpan<char> Value { get; set; }
    }
}