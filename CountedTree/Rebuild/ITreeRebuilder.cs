using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;
using CountedTree.Core;
using CountedTree.Node;
using CountedTree.UintSets;

namespace CountedTree.Rebuild
{
    [Lazinator((int) CountedTreeLazinatorUniqueIDs.TreeRebuilder)]
    public interface ITreeRebuilder<TKey> where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
    {
        bool Rebuilding { get; set; }
        bool Complete { get; set; }
        byte DepthOfLeafNodes { get; set; }
        long NextNodeID { get; set; }
        Stack<PlannedRebuildNode<TKey>> RebuildingStack { get; set; }
        IRebuildSource<TKey> RebuildSource { get; set; }
        KeyAndID<TKey>? LastItemAdded { get; set; }
        CountedNode<TKey> ReplacementRoot { get; set; }
        uint NumToDo_SplitRangeEvenly { get; set; }
        bool Rebuilding_SplitRangeEvenly { get; set; }
        uint NumComplete_SplitRangeEvenly { get; set; }
        Stack<UintSetWithLoc> UintStack { get; set; } // for each planned internal node, we add a new empty UintSetWithLoc to the stack. When we visit a leaf node, we add the Uints to EVERY UintSet on the stack, because each one is an ancestor node of the leaf node. We also specify their locations by considering the route to the item.
    }
}