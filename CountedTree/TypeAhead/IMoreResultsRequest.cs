using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.TypeAhead
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.MoreResultsRequest)]
    public interface IMoreResultsRequest<R> where R : ILazinator, IComparable, IComparable<R>, IEquatable<R>
    {
        char[] NextChars { get; set; }
        int MaxNumResults { get; set; }
        TypeAheadItem<R> LowestExistingItem { get; set; }
    }
}