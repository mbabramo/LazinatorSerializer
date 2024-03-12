using System;
using System.Linq;
using System.Threading.Tasks;
using R8RUtilities;
using Xunit;
using System.Collections.Generic;
using CountedTree.Core;
using CountedTree.Node;
using CountedTree.NodeStorage;
using CountedTree.PendingChanges;
using CountedTree.Updating;
using FluentAssertions;
using Utility;
using Lazinator.Wrappers;

namespace CountedTree.Tests
{
    public partial class CountedTreeTests
    {
        [Fact]
        public async Task BufferedPendingQueriesAreAddedToTree()
        {
            bool splitEvenly = false;
            int numAddingRepetitions = 5;
            int numItemsPerRepetition = 20; // first one won't be buffered, but subsequent ones will be buffered
            TreeStructure ts = new TreeStructure(splitEvenly, 3, 10, false, false);
            var treeInfo = await SetupTree(ts, new TreeUpdateSettings(10, 20, 25, TimeSpan.FromDays(1)));
            ITreeHistoryManager<WFloat> thm = await StorageFactory.GetTreeHistoryManagerAccess().Get<WFloat>(treeInfo.TreeID);
            List<KeyAndID<WFloat>> combinedItems = new List<KeyAndID<WFloat>>();
            uint startRange = 0;
            long versionNumber = 0;
            for (int i = 0; i < numAddingRepetitions; i++)
            {
                var items = Enumerable.Range((int)startRange, numItemsPerRepetition).Select(x => new KeyAndID<WFloat>(RandomGenerator.GetRandom(0.0F, 100.0F), (uint)x)).OrderBy(x => x).ToList();
                var pendingChanges = items.Select(x =>
                    new PendingChange<WFloat>(
                        x,
                        false)
                    ).ToArray();
                startRange += (uint)numItemsPerRepetition;
                combinedItems.AddRange(items);
                treeInfo = await thm.AddPendingChanges(new PendingChangesAtTime<WFloat>(GetDateTimeProvider().Now, new PendingChangesCollection<WFloat>(pendingChanges, true)), new Guid(), ++versionNumber);
            }
            (await thm.CatchupPendingChangesAreStored()).Should().BeTrue();
            combinedItems = combinedItems.OrderBy(x => x).ToList();
            treeInfo = await thm.DoWorkRepeatedly(0);
            (await thm.CatchupPendingChangesAreStored()).Should().BeFalse();
            treeInfo = await AddPendingChangesAndVerify(treeInfo, thm, combinedItems, new PendingChange<WFloat>[] { }, 1); // just checking values 
        }

        [Fact]
        public async Task OldNodesAreDeleted()
        {
            TreeStructure ts = new TreeStructure(false, 3, 5, true, true); // we will not use evenly divided ranges, because then we will have some uncreated nodes, which will mess up our deletion calculations below (while not being relevant to testing). Meanwhile, we will store UintSets and locations to make sure that works properly.
            var treeInfo = await SetupTree(ts, new TreeUpdateSettings(10, 20, 25, TimeSpan.FromDays(1))); // we'll use 1 day as old, so that time doesn't pass too quickly as a result of calls to DateTimeProvider.Now (since AbsoluteFakeDateTimeProvider.Now advances the clock 1 minute)
            ITreeHistoryManager<WFloat> thm = await StorageFactory.GetTreeHistoryManagerAccess().Get<WFloat>(treeInfo.TreeID);
            int numToAdd = 250;
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
            var additionsOrdered = additions.OrderBy(x => x.Item).ToList();
            var remainingAdditionsOrdered = additionsOrdered.ToList();
            int numToAddAtATime = 25;
            long versionNumber = 0;

            while (remainingAdditionsOrdered.Any())
            {
                var additionsGroup = remainingAdditionsOrdered.Take(numToAddAtATime);
                remainingAdditionsOrdered = remainingAdditionsOrdered.Skip(numToAddAtATime).ToList();
                treeInfo = await thm.AddPendingChanges(new PendingChangesAtTime<WFloat>(GetDateTimeProvider().Now, new PendingChangesCollection<WFloat>(additionsGroup, true)), new Guid(), ++versionNumber);
                treeInfo = await thm.DoWorkRepeatedly(0); // do work until tree is perfect
                GetDateTimeProvider().SleepOrSkipTime(1000 * 60 * 60 * 3); // 3 hours forwards
            }

            // So far, we'll have only one node to delete. So, let's delete some items too.
            int numDeletions = 10; // just enough to force some work
            var deletionItems = RandomGenerator.PickRandomItems<KeyAndID<WFloat>>(additionsOrdered.Select(x => x.Item).ToList(), numDeletions);
            PendingChange<WFloat>[] deletions =
                deletionItems
                .Select(x =>
                    new PendingChange<WFloat>(
                        x,
                        true)
                    )
                .ToArray();
            treeInfo = await thm.AddPendingChanges(new PendingChangesAtTime<WFloat>(GetDateTimeProvider().Now, new PendingChangesCollection<WFloat>(deletions, true)), new Guid(), ++versionNumber);
            treeInfo = await thm.DoWorkRepeatedly(0); // do work until tree is perfect

            var rootNode = await treeInfo.GetRoot<WFloat>();
            int numNodes = rootNode.NodeInfo.NumNodes;
            int numNodesBeforeSkippingTime = await ConfirmNumNodesInStorage(treeInfo, numNodes, true);
            int inMemoryUintSetCount = GetInMemoryUintSetCount();
            GetDateTimeProvider().SleepOrSkipTime(1000L * 60 * 60 * 24 * 30); // skip ahead a month
            await thm.UpdateWorkNeeded();
            treeInfo = await thm.DoWorkRepeatedly(0);
            rootNode = await treeInfo.GetRoot<WFloat>();
            rootNode.NodeInfo.NumNodes.Should().Be(numNodes); // tree is same size
            int numNodesAfterSkippingTime = await ConfirmNumNodesInStorage(treeInfo, numNodes, true);
            if (numNodesAfterSkippingTime > 0) // both 0 if testing anywhere other than in memory
                numNodesAfterSkippingTime.Should().BeLessThan(numNodesBeforeSkippingTime);
            ConfirmUintSetStorage(inMemoryUintSetCount, true);

            await thm.InitiateDeleteTreeAndHistory();
            while (await thm.GetDeletionWorkNeeded() == int.MaxValue)
            {
                await thm.DoRoundOfWork();
                await thm.UpdateWorkNeeded();
            }
            await ConfirmNumNodesInStorage(treeInfo, 0);
            ConfirmUintSetStorage(0, false);
            (await thm.GetTreeIsDeleted()).Should().Be(true);
        }

        private async static Task<int> ConfirmNumNodesInStorage(TreeInfo treeInfo, int numNodes, bool greaterThan = false)
        {
            int ns_count = 0;
            if (GetNodeStorage() is InMemoryNodeStorage)
            {
                var ns = StorageFactory.GetNodeStorage() as InMemoryNodeStorage;
                ns_count = ns.AsEnumerable<WFloat>().Count();
                if (greaterThan)
                    ns_count.Should().BeGreaterThan(numNodes);
                else
                    ns_count.Should().Be(numNodes); 
            }
            if (numNodes == 0 && !greaterThan)
            {
                var pendingChangesStorage = StorageFactory.GetPendingChangesStorage();
                if (pendingChangesStorage is PendingChangesOverTimeStorage)
                {
                    var ps = pendingChangesStorage as PendingChangesOverTimeStorage;
                    if (ps.BlobStorage.SupportsEnumeration())
                        (await ps.BlobStorage.AsEnumerable<object>()).Count().Should().Be(0);
                }
                ConfirmUintSetStorage(0, false);
            }
            return ns_count;
        }

        private static int GetInMemoryUintSetCount()
        {
            return ConfirmUintSetStorage(int.MaxValue, true);
        }

        private static int ConfirmUintSetStorage(int numberItems, bool lessThan)
        {
            var uintStorage = StorageFactory.GetUintSetStorage();
            if (uintStorage is InMemoryBlob<Guid>)
            {
                var uintSets = uintStorage as InMemoryBlob<Guid>;
                var itemsCount = uintSets.AsEnumerable().Count();
                if (lessThan)
                    itemsCount.Should().BeLessThan(numberItems);
                else
                    itemsCount.Should().Be(numberItems);
                return itemsCount;
            }
            return 0;
        }

        private static TreeUpdateSettings StandardTreeUpdateSettings =>
            new TreeUpdateSettings(10, 20, 5000, TimeSpan.FromMinutes(30));

        [Fact]
        public async Task CanChangeAndDeleteFromCountedTree()
        {
            await CanChangeAndDeleteFromCountedTree_Helper(4000, 10, 10);
        }


        internal async Task CanChangeAndDeleteFromCountedTree_Helper(int numAdditions, int numDeletions, int numChanges)
        {
            int numNodesPerInternal = 20;
            int maxItemsPerLeaf = 200;
            var ts = new TreeStructure(true, numNodesPerInternal, maxItemsPerLeaf, false, false);
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
            var pendingChanges = additions;
            var items = additions.Select(x => x.Item).OrderBy(x => x).ToList();
            long versionNumber = 0;
            treeInfo = await AddPendingChangesAndVerify(treeInfo, thm, items, pendingChanges, versionNumber);
            // Now make deletions
            var deletionItems = RandomGenerator.PickRandomItems<KeyAndID<WFloat>>(items, numDeletions);
            PendingChange<WFloat>[] deletions =
                deletionItems
                .Select(x =>
                    new PendingChange<WFloat>(
                        x,
                        true)
                    )
                .ToArray();
            pendingChanges = deletions;
            items = items.Where(x => !deletionItems.Contains(x)).ToList();
            treeInfo = await AddPendingChangesAndVerify(treeInfo, thm, items, pendingChanges, ++versionNumber);
            // And now make changes
            var changesItems = RandomGenerator.PickRandomItems<KeyAndID<WFloat>>(items, numChanges);
            deletions =
                changesItems
                .Select(x =>
                    new PendingChange<WFloat>(
                        x,
                        true)
                    )
                .ToArray();
            additions =
                changesItems
                .Select(x =>
                    new PendingChange<WFloat>(
                        new KeyAndID<WFloat>(RandomGenerator.GetRandom(0, 100F), x.ID),
                        false)
                    )
                .ToArray();
            var changes = deletions.Concat(additions).ToArray();
            pendingChanges = changes;
            items = items.Where(x => !changesItems.Contains(x)).Concat(additions.Select(x => x.Item)).OrderBy(x => x).ToList();
            treeInfo = await AddPendingChangesAndVerify(treeInfo, thm, items, pendingChanges, ++versionNumber);

        }

        private static async Task<TreeInfo> AddPendingChangesAndVerify(TreeInfo treeInfo, ITreeHistoryManager<WFloat> thm, List<KeyAndID<WFloat>> items, PendingChange<WFloat>[] pendingChanges, long versionNumber)
        {
            treeInfo = await thm.AddPendingChanges(new PendingChangesAtTime<WFloat>(GetDateTimeProvider().Now, new PendingChangesCollection<WFloat>(pendingChanges, true)), new Guid(), versionNumber);
            // verify match both before and after flushing the pending changes from the history manager to the root node
            await VerifyMatch(treeInfo, items);
            treeInfo = treeInfo = await thm.AddPendingChangesFromStorageToTree();
            await VerifyMatch(treeInfo, items);
            GetDateTimeProvider().SleepOrSkipTime(1);
            return treeInfo;
        }

        [Fact]
        public async Task CanUpdateCountedTree()
        {
            int numNodesPerInternal = 3;
            int maxItemsPerLeaf = 30;
            var ts = new TreeStructure(true, numNodesPerInternal, maxItemsPerLeaf, false, false);
            var treeInfo = await SetupTree(ts, StandardTreeUpdateSettings);
            ITreeHistoryManager<WFloat> thm = await StorageFactory.GetTreeHistoryManagerAccess().Get<WFloat>(treeInfo.TreeID);

            int numCycles = 50;
            int maxAdditionsPerCycle = 50;
            uint id = 0;
            long versionNumber = 0;
            for (int i = 0; i < numCycles; i++)
            {
                int numToAddAtOnce = RandomGenerator.GetRandom(0, maxAdditionsPerCycle);
                PendingChange<WFloat>[] additions = Enumerable.Range((int)id, numToAddAtOnce).Select(x => new PendingChange<WFloat>(new KeyAndID<WFloat>(RandomGenerator.GetRandom(0, 100.0F), (uint)x), false)).ToArray();
                id += (uint)additions.Count();
                treeInfo = await thm.AddPendingChanges(new PendingChangesAtTime<WFloat>(GetDateTimeProvider().Now, new PendingChangesCollection<WFloat>(additions, true)), new Guid(), ++versionNumber);
                treeInfo = treeInfo = await thm.AddPendingChangesFromStorageToTree();
            }
        }

        private async Task ConfirmTreeAddsUp(Guid treeID, INodeStorage storage, CountedInternalNode<WFloat> InternalNode)
        {
            List<CountedNode<WFloat>> children = await GetChildrenOfNode(treeID, storage, InternalNode);
            InternalNode.NodeInfo.MaxDepth.Should().Be(children.Select(x => x.NodeInfo.MaxDepth).Max());
            InternalNode.NodeInfo.MaxWorkNeededInSubtree.Should().Be(Math.Max(InternalNode.NodeInfo.WorkNeeded, children.Select(x => x.NodeInfo.MaxWorkNeededInSubtree).Max()));
            ((int)InternalNode.NodeInfo.NumSubtreeValues - InternalNode.MainBuffer.GetNetItemChanges()).Should().Be(children.Select(x => (int)x.NodeInfo.NumSubtreeValues).Sum());
            foreach (var child in children)
            {
                CountedInternalNode<WFloat> anotherInternalNode = child as CountedInternalNode<WFloat>;
                if (anotherInternalNode != null)
                    await ConfirmTreeAddsUp(treeID, storage, anotherInternalNode);
                else
                {
                    CountedLeafNode<WFloat> leafNode = child as CountedLeafNode<WFloat>;
                    leafNode.NodeInfo.MaxDepth.Should().Be(leafNode.NodeInfo.Depth);
                    leafNode.NodeInfo.MaxWorkNeededInSubtree.Should().Be(leafNode.NodeInfo.WorkNeeded);
                }
            }
        }

        private static async Task<List<CountedNode<WFloat>>> GetChildrenOfNode(Guid treeID, INodeStorage storage, CountedInternalNode<WFloat> InternalNode)
        {
            var childrenTasks = InternalNode.ChildNodeInfos.Select(x => x.NodeID).Select(async x => await storage.GetNode<WFloat>(treeID, x)).ToArray();
            await Task.WhenAll(childrenTasks);
            var children = childrenTasks.Select(x => x.Result).ToList();
            return children;
        }
    }
}
