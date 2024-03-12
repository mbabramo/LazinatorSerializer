using System.Collections.Generic;

namespace CountedTree.Updating
{
    public partial class NodeDeferralPlan : INodeDeferralPlan
    {
        public NodeDeferralPlan()
        {
            DeferredNodes = new Dictionary<NodeToDelete, long>();
            DeferredUntilThis = new Dictionary<long, List<NodeToDelete>>();
        }

        public void AddDeferredNode(NodeToDeleteLater node)
        {
            ReverseEarlierDeferral(node.NodeToDelete);
            AddNewDeferral(node);
        }

        private void AddNewDeferral(NodeToDeleteLater node)
        {
            DeferredNodes.Add(node.NodeToDelete, node.DeleteWhenThisDeleted);

            List<NodeToDelete> currentList = null;
            if (DeferredUntilThis.ContainsKey(node.DeleteWhenThisDeleted))
                currentList = DeferredUntilThis[node.DeleteWhenThisDeleted];
            else
                currentList = new List<NodeToDelete>();
            currentList.Add(node.NodeToDelete);
            DeferredUntilThis[node.DeleteWhenThisDeleted] = currentList;
        }

        private void ReverseEarlierDeferral(NodeToDelete nodeToDelete)
        {
            if (DeferredNodes.ContainsKey(nodeToDelete))
            {
                long previouslyDeferringUntil = DeferredNodes[nodeToDelete];
                DeferredNodes.Remove(nodeToDelete);
                List<NodeToDelete> listToReduce = DeferredUntilThis[previouslyDeferringUntil];
                listToReduce.Remove(nodeToDelete);
                DeferredUntilThis[previouslyDeferringUntil] = listToReduce;
            }
        }

        public List<NodeToDelete> GetAllNodesToDelete(NodeToDelete nodeBeingDeleted)
        {
            List<NodeToDelete> list = new List<NodeToDelete>();
            // check whether this node's deletion is deferred
            if (!DeferredNodes.ContainsKey(nodeBeingDeleted))
                list.Add(nodeBeingDeleted);
            if (DeferredUntilThis.ContainsKey(nodeBeingDeleted.NodeID))
            {
                // find other nodes deferred until now
                var otherNodesToDelete = DeferredUntilThis[nodeBeingDeleted.NodeID];
                list.AddRange(otherNodesToDelete);
                DeferredUntilThis.Remove(nodeBeingDeleted.NodeID);
                foreach (var nodeToDelete in otherNodesToDelete)
                    DeferredNodes.Remove(nodeToDelete);
            }
            return list;
        }
    }
}
