using CountedTree.Queries;
using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;

namespace CountedTree.NodeResults
{
    [NonexclusiveLazinator((int) CountedTreeLazinatorUniqueIDs.FurtherQueries)]
    public interface IFurtherQueries<TKey> : ILazinator where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        List<NodeQueryBase<TKey>> FurtherQueries { get; set; }
        IEnumerable<object> GetResults();
    }
}
