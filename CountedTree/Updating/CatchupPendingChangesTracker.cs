using CountedTree.Core;
using CountedTree.PendingChanges;
using Lazinator.Core;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CountedTree.Updating
{
    public partial class CatchupPendingChangesTracker<TKey> : ICatchupPendingChangesTracker<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        /// <summary>
        /// The maximum size of each catchup buffer. When this has been exceeded, we create a new catchup buffer.
        /// </summary>
        public static int MaxCatchupBufferSize = 5000;
        /// <summary>
        /// 
        /// </summary>
        [NonSerialized] // critical that this be nonserialized
        public bool UpdatedSinceLoad = false;

        public CatchupPendingChangesTracker()
        {
            CurrentCatchupBufferedPendingChangesID = -1;
        }

        public CatchupPendingChangesTracker<TKey> Clone()
        {
            return new CatchupPendingChangesTracker<TKey>() { LastCatchupBufferedPendingChangesIDDeleted = LastCatchupBufferedPendingChangesIDDeleted, LastCatchupBufferedPendingChangesIDAdded = LastCatchupBufferedPendingChangesIDAdded, CurrentCatchupBufferedPendingChangesID = CurrentCatchupBufferedPendingChangesID, NumPendingChangesInCurrentCatchupBuffer = NumPendingChangesInCurrentCatchupBuffer };
        }

        public async Task DeleteNoLongerNeededCatchupBufferedPendingChanges(Guid treeID, bool deletingContainingNode)
        {
            if (UpdatedSinceLoad && !deletingContainingNode)
                return; // if the node still exists, we don't want to overwrite any changes in the pending changes until this object has successfully saved
            while (LastCatchupBufferedPendingChangesIDDeleted > LastCatchupBufferedPendingChangesIDAdded)
            {
                PendingChangesOverTimeStorage ps = StorageFactory.GetPendingChangesStorage();
                LastCatchupBufferedPendingChangesIDDeleted--;
                await ps.RemoveAllPendingChangesForNode<TKey>(treeID, LastCatchupBufferedPendingChangesIDDeleted);
            }
        }

        private void GoToNextCatchupBufferIfNecessary(int numItemsToAdd)
        {
            if (NumPendingChangesInCurrentCatchupBuffer != 0 && numItemsToAdd + NumPendingChangesInCurrentCatchupBuffer > MaxCatchupBufferSize)
                RespondToFilledCatchupBuffer();
        }

        private void RespondToFilledCatchupBuffer()
        {
            CurrentCatchupBufferedPendingChangesID--;
            NumPendingChangesInCurrentCatchupBuffer = 0;
            UpdatedSinceLoad = true;
        }

        public async Task AddPendingChangesToCatchupBuffer(Guid treeID, PendingChangesAtTime<TKey> pendingChangesAtTime, long versionNumber)
        {
            GoToNextCatchupBufferIfNecessary(pendingChangesAtTime.PendingChanges.Count);
            PendingChangesOverTimeStorage ps = StorageFactory.GetPendingChangesStorage();
            if (NumPendingChangesInCurrentCatchupBuffer == 0)
                await ps.InitializePendingChangesForNode<TKey>(treeID, CurrentCatchupBufferedPendingChangesID, pendingChangesAtTime, versionNumber);
            else
                await ps.AddPendingChangesAtTime(treeID, CurrentCatchupBufferedPendingChangesID, pendingChangesAtTime, versionNumber);
            NumPendingChangesInCurrentCatchupBuffer += pendingChangesAtTime.PendingChanges.Count;
            UpdatedSinceLoad = true;
        }

        public async Task<PendingChangesCollection<TKey>> GetNextCatchupBufferToAddToPermanentStorage(Guid treeID)
        {
            PendingChangesCollection<TKey> pendingChanges = await PeekNextCatchupBufferToAddToPermanentStorage(treeID);
            LastCatchupBufferedPendingChangesIDAdded--;
            if (CurrentCatchupBufferedPendingChangesID == LastCatchupBufferedPendingChangesIDAdded)
                RespondToFilledCatchupBuffer();
            UpdatedSinceLoad = true;
            return pendingChanges;
        }

        private async Task<PendingChangesCollection<TKey>> PeekNextCatchupBufferToAddToPermanentStorage(Guid treeID)
        {
            return (await StorageFactory.GetPendingChangesStorage().GetAllPendingChanges<TKey>(treeID, LastCatchupBufferedPendingChangesIDAdded - 1));
        }

        public bool CatchupPendingChangesStored => CurrentCatchupBufferedPendingChangesID < LastCatchupBufferedPendingChangesIDAdded && (NumPendingChangesInCurrentCatchupBuffer > 0 || CurrentCatchupBufferedPendingChangesID < LastCatchupBufferedPendingChangesIDAdded - 1);

        public async Task<string> PrintOut(Guid treeID)
        {
            StringBuilder b = new StringBuilder();
            for (long i = LastCatchupBufferedPendingChangesIDDeleted - 1; i >= CurrentCatchupBufferedPendingChangesID; i++)
            {
                var pendingChanges = await StorageFactory.GetPendingChangesStorage().GetAllPendingChanges<TKey>(treeID, LastCatchupBufferedPendingChangesIDAdded);
                b.Append($"{i}: {pendingChanges}\n");
            }
            return b.ToString();
        }
    }
}
