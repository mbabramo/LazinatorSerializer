using System;
using Lazinator.Attributes;

namespace Lazinator.Collections.OffsetList
{
    [AllowNonlazinatorOpenGenerics]
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorFastReadList, -1, true)]
    public interface ILazinatorFastReadList<T> where T : struct
    {
        ReadOnlySpan<byte> ReadOnlyBytes { get; set; }
    }
}
