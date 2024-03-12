using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazinator.Wrappers;
using R8RUtilities;
using Utility;

namespace CountedTree.UintSets
{
    public class WholeUintSetAccess : UintSetAccessBase, IUintSetAccess
    {
        Guid Context;
        Guid LocationsContext => MD5HashGenerator.GetDeterministicGuid(new Tuple<string, Guid>("LocCtx", Context));

        public WholeUintSetAccess(Guid context, bool includeLocations, IBlob<Guid> storage)
        {
            Context = context;
            IncludeLocations = includeLocations;
            base.Storage = storage;
        }

        private async Task StoreUintSetAndLoc(UintSet u, UintSetLoc l = null)
        {
            if (IncludeLocations)
                await Task.WhenAll(new Task[]
                {
                    Storage.SetBlob<UintSet>(Context, u),
                    Storage.SetBlob<UintSetLoc>(LocationsContext, l)
                });
            else
                await Storage.SetBlob<UintSet>(Context, u);
        }



        public override Task<UintSet> GetUintSet()
        {
            var set = Storage.GetBlob<UintSet>(Context);
            return set;
        }

        public override async Task<UintSetWithLoc> GetUintSetWithLoc()
        {
            UintSetWithLoc uwl = new UintSetWithLoc();
            if (IncludeLocations)
            {
                Task<UintSet> tSet = Storage.GetBlob<UintSet>(Context);
                Task<UintSetLoc> lSet = Storage.GetBlob<UintSetLoc>(LocationsContext);
                await Task.WhenAll(new Task[]
                {
                    tSet,
                    lSet
                });
                uwl.Set = tSet.Result;
                uwl.Loc = lSet.Result;
            }
            else
                uwl.Set = await Storage.GetBlob<UintSet>(Context);
            return uwl;
        }

        public override async Task Initialize(UintSet uintSet)
        {
            await StoreUintSetAndLoc(uintSet.Clone(), new UintSetLoc(false));
        }

        public override async Task InitializeWithLoc(UintSetWithLoc uintSetWithLoc)
        {
            await StoreUintSetAndLoc(uintSetWithLoc.Set.Clone(), uintSetWithLoc.Loc.Clone());
        }

        public override async Task Delete()
        {
            await Initialize(null);
        }

        public override async Task Change(bool alwaysIntegrateIntoMain, List<WUInt32> indicesToRemove, List<WUInt32> indicesToAdd, List<byte> locationsToAdd)
        {
            UintSetWithLoc setWithLoc = await GetUintSetWithLoc();
            var set = setWithLoc.Set;
            var loc = setWithLoc.Loc;
            UintSetWithLoc revisedSetWithLoc = null;
            if (loc != null)
            { // change the base set and deltas
                UintSetDeltasLoc d = new UintSetDeltasLoc(indicesToAdd, locationsToAdd, indicesToRemove);
                revisedSetWithLoc = d.IntegrateIntoUintSetLoc(set, loc);
            }
            else
            { // only need to change the base set

                var revisedSet = set.Clone();
                revisedSet.AddUints(indicesToAdd.AsEnumerable());
                revisedSet.RemoveUints(indicesToRemove.AsEnumerable());
                revisedSetWithLoc = new UintSetWithLoc(revisedSet, null);
            }
            await StoreUintSetAndLoc(revisedSetWithLoc.Set, revisedSetWithLoc.Loc);
        }
    }
}
