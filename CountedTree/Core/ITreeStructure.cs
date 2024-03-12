using Lazinator.Attributes;
using System;

namespace CountedTree.Core
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.TreeStructure)]
    public interface ITreeStructure
    {
        /// <summary>
        /// The number of child nodes for each internal node.
        /// </summary>
        [SetterAccessibility("private")]
        public int NumChildrenPerInternalNode { get; }
        /// <summary>
        /// The maximum number of items that ideally should be kept in a leaf node. When the number exceeds this, we note that there is work to do on this leaf.
        /// </summary>
        [SetterAccessibility("private")]
        public int MaxItemsPerLeaf { get; }
        /// <summary>
        /// When a leaf node splits into an internal node and many leafs, this indicates whether ranges should be based on equal subdivisions (which is permissible only if there is a bounded range) or based on the items in the node.
        /// </summary>
        [SetterAccessibility("private")]
        public bool SplitRangeEvenly { get; }
        /// <summary>
        /// The maximum imbalance in a tree (i.e., difference between maximum depth and max depth of a balanced tree) before a rebuild will be automatically triggered.
        /// </summary>
        [SetterAccessibility("private")]
        public byte MaxTolerableImbalance { get; }
        /// <summary>
        /// An ID used to determine where to store bit set filters, if filters are to be stored with each level of the tree.
        /// </summary>
        [SetterAccessibility("private")]
        public Guid UintSetStorageContext { get; }
        /// <summary>
        /// Indicates whether UintSetLocs will be stored representing the child node location of each item in the UintSet. StoreUintSets must be true for this to be true. 
        /// </summary>
        [SetterAccessibility("private")]
        public bool StoreUintSetLocs { get; }
    }
}