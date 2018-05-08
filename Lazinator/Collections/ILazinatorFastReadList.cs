using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorFastReadList, 0, false)]
    interface ILazinatorFastReadList<T> where T : struct
    {
        ReadOnlySpan<T> ReadOnly { get; set; }
    }
}
