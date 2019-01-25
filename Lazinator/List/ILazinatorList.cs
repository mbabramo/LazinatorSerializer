using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.OffsetList;
using System;

namespace Lazinator.List
{
    // include the non-generic properties in a separate non-Lazinator interface
    interface ILazinatorList
    {
        int Count { get; }
        void Clear();
        void RemoveAt(int index);
    }

    [Lazinator((int)LazinatorCoreUniqueIDs.ILazinatorList)]
    [UnofficiallyIncorporateInterface("Lazinator.List.ILazinatorListUnofficial", "protected")]
    public interface ILazinatorList<T> where T : ILazinator
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
    }

    [Lazinator((int)LazinatorCoreUniqueIDs.ILazinatorListUnofficial)]
    public interface ILazinatorListUnofficial
    {
        [PlaceholderMemory("WriteMainList")]
        ReadOnlyMemory<byte> MainListSerialized { get; set; }
        LazinatorOffsetList Offsets { get; set; }
    }
}