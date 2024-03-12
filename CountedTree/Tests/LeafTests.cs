using System;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using CountedTree.Core;
using CountedTree.Node;
using CountedTree.NodeResults;
using CountedTree.PendingChanges;
using CountedTree.Queries;
using FluentAssertions;
using System.Threading.Tasks;
using Utility;
using CountedTree.UintSets;
using Lazinator.Wrappers;

namespace CountedTree.Tests
{
    public partial class CountedTreeTests
    {
        [Fact]
        public async Task LeafAddsPendingChanges()
        {
            int numOriginalItems = 20;
            int numToAddToOriginal = 5;
            int numToDeleteFromOriginal = 3;
            int numToChangeInOriginal = 5;
            List<KeyAndID<WFloat>> original;
            List<KeyAndID<WFloat>> replacement;
            PendingChangesCollection<WFloat> changes;
            GetOriginalAndReplacementItemsAndTransformativeChanges(numOriginalItems, numToAddToOriginal, numToDeleteFromOriginal, numToChangeInOriginal, out original, out replacement, out changes);


            TreeStructure ts = new TreeStructure(false, 50, 300, false, false);
            CountedLeafNode<WFloat> leaf = new CountedLeafNode<WFloat>(original, 0, 1, ts, null, null);
            var replacementLeaf = (await leaf.FlushToNode(0, changes)).First() as CountedLeafNode<WFloat>;
            replacementLeaf.Items.SequenceEqual(replacement).Should().BeTrue();
        }

        [Fact]
        public async Task RandomizedLeafTest()
        {
            RandomGenerator.SeedOverride = 1;
            for (int i = 0; i <= 15; i++)
            {
                await LeafConvertsToInternalPlusLeaves();
                RandomGenerator.Reset(i + 1);
            }
        }

        [Fact]
        public async Task LeafConvertsToInternalPlusLeaves()
        {
            await LeafConvertsToInternalPlusLeaves_WithSplitRangeEvenlyOption(false);
            await LeafConvertsToInternalPlusLeaves_WithSplitRangeEvenlyOption(true);
        }

        internal async Task LeafConvertsToInternalPlusLeaves_WithSplitRangeEvenlyOption(bool splitRangeEvenly)
        {
            int numOriginalItems = 31;
            int numToAddToOriginal = 5;
            int numToDeleteFromOriginal = 5;
            int numToChangeInOriginal = 5;
            List<KeyAndID<WFloat>> original;
            List<KeyAndID<WFloat>> replacement;
            PendingChangesCollection<WFloat> changes;
            GetOriginalAndReplacementItemsAndTransformativeChanges(numOriginalItems, numToAddToOriginal, numToDeleteFromOriginal, numToChangeInOriginal, out original, out replacement, out changes);

            TreeStructure ts = new TreeStructure(splitRangeEvenly, 5, 10, false, false);
            CountedLeafNode<WFloat> leaf = new CountedLeafNode<WFloat>(original, 0, 1, ts, new KeyAndID<WFloat>(0, uint.MinValue), new KeyAndID<WFloat>(100, uint.MaxValue));
            long previousRootID = 1000;
            var replacementNodes = await leaf.FlushToNode(previousRootID + 1, changes);
            var replacementLeaves = replacementNodes.Where(x => x is CountedLeafNode<WFloat>).Select(x => (CountedLeafNode<WFloat>)x).ToList();
            var replacementInternalNodes = replacementNodes.Where(x => x is CountedInternalNode<WFloat>).Select(x => (CountedInternalNode<WFloat>)x);
            foreach (var replacementInternal in replacementInternalNodes)
            {
                List<KeyAndID<WFloat>?> ranges = new List<KeyAndID<WFloat>?>();
                ranges.Add(new KeyAndID<WFloat>(replacementInternal.NodeInfo.FirstExclusive?.Key ?? float.MinValue, uint.MinValue));
                ranges.AddRange(replacementInternal.ChildNodeInfos.Select(x => x.LastInclusive).Take(replacementInternal.ChildNodeInfos.Length - 1));
                ranges.Add(new KeyAndID<WFloat>(replacementInternal.NodeInfo.LastInclusive?.Key ?? float.MaxValue, uint.MaxValue));
                int totalItems = 0;
                int i = 0;
                bool allChildrenAreLeaves = true;
                foreach (NodeInfo<WFloat> childNode in replacementInternal.ChildNodeInfos)
                {
                    if (childNode.MaxDepth > childNode.Depth)
                    { // it's another Internal node
                        allChildrenAreLeaves = false;
                        break;
                    }
                    var expectedItems = replacement.Where(x => x.IsInRange(ranges[i], ranges[i + 1])).ToList();
                    childNode.NumSubtreeValues.Should().Be((uint)expectedItems.Count());
                    i++;
                    if (childNode.Created)
                    {
                        var replacementLeaf = replacementLeaves.Single(x => x.NodeInfo.Equals(childNode));
                        totalItems += replacementLeaf.Items.Count();
                        replacementLeaf.Items.SequenceEqual(expectedItems).Should().BeTrue();
                        replacementLeaf.TreeStructure.NumChildrenPerInternalNode.Should().Be(ts.NumChildrenPerInternalNode);
                        replacementLeaf.TreeStructure.MaxItemsPerLeaf.Should().Be(ts.MaxItemsPerLeaf);
                    }
                }
                if (allChildrenAreLeaves && replacementInternalNodes.Count() == 1) // the following depends on this assumption (we can vary random number generation to test both ways)
                {
                    replacementInternal.NodeInfo.NumSubtreeValues.Should().Be((uint)replacementLeaves.Sum(x => x.NodeInfo.NumSubtreeValues));
                    replacementInternal.NodeInfo.NodeID.Should().Be(previousRootID + replacementNodes.Count()); // replacement Internal should have highest number
                    replacementLeaves.Select(x => x.NodeInfo.NodeID).OrderBy(x => x).Should().Equal(Enumerable.Range((int)previousRootID + 1, replacementLeaves.Count()));
                }
            }
        }

        [Fact]
        public async Task LeafConverts_ManyAddedAtOnce()
        {
            int numOriginalItems = 1;
            int numToAddToOriginal = 20;
            int numToDeleteFromOriginal = 0;
            int numToChangeInOriginal = 0;
            TreeStructure ts = new TreeStructure(false, 2, 2, false, false);
            List<KeyAndID<WFloat>> original;
            List<KeyAndID<WFloat>> replacement;
            PendingChangesCollection<WFloat> changes;
            GetOriginalAndReplacementItemsAndTransformativeChanges(numOriginalItems, numToAddToOriginal, numToDeleteFromOriginal, numToChangeInOriginal, out original, out replacement, out changes);
            CountedLeafNode<WFloat> leaf = new CountedLeafNode<WFloat>(original, 0, 1, ts, null, null);
            long previousRootID = 1000;
            var replacementNodes = await leaf.FlushToNode(previousRootID + 1, changes);
            var replacementLeaves = replacementNodes.Where(x => x is CountedLeafNode<WFloat>).Select(x => (CountedLeafNode<WFloat>)x).ToList();
            var replacementInternal = replacementNodes.Where(x => x is CountedInternalNode<WFloat>).Select(x => (CountedInternalNode<WFloat>)x).ToList();
            replacementInternal.Count().Should().BeGreaterThan(1);
            var replacementIDs = replacementNodes.Select(x => x.ID);
            replacementIDs.Distinct().Count().Should().Be(replacementNodes.Count());
            replacementIDs.Contains(previousRootID + 1).Should().BeFalse(); // this should be replaced by a higher number
        }

        [Fact]
        public async Task LeafQueriesWork()
        {
            await LeafQueriesWork_Helper(AnomalyType.NoAnomaly);
            await LeafQueriesWork_Helper(AnomalyType.RedundantDeletion);
            await LeafQueriesWork_Helper(AnomalyType.RedundantInsertion);
        }

        internal async Task LeafQueriesWork_Helper(AnomalyType anomaly)
        {
            int numOriginalItems = 31;
            int numToAddToOriginal = 5;
            int numToDeleteFromOriginal = 3;
            int numToChangeInOriginal = 4;
            List<KeyAndID<WFloat>> original;
            List<KeyAndID<WFloat>> replacement;
            PendingChangesCollection<WFloat> changes;
            if (anomaly == AnomalyType.NoAnomaly)
                GetOriginalAndReplacementItemsAndTransformativeChanges(numOriginalItems, numToAddToOriginal, numToDeleteFromOriginal, numToChangeInOriginal, out original, out replacement, out changes);
            else
                GetOriginalAndReplacementItemsAndTransformativeChangesWithAnomaly(numOriginalItems, numToAddToOriginal, numToDeleteFromOriginal, numToChangeInOriginal, anomaly, out original, out replacement, out changes);

            TreeStructure ts = new TreeStructure(false, 50, 100, false, false);
            CountedLeafNode<WFloat> leaf = new CountedLeafNode<WFloat>(original, 0, 1, ts, null, null);

            foreach (bool ascending in new bool[] { true, false })
                foreach (uint skip in new uint[] { 0, 3 })
                    foreach (uint? take in new uint?[] { null, 4, 100 })
                        foreach (float? min in new float?[] { null, 40.0F })
                            foreach (float? max in new float?[] { null, 60.0F })
                                foreach (bool supersetFilter in new bool[] { false, true })
                                    foreach (bool filteredSetFilter in new bool[] { false, true })
                                    {
                                        if ((supersetFilter || filteredSetFilter) && (min != null || max != null))
                                            continue; // using min and max is not supported with filters (which can achieve same functionality, however)
                                        if ((supersetFilter || filteredSetFilter) && anomaly != AnomalyType.NoAnomaly)
                                            continue; // we correct for anomalies when using filters before getting to the stage of checking leaf results
                                        await GetExpectedResultsAndConfirmLeafQuery(replacement, changes, leaf, ascending, skip, take, min, max, anomaly, supersetFilter, filteredSetFilter);
                                    }
        }

        private static async Task GetExpectedResultsAndConfirmLeafQuery(List<KeyAndID<WFloat>> itemsIntegratingChanges, PendingChangesCollection<WFloat> changes, CountedLeafNode<WFloat> leaf, bool ascending, uint skip, uint? take, float? min, float? max, AnomalyType anomaly, bool useSupersetFilter, bool useFilteredSetFilter)
        {
            var expectedResults = ascending ? itemsIntegratingChanges.ToList() : itemsIntegratingChanges.OrderByDescending(x => x).ToList();
            if (min != null)
                expectedResults = expectedResults.Where(x => x.Key >= min).ToList();
            if (max != null)
                expectedResults = expectedResults.Where(x => x.Key <= max).ToList();

            int supersetCount = expectedResults.Count();
            QueryFilter queryFilter = GetQueryFilter(useSupersetFilter, useFilteredSetFilter);
            if (useSupersetFilter || useFilteredSetFilter)
            {
                UintSet filteredSet = (queryFilter.SearchWithin ?? queryFilter.Superset);
                UintSet superset = queryFilter.Superset;
                if (superset != null)
                    supersetCount = expectedResults.Where(e => superset.Contains(e.ID)).Count();
                expectedResults = expectedResults.Where(e => filteredSet.Contains(e.ID)).ToList();
            }
            var expectedResultsBeforeSkipTake = expectedResults;

            // Note that skip and take do NOT affect the included indices. The included indices for a node are the indices ignoring skip/take. This makes sense since we want to be able to return information about the entire set of items being searched, even if we are only actually returning some of them.
            if (skip != 0)
                expectedResults = expectedResults.Skip((int)skip).ToList();
            if (take != null)
                expectedResults = expectedResults.Take((int)take).ToList();

            // arbitrarily assume a value for both the first index in the superset and the first index in the filtered set.
            const uint expectedFirstIndexInSuperset = 17;
            uint expectedFirstIndexInFilteredSet = useFilteredSetFilter ? 13 : expectedFirstIndexInSuperset;
            uint expectedLastIndexInSuperset = expectedFirstIndexInSuperset + (uint)supersetCount - 1;
            uint expectedLastIndexInFilteredSet = expectedFirstIndexInFilteredSet + (uint)expectedResultsBeforeSkipTake.Count() - 1;
            if (min == null && max == null && (take == null || take == 100))
            { // with min/max, there is a range of potential values, so there will be no anomaly adjustment
                if (anomaly == AnomalyType.RedundantDeletion)
                {
                    expectedLastIndexInSuperset--; // we expect less than exists (i.e., there is one more than we expect) because of the redundant deletion
                    expectedLastIndexInFilteredSet--; // our redundant deletion in the test will always be a filtered item
                    expectedResults = expectedResults.Take(expectedResults.Count() - 1).ToList(); // so, we should expect the last item not to be sent back to us, to make the results conform with the expectations
                }
                else if (anomaly == AnomalyType.RedundantInsertion)
                {
                    expectedLastIndexInSuperset++;
                    expectedLastIndexInFilteredSet++;
                    expectedResults.Add(RankKeyAndID<WFloat>.GetAnomalyPlaceholder().GetKeyAndID());
                }
            }

            await ConfirmLeafQuery(changes, leaf, ascending, min, max, skip, take, expectedResults, expectedFirstIndexInSuperset, expectedLastIndexInSuperset, expectedFirstIndexInFilteredSet, expectedLastIndexInFilteredSet, queryFilter);
        }

        private static async Task ConfirmLeafQuery(PendingChangesCollection<WFloat> changes, CountedLeafNode<WFloat> leaf, bool ascending, float? min, float? max, uint skip, uint? take, List<KeyAndID<WFloat>> expectedResults, uint expectedFirstIndexInSuperset, uint expectedLastIndexInSuperset, uint expectedFirstIndexInFilteredSet, uint expectedLastIndexInFilteredSet, QueryFilter filter)
        {
            foreach (QueryResultType resultType in Enum.GetValues(typeof(QueryResultType)))
            {
                if (resultType != QueryResultType.KeysIDsAndDistance)
                {
                    NodeQueryLinearBase<WFloat> q;
                    if (min == null && max == null)
                        q = new NodeQueryIndexRange<WFloat>(leaf.NodeInfo.NodeID, leaf.NodeInfo.Created, ascending, skip, take, new IncludedIndices(expectedFirstIndexInSuperset, expectedLastIndexInSuperset, expectedFirstIndexInFilteredSet, expectedLastIndexInFilteredSet), changes, resultType, filter);
                    else
                        q = new NodeQueryValueRange<WFloat>(leaf.NodeInfo.NodeID, leaf.NodeInfo.Created, ascending, skip, take, new IncludedIndices(expectedFirstIndexInSuperset, expectedLastIndexInSuperset, expectedFirstIndexInFilteredSet, expectedLastIndexInFilteredSet), changes, resultType, min == null ? null : (KeyAndID<WFloat>?)new KeyAndID<WFloat>((float)min, uint.MinValue), max == null ? null : (KeyAndID<WFloat>?)new KeyAndID<WFloat>((float)max, uint.MaxValue));
                    NodeResultLinearBase<WFloat> theResult = await leaf.ProcessQuery(q) as NodeResultLinearBase<WFloat>;
                    ConfirmLeafQueryResults(theResult, expectedResults, resultType);
                }
            }
        }

        private static void ConfirmLeafQueryResults(NodeResultLinearBase<WFloat> theResult, IEnumerable<KeyAndID<WFloat>> expectedResults, QueryResultType resultType)
        {
            switch (resultType)
            {
                case QueryResultType.IDsOnly:
                case QueryResultType.IDsAsBitSet: // we're confirming leaf query, so we won't get back NodeResultUintSet
                    var idsResult = (NodeResultIDs<WFloat>)theResult;
                    idsResult.IDs.Should().Equal(expectedResults.Select(x => x.ID));
                    break;
                case QueryResultType.KeysOnly:
                    var keysResult = (NodeResultKeys<WFloat>)theResult;
                    keysResult.Keys.Should().Equal(expectedResults.Select(x => x.Key));
                    break;
                case QueryResultType.KeysAndIDs:
                    var keysAndIDsResult = (NodeResultKeysAndIDs<WFloat>)theResult;
                    keysAndIDsResult.Values.Should().Equal(expectedResults);
                    break;
                case QueryResultType.IDsAndRanks:
                    var ranksResult = (NodeResultRanksAndIDs<WFloat>)theResult;
                    ranksResult.Values.Select(x => x.ID).Should().Equal(expectedResults.Select(x => x.ID));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
