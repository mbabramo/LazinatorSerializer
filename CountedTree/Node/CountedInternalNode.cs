using System;
using System.Collections.Generic;
using System.Linq;
using CountedTree.Core;
using CountedTree.NodeBuffers;
using CountedTree.PendingChanges;
using CountedTree.NodeResults;
using CountedTree.Queries;
using R8RUtilities;
using CountedTree.UintSets;
using System.Threading.Tasks;
using Utility;
using Lazinator.Wrappers;
using Lazinator.Core;
using Lazinator.Attributes;

namespace CountedTree.Node
{
    [Implements(new string[] { "PostDeserialization" })]
    public partial class CountedInternalNode<TKey> : CountedNode<TKey>, ICountedInternalNode<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        /// <summary>
        /// The number of items in a subtree (not including pending changes)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public uint NumItemsInSubtree(int index) => ChildNodeInfos[index].NumSubtreeValues;

        public const int MinBeforeUsingDeltaSet = 500; // If less than this, we will always integrate into the main set.

        internal bool InheritMainSetContext => InheritedMainSetNodeID != null;
        internal bool InheritDeltaSetsContext => InheritedDeltaSetsNodeID != null;

        #region Construction

        public CountedInternalNode()
        {

        }

        public CountedInternalNode(NodeInfo<TKey>[] childNodeInfos, PendingChangesCollection<TKey> pendingChanges, long nodeID, byte depth, TreeStructure treeStructure) : base(depth, treeStructure)
        {
            KeyAndID<TKey>?[] splitValues = childNodeInfos.Select(x => x.LastInclusive).Take(childNodeInfos.Length - 1).ToArray();
            if (splitValues.Count() != TreeStructure.NumChildrenPerInternalNode - 1)
                throw new Exception("Invalid split values.");
            ChildNodeInfos = childNodeInfos;
            MainBuffer = new InternalNodeBuffer<TKey>(TreeStructure.NumChildrenPerInternalNode, SplitValue, pendingChanges);
            NodeInfo = CalculateNodeInfoBasedOnChildren(nodeID, depth, pendingChanges.ExpectedNetPendingChanges);
            Cumulator = new CumulativeItemsCounter<TKey>(NumItemsInSubtree);
            Cumulator.CalculateCumulativeItemsInSubtreeIfNecessary(TreeStructure.NumChildrenPerInternalNode);

        }
            
        public CountedInternalNode(byte depth, NodeInfo<TKey> thisNodeInfo, NodeInfo<TKey>[] childNodeInfos, INodeBufferBaseMethods<TKey> mainBuffer, TreeStructure treeStructure) : base(depth, treeStructure)
        {
            // this is used for cloning with a new ID
            ChildNodeInfos = childNodeInfos;
            MainBuffer = mainBuffer;
            NodeInfo = thisNodeInfo;
        }


        public CountedInternalNode(NodeInfo<TKey> nodeInfo, NodeInfo<TKey>[] childNodeInfos, CumulativeItemsCounter<TKey> cumulator, INodeBufferBaseMethods<TKey> mainBuffer, long? inheritedMainSetNodeID, long? inheritedDeltaSetsNodeID, bool inheritMainSetContext, bool inheritDeltaSetsContext, bool uintSetInitialized, byte depth, TreeStructure treeStructure) : base(depth, treeStructure)
        {
            NodeInfo = nodeInfo;
            ChildNodeInfos = childNodeInfos;
            Cumulator = cumulator;
            MainBuffer = mainBuffer;
            if (inheritMainSetContext)
                InheritedMainSetNodeID = inheritedMainSetNodeID;
            if (inheritDeltaSetsContext)
                InheritedDeltaSetsNodeID = inheritedDeltaSetsNodeID;
            UintSetInitialized = uintSetInitialized;
        }

        public void PostDeserialization()
        {
            if (Cumulator != null)
                Cumulator.NumItemsInSubtreeFn = NumItemsInSubtree;
        }


        public static async Task<CountedInternalNode<TKey>> GetCountedInternalNodeWithUintSet(NodeInfo<TKey>[] childNodeInfos, PendingChangesCollection<TKey> pendingChangesToBuffer, long nodeID, byte depth, TreeStructure treeStructure, IBlob<Guid> uintSetStorage, UintSetWithLoc uintSetWithLoc)
        {
            // Note: We use a static factory method here and below, because we can't use async constructors. 
            CountedInternalNode<TKey> node = new CountedInternalNode<TKey>(childNodeInfos, pendingChangesToBuffer, nodeID, depth, treeStructure);
            if (uintSetStorage != null && uintSetWithLoc != null)
            {
                await node.SetUintSetStorage(uintSetStorage);
                await node.InitializeUintSetStorage(uintSetWithLoc);
            }
            return node;
        }

        public static async Task<CountedInternalNode<TKey>> GetCountedInternalNodeWithUintSet(NodeInfo<TKey>[] childNodeInfos, PendingChangesCollection<TKey> pendingChangesToBuffer, long nodeID, byte depth, TreeStructure treeStructure, IBlob<Guid> uintSetStorage, long? inheritedNodeID, long? deltaSetsNodeID, PendingChangesCollection<TKey> pendingChangesToAddToUintSet)
        {
            // Note: We use a static factory method here, because we can't use async constructors. 
            CountedInternalNode<TKey> node = new CountedInternalNode<TKey>(childNodeInfos, pendingChangesToBuffer, nodeID, depth, treeStructure);
            await node.SetupUintSetStorage(uintSetStorage, inheritedNodeID, deltaSetsNodeID, pendingChangesToAddToUintSet); // note that this will create new UintSet storage if pendingChangesToAddToUintSet has items
            return node;
        }

        public override string ToString()
        {
            var pendingChangesCollection = MainBuffer.AllPendingChanges();
            var mainNodeInfo = $"{NodeInfo} Buffer (num: {pendingChangesCollection.Count} net: {pendingChangesCollection.ExpectedNetPendingChanges}): {pendingChangesCollection}";
            //var childrenInfo = $"{String.Join(", ", ChildNodeInfos.Select(x => $"{x.NodeID}: {x.WorkNeeded}&{x.MaxWorkNeededInSubtree}"))}";
            return mainNodeInfo; // + " Children work needed: " + childrenInfo;
        }

        

        private NodeInfo<TKey> CalculateNodeInfoBasedOnChildren(long thisNodeID, byte depth, int netPendingChanges)
        {
            int amountOfWorkNeeded = MainBuffer.GetMaxPendingChanges();
            var childWithMostWorkNeeded = ChildNodeInfos.OrderByDescending(x => x.MaxWorkNeededInSubtree).First();
            bool childIsMostWorkNeeded = childWithMostWorkNeeded.MaxWorkNeededInSubtree > amountOfWorkNeeded;
            int mostWorkNeeded = childIsMostWorkNeeded ? childWithMostWorkNeeded.MaxWorkNeededInSubtree : amountOfWorkNeeded;
            int numNodes = (int)ChildNodeInfos.Sum(x => x.NumNodes) + 1;
            uint numSubtreeValues = (uint)ChildNodeInfos.Sum(x => x.NumSubtreeValues) + (uint)netPendingChanges;
            byte maxDepth = ChildNodeInfos.Max(x => x.MaxDepth);
            return new NodeInfo<TKey>(thisNodeID, numNodes, numSubtreeValues, amountOfWorkNeeded, mostWorkNeeded, depth, maxDepth, true, ChildNodeInfos.First().FirstExclusive, ChildNodeInfos.Last().LastInclusive);
        }

        #endregion

        #region Children information

        public override List<long> GetChildrenIDs()
        {
            return ChildNodeInfos.Select(x => x.NodeID).ToList();
        }

        /// <summary>
        /// The highest value possible in each of the lower nodes, other than the top one (which can contain any value). The number of items in this array is equal to NumNodeReferences - 1. These values will stay the same over the lifetime of this node. Note that we use both key and ID to split, since there could be many items with the same key. 
        /// </summary>
        public KeyAndID<TKey>? SplitValue(int index)
        {
            return ChildNodeInfos[index].LastInclusive;
        }

        /// <summary>
        /// Enumerates the highest value possible in each node but the last (which has no maximum value).
        /// </summary>
        /// <returns></returns>
        private IEnumerable<KeyAndID<TKey>?> GetSplitValues()
        {
            foreach (var cni in ChildNodeInfos)
                if (cni.LastInclusive != null)
                    yield return cni.LastInclusive;
        }

        /// <summary>
        /// Enumerate the indices corresponding to children of this node containing the respective sorted items.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public IEnumerable<byte> GetChildrenIndices(IEnumerable<KeyAndID<TKey>> sortedItems)
        {
            var itemsEnumerator = sortedItems.GetEnumerator();
            bool anotherItem = itemsEnumerator.MoveNext();
            byte child = 0;
            foreach (var splitValue in GetSplitValues())
            {
                while (anotherItem && itemsEnumerator.Current <= splitValue)
                {
                    yield return child;
                    anotherItem = itemsEnumerator.MoveNext();
                    if (!anotherItem)
                        yield break;
                }
                child++;
            }
            // Account for children in last node.
            while (anotherItem)
            {
                yield return (byte) (TreeStructure.NumChildrenPerInternalNode - 1);
                anotherItem = itemsEnumerator.MoveNext();
            }
        }

        /// <summary>
        /// Identifies the IDs, child locations, and deletion instruction for each pending change.
        /// </summary>
        /// <param name="sortedPendingChanges"></param>
        /// <returns></returns>
        public IEnumerable<PendingChangeEffect> GetPendingChangeEffects(IEnumerable<PendingChange<TKey>> sortedPendingChanges)
        {
            var children = GetChildrenIndices(sortedPendingChanges.Select(x => x.Item));
            return sortedPendingChanges.Zip(children, (pc, c) => new PendingChangeEffect(pc.Item.ID, c, pc.Delete));
        }

        public bool ItemIsInChild(KeyAndID<TKey> item, byte child)
        {
            return item.IsInRange(ChildNodeInfos[child].FirstExclusive, ChildNodeInfos[child].LastInclusive);
        }

        #endregion

        #region Changes

        public async Task<CountedInternalNode<TKey>> ReplaceChildInfo(long replacementID, NodeInfo<TKey> replacementChildNodeInfo, TemporarilyInMemoryBlob<Guid> temporaryUintSetStorage)
        {
            // Note: We don't need to actually update any UintSets. Changing a child never changes the parent's uint set, because the parent has already processed any changes.
            int childNodeIndex = Enumerable.Range(0, TreeStructure.NumChildrenPerInternalNode).Where(x => ChildNodeInfos[x].LastInclusive == replacementChildNodeInfo.LastInclusive).First();
            return await ReplaceChildInfo(replacementID, childNodeIndex, replacementChildNodeInfo, NodeForDescendantsToInheritFrom, NodeForDescendantDeltaSetsToBuildOn, temporaryUintSetStorage);
        }

        public async Task<CountedInternalNode<TKey>> ReplaceChildInfo(long replacementID, int childNodeIndex, NodeInfo<TKey> replacementChildNodeInfo, long? nodeForDescendantsToInheritFrom, long? nodeForDescendantDeltaSetsToBuildOn, TemporarilyInMemoryBlob<Guid> temporaryUintSetStorage)
        {
            var replacementChildInfos = ChildNodeInfos.Select(x => x.Clone()).ToArray();
            replacementChildInfos[childNodeIndex] = replacementChildNodeInfo;
            CountedInternalNode<TKey> node;
            if (TreeStructure.StoreUintSets)
                node = await CountedInternalNode<TKey>.GetCountedInternalNodeWithUintSet(replacementChildInfos, MainBuffer.AllPendingChanges(), replacementID, Depth, TreeStructure, temporaryUintSetStorage, nodeForDescendantsToInheritFrom, nodeForDescendantDeltaSetsToBuildOn, null); // will not actually trigger creation of new UintSet storage -- will just save info
            else
                node = new CountedInternalNode<TKey>(replacementChildInfos, MainBuffer.AllPendingChanges(), replacementID, Depth, TreeStructure);
            return node;
        }

        /// <summary>
        /// Replace the node with one or more nodes incorporating pending changes. 
        /// </summary>
        /// <param name="nextIDToUse"></param>
        /// <param name="changesToIncorporate"></param>
        /// <returns></returns>
        public override async Task<List<CountedNode<TKey>>> FlushToNode(long nextIDToUse, PendingChangesCollection<TKey> changesToIncorporate)
        {
            // We are not flushing results from this node to lower nodes at this point, but rather just completing the flush to this node. So we just create an identical node, but with this additional set of pending changes incorporated.
            CombinedInternalNodeBuffer<TKey> combinedBuffer = new CombinedInternalNodeBuffer<TKey>(this, changesToIncorporate);
            var combinedPendingChanges = combinedBuffer.AllPendingChanges();
            CountedInternalNode<TKey> replacementNode = await CountedInternalNode<TKey>.GetCountedInternalNodeWithUintSet(ChildNodeInfos, combinedPendingChanges, nextIDToUse, Depth, TreeStructure, UintSetStorage, NodeForDescendantsToInheritFrom, NodeForDescendantDeltaSetsToBuildOn, changesToIncorporate); // will trigger updating of the UintSet
            return new List<CountedNode<TKey>>() { replacementNode };
        }

        /// <summary>
        /// Get a version of this node excluding items that are to be flushed to a particular child of this node. This is used in the process of completing a flush.
        /// </summary>
        /// <param name="nextIDToUse"></param>
        /// <param name="childIndex"></param>
        /// <returns></returns>
        public async Task<Tuple<CountedInternalNode<TKey>, PendingChangesCollection<TKey>>> GetNodeMinusChangesFlushedToChild(long nextIDToUse, int childIndex)
        {
            PendingChangesCollection<TKey> flushedChanges = MainBuffer.PendingChangesAtNodeIndex(childIndex);
            int belowIndex = MainBuffer.NumPendingChangesBelowNodeIndex(childIndex);
            int atIndex = MainBuffer.NumPendingChangesAtNodeIndex(childIndex);
            int aboveIndex = MainBuffer.NumPendingChangesAboveNodeIndex(childIndex);
            var remainingChanges = MainBuffer.AllPendingChanges().AsEnumerable().SkipItemsInMiddle(belowIndex, atIndex).ToArray();

            CountedInternalNode<TKey> temporaryReplacementNode = await CountedInternalNode<TKey>.GetCountedInternalNodeWithUintSet(ChildNodeInfos, new PendingChangesCollection<TKey>(remainingChanges, false), nextIDToUse, Depth, TreeStructure, UintSetStorage, NodeForDescendantsToInheritFrom, NodeForDescendantDeltaSetsToBuildOn, null); // will not actually allocate any new storage
            return new Tuple<CountedInternalNode<TKey>, PendingChangesCollection<TKey>>(temporaryReplacementNode, flushedChanges);

        }

        /// <summary>
        /// Returns a list of child nodes (specified by ID plus whether they have been created) that should be flushed to, because the number of changes in this node for those children indicate that each of those children should receive a a number of pending changes exceeding the work threshold.
        /// </summary>
        /// <param name="workThreshold"></param>
        /// <returns></returns>
        public override IEnumerable<Tuple<long, bool>> GetNodesToFlushTo(int workThreshold)
        {
            var pendingChangesAtNodes = Enumerable.Range(0, TreeStructure.NumChildrenPerInternalNode).Select(x => MainBuffer.NumPendingChangesAtNodeIndex(x)).ToList();
            var sufficientWorkNeeded = Enumerable.Range(0, TreeStructure.NumChildrenPerInternalNode).Where(x => pendingChangesAtNodes[x] > workThreshold).OrderByDescending(x => x); // note that we use > rather than >= so that if workThreshold = 0, we will not return every node.
            return sufficientWorkNeeded.Select(x => new Tuple<long, bool>(ChildNodeInfos[x].NodeID, ChildNodeInfos[x].Created));
        }

        /// <summary>
        /// Returns a list of child nodes that should be flushed from (to their own children), because they have work needed exceeding the work threshold.
        /// </summary>
        /// <param name="workThreshold"></param>
        /// <returns></returns>
        public override IEnumerable<long> GetNodesToFlushFrom(int workThreshold)
        {
            var childrenWithWork = Enumerable.Range(0, TreeStructure.NumChildrenPerInternalNode).Where(x => ChildNodeInfos[x].MaxWorkNeededInSubtree > workThreshold).ToList();
            return childrenWithWork.Select(x => ChildNodeInfos[x].NodeID);
        }

        /// <summary>
        /// Returns a new version of this node replacing a child with the specified replacement information. The pending changes for this node will not be changed.
        /// </summary>
        /// <param name="previousNodeID"></param>
        /// <param name="replacementNodeInfo"></param>
        /// <returns></returns>
        public CountedInternalNode<TKey> ReplaceChildNode(long previousNodeID, NodeInfo<TKey> replacementNodeInfo)
        {
            int nodeIndex = Enumerable.Range(0, TreeStructure.NumChildrenPerInternalNode).First(x => ChildNodeInfos[x].NodeID == previousNodeID);
            var childNodeInfos = ChildNodeInfos.Take(nodeIndex).Select(x => x.Clone()).ToList();
            childNodeInfos.Add(replacementNodeInfo);
            childNodeInfos.AddRange(ChildNodeInfos.Skip(nodeIndex + 1).Select(x => x.Clone()));
            var replacementNode = new CountedInternalNode<TKey>(childNodeInfos.ToArray(), MainBuffer.AllPendingChanges(), ID, NodeInfo.Depth, TreeStructure);
            return replacementNode;
        }

        #endregion

        #region Queries

        public async override Task<NodeResultBase<TKey>> ProcessQuery(NodeQueryBase<TKey> request)
        {
            request = ModifyQueryPendingChanges(request);
            NodeQueryLinearBase<TKey> linearRequest = (NodeQueryLinearBase<TKey>)request;
            return await GetUintSetShortcutNodeResult(linearRequest) ?? await GetChildQueriesNodeResult(linearRequest);
        }

        private async Task<NodeResultLinearBase<TKey>> GetUintSetShortcutNodeResult(NodeQueryLinearBase<TKey> request)
        {
            // If we are returning a UintSet and all child items are included in a UintSet, then we can process this quickly simply by returning the UintSet.
            if (request.NodeResultType == QueryResultType.IDsAsBitSet && TreeStructure.StoreUintSets && request.AllItemsAreInRange(NodeInfo.FirstExclusive, NodeInfo.LastInclusive))
            {
                await SetUintSetStorage(StorageFactory.GetUintSetStorage());
                var uintSet = await GetUintSetWithChanges(request.PendingChanges);
                if (request.Filter == null || (request.Filter.SearchWithin == null && request.Filter.Superset == null))
                    return new NodeResultUintSet<TKey>(uintSet, request.IncludedIndices, uintSet.Count, uintSet.Count);
                var superset = uintSet;
                if (request.Filter.Superset != null)
                    superset = superset.Intersect(request.Filter.Superset);
                var filteredSet = uintSet.Intersect(request.Filter.SearchWithin ?? request.Filter.Superset);
                return new NodeResultUintSet<TKey>(filteredSet, request.IncludedIndices, filteredSet.Count, superset.Count);
            }
            else
                return null;
        }

        private async Task<NodeResultLinearBase<TKey>> GetChildQueriesNodeResult(NodeQueryLinearBase<TKey> request)
        {
            // If we are doing a filtered search, then we must load the node UintSet, since this will be used to figure out how to adjust skip/take for each child.
            UintSetWithLoc nodeUintSetWithLoc = null;
            if (request.Filter?.SearchWithin != null)
            {
                await SetUintSetStorage(StorageFactory.GetUintSetStorage());
                nodeUintSetWithLoc = await GetUintSetWithLoc();
            }
            // We need to filter the pending changes that are part of the request, both by whatever is in the superset and whatever is in the filtered set. This will allow us to adjust our information on how many items are in each child accurately.
            InternalNodeBuffer<TKey> supersetRequestBuffer, filteredSetRequestBuffer;
            GetFilteredRequestBuffers(request, out supersetRequestBuffer, out filteredSetRequestBuffer);
            // We need to go through the child nodes to identify all of those that might be necessary to process this search, given the skip/take request and the number of items in the children.
            uint filteredMatches, supersetMatches;
            IEnumerable<IncludedIndices> includedIndicesForChildNodes = GetIncludedIndicesForChildNodes(request, nodeUintSetWithLoc, supersetRequestBuffer, filteredSetRequestBuffer, out filteredMatches, out supersetMatches);
            // Create a combined internal node buffer, including both the pending changes already stored in the node's main buffer and those sent along specifically with the request (because they haven't been stored in the node yet but need to be sent to child queries). We can't just send the filtered buffer, because then we wouldn't be able to calculate rank of items in leaf nodes correctly.
            CombinedInternalNodeBuffer<TKey> combinedBuffer = new CombinedInternalNodeBuffer<TKey>(this, supersetRequestBuffer.PendingChanges);
            // Generate the child queries based on the combined buffer.
            List<NodeQueryBase<TKey>> childQueries = GetChildQueries(request, combinedBuffer, includedIndicesForChildNodes);
            return new NodeResultLinearFurtherQueries<TKey>(request.IncludedIndices, childQueries, filteredMatches, supersetMatches);
        }

        private void GetFilteredRequestBuffers(NodeQueryLinearBase<TKey> request, out InternalNodeBuffer<TKey> supersetRequestBuffer, out InternalNodeBuffer<TKey> filteredSetRequestBuffer)
        {
            PendingChangesCollection<TKey> filteredSetRequestChanges, supersetRequestChanges;
            if (request.Filter?.Superset == null)
                supersetRequestChanges = request.PendingChanges;
            else
                supersetRequestChanges = new PendingChangesCollection<TKey>(request.PendingChanges, request.Filter.Superset);
            if (request.Filter?.SearchWithin == null)
                filteredSetRequestChanges = supersetRequestChanges;
            else
                filteredSetRequestChanges = new PendingChangesCollection<TKey>(request.PendingChanges, request.Filter.SearchWithin);
            supersetRequestBuffer = new InternalNodeBuffer<TKey>(TreeStructure.NumChildrenPerInternalNode, i => SplitValue(i), supersetRequestChanges);
            filteredSetRequestBuffer = new InternalNodeBuffer<TKey>(TreeStructure.NumChildrenPerInternalNode, i => SplitValue(i), filteredSetRequestChanges);
        }

        private List<NodeQueryBase<TKey>> GetChildQueries(NodeQueryLinearBase<TKey> request, CombinedInternalNodeBuffer<TKey> combinedBuffer, IEnumerable<IncludedIndices> includedIndicesForChildNodes)
        {
            List<NodeQueryBase<TKey>> childQueries = new List<NodeQueryBase<TKey>>();
            int searchRelativeNodeIndex = 0;
            uint skipRemaining = request.Skip; // when we skip a child, we will reduce this
            uint? takeRemaining = request.Take; // if this is non-null, once we plan to take results from a child, we will reduce this
            foreach (var childNodeIncludedIndices in includedIndicesForChildNodes)
            {
                var childNodeInfo = GetChildNodeInfo(searchRelativeNodeIndex, request.Ascending);
                if (childNodeIncludedIndices != null)
                {
                    uint minNumberSkipped, maxNumberSkipped, minNumberAvailableAfterSkipping, maxNumberAvailableAfterSkipping, minNumberReturned, maxNumberReturned;
                    request.RangeOfNumberExpectedToReturn(childNodeIncludedIndices.FirstIndexInFilteredSet, childNodeIncludedIndices.LastIndexInFilteredSet, childNodeInfo.FirstExclusive, childNodeInfo.LastInclusive, skipRemaining, takeRemaining, out minNumberSkipped, out maxNumberSkipped, out minNumberAvailableAfterSkipping, out maxNumberAvailableAfterSkipping, out minNumberReturned, out maxNumberReturned);
                    if (maxNumberReturned > 0)
                    {
                        NodeQueryLinearBase<TKey> childQuery = GetChildQuery(request, combinedBuffer, searchRelativeNodeIndex, skipRemaining, takeRemaining, childNodeIncludedIndices, childNodeInfo, maxNumberAvailableAfterSkipping);
                        childQueries.Add(childQuery);
                    }
                    // Ensure that we return a sufficient number of results, by minimizing skips and maximizing takes given uncertainty.
                    skipRemaining -= maxNumberSkipped;
                    if (takeRemaining != 0)
                        takeRemaining -= minNumberReturned;
                    if (skipRemaining == 0 && takeRemaining == 0) // note that if Take == null, then we will never break, because we want to keep taking.
                        break;
                }
                searchRelativeNodeIndex++;
            }
            return childQueries;
        }

        private NodeQueryLinearBase<TKey> GetChildQuery(NodeQueryLinearBase<TKey> request, CombinedInternalNodeBuffer<TKey> combinedBuffer, int searchRelativeNodeIndex, uint skipRemaining, uint? takeRemaining, IncludedIndices childNodeIncludedIndices, NodeInfo<TKey> childNodeInfo, uint maxNumberAvailableAfterSkipping)
        {
            var absoluteNodeIndex = GetAbsoluteNodeIndex(searchRelativeNodeIndex, request.Ascending);
            var combinedPendingChangesAtNode = combinedBuffer.PendingChangesAtNodeIndex(absoluteNodeIndex);
            uint? numToTake;
            if (takeRemaining == null)
                numToTake = takeRemaining;
            else
                numToTake = Math.Min((uint)takeRemaining, maxNumberAvailableAfterSkipping);
            var childQuery = request.GenerateChildQuery(childNodeInfo.NodeID, childNodeInfo.Created, childNodeIncludedIndices, skipRemaining, numToTake, combinedPendingChangesAtNode);
            return childQuery;
        }

        private IEnumerable<IncludedIndices> GetIncludedIndicesForChildNodes(NodeQueryLinearBase<TKey> request, UintSetWithLoc nodeUintSetWithLoc, INodeBufferBaseMethods<TKey> supersetRequestBuffer, INodeBufferBaseMethods<TKey> filteredSetRequestBuffer, out uint filteredMatches, out uint supersetMatches)
        {
            List<Tuple<uint, uint>> supersetIndexRanges = GetIncludedIndicesForNodesStartingAtZero(supersetRequestBuffer, request.Ascending, nodeUintSetWithLoc, request.Filter?.Superset).ToList();
            List<Tuple<uint, uint>> filteredSetIndexRanges;
            if (request.Filter?.Superset == request.Filter?.SearchWithin || request.Filter?.SearchWithin == null)
                filteredSetIndexRanges = supersetIndexRanges;
            else
                filteredSetIndexRanges = GetIncludedIndicesForNodesStartingAtZero(filteredSetRequestBuffer, request.Ascending, nodeUintSetWithLoc, request.Filter?.SearchWithin).ToList();
            IEnumerable<IncludedIndices> nodeIndexRanges = 
                supersetIndexRanges.Zip(
                filteredSetIndexRanges, 
                (s, f) => (s == null || f == null) ? null : 
                    new IncludedIndices( // We always number from the first index in THIS node.
                        request.IncludedIndices.FirstIndexInSuperset + s.Item1, 
                        request.IncludedIndices.FirstIndexInSuperset + s.Item2, 
                        request.IncludedIndices.FirstIndexInFilteredSet + f.Item1,
                        request.IncludedIndices.FirstIndexInFilteredSet + f.Item2
                        ));
            filteredMatches = filteredSetIndexRanges.LastOrDefault()?.Item2 ?? 0;
            supersetMatches = supersetIndexRanges.LastOrDefault()?.Item2 ?? 0;
            return nodeIndexRanges;
        }

        #endregion

        #region Node index query utilities

        /// <summary>
        /// Returns the absolute node index, based on the search relative node index. For example, if the search-relative node index is 0 and the search is descending, this will return the node index of the last node.
        /// </summary>
        /// <param name="searchRelativeNodeIndex"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        private int GetAbsoluteNodeIndex(int searchRelativeNodeIndex, bool ascending)
        {
            if (ascending)
                return searchRelativeNodeIndex;
            else
                return TreeStructure.NumChildrenPerInternalNode - searchRelativeNodeIndex - 1;
        }

        /// <summary>
        /// Gets child node information for a node based on a search-relative node index (where 0 is the node containing the earliest possible results to return).
        /// </summary>
        /// <param name="searchRelativeNodeIndex"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public NodeInfo<TKey> GetChildNodeInfo(int searchRelativeNodeIndex, bool ascending)
        {
            return ChildNodeInfos[GetAbsoluteNodeIndex(searchRelativeNodeIndex, ascending)];
        }

        /// <summary>
        /// Enumerates the index ranges of items for all child nodes, counting the first child as starting with index 0. In a descending search, this starts with the first node that would be returned (i.e., the last node), and the enumeration begins with the range of indices for the items in that node.
        /// </summary>
        /// <param name="filteredRequestBuffer"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<uint, uint>> GetIncludedIndicesForNodesStartingAtZero(INodeBufferBaseMethods<TKey> filteredRequestBuffer, bool ascending, UintSetWithLoc nodeUintSetWithLoc, UintSet filter)
        {
            INodeBufferBaseMethods<TKey> bufferModifyingCumulator;
            CumulativeItemsCounter<TKey> cumulator;
            if (filter == null) // equivalent to a filter with all items present
            {
                cumulator = Cumulator; // this default Cumulator excludes the items in the main buffer. This isn't a problem, because the main buffer can tell us the number of items at a particular index, and we need to combine the main buffer with the request buffer anyway.
                bufferModifyingCumulator = new CombinedInternalNodeBuffer<TKey>(this, filteredRequestBuffer); // both the main buffer and the request buffer (which won't really be filtered in this case) modify the cumulator. 
            }
            else
            {
                // We will use the bitset information to count the number of children in each node. This will INCLUDE the main buffer.
                uint[] numItemsInSubtree = nodeUintSetWithLoc.CountForEachChild((byte)TreeStructure.NumChildrenPerInternalNode, filter); // note that this does NOT include items in the request buffer, but it will include items in the main buffer
                cumulator = new CumulativeItemsCounter<TKey>(i => numItemsInSubtree[i]);
                cumulator.CalculateCumulativeItemsInSubtreeIfNecessary(TreeStructure.NumChildrenPerInternalNode);
                bufferModifyingCumulator = filteredRequestBuffer; // Because the cumulator takes into account the main buffer, we need add only the filtered request buffer.
            }

            // Much of this code is driven by the following edge case:
            // If a node, counting the buffer, has a negative expected number of items, then we must make an adjustment.
            // Suppose node 0 has 0 items but 1 pending delete in the buffer. So, NumItemsInSubtreeIncludingBuffer < 0. We won't enumerate an index range at all in this case.
            // Then, node 1 has 1 item and 0 pending items. We should return (0,0), indicating the search index range of the item in node 1.
            // Then, our calculation of cumulative numbers will be off for subsequent items. CumulativeItemsInSubtreeBelowIndexIncludingBuffer will return -1, so we end up getting a cumulative range of (-1, -1). 
            // If we add the cumulativeAdjustmentForNegativeNodes, then we get back to (0, 0).

            int cumulativeAdjustmentForNegativeNodes = 0; 
            for (int i = 0; i < TreeStructure.NumChildrenPerInternalNode; i++)
            {
                var indexRange = GetIncludedIndicesForNodeStartingAtZero(cumulator, bufferModifyingCumulator, i, ascending);
                if (indexRange == null)
                {
                    int absoluteNodeIndex = GetAbsoluteNodeIndex(i, ascending);
                    long itemsAtNode = cumulator.NumItemsInSubtreeIncludingBuffer(bufferModifyingCumulator, absoluteNodeIndex);
                    if (itemsAtNode < 0)
                        cumulativeAdjustmentForNegativeNodes += (int)(0 - itemsAtNode);
                    yield return null;
                }
                else // i.e., if there are some expected items in the node
                {
                    // Here's where we adjust for the negative nodes. Note that the incoming index ranges are longs (to take into account the possibility of negative expectations). Once we adjust for cumulative negative nodes, we can cast back to uint.
                    yield return new Tuple<uint, uint>((uint) (indexRange.Item1 + cumulativeAdjustmentForNegativeNodes), (uint) (indexRange.Item2 + cumulativeAdjustmentForNegativeNodes));
                }
            }
        }

        /// <summary>
        /// Returns the index range for a node (counting the first item in the first node as index zero). The nodeIndex is the order of the node in the search (so in a descending search, Z might be the zero index).
        /// </summary>
        /// <param name="filteredRequestBuffer"></param>
        /// <param name="searchRelativeNodeIndex"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        private Tuple<long, long> GetIncludedIndicesForNodeStartingAtZero(CumulativeItemsCounter<TKey> cumulatorExcludingRequestBuffer, INodeBufferBaseMethods<TKey> filteredRequestBuffer, int searchRelativeNodeIndex, bool ascending)
        {
            if (ascending)
                return GetIncludedIndicesForNodeAscendingStartingAtZero(cumulatorExcludingRequestBuffer, filteredRequestBuffer, searchRelativeNodeIndex);
            else
                return GetIncludedIndicesForNodeDescendingStartAtZero(cumulatorExcludingRequestBuffer, filteredRequestBuffer, searchRelativeNodeIndex);
        }

        /// <summary>
        /// Returns the index range for a particular node, taking into account the buffer, assuming an ascending search.
        /// </summary>
        /// <param name="filteredRequestBuffer"></param>
        /// <param name="nodeIndex"></param>
        /// <returns></returns>
        private Tuple<long, long> GetIncludedIndicesForNodeAscendingStartingAtZero(CumulativeItemsCounter<TKey> cumulator, INodeBufferBaseMethods<TKey> filteredRequestBuffer, int nodeIndex)
        {
            long itemsBeforeNode = cumulator.CumulativeItemsInSubtreeBelowIndexIncludingBuffer(filteredRequestBuffer, nodeIndex);
            long itemsAtNode = cumulator.NumItemsInSubtreeIncludingBuffer(filteredRequestBuffer, nodeIndex);
            if (itemsAtNode <= 0)
                return null;
            return new Tuple<long, long>(itemsBeforeNode, (itemsBeforeNode + itemsAtNode - 1));
        }

        /// <summary>
        /// Returns the search-relative index range for a particular node in a descending search, based on the absolute node index (where 0 is the last node in a descending search)
        /// </summary>
        /// <param name="filteredRequestBuffer"></param>
        /// <param name="absoluteNodeIndex"></param>
        /// <returns></returns>
        private Tuple<long, long> GetIncludedIndicesForNodeDescendingStartingAtZero_FromAbsoluteNodeIndex(CumulativeItemsCounter<TKey> cumulator, INodeBufferBaseMethods<TKey> filteredRequestBuffer, int absoluteNodeIndex)
        {
            // So now, node 0 (the last in a descending search)
            long itemsAboveNode = cumulator.CumulativeItemsInSubtreeAboveIndexIncludingBuffer(filteredRequestBuffer, absoluteNodeIndex); // note that this is items ABOVE index, since this is a descending search
            long itemsAtNode = cumulator.NumItemsInSubtreeIncludingBuffer(filteredRequestBuffer, absoluteNodeIndex);
            if (itemsAtNode <= 0)
                return null;
            return new Tuple<long, long>(itemsAboveNode, (itemsAboveNode + itemsAtNode - 1));
        }

        /// <summary>
        /// Returns the search-relative index range for a particular node in a descending search, based on the absolute node index (where 0 is the first node in a descending search)
        /// </summary>
        /// <param name="filteredRequestBuffer"></param>
        /// <param name="searchRelativeNodeIndex"></param>
        /// <returns></returns>
        private Tuple<long, long> GetIncludedIndicesForNodeDescendingStartAtZero(CumulativeItemsCounter<TKey> cumulator, INodeBufferBaseMethods<TKey> filteredRequestBuffer, int searchRelativeNodeIndex)
        {
            return GetIncludedIndicesForNodeDescendingStartingAtZero_FromAbsoluteNodeIndex(cumulator, filteredRequestBuffer, GetAbsoluteNodeIndex(searchRelativeNodeIndex, false));
        }

        /// <summary>
        /// Returns the value range for a particular child node (i.e., the range of values and IDs included in the node).
        /// </summary>
        /// <param name="searchRelativeNodeIndex"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        private Tuple<KeyAndID<TKey>?, KeyAndID<TKey>?> GetValueRangeForNode(int searchRelativeNodeIndex, bool ascending)
        {
            if (!ascending)
                searchRelativeNodeIndex = TreeStructure.NumChildrenPerInternalNode - searchRelativeNodeIndex - 1;
            if (searchRelativeNodeIndex == 0)
                return new Tuple<KeyAndID<TKey>?, KeyAndID<TKey>?>(null, SplitValue(searchRelativeNodeIndex));
            else if (searchRelativeNodeIndex == TreeStructure.NumChildrenPerInternalNode - 1)
                return new Tuple<KeyAndID<TKey>?, KeyAndID<TKey>?>(SplitValue(searchRelativeNodeIndex - 1), null);
            else
                return new Tuple<KeyAndID<TKey>?, KeyAndID<TKey>?>(SplitValue(searchRelativeNodeIndex - 1), SplitValue(searchRelativeNodeIndex));
        }

        #endregion

        #region Uintsets
        
        /// <summary>
        /// A unique identifier allowing access to a bit set reflecting all of the items in the node. This bit set can be used for generating filters quickly, without the need to visit all leaf nodes.
        /// </summary>
        public Guid UintSetContext => GetUintSetContext(TreeStructure.StoreUintSets, TreeStructure.UintSetStorageContext, ID);

        public override long? NodeForDescendantsToInheritFrom => InheritedMainSetNodeID ?? ID;
        public override long? NodeForDescendantDeltaSetsToBuildOn => InheritedDeltaSetsNodeID ?? ID;

        public Guid InheritedContext(long? inheritedNodeID) => inheritedNodeID == null ? Guid.Empty : GetUintSetContext(TreeStructure.StoreUintSets, TreeStructure.UintSetStorageContext, (long)inheritedNodeID);

        public Guid InheritedMainSetContext => InheritedContext(InheritedMainSetNodeID);
        public Guid InheritedDeltaSetsContext => InheritedContext(InheritedDeltaSetsNodeID);

        /// <summary>
        /// Compute a unique identifier allowing access to a bit set for this node.
        /// </summary>
        /// <param name="storeBitSets"></param>
        /// <param name="treeContext"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Guid GetUintSetContext(bool storeBitSets, Guid treeContext, long ID) => storeBitSets ? MD5HashGenerator.GetDeterministicGuid(new Tuple<Guid, long, string>(treeContext, ID, "NodeBits")) : Guid.Empty;

        [NonSerialized]
        public SplitUintSetAccess UintSetAccess;

        public IBlob<Guid> UintSetStorage => UintSetAccess?.Storage;

        public const int MaxItemsInCachedUintSet = 100;

        public override async Task SetUintSetStorage(IBlob<Guid> uintSetStorage)
        {
            await SetupUintSetStorage(uintSetStorage, null, null);
        }

        private async Task SetupUintSetStorage(IBlob<Guid> uintSetStorage, long? inheritedNodeID, long? deltaSetsNodeID, PendingChangesCollection<TKey> pendingChangesToAddToUintSetStorage = null)
        {
            if (uintSetStorage == null)
                return;
            // Change the storage if necessary (i.e., switching from temporary to permanent UintSetStorage)
            if (UintSetStorage != null && uintSetStorage != UintSetStorage)
                await UintSetAccess.UpdateStorage(uintSetStorage);
            // Create a UintSet from scratch, if not inherited.
            if (inheritedNodeID == null && InheritedMainSetNodeID == null)
                SetupUintSetStorage_NotInherited(uintSetStorage);
            else
                await SetupUintSetStorage_Inherited(uintSetStorage, inheritedNodeID, deltaSetsNodeID, pendingChangesToAddToUintSetStorage);
        }

        private async Task SetupUintSetStorage_Inherited(IBlob<Guid> uintSetStorage, long? inheritedNodeID, long? deltaSetsNodeID, PendingChangesCollection<TKey> pendingChangesToAddToUintSetStorage)
        {
            // CountedInternalNode is not strictly immutable, in part because C# does not allow asynchronous constructors, and we SetupUintSetStorage using async. Nonetheless, we are trying to follow an approximate immutability pattern with bitsets. So, we make sure that we set InheritedMainSetNodeID just once when creating CountedInternalNode. 
            if (inheritedNodeID != null)
            {
                // Make sure that we're setting InheritedMainSetNodeID for the first time. 
                if (InheritedMainSetNodeID != null)
                    throw new Exception("Inherited node ID has already been set.");
                InheritedMainSetNodeID = inheritedNodeID;
            }
            if (deltaSetsNodeID != null)
            {
                // Again, make sure that we haven't set it.
                if (InheritedDeltaSetsNodeID != null)
                    throw new Exception("Inherited delta sets node ID has already been set.");
                InheritedDeltaSetsNodeID = deltaSetsNodeID;
            }
            if (UintSetAccess == null)
                UintSetAccess = new SplitUintSetAccess(ID, UintSetContext, InheritedMainSetContext, InheritedDeltaSetsContext, InheritMainSetContext, InheritDeltaSetsContext, MaxItemsInCachedUintSet, true, uintSetStorage);
            else
                UintSetAccess.SetStorage(uintSetStorage); // We're changing the storage

            if (pendingChangesToAddToUintSetStorage != null && pendingChangesToAddToUintSetStorage.Any())
                await AddPendingChangesToInherited(pendingChangesToAddToUintSetStorage, deltaSetsNodeID);
        }

        private void SetupUintSetStorage_NotInherited(IBlob<Guid> uintSetStorage)
        {
            UintSetAccess = new SplitUintSetAccess(ID, UintSetContext, MaxItemsInCachedUintSet, true, uintSetStorage);
            InheritedMainSetNodeID = null;
            InheritedDeltaSetsNodeID = null;
            // At this point, the UintSet is empty, but it can be filled in with a call to Initialize.
        }

        public override async Task MakeNotInherited(long idOfNodeNotToInheritFrom)
        {
            if (InheritedMainSetNodeID == idOfNodeNotToInheritFrom)
                await UintSetAccess.MakeNotInherited(true);
            if (InheritedDeltaSetsNodeID == idOfNodeNotToInheritFrom)
                await UintSetAccess.MakeNotInherited(false);
            UpdateInherited();
        }

        /// <summary>
        /// Add pending changes to a UintSet that inherits from another UintSet. 
        /// </summary>
        /// <param name="pendingChanges"></param>
        /// <param name="deltaSetsNodeID"></param>
        /// <returns></returns>
        private async Task AddPendingChangesToInherited(PendingChangesCollection<TKey> pendingChanges, long? deltaSetsNodeID)
        {
            List<WUInt32> indicesToAdd, indicesToRemove;
            List<byte> locationsToAdd;
            DifferentiatePendingChanges(pendingChanges, ChildNodeInfos, out indicesToAdd, out locationsToAdd, out indicesToRemove);
            await UintSetAccess.Change(NodeInfo.NumSubtreeValues < MinBeforeUsingDeltaSet, indicesToRemove, indicesToAdd, locationsToAdd);
            UpdateInherited();
            if (!InheritMainSetContext && InheritDeltaSetsContext)
                throw new Exception("Internal error. Should not inherit delta sets context without inheriting main sets context.");
            UintSetInitialized = true;
        }

        public NodeQueryBase<TKey> ModifyQueryPendingChanges(NodeQueryBase<TKey> originalRequest)
        {
            return WithIncludedIndices(originalRequest);
        }

        public NodeQueryBase<TKey> WithIncludedIndices(NodeQueryBase<TKey> request)
        {
            if (Depth == 0)
                return request.WithRevisedPendingChanges(NodeInfo.NumSubtreeValues, request.PendingChanges); // will update IncludedIndices
            else
                return request;
        }

        private void UpdateInherited()
        {
            if (UintSetAccess != null)
            {
                if (!UintSetAccess.InheritMainSetContext)
                    InheritedMainSetNodeID = null;
                if (!UintSetAccess.InheritDeltaSetsContext)
                    InheritedDeltaSetsNodeID = null; 
            }
        }

        public async Task InitializeUintSetStorage(UintSetWithLoc uintSetWithLoc = null)
        {
            if (UintSetAccess == null)
                throw new Exception("Internal error. UintSetAccess must be set up.");
            if (UintSetInitialized)
                throw new Exception("Internal error. Already initialized.");
            await UintSetAccess.InitializeWithLoc(uintSetWithLoc ?? new UintSetWithLoc());
            UintSetInitialized = true;
        }

        [NonSerialized]
        UintSetWithLoc AlreadyLoadedUintSetWithLoc;
        [NonSerialized]
        UintSet AlreadyLoadedUintSet;

        public async Task<UintSetWithLoc> GetUintSetWithLoc()
        {
            if (AlreadyLoadedUintSetWithLoc == null)
                AlreadyLoadedUintSetWithLoc = await UintSetAccess.GetUintSetWithLoc();
            return AlreadyLoadedUintSetWithLoc;
        }

        public async Task<UintSet> GetUintSet()
        {
            if (AlreadyLoadedUintSetWithLoc != null)
                return AlreadyLoadedUintSetWithLoc.Set;
            if (AlreadyLoadedUintSet == null)
                AlreadyLoadedUintSet = await UintSetAccess.GetUintSet();
            return await UintSetAccess.GetUintSet();
        }

        public async Task<UintSet> GetUintSetWithChanges(PendingChangesCollection<TKey> pendingChanges)
        {
            var uintSet = await GetUintSet();
            if (pendingChanges != null && pendingChanges.Any())
            {
                List<WUInt32> indicesToAdd, itemsToRemove;
                List<byte> locationsToAdd;
                DifferentiatePendingChanges(pendingChanges, ChildNodeInfos, out indicesToAdd, out locationsToAdd, out itemsToRemove);
                uintSet.AddUints(indicesToAdd);
                uintSet.RemoveUints(itemsToRemove);
            }
            return uintSet;
        }

        /// <summary>
        /// Divides pending changes into indices/locations to add and indices to remove, eliminating redundancies.
        /// </summary>
        /// <param name="pendingChanges"></param>
        /// <param name="indicesToAdd"></param>
        /// <param name="locationsToAdd"></param>
        /// <param name="itemsToRemove"></param>
        private static void DifferentiatePendingChanges(PendingChangesCollection<TKey> pendingChanges, IEnumerable<NodeInfo<TKey>> childNodes, out List<WUInt32> indicesToAdd, out List<byte> locationsToAdd, out List<WUInt32> itemsToRemove)
        {
            indicesToAdd = new List<WUInt32>();
            locationsToAdd = new List<byte>();
            itemsToRemove = new List<WUInt32>();

            var childNodesEnumerator = childNodes.GetEnumerator();
            bool moreSpots = childNodesEnumerator.MoveNext();
            byte childIndex = 0;
            foreach (var pendingChange in pendingChanges.AsEnumerable())
            {
                while (moreSpots && pendingChange.Item > childNodesEnumerator.Current.LastInclusive && childNodesEnumerator.Current.LastInclusive != null) // note that we define null as lowest value in KeyAndID.
                {
                    moreSpots = childNodesEnumerator.MoveNext();
                    childIndex++;
                }
                if (pendingChange.Delete)
                    itemsToRemove.Add(pendingChange.Item.ID);
                else
                {
                    indicesToAdd.Add(pendingChange.Item.ID);
                    locationsToAdd.Add(childIndex);
                }
            }
        }

        //public long? InheritedNodeToDeleteMainSet => InheritMainSetContext == false ? InheritedNodeID : (long?)null;
        //public long? InheritedNodeToDeleteDeltaSets => InheritDeltaSetsContext == false ? InheritedNodeID : (long?)null;

        public override async Task DeleteUintSet()
        {
            await UintSetAccess.Delete();
        }

        #endregion
    }
}
