using CountedTree.Core;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountedTree.Updating
{
    public partial class NodeDeletionManager<TKey> : INodeDeletionManager<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public NodeDeletionManager(bool storeUintSets, TimeSpan minimumRetentionTime)
        {
            DeletionPlans = new Queue<DeletionPlan>(); DeferralPlan = new NodeDeferralPlan();
            StoreUintSets = storeUintSets;
            MinimumRetentionTime = minimumRetentionTime;
        }
        

        public void AddDeletionPlan(DeletionPlan deletionPlan)
        {
            DeletionPlans.Enqueue(deletionPlan);
            if (deletionPlan is NodesDeletionPlan)
            {
                NodesDeletionPlan ndp = (NodesDeletionPlan)deletionPlan;
                //if (ndp.NodesToDelete.Any(x => x.NodeID == 123))
                //    Debug.WriteLine("Plan to delete " + String.Join(",", ndp) + " at " + ndp.DeletionTime);
            }
        }

        public void AddDeferredDeletion(NodeToDeleteLater deferredItem)
        {
            DeferralPlan.AddDeferredNode(deferredItem);
            //if (deferredItem.NodeToDelete.NodeID == 123 || deferredItem.DeleteWhenThisDeleted == 123)
            //    Debug.WriteLine($"{deferredItem}");
        }

        public int GetDeletionWorkNeeded()
        {
            if (DeletionStack != null)
                return int.MaxValue; // must finish with this deletion before doing anything else
            int deletionOfOldNodes = 0;
            if (DeletionPlans.Any())
            {
                var oldest = DeletionPlans.Peek();
                var shouldDeleteBefore = StorageFactory.GetDateTimeProvider().Now - MinimumRetentionTime;
                if (oldest.DeletionTime < shouldDeleteBefore)
                    deletionOfOldNodes = (int)(shouldDeleteBefore - oldest.DeletionTime).TotalMinutes;
            }
            return deletionOfOldNodes;
        }

        public async Task<bool> DoDeletionWork(Guid treeID, bool treeIsDeletedGoal, CatchupPendingChangesTracker<TKey> catchupTracker)
        {
            if (DeletionStack != null)
                return await ContinueFullTreeDeletion(treeID); // don't delete anything else until done with this
            // if not already in full-tree deletion mode, look at the deletion plans
            return await ProcessDeletionPlans(treeID, treeIsDeletedGoal, catchupTracker);
        }

        public async Task<bool> ProcessDeletionPlans(Guid treeID, bool treeIsDeletedGoal, CatchupPendingChangesTracker<TKey> catchupTracker)
        {
            bool complete = false;
            const int approxMaxNodesToDeleteAtOnce = 100; // once we exceed this, we won't do any more.
            int numNodesDeletedThisRound = 0;
            bool doMore = true;
            while (doMore && DeletionPlans.Any() && (treeIsDeletedGoal || DeletionPlans.Peek().DeletionTime + MinimumRetentionTime < StorageFactory.GetDateTimeProvider().Now))
            {
                var plan = DeletionPlans.Dequeue();
                if (plan is TreeDeletionPlan)
                {
                    complete = await InitiateFullTreeDeletion(treeID, ((TreeDeletionPlan)plan).RootID, catchupTracker);
                    doMore = false; // we'll wait until next round of work to do something other than the tree, even if it's done
                }
                else
                {
                    await DeleteSpecificNodesInPlan(treeID, (NodesDeletionPlan)plan);
                    numNodesDeletedThisRound += ((NodesDeletionPlan)plan).NodesToDelete.Count();
                    doMore = numNodesDeletedThisRound < approxMaxNodesToDeleteAtOnce;
                }
            }
            return complete;
        }

        public async Task<bool> InitiateFullTreeDeletion(Guid treeID, long rootID, CatchupPendingChangesTracker<TKey> catchupTracker)
        {
            await StorageFactory.GetPendingChangesStorage().RemoveAllPendingChangesForNode<TKey>(treeID, rootID); // delete any pending changes storage associated with this root node
            await catchupTracker.DeleteNoLongerNeededCatchupBufferedPendingChanges(treeID, true);
            DeletionStack = new Stack<NodeDeletionStatus>();
            List<long> children = await GetChildrenOfNode(treeID, rootID);
            DeletionStack.Push(new NodeDeletionStatus(rootID, 0, children));
            return await ContinueFullTreeDeletion(treeID);
        }

        private async Task<bool> ContinueFullTreeDeletion(Guid treeID)
        {
            const int maxNodesAtOnce = 20;
            int numNodesDeleted = 0;
            while (DeletionStack.Any() && numNodesDeleted < maxNodesAtOnce)
            {
                NodeDeletionStatus top = DeletionStack.Peek();
                if (top.NumChildrenVisited < top.Children.Count())
                {
                    long child = top.Children[top.NumChildrenVisited];
                    DeletionStack.Push(new NodeDeletionStatus(child, 0, await GetChildrenOfNode(treeID, child)));
                    top.NumChildrenVisited++;
                }
                else
                {
                    DeletionStack.Pop();
                    bool isRoot = !DeletionStack.Any();
                    await DeleteSpecificNodes(treeID, new List<NodeToDelete>() { new NodeToDelete(top.NodeID, isRoot) });
                    numNodesDeleted++;
                }
            }
            if (!DeletionStack.Any())
            {
                DeletionStack = null;
                return true;
            }
            return false;
        }

        private async Task<List<long>> GetChildrenOfNode(Guid treeID, long nodeID)
        {
            var node = await StorageFactory.GetNodeStorage().GetNode<TKey>(treeID, nodeID);
            return node.GetChildrenIDs();
        }

        private async Task DeleteSpecificNodesInPlan(Guid treeID, NodesDeletionPlan plan)
        {
            var list = plan.NodesToDelete ?? new List<NodeToDelete>();
            await DeleteSpecificNodes(treeID, list);
        }

        private async Task DeleteSpecificNodes(Guid treeID, List<NodeToDelete> list)
        {
            List<Task> taskList = new List<Task>();
            foreach (var nodeToDelete in list)
            {
                var allNodesToDelete = DeferralPlan.GetAllNodesToDelete(nodeToDelete);
                foreach (var node in allNodesToDelete)
                    AddDeletionTasks(treeID, taskList, node);
            }
            await Task.WhenAll(taskList.ToArray());
        }

        private void AddDeletionTasks(Guid treeID, List<Task> taskList, NodeToDelete nodeToDelete)
        {
            if (nodeToDelete.IsRoot)
            {
                taskList.Add(StorageFactory.GetPendingChangesStorage().RemoveAllPendingChangesForNode<TKey>(treeID, nodeToDelete.NodeID));
            }
            taskList.Add(DeleteNode(treeID, nodeToDelete.NodeID));
        }

        /// <summary>
        /// Deletes the node, potentially along with the UintSet associated with it.
        /// </summary>
        /// <param name="nodeToDelete">The nodeID to delete</param>
        /// <param name="deleteInheritedOnly">If TRUE, then we delete the inherited UintSet -- and only if the UintSet is no longer being used. If FALSE, then we also delete the node's own UintSet.</param>
        /// <returns></returns>
        private async Task DeleteNode(Guid treeID, long nodeToDelete)
        {
            await StorageFactory.GetNodeStorage().DeleteNode<TKey>(treeID, nodeToDelete, StoreUintSets ? StorageFactory.GetUintSetStorage() : null);
        }
    }
}
