using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorFastReadList, -1, false)]
    interface ILazinatorFastReadList 
    {
        ReadOnlySpan<byte> ReadOnlyBytes { get; set; }
    }
}
