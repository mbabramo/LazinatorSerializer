using CountedTree.Core;
using CountedTree.PendingChanges;
using CountedTree.Node;
using CountedTree.NodeStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using R8RUtilities;
using System.Diagnostics;
using Lazinator.Core;

namespace CountedTree.Updating
{
    public class TreeFlusher<TKey> : TreeCoordinatorBase<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        long NextNodeID;
        public int WorkNeeded;
        public byte MaxDepth;
        public uint NumValuesInTree;
        public int NumNodesChangedOrAdded;
        public List<NodeToDelete> NodesToMarkForDeletion;
        public Guid TreeID;
        TreeUpdateSettings UpdateSettings;
        TemporarilyInMemoryBlob<Guid> TemporaryUintSetStorage;

        #region Flushing

        public async Task<List<CountedNode<TKey>>> FlushTree(Guid treeID, INodeStorage nodeStorage, long initialRootID, TreeUpdateSettings updateSettings, TemporarilyInMemoryBlob<Guid> temporaryUintSetStorage)
        {
            TreeID = treeID;
            NodeStorage = nodeStorage;
            TemporaryUintSetStorage = temporaryUintSetStorage;
            RootID = initialRootID;
            UpdateSettings = updateSettings;
            NextNodeID = initialRootID + 1;
            CountedNode<TKey> rootNode = await GetInitialRootNode(TreeID);
            await rootNode.SetUintSetStorage(TemporaryUintSetStorage);
            await DoFlushing();
            NodesToMarkForDeletion = await TemporaryNodeStorage.PrepareReplacedNodesForDeletion(RootID, TemporaryUintSetStorage);
            return await AddReplacementNodes(rootNode);
        }
        
        private async Task DoFlushing()
        {
            await LoadNodesForFlushing(UpdateSettings.MinWorkThreshold);
            // we flush only from internal nodes and only when buffers to specific children are too large. The leaf nodes will never be too large because when we flush to them, we get multiple nodes if necessary.
            var internalNodes = TemporaryNodeStorage.ToList().Where(x => x is CountedInternalNode<TKey>).Cast<CountedInternalNode<TKey>>().ToList();
            foreach (var internalNode in internalNodes)
            {
                var updatedVersion = (CountedInternalNode<TKey>)TemporaryNodeStorage.GetNodeOrReplacement(internalNode.ID); // we may be flushing from a node that has already been replaced, when a parent flushed to it.
                await FlushFromInternalNode(UpdateSettings.MinWorkThreshold, updatedVersion);
                // var updatedVersion2 = (CountedInternalNode<TKey>)Nodes.GetNodeOrReplacement(internalNode.ID);
            }
        }
        
        private async Task FlushFromInternalNode(int minWorkThreshold, CountedInternalNode<TKey> internalNode)
        {
            for (int c = 0; c < internalNode.TreeStructure.NumChildrenPerInternalNode; c++)
            {
                if (internalNode.MainBuffer.NumPendingChangesAtNodeIndex(c) > minWorkThreshold)
                {
                    // reload the nodes, in case there's been a transformation
                    internalNode = (CountedInternalNode<TKey>)TemporaryNodeStorage.GetNodeOrReplacement(internalNode.ID);
                    var targetNode = TemporaryNodeStorage.GetNodeOrReplacement(internalNode.ChildNodeInfos[c].NodeID);
                    if (targetNode != null) // could be null if not loaded
                        await FlushToChild(internalNode, c, targetNode); 
                }
            }
        }

        private async Task FlushToChild(CountedInternalNode<TKey> sourceInternalNode, int childIndex, CountedNode<TKey> targetNode)
        {
            // We copy from sourceInternalNode to temporaryInternalNode and then create finalInternalNode.
            // First, create a temporary replacement for the source, excluding the changes we'll be flushing to a child. This replacement should have the exact same UintSet as the source. It's only temporary because it doesn't have the information on its new children, since we don't create them until the next step.
            var results = await sourceInternalNode.GetNodeMinusChangesFlushedToChild(NextNodeID, childIndex);
            CountedInternalNode<TKey> temporaryInternalNode = results.Item1;
            PendingChangesCollection<TKey> flushedChanges = results.Item2;
            NextNodeID++;
            if (sourceInternalNode.TreeStructure.StoreUintSets)
                await targetNode.SetUintSetStorage(TemporaryUintSetStorage); // leaf node may not yet be set up  and must be able to initialize new internal node's UintSet
            // Next, get the new nodes that are the descendants of this temporary replacement for the source node.
            var nodesToAdd = await targetNode.FlushToNode(NextNodeID, flushedChanges);
            if (nodesToAdd.Any())
                NextNodeID = nodesToAdd.Max(x => x.ID) + 1; // An internal node always must have a higher ID number than its children. Note that we can't just increment by nodesToAdd.Count(), because there may be skipped numbers as a result of empty nodes.
            // Finally, we can create the final version of the internal node, replacing the temporary version. 
            NodeInfo<TKey> replacementChildNodeInfo = nodesToAdd.Last().NodeInfo.Clone(); // this is the highest of the nodes to add (possibly the only one)
            var finalInternalNode = await temporaryInternalNode.ReplaceChildInfo(NextNodeID, childIndex, replacementChildNodeInfo, sourceInternalNode.NodeForDescendantsToInheritFrom, sourceInternalNode.NodeForDescendantDeltaSetsToBuildOn, TemporaryUintSetStorage); // we pass sourceInternalNode, because any UintSet has not been copied to temporaryInternalNode.
            nodesToAdd.Add(finalInternalNode);
            NextNodeID++;
            await InsertReplacementNodesFollowingFlush(sourceInternalNode.ID, targetNode.ID, nodesToAdd);
        }

        private async Task InsertReplacementNodesFollowingFlush(long originalNodeID, long targetNodeID, List<CountedNode<TKey>> nodesToAdd)
        {
            var replacementForOriginal = nodesToAdd.Last();
            var nodesToAddCount = nodesToAdd.Count();
            for (int i = nodesToAddCount - 1; i >= 0; i--)
            // for (int i = 0; i < nodesToAddCount; i++)
            {
                var nodeToAdd = nodesToAdd[i];
                if (i == nodesToAddCount - 2) // this is a replacement for the original target (i.e., node being flushed to)
                    await TemporaryNodeStorage.AddTransformedNodeToNodeStorage(node: nodeToAdd, previousID: targetNodeID);
                else if (i == nodesToAddCount - 1) // this is the replacement for the original node from which we are flushing, so we need to note the replacement again
                    await TemporaryNodeStorage.AddTransformedNodeToNodeStorage(node: replacementForOriginal, previousID: originalNodeID);
                else // this is brand new
                    await TemporaryNodeStorage.AddNewOrUnmutatedNodeToNodeStorage(nodeToAdd, parentID: replacementForOriginal.ID, isEmpty: false);
            }
            NextNodeID = replacementForOriginal.ID + 1;
            await InsertReplacementAncestorNodes(replacementForOriginal);
        }

        private async Task InsertReplacementAncestorNodes(CountedNode<TKey> highestChangedNode)
        {
            // We will move up the tree, creating ancestors.
            CountedNode<TKey> child = highestChangedNode; // the original node that was changed is now the child of the first ancestor (this will change below as we move up the tree)
            CountedInternalNode<TKey> originalParent = TemporaryNodeStorage.GetParentOfNode(highestChangedNode); // this previously was the child's parent; we're going to need a new parent
            while (originalParent != null)
            {
                CountedInternalNode<TKey> replacementParent = await originalParent.ReplaceChildInfo(NextNodeID++, child.NodeInfo, TemporaryUintSetStorage);
                await TemporaryNodeStorage.AddTransformedNodeToNodeStorage(node: replacementParent, previousID: originalParent.ID, setChildParentsToThisNode: true);
                child = replacementParent;
                originalParent = TemporaryNodeStorage.GetParentOfNode(replacementParent);
            }
        }

        private async Task<List<CountedNode<TKey>>> AddReplacementNodes(CountedNode<TKey> originalRootNode)
        {
            List<CountedNode<TKey>> replacementNodes = new List<CountedNode<TKey>>();
            CountedNode<TKey> newRoot = null;
            NumNodesChangedOrAdded = 0;
            foreach (var node in TemporaryNodeStorage.ToList())
            {
                if (newRoot == null || node.ID > RootID)
                {
                    newRoot = node;
                    RootID = node.ID;
                }
                await node.SetUintSetStorage(TemporaryUintSetStorage);
                await NodeStorage.AddNode(TreeID, node, node.TreeStructure.StoreUintSets ? StorageFactory.GetUintSetStorage() : null);
                replacementNodes.Add(node);
                NumNodesChangedOrAdded++;
            }
            newRoot = newRoot ?? originalRootNode;
            WorkNeeded = newRoot.NodeInfo.MaxWorkNeededInSubtree;
            MaxDepth = newRoot.NodeInfo.MaxDepth;
            NumValuesInTree = newRoot.NodeInfo.NumSubtreeValues;
            return replacementNodes;
        }

        #endregion

        #region Loading nodes before flushing

        // Loading algorithm: Using this algorithm, we visit various nodes in depth-first order, adding them to our node collection. At any point, look for nodes to flush to first (i.e., nodes that the current node has data for). We do this first because we don't want to fill up on nodes with data to flush from. If there are such nodes, load them right away and process them. If not, load the first node to flush from and process it. After that, load the rest of the nodes to flush from and process them. At any point, if we have processed all nodes, or if we have reached our preferred limit on nodes and have at least one node to flush to, then we stop. 

        public class FlushingInfo
        {
            public long NodeID;
            public List<long> NodesToFlushTo;
            public List<long> NodesToFlushFrom;
            public bool AllNodesToFlushFromProcessed = false;
        }

        int NumRequests = 0;

        private int MaxRequestsLeft => UpdateSettings.MaxSimultaneousUpdateNodes - NumRequests;

        private async Task LoadNodesForFlushing(int minWorkThreshold)
        {
            Stack<FlushingInfo> FlushingStack = new Stack<FlushingInfo>();
            FlushingStack.Push(new FlushingInfo() { NodeID = RootID });
            bool atLeastOneNodeToFlushTo = false;
            bool done = false;
            while (!done)
            { 
                FlushingInfo topItem = FlushingStack.Peek();
                var topItemNode = TemporaryNodeStorage.GetNode(topItem.NodeID);
                if (topItem.NodesToFlushTo == null)
                {
                    topItem.NodesToFlushTo = await GetNodesToFlushTo(minWorkThreshold, topItemNode);
                    if (topItem.NodesToFlushTo.Any())
                    {
                        atLeastOneNodeToFlushTo = true;
                        //if (topItemNode is CountedInternalNode<TKey>)
                        //{
                        //    var nodeAsInternal = ((CountedInternalNode<TKey>)topItemNode);
                        //    foreach (var notYetCreatedNode in nodeAsInternal.ChildNodeInfos.Where(x => !x.Created))
                        //        await AddNotYetCreatedNode(notYetCreatedNode, nodeAsInternal.ID, nodeAsInternal.TreeStructure);
                        //}
                        await AddNodesToFlushingStack(FlushingStack, topItem.NodesToFlushTo, topItem.NodeID);
                    }
                }
                else if (topItem.NodesToFlushFrom == null)
                { // load all nodes to flush from, but put only the first on the stack
                    topItem.NodesToFlushFrom = topItemNode.GetNodesToFlushFrom(minWorkThreshold).ToList();
                    if (topItem.NodesToFlushFrom.Any())
                        await AddNodesToFlushingStack(FlushingStack, new List<long>() { topItem.NodesToFlushFrom.First() }, topItem.NodeID);
                }
                else if (!topItem.AllNodesToFlushFromProcessed)
                { // we've now visited everything below, so we can put the remaining nodes to flush from onto the stack
                    if (topItem.NodesToFlushFrom.Count() > 1)
                        await AddNodesToFlushingStack(FlushingStack, topItem.NodesToFlushFrom.Skip(1).ToList(), topItem.NodeID);
                    topItem.AllNodesToFlushFromProcessed = true;
                }
                else // we're done with this
                    FlushingStack.Pop();
                done = !FlushingStack.Any() || (MaxRequestsLeft <= 0 && atLeastOneNodeToFlushTo); 
            }
        }

        private async Task<List<long>> GetNodesToFlushTo(int minWorkThreshold, CountedNode<TKey> topItemNode)
        {
            List<Tuple<long, bool>> nodeIDsAndCreationInfo = topItemNode.GetNodesToFlushTo(minWorkThreshold).ToList();
            await CreateEmptyNodesToBeFlushedTo(topItemNode, nodeIDsAndCreationInfo);
            var nodesToFlushTo = nodeIDsAndCreationInfo.Select(x => x.Item1).ToList();
            return nodesToFlushTo;
        }

        private async Task CreateEmptyNodesToBeFlushedTo(CountedNode<TKey> topItemNode, List<Tuple<long, bool>> nodeIDsAndCreationInfo)
        {
            foreach (var emptyNodeToCreate in nodeIDsAndCreationInfo.Where(x => !x.Item2))
            { // this node hasn't been created yet -- so we must create it so that we can flush to it.
                CountedInternalNode<TKey> topItemInternalNode = (CountedInternalNode<TKey>)topItemNode;
                NodeInfo<TKey> nodeInfo = topItemInternalNode.ChildNodeInfos.Single(x => x.NodeID == emptyNodeToCreate.Item1);
                CountedLeafNode<TKey> emptyNode = new CountedLeafNode<TKey>(new List<KeyAndID<TKey>>(), emptyNodeToCreate.Item1, nodeInfo.Depth, topItemInternalNode.TreeStructure, nodeInfo.FirstExclusive, nodeInfo.LastInclusive); // since this is empty, it has no UintSet.
                await TemporaryNodeStorage.AddNewOrUnmutatedNodeToNodeStorage(emptyNode, topItemNode.ID, isEmpty: true);
            }
        }

        private async Task AddNodesToFlushingStack(Stack<FlushingInfo> FlushingStack, List<long> nodeIDs, long? parentNodeID)
        {
            await RequestNodesByID(TreeID, nodeIDs, parentNodeID); // note that the nodes might already be loaded (by a previous work cycle)
            foreach (long n in nodeIDs)
            {
                FlushingStack.Push(new FlushingInfo() { NodeID = n });
                NumRequests++;
            }
        }

        #endregion

        #region Utility methods


        private async Task PrintTree(CountedNode<TKey> node)
        {
            if (node.Depth == 0)
                Debug.WriteLine("");
            for (int i = 0; i < node.Depth; i++)
                Debug.Write("   ");
            Debug.WriteLine(node);
            CountedInternalNode<TKey> internalNode = node as CountedInternalNode<TKey>;
            if (internalNode != null)
            {
                List<CountedNode<TKey>> children = new List<CountedNode<TKey>>();
                foreach (var cni in internalNode.ChildNodeInfos)
                {
                    var n = TemporaryNodeStorage.GetNode(cni.NodeID) ?? await NodeStorage.GetNode<TKey>(TreeID, cni.NodeID);
                    children.Add(n);
                    await PrintTree(n);
                }
            }
        }
        #endregion
    }
}
