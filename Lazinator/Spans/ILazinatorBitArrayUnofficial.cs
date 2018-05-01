using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections;


namespace Lazinator.Spans
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorBitArrayUnofficial)]
    internal interface ILazinatorBitArrayUnofficial
    {
        LazinatorByteSpan ByteSpan { get; set; }
        int m_length { get; set; }
        int _version { get; set; }
    }
}
