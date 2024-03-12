using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;

namespace CountedTree.Updating
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.NodeDeferralPlan)]
    public interface INodeDeferralPlan
    {
        // A node can be deferred to a set time, represented by the node ID of the node whose deletion we are waiting for. If we need it deferred longer, we can change that node ID.
        // But more than one node can be deferred to any particular time. 
        Dictionary<NodeToDelete, long> DeferredNodes  { get; set; }
        Dictionary<long, List<NodeToDelete>> DeferredUntilThis  { get; set; }

    }
}