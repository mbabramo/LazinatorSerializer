using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace CountedTree.Node
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.CumulativeItemsCounter)]
    public interface ICumulativeItemsCounter<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {

        /// <summary>
        /// The cumulative number of items in the subtrees through this index (not including pending changes)
        /// </summary>
        uint[] CumulativeItemsInSubtreeAscending { get; set; }

        /// <summary>
        /// The cumulative number of items in the subtrees, when starting at the last subtree, through this index.
        /// </summary>
        uint[] CumulativeItemsInSubtreeDescending { get; set; }
    }
}