using System;
using Lazinator.Core;

namespace CountedTree.Updating
{
    public enum DifferentWorkTypes
    {
        AddingPendingChangesToTree,
        FlushingTree,
        DeletionOfOldNodes,
        Rebuilding
    }
}
