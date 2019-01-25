using Lazinator.Attributes;
using LazinatorCollections.OffsetList;
using Lazinator.Core;
using System;

namespace LazinatorCollections
{
    // include the non-generic properties in a separate non-Lazinator interface
    interface ILazinatorList
    {
        int Count { get; }
        void Clear();
        void RemoveAt(int index);
    }

    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorList)]
    [UnofficiallyIncorporateInterface("LazinatorCollections.ILazinatorListUnofficial", "protected")]
    public interface ILazinatorList<T> where T : ILazinator
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
    }

    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorListUnofficial)]
    public interface ILazinatorListUnofficial
    {
        [PlaceholderMemory("WriteMainList")]
        ReadOnlyMemory<byte> MainListSerialized { get; set; }
        LazinatorOffsetList Offsets { get; set; }
    }
}