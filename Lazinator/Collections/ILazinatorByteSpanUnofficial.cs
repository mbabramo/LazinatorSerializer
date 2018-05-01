using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ByteSpanUnofficial)]
    interface ILazinatorByteSpanUnofficial
    {
        Memory<byte> ReadOrWrite { get; set; }
        ReadOnlySpan<byte> ReadOnly { get; set; }
    }
}
