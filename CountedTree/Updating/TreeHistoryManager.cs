using CountedTree.PendingChanges;
using CountedTree.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CountedTree.Core;
using CountedTree.Rebuild;
using R8RUtilities;
using System.Diagnostics;
using System.Text;
using Lazinator.Core;

namespace CountedTree.Updating
{
    public partial class TreeHistoryManager<TKey> : ITreeHistoryManager<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        int OverallWorkNeeded => CurrentTreeInfo.WorkNeeded;
        public Task<bool> GetTreeIsDeleted() => Task.FromResult(TreeIsDeleted);

        #region Construction

        public TreeHistoryManager(TreeInfo treeInfo, TreeUpdateSettings updateSettings, TreeStructure treeStructure)
        {
            CurrentTreeInfo = treeInfo;
            UpdateSettings = updateSettings;
            TreeStructure = treeStructure;
            NodeDeletionManager =  new NodeDeletionManager<TKey>(TreeStructure.StoreUintSets, UpdateSettings.MinimumRetentionTime);
            CatchupPendingChangesTracker = new CatchupPendingChangesTracker<TKey>();
            RedundancyAvoider = new RedundancyAvoider();
        }

        public Task<ITreeHistoryManager<TKey>> Clone()
        {
            // this is shallow except for catchup tracker -- we do this so that we can test whether that works correctly
            return Task.FromResult((ITreeHistoryManager<TKey>) new TreeHistoryManager<TKey>(CurrentTreeInfo, UpdateSettings, TreeStructure) { ExpectedNumItemsInTree = ExpectedNumItemsInTree, NetPendingChangesInRequestBuffer = NetPendingChangesInRequestBuffer, NumPendingChangesInRequestBuffer = NumPendingChangesInRequestBuffer, NumPendingChangesSetsAdded = NumPendingChangesSetsAdded, FlushingWorkNeeded = FlushingWorkNeeded, TypeOfWorkMostNeeded = TypeOfWorkMostNeeded, Rebuilder = Rebuilder, TreeIsDeletedGoal = TreeIsDeletedGoal, TreeIsDeleted = TreeIsDeleted, NodeDeletionManager = NodeDeletionManager, CatchupPendingChangesTracker = CatchupPendingChangesTracker.Clone(), RedundancyAvoider = RedundancyAvoider });
        }

        public Task<CountedNode<TKey>> GetCurrentRoot()
        {
            return CurrentTreeInfo.GetRoot<TKey>();
        }

        public async Task SetTreeToNewRoot(CountedNode<TKey> newRoot, bool copyPendingChanges)
        {
            FlushingWorkNeeded = Math.Max(newRoot.NodeInfo.WorkNeeded, newRoot.NodeInfo.MaxWorkNeededInSubtree);
            CurrentTreeInfo.MaxDepth = newRoot.NodeInfo.MaxDepth;
            CurrentTreeInfo.NumValuesInTree = newRoot.NodeInfo.NumSubtreeValues;
            long? formerRootID = CurrentTreeInfo.CurrentRootID;
            if (newRoot.ID == 0)
                formerRootID = null;
            if (formerRootID != newRoot.ID)
            {
                await InitializePendingChangesStorage(formerRootID, newRoot.ID, copyPendingChanges);
                CurrentTreeInfo.CurrentRootID = newRoot.ID;
            }
            // uncomment if having difficulty figuring out why something is deleting prematurely await VerifyTreeNotMarkedForDeletion(newRoot);
        }

        #endregion

        #region Storage

        // Pending changes storage: Before even flushing changes to root, we can store them into a temporary storage location. That way, we can flush them when time is available.

        /// <summary>
        /// When a new tree is created, we must copy the pending changes storage from its previous location.
        /// </summary>
        /// <param name="formerRootID"></param>
        /// <param name="replacementRootID"></param>
        /// <returns></returns>
        private async Task InitializePendingChangesStorage(long? formerRootID, long replacementRootID, bool copyPendingChangesStorage)
        {
            if (formerRootID != replacementRootID)
            {
                if (formerRootID != null && copyPendingChangesStorage)
                {
                    var submissionTime = StorageFactory.GetDateTimeProvider().Now;
                    var pendingChangesToCopy = await StorageFactory.GetPendingChangesStorage().GetAllPendingChanges<TKey>(CurrentTreeInfo.TreeID, (long)formerRootID);
                    await StorageFactory.GetPendingChangesStorage().InitializePendingChangesForNode(CurrentTreeInfo.TreeID, replacementRootID, new PendingChangesAtTime<TKey>(submissionTime, pendingChangesToCopy), ++NumPendingChangesSetsAdded);
                    NumPendingChangesInRequestBuffer = pendingChangesToCopy.AsEnumerable().Count();
                }
                else
                    await StorageFactory.GetPendingChangesStorage().InitializePendingChangesForNode<TKey>(CurrentTreeInfo.TreeID, replacementRootID);
            }
        }

        public async Task<TreeInfo> AddPendingChanges(PendingChangesAtTime<TKey> pendingChangesAtTime, Guid clientID, long versionNumber) 
        {
            if (RedundancyAvoider.IsRedundant(clientID, versionNumber))
                return CurrentTreeInfo;
            if (TreeIsDeleted || TreeIsDeletedGoal)
                throw new Exception("Tree has already been set for permanent deletion.");
            if (NumPendingChangesInRequestBuffer + pendingChangesAtTime.PendingChanges.AsEnumerable().Count() < UpdateSettings.MaxRequestBufferSize && Rebuilder == null && !CatchupPendingChangesTracker.CatchupPendingChangesStored)
            { // we will store the pending changes in the place where we keep "active" pending changes, i.e. those that are immediately accessible for subsequent queries, even though the changes haven't yet been added to the tree. These will be associated with the current root (and will always be added to the next root before it's created).
                await StorageFactory.GetPendingChangesStorage().AddPendingChangesAtTime(CurrentTreeInfo.TreeID, CurrentTreeInfo.CurrentRootID, pendingChangesAtTime, ++NumPendingChangesSetsAdded);
                NumPendingChangesInRequestBuffer += pendingChangesAtTime.PendingChanges.AsEnumerable().Count();
                NetPendingChangesInRequestBuffer += pendingChangesAtTime.PendingChanges.ExpectedNetPendingChanges;
            }
            else
            { // this would be too much to process in queries before being added to the tree, so we're going to buffer these pending changes by storing them somewhere else for now. We use a root of -1 to identify this root.
                await CatchupPendingChangesTracker.AddPendingChangesToCatchupBuffer(CurrentTreeInfo.TreeID, pendingChangesAtTime, ++NumPendingChangesSetsAdded);
            }
            await UpdateWorkNeeded();
            return CurrentTreeInfo;
        }

        // UintSetStorage: While we are transforming a tree, we use a temporary in-memory storage for the UintSets created. That way, if some nodes are created but then displaced by other nodes, we won't save their UintSets unnecessarily.

        private TemporarilyInMemoryBlob<Guid> GetTemporaryUintSetStorage()
        {
            if (TreeStructure.StoreUintSets)
            {
                IBlob<Guid> uintSetStorage = StorageFactory.GetUintSetStorage();
                TemporarilyInMemoryBlob<Guid> temporaryUintStorage = new TemporarilyInMemoryBlob<Guid>(uintSetStorage);
                return temporaryUintStorage;
            }
            return null;
        }

        private void SetupTemporaryUintSetStorage(CountedNode<TKey> node)
        {
            if (TreeStructure.StoreUintSets)
            {
                TemporarilyInMemoryBlob<Guid> temporaryUintSetStorage = GetTemporaryUintSetStorage();
                node.SetUintSetStorage(temporaryUintSetStorage);
            }
        }

        /// <summary>
        /// Move nodes from temporary into permanent storage.
        /// </summary>
        /// <param name="nodes">The nodes to copy</param>
        /// <returns></returns>
        private async Task CopyNodesFromAddPendingChangesToPermanentStorage(List<CountedNode<TKey>> nodes)
        {
            int i = 1;
            int nodeCount = nodes.Count();
            foreach (var nodeToAdd in nodes)
            {
                IBlob<Guid> uintSetStorage = null;
                var countedInternalNode = (nodeToAdd as CountedInternalNode<TKey>);
                if (countedInternalNode != null && TreeStructure.StoreUintSets)
                {
                    Guid? context = countedInternalNode?.UintSetContext;
                    if (context != null && context != Guid.Empty)
                        uintSetStorage = StorageFactory.GetUintSetStorage();
                    CheckDeferredDeletion(countedInternalNode);
                }
                await StorageFactory.GetNodeStorage().AddNode(CurrentTreeInfo.TreeID, nodeToAdd, uintSetStorage);
                if (i == nodeCount)
                    await SetTreeToNewRoot(nodeToAdd, false); // we're calling this when adding pending changes, so we don't want them associated with the new root
                i++;
            }
        }

        private void CheckDeferredDeletion(CountedInternalNode<TKey> countedInternalNode)
        {
            if (countedInternalNode.InheritMainSetContext)
                NodeDeletionManager.AddDeferredDeletion(new NodeToDeleteLater(
                    new NodeToDelete(
                        (long)countedInternalNode.InheritedMainSetNodeID,
                        countedInternalNode.Depth == 0),
                    countedInternalNode.ID)
                    );
            if (countedInternalNode.InheritDeltaSetsContext && (!countedInternalNode.InheritMainSetContext || countedInternalNode.InheritedDeltaSetsNodeID != countedInternalNode.InheritedMainSetNodeID))
                NodeDeletionManager.AddDeferredDeletion(new NodeToDeleteLater(
                    new NodeToDelete(
                        (long)countedInternalNode.InheritedDeltaSetsNodeID,
                        countedInternalNode.Depth == 0),
                    countedInternalNode.ID)
                    );
        }

#endregion
#region Work management

        public async Task UpdateWorkNeeded()
        {
            int addingPendingChangesWork = await GetAddingPendingChangesWork();
            int flushingWork = FlushingWorkNeeded;
            int rebuildingWork = await GetRebuildingWorkNeeded();
            int deletionWork = NodeDeletionManager.GetDeletionWorkNeeded();
            CurrentTreeInfo.WorkNeeded = Math.Max(addingPendingChangesWork, Math.Max(flushingWork, Math.Max(rebuildingWork, deletionWork)));

            if (CurrentTreeInfo.WorkNeeded == 0)
                TypeOfWorkMostNeeded = null;
            else if (CurrentTreeInfo.WorkNeeded == rebuildingWork) // always takes priority over everything else until it's done
                TypeOfWorkMostNeeded = DifferentWorkTypes.Rebuilding;
            else if (CurrentTreeInfo.WorkNeeded == addingPendingChangesWork)
                TypeOfWorkMostNeeded = DifferentWorkTypes.AddingPendingChangesToTree;
            else if (CurrentTreeInfo.WorkNeeded == flushingWork)
                TypeOfWorkMostNeeded = DifferentWorkTypes.FlushingTree;
            else
                TypeOfWorkMostNeeded = DifferentWorkTypes.DeletionOfOldNodes;
        }

        private Task<int> GetAddingPendingChangesWork()
        {
            if (NumPendingChangesInRequestBuffer > 50)
                return Task.FromResult(NumPendingChangesInRequestBuffer); // we'll keep 50 pending changes here as a matter of routine.
            if (CatchupPendingChangesTracker.CatchupPendingChangesStored)
                return Task.FromResult(15); // If we have more than 15 units of flushing, we'll do that first, before adding more items to the tree. This ensures that we don't have to pass too many queries around the tree.
            return Task.FromResult(NumPendingChangesInRequestBuffer == 0 ? 0 : 1); // 1 indicates that there is still something to do, though it's low priority (only if we set work threshold to 0 will this be cleaned up)
        }

        private async Task<int> GetRebuildingWorkNeeded()
        {
            if (Rebuilder != null)
            {
                if (Rebuilder.Rebuilding)
                    return int.MaxValue; // highest priority
                else if (Rebuilder.Complete)
                    Rebuilder = null; // all done! resume ordinary activity
                else
                    return int.MaxValue; // we haven't even started rebuilding yet, but we need to do so.
            }
            if (Rebuilder == null)
            {
                if (TreeStructure.SplitRangeEvenly)
                    return 0; // never trigger a rebuild
                else
                {
                    double minNeededLeafNodes = (double) CurrentTreeInfo.NumValuesInTree / (double) TreeStructure.MaxItemsPerLeaf;
                    if (minNeededLeafNodes != Math.Floor(minNeededLeafNodes))
                        minNeededLeafNodes = Math.Floor(minNeededLeafNodes) + 1.0;
                    byte expectedDepth = 0;
                    uint itemsStorable = (uint) TreeStructure.MaxItemsPerLeaf;
                    while (itemsStorable < CurrentTreeInfo.NumValuesInTree)
                    {
                        itemsStorable *= (uint) TreeStructure.NumChildrenPerInternalNode;
                        expectedDepth++;
                    }
                    if (CurrentTreeInfo.MaxDepth > expectedDepth + TreeStructure.MaxTolerableImbalance)
                    { // tree is unbalanced -- start rebuilding
                        await InitiateRebalancing();
                        return int.MaxValue;
                    }
                }
            }
            return 0;
        }

        public async Task InitiateRebalancing()
        {
            // This rebuilds the tree from the tree's existing values, thus rebalancing the tree by creating a new one.
            // Note: We set up the Rebuilder with a function, because the rebuilder adds pending changes and flushes the tree before it actually starts rebuilding, so this ensures that we get the latest version of the tree.
            IRebuildSource<TKey> rebuildSource = new RebuildFromTreeSource<TKey>(CurrentTreeInfo, StorageFactory.GetDateTimeProvider().Now);
            await Rebuild(rebuildSource);
        }

        public async Task<TreeInfo> InitiateRebuildFromExternalSource(IRebuildSource<TKey> rebuildSource)
        {
            await Rebuild(rebuildSource);
            return CurrentTreeInfo;
        }

        private async Task Rebuild(IRebuildSource<TKey> rebuildSource)
        {
            // this may replace any previous rebuild command.
            Rebuilder = new TreeRebuilder<TKey>(rebuildSource);
            await UpdateWorkNeeded();
        }

        public async Task<TreeInfo> DoWorkRepeatedly(int workNeededGoal, Task doAfterEachRound = null)
        {
            while (CurrentTreeInfo.WorkNeeded > workNeededGoal)
            {
                await DoRoundOfWork(workNeededGoal);
                if (doAfterEachRound != null)
                    await doAfterEachRound;
            }
            return CurrentTreeInfo;
        }

        public async Task<TreeInfo> DoRoundOfWork(int workNeededGoal = 0)
        {
            switch (TypeOfWorkMostNeeded)
            {
                case DifferentWorkTypes.AddingPendingChangesToTree:
                    await AddPendingChangesFromStorageToTree();
                    break;

                case DifferentWorkTypes.FlushingTree:
                    await FlushTree(new TreeUpdateSettings(workNeededGoal, UpdateSettings.MaxSimultaneousUpdateNodes, 
                        UpdateSettings.MaxRequestBufferSize,
                        UpdateSettings.MinimumRetentionTime));
                    break;

                case DifferentWorkTypes.DeletionOfOldNodes:
                    bool treeIsDeleted = await NodeDeletionManager.DoDeletionWork(CurrentTreeInfo.TreeID, TreeIsDeletedGoal, CatchupPendingChangesTracker);
                    if (TreeIsDeletedGoal == true && treeIsDeleted)
                        TreeIsDeleted = true;
                    break;

                case DifferentWorkTypes.Rebuilding:
                    await Rebuilder.TakeRebuildingStep(this);
                    break;

                default:
                    break;
            }
            await UpdateWorkNeeded();
            //await PrintTree(await CurrentTreeInfo.GetRoot<TKey>()); // uncomment to visualize effect of round of work (not practical for a very large tree)
            return CurrentTreeInfo;
        }

        public async Task PrintTree(bool redundantChanges)
        {
            StringBuilder s = new StringBuilder();
            var root = await GetCurrentRoot();
            var pendingChanges = await StorageFactory.GetPendingChangesStorage().GetAllPendingChanges<TKey>(CurrentTreeInfo.TreeID, root.ID);
            s.Append("****************\n");
            s.Append($"Tree root {root.ID}\n");
            s.Append("****************\n");
            s.Append($"Pending changes in history (number {pendingChanges.Count} net {pendingChanges.ExpectedNetPendingChanges}): {pendingChanges}\n");
            if (await CatchupPendingChangesAreStored())
            {
                s.Append("Catchup changes\n");
                s.Append(await CatchupPendingChangesTracker.PrintOut(CurrentTreeInfo.TreeID));
            }
            s.Append(await PrintTree(root));
            string completeTree = HighlightAnomalies(s.ToString());
            Debug.WriteLine(completeTree);
        }

        public async Task<string> PrintTree(CountedNode<TKey> node)
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < node.Depth; i++)
                s.Append("   ");
            s.Append(node);
            s.Append("\n");
            CountedInternalNode<TKey> internalNode = node as CountedInternalNode<TKey>;
            if (internalNode != null)
            {
                List<CountedNode<TKey>> children = new List<CountedNode<TKey>>();
                foreach (var cni in internalNode.ChildNodeInfos.Where(x => x.Created))
                {
                    var n = await StorageFactory.GetNodeStorage().GetNode<TKey>(CurrentTreeInfo.TreeID, cni.NodeID);
                    children.Add(n);
                    s.Append(await PrintTree(n));
                }
            }
            return s.ToString();
        }

        public string HighlightAnomalies(string s)
        {
            // Add a * after anything where the net effect of the changes seems to make a negative number. This may be correct but is worth flagging. 
            Dictionary<string, int> counter = new Dictionary<string, int>();
            Dictionary<string, int> net = new Dictionary<string, int>();
            StringBuilder current = null;
            int numThisLine = 0;
            char last = ' ';
            bool deletion = false;
            foreach (char c in s)
            {
                if (c == '\n')
                    numThisLine = 0;
                if (c == '<')
                {
                    current = new StringBuilder("<");
                    deletion = last == 'X';
                }
                else if (current != null)
                {
                    current.Append(c);
                    if (c == '>')
                    {
                        numThisLine++;
                        if (numThisLine > 2)
                        { // don't count the first two, which are used for ranges
                            string completeItem = current.ToString();
                            if (counter.ContainsKey(completeItem))
                                counter[completeItem] = counter[completeItem] + 1;
                            else
                            {
                                counter[completeItem] = 1;
                                net[completeItem] = 0;
                            }
                            net[completeItem] = net[completeItem] + (deletion ? -1 : 1);
                        }
                    }
                }
                last = c;
            }
            var anomalies = counter.Where(x => net[x.Key] < 0).ToList();
            foreach (var anomaly in anomalies)
                s = s.Replace(anomaly.Key, anomaly.Key.ToString() + "*" + anomaly.Value);
            return s;
        }

        #endregion

        #region Work execution

        public Task<bool> CatchupPendingChangesAreStored()
        {
            return Task.FromResult<bool>(CatchupPendingChangesTracker.CatchupPendingChangesStored);
        }

        public async Task<TreeInfo> AddPendingChangesFromStorageToTree()
        {
            await CatchupPendingChangesTracker.DeleteNoLongerNeededCatchupBufferedPendingChanges(CurrentTreeInfo.TreeID, false);
            var pendingChanges = (await StorageFactory.GetPendingChangesStorage().GetAllPendingChanges<TKey>(CurrentTreeInfo.TreeID, CurrentTreeInfo.CurrentRootID)); // we leave these pending changes in storage at the current root, so that they can be used for historical queries
            if (!pendingChanges.Any() && CatchupPendingChangesTracker.CatchupPendingChangesStored)
                pendingChanges = await CatchupPendingChangesTracker.GetNextCatchupBufferToAddToPermanentStorage(CurrentTreeInfo.TreeID);
            if (pendingChanges.Any())
                await AddPendingChangesToTree(pendingChanges);
            NumPendingChangesInRequestBuffer = 0;
            return CurrentTreeInfo;
        }

        private async Task AddPendingChangesToTree(PendingChangesCollection<TKey> pendingChanges)
        {
            long formerRootID = CurrentTreeInfo.CurrentRootID;
            var currentRoot = await StorageFactory.GetNodeStorage().GetNode<TKey>(CurrentTreeInfo.TreeID, formerRootID);
            SetupTemporaryUintSetStorage(currentRoot);
            var resultingNodes = await currentRoot.FlushToNode(formerRootID + 1, pendingChanges); // will be multiple nodes only if root was previously a leaf and is now full, but in any event this will be replacing only one node.
            await CopyNodesFromAddPendingChangesToPermanentStorage(resultingNodes);
            NodeDeletionManager.AddDeletionPlan(new NodesDeletionPlan(StorageFactory.GetDateTimeProvider().Now, new List<NodeToDelete> { new NodeToDelete(formerRootID, true) })); // delete only the former root
        }

        public async Task FlushTree(TreeUpdateSettings updateSettingsOverride = null)
        {
            updateSettingsOverride = updateSettingsOverride ?? UpdateSettings;
            long formerRootID = CurrentTreeInfo.CurrentRootID;
            var treeFlusher = new TreeFlusher<TKey>();
            var newNodes = await treeFlusher.FlushTree(CurrentTreeInfo.TreeID, StorageFactory.GetNodeStorage(), CurrentTreeInfo.CurrentRootID, updateSettingsOverride, GetTemporaryUintSetStorage());
            foreach (var newNode in newNodes)
                if (newNode is CountedInternalNode<TKey>)
                    CheckDeferredDeletion((CountedInternalNode<TKey>)newNode);
            var nodesToDelete = treeFlusher.NodesToMarkForDeletion;
            var deletionTime = StorageFactory.GetDateTimeProvider().Now;
            NodeDeletionManager.AddDeletionPlan(new NodesDeletionPlan(deletionTime, nodesToDelete));
            var newRoot = newNodes.SingleOrDefault(x => x.Depth == 0);
            if (newRoot != null)
                await SetTreeToNewRoot(newRoot, true);

        }

        #endregion

        #region Deletion of entire tree

        // Note that most deletion tasks are controlled by the NodeDeletionManager

        public async Task InitiateDeleteTreeAndHistory()
        {
            TreeIsDeletedGoal = true;
            await StorageFactory.GetPendingChangesStorage().RemoveAllPendingChangesForNode<TKey>(CurrentTreeInfo.TreeID, -1); // inactive items storage
            NodeDeletionManager.AddDeletionPlan(new TreeDeletionPlan(StorageFactory.GetDateTimeProvider().Now, CurrentTreeInfo.CurrentRootID));
            // Deletion could be immediate or it could occur after further rounds of work
            bool isDeleted = await NodeDeletionManager.ProcessDeletionPlans(CurrentTreeInfo.TreeID, TreeIsDeletedGoal, CatchupPendingChangesTracker);
            if (isDeleted)
                TreeIsDeleted = true;
            await UpdateWorkNeeded(); // more nodes may need deletion over time before the deletion is complete
        }

        public Task ReinstateTree(CountedNode<TKey> newRootNode, TreeInfo treeInfo, TreeUpdateSettings updateSettings, TreeStructure treeStructure)
        {
            // You would call this after a tree is completely deleted to reinstate a tree at the same ID. You would pass it information created by calling the TreeInfoFactory. Of course, you could also just create a new TreeHistoryManager instance. But this is useful if we keep all TreeHistoryManager instances around and then need to revive one when we are reviving storage.
            if (TreeIsDeletedGoal != TreeIsDeleted)
                throw new Exception("Must wait until tree is deleted before reinstating the tree."); // We could fix this by saving the other parameters and then setting them once the previous deletion is complete.
            TreeIsDeletedGoal = false;
            TreeIsDeleted = false;
            CurrentTreeInfo = treeInfo;
            UpdateSettings = updateSettings;
            TreeStructure = treeStructure;
            return Task.CompletedTask;
        }

        public Task<int> GetDeletionWorkNeeded()
        {
            return Task.FromResult(NodeDeletionManager.GetDeletionWorkNeeded());
        }

        #endregion
    }
}
