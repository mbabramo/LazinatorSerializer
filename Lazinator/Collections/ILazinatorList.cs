using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Support;
using Lazinator.Core;
using System;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorList)]
    interface ILazinatorList<T> where T : ILazinator
    {
        ReadOnlyMemory<byte> MainListSerialized { get; set; }
        LazinatorOffsetList Offsets { get; set; }
    }
}