using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Support;
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
    public interface ILazinatorList<T> where T : ILazinator
    {
        [CustomNonlazinatorWrite("WriteMainList")]
        [DoNotEnumerate]
        Memory<byte> MainListSerialized { get; set; }
        bool MainListSerialized_Dirty { get; set; }
        LazinatorOffsetList Offsets { get; set; }
    }
}