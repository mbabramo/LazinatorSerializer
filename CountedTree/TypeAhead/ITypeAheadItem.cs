using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.TypeAhead
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.TypeAheadItem)]
    public interface ITypeAheadItem<R> where R : ILazinator, IComparable, IComparable<R>, IEquatable<R>
    {
        R SearchResult { get; set; }
        float Popularity { get; set; }
    }
}