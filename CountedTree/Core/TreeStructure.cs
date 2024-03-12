using System;
using Utility;

namespace CountedTree.Core
{
    public partial class TreeStructure : ITreeStructure
    {
        /// <summary>
        /// Whether each internal node will store a filter of all items in its subtree. These filters allow faster generation of filters.
        /// </summary>
        public bool StoreUintSets => UintSetStorageContext != Guid.Empty;

        public TreeStructure(bool splitRangeEvenly, int numChildrenPerInternalNode, int maxItemsPerLeaf, bool storeUintSets, bool storeUintSetLocs, byte maxTolerableImbalance = 2)
        {
            SplitRangeEvenly = splitRangeEvenly;
            NumChildrenPerInternalNode = numChildrenPerInternalNode;
            MaxItemsPerLeaf = maxItemsPerLeaf;
            MaxTolerableImbalance = maxTolerableImbalance;
            if (storeUintSets)
                UintSetStorageContext = RandomGenerator.GetGuid();
            else
            {
                UintSetStorageContext = default(Guid);
                if (storeUintSetLocs)
                    throw new Exception("Can store location of items in the bit set only if storing the bit set itself.");
            }
            StoreUintSetLocs = storeUintSetLocs;
            if (StoreUintSetLocs && numChildrenPerInternalNode > 256)
                throw new Exception("Children per internal node must be no greater than 256 if storing UintSet locations.");
            if (SplitRangeEvenly)
                MaxTolerableImbalance = byte.MaxValue;
        }
    }
}
