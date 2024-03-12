using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;

namespace CountedTree.Updating
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodesDeletionPlan)]
    public interface INodesDeletionPlan : IDeletionPlan
    {
        [SetterAccessibility("private")]
        List<NodeToDelete> NodesToDelete { get; }

    }
}