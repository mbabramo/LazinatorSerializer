using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazinator.Wrappers;
using R8RUtilities;
using Utility;

namespace CountedTree.UintSets
{
    public class SplitUintSetAccess : UintSetAccessBase, IUintSetAccess
    {
        long NodeID;
        Guid NodeContext;
        Guid InheritedMainSetContext;
        Guid InheritedDeltaSetsContext;
        public bool InheritMainSetContext;
        public bool InheritDeltaSetsContext;
        int MaxItemsInDeltaSet;
        bool StorageIsInMemory => Storage is InMemoryBlob<Guid>;

        public SplitUintSetAccess(long nodeID, Guid mainContext, int maxItemsInDeltaSet, bool includeLocations, IBlob<Guid> storage) : this(nodeID, mainContext, Guid.Empty, Guid.Empty, false, false, maxItemsInDeltaSet, includeLocations, storage)
        {
        }

        public SplitUintSetAccess(long nodeID, Guid mainContext, Guid inheritedMainSetContext, Guid inheritedDeltaSetsContext, bool inheritMainSetContext, bool inheritDeltaSetsContext, int maxItemsInDeltaSet, bool includeLocations, IBlob<Guid> storage)
        {
            NodeID = nodeID;
            NodeContext = mainContext;
            InheritedMainSetContext = inheritedMainSetContext;
            InheritedDeltaSetsContext = inheritedDeltaSetsContext;
            InheritMainSetContext = inheritMainSetContext;
            InheritDeltaSetsContext = inheritDeltaSetsContext;
            MaxItemsInDeltaSet = maxItemsInDeltaSet;
            IncludeLocations = includeLocations;
            base.Storage = storage;
        }

        public void SetStorage(IBlob<Guid> storage)
        {
            Storage = storage;
        }

        public override async Task Initialize(UintSet uintSet)
        {
            InheritMainSetContext = false;
            InheritDeltaSetsContext = false;
            var mainSetUpdateTask = SetMainSet(StorageIsInMemory ? uintSet.Clone() : uintSet);
            var deltaResetTask = SetDeltaSets(new UintSetDeltas());
            await Task.WhenAll(new Task[] { mainSetUpdateTask, deltaResetTask });
        }

        public override async Task InitializeWithLoc(UintSetWithLoc uintSetWithLoc)
        {
            InheritMainSetContext = false;
            InheritDeltaSetsContext = false;
            var mainSetUpdateTask = SetMainSet(StorageIsInMemory ? uintSetWithLoc.Set.Clone() : uintSetWithLoc.Set);
            var deltaResetTask = SetDeltaSets(new UintSetDeltas());
            var mainSetLocUpdateTask = SetMainSetLoc(StorageIsInMemory ? uintSetWithLoc.Loc.Clone() : uintSetWithLoc.Loc ?? new UintSetLoc(false));
            var deltaLocResetTask = SetDeltaSetsLoc(new UintSetDeltasLoc());
            await Task.WhenAll(new Task[] { mainSetUpdateTask, deltaResetTask, mainSetLocUpdateTask, deltaLocResetTask });
        }
        
        /// <summary>
        /// Changes the storage provider. This is useful when changing from temporary in-memory storage to persistent storage.
        /// </summary>
        /// <param name="revisedStorage"></param>
        /// <returns></returns>
        public async Task UpdateStorage(IBlob<Guid> revisedStorage)
        {
            UintSet mainSet = null;
            UintSetDeltas deltaSets = null;
            UintSetLoc mainLoc = null;
            UintSetDeltasLoc deltaSetsLoc = null;
            await RunAll(
                async () =>
                {
                    if (!InheritMainSetContext)
                        mainSet = await GetMainSet(false);
                }, 
                async () =>
                {
                    if (!InheritDeltaSetsContext)
                        deltaSets = await GetDeltaSets(false);
                },
                async () =>
                {
                    if (IncludeLocations && !InheritMainSetContext)
                        mainLoc = await GetMainSetLoc(false);
                },
                async () =>
                {
                    if (IncludeLocations && !InheritDeltaSetsContext)
                        deltaSetsLoc = await GetDeltaSetsLoc(false);
                }

            );
            Storage = revisedStorage;
            await RunAll(
                async () =>
                {
                    if (mainSet != null)
                        await SetMainSet(mainSet);
                },
                async () =>
                {
                    if (deltaSets != null)
                        await SetDeltaSets(deltaSets);
                },
                async () =>
                {
                    if (mainLoc != null)
                        await SetMainSetLoc(mainLoc);
                },
                async () =>
                {
                    if (deltaSetsLoc != null)
                        await SetDeltaSetsLoc(deltaSetsLoc);
                }

            );
        }
        
        /// <summary>
        /// Change a node so that it stores its own UintSet (either for the main set or the delta set), rather than using the inherited one.
        /// This is useful if we are inheriting from a set that is only in memory and that does not need to be persisted.
        /// </summary>
        /// <param name="affectMainSet"></param>
        /// <returns></returns>
        public async Task MakeNotInherited(bool affectMainSet)
        {
            // todo: where we make multiple calls, make them simultaneous.
            if (affectMainSet)
            {
                if (!InheritMainSetContext)
                    return;
                var mainSet = await GetMainSet(true);
                var mainLoc = IncludeLocations ? await GetMainSetLoc(true) : null;
                InheritMainSetContext = false;
                await SetMainSet(mainSet);
                if (mainLoc != null)
                    await SetMainSetLoc(mainLoc);
            }
            else
            {
                if (!InheritDeltaSetsContext)
                    return;
                var deltaSets = await GetDeltaSets(true);
                var deltaLoc = IncludeLocations ? await GetDeltaSetsLoc(true) : null;
                InheritDeltaSetsContext = false;
                await SetDeltaSets(deltaSets);
                if (deltaLoc != null)
                    await SetDeltaSetsLoc(deltaLoc);
            }
        }

        private async Task<UintSet> GetMainSet(bool useInheritedContext)
        {
            Guid mainSetContext = GetGuid(useInheritedContext, true, false);
            var set = await Storage.GetBlob<UintSet>(mainSetContext);
            if (set == null)
                throw new Exception("Internal error. Main set could not be found.");
            return set;
        }

        private async Task<UintSetDeltas> GetDeltaSets(bool useInheritedContext)
        {
            Guid deltaContext = GetGuid(useInheritedContext, false, false);
            var sets = await Storage.GetBlob<UintSetDeltas>(deltaContext);
            if (sets == null)
                throw new Exception("Internal error. Delta sets could not be found.");
            return sets;
        }

        private async Task<UintSetLoc> GetMainSetLoc(bool useInheritedContext)
        {
            Guid mainSetContext = GetGuid(useInheritedContext, true, true);
            var set = await Storage.GetBlob<UintSetLoc>(mainSetContext);
            if (set == null)
                throw new Exception("Internal error. Main set loc could not be found.");
            return set;
        }

        private async Task<UintSetDeltasLoc> GetDeltaSetsLoc(bool useInheritedContext)
        {
            Guid deltaContext = GetGuid(useInheritedContext, false, true);
            var sets = await Storage.GetBlob<UintSetDeltasLoc>(deltaContext);
            if (sets == null)
                throw new Exception("Internal error. Delta sets loc could not be found.");
            return sets;
        }

        public override async Task Delete()
        {
            // We never delete an inherited main set or delta set. That will get deleted when the node owning it gets deleted.

            List<Task> t = new List<Task>();
            if (!InheritMainSetContext)
                t.Add(SetMainSet(null, false));
            if (!InheritDeltaSetsContext)
                t.Add(SetDeltaSets(null, false));
            if (IncludeLocations)
            {
                if (!InheritMainSetContext)
                    t.Add(SetMainSetLoc(null, false));
                if (!InheritDeltaSetsContext)
                    t.Add(SetDeltaSetsLoc(null, false));
            }
            await Task.WhenAll(t.ToArray());
        }
        
        private async Task SetMainSet(UintSet mainSet, bool useInheritedContext = false)
        {
            bool mainSetNotDeltaSet = true;
            bool locsNotRegularUintSet = false;
            Guid mainSetContext = GetGuid(useInheritedContext, mainSetNotDeltaSet, locsNotRegularUintSet);
            await Storage.SetBlob<UintSet>(mainSetContext, mainSet);
        }

        private async Task SetDeltaSets(UintSetDeltas deltaSets, bool useInheritedContext = false)
        {
            bool mainSetNotDeltaSet = false;
            bool locsNotRegularUintSet = false;
            Guid deltaSetContext = GetGuid(useInheritedContext, mainSetNotDeltaSet, locsNotRegularUintSet);
            await Storage.SetBlob<UintSetDeltas>(deltaSetContext, deltaSets);
        }

        private async Task SetMainSetLoc(UintSetLoc mainSetLoc, bool useInheritedContext = false)
        {
            bool mainSetNotDeltaSet = true;
            bool locsNotRegularUintSet = true;
            Guid mainSetContext = GetGuid(useInheritedContext, mainSetNotDeltaSet, locsNotRegularUintSet);
            await Storage.SetBlob<UintSetLoc>(mainSetContext, mainSetLoc);
        }

        private async Task SetDeltaSetsLoc(UintSetDeltasLoc deltaSetsLoc, bool useInheritedContext = false)
        {
            bool mainSetNotDeltaSet = false;
            bool locsNotRegularUintSet = true;
            Guid deltaSetContext = GetGuid(useInheritedContext, mainSetNotDeltaSet, locsNotRegularUintSet);
            await Storage.SetBlob<UintSetDeltasLoc>(deltaSetContext, deltaSetsLoc);
        }

        private Guid GetGuid(bool useInheritedContext, bool mainSetNotDeltaSet, bool locsNotRegularUintSet)
        {
            Guid context = useInheritedContext ? GetInheritedContext(mainSetNotDeltaSet) : NodeContext;
            return MD5HashGenerator.GetDeterministicGuid(new Tuple<Guid, string, bool, bool>(context, "Split", mainSetNotDeltaSet, locsNotRegularUintSet));
        }

        private Guid GetInheritedContext(bool mainSetNotDeltaSet)
        {
            return (mainSetNotDeltaSet ? InheritedMainSetContext : InheritedDeltaSetsContext);
        }

        public override async Task Change(bool alwaysIntegrateIntoMain, List<WUInt32> indicesToRemove, List<WUInt32> indicesToAdd, List<byte> locationsToAdd)
        {
            // Load the delta sets (cloning if in memory, since we're going to change them)
            var deltaSets = await GetDeltaSets(InheritDeltaSetsContext);
            UintSetDeltasLoc deltaSetLocs = null;
            if (IncludeLocations)
                deltaSetLocs = await GetDeltaSetsLoc(InheritDeltaSetsContext);
            deltaSets = deltaSets.Update(indicesToRemove, indicesToAdd);
            if (IncludeLocations)
                deltaSetLocs = deltaSetLocs.Update(indicesToAdd, locationsToAdd, indicesToRemove);
            InheritDeltaSetsContext = false; // the delta set must be written to the current context, not an inherited one
            bool smallEnough = deltaSets.Count() < MaxItemsInDeltaSet;
            if (smallEnough && !alwaysIntegrateIntoMain)
            {
                if (IncludeLocations)
                {
                    var deltasUpdateTask = SetDeltaSets(deltaSets);
                    var deltaLocsUpdateTask = SetDeltaSetsLoc(deltaSetLocs);
                    await Task.WhenAll(new Task[] { deltasUpdateTask, deltaLocsUpdateTask });
                }
                else
                    await SetDeltaSets(deltaSets);
            }
            // Otherwise, we need to roll these changes into the main set.
            else
                await IntegrateDeltasIntoMain(deltaSets, deltaSetLocs);
        }

        private async Task IntegrateDeltasIntoMain(UintSetDeltas deltaSets, UintSetDeltasLoc deltaSetLocs)
        {
            if (deltaSetLocs == null)
            {
                var mainSet = await GetMainSet(InheritMainSetContext);
                if (InheritMainSetContext && StorageIsInMemory)
                    mainSet = mainSet.Clone(); // in case we're using in memory storage, we don't want to mutate the original
                deltaSets.IntegrateIntoUintSet(mainSet);
                InheritMainSetContext = false; // if we were inherited the main set context, we're not anymore
                InheritDeltaSetsContext = false; // and we now have an empty delta sets context
                var mainSetUpdateTask = SetMainSet(mainSet);
                var deltasResetTask = SetDeltaSets(new UintSetDeltas());
                await Task.WhenAll(new Task[] { mainSetUpdateTask, deltasResetTask });
            }
            else
            {
                // load the main set bits and locations
                var mainSetTask = GetMainSet(InheritMainSetContext);
                var mainSetLocsTask = GetMainSetLoc(InheritMainSetContext);
                var tasks = new Task[] { mainSetTask, mainSetLocsTask };
                var mainSet = ((Task<UintSet>)tasks[0]).Result;
                var mainSetLoc = ((Task<UintSetLoc>)tasks[1]).Result;
                if (InheritMainSetContext && StorageIsInMemory)
                {
                    mainSet = mainSet.Clone(); // in case we're using in memory storage, we don't want to mutate the original
                    mainSetLoc = mainSetLoc.Clone();
                }
                // integrate
                var replacementSet = deltaSetLocs.IntegrateIntoUintSetLoc(mainSet, mainSetLoc);
                mainSet = replacementSet.Set;
                mainSetLoc = replacementSet.Loc;
                // update
                InheritMainSetContext = false; // if we were inherited the main set context, we're not anymore
                InheritDeltaSetsContext = false; // and we now have an empty delta sets context
                var mainSetUpdateTask = SetMainSet(mainSet);
                var mainSetLocsUpdateTask = SetMainSetLoc(mainSetLoc);
                var deltasResetTask = SetDeltaSets(new UintSetDeltas());
                var deltaLocsResetTask = SetDeltaSetsLoc(new UintSetDeltasLoc());
                await Task.WhenAll(new Task[] { mainSetUpdateTask, mainSetLocsUpdateTask, deltasResetTask, deltaLocsResetTask });
            }
        }

        public override async Task<UintSet> GetUintSet()
        {
            UintSet mainSet = null;
            UintSetDeltas deltaSets = null;
            await RunAll(
                async () => mainSet = await GetMainSet(InheritMainSetContext),
                async () => deltaSets = await GetDeltaSets(InheritDeltaSetsContext)
                );
            if (mainSet == null)
                throw new Exception("Internal error. Main uintset was not in storage.");
            if (deltaSets == null)
                throw new Exception("Internal error. Delta uintsets not in storage.");
            // Now combine them.
            if (StorageIsInMemory)
                mainSet = mainSet.Clone();
            deltaSets.IntegrateIntoUintSet(mainSet);
            return mainSet;
        }

        public override async Task<UintSetWithLoc> GetUintSetWithLoc()
        {
            if (!IncludeLocations)
                return new UintSetWithLoc(await GetUintSet(), null);
            // Simultaneously load the main set and delta sets.
            UintSet mainSet = null;
            UintSetDeltas deltaSets = null;
            UintSetLoc mainSetLoc = null;
            UintSetDeltasLoc deltaSetsLoc = null;
            await RunAll(
                async () => mainSet = await GetMainSet(InheritMainSetContext),
                async () => deltaSets = await GetDeltaSets(InheritDeltaSetsContext),
                async () => mainSetLoc = await GetMainSetLoc(InheritMainSetContext),
                async () => deltaSetsLoc = await GetDeltaSetsLoc(InheritDeltaSetsContext)
                );

            if (mainSet == null)
                throw new Exception("Internal error. Main uintset was not in storage.");
            if (deltaSets == null)
                throw new Exception("Internal error. Delta uintsets not in storage.");
            if (mainSetLoc == null)
                throw new Exception("Internal error. Main uintset loc was not in storage.");
            if (deltaSetsLoc == null)
                throw new Exception("Internal error. Delta uintsets loc not in storage.");
            UintSetWithLoc replacementSet = deltaSetsLoc.IntegrateIntoUintSetLoc(mainSet, mainSetLoc);

            return replacementSet;
        }

        static async Task RunAll(params Func<Task>[] funcs)
        {
            await Task.WhenAll(funcs.Select(f => f()));
        }
    }
}
