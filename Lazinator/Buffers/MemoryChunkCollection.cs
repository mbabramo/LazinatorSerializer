using Lazinator.Exceptions;
using Lazinator.Persistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class MemoryChunkCollection : IMemoryChunkCollection, IEnumerable<MemorySegment>
    {
        public IBlobManager BlobManager { get; set; }
        protected List<MemoryChunk> MemoryChunks = new List<MemoryChunk>();
        protected Dictionary<MemoryBlockID, int> MemoryChunksIndexFromBlockID = null;
        public long Length { get; private set; }
        public MemoryBlockID HighestMemoryBlockID { get; private set; }
        public MemoryBlockID GetNextMemoryBlockID() => HighestMemoryBlockID.Next();
        protected int NumMemoryChunks => MemoryChunks?.Count ?? 0;
        public virtual int NumMemorySegments => NumMemoryChunks;

        protected virtual int GetLengthOfSegment(int segmentIndex) => MemoryBlocksLoadingInfo[segmentIndex].MemoryBlockLength;
        protected virtual int GetOffsetIntoChunkForSegment(int segmentIndex) => 0;
        protected virtual int GetMemoryChunkIndexFromMemorySegmentIndex(int i) => i;

        #region Construction

        public MemoryChunkCollection()
        {

        }

        public MemoryChunkCollection(IEnumerable<MemoryChunk> memoryChunks)
        {
            MemoryChunks = memoryChunks.ToList();
            HighestMemoryBlockID = MemoryChunks.Any() ? new MemoryBlockID(MemoryChunks.Max(x => x.MemoryBlockID.GetIntID())) : new MemoryBlockID(0);
            Length = MemoryChunks.Sum(x => (long) x.Length);
            InitializeMemoryBlocksInformation();
            if (MemoryChunks.Count != MemoryBlocksLoadingInfo.Count)
                throw new Exception("DEBUG");
        }

        public LazinatorMemory ToLazinatorMemory()
        {
            return new LazinatorMemory(this);
        }

        public virtual MemoryChunkCollection DeepCopy()
        {
            var collection = new MemoryChunkCollection(MemoryChunks);
            return collection;
        }

        public virtual void AppendMemoryChunk(MemoryChunk memoryChunk)
        {
            if (MemoryBlocksLoadingInfo == null)
                MemoryBlocksLoadingInfo = new List<MemoryBlockLoadingInfo>();
            if (MemoryChunksIndexFromBlockID == null)
                MemoryChunksIndexFromBlockID = new Dictionary<MemoryBlockID, int>();
            MemoryBlocksLoadingInfo.Add(memoryChunk.LoadingInfo);
            MemoryChunksIndexFromBlockID[memoryChunk.MemoryBlockID] = MemoryBlocksLoadingInfo.Count() - 1;
            MemoryChunks.Add(memoryChunk);
            if (memoryChunk.MemoryBlockID > HighestMemoryBlockID)
                HighestMemoryBlockID = memoryChunk.MemoryBlockID;
            Length += (long)memoryChunk.Length;
        }

        public void SetChunks(IEnumerable<MemoryChunk> chunks)
        {
            MemoryChunks = new List<MemoryChunk>();
            Length = 0;
            int i = 0;
            if (chunks == null)
            {
                HighestMemoryBlockID = new MemoryBlockID(-1);
                return;
            }
            foreach (var chunk in chunks)
            {
                if (i == 0 || chunk.MemoryBlockID > HighestMemoryBlockID)
                    HighestMemoryBlockID = chunk.MemoryBlockID;
                MemoryChunks.Add(chunk);
                Length += (long)chunk.Length;
                i++;
            }
            InitializeMemoryBlocksInformation();
        }

        #endregion

        #region MemorySegment access

        // In MemoryChunkCollection, a segment is a chunk.

        public virtual MemorySegment MemorySegmentAtIndex(int i)
        {
            var chunk = MemoryChunkAtIndex(i);
            return new MemorySegment(chunk, new MemoryChunkSlice(0, chunk.Length));
        }

        public async virtual ValueTask<MemorySegment> MemorySegmentAtIndexAsync(int i)
        {
            var chunk = await MemoryChunkAtIndexAsync(i);
            return new MemorySegment(chunk, new MemoryChunkSlice(0, chunk.Length));
        }

        public virtual IEnumerable<MemorySegment> EnumerateMemorySegments()
        {
            for (int i = 0; i < NumMemorySegments; i++)
            {
                yield return MemorySegmentAtIndex(i);
            }
        }

        public IEnumerable<MemoryChunk> EnumerateMemoryChunks()
        {
            if (MemoryChunks != null)
                foreach (var chunk in MemoryChunks)
                    yield return chunk;
        }

        public IEnumerator<MemorySegment> GetEnumerator()
        {
            foreach (var memorySegment in EnumerateMemorySegments())
                yield return memorySegment;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int GetMemoryChunkIndexFromBlockID(MemoryBlockID memoryBlockID)
        {
            var d = GetMemoryChunkIndicesFromIDs();
            return d.ContainsKey(memoryBlockID) ? d[memoryBlockID] : -1;
        }

        public MemoryChunk GetMemoryChunkByMemoryBlockID(MemoryBlockID memoryBlockID)
        {
            var d = GetMemoryChunkIndicesFromIDs();
            if (!d.ContainsKey(memoryBlockID))
                return null;
            return MemoryChunkAtIndex(d[memoryBlockID]);
        }
        
        protected Dictionary<MemoryBlockID, int> GetMemoryChunkIndicesFromIDs()
        {
            if (MemoryChunksIndexFromBlockID == null)
                InitializeMemoryBlocksInformation();
            return MemoryChunksIndexFromBlockID;
        }

        public string ToStringByChunk()
        {
            return String.Join("; ", EnumerateMemoryChunks().Select(x => x.ToString().Replace("\n", " ")));
        }

        #endregion

        #region Memory blocks loading information
        
        private void InitializeMemoryBlocksInformation()
        {
            Dictionary<MemoryBlockID, int> d = new Dictionary<MemoryBlockID, int>(); 
            MemoryBlocksLoadingInfo = new List<MemoryBlockLoadingInfo>();
            for (int i = 0; i < MemoryChunks.Count; i++)
            {
                MemoryChunk memoryChunk = MemoryChunks[i];
                MemoryBlockID chunkID = memoryChunk.MemoryBlockID;
                if (!d.ContainsKey(chunkID))
                    d[chunkID] = i;
                MemoryBlocksLoadingInfo.Add(memoryChunk.LoadingInfo);
            }
            MemoryChunksIndexFromBlockID = d;
        }

        private void UpdateLoadingOffset(MemoryBlockID memoryBlockID, long offset)
        {
            int i = GetMemoryChunkIndicesFromIDs()[memoryBlockID];
            MemoryBlocksLoadingInfo[i] = MemoryBlocksLoadingInfo[i].WithLoadingOffset(offset);
        }

        #endregion

        #region Memory chunk creation and loading

        public MemoryChunk MemoryChunkAtIndex(int i)
        {
            if (MemoryChunks == null)
                MemoryChunks = MemoryBlocksLoadingInfo.Select(x => (MemoryChunk)null).ToList();
            if (MemoryChunks[i] == null)
                MemoryChunks[i] = CreateMemoryChunkForIndex(i);
            var chunk = MemoryChunks[i];
            return MemoryChunks[i];
        }

        public async ValueTask<MemoryChunk> MemoryChunkAtIndexAsync(int i)
        {
            if (MemoryChunks == null)
                MemoryChunks = MemoryBlocksLoadingInfo.Select(x => (MemoryChunk)null).ToList();
            if (MemoryChunks[i] == null)
                MemoryChunks[i] = await CreateMemoryChunkForIndexAsync(i);
            var chunk = MemoryChunks[i];
            return MemoryChunks[i];
        }

        private MemoryChunk CreateMemoryChunkForIndex(int i)
        {
            string path = GetPathForIndex();
            var loadingInfo = MemoryBlocksLoadingInfo[i];
            ReadOnlyMemory<byte> memory = BlobManager.Read(path, loadingInfo.GetLoadingOffset(), loadingInfo.MemoryBlockLength);
            ReadOnlyBytes readOnlyBytes = new ReadOnlyBytes(memory);
            MemoryChunk chunk = new MemoryChunk(readOnlyBytes) { IsPersisted = true };
            return chunk;
        }

        private async ValueTask<MemoryChunk> CreateMemoryChunkForIndexAsync(int i)
        {
            string path = GetPathForIndex();
            var loadingInfo = MemoryBlocksLoadingInfo[i];
            ReadOnlyMemory<byte> memory = await BlobManager.ReadAsync(path, loadingInfo.GetLoadingOffset(), loadingInfo.MemoryBlockLength);
            ReadOnlyBytes readOnlyBytes = new ReadOnlyBytes(memory);
            MemoryChunk chunk = new MemoryChunk(readOnlyBytes) { IsPersisted = true };
            return chunk;
        }

        #endregion

        #region Slicing

        public virtual MemorySegmentLocationByIndex GetMemorySegmentInfoAtOffsetFromStartPosition(int indexInitialSegment, int offsetInInitialSegment, long offsetFromStart)
        {
            long offsetRemaining = offsetFromStart;
            int revisedStartIndex = indexInitialSegment;
            int revisedStartPosition = offsetInInitialSegment;
            int numSegments = NumMemorySegments; 
            int revisedLength = 0;
            while (offsetRemaining > 0)
            {
                int segmentLength = GetLengthOfSegment(revisedStartIndex);
                int remainingBytesThisMemory = segmentLength - revisedStartPosition;
                if (remainingBytesThisMemory <= offsetRemaining)
                {
                    offsetRemaining -= remainingBytesThisMemory;
                    if (offsetRemaining == 0 && revisedStartIndex == numSegments - 1)
                        break; // we are at the very end of the last LazinatorMemory
                    revisedStartIndex++;
                    revisedStartPosition = 0;
                }
                else
                {
                    revisedStartPosition += (int)offsetRemaining;
                    revisedLength = segmentLength - revisedStartPosition;
                    offsetRemaining = 0;
                }
            }
            return new MemorySegmentLocationByIndex(revisedStartIndex, revisedStartPosition, revisedLength);
        }

        public IEnumerable<MemorySegmentLocationByIndex> EnumerateMemorySegmentLocationsByIndex(int indexInitialSegment, int offsetInInitialSegment, long length)
        {
            long bytesRemaining = length;
            int numSegments = NumMemorySegments;
            for (int index = 0; index < numSegments; index++)
            {
                int segmentLength = GetLengthOfSegment(index);
                int offsetIntoChunk = GetOffsetIntoChunkForSegment(index);
                if (index == indexInitialSegment)
                { // return memory segment starting at offset
                    int totalOffset = offsetIntoChunk + offsetInInitialSegment;
                    int lengthToInclude = (int)Math.Min(segmentLength - offsetInInitialSegment, bytesRemaining);
                    bytesRemaining -= lengthToInclude;
                    yield return new MemorySegmentLocationByIndex(index, totalOffset, lengthToInclude);
                }
                else if (index > indexInitialSegment && bytesRemaining > 0)
                { // return memory segment until end or until bytes remaining are exhausted
                    int lengthToInclude = (int)Math.Min(segmentLength, bytesRemaining);
                    bytesRemaining -= lengthToInclude;
                    yield return new MemorySegmentLocationByIndex(index, offsetIntoChunk, lengthToInclude);
                }
                if (bytesRemaining == 0)
                    yield break;
            }
        }
        
        public IEnumerable<MemorySegmentLocationByID> EnumerateMemorySegmentLocationsByID(int indexInitialSegment, int offsetInInitialSegment, long length)
        {
            foreach (MemorySegmentLocationByIndex indexAndSlice in EnumerateMemorySegmentLocationsByIndex(indexInitialSegment, offsetInInitialSegment, length))
            {
                int memorySegmentIndex = indexAndSlice.MemorySegmentIndex;
                var memoryBlockID = GetMemoryBlockIDForMemorySegmentIndex(memorySegmentIndex);
                yield return new MemorySegmentLocationByID(memoryBlockID, indexAndSlice.OffsetIntoMemoryChunk, indexAndSlice.Length);
            }
        }

        private MemoryBlockID GetMemoryBlockIDForMemorySegmentIndex(int memorySegmentIndex)
        {
            return MemoryChunkAtIndex(GetMemoryChunkIndexFromMemorySegmentIndex(memorySegmentIndex)).MemoryBlockID;
        }

        public virtual IEnumerable<MemorySegment> EnumerateMemorySegments(int indexInitialSegment, int offsetInInitialSegment, long length)
        {
            foreach (MemorySegmentLocationByIndex indexAndSlice in EnumerateMemorySegmentLocationsByIndex(indexInitialSegment, offsetInInitialSegment, length))
            {
                MemorySegment memorySegment = MemorySegmentAtIndex(indexAndSlice.MemorySegmentIndex);
                yield return memorySegment;
            }
        }

        #endregion

        #region Persistence

        internal List<MemoryChunk> GetUnpersistedMemoryChunks()
        {
            List<MemoryChunk> memoryChunks = new List<MemoryChunk>();
            HashSet<MemoryBlockID> memoryBlockIDs = new HashSet<MemoryBlockID>();
            foreach (MemoryChunk memoryChunk in MemoryChunks)
            {
                if (memoryChunk.IsPersisted)
                    continue;
                MemoryBlockID memoryBlockID = memoryChunk.MemoryBlockID;
                if (!memoryBlockIDs.Contains(memoryBlockID))
                {
                    memoryBlockIDs.Add(memoryBlockID);
                    memoryChunks.Add(memoryChunk);
                }
            }
            return memoryChunks;
        }

        public void PersistMemoryChunks(bool isInitialVersion)
        {
            var memoryChunksToPersist = GetUnpersistedMemoryChunks();

            long offset = 0;
            string pathForSingleBlob = ContainedInSingleBlob ? GetPathForMemoryBlock(new MemoryBlockID(0)) : null;
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
                MemoryBlockID memoryBlockID = memoryChunkToPersist.MemoryBlockID;
                string path = ContainedInSingleBlob ? pathForSingleBlob : GetPathForMemoryBlock(memoryBlockID); 
                if (ContainedInSingleBlob)
                {
                    BlobManager.Append(path, memoryChunkToPersist.MemoryAsLoaded.ReadOnlyMemory);
                    UpdateLoadingOffset(memoryChunkToPersist.MemoryBlockID, offset);
                    offset += memoryChunkToPersist.LoadingInfo.MemoryBlockLength;
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
            string pathForSingleBlob = ContainedInSingleBlob ? GetPathForMemoryBlock(new MemoryBlockID(0)) : null;
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
                MemoryBlockID memoryBlockID = memoryChunkToPersist.MemoryBlockID;
                string path = ContainedInSingleBlob ? pathForSingleBlob : GetPathForMemoryBlock(memoryBlockID);
                if (ContainedInSingleBlob)
                {
                    await BlobManager.AppendAsync(path, memoryChunkToPersist.MemoryAsLoaded.ReadOnlyMemory);
                    UpdateLoadingOffset(memoryChunkToPersist.MemoryBlockID, offset);
                    offset += memoryChunkToPersist.LoadingInfo.MemoryBlockLength;
                }
                else
                    BlobManager.Write(path, memoryChunkToPersist.MemoryAsLoaded.ReadOnlyMemory);
                memoryChunkToPersist.IsPersisted = true;
                OnMemoryChunkPersisted(memoryBlockID);
            }
            if (ContainedInSingleBlob)
                BlobManager.CloseAfterWriting(pathForSingleBlob);
        }

        public virtual void OnMemoryChunkPersisted(MemoryBlockID memoryBlockID)
        {

        }

        #endregion

        #region Path

        public virtual string GetPathForIndex() => GetPathHelper(BaseBlobPath, null, " Index");
        public virtual string GetPathForMemoryBlock(MemoryBlockID memoryBlockID) => GetPathHelper(BaseBlobPath, null, ContainedInSingleBlob ? " AllChunks" : (" Chunk " + memoryBlockID.ToString()));

        public static string GetPathForIndex(string baseBlobPath, IEnumerable<int> forkInformation, int versionNumber) => GetPathHelper(baseBlobPath, forkInformation, " Index " + versionNumber.ToString());

        internal static string GetPathHelper(string baseBlobPath, IEnumerable<int> forkInformation, string identifyingInformation)
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
