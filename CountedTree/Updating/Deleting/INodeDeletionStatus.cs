using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;

namespace CountedTree.Updating
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeDeletionStatus)]
    
    public interface INodeDeletionStatus
    {
        [SetterAccessibility("private")]
        long NodeID { get; }
        [SetterAccessibility("private")]
        List<long> Children { get; }
        int NumChildrenVisited { get; set; }
    }
}