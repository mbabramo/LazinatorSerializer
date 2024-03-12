using Lazinator.Core;
using Lazinator.Attributes;
using System;
using CountedTree.Core;

namespace CountedTree.Node
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.NodeInfo)]
    public interface INodeInfo<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// The ID of this node.
        /// </summary>
        long NodeID { get; set; }
        /// <summary>
        /// The depth of this node. The top has depth 0.
        /// </summary>
        byte Depth { get; set; }
        /// <summary>
        /// The maximum depth of this subtree. The top has depth 0.
        /// </summary>
        byte MaxDepth { get; set; }
        /// <summary>
        ///  The number of values in this node and its subtree
        /// </summary>
        uint NumSubtreeValues { get; set; }
        /// <summary>
        /// The number of active nodes in this subtree, including this node.
        /// </summary>
        int NumNodes { get; set; }
        /// <summary>
        /// The amount of work that this node needs (ignoring work needed in subtrees).
        /// </summary>
        int WorkNeeded { get; set; }
        /// <summary>
        /// The amount of work needed by the node in this subtree (possibly this node) needing the most work.
        /// </summary>
        int MaxWorkNeededInSubtree { get; set; }
        /// <summary>
        /// Every item in this node must be greater than this value.
        /// </summary>
        KeyAndID<TKey>? FirstExclusive { get; set; }
        /// <summary>
        /// Every item in this node must be less than or equal to this value.
        /// </summary>
        KeyAndID<TKey>? LastInclusive { get; set; }
        /// <summary>
        /// This is true once the node is created. In a tree with evenly split ranges, an internal node may point to a leaf node that does not yet exist, because it does not contain items.
        /// </summary>
        bool Created { get; set; }
    }
}