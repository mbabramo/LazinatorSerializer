using System;
using Lazinator.Attributes;

namespace Lazinator.Collections.ByteSpan
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IByteSpanUnofficial)]
    interface ILazinatorByteSpanUnofficial
    {
        Memory<byte> ReadOrWrite { get; set; }
        ReadOnlySpan<byte> ReadOnly { get; set; }
    }
}
