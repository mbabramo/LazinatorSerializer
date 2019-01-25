using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.OffsetList;
using System;

namespace LazinatorCollections.ListDerivatives
{
    // include the non-generic properties in a separate non-Lazinator interface
    interface ILazinatorList
    {
        int Count { get; }
        void Clear();
        void RemoveAt(int index);
    }

    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorList)]
    [UnofficiallyIncorporateInterface("LazinatorCollections.ListDerivatives.ILazinatorListUnofficial", "protected")]
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