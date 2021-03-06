﻿using Lazinator.Core;
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
    public partial class PersistentIndex : IPersistentIndex
    {

        IBlobManager BlobManager;

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
            InitializeMemoryChunkStatusFromPrevious();
            if (additionalFork is int forkToAdd)
            {
                ForkInformation = previousPersistentIndex.ForkInformation?.ToList() ?? new List<(int lastMemoryChunkBeforeFork, int forkNumber)>();
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

        private IEnumerable<int> GetForkNumbersPrecedingMemoryBlockID(int memoryBlockID)
        {
            if (ForkInformation == null)
                yield break;
            foreach (var fi in ForkInformation)
                if (fi.lastMemoryChunkBeforeFork < memoryBlockID)
                    yield return fi.forkNumber;
        }

        private bool MemoryChunkIsOnSameFork(int memoryBlockID)
        {
            return GetForkNumbers().SequenceEqual(GetForkNumbersPrecedingMemoryBlockID(memoryBlockID));
        }

        public string GetPathForIndex() => GetPathHelper(BaseBlobPath, GetForkNumbers(), " Index " + IndexVersion.ToString());
        public string GetPathForMemoryChunk(int memoryBlockID) => GetPathHelper(BaseBlobPath, GetForkNumbersPrecedingMemoryBlockID(memoryBlockID), ContainedInSingleBlob ? " AllChunks" : (" Chunk " + memoryBlockID.ToString()));

        private static string GetPathForIndex(string baseBlobPath, IEnumerable<int> forkInformation, int versionNumber) => GetPathHelper(baseBlobPath, forkInformation, " Index " + versionNumber.ToString());

        private static string GetPathHelper(string baseBlobPath, IEnumerable<int> forkInformation, string identifyingInformation)
        {
            string pathOnly = Path.GetDirectoryName(baseBlobPath);
            if (pathOnly != null && pathOnly.Length > 0)
                pathOnly += "\\";
            string withoutExtension = Path.GetFileNameWithoutExtension(baseBlobPath);
            string forkInformationString = forkInformation == null || !forkInformation.Any() ? "" : " " + String.Join(",", forkInformation);
            string combined = pathOnly + withoutExtension + forkInformationString + identifyingInformation + Path.GetExtension(baseBlobPath);
            return combined;
        }

        public PersistentIndexMemoryChunkStatus GetMemoryChunkStatus(int memoryBlockID)
        {
            if (memoryBlockID >= MemoryChunkStatus.Length)
                return PersistentIndexMemoryChunkStatus.NotYetUsed;
            return (PersistentIndexMemoryChunkStatus)MemoryChunkStatus.Span[memoryBlockID];
        }

        private void SetMemoryChunkStatus(int memoryBlockID, PersistentIndexMemoryChunkStatus status)
        {
            if (memoryBlockID >= MemoryChunkStatus.Length)
            {
                const int numToAddAtOnce = 10;
                byte[] memoryChunkStatus = new byte[memoryBlockID + numToAddAtOnce];
                for (int i = 0; i < MemoryChunkStatus.Length; i++)
                    memoryChunkStatus[i] = (byte)MemoryChunkStatus.Span[i];
                MemoryChunkStatus = memoryChunkStatus;
            }
            MemoryChunkStatus.Span[memoryBlockID] = (byte)status;
        }

        private void InitializeMemoryChunkStatusFromPrevious()
        {
            int length = PreviousPersistentIndex.MemoryChunkStatus.Length;
            byte[] updated = new byte[length];
            for (int memoryBlockID = 0; memoryBlockID < length; memoryBlockID++)
            {
                PersistentIndexMemoryChunkStatus status = PreviousPersistentIndex.GetMemoryChunkStatus(memoryBlockID);
                PersistentIndexMemoryChunkStatus revisedStatus = status switch
                {
                    PersistentIndexMemoryChunkStatus.NotYetUsed => PersistentIndexMemoryChunkStatus.NotYetUsed,
                    PersistentIndexMemoryChunkStatus.PreviouslyIncluded => PersistentIndexMemoryChunkStatus.PreviouslyIncluded,
                    PersistentIndexMemoryChunkStatus.NewlyIncluded => PersistentIndexMemoryChunkStatus.PreviouslyIncluded,
                    PersistentIndexMemoryChunkStatus.PreviouslyOmitted => PersistentIndexMemoryChunkStatus.PreviouslyOmitted,
                    PersistentIndexMemoryChunkStatus.NewlyOmitted => PersistentIndexMemoryChunkStatus.PreviouslyOmitted,
                    _ => throw new NotImplementedException()
                };
                updated[memoryBlockID] = (byte)revisedStatus;
            }
            MemoryChunkStatus = updated;
        }

        public int GetLastMemoryBlockID()
        {
            int memoryBlockID = MemoryChunkStatus.Length;
            while (GetMemoryChunkStatus(memoryBlockID) == PersistentIndexMemoryChunkStatus.NotYetUsed)
                memoryBlockID--;
            return memoryBlockID;
        }

        public LazinatorMemory GetLazinatorMemory()
        {
            var memoryChunks = GetMemoryChunks();
            LazinatorMemory lazinatorMemory = new LazinatorMemory(memoryChunks.ToList(), 0, 0, memoryChunks.Sum(x => x.Length));
            lazinatorMemory.LoadInitialReadOnlyMemory();
            return lazinatorMemory;
        }

        public async ValueTask<LazinatorMemory> GetLazinatorMemoryAsync()
        {
            var memoryChunks = GetMemoryChunks();
            LazinatorMemory lazinatorMemory = new LazinatorMemory(memoryChunks.ToList(), 0, 0, memoryChunks.Sum(x => x.Length));
            await lazinatorMemory.LoadInitialReadOnlyMemoryAsync();
            return lazinatorMemory;
        }

        private List<MemoryChunk> GetMemoryChunks()
        {
            List<MemoryChunk> memoryChunks = new List<MemoryChunk>();
            long numBytesProcessed = 0;
            for (int i = 0; i < MemoryChunkReferences.Count; i++)
            {
                MemoryChunkReference memoryChunkReference = MemoryChunkReferences[i];
                string referencePath = GetPathForMemoryChunk(memoryChunkReference.MemoryBlockID);
                var blobMemoryChunk = new BlobMemoryChunk(referencePath, (IBlobManager)this.BlobManager, memoryChunkReference.LoadingInfo, memoryChunkReference.SliceInfo);
                blobMemoryChunk.IsPersisted = GetMemoryChunkStatus(blobMemoryChunk.MemoryBlockID) != PersistentIndexMemoryChunkStatus.NotYetUsed;
                memoryChunks.Add(blobMemoryChunk);
                numBytesProcessed += memoryChunkReference.FinalLength;
            }
            return memoryChunks;
        }

        public void Delete(PersistentIndexMemoryChunkStatus statusToDelete, bool includeChunksFromEarlierForks)
        {
            foreach (string path in GetPathsOfMemoryChunksToDelete(statusToDelete, includeChunksFromEarlierForks))
            {
                BlobManager.Delete(path);
            }
        }

        private IEnumerable<string> GetPathsOfMemoryChunksToDelete(PersistentIndexMemoryChunkStatus statusToDelete, bool includeChunksFromEarlierForks)
        {
            int numIDs = MemoryChunkStatus.Length;
            for (int memoryBlockID = 0; memoryBlockID < numIDs; memoryBlockID++)
            {
                PersistentIndexMemoryChunkStatus status = GetMemoryChunkStatus(memoryBlockID);
                if (status == statusToDelete)
                {
                    if (includeChunksFromEarlierForks || MemoryChunkIsOnSameFork(memoryBlockID))
                    {
                        string fullPath = GetPathForMemoryChunk(memoryBlockID);
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
            var memoryChunks = lazinatorMemory.EnumerateMemoryChunks(false).ToList();
            var memoryChunksToPersist = lazinatorMemory.GetUnpersistedMemoryChunks();
            MemoryChunkReferences = memoryChunks.Select(x => x.Reference).ToList();

            long offset = 0;
            string pathForSingleBlob = ContainedInSingleBlob ? GetPathForMemoryChunk(0) : null;
            if (ContainedInSingleBlob)
            {
                if (IndexVersion == 0)
                {
                    if (BlobManager.Exists(pathForSingleBlob))
                        BlobManager.Delete(pathForSingleBlob);
                    offset = 0;
                }
                else
                    offset = BlobManager.GetLength(pathForSingleBlob);
                BlobManager.OpenForWriting(pathForSingleBlob);
            }
            foreach (var memoryChunkToPersist in memoryChunksToPersist)
            {
                memoryChunkToPersist.LoadMemory();
                int memoryBlockID = memoryChunkToPersist.MemoryBlockID;
                string path = GetPathForMemoryChunk(memoryBlockID);
                var status = GetMemoryChunkStatus(memoryBlockID);
                if (status != PersistentIndexMemoryChunkStatus.NotYetUsed)
                    throw new Exception($"There is or was previously memory persisted for chunk ID {memoryBlockID}");
                if (ContainedInSingleBlob)
                {
                    BlobManager.Append(path, memoryChunkToPersist.MemoryAsLoaded.ReadOnlyMemory);
                    UpdateMemoryChunkReferenceToLoadingOffset(memoryChunkToPersist.MemoryBlockID, offset);
                    offset += memoryChunkToPersist.LoadingInfo.PreTruncationLength;
                }
                else
                    BlobManager.Write(path, memoryChunkToPersist.MemoryAsLoaded.ReadOnlyMemory);
                memoryChunkToPersist.IsPersisted = true;
                SetMemoryChunkStatus(memoryBlockID, PersistentIndexMemoryChunkStatus.NewlyIncluded);
            }
            if (ContainedInSingleBlob)
                BlobManager.CloseAfterWriting(pathForSingleBlob);

            PersistSelf();
        }

        public async ValueTask PersistLazinatorMemoryAsync(LazinatorMemory lazinatorMemory)
        {
            var memoryChunks = lazinatorMemory.EnumerateMemoryChunks(false).ToList();
            var memoryChunksToPersist = lazinatorMemory.GetUnpersistedMemoryChunks();
            MemoryChunkReferences = memoryChunks.Select(x => x.Reference).ToList();

            long offset = 0;
            string pathForSingleBlob = ContainedInSingleBlob ? GetPathForMemoryChunk(0) : null;
            if (ContainedInSingleBlob)
            {
                if (IndexVersion == 0)
                {
                    if (BlobManager.Exists(pathForSingleBlob))
                        BlobManager.Delete(pathForSingleBlob);
                    offset = 0;
                }
                else
                    offset = BlobManager.GetLength(pathForSingleBlob);
                BlobManager.OpenForWriting(pathForSingleBlob);
            }
            foreach (var memoryChunkToPersist in memoryChunksToPersist)
            {
                await memoryChunkToPersist.LoadMemoryAsync();
                int memoryBlockID = memoryChunkToPersist.MemoryBlockID;
                string path = GetPathForMemoryChunk(memoryBlockID);
                var status = GetMemoryChunkStatus(memoryBlockID);
                if (status != PersistentIndexMemoryChunkStatus.NotYetUsed)
                    throw new Exception($"There is or was previously memory persisted for chunk ID {memoryBlockID}");
                if (ContainedInSingleBlob)
                {
                    await BlobManager.AppendAsync(path, memoryChunkToPersist.MemoryAsLoaded.ReadOnlyMemory);
                    UpdateMemoryChunkReferenceToLoadingOffset(memoryChunkToPersist.MemoryBlockID, offset);
                    offset += memoryChunkToPersist.LoadingInfo.PreTruncationLength;
                }
                else
                    await BlobManager.WriteAsync(path, memoryChunkToPersist.MemoryAsLoaded.ReadOnlyMemory);
                memoryChunkToPersist.IsPersisted = true;
                SetMemoryChunkStatus(memoryBlockID, PersistentIndexMemoryChunkStatus.NewlyIncluded);
            }
            if (ContainedInSingleBlob)
                BlobManager.CloseAfterWriting(pathForSingleBlob);

            PersistSelf();
        }

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

        private void UpdateMemoryChunkReferenceToLoadingOffset(int memoryBlockID, long offset)
        {
            for (int i = 0; i < MemoryChunkReferences.Count; i++)
                if (MemoryChunkReferences[i].MemoryBlockID == memoryBlockID)
                    MemoryChunkReferences[i] = MemoryChunkReferences[i].WithLoadingOffset(offset);
        }

    }
}
