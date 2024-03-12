using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using CountedTree.Core;
using CountedTree.NodeResults;
using CountedTree.PendingChanges;
using CountedTree.Updating;
using FluentAssertions;
using CountedTree.QueryExecution;
using System.Diagnostics;
using CountedTree.Queries;
using CountedTree.UintSets;
using Utility;
using Lazinator.Wrappers;

namespace CountedTree.Tests
{
    public partial class CountedTreeTests
    {
        private async Task<TreeInfo> SetupTree(TreeStructure ts, TreeUpdateSettings tu)
        {
            Guid treeID = RandomGenerator.GetGuid();
            TreeInfo treeInfo = await TreeFactory.CreateLinearTree<WFloat>(treeID, ts, tu, null, null);
            return treeInfo;
        }

        [Fact]
        public async Task CanQueryEmptyTree()
        {
            await CanChangeAndDeleteFromCountedTree_Helper(0, 0, 0);
            await CanChangeAndDeleteFromCountedTree_Helper(10, 10, 0);
        }

        [Fact]
        public async Task CanQueryCountedTree_WithSkipAndTake()
        {
            TreeStructure ts = new TreeStructure(true, 20, 200, false, false);
            var treeInfo = await SetupTree(ts, StandardTreeUpdateSettings);
            ITreeHistoryManager<WFloat> thm = await StorageFactory.GetTreeHistoryManagerAccess().Get<WFloat>(treeInfo.TreeID);
            int numToAdd = 4000;
            PendingChange<WFloat>[] additions =
                Enumerable.Range(0, numToAdd)
                .Select(x =>
                    new PendingChange<WFloat>(
                        new KeyAndID<WFloat>(
                            RandomGenerator.GetRandom(0, 100.0F),
                            (uint)x),
                        false)
                    )
                .ToArray();
            var items = additions.Select(x => x.Item).OrderBy(x => x).ToList();
            var itemsDescending = items.OrderByDescending(x => x).ToList();

            long versionNumber = 0;
            treeInfo = await thm.AddPendingChanges(new PendingChangesAtTime<WFloat>(GetDateTimeProvider().Now, new PendingChangesCollection<WFloat>(additions, true)), new Guid(), ++versionNumber);
            treeInfo = await thm.DoWorkRepeatedly(10);
            // now query
            foreach (bool ascending in new bool[] { true, false })
                foreach (int skip in new int[] { 0, 1, ts.NumChildrenPerInternalNode + 1, ts.MaxItemsPerLeaf + 1, 500, 20000 })
                    foreach (uint? take in new uint?[] { null, 0, 1, 100, 1000000 })
                    {
                        //Stopwatch s = new Stopwatch();
                        //s.Start();
                        List<KeyAndID<WFloat>> results = await DoQuery(treeInfo, ascending, (uint)skip, take, null);
                        //s.Stop();
                        //Debug.WriteLine($"Milliseconds for {ascending} {skip} {take}: {s.ElapsedMilliseconds}");
                        var itemsProperlySorted = (ascending ? items : itemsDescending);
                        var expectedResults = itemsProperlySorted.Skip((int)skip).Take((int)(take ?? int.MaxValue)).ToList();
                        results.SequenceEqual(expectedResults).Should().BeTrue();
                    }
        }

        [Fact]
        public async Task TreeQueryIntegration_Basic()
        {
            await TreeQueryIntegrationHelper(StandardTreeUpdateSettings, 1000, 10, 100, 5, 10, 10, true, true, false, false, false, false, 0.33F, 0.33F);
        }

        [Fact]
        public async Task TreeQueryIntegration_ImmediateDeletion()
        {
            var tus = new TreeUpdateSettings(10, 20, 5000, TimeSpan.FromMinutes(0));
            await TreeQueryIntegrationHelper(tus, 1000, 10, 100, 5, 10, 10, true, true, false, false, false, false, 0.33F, 0.33F);
        }

        [Fact]
        public async Task TreeQueryIntegration_SmallRequestBuffer()
        {
            var tus = new TreeUpdateSettings(10, 20, 5, TimeSpan.FromMinutes(30));
            await TreeQueryIntegrationHelper(tus, 1000, 10, 100, 5, 10, 10, true, true, false, false, false, false, 0.33F, 0.33F);
        }

        [Fact]
        public async Task TreeQueryIntegration_NoRequestBuffer()
        {
            var tus = new TreeUpdateSettings(10, 20, 5, TimeSpan.FromMinutes(30));
            await TreeQueryIntegrationHelper(tus, 1000, 10, 100, 5, 10, 10, true, true, false, false, false, false, 0.33F, 0.33F);
        }

        [Fact]
        public async Task TreeQueryIntegration_RedundantChanges()
        {
            // redundant changes must be done with UintSets.
            await TreeQueryIntegrationHelper(StandardTreeUpdateSettings, 1000, 10, 100, 5, 10, 10, true, true, false, false, false, true, 0.33F, 0.33F);
        }

        [Fact]
        public async Task TreeQueryIntegration_NoUintSets()
        {
            await TreeQueryIntegrationHelper(StandardTreeUpdateSettings, 1000, 10, 100, 5, 10, 10, false, true, false, false, false, false, 0.33F, 0.33F);
        }

        [Fact]
        public async Task TreeQueryIntegration_DoingWorkRarely()
        {
            await TreeQueryIntegrationHelper(StandardTreeUpdateSettings, 1000, 10, 100, 5, 10, 10, true, true, false, false, true, false, 0.33F, 0.33F);
        }

        [Fact]
        public async Task TreeQueryIntegration_AddingThenDeleting()
        {
            await TreeQueryIntegrationHelper(StandardTreeUpdateSettings, 1000, 10, 100, 5, 10, 10, true, true, false, false, false, false, 0.33F, 0.85F);
        }

        static bool runLongIntegrationTests = false; // DEBUG

        [Fact]
        public async Task TreeQueryIntegration_Long()
        {
            if (runLongIntegrationTests)
                await TreeQueryIntegrationHelper(StandardTreeUpdateSettings, 1000000, 1000, 1000, 64, 500, 10, true, false, false, false, true, false, 0.2F, 0.2F);
        }

        [Fact]
        public async Task TreeQueryIntegration_TriggeringRebuilds()
        {
            if (runLongIntegrationTests)
                await TreeQueryIntegrationHelper(StandardTreeUpdateSettings,  1000, 10, 500, 2, 3, 0, true, false, false, true, false, false, 0.33F, 0.50F);
        }

        private async Task TreeQueryIntegrationHelper(TreeUpdateSettings tus, int maxListLength, int maxSimultaneousChanges, int numCycles, int numChildrenPerInternalNode, int maxItemsPerLeaf, int finalWorkNeededThreshold, bool useFiltersAndUintSets, bool splitRangeEvenly, bool queryAfterEachChange, bool graduallyWeightToTop, bool doWorkRarely, bool redundantChanges, float probabilityDeletionFirstHalf, float probabilityDeletionSecondHalf)
        {
            bool print = false; // change to print out trees
            float?[] itemsInEachID = new float?[maxListLength];

            UintSet filter = null;
            if (useFiltersAndUintSets)
            {
                filter = new UintSet();
                for (int i = 0; i < maxListLength; i++)
                    if (i % 5 != 0)
                        filter.AddUint((uint)i);
            }

            TreeStructure ts = new TreeStructure(splitRangeEvenly, numChildrenPerInternalNode, maxItemsPerLeaf, useFiltersAndUintSets, useFiltersAndUintSets);
            var treeInfo = await SetupTree(ts, tus);
            ITreeHistoryManager<WFloat> thm = await StorageFactory.GetTreeHistoryManagerAccess().Get<WFloat>(treeInfo.TreeID);

            float bottomOfNumericRange = 0;
            long versionNumber = 0;
            for (int cycle = 0; cycle < numCycles; cycle++)
            {
                if (graduallyWeightToTop)
                    bottomOfNumericRange = bottomOfNumericRange + 0.50F * (100F - bottomOfNumericRange);
                    //bottomOfNumericRange = (float) MonotonicCurve.CalculateValueBasedOnProportionOfWayBetweenValues(0, 100M, 500M, (decimal)cycle / (decimal)numCycles);
                List<PendingChange<WFloat>> changes = new List<PendingChange<WFloat>>();
                int numChangesThisCycle = RandomGenerator.GetRandom(0, maxSimultaneousChanges);
                bool flagForPrinting = false;
                for (int change = 0; change < numChangesThisCycle; change++)
                {
                    bool del = RandomGenerator.GetRandom() < (cycle < numCycles / 2 ? probabilityDeletionFirstHalf : probabilityDeletionSecondHalf);
                    int i = RandomGenerator.GetRandom(0, maxListLength - 1);
                    float v = RandomGenerator.GetRandom(bottomOfNumericRange, 100F);
                    if (itemsInEachID[i] != null) // if there is something there, record a deletion change.
                        changes.Add(new PendingChange<WFloat>(new KeyAndID<WFloat>((float) itemsInEachID[i], (uint)i), true));
                    if (del)
                        itemsInEachID[i] = null;
                    else
                    { // record an adding item change
                        changes.Add(new PendingChange<WFloat>(new KeyAndID<WFloat>(v, (uint)i), false));
                        //if (changes.Any(x => x.Item.ID == 443))
                        //    flagForPrinting = true;
                        itemsInEachID[i] = v;
                    }
                }
                int printoutCycle = -1; 
                if (cycle == printoutCycle || flagForPrinting)
                {
                    Debug.WriteLine("");
                    Debug.WriteLine($"Cycle {cycle}");
                    Debug.WriteLine("");
                    await thm.PrintTree(redundantChanges);
                }
                if (redundantChanges)
                {
                    // to test redundancy, we create a clone of the tree history manager. (note that this will be a deep clone as to the backup pending changes tracker) We add pending changes to it. But then the thm2 object disappears, and we try to add things again on the saved version of our usual thm object
                    var thm2 = await thm.Clone();
                    treeInfo = await thm2.AddPendingChanges(new PendingChangesAtTime<WFloat>(GetDateTimeProvider().Now, new PendingChangesCollection<WFloat>(changes, true)), new Guid(), ++versionNumber);
                    thm = await StorageFactory.GetTreeHistoryManagerAccess().Get<WFloat>(treeInfo.TreeID);
                }
                await thm.AddPendingChanges(new PendingChangesAtTime<WFloat>(GetDateTimeProvider().Now, new PendingChangesCollection<WFloat>(changes, true)), new Guid(), ++versionNumber);
                if (cycle == printoutCycle || flagForPrinting)
                {
                    Debug.WriteLine("");
                    Debug.WriteLine($"Cycle {cycle} after adding {String.Join(", ", changes)} {(redundantChanges ? "redundantly" : "")}");
                    Debug.WriteLine("");
                    await thm.PrintTree(redundantChanges);
                }
                if (cycle == numCycles - 1 || queryAfterEachChange)
                {
                    // At the end, we want to make sure that everything from the test is incorporated; we verify the query each step of the way
                    if (cycle == numCycles - 1)
                        treeInfo = await thm.DoWorkRepeatedly(finalWorkNeededThreshold, queryAfterEachChange ? VerifyQuery(thm, cycle, itemsInEachID, treeInfo, true, null) : null);

                    await VerifyQuery(thm, cycle, itemsInEachID, treeInfo, true, null);
                    const bool doFilterTestsEachChangeIfQueryingEachChange = true;
                    if (cycle == numCycles - 1 || doFilterTestsEachChangeIfQueryingEachChange)
                    {
                        await VerifyQuery(thm, cycle, itemsInEachID, treeInfo, false, null);
                        if (useFiltersAndUintSets)
                        {
                            await VerifyQuery(thm, cycle, itemsInEachID, treeInfo, true, filter);
                            await VerifyQuery(thm, cycle, itemsInEachID, treeInfo, false, filter);
                        }
                    }
                }
                if (cycle < numCycles - 1 && (!doWorkRarely || cycle % 5 == 0))
                {
                    treeInfo = await thm.DoRoundOfWork();
                    if (cycle == printoutCycle || flagForPrinting)
                    {
                        Debug.WriteLine("");
                        Debug.WriteLine($"Cycle {cycle} after doing work");
                        Debug.WriteLine("");
                        await thm.PrintTree(redundantChanges);
                    }
                    if (queryAfterEachChange)
                        await VerifyQuery(thm, cycle, itemsInEachID, treeInfo, true, null);
                }
                if (print)
                {
                    Debug.WriteLine($"Cycle {cycle} Changes {String.Join(",", changes)}");
                    var pc = await StorageFactory.GetPendingChangesStorage().GetAllPendingChanges<WFloat>(treeInfo.TreeID, treeInfo.CurrentRootID);
                    Debug.WriteLine($"Pending changes storage {pc} ");
                    await thm.PrintTree(redundantChanges);
                }
                if (redundantChanges) // must make sure that we save the tree history manager so that we can simulate a failure after trying to add pending changes
                    await StorageFactory.GetTreeHistoryManagerAccess().Set<WFloat>(treeInfo.TreeID, thm);
            }
            
        }

        private static async Task VerifyQuery(ITreeHistoryManager<WFloat> thm, int cycle, float?[] itemsInEachID, TreeInfo treeInfo, bool ascending, UintSet filterToEliminateEveryFifthItem)
        {
            while (await thm.CatchupPendingChangesAreStored())
                await thm.DoRoundOfWork();
            List<KeyAndID<WFloat>> expectedResults = null;
            var items = itemsInEachID.Select((item, index) => new KeyAndID<WFloat>(item ?? -1F, (uint)index)).Where(x => x.Key != -1);
            if (filterToEliminateEveryFifthItem != null)
                items = items.Where(x => x.ID % 5 != 0);
            expectedResults = ascending ? items.OrderBy(x => x).ToList() : items.OrderByDescending(x => x).ToList();
            List<KeyAndID<WFloat>> results = await DoQuery(treeInfo, ascending, 0, null, filterToEliminateEveryFifthItem);
            for (int i = 0; i < results.Count(); i++)
                if (results[i] != expectedResults[i])
                    throw new Exception($"Cycle {cycle}: Result {i} was expected to be {expectedResults[i]} but was {results[i]}.");
            results.Count().Should().Be(expectedResults.Count());
        }

        private static async Task VerifyMatch(TreeInfo treeInfo, List<KeyAndID<WFloat>> items)
        {
            await VerifyMatch(treeInfo, true, 0, null, items);
        }

        private static async Task VerifyMatch(TreeInfo treeInfo, bool ascending, uint skip, uint? take, List<KeyAndID<WFloat>> items)
        {
            List<KeyAndID<WFloat>> results = await DoQuery(treeInfo, ascending, skip, take, null);
            results.Count().Should().Be(items.Count());
            for (int i = 0; i < results.Count(); i++)
                results[i].Equals(items[i]).Should().BeTrue();
        }

        public static async Task<List<KeyAndID<WFloat>>> DoQuery(TreeInfo treeInfo, bool ascending, uint skip, uint? take, UintSet filter)
        {
            DateTime asOfTime = GetDateTimeProvider().Now;
            QueryFilter queryFilter = null;
            if (filter != null)
                queryFilter = new QueryFilter(filter, QueryFilterRankingOptions.RankWithinAllItems, null);
            TreeQueryExecutorLinear<WFloat> queryExecutor = new TreeQueryExecutorLinear<WFloat>(treeInfo, asOfTime, skip, take, QueryResultType.KeysAndIDs, ascending, null, null, queryFilter);
            var results = (await queryExecutor.GetKeysAndIDs()).ToList();
            return results;
        }

        [Fact]
        public async Task CanQueryForJustIDsOrBitSetOrKeys()
        {
            int numAdditions = 1000;
            int numNodesPerInternal = 20;
            int maxItemsPerLeaf = 200;
            // We want to make sure that we can query for any result type, regardless of whether we have bit set filters. But we will not test storing bit set child locations here.
            var withBitSetFilters = new TreeStructure(true, numNodesPerInternal, maxItemsPerLeaf, true, false);
            await CanQueryForJustIDsOrBitSetOrKeys_Helper(numAdditions, withBitSetFilters);
            var withoutBitSetFilters = new TreeStructure(true, numNodesPerInternal, maxItemsPerLeaf, false, false);
            await CanQueryForJustIDsOrBitSetOrKeys_Helper(numAdditions, withoutBitSetFilters);
        }

        private async Task CanQueryForJustIDsOrBitSetOrKeys_Helper(int numAdditions, TreeStructure ts)
        {
            var treeInfo = await SetupTree(ts, StandardTreeUpdateSettings);
            ITreeHistoryManager<WFloat> thm = await StorageFactory.GetTreeHistoryManagerAccess().Get<WFloat>(treeInfo.TreeID);
            PendingChange<WFloat>[] additions =
                Enumerable.Range(0, numAdditions)
                .Select(x =>
                    new PendingChange<WFloat>(
                        new KeyAndID<WFloat>(
                            RandomGenerator.GetRandom(0, 100.0F),
                            (uint)x),
                        false)
                    )
                .ToArray();
            var pendingChanges = new PendingChangesCollection<WFloat>(additions, true);
            var items = additions.Select(x => x.Item).OrderBy(x => x).ToList();
            long versionNumber = 0;
            treeInfo = await thm.AddPendingChanges(new PendingChangesAtTime<WFloat>(GetDateTimeProvider().Now, pendingChanges), new Guid(), ++versionNumber);
            treeInfo = await thm.AddPendingChangesFromStorageToTree();
            GetDateTimeProvider().SleepOrSkipTime(1);
            DateTime asOfTime = GetDateTimeProvider().Now;
            TreeQueryExecutorLinear<WFloat> queryExecutor = new TreeQueryExecutorLinear<WFloat>(treeInfo, asOfTime, 0, null, QueryResultType.KeysOnly, true, null, null, null);
            var keyResults = (await queryExecutor.GetKeys()).ToList();
            keyResults.Should().Equal(additions.Select(x => x.Item.Key).OrderBy(x => x));
            queryExecutor = new TreeQueryExecutorLinear<WFloat>(treeInfo, asOfTime, 0, null, QueryResultType.IDsOnly, true, null, null, null);
            var idResults = (await queryExecutor.GetIDs()).ToList();
            idResults.Should().Equal(additions.OrderBy(x => x.Item.Key).Select(x => x.Item.ID));
            queryExecutor = new TreeQueryExecutorLinear<WFloat>(treeInfo, asOfTime, 0, null, QueryResultType.IDsAsBitSet, true, null, null, null);
            UintSet bitSet = (await queryExecutor.GetIDsAsBitSet());
            bitSet.AsEnumerable().Should().Equal(additions.OrderBy(x => x.Item.ID).Select(x => x.Item.ID));
        }
    }
}
