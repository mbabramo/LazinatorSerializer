using System;
using Lazinator.Attributes;

namespace LazinatorCollections.ByteSpan
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IByteSpanUnofficial)]
    interface ILazinatorByteSpanUnofficial
    {
        Memory<byte> ReadOrWrite { get; set; }
        ReadOnlySpan<byte> ReadOnly { get; set; }
    }
}
