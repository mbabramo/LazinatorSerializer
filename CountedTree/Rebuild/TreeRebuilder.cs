using CountedTree.Core;
using CountedTree.Node;
using CountedTree.PendingChanges;
using CountedTree.Updating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using R8RUtilities;
using CountedTree.UintSets;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace CountedTree.Rebuild
{
    public partial class TreeRebuilder<TKey> : ITreeRebuilder<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        // We may want to rebuild our index from scratch for two reasons. First, we may perceive that the tree is unbalanced. In that case, index rebuilding becomes the highest priority work item. Second, we might be building the index initially, or rebuilding it because all of the values have changed. In this case, the new values must come from an external source. 
        // Algorithm: We build an index depth first. So, at the first level, we may see that we need items 0-3527. Then, we note at the second level how we'll divide the 3528 items so that we use all the slots from the top level. And so on. When we fill in all the items for the parent item, we go back up to the parent.
        // We don't change the RootID until we're all done. At that point, we'll designate the previous tree as needing to be deleted at some point in the future.

        public TreeRebuilder(IRebuildSource<TKey> rebuildSource)
        {
            RebuildSource = rebuildSource;
        }

        public async Task TakeRebuildingStep(TreeHistoryManager<TKey> historyManager)
        {
            if (!Rebuilding)
            { // prepare to initiate the rebuilding -- we want to process any stored pending changes
                if (historyManager.NumPendingChangesInRequestBuffer == 0)
                    await PlanRebuild(historyManager);
                else
                    await historyManager.AddPendingChangesFromStorageToTree();
            }
            else
            {
                if (Rebuilding_SplitRangeEvenly)
                    await TakeSplitEvenlyStep(historyManager);
                else
                    await VisitRebuildingStack(historyManager);
            }
        }

        public async Task PlanRebuild(TreeHistoryManager<TKey> historyManager)
        {
            Rebuilding = true;
            uint numberOfItems = await RebuildSource.GetNumberItems();
            if (numberOfItems == 0)
            {
                await RebuildAsEmpty(historyManager);
                return;
            }
            var oldRoot = await historyManager.GetCurrentRoot();
            if (oldRoot.TreeStructure.StoreUintSets)
                UintStack = new Stack<UintSetWithLoc>(); 
            Rebuilding_SplitRangeEvenly = oldRoot.TreeStructure.SplitRangeEvenly;
            if (Rebuilding_SplitRangeEvenly)
            { // use special procedure -- essentially, just flush all items to nodes. Note that as currently implemented, the tree will have no values for queries as of the time of the beginning of the rebuilding. This is different from the behavior with other rebuilds, where the tree has its structure frozen in place until the rebuild is complete. But since in general, we don't need to rebuild trees split evenly -- that is, we use this only for initially creating trees split evenly -- it shouldn't be a problem.
                await RebuildAsEmpty(historyManager); // start empty, then we'll add to it from the RebuildSource
                NumToDo_SplitRangeEvenly = numberOfItems;
                return;
            }
            DepthOfLeafNodes = 0;
            uint numItemsContainedAtDepth = (uint) oldRoot.TreeStructure.MaxItemsPerLeaf;
            while (numItemsContainedAtDepth < numberOfItems)
            {
                DepthOfLeafNodes++;
                numItemsContainedAtDepth *= (uint)oldRoot.TreeStructure.NumChildrenPerInternalNode;
            }
            NextNodeID = historyManager.CurrentTreeInfo.CurrentRootID + 1;
            RebuildingStack = new Stack<PlannedRebuildNode<TKey>>();
            RebuildingStack.Push(new PlannedRebuildNode<TKey>(0, DepthOfLeafNodes, new byte[] { }, 0, 0, (uint)((uint)numberOfItems - 1), oldRoot.TreeStructure, null));
            LastItemAdded = await RebuildSource.GetFirstExclusive(); // this will be the first item in the first range
        }

        public async Task RebuildAsEmpty(TreeHistoryManager<TKey> historyManager)
        {
            NextNodeID = historyManager.CurrentTreeInfo.CurrentRootID + 1;
            var oldRoot = await historyManager.GetCurrentRoot();
            ReplacementRoot = new CountedLeafNode<TKey>(new List<KeyAndID<TKey>>(), NextNodeID, 0, oldRoot.TreeStructure, oldRoot.NodeInfo.FirstExclusive, oldRoot.NodeInfo.LastInclusive);
            await StorageFactory.GetNodeStorage().AddNode(historyManager.CurrentTreeInfo.TreeID, ReplacementRoot, null); // no uintset for leaf node
            NextNodeID++;
            await CompleteRebuild(historyManager);
        }


        private async Task TakeSplitEvenlyStep(TreeHistoryManager<TKey> historyManager)
        {
            // Note that this will create new nodes but not an entirely new future tree. But it still should be reasonably efficient because we'll be flushing items in order, rather than in random places in the tree.
            uint numAtOnce = (uint) historyManager.UpdateSettings.MaxRequestBufferSize;
            uint startRange = NumComplete_SplitRangeEvenly;
            uint endRange = NumComplete_SplitRangeEvenly + numAtOnce;
            if (endRange > NumToDo_SplitRangeEvenly - 1)
                endRange = NumToDo_SplitRangeEvenly - 1;
            var numToDo = (int)(endRange - startRange + 1);
            var items = await RebuildSource.GetNextItems(numToDo);
            var pendingChanges = new PendingChangesCollection<TKey>(items.Select(x => new PendingChange<TKey>(x, false)), true /* technically not, but we have only one item per ID, so we don't need to reorder */);
            long versionNumber = 0;
            await historyManager.AddPendingChanges(new PendingChangesAtTime<TKey>(StorageFactory.GetDateTimeProvider().Now, pendingChanges), new Guid(), ++versionNumber);
            await historyManager.AddPendingChangesFromStorageToTree();
            while (historyManager.FlushingWorkNeeded > 10)
                await historyManager.FlushTree(historyManager.UpdateSettings);
            NumComplete_SplitRangeEvenly += (uint) numToDo;
            if (NumComplete_SplitRangeEvenly == NumToDo_SplitRangeEvenly)
                await FinalizeRebuild();
        }

        private async Task VisitRebuildingStack(TreeHistoryManager<TKey> historyManager)
        {
            const int maxNumLeavesAtOnce = 20;
            int numLeavesBuilt = 0;
            while (RebuildingStack.Any() && numLeavesBuilt < maxNumLeavesAtOnce)
            {
                PlannedRebuildNode<TKey> top = RebuildingStack.Peek();
                if (top.Depth == top.DepthOfLeafNodes)
                {
                    await AddLeafNode(top, historyManager);
                    RebuildingStack.Pop();
                }
                else
                {
                    // This is an internal node. 
                    // First, let's create a blank UintSet if necessary. We'll fill this in if necessary when adding leaf nodes somewhere under the internal node.
                    if (top.NumChildrenCreated == 0 && top.TreeStructure.StoreUintSets)
                        UintStack.Push(new UintSetWithLoc(new UintSet(), top.TreeStructure.StoreUintSetLocs ? new UintSetLoc(top.TreeStructure.NumChildrenPerInternalNode <= 16) : null));
                    // Now, let's plan to build the children of this internal node. After we build each child, we'll fully visit the child before coming back and building the next child.
                    if (top.NumChildrenCreated < top.TreeStructure.NumChildrenPerInternalNode)
                    { // add a new child (and visit there before continuing)
                        PlannedRebuildNode<TKey> child = top.GetPlannedChild(top.NumChildrenCreated);
                        top.NumChildrenCreated++;
                        RebuildingStack.Push(child);
                    }
                    else
                    {
                        // All the children are done. So we can add this internal node.
                        await AddInternalNode(top, historyManager);
                        RebuildingStack.Pop();
                        //if (top.TreeStructure.StoreBitSets)
                        //    await StoreUintSet(CountedInternalNode<TKey>.GetUintSetContext(true, top.TreeStructure.BitSetStorageContext, NextNodeID - 1), top.UintSet);
                    }
                }
            }
            if (!RebuildingStack.Any())
                await CompleteRebuild(historyManager);
        }

        private async Task AddLeafNode(PlannedRebuildNode<TKey> plannedNode, TreeHistoryManager<TKey> historyManager)
        {
            var items = await RebuildSource.GetNextItems((int)(plannedNode.EndIndex - plannedNode.StartIndex + 1));
            if (!items.Any())
                throw new Exception("Internal error. Rebuild source should return items for each leaf node.");
            KeyAndID<TKey>? first = LastItemAdded;
            KeyAndID<TKey>? last = plannedNode.IsLastOfAllThisLevel ? await RebuildSource.GetLastInclusive() : items.Last();
            CountedLeafNode<TKey> leafNode = new CountedLeafNode<TKey>(items, NextNodeID, plannedNode.Depth, plannedNode.TreeStructure, first, last);
            LastItemAdded = items.Last();
            await StorageFactory.GetNodeStorage().AddNode(historyManager.CurrentTreeInfo.TreeID, leafNode, null); // no uintset for leaf node
            if (plannedNode.Parent == null)
                ReplacementRoot = leafNode;
            else
                plannedNode.Parent.ChildNodeInfos.Add(new NodeInfo<TKey>(NextNodeID, 1, (uint) items.Count(), 0, 0, plannedNode.Depth, plannedNode.DepthOfLeafNodes, true, first, last));
            if (plannedNode.TreeStructure.StoreUintSets)
                AddUintsToUintSetStack(plannedNode.RouteToHere, items.Select(x => (WUInt32)x.ID));
            NextNodeID++;
        }

        private async Task AddInternalNode(PlannedRebuildNode<TKey> plannedNode, TreeHistoryManager<TKey> historyManager)
        {
            UintSetWithLoc uintSetWithLoc = null;
            IBlob<Guid> uintSetStorage = null;
            if (plannedNode.TreeStructure.StoreUintSets)
            {
                uintSetWithLoc = UintStack.Pop();
                uintSetStorage = StorageFactory.GetUintSetStorage();
            }
            CountedInternalNode<TKey> internalNode = await CountedInternalNode<TKey>.GetCountedInternalNodeWithUintSet(plannedNode.ChildNodeInfos.ToArray(), new PendingChanges.PendingChangesCollection<TKey>(), NextNodeID, plannedNode.Depth, plannedNode.TreeStructure, uintSetStorage, uintSetWithLoc);
            await StorageFactory.GetNodeStorage().AddNode(historyManager.CurrentTreeInfo.TreeID, internalNode, null);

            if (plannedNode.Parent == null)
                ReplacementRoot = internalNode;
            else
                plannedNode.Parent.ChildNodeInfos.Add(new NodeInfo<TKey>(NextNodeID, plannedNode.ChildNodeInfos.Sum(x => x.NumNodes), (uint)plannedNode.ChildNodeInfos.Sum(x => x.NumSubtreeValues), 0, 0, plannedNode.Depth, plannedNode.DepthOfLeafNodes, true, plannedNode.ChildNodeInfos.First().FirstExclusive, plannedNode.ChildNodeInfos.Last().LastInclusive));
            NextNodeID++;
        }

        private async Task CompleteRebuild(TreeHistoryManager<TKey> historyManager)
        {
            if (!Rebuilding_SplitRangeEvenly)
                await FinalizeRebuild(); // these finalization steps will take place later when splitting the range evenly
            RebuildingStack = null;
            long formerRootID = historyManager.CurrentTreeInfo.CurrentRootID;
            await historyManager.SetTreeToNewRoot(ReplacementRoot, false); // we've already copied pending changes, so we don't recopy them
            var oldRoot = new TreeDeletionPlan(StorageFactory.GetDateTimeProvider().Now, formerRootID);
            historyManager.NodeDeletionManager.AddDeletionPlan(oldRoot);
        }

        private async Task FinalizeRebuild()
        {
            Rebuilding = false;
            Complete = true;
            await RebuildSource.ReportComplete();
            RebuildSource = null;
        }

        #region UintSets

        private void AddUintsToUintSetStack(byte[] routeToHere, IEnumerable<WUInt32> uintsToSet)
        {
            IEnumerator<byte> childIndexEnumerator = routeToHere.Reverse().GetEnumerator();
            foreach (UintSetWithLoc u in UintStack.AsEnumerable())
            {
                if (u.Loc != null)
                {
                    childIndexEnumerator.MoveNext();
                    byte childIndex = childIndexEnumerator.Current;
                    List<byte> childIndexRepeated = Enumerable.Range(0, uintsToSet.Count()).Select(x => childIndex).ToList();
                    u.Loc = u.Loc.AddItemsAtUintIndices(u.Set, uintsToSet.ToList(), childIndexRepeated);
                }
                u.Set.AddUints(uintsToSet);
            }
        }

        #endregion Uintsets
    }
}
