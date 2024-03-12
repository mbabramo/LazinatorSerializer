using CountedTree.Core;
using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace CountedTree.Node
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.CountedNode)]
    public interface ICountedNode<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Settings governing how many items/children per node and how to split internal nodes.
        /// </summary>
        public TreeStructure TreeStructure { get; set; }
        /// <summary>
        /// Information on this node, including its ID and aggregating information on its descendants.
        /// </summary>
        public NodeInfo<TKey> NodeInfo { get; set; }
    }
}