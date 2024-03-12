using CountedTree.Core;
using Lazinator.Core;
using Lazinator.Attributes;
using System;
using System.Collections.Generic;
using CountedTree.UintSets;
using CountedTree.Node;

namespace CountedTree.Rebuild
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.PlannedRebuildNode)]
    public interface IPlannedRebuildNode<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        byte Depth { get; set; }
        byte DepthOfLeafNodes { get; set; }
        byte[] RouteToHere { get; set; } // e.g., {1, 5, 7} would indicate that we took the child with index one, then the 5th child, then the 7th
        int ChildIndex { get; set; }
        uint StartIndex { get; set; }
        uint EndIndex { get; set; }
        TreeStructure TreeStructure { get; set; }
        int NumChildrenCreated { get; set; }
        PlannedRebuildNode<TKey> Parent { get; set; }
        List<NodeInfo<TKey>> ChildNodeInfos { get; set; }
        UintSet UintSet { get; set; }
        bool IsFirstOfAllThisLevel { get; set; }
        bool IsLastOfAllThisLevel { get; set; }
    }
}