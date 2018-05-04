using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperReadOnlySpanChar, -1)]
    public interface ILazinatorWrapperReadOnlySpanChar
    {
        ReadOnlySpan<char> Value { get; set; }
    }
}