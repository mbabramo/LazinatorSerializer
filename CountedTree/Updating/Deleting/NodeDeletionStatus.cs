using System.Collections.Generic;

namespace CountedTree.Updating
{
    public partial class NodeDeletionStatus : INodeDeletionStatus
    {

        public NodeDeletionStatus(long nodeID, int numChildrenVisited, List<long> children)
        {
            NodeID = nodeID;
            NumChildrenVisited = numChildrenVisited;
            Children = children;
        }
    }
}
