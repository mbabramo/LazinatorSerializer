using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.NodeResults;
using CountedTree.PendingChanges;

namespace CountedTree.Queries
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeQueryBase)]
    public interface INodeQueryBase<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// The ID of the node to query.
        /// </summary>
        long NodeID { get; set;  }
        /// <summary>
        /// Whether the node has storage connected to it. An empty non-root leaf node does not have storage initially allocated to it, so the query can be handled without loading any saved items.
        /// </summary>
        bool NodeHasStorage { get; set;  }
        /// <summary>
        /// Pending changes to the node that should be incorporated within the query. These are changes to the tree that have been buffered but not yet added to the tree.
        /// </summary>
        PendingChangesCollection<TKey> PendingChanges { get; set; }
        /// <summary>
        /// Indicates which items within the tree should be used to return results and to rank items.
        /// </summary>
        QueryFilter Filter { get; set; }
        /// <summary>
        /// The number of items to return, or null to return all items.
        /// </summary>
        uint? Take { get; set; }
        /// <summary>
        /// The type of query result requested (i.e., ids, ids/values, uintset, etc.)
        /// </summary>
        QueryResultType NodeResultType { get; set; }
    }
}