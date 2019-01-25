using Lazinator.Attributes;
using System;

namespace LazinatorCollections.BitArray
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorBitArrayUnofficial)]
    internal interface ILazinatorBitArrayUnofficial
    {
        Memory<int> IntStorage { get; set; }
        int m_length { get; set; }
        int _version { get; set; }
    }
}
