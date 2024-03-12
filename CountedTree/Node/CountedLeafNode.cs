using System;
using System.Collections.Generic;
using System.Linq;
using CountedTree.Core;
using CountedTree.PendingChanges;
using CountedTree.NodeResults;
using CountedTree.Queries;
using R8RUtilities;
using System.Threading.Tasks;
using CountedTree.UintSets;
using CountedTree.ByteUtilities;
using Utility;
using Lazinator.Core;
using Lazinator.Collections;

namespace CountedTree.Node
{

    public partial class CountedLeafNode<TKey> : CountedNode<TKey>, ICountedLeafNode<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        [NonSerialized]
        public IBlob<Guid> UintSetStorage; // The leaf node does not store bitsets, but we need this in case we are splitting into internal nodes

        public CountedLeafNode(List<KeyAndID<TKey>> items, long id, byte depth, TreeStructure treeStructure, KeyAndID<TKey>? firstExclusive, KeyAndID<TKey>? lastInclusive) : base(depth, treeStructure)
        {
            Items = new LazinatorList<KeyAndID<TKey>>(items, true);
            var maxItemsPerLeaf = TreeStructure?.MaxItemsPerLeaf ?? 1000;
            int amountOfWorkNeeded = Items.Count() > maxItemsPerLeaf ? Items.Count() - maxItemsPerLeaf : 0;
            NodeInfo = new NodeInfo<TKey>(id, 1, (uint)items.Count(), amountOfWorkNeeded, amountOfWorkNeeded, depth, depth, true, firstExclusive, lastInclusive);
        }

        public override Task SetUintSetStorage(IBlob<Guid> uintSetStorage)
        {
            UintSetStorage = uintSetStorage;
            return Task.CompletedTask;
        }

        public override List<long> GetChildrenIDs()
        {
            return new List<long>();
        }


        public override string ToString()
        {
            return $"{NodeInfo} Items ({Items.Count()}):{String.Join(", ", Items)}";
        }

        #region Changes

        public override async Task<List<CountedNode<TKey>>> FlushToNode(long nextIDToUse, PendingChangesCollection<TKey> changesToIncorporate)
        {
            List<CountedNode<TKey>> replacementNodes;
            
            List<KeyAndID<TKey>> replacementItems = changesToIncorporate.ApplyToCreateNewList(Items);
            if (replacementItems.Count() <= TreeStructure.MaxItemsPerLeaf)
            {
                replacementNodes = new List<CountedNode<TKey>>() { GetSingleNodeReplacement(nextIDToUse, replacementItems, NodeInfo.FirstExclusive, NodeInfo.LastInclusive) };
            }
            else
            {
                var multipleNodesReplacementResults = await GetMultipleNodesReplacement(nextIDToUse, replacementItems, NodeInfo.FirstExclusive, NodeInfo.LastInclusive, Depth);
                replacementNodes = multipleNodesReplacementResults.AdditionalNodes;
                replacementNodes.Add(multipleNodesReplacementResults.InPlaceReplacement); // we must add the new highest-level node to our list, since GetMultipleNodesReplacement returns it separately.
            }
            return replacementNodes;
        }

        internal virtual CountedLeafNode<TKey> GetSingleNodeReplacement(long nextIDToUse, List<KeyAndID<TKey>> replacementItems, KeyAndID<TKey>? firstExclusive, KeyAndID<TKey>? lastInclusive)
        {
            var replacement = GetCountedLeafNode(nextIDToUse, replacementItems, firstExclusive, lastInclusive, Depth);
            return replacement;
        }

        private async Task<MultipleNodesReplacementResults<TKey>> GetMultipleNodesReplacement(long nextIDToUseForChild, IEnumerable<KeyAndID<TKey>> replacementItems, KeyAndID<TKey>? firstExclusive, KeyAndID<TKey>? lastInclusive, byte depth)
        {
            List<CountedNode<TKey>> additionalNodes = new List<CountedNode<TKey>>();
            List<List<KeyAndID<TKey>>> itemsGrouped;
            KeyAndID<TKey>[] splitValues;
            GetItemGroupsAndSplitValues(firstExclusive, lastInclusive, depth, replacementItems.ToList(), TreeStructure.NumChildrenPerInternalNode, out itemsGrouped, out splitValues);
            var rangeForEachNode = GetRangeForNodesFromSplitValues(firstExclusive, lastInclusive, splitValues);
            int workNeededPerGroup = 0;
            if (itemsGrouped.First().Count() > TreeStructure.MaxItemsPerLeaf)
                workNeededPerGroup = itemsGrouped.First().Count() - TreeStructure.MaxItemsPerLeaf;
            long[] leafIDs = Enumerable.Range(0, TreeStructure.NumChildrenPerInternalNode)
                .Select(x => nextIDToUseForChild + (uint)x).ToArray();
            List<CountedNode<TKey>> childNodes = Enumerable.Range(0, itemsGrouped.Count())
                .Select(i => (CountedNode<TKey>)GetCountedLeafNode(leafIDs[i], itemsGrouped[i].ToList(), rangeForEachNode[i].Item1, rangeForEachNode[i].Item2, (byte)(depth + 1)))
                .ToList();
            // It's possible that some nodes could STILL be too big. So, we use recursion if necessary. 
            var childrenTooBig = childNodes.Where(x => x.NodeInfo.NumSubtreeValues > TreeStructure.MaxItemsPerLeaf).ToList();
            var replacementsForTooBigChildren = new List<CountedNode<TKey>>();
            nextIDToUseForChild += TreeStructure.NumChildrenPerInternalNode;
            foreach (var bigChild in childrenTooBig)
            {
                var recursiveResult = await GetMultipleNodesReplacement(nextIDToUseForChild, ((CountedLeafNode<TKey>)bigChild).Items, bigChild.NodeInfo.FirstExclusive, bigChild.NodeInfo.LastInclusive, (byte) (depth + 1));
                CountedInternalNode<TKey> replacementForChild = recursiveResult.InPlaceReplacement;
                List<CountedNode<TKey>> grandchildren = recursiveResult.AdditionalNodes;
                int index = childNodes.IndexOf(bigChild);
                childNodes[index] = replacementForChild;
                additionalNodes.AddRange(grandchildren);
                nextIDToUseForChild = replacementForChild.ID + 1; // the replacement will always have the highest number.
            }
            // It's also possible that some nodes could have no items -- so it's not necessary to actually create them
            var emptyChildren = childNodes.Where(x => x.NodeInfo.NumSubtreeValues == 0).ToList();
            foreach (var emptyChild in emptyChildren)
            {
                int index = childNodes.IndexOf(emptyChild);
                childNodes[index].NodeInfo.Created = false;
            }
            // create the replacement internal node
            CountedInternalNode<TKey> inPlaceReplacement = null;
            if (TreeStructure.StoreUintSets)
                inPlaceReplacement = await GetInPlaceReplacement_WithUintSetLoc(nextIDToUseForChild, childNodes, replacementItems.ToList(), depth);
            else
                inPlaceReplacement = GetInPlaceReplacement_NoUintSetLoc(nextIDToUseForChild, childNodes, depth);
            // And add the child nodes that have been created
            childNodes = childNodes.Where(x => x.NodeInfo.Created).ToList();
            additionalNodes.AddRange(childNodes);
            return new MultipleNodesReplacementResults<TKey>() { AdditionalNodes = additionalNodes, InPlaceReplacement = inPlaceReplacement };
        }

        internal async virtual Task<CountedInternalNode<TKey>> GetInPlaceReplacement_WithUintSetLoc(long nextIDToUseForChild, List<CountedNode<TKey>> childNodes, List<KeyAndID<TKey>> itemsToIncludeInUintSet, byte depth)
        {
            UintSetWithLoc uintSetWithLoc = ConstructUintSetWithLoc(childNodes, itemsToIncludeInUintSet, !TreeStructure.StoreUintSetLocs, TreeStructure.NumChildrenPerInternalNode <= 16);
            CountedInternalNode<TKey> inPlaceReplacement = await CountedInternalNode<TKey>.GetCountedInternalNodeWithUintSet(childNodes.Select(x => x.NodeInfo).ToArray(), new PendingChangesCollection<TKey>(), nextIDToUseForChild, depth, TreeStructure, UintSetStorage, uintSetWithLoc);
            return inPlaceReplacement;
        }

        private static UintSetWithLoc ConstructUintSetWithLoc(List<CountedNode<TKey>> childNodes, List<KeyAndID<TKey>> itemsToIncludeInUintSet, bool skipLocations, bool noMoreThan16ChildrenPerNode)
        {
            IEnumerable<KeyAndID<TKey>?> lastInclusives = childNodes.Select(x => x.NodeInfo.LastInclusive);
            return UintSetWithLoc.ConstructFromItems(itemsToIncludeInUintSet, lastInclusives, skipLocations, noMoreThan16ChildrenPerNode);
        }

        internal virtual CountedInternalNode<TKey> GetInPlaceReplacement_NoUintSetLoc(long newInternalNodeID, IEnumerable<CountedNode<TKey>> childNodes, byte depth)
        {
            return new CountedInternalNode<TKey>(childNodes.Select(x => x.NodeInfo).ToArray(), new PendingChangesCollection<TKey>(), newInternalNodeID, depth, TreeStructure);
        }

        internal virtual CountedLeafNode<TKey> GetCountedLeafNode(long id, List<KeyAndID<TKey>> items, KeyAndID<TKey>? firstExclusive, KeyAndID<TKey>? lastInclusive, byte depth)
        {
            var clone = new CountedLeafNode<TKey>(items, id, depth, TreeStructure, firstExclusive, lastInclusive);
            return clone;
        }

        private static List<Tuple<KeyAndID<TKey>?, KeyAndID<TKey>?>> GetRangeForNodesFromSplitValues(KeyAndID<TKey>? firstExclusive, KeyAndID<TKey>? lastInclusive, KeyAndID<TKey>[] splitValues)
        {
            return Enumerable.Range(0, splitValues.Count() + 1).Select(x => new Tuple<KeyAndID<TKey>?, KeyAndID<TKey>?>(x == 0 ? firstExclusive : splitValues[x - 1], x == splitValues.Count() ? lastInclusive : splitValues[x])).ToList();
        }

        internal virtual void GetItemGroupsAndSplitValues(KeyAndID<TKey>? firstExclusive, KeyAndID<TKey>? lastInclusive, byte depth, List<KeyAndID<TKey>> itemsWithPendingChanges, int numChildrenPerInternalNode, out List<List<KeyAndID<TKey>>> itemsGrouped, out KeyAndID<TKey>[] splitValues)
        {
            bool divideEvenly = firstExclusive != null && lastInclusive != null && DivideEvenly((KeyAndID<TKey>)firstExclusive, (KeyAndID<TKey>)lastInclusive, itemsWithPendingChanges, depth);
            if (divideEvenly)
                GetItemGroupsAndSplitValues_DividingEvenly((KeyAndID<TKey>)firstExclusive, (KeyAndID<TKey>) lastInclusive, depth, itemsWithPendingChanges, numChildrenPerInternalNode, out itemsGrouped, out splitValues);
            else
                GetItemGroupsAndSplitValues_BasedOnValues(itemsWithPendingChanges, numChildrenPerInternalNode, out itemsGrouped, out splitValues);
        }

        internal virtual bool TypeDividesEvenly()
        {
            Type t = typeof(TKey);
            return t == typeof(float) || t == typeof(double);
        }

        internal virtual bool DivideEvenly(KeyAndID<TKey> firstExclusive, KeyAndID<TKey> lastInclusive, List<KeyAndID<TKey>> itemsWithPendingChanges, byte depth)
        {
            if (TreeStructure.SplitRangeEvenly)
            {
                bool canDivideEvenly = TypeDividesEvenly() && !itemsWithPendingChanges.First().Key.Equals(itemsWithPendingChanges.Last().Key);
                if (!canDivideEvenly)
                    return false;
            }
            return TreeStructure.SplitRangeEvenly;
        }

        internal virtual void GetItemGroupsAndSplitValues_DividingEvenly(KeyAndID<TKey> firstExclusive, KeyAndID<TKey> lastInclusive, byte depth, List<KeyAndID<TKey>> itemsWithPendingChanges, int numChildrenPerInternalNode, out List<List<KeyAndID<TKey>>> itemsGrouped, out KeyAndID<TKey>[] splitValues)
        {
            splitValues = GetEvenlyDividedSplits(firstExclusive, lastInclusive, depth, numChildrenPerInternalNode).Select(x => new KeyAndID<TKey>(x, uint.MaxValue)).ToArray();
            var splitValuesPlusMaximum = splitValues.Concat(new KeyAndID<TKey>[] { lastInclusive }).ToArray();
            itemsGrouped = itemsWithPendingChanges.SplitValues(splitValuesPlusMaximum).ToList();
        }

        internal virtual IEnumerable<TKey> GetEvenlyDividedSplits(KeyAndID<TKey> firstExclusive, KeyAndID<TKey> lastInclusive, byte depth, int numChildrenPerInternalNode)
        {
            var splitter = GetRangeSplitter.GetSplitter<TKey>();
            var splits = splitter.GetInteriorSplits(firstExclusive.Key, lastInclusive.Key, numChildrenPerInternalNode).ToList();
            if (splits.Count() != numChildrenPerInternalNode - 1)
                throw new Exception("Internal except. Wrong number of split values.");
            return splits;
        }
            

        internal virtual void GetItemGroupsAndSplitValues_BasedOnValues(List<KeyAndID<TKey>> itemsWithPendingChanges, int numChildrenPerInternalNode, out List<List<KeyAndID<TKey>>> itemsGrouped, out KeyAndID<TKey>[] splitValues)
        {
            itemsGrouped = itemsWithPendingChanges.Split(numChildrenPerInternalNode).ToList();
            splitValues = itemsGrouped.Take(itemsGrouped.Count() - 1).Select(x => x.Last()).ToArray();
        }

        public override IEnumerable<Tuple<long, bool>> GetNodesToFlushTo(int minWork)
        {
            yield break; // there are no existing nodes to flush to -- we flush to new nodes.
        }

        public override IEnumerable<long> GetNodesToFlushFrom(int minWork)
        {
            yield break; // we don't have work to do with leaf nodes, because when we flush to them, we always produce multiple nodes if necessary
        }

        #endregion

        #region Queries

        public override Task<NodeResultBase<TKey>> ProcessQuery(NodeQueryBase<TKey> request)
        {
            var linearRequest = request as NodeQueryLinearBase<TKey>;
            uint filteredMatches, supersetMatches;
            List<RankKeyAndID<TKey>> matches = MatchingIntegratedItems(linearRequest, out filteredMatches, out supersetMatches);
            NodeResultBase<TKey> result = request.GetResultFromMatches(matches, filteredMatches, supersetMatches);
            return Task.FromResult(result);
        }

        internal virtual List<RankKeyAndID<TKey>> MatchingIntegratedItems(NodeQueryLinearBase<TKey> request, out uint filteredMatches, out uint supersetMatches)
        {
            CountedLeafNodeMatcher<TKey> matcher = new CountedLeafNodeMatcher<TKey>(request);
            foreach (var item in request.PendingChanges.Integrate(Items, request.Ascending, request.ItemIsPotentialMatch))
            {
                matcher.ProcessItem(item);
                if (matcher.Done)
                    break;
            }
            filteredMatches = matcher.FilteredMatches;
            supersetMatches = matcher.SupersetMatches;
            var matches = AdjustMatchesToCompensateForAnomalies(NodeInfo.FirstExclusive, NodeInfo.LastInclusive, request, matcher.Matches);
            return matches;
        }

        public static List<RankKeyAndID<TKey>> AdjustMatchesToCompensateForAnomalies(KeyAndID<TKey>? firstExclusive, KeyAndID<TKey>? lastInclusive, NodeQueryLinearBase<TKey> request, List<RankKeyAndID<TKey>> matches)
        {
            if (request.IncludedIndices == null)
                return matches; // we're looking for all the matches
            // If the number of items is not within the range expected (as a result of our pending changes including deletes/adds that have already been implemented), we adjust our results to match the expectations. This is not ideal (and moreover should be avoidable if we keep track of the versions that we are added), but should work to handle this rare situation for our purposes.
            int actualResultCount = matches.Count();
            uint minNumberSkipped, maxNumberSkipped, minNumberAvailableAfterSkipping, maxNumberAvailableAfterSkipping, minNumberReturned, maxNumberReturned;
            request.RangeOfNumberExpectedToReturn(firstExclusive, lastInclusive, out minNumberSkipped, out maxNumberSkipped, out minNumberAvailableAfterSkipping, out maxNumberAvailableAfterSkipping, out minNumberReturned, out maxNumberReturned);
            if (minNumberReturned > actualResultCount)
                for (int i = 0; i < minNumberReturned - actualResultCount; i++)
                    matches.Add(RankKeyAndID<TKey>.GetAnomalyPlaceholder());
            else if (maxNumberReturned < actualResultCount)
                matches = matches.Take((int)maxNumberReturned).ToList();
            return matches;
        }
        
        #endregion
    }
}
