using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Wrappers
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorWrapperReadOnlySpanChar)]
    public interface ILazinatorWrapperReadOnlySpanChar
    {
        ReadOnlySpan<char> Value { get; set; }
    }
}