using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Spans
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ByteSpanUnofficial)]
    interface ILazinatorByteSpanUnofficial
    {
        Memory<byte> ReadOrWrite { get; set; }
        ReadOnlySpan<byte> ReadOnly { get; set; }
    }
}
