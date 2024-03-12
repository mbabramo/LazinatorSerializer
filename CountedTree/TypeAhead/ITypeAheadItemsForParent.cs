using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;

namespace CountedTree.TypeAhead
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.TypeAheadItemsForParent)]
    public interface ITypeAheadItemsForParent<R> where R : IComparable, IComparable<R>, IEquatable<R>, ILazinator
    {
        List<TypeAheadItem<R>> CandidatesForParent { get; set; }
        TypeAheadItem<R> MostPopularRejected { get; set; }
        string SearchString { get; set; }
        char[] NextChars { get; set; }
    }
}