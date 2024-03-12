using Lazinator.Core;
using System;
using Lazinator.Attributes;

namespace CountedTree.NodeResults
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.NodeResultBase)]
    public interface INodeResultBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        uint ResultsCount { get; set; }
        uint FilteredMatches { get; set; }
        uint SupersetMatches { get; set; }
    }
}