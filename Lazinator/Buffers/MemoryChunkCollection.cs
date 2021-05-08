using Lazinator.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class MemoryChunkCollection : IMemoryChunkCollection, IEnumerable<MemoryChunk>
    {
        public IBlobManager BlobManager { get; set; }
        protected List<MemoryChunk> MemoryChunks = new List<MemoryChunk>();
        protected Dictionary<int, MemoryChunk> MemoryChunksByID = null;
        public long Length { get; private set; }
        public int MaxMemoryBlockID { get; private set; }
        public int GetNextMemoryBlockID() => MaxMemoryBlockID + 1;
        public int NumMemoryChunks => MemoryChunks?.Count ?? 0;

        #region Construction

        public MemoryChunkCollection()
        {

        }

        public MemoryChunkCollection(List<MemoryChunk> memoryChunks)
        {
            MemoryChunks = memoryChunks;
            MaxMemoryBlockID = MemoryChunks.Any() ? MemoryChunks.Max(x => x.MemoryBlockID) : 0;
            Length = MemoryChunks.Sum(x => (long) x.Length);
            InitializeMemoryBlocksInformation();
        }

        public LazinatorMemory ToLazinatorMemory()
        {
            return new LazinatorMemory(this);
        }

        public MemoryChunkCollection DeepCopy()
        {
            var collection = new MemoryChunkCollection();
            collection.SetChunks(MemoryChunks);
            return collection;
        }

        public virtual MemoryChunkCollection WithAppendedMemoryChunk(MemoryChunk memoryChunk)
        {
            List<MemoryChunk> memoryChunks = MemoryChunks.ToList();
            var collection = new MemoryChunkCollection(memoryChunks);
            collection.MemoryChunksByID = null;
            collection.AppendMemoryChunk(memoryChunk);
            return collection;
        }

        public void AppendMemoryChunk(MemoryChunk memoryChunk)
        {
            bool alreadyContained = false;
            alreadyContained = MemoryChunksByID?.ContainsKey(memoryChunk.MemoryBlockID) ?? false;
            if (!alreadyContained)
            {
                if (MemoryBlocksLoadingInfo == null)
                    MemoryBlocksLoadingInfo = new List<MemoryBlockLoadingInfo>();
                if (MemoryChunksByID == null)
                    MemoryChunksByID = new Dictionary<int, MemoryChunk>();
                MemoryBlocksLoadingInfo.Add(memoryChunk.LoadingInfo);
                MemoryChunksByID[memoryChunk.MemoryBlockID] = memoryChunk;
            }
            MemoryChunks.Add(memoryChunk);
            if (memoryChunk.MemoryBlockID > MaxMemoryBlockID)
                MaxMemoryBlockID = memoryChunk.MemoryBlockID;
            Length += (long)memoryChunk.Length;
        }

        #endregion

        #region MemoryChunk access

        public virtual MemoryChunk MemoryAtIndex(int i)
        {
            if (MemoryChunks == null)
                MemoryChunks = MemoryBlocksLoadingInfo.Select(x => (MemoryChunk)null).ToList();
            if (MemoryChunks[i] == null)
                LoadMemoryAtIndex(i);
            return MemoryChunks[i];
        }

        public virtual IEnumerable<MemoryChunk> EnumerateMemoryChunks()
        {
            if (MemoryChunks is not null)
                for (int i = 0; i < MemoryChunks.Count; i++)
                {
                    yield return MemoryAtIndex(i);
                }
        }

        public IEnumerator<MemoryChunk> GetEnumerator()
        {
            foreach (var memoryChunk in EnumerateMemoryChunks())
                yield return memoryChunk;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // DEBUG TODO -- delete BlobMemoryChunk and replace with a chunk type that is connected to MemoryChunkCollection, so that we can load and unload. In this case, we could just create the MemoryChunks in advance, but maybe there is no reason to do this before it is needed. Also make it so that we can unload after persisting all. But then we need to ensure that if a MemoryChunk calls unload, the unloading can be accomplished here. Ideally, we should make it so that MemoryChunk is purely internal and is never accessed by the consumer code. So long as all the MemoryChunks are stored in a MemoryChunkCollection, just unloading the one memory chunk should suffice. Users may still have access to ReadOnlyMemory<byte>, however. (MemoryChunks can be sliced, but if it's internal, that will only be done by this library.) 

        private void LoadMemoryAtIndex(int i)
        {
            string path = GetPathForIndex();
            var loadingInfo = MemoryBlocksLoadingInfo[i];
            ReadOnlyMemory<byte> memory = BlobManager.Read(path, loadingInfo.GetLoadingOffset(), loadingInfo.PreTruncationLength);
            ReadOnlyBytes readOnlyBytes = new ReadOnlyBytes(memory);
            MemoryChunk chunk = new MemoryChunk(readOnlyBytes) { IsPersisted = true }; 
            MemoryChunks[i] = chunk;
        }

        public MemoryChunk GetMemoryChunkByMemoryBlockID(int memoryBlockID)
        {
            var d = GetMemoryChunksByMemoryBlockID();
            if (!d.ContainsKey(memoryBlockID))
                return null;
            return d[memoryBlockID];
        }

        public Dictionary<int, MemoryChunk> GetMemoryChunksByMemoryBlockID()
        {
            if (MemoryChunksByID == null)
                InitializeMemoryBlocksInformation();
            return MemoryChunksByID;
        }

        #endregion

        #region Memory blocks loading information

        public void SetChunks(IEnumerable<MemoryChunk> chunks)
        {
            MemoryChunks = new List<MemoryChunk>();
            Length = 0;
            int i = 0;
            if (chunks == null)
            {
                MaxMemoryBlockID = -1;
                return;
            }
            foreach (var chunk in chunks)
            {
                if (i == 0 || chunk.MemoryBlockID > MaxMemoryBlockID)
                    MaxMemoryBlockID = chunk.MemoryBlockID;
                MemoryChunks.Add(chunk);
                Length += (long)chunk.Length;
                i++;
            }
            InitializeMemoryBlocksInformation();
        }

        private void InitializeMemoryBlocksInformation()
        {
            Dictionary<int, MemoryChunk> d = new Dictionary<int, MemoryChunk>(); 
            MemoryBlocksLoadingInfo = new List<MemoryBlockLoadingInfo>();
            foreach (MemoryChunk memoryChunk in MemoryChunks)
            {
                int chunkID = memoryChunk.MemoryBlockID;
                if (!d.ContainsKey(chunkID))
                {
                    d[chunkID] = memoryChunk;
                    MemoryBlocksLoadingInfo.Add(memoryChunk.LoadingInfo);
                }
            }
            MemoryChunksByID = d;
        }

        private void UpdateLoadingOffset(int memoryBlockID, long offset)
        {
            for (int i = 0; i < MemoryBlocksLoadingInfo.Count; i++)
                if (MemoryBlocksLoadingInfo[i].MemoryBlockID == memoryBlockID)
                    MemoryBlocksLoadingInfo[i] = MemoryBlocksLoadingInfo[i].WithLoadingOffset(offset);
        }

        #endregion

        #region Slicing

        public virtual MemorySegmentIndexAndSlice GetMemorySegmentAtOffset(int startIndex, int startOffset, long furtherOffset)
        {
            long positionRemaining = furtherOffset;
            int revisedStartIndex = startIndex;
            int revisedStartPosition = startOffset;
            int numChunks = NumMemoryChunks; 
            int revisedLength = 0;
            while (positionRemaining > 0)
            {
                MemoryBlockLoadingInfo current = MemoryBlocksLoadingInfo[revisedStartIndex];
                int remainingBytesThisMemory = current.PreTruncationLength - revisedStartPosition;
                if (remainingBytesThisMemory <= positionRemaining)
                {
                    positionRemaining -= remainingBytesThisMemory;
                    if (positionRemaining == 0 && revisedStartIndex == numChunks - 1)
                        break; // we are at the very end of the last LazinatorMemory
                    revisedStartIndex++;
                    revisedStartPosition = 0;
                }
                else
                {
                    revisedStartPosition += (int)positionRemaining;
                    revisedLength = current.PreTruncationLength - revisedStartPosition;
                    positionRemaining = 0;
                }
            }
            return new MemorySegmentIndexAndSlice(revisedStartIndex, revisedStartPosition, revisedLength);
        }

        debug; // add length to next. Then use above to get starting position, and call below.

        public virtual IEnumerable<MemorySegmentIndexAndSlice> EnumerateMemoryBlockIndexAndSlice(int startIndex, int offset)
        {
            int index = 0;
            long bytesRemaining = Length;
            int i = 0;
            foreach (var memoryChunk in EnumerateMemoryChunks())
            {
                if (index == startIndex)
                {
                    int lengthToInclude = (int)Math.Min(memoryChunk.Length - offset, bytesRemaining);
                    bytesRemaining -= lengthToInclude;
                    yield return new MemorySegmentIndexAndSlice(i, offset, lengthToInclude);
                }
                else if (index > startIndex && bytesRemaining > 0)
                {
                    int lengthToInclude = (int)Math.Min(memoryChunk.Length, bytesRemaining);
                    bytesRemaining -= lengthToInclude;
                    yield return new MemorySegmentIndexAndSlice(i, 0, lengthToInclude);
                }
                if (bytesRemaining == 0)
                    yield break;
                i++;
            }
        }

        public virtual IEnumerable<MemoryBlockIDAndSlice> EnumerateMemoryBlockIDAndSlice(int startIndex, int offset)
        {
            foreach (MemorySegmentIndexAndSlice indexAndSlice in EnumerateMemoryBlockIndexAndSlice(startIndex, offset))
            {
                yield return new MemoryBlockIDAndSlice(MemoryChunks[indexAndSlice.MemorySegmentIndex].MemoryBlockID, indexAndSlice.Offset, indexAndSlice.Length);
            }
        }

        public virtual IEnumerable<MemorySegment> EnumerateMemorySegment(int startIndex, int offset)
        {
            foreach (MemorySegmentIndexAndSlice indexAndSlice in EnumerateMemoryBlockIndexAndSlice(startIndex, offset))
            {
                yield return new MemorySegment(MemoryChunks[indexAndSlice.MemorySegmentIndex], new MemoryBlockSlice(indexAndSlice.Offset, indexAndSlice.Length));
            }
        }

        #endregion

        #region Persistence

        internal List<MemoryChunk> GetUnpersistedMemoryChunks()
        {
            List<MemoryChunk> memoryChunks = new List<MemoryChunk>();
            HashSet<int> ids = new HashSet<int>();
            foreach (MemoryChunk memoryChunk in MemoryChunks)
            {
                if (memoryChunk.IsPersisted)
                    continue;
                int chunkID = memoryChunk.MemoryBlockID;
                if (!ids.Contains(chunkID))
                {
                    ids.Add(chunkID);
                    memoryChunks.Add(memoryChunk);
                }
            }
            return memoryChunks;
        }

        public void PersistMemoryChunks(bool isInitialVersion)
        {
            var memoryChunksToPersist = GetUnpersistedMemoryChunks();

            long offset = 0;
            string pathForSingleBlob = ContainedInSingleBlob ? GetPathForMemoryChunk(0) : null;
            if (ContainedInSingleBlob)
            {
                if (isInitialVersion)
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
                if (ContainedInSingleBlob)
                {
                    BlobManager.Append(path, memoryChunkToPersist.MemoryAsLoaded.ReadOnlyMemory);
                    UpdateLoadingOffset(memoryChunkToPersist.MemoryBlockID, offset);
                    offset += memoryChunkToPersist.LoadingInfo.PreTruncationLength;
                }
                else
                    BlobManager.Write(path, memoryChunkToPersist.MemoryAsLoaded.ReadOnlyMemory);
                memoryChunkToPersist.IsPersisted = true;
                OnMemoryChunkPersisted(memoryBlockID);
            }
            if (ContainedInSingleBlob)
                BlobManager.CloseAfterWriting(pathForSingleBlob);
        }

        public async ValueTask PersistMemoryChunksAsync(bool isInitialVersion)
        {
            var memoryChunksToPersist = GetUnpersistedMemoryChunks();

            long offset = 0;
            string pathForSingleBlob = ContainedInSingleBlob ? GetPathForMemoryChunk(0) : null;
            if (ContainedInSingleBlob)
            {
                if (isInitialVersion)
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
                if (ContainedInSingleBlob)
                {
                    await BlobManager.AppendAsync(path, memoryChunkToPersist.MemoryAsLoaded.ReadOnlyMemory);
                    UpdateLoadingOffset(memoryChunkToPersist.MemoryBlockID, offset);
                    offset += memoryChunkToPersist.LoadingInfo.PreTruncationLength;
                }
                else
                    BlobManager.Write(path, memoryChunkToPersist.MemoryAsLoaded.ReadOnlyMemory);
                memoryChunkToPersist.IsPersisted = true;
                OnMemoryChunkPersisted(memoryBlockID);
            }
            if (ContainedInSingleBlob)
                BlobManager.CloseAfterWriting(pathForSingleBlob);
        }

        public virtual void OnMemoryChunkPersisted(int memoryBlockID)
        {

        }

        #endregion

        #region Path

        public virtual string GetPathForIndex() => GetPathHelper(BaseBlobPath, null, " Index");
        public virtual string GetPathForMemoryChunk(int memoryBlockID) => GetPathHelper(BaseBlobPath, null, ContainedInSingleBlob ? " AllChunks" : (" Chunk " + memoryBlockID.ToString()));

        public static string GetPathForIndex(string baseBlobPath, IEnumerable<int> forkInformation, int versionNumber) => GetPathHelper(baseBlobPath, forkInformation, " Index " + versionNumber.ToString());

        public static string GetPathHelper(string baseBlobPath, IEnumerable<int> forkInformation, string identifyingInformation)
        {
            string pathOnly = Path.GetDirectoryName(baseBlobPath);
            if (pathOnly != null && pathOnly.Length > 0)
                pathOnly += "\\";
            string withoutExtension = Path.GetFileNameWithoutExtension(baseBlobPath);
            string forkInformationString = forkInformation == null || !forkInformation.Any() ? "" : " " + String.Join(",", forkInformation);
            string combined = pathOnly + withoutExtension + forkInformationString + identifyingInformation + Path.GetExtension(baseBlobPath);
            return combined;
        }

        #endregion
    }
}
