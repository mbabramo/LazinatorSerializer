using Lazinator.Core;
using Lazinator.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Buffers;
using System.Diagnostics;

namespace Lazinator.Persistence
{
    public partial class PersistentIndex : MemoryRangeCollection, IPersistentIndex
    {

        public PersistentIndex PreviousPersistentIndex = null;

        /// <summary>
        /// Prepares for index file to be created, not based on any previous index
        /// </summary>
        /// <param name="baseBlobPath"></param>
        public PersistentIndex(string baseBlobPath, IBlobManager blobManager, bool containedInSingleBlob)
        {
            BaseBlobPath = baseBlobPath;
            BlobManager = blobManager;
            ContainedInSingleBlob = containedInSingleBlob;
            IsPersisted = false;
            IndexVersion = 0;
        }

        /// <summary>
        /// Prepares for new version of index file to be created, based on previous version of the index
        /// </summary>
        /// <param name="previousPersistentIndex"></param>
        public PersistentIndex(PersistentIndex previousPersistentIndex, int? additionalFork = null)
        {
            PreviousPersistentIndex = previousPersistentIndex;
            BaseBlobPath = previousPersistentIndex.BaseBlobPath;
            BlobManager = previousPersistentIndex.BlobManager;
            ContainedInSingleBlob = previousPersistentIndex.ContainedInSingleBlob;
            IsPersisted = false;
            IndexVersion = previousPersistentIndex.IndexVersion + 1;
            InitializeMemoryBlockStatusFromPrevious();
            if (additionalFork is int forkToAdd)
            {
                ForkInformation = previousPersistentIndex.ForkInformation?.ToList() ?? new List<(int lastMemoryBlockIDBeforeFork, int forkNumber)>();
                ForkInformation.Add((PreviousPersistentIndex.GetLastMemoryBlockID(), forkToAdd));
            }
        }

        public static PersistentIndex ReadFromBlob(IBlobManager blobManager, string baseBlobPath, IEnumerable<int> forkInformation, int versionNumber)
        {
            string path = GetPathForIndex(baseBlobPath, forkInformation, versionNumber);
            ReadOnlyMemory<byte> bytes = blobManager.ReadAll(path);
            return CreateFromBytes(blobManager, bytes);
        }

        public static async ValueTask<PersistentIndex> ReadFromBlobAsync(IBlobManager blobManager, string baseBlobPath, IEnumerable<int> forkInformation, int versionNumber)
        {
            string path = GetPathForIndex(baseBlobPath, forkInformation, versionNumber);
            ReadOnlyMemory<byte> bytes = await blobManager.ReadAllAsync(path);
            return CreateFromBytes(blobManager, bytes);
        }

        private static PersistentIndex CreateFromBytes(IBlobManager blobManager, ReadOnlyMemory<byte> bytes)
        {
            var index = new PersistentIndex(new LazinatorMemory(bytes));
            index.BlobManager = blobManager;
            return index;
        }

        private IEnumerable<int> GetForkNumbers()
        {
            if (ForkInformation == null)
                yield break;
            foreach (var fi in ForkInformation)
                yield return fi.forkNumber;
        }

        private IEnumerable<int> GetForkNumbersPrecedingMemoryBlockID(MemoryBlockID memoryBlockID)
        {
            if (ForkInformation == null)
                yield break;
            foreach (var fi in ForkInformation)
                if (fi.lastMemoryBlockIDBeforeFork < memoryBlockID.GetIntID())
                    yield return fi.forkNumber;
        }
        
        private bool MemoryBlockIsOnSameFork(MemoryBlockID memoryBlockID)
        {
            return GetForkNumbers().SequenceEqual(GetForkNumbersPrecedingMemoryBlockID(memoryBlockID));
        }
        
        public override string GetPathForIndex() => GetPathHelper(BaseBlobPath, GetForkNumbers(), " Index " + IndexVersion.ToString());
        
        public override string GetPathForMemoryBlock(MemoryBlockID memoryBlockID) => GetPathHelper(BaseBlobPath, GetForkNumbersPrecedingMemoryBlockID(memoryBlockID), ContainedInSingleBlob ? " AllBlocks" : (" Block " + memoryBlockID.ToString()));

        public PersistentIndexMemoryBlockStatus GetMemoryBlockStatus(int memoryBlockID)
        {
            if (memoryBlockID >= MemoryBlockStatus.Length)
                return PersistentIndexMemoryBlockStatus.NotYetUsed;
            return (PersistentIndexMemoryBlockStatus)MemoryBlockStatus.Span[memoryBlockID];
        }

        private void SetMemoryBlockStatus(MemoryBlockID memoryBlockID, PersistentIndexMemoryBlockStatus status)
        {
            if (memoryBlockID.GetIntID() >= MemoryBlockStatus.Length)
            {
                const int numToAddAtOnce = 10;
                byte[] memoryBlockStatus = new byte[memoryBlockID.GetIntID() + numToAddAtOnce];
                for (int i = 0; i < MemoryBlockStatus.Length; i++)
                    memoryBlockStatus[i] = (byte)MemoryBlockStatus.Span[i];
                MemoryBlockStatus = memoryBlockStatus;
            }
            MemoryBlockStatus.Span[memoryBlockID.GetIntID()] = (byte)status;
        }

        private void InitializeMemoryBlockStatusFromPrevious()
        {
            int length = PreviousPersistentIndex.MemoryBlockStatus.Length;
            byte[] updated = new byte[length];
            for (int memoryBlockID = 0; memoryBlockID < length; memoryBlockID++)
            {
                PersistentIndexMemoryBlockStatus status = PreviousPersistentIndex.GetMemoryBlockStatus(memoryBlockID);
                PersistentIndexMemoryBlockStatus revisedStatus = status switch
                {
                    PersistentIndexMemoryBlockStatus.NotYetUsed => PersistentIndexMemoryBlockStatus.NotYetUsed,
                    PersistentIndexMemoryBlockStatus.PreviouslyIncluded => PersistentIndexMemoryBlockStatus.PreviouslyIncluded,
                    PersistentIndexMemoryBlockStatus.NewlyIncluded => PersistentIndexMemoryBlockStatus.PreviouslyIncluded,
                    PersistentIndexMemoryBlockStatus.PreviouslyOmitted => PersistentIndexMemoryBlockStatus.PreviouslyOmitted,
                    PersistentIndexMemoryBlockStatus.NewlyOmitted => PersistentIndexMemoryBlockStatus.PreviouslyOmitted,
                    _ => throw new NotImplementedException()
                };
                updated[memoryBlockID] = (byte)revisedStatus;
            }
            MemoryBlockStatus = updated;
        }

        public int GetLastMemoryBlockID()
        {
            int memoryBlockID = MemoryBlockStatus.Length;
            while (GetMemoryBlockStatus(memoryBlockID) == PersistentIndexMemoryBlockStatus.NotYetUsed)
                memoryBlockID--;
            return memoryBlockID;
        }

        public LazinatorMemory GetLazinatorMemory()
        {
            LazinatorMemory lazinatorMemory = new LazinatorMemory(this);
            lazinatorMemory.LoadInitialReadOnlyMemory();
            return lazinatorMemory;
        }

        public async ValueTask<LazinatorMemory> GetLazinatorMemoryAsync()
        {
            LazinatorMemory lazinatorMemory = new LazinatorMemory(this);
            await lazinatorMemory.LoadInitialReadOnlyMemoryAsync();
            return lazinatorMemory;
        }

        public void Delete(PersistentIndexMemoryBlockStatus statusToDelete, bool includeChunksFromEarlierForks)
        {
            foreach (string path in GetPathsOfMemoryChunksToDelete(statusToDelete, includeChunksFromEarlierForks))
            {
                BlobManager.Delete(path);
            }
        }

        private IEnumerable<string> GetPathsOfMemoryChunksToDelete(PersistentIndexMemoryBlockStatus statusToDelete, bool includeChunksFromEarlierForks)
        {
            int numIDs = MemoryBlockStatus.Length;
            for (int memoryBlockID = 0; memoryBlockID < numIDs; memoryBlockID++)
            {
                PersistentIndexMemoryBlockStatus status = GetMemoryBlockStatus(memoryBlockID);
                if (status == statusToDelete)
                {
                    if (includeChunksFromEarlierForks || MemoryBlockIsOnSameFork(new MemoryBlockID(memoryBlockID)))
                    {
                        string fullPath = GetPathForMemoryBlock(new MemoryBlockID(memoryBlockID));
                        yield return fullPath;
                    }
                }
            }
        }

        public void DeleteIndex()
        {
            BlobManager.Delete(GetPathForIndex());
        }

        public void PersistLazinatorMemory(LazinatorMemory lazinatorMemory)
        {
            SetFromLazinatorMemory(lazinatorMemory);
            PersistMemoryChunks(IndexVersion == 0);
            PersistSelf();
        }

        public async ValueTask PersistLazinatorMemoryAsync(LazinatorMemory lazinatorMemory)
        {
            SetFromLazinatorMemory(lazinatorMemory);
            await PersistMemoryChunksAsync(IndexVersion == 0);
            PersistSelf();
        }

        public override void OnMemoryChunkPersisted(MemoryBlockID memoryBlockID) => SetMemoryBlockStatus(memoryBlockID, PersistentIndexMemoryBlockStatus.NewlyIncluded);

        private void PersistSelf()
        {
            string indexPath = GetPathForIndex();
            SerializeLazinator();
            ReadOnlyMemory<byte> bytes = LazinatorMemoryStorage.InitialReadOnlyMemory;
            BlobManager.Write(indexPath, bytes);
        }

        private async ValueTask PersistSelfAsync()
        {
            string indexPath = GetPathForIndex();
            SerializeLazinator();
            ReadOnlyMemory<byte> bytes = LazinatorMemoryStorage.InitialReadOnlyMemory; // this is not separable (currently), so it will all be together
            await BlobManager.WriteAsync(indexPath, bytes);
        }

    }
}
