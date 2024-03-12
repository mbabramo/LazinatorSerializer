using System;
using System.Linq;
using System.Threading.Tasks;
using R8RUtilities;
using Xunit;
using System.Collections.Generic;
using CountedTree.Core;
using CountedTree.PendingChanges;
using CountedTree.Updating;
using FluentAssertions;
using CountedTree.Rebuild;
using CountedTree.Node;
using CountedTree.NodeStorage;
using CountedTree.UintSets;
using Utility;
using Lazinator.Wrappers;

namespace CountedTree.Tests
{
    public partial class CountedTreeTests
    {
        [Fact]
        public async Task CanManuallyTriggerTreeRebalance()
        {
            await TreeRebalances_Helper(false, true);
        }

        [Fact]
        public async Task TreeWillAutomaticallyRebalance()
        {
            await TreeRebalances_Helper(false, false);
        }

        [Fact]
        public async Task CanTriggerTreeRebalanceWithEvenRanges()
        {
            await TreeRebalances_Helper(true, true);
        }

        internal async Task TreeRebalances_Helper(bool splitRangesEvenly, bool manuallyTriggerRebuild)
        {
            long versionNumber = 0;
            // we will add items to a tree very asymmetrically. We will also disable tree rebuids.
            TreeStructure ts = new TreeStructure(false, 3, 10, true, true, manuallyTriggerRebuild ? byte.MaxValue : (byte) 2);
            var treeInfo = await SetupTree(ts, new TreeUpdateSettings(10, 20, 5000, TimeSpan.FromDays(1))); // we'll use 1 day as old, so that time doesn't pass too quickly as a result of calls to DateTimeProvider.Now (since AbsoluteFakeDateTimeProvider.Now advances the clock 1 minute)
            ITreeHistoryManager<WFloat> thm = await StorageFactory.GetTreeHistoryManagerAccess().Get<WFloat>(treeInfo.TreeID); 
            int numToAdd = 85; // enough to get two levels of rebalancing
            PendingChange<WFloat>[] additions =
                Enumerable.Range(0, numToAdd)
                .Select(x =>
                    new PendingChange<WFloat>(
                        new KeyAndID<WFloat>(
                            (float)x,
                            (uint)x),
                        false)
                    )
                .ToArray();
            bool depthHasDeclinedAtLeastOnce = false;
            for (int i = 0; i < numToAdd; i++)
            { // add them one at a time, doing work to flush them
                treeInfo = await thm.AddPendingChanges(new PendingChangesAtTime<WFloat>(GetDateTimeProvider().Now, new PendingChangesCollection<WFloat>(new PendingChange<WFloat>[] { additions[i] }, true)), new Guid(), ++versionNumber);
                var originalDepth = (await treeInfo.GetRoot<WFloat>()).NodeInfo.MaxDepth;
                treeInfo = await thm.DoWorkRepeatedly(0); // this should flush but also eventually rebalance
                var afterDepth = (await treeInfo.GetRoot<WFloat>()).NodeInfo.MaxDepth;
                depthHasDeclinedAtLeastOnce = depthHasDeclinedAtLeastOnce || afterDepth < originalDepth;
                treeInfo.NumValuesInTree.Should().Be((uint) i + 1);
            }
            var items = additions.Select(x => x.Item).OrderBy(x => x).ToList();
            treeInfo = await AddPendingChangesAndVerify(treeInfo, thm, items, new PendingChange<WFloat>[] { }, ++versionNumber); // just checking values previously added (not adding anything new)
            if (manuallyTriggerRebuild)
                await thm.InitiateRebalancing();
            treeInfo = await thm.DoWorkRepeatedly(0);  
            treeInfo = await AddPendingChangesAndVerify(treeInfo, thm, items, new PendingChange<WFloat>[] { }, ++versionNumber); // just checking values previously added (not adding anything new)
            if (splitRangesEvenly)
                return;
            var root = await treeInfo.GetRoot<WFloat>();
            if (!splitRangesEvenly && manuallyTriggerRebuild)
                root.NodeInfo.MaxDepth.Should().Be(2);
            else if (!splitRangesEvenly && !manuallyTriggerRebuild)
                depthHasDeclinedAtLeastOnce.Should().BeTrue();
            root.NodeInfo.NumSubtreeValues.Should().Be((uint) numToAdd);
        }

        private async Task ConfirmUintSets(Guid treeID, CountedInternalNode<WFloat> root, INodeStorage nodeStorage, IBlob<Guid> uintSetStorage)
        {
            await root.SetUintSetStorage(uintSetStorage);
            UintSet u = await root.GetUintSet();
            u.Count.Should().Be(root.NodeInfo.NumSubtreeValues);
            var childrenTasks = root.ChildNodeInfos.Select(async x => await nodeStorage.GetNode<WFloat>(treeID, x.NodeID)).ToArray();
            await Task.WhenAll(childrenTasks);
            foreach (var childTask in childrenTasks)
            {
                var child = await childTask;
                if (child is CountedInternalNode<WFloat>)
                    await ConfirmUintSets(treeID, (CountedInternalNode<WFloat>)child, nodeStorage, uintSetStorage);
            }
        }

        [Fact]
        public async Task CanBuildIndexFromScratchViaRebuild()
        {
            await CanBuildIndexFromScratchViaRebuild_Helper(true, 100);
            await CanBuildIndexFromScratchViaRebuild_Helper(false, 0);
            await CanBuildIndexFromScratchViaRebuild_Helper(false, 4);
            await CanBuildIndexFromScratchViaRebuild_Helper(false, 50);
            await CanBuildIndexFromScratchViaRebuild_Helper(false, 7000);
        }

        internal async Task CanBuildIndexFromScratchViaRebuild_Helper(bool splitEvenly, int numItems)
        {
            TreeStructure ts = new TreeStructure(splitEvenly, 3, 10, false, false);
            var treeInfo = await SetupTree(ts, new TreeUpdateSettings(10, 20, 5000, TimeSpan.FromDays(1)));
            ITreeHistoryManager<WFloat> thm = await StorageFactory.GetTreeHistoryManagerAccess().Get<WFloat>(treeInfo.TreeID);
            var items = Enumerable.Range(0, numItems).Select(x => new KeyAndID<WFloat>(RandomGenerator.GetRandom(0.0F, 100.0F), (uint)x)).OrderBy(x => x).ToList();
            InMemoryRebuildSource<WFloat> rebuildSource = new InMemoryRebuildSource<WFloat>(items, new KeyAndID<WFloat>(0, uint.MinValue), new KeyAndID<WFloat>(100.0F, uint.MaxValue));
            treeInfo = await thm.InitiateRebuildFromExternalSource(rebuildSource);
            treeInfo = await thm.DoWorkRepeatedly(0);
            treeInfo = await AddPendingChangesAndVerify(treeInfo, thm, items, new PendingChange<WFloat>[] { }, 1); // just checking values 
        }

        [Fact]
        public async Task CompositeRebuildSourceWorks()
        {
            bool splitEvenly = false;
            int numRebuildSources = 10;
            int numItemsPerRebuildSource = 100;
            CombinedRebuildSource<WFloat>.CombinedNumValuesToHoldInBuffers = 30; // use a smaller than usual number to test functionality fully
            TreeStructure ts = new TreeStructure(splitEvenly, 3, 10, false, false);
            var treeInfo = await SetupTree(ts, new TreeUpdateSettings(10, 20, 5000, TimeSpan.FromDays(1)));
            ITreeHistoryManager<WFloat> thm = await StorageFactory.GetTreeHistoryManagerAccess().Get<WFloat>(treeInfo.TreeID);
            var rebuildSources = new List<IRebuildSource<WFloat>>();
            List<KeyAndID<WFloat>> combinedItems = new List<KeyAndID<WFloat>>();
            uint startRange = 0;
            for (int i = 0; i < numRebuildSources; i++)
            {
                var items = Enumerable.Range((int) startRange, numItemsPerRebuildSource).Select(x => new KeyAndID<WFloat>(RandomGenerator.GetRandom(0.0F, 100.0F), (uint)x)).OrderBy(x => x).ToList();
                startRange += (uint) numItemsPerRebuildSource;
                combinedItems.AddRange(items);
                InMemoryRebuildSource<WFloat> rebuildSource = new InMemoryRebuildSource<WFloat>(items, new KeyAndID<WFloat>(0, uint.MinValue), new KeyAndID<WFloat>(100.0F, uint.MaxValue));
                rebuildSources.Add(rebuildSource);
            }
            combinedItems = combinedItems.OrderBy(x => x).ToList();
            CombinedRebuildSource<WFloat> rebuildSource_combined = new CombinedRebuildSource<WFloat>( rebuildSources, new KeyAndID<WFloat>(0, uint.MinValue), new KeyAndID<WFloat>(100.0F, uint.MaxValue));
            treeInfo = await thm.InitiateRebuildFromExternalSource(rebuildSource_combined);
            treeInfo = await thm.DoWorkRepeatedly(0);
            treeInfo = await AddPendingChangesAndVerify(treeInfo, thm, combinedItems, new PendingChange<WFloat>[] { }, 1); // just checking values 
        }

    }
}
