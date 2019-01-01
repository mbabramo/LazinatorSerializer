using Lazinator.Attributes;
using Lazinator.Collections;
using System;

namespace Lazinator.Spans
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorBitArrayUnofficial)]
    internal interface ILazinatorBitArrayUnofficial
    {
        Memory<int> IntStorage { get; set; }
        int m_length { get; set; }
        int _version { get; set; }
    }
}
