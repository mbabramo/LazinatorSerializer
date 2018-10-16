using System;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [AllowNonlazinatorOpenGenerics]
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorFastReadList, -1, false)]
    interface ILazinatorFastReadList<T> where T : struct
    {
        ReadOnlySpan<byte> ReadOnlyBytes { get; set; }
    }
}
