using Lazinator.Core;
using Lazinator.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Buffers;

namespace Lazinator.Persistence
{
    public partial class PersistentIndex : IPersistentIndex
    {

        IBlobManager BlobManager;

        private PersistentIndex PreviousPersistentIndex = null;

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
        public PersistentIndex(PersistentIndex previousPersistentIndex)
        {
            PreviousPersistentIndex = previousPersistentIndex;
            BaseBlobPath = previousPersistentIndex.BaseBlobPath;
            BlobManager = previousPersistentIndex.BlobManager;
            ContainedInSingleBlob = previousPersistentIndex.ContainedInSingleBlob;
            IsPersisted = false;
            IndexVersion = previousPersistentIndex.IndexVersion + 1;
            InitializeMemoryChunkStatusFromPrevious();
        }

        public static PersistentIndex ReadFromBlob(IBlobManager blobManager, string baseBlobPath, IEnumerable<int> forkInformation, int versionNumber)
        {
            string path = GetPathForIndex(baseBlobPath, forkInformation, versionNumber);
            Memory<byte> bytes = blobManager.ReadAll(path);
            return CreateFromBytes(blobManager, bytes);
        }

        public static async ValueTask<PersistentIndex> ReadFromBlobAsync(IBlobManager blobManager, string baseBlobPath, IEnumerable<int> forkInformation, int versionNumber)
        {
            string path = GetPathForIndex(baseBlobPath, forkInformation, versionNumber);
            Memory<byte> bytes = await blobManager.ReadAllAsync(path);
            return CreateFromBytes(blobManager, bytes);
        }

        private static PersistentIndex CreateFromBytes(IBlobManager blobManager, Memory<byte> bytes)
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

        private IEnumerable<int> GetForkNumbersPrecedingMemoryChunkID(int memoryChunkID)
        {
            if (ForkInformation == null)
                yield break;
            foreach (var fi in ForkInformation)
                if (fi.lastMemoryChunkBeforeFork < memoryChunkID)
                    yield return fi.forkNumber;
        }

        private bool MemoryChunkIsOnSameFork(int memoryChunkID)
        {
            return GetForkNumbers().SequenceEqual(GetForkNumbersPrecedingMemoryChunkID(memoryChunkID));
        }

        public string GetPathForIndex() => GetPathHelper(BaseBlobPath, GetForkNumbers(), " Index " + IndexVersion.ToString());
        public string GetPathForMemoryChunk(int memoryChunkID) => GetPathHelper(BaseBlobPath, GetForkNumbersPrecedingMemoryChunkID(memoryChunkID), ContainedInSingleBlob ? " AllChunks" : (" Chunk " + memoryChunkID.ToString()));

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

        public PersistentIndexMemoryChunkStatus GetMemoryChunkStatus(int memoryChunkID)
        {
            if (memoryChunkID >= MemoryChunkStatus.Length)
                return PersistentIndexMemoryChunkStatus.NotYetUsed;
            return (PersistentIndexMemoryChunkStatus)MemoryChunkStatus.Span[memoryChunkID];
        }

        private void SetMemoryChunkStatus(int memoryChunkID, PersistentIndexMemoryChunkStatus status)
        {
            if (memoryChunkID >= MemoryChunkStatus.Length)
            {
                const int numToAddAtOnce = 10;
                byte[] memoryChunkStatus = new byte[memoryChunkID + numToAddAtOnce];
                for (int i = 0; i < MemoryChunkStatus.Length; i++)
                    memoryChunkStatus[i] = (byte)MemoryChunkStatus.Span[i];
                MemoryChunkStatus = memoryChunkStatus;
            }
            MemoryChunkStatus.Span[memoryChunkID] = (byte)status;
        }

        private void InitializeMemoryChunkStatusFromPrevious()
        {
            int length = PreviousPersistentIndex.MemoryChunkStatus.Length;
            byte[] updated = new byte[length];
            for (int memoryChunkID = 0; memoryChunkID < length; memoryChunkID++)
            {
                PersistentIndexMemoryChunkStatus status = PreviousPersistentIndex.GetMemoryChunkStatus(memoryChunkID);
                PersistentIndexMemoryChunkStatus revisedStatus = status switch
                {
                    PersistentIndexMemoryChunkStatus.NotYetUsed => PersistentIndexMemoryChunkStatus.NotYetUsed,
                    PersistentIndexMemoryChunkStatus.PreviouslyIncluded => PersistentIndexMemoryChunkStatus.PreviouslyIncluded,
                    PersistentIndexMemoryChunkStatus.NewlyIncluded => PersistentIndexMemoryChunkStatus.PreviouslyIncluded,
                    PersistentIndexMemoryChunkStatus.PreviouslyOmitted => PersistentIndexMemoryChunkStatus.PreviouslyOmitted,
                    PersistentIndexMemoryChunkStatus.NewlyOmitted => PersistentIndexMemoryChunkStatus.PreviouslyOmitted,
                    _ => throw new NotImplementedException()
                };
                updated[memoryChunkID] = (byte)revisedStatus;
            }
            MemoryChunkStatus = updated;
        }

        public LazinatorMemory GetLazinatorMemory()
        {
            var memoryChunks = GetMemoryChunks();
            LazinatorMemory lazinatorMemory = new LazinatorMemory(memoryChunks.First(), memoryChunks.Skip(1).ToList(), 0, 0, memoryChunks.Sum(x => x.Reference.FinalLength));
            lazinatorMemory.LoadInitialMemory();
            return lazinatorMemory;
        }

        public async ValueTask<LazinatorMemory> GetLazinatorMemoryAsync()
        {
            var memoryChunks = GetMemoryChunks();
            LazinatorMemory lazinatorMemory = new LazinatorMemory(memoryChunks.First(), memoryChunks.Skip(1).ToList(), 0, 0, memoryChunks.Sum(x => x.Reference.FinalLength));
            await lazinatorMemory.LoadInitialMemoryAsync();
            return lazinatorMemory;
        }

        private List<MemoryChunk> GetMemoryChunks()
        {
            List<MemoryChunk> memoryChunks = new List<MemoryChunk>();
            long numBytesProcessed = 0;
            for (int i = 0; i < MemoryChunkReferences.Count; i++)
            {
                MemoryChunkReference memoryChunkReference = MemoryChunkReferences[i];
                string referencePath = GetPathForMemoryChunk(memoryChunkReference.MemoryChunkID);
                var blobMemoryChunk = new BlobMemoryChunk(referencePath, (IBlobManager)this.BlobManager, memoryChunkReference);
                blobMemoryChunk.IsPersisted = GetMemoryChunkStatus(blobMemoryChunk.MemoryChunkID) != PersistentIndexMemoryChunkStatus.NotYetUsed;
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
            for (int memoryChunkID = 0; memoryChunkID < numIDs; memoryChunkID++)
            {
                PersistentIndexMemoryChunkStatus status = GetMemoryChunkStatus(memoryChunkID);
                if (status == statusToDelete)
                {
                    if (includeChunksFromEarlierForks || MemoryChunkIsOnSameFork(memoryChunkID))
                    {
                        string fullPath = GetPathForMemoryChunk(memoryChunkID);
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
            var memoryChunks = lazinatorMemory.EnumerateMemoryChunks().ToList();
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
                int memoryChunkID = memoryChunkToPersist.MemoryChunkID;
                string path = GetPathForMemoryChunk(memoryChunkID);
                var status = GetMemoryChunkStatus(memoryChunkID);
                if (status != PersistentIndexMemoryChunkStatus.NotYetUsed)
                    throw new Exception($"There is or was previously memory persisted for chunk ID {memoryChunkID}");
                if (ContainedInSingleBlob)
                {
                    BlobManager.Append(path, memoryChunkToPersist.MemoryAsLoaded.Memory);
                    UpdateMemoryChunkReferenceToLoadingOffset(memoryChunkToPersist.MemoryChunkID, offset);
                    offset += memoryChunkToPersist.Memory.Length;
                }
                else
                    BlobManager.Write(path, memoryChunkToPersist.MemoryAsLoaded.Memory);
                memoryChunkToPersist.IsPersisted = true;
                SetMemoryChunkStatus(memoryChunkID, PersistentIndexMemoryChunkStatus.NewlyIncluded);
            }
            if (ContainedInSingleBlob)
                BlobManager.CloseAfterWriting(pathForSingleBlob);

            PersistSelf();
        }

        public async ValueTask PersistLazinatorMemoryAsync(LazinatorMemory lazinatorMemory)
        {
            var memoryChunks = lazinatorMemory.EnumerateMemoryChunks().ToList();
            var memoryChunksToPersist = lazinatorMemory.GetUnpersistedMemoryChunks();
            MemoryChunkReferences = memoryChunks.Select(x => x.Reference).ToList();

            long offset = 0;
            string pathForSingleBlob = ContainedInSingleBlob ? GetPathForMemoryChunk(0) : null;
            if (ContainedInSingleBlob)
            {
                offset = IndexVersion == 0 ? 0 : BlobManager.GetLength(pathForSingleBlob);
                BlobManager.OpenForWriting(pathForSingleBlob);
            }
            foreach (var memoryChunkToPersist in memoryChunksToPersist)
            {
                await memoryChunkToPersist.LoadMemoryAsync();
                int memoryChunkID = memoryChunkToPersist.MemoryChunkID;
                string path = GetPathForMemoryChunk(memoryChunkID);
                var status = GetMemoryChunkStatus(memoryChunkID);
                if (status != PersistentIndexMemoryChunkStatus.NotYetUsed)
                    throw new Exception($"There is or was previously memory persisted for chunk ID {memoryChunkID}");
                if (ContainedInSingleBlob)
                {
                    await BlobManager.AppendAsync(path, memoryChunkToPersist.MemoryAsLoaded.Memory);
                    UpdateMemoryChunkReferenceToLoadingOffset(memoryChunkToPersist.MemoryChunkID, offset);
                    offset += memoryChunkToPersist.Memory.Length;
                }
                else
                    await BlobManager.WriteAsync(path, memoryChunkToPersist.MemoryAsLoaded.Memory);
                memoryChunkToPersist.IsPersisted = true;
                SetMemoryChunkStatus(memoryChunkID, PersistentIndexMemoryChunkStatus.NewlyIncluded);
            }
            if (ContainedInSingleBlob)
                BlobManager.CloseAfterWriting(pathForSingleBlob);

            await PersistSelfAsync();
        }

        private void PersistSelf()
        {
            string indexPath = GetPathForIndex();
            SerializeLazinator();
            Memory<byte> bytes = LazinatorMemoryStorage.Memory;
            BlobManager.Write(indexPath, bytes);
        }

        private async ValueTask PersistSelfAsync()
        {
            string indexPath = GetPathForIndex();
            SerializeLazinator();
            Memory<byte> bytes = LazinatorMemoryStorage.Memory; // this is not separable (currently), so it will all be together
            await BlobManager.WriteAsync(indexPath, bytes);
        }

        private void UpdateMemoryChunkReferenceToLoadingOffset(int memoryChunkID, long offset)
        {
            for (int i = 0; i < MemoryChunkReferences.Count; i++)
                if (MemoryChunkReferences[i].MemoryChunkID == memoryChunkID)
                    MemoryChunkReferences[i] = MemoryChunkReferences[i].WithLoadingOffset(offset);
        }

    }
}
