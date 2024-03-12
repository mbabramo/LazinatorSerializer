using CountedTree.Core;
using CountedTree.Node;
using CountedTree.UintSets;
using Lazinator.Core;
using System;
using System.Collections.Generic;

namespace CountedTree.Rebuild
{
    /// <summary>
    /// Information about a node to be created during the rebuilding of a tree, including the location of the node in the tree,
    /// the depth within the tree, the depth of leaf nodes, and the child node information.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public partial class PlannedRebuildNode<TKey> : IPlannedRebuildNode<TKey> where TKey : struct, ILazinator,
      IComparable,
      IComparable<TKey>,
      IEquatable<TKey>
    {

        bool IsLeafNode => Depth == DepthOfLeafNodes;

        public PlannedRebuildNode(byte depth, byte depthOfLeafNodes, byte[] routeToHere, int childIndex, uint startIndex, uint endIndex, TreeStructure structure, PlannedRebuildNode<TKey> parent)
        {
            Depth = depth;
            DepthOfLeafNodes = depthOfLeafNodes;
            RouteToHere = routeToHere;
            ChildIndex = childIndex;
            StartIndex = startIndex;
            EndIndex = endIndex;
            TreeStructure = structure;
            Parent = parent;
            if (Parent == null)
            {
                IsFirstOfAllThisLevel = true;
                IsLastOfAllThisLevel = true;
            }
            else
            {
                IsFirstOfAllThisLevel = Parent.IsFirstOfAllThisLevel && childIndex == 0;
                IsLastOfAllThisLevel = Parent.IsLastOfAllThisLevel && childIndex == structure.NumChildrenPerInternalNode - 1;
            }

            if (!IsLeafNode)
            {
                ChildNodeInfos = new List<NodeInfo<TKey>>();
                if (structure.StoreUintSets)
                    UintSet = new UintSet();
            }
        }

        public int NumItemsInNode => (int)(EndIndex - StartIndex + 1);
        public int NumNodesThisLevel()
        {
            int n = 1;
            for (int d = 1; d <= Depth; d++)
                n *= TreeStructure.NumChildrenPerInternalNode;
            return n;
        }

        // Suppose we have 9936 items to be split across 100 children nodes. Then, each requires 99 items, plus 36 require an extra item.
        public int MinNumItemsPerChildNode => NumItemsInNode / TreeStructure.NumChildrenPerInternalNode;
        public int NumberOfChildrenRequiringExtra => NumItemsInNode - TreeStructure.NumChildrenPerInternalNode * MinNumItemsPerChildNode;
        public int NumberOfItemsInChild(int index) => MinNumItemsPerChildNode + (index < NumberOfChildrenRequiringExtra ? 1 : 0);
        public int NumberOfItemsPrecedingChild(int index) => MinNumItemsPerChildNode * (index) + Math.Min(index, NumberOfChildrenRequiringExtra);
        public uint ChildStartRange(int index) => StartIndex + (uint)NumberOfItemsPrecedingChild(index);
        public uint ChildEndRange(int index) => ChildStartRange(index) + (uint)NumberOfItemsInChild(index) - 1;

        public PlannedRebuildNode<TKey> GetPlannedChild(int childIndex)
        {
            int numNodesThisLevel = NumNodesThisLevel();
            byte[] routeToHere = null;
            if (TreeStructure.StoreUintSetLocs)
            {
                routeToHere = new byte[RouteToHere.Length + 1];
                RouteToHere.CopyTo(routeToHere, 0);
                routeToHere[RouteToHere.Length] = (byte)childIndex;
            }
            return new PlannedRebuildNode<TKey>((byte)(Depth + 1), DepthOfLeafNodes, routeToHere, childIndex, ChildStartRange(childIndex), ChildEndRange(childIndex), TreeStructure, this);
        }
    }
}
