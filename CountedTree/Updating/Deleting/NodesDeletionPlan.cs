using System;
using System.Collections.Generic;

namespace CountedTree.Updating
{
    public partial class NodesDeletionPlan : DeletionPlan, INodesDeletionPlan
    {
        public NodesDeletionPlan(DateTime deletionTime, List<NodeToDelete> itemsToDelete) : base(deletionTime)
        {
            NodesToDelete = itemsToDelete;
        }
    }
}
