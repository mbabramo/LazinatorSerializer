using System;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace Lazinator.Spans
{
    [Lazinator((int)LazinatorCoreUniqueIDs.IByteSpanUnofficial)]
    interface ILazinatorByteSpanUnofficial
    {
        Memory<byte> ReadOrWrite { get; set; }
        ReadOnlySpan<byte> ReadOnly { get; set; }
    }
}
