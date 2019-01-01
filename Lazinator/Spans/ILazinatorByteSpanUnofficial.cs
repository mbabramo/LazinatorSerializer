using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Spans
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IByteSpanUnofficial)]
    interface ILazinatorByteSpanUnofficial
    {
        Memory<byte> ReadOrWrite { get; set; }
        ReadOnlySpan<byte> ReadOnly { get; set; }
    }
}
