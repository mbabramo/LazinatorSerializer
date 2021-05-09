﻿using Lazinator.Persistence;
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
    public partial class MemoryChunkCollection : IMemoryChunkCollection, IEnumerable<MemorySegment>
    {
        public IBlobManager BlobManager { get; set; }
        protected List<MemoryChunk> MemoryChunks = new List<MemoryChunk>();
        protected Dictionary<int, int> MemoryChunksIndexFromID = null;
        public long Length { get; private set; }
        public int MaxMemoryBlockID { get; private set; }
        public int GetNextMemoryBlockID() => MaxMemoryBlockID + 1;
        protected int NumMemoryChunks => MemoryChunks?.Count ?? 0;
        public virtual int NumMemorySegments => NumMemoryChunks;

        protected virtual int GetLengthOfSegment(int segmentIndex) => MemoryBlocksLoadingInfo[segmentIndex].PreTruncationLength;

        #region Construction

        public MemoryChunkCollection()
        {

        }

        public MemoryChunkCollection(IEnumerable<MemoryChunk> memoryChunks)
        {
            MemoryChunks = memoryChunks.ToList();
            MaxMemoryBlockID = MemoryChunks.Any() ? MemoryChunks.Max(x => x.MemoryBlockID) : 0;
            Length = MemoryChunks.Sum(x => (long) x.Length);
            InitializeMemoryBlocksInformation();
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
            if (MemoryChunksIndexFromID == null)
                MemoryChunksIndexFromID = new Dictionary<int, int>();
            MemoryBlocksLoadingInfo.Add(memoryChunk.LoadingInfo);
            MemoryChunksIndexFromID[memoryChunk.MemoryBlockID] = MemoryBlocksLoadingInfo.Count() - 1;
            MemoryChunks.Add(memoryChunk);
            if (memoryChunk.MemoryBlockID > MaxMemoryBlockID)
                MaxMemoryBlockID = memoryChunk.MemoryBlockID;
            Length += (long)memoryChunk.Length;
        }

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

        #endregion

        #region MemoryChunk access

        public virtual MemorySegment MemoryAtIndex(int i)
        {
            if (MemoryChunks == null)
                MemoryChunks = MemoryBlocksLoadingInfo.Select(x => (MemoryChunk)null).ToList();
            if (MemoryChunks[i] == null)
                LoadMemoryAtIndex(i);
            var chunk = MemoryChunks[i];
            return new MemorySegment(chunk, new MemoryBlockSlice(0, chunk.Length));
        }

        public async virtual ValueTask<MemorySegment> MemoryAtIndexAsync(int i)
        {
            if (MemoryChunks == null)
                MemoryChunks = MemoryBlocksLoadingInfo.Select(x => (MemoryChunk)null).ToList();
            if (MemoryChunks[i] == null)
                await LoadMemoryAtIndexAsync(i);
            var chunk = MemoryChunks[i];
            return new MemorySegment(chunk, new MemoryBlockSlice(0, chunk.Length));
        }

        public virtual IEnumerable<MemorySegment> EnumerateMemoryChunks()
        {
            if (MemoryChunks is not null)
                for (int i = 0; i < MemoryChunks.Count; i++)
                {
                    yield return MemoryAtIndex(i);
                }
        }

        public IEnumerator<MemorySegment> GetEnumerator()
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

        private async ValueTask LoadMemoryAtIndexAsync(int i)
        {
            string path = GetPathForIndex();
            var loadingInfo = MemoryBlocksLoadingInfo[i];
            ReadOnlyMemory<byte> memory = await BlobManager.ReadAsync(path, loadingInfo.GetLoadingOffset(), loadingInfo.PreTruncationLength);
            ReadOnlyBytes readOnlyBytes = new ReadOnlyBytes(memory);
            MemoryChunk chunk = new MemoryChunk(readOnlyBytes) { IsPersisted = true };
            MemoryChunks[i] = chunk;
        }

        public MemoryChunk GetMemoryChunkByMemoryBlockID(int memoryBlockID)
        {
            var d = GetMemoryChunkIndicesFromIDs();
            if (!d.ContainsKey(memoryBlockID))
                return null;
            return MemoryChunks[d[memoryBlockID]];
        }

        private Dictionary<int, int> GetMemoryChunkIndicesFromIDs()
        {
            if (MemoryChunksIndexFromID == null)
                InitializeMemoryBlocksInformation();
            return MemoryChunksIndexFromID;
        }

        #endregion

        #region Memory blocks loading information
        private void InitializeMemoryBlocksInformation()
        {
            Dictionary<int, int> d = new Dictionary<int, int>(); 
            MemoryBlocksLoadingInfo = new List<MemoryBlockLoadingInfo>();
            for (int i = 0; i < MemoryChunks.Count; i++)
            {
                MemoryChunk memoryChunk = MemoryChunks[i];
                int chunkID = memoryChunk.MemoryBlockID;
                if (!d.ContainsKey(chunkID))
                {
                    d[chunkID] = i;
                    MemoryBlocksLoadingInfo.Add(memoryChunk.LoadingInfo);
                }
            }
            MemoryChunksIndexFromID = d;
        }

        private void UpdateLoadingOffset(int memoryBlockID, long offset)
        {
            int i = GetMemoryChunkIndicesFromIDs()[memoryBlockID];
            MemoryBlocksLoadingInfo[i] = MemoryBlocksLoadingInfo[i].WithLoadingOffset(offset);
        }

        #endregion

        #region Slicing

        public virtual MemorySegmentIndexAndSlice GetMemorySegmentAtOffsetFromStartPosition(int indexInitialSegment, int offsetInInitialSegment, long offsetFromStart)
        {
            long positionRemaining = offsetFromStart;
            int revisedStartIndex = indexInitialSegment;
            int revisedStartPosition = offsetInInitialSegment;
            int numChunks = NumMemoryChunks; 
            int revisedLength = 0;
            while (positionRemaining > 0)
            {
                int segmentLength = GetLengthOfSegment(revisedStartIndex);
                int remainingBytesThisMemory = segmentLength - revisedStartPosition;
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
                    revisedLength = segmentLength - revisedStartPosition;
                    positionRemaining = 0;
                }
            }
            return new MemorySegmentIndexAndSlice(revisedStartIndex, revisedStartPosition, revisedLength);
        }

        public virtual IEnumerable<MemorySegmentIndexAndSlice> EnumerateMemorySegmentIndexAndSlices(int indexInitialSegment, int offsetInInitialSegment, long length)
        {
            long bytesRemaining = length;
            int numSegments = NumMemorySegments;
            for (int index = 0; index < numSegments; index++)
            {
                int segmentLength = GetLengthOfSegment(index);
                if (index == indexInitialSegment)
                { // return memory block starting at offset
                    int lengthToInclude = (int)Math.Min(segmentLength - offsetInInitialSegment, bytesRemaining);
                    bytesRemaining -= lengthToInclude;
                    yield return new MemorySegmentIndexAndSlice(index, offsetInInitialSegment, lengthToInclude);
                }
                else if (index > indexInitialSegment && bytesRemaining > 0)
                { // return memory block until end or until bytes remaining are exhausted
                    int lengthToInclude = (int)Math.Min(segmentLength, bytesRemaining);
                    bytesRemaining -= lengthToInclude;
                    yield return new MemorySegmentIndexAndSlice(index, 0, lengthToInclude);
                }
                if (bytesRemaining == 0)
                    yield break;
            }
        }

        public IEnumerable<MemoryBlockIDAndSlice> EnumerateMemoryBlockIDAndSlices(int indexInitialSegment, int offsetInInitialSegment, long length)
        {
            foreach (MemorySegmentIndexAndSlice indexAndSlice in EnumerateMemorySegmentIndexAndSlices(indexInitialSegment, offsetInInitialSegment, length))
            {
                yield return new MemoryBlockIDAndSlice(MemoryChunks[indexAndSlice.MemorySegmentIndex].MemoryBlockID, indexAndSlice.Offset, indexAndSlice.Length);
            }
        }

        public IEnumerable<MemorySegment> EnumerateMemorySegments(int indexInitialSegment, int offsetInInitialSegment, long length)
        {
            foreach (MemorySegmentIndexAndSlice indexAndSlice in EnumerateMemorySegmentIndexAndSlices(indexInitialSegment, offsetInInitialSegment, length))
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
