using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.WReadOnlySpanChar, -1)]
    interface IWReadOnlySpanChar
    {
        ReadOnlySpan<char> Value { get; set; }
    }
}