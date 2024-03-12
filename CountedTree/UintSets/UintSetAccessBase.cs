using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lazinator.Wrappers;
using R8RUtilities;

namespace CountedTree.UintSets
{
    public abstract class UintSetAccessBase : IUintSetAccess
    {
        public abstract Task Change(bool alwaysIntegrateIntoMain, List<WUInt32> indicesToRemove, List<WUInt32> indicesToAdd, List<byte> locationsToAdd);
        public abstract Task<UintSet> GetUintSet();
        public abstract Task<UintSetWithLoc> GetUintSetWithLoc();
        public abstract Task Initialize(UintSet uintSet);
        public abstract Task InitializeWithLoc(UintSetWithLoc uintSetWithLoc);
        public abstract Task Delete();

        public bool IncludeLocations { get; protected set; }
        public IBlob<Guid> Storage;


        public virtual async Task Initialize()
        {
            await Initialize(new UintSet());
        }

        public virtual async Task InitializeWithLoc(bool noMoreThan16ChildrenPerNode)
        {
            await InitializeWithLoc(new UintSetWithLoc(new UintSet(), new UintSetLoc(noMoreThan16ChildrenPerNode)));
        }
    }
}
