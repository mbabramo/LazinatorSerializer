using System;
using Lazinator.Attributes;

namespace Lazinator.OffsetList
{
    [AllowNonlazinatorOpenGenerics]
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorFastReadList, -1, false)]
    interface ILazinatorFastReadList<T> where T : struct
    {
        ReadOnlySpan<byte> ReadOnlyBytes { get; set; }
    }
}
