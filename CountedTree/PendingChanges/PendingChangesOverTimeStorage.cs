using Lazinator.Core;
using R8RUtilities;
using System;
using System.Threading.Tasks;
using Utility;

namespace CountedTree.PendingChanges
{
    public class PendingChangesOverTimeStorage
    {
        public IBlob<Guid> BlobStorage;

        public PendingChangesOverTimeStorage(IBlob<Guid> blobStorage)
        {
            BlobStorage = blobStorage;
        }

        private Guid GetFullID(Guid treeID, long nodeID)
        {
            return MD5HashGenerator.GetDeterministicGuid(Tuple.Create(treeID, nodeID, 1)); // the 1 distinguishes it from node storage
        }

        private Task<PendingChangesOverTime<TKey>> GetAllPendingChangesForNode<TKey>(Guid treeID, long nodeID) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            return GetAllPendingChangesForNode<TKey>(GetFullID(treeID, nodeID));
        }

        private Task<PendingChangesOverTime<TKey>> GetAllPendingChangesForNode<TKey>(Guid fullID) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            return BlobStorage.GetBlob<PendingChangesOverTime<TKey>>(fullID);
        }

        private Task SetAllPendingChangesForNode<TKey>(Guid treeID, long nodeID, PendingChangesOverTime<TKey> pendingChanges) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            return SetAllPendingChangesForNode<TKey>(GetFullID(treeID, nodeID), pendingChanges);
        }

        private Task SetAllPendingChangesForNode<TKey>(Guid fullID, PendingChangesOverTime<TKey> pendingChanges) where TKey : struct, ILazinator, IComparable, IComparable<TKey>, IEquatable<TKey>
        {
            return BlobStorage.SetBlob<PendingChangesOverTime<TKey>>(fullID, pendingChanges);
        }

        public Task InitializePendingChangesForNode<TKey>(Guid treeID, long nodeID) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
        {
            return SetAllPendingChangesForNode<TKey>(treeID, nodeID, new PendingChangesOverTime<TKey>(nodeID));
        }


        public Task InitializePendingChangesForNode<TKey>(Guid treeID, long nodeID, PendingChangesAtTime<TKey> pendingChanges, long versionNumber) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
        {
            var p = new PendingChangesOverTime<TKey>(nodeID);
            bool added = p.AddPendingChangesAtTime(pendingChanges, versionNumber);
            return SetAllPendingChangesForNode<TKey>(treeID, nodeID, p);
        }

        public Task RemoveAllPendingChangesForNode<TKey>(Guid treeID, long nodeID) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
        {
            return SetAllPendingChangesForNode<TKey>(treeID, nodeID, null);
        }

        public async Task AddPendingChangesAtTime<TKey>(Guid treeID, long nodeID, PendingChangesAtTime<TKey> pendingChanges, long versionNumber) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
        {
            var storedPendingChanges = await GetAllPendingChangesForNode<TKey>(treeID, nodeID);
            storedPendingChanges.AddPendingChangesAtTime(pendingChanges, versionNumber);
            await SetAllPendingChangesForNode<TKey>(treeID, nodeID, storedPendingChanges);
        }
        

        public async Task<PendingChangesCollection<TKey>> GetPendingChangesAsOfTime<TKey>(Guid treeID, long nodeID, DateTime asOfTime) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
        {
            var allPendingChanges = await GetAllPendingChangesForNode<TKey>(treeID, nodeID);
            var result = allPendingChanges.GetPendingChangesAsOfTime(asOfTime);
            return result;
        }

        public async Task<PendingChangesCollection<TKey>> GetAllPendingChanges<TKey>(Guid treeID, long nodeID) where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
        {
            var allPendingChanges = await GetAllPendingChangesForNode<TKey>(treeID, nodeID);
            var pcc = allPendingChanges.GetAllPendingChanges();
            return pcc;
        }
    }
}
