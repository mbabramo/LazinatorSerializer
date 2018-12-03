using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace Lazinator.Collections
{
    // include the non-generic properties in a separate non-Lazinator interface
    interface ILazinatorList
    {
        int Count { get; }
        void Clear();
        void RemoveAt(int index);
    }

    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorList)]
    [UnofficiallyIncorporateInterface("Lazinator.Collections.ILazinatorListUnofficial", "protected")]
    public interface ILazinatorList<T> where T : ILazinator
    {
    }

    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorListUnofficial)]
    public interface ILazinatorListUnofficial
    {
        [PlaceholderMemory("WriteMainList")]
        ReadOnlyMemory<byte> MainListSerialized { get; set; }
        LazinatorOffsetList Offsets { get; set; }
    }
}