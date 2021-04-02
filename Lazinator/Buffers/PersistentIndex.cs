using Lazinator.Core;
using Lazinator.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class PersistentIndex : IPersistentIndex, IPersistentLazinator
    {

        IBlobManager BlobManager;

        int _BytesUsedForIndexWithPrefix;

        /// <summary>
        /// Creates a reference to an index file to be created (which may or may not contain all of the remaining data).
        /// </summary>
        /// <param name="path"></param>
        public PersistentIndex(string path, IBlobManager blobManager, bool containedInSingleBlob)
        {
            BlobPath = path;
            BlobManager = blobManager;
            ContainedInSingleBlob = containedInSingleBlob;
            IsPersisted = false;
        }

        public static PersistentIndex ReadFromBlobWithIntPrefix(IBlobManager blobManager, string blobPath)
        {
            Memory<byte> intHolder = blobManager.Read(blobPath, 0, 4);
            int numBytesRead = 0;
            int numBytes = ReadUncompressedPrimitives.ToInt32(intHolder.Span, ref numBytesRead);
            Memory<byte> mainBytes = blobManager.Read(blobPath, 4, numBytes);
            return CreateFromBytes(blobManager, mainBytes);
        }

        public static async ValueTask<PersistentIndex> ReadFromBlobWithIntPrefixAsync(IBlobManager blobManager, string blobPath)
        {
            Memory<byte> intHolder = await blobManager.ReadAsync(blobPath, 0, 4);
            int numBytesRead = 0;
            int numBytes = ReadUncompressedPrimitives.ToInt32(intHolder.Span, ref numBytesRead);
            Memory<byte> mainBytes = await blobManager.ReadAsync(blobPath, 4, numBytes);
            return CreateFromBytes(blobManager, mainBytes);
        }

        private static PersistentIndex CreateFromBytes(IBlobManager blobManager, Memory<byte> mainBytes)
        {
            var index = new PersistentIndex(new LazinatorMemory(new SimpleMemoryOwner<byte>(mainBytes)));
            index._BytesUsedForIndexWithPrefix = sizeof(int) + mainBytes.Length;
            index.BlobManager = blobManager;
            return index;
        }

        public LazinatorMemory GetLazinatorMemory()
        {
            var memoryChunks = GetMemoryChunks();
            LazinatorMemory lazinatorMemory = new LazinatorMemory(memoryChunks.First(), memoryChunks.Skip(1).ToList(), 0, 0, memoryChunks.Sum(x => x.ReferenceForLoading.Length));
            lazinatorMemory.LoadInitialMemory();
            return lazinatorMemory;
        }

        public async ValueTask<LazinatorMemory> GetLazinatorMemoryAsync()
        {
            var memoryChunks = GetMemoryChunks();
            LazinatorMemory lazinatorMemory = new LazinatorMemory(memoryChunks.First(), memoryChunks.Skip(1).ToList(), 0, 0, memoryChunks.Sum(x => x.ReferenceForLoading.Length));
            await lazinatorMemory.LoadInitialMemoryAsync();
            return lazinatorMemory;
        }


        private List<MemoryChunk> GetMemoryChunks()
        {
            List<MemoryChunk> memoryChunks = new List<MemoryChunk>();
            long numBytesProcessed = _BytesUsedForIndexWithPrefix;
            for (int i = 0; i < MemoryChunkReferences.Count; i++)
            {
                MemoryChunkReference memoryChunkReference = MemoryChunkReferences[i];
                if (ContainedInSingleBlob)
                {
                    if (numBytesProcessed > int.MaxValue)
                        ThrowHelper.ThrowTooLargeException(int.MaxValue);
                    memoryChunkReference = new MemoryChunkReference(MemoryChunkReferences[0].MemoryChunkID, (int)numBytesProcessed, memoryChunkReference.Length);
                }
                string referencePath = ContainedInSingleBlob ? BlobPath : GetPathForMemoryChunk(BlobPath, memoryChunkReference.MemoryChunkID);
                memoryChunks.Add(new BlobMemoryChunk(referencePath, (IBlobManager)this.BlobManager, memoryChunkReference, null));
                numBytesProcessed += memoryChunkReference.Length;
            }
            return memoryChunks;
        }

        public static string GetPathForMemoryChunk(string indexBlobPath, int i)
        {
            string withoutExtension = Path.GetFileNameWithoutExtension(indexBlobPath);
            string withoutNumber = withoutExtension.EndsWith("-0") ? withoutExtension.Substring(0, withoutExtension.Length - 2) : withoutExtension;
            string withNumber = withoutNumber + "-" + i.ToString();
            string withExtension = withNumber + Path.GetExtension(indexBlobPath);
            return withExtension;
        }

        public void CombineToSameChunk(int memoryChunkID, bool separateReferences) => CombineToSameChunk(0, MemoryChunkReferences.Count(), memoryChunkID, separateReferences);

        /// <summary>
        /// Combine entries in the memory chunk reference list, in preparation for writing multiple entries to a single blob. 
        /// The entries can be renumbered to a new memory chunk ID. Meanwhile, the entries can be combined into a single 
        /// reference only (if we want these separate chunks to be read into memory all at once) or their independence
        /// can be maintained but sharing the same memory chunk ID, since they will be in the same blob file. 
        /// </summary>
        /// <param name="startingWithIndex"></param>
        /// <param name="numEntriesToCompact"></param>
        /// <param name="memoryChunkID"></param>
        /// <param name="singleReferenceOnly"></param>
        public void CombineToSameChunk(int startingWithIndex, int numEntriesToCompact, int memoryChunkID, bool separateReferences)
        {
            var revisedReferences = new List<MemoryChunkReference>();
            int initialNumReferences = MemoryChunkReferences.Count();
            for (int i = 0; i < initialNumReferences; i++)
            {
                if ((i <= startingWithIndex && separateReferences) || (i < startingWithIndex && !separateReferences) || i >= startingWithIndex + numEntriesToCompact)
                    revisedReferences.Add(MemoryChunkReferences[i]);
                else
                {
                    if (!separateReferences)
                    {
                        var original = revisedReferences[startingWithIndex];
                        revisedReferences[startingWithIndex] = new MemoryChunkReference(memoryChunkID, original.Offset, original.Length + MemoryChunkReferences[i].Length);
                    }
                    else
                    {
                        var original = MemoryChunkReferences[i];
                        var previous = revisedReferences[i - 1];
                        MemoryChunkReference toAdd = new MemoryChunkReference(memoryChunkID, previous.Offset + previous.Length, original.Length);
                        revisedReferences.Add(toAdd);
                    }
                }
            }
            MemoryChunkReferences = revisedReferences;
        }

        public void Delete(bool memoryDroppedFromPreviousIndex, bool memoryUsedInThisIndex, bool indexItself)
        {
            throw new NotImplementedException();
        }
        
        public ValueTask DeleteAsync(bool memoryDroppedFromPreviousIndex, bool memoryUsedInThisIndex, bool indexItself)
        {

            throw new NotImplementedException();
        }

        public IPersistentLazinator PersistLazinatorMemory(LazinatorMemory lazinatorMemory)
        {
            GetChunksAndWriterAssumingSuccessfulPersistence(lazinatorMemory, out List<MemoryChunk> chunks, out List<bool> actuallyPersisted, out BinaryBufferWriter writer);

            if (ContainedInSingleBlob)
            {
                BlobManager.OpenForWriting(BlobPath);
                BlobManager.Append(BlobPath, writer.ActiveMemoryWritten);
            }
            else
                BlobManager.Write(BlobPath, writer.ActiveMemoryWritten);

            try
            {
                long totalBytes = writer.ActiveMemoryPosition;
                for (int i = 0; i < chunks.Count; i++)
                {
                    MemoryChunk chunk = chunks[i];
                    bool mustWriteChunk = ContainedInSingleBlob || !actuallyPersisted[i];
                    if (mustWriteChunk)
                    {
                        if (!chunk.IsLoaded)
                            chunk.LoadMemory();
                        GetBlobMemoryChunkAndInfo(chunk, totalBytes, i, !ContainedInSingleBlob, out Memory<byte> memory, out string revisedPath, out BlobMemoryChunk blobMemoryChunk);
                        if (ContainedInSingleBlob)
                            BlobManager.Append(revisedPath, memory);
                        else if (!blobMemoryChunk.IsPersisted)
                            BlobManager.Write(revisedPath, memory);
                        if (ContainedInSingleBlob && i == chunks.Count - 1)
                            BlobManager.CloseAfterWriting(revisedPath);
                    }
                    totalBytes += chunk.ReferenceOnceLoaded.Length;
                    actuallyPersisted[i] = true;
                }
            }
            catch
            {
                for (int i = 0; i < chunks.Count; i++)
                {
                    MemoryChunk chunk = chunks[i];
                    chunk.IsPersisted = actuallyPersisted[i];
                }
                throw;
            }
            return this;
        }

        public async ValueTask<IPersistentLazinator> PersistLazinatorMemoryAsync(LazinatorMemory lazinatorMemory)
        {
            GetChunksAndWriterAssumingSuccessfulPersistence(lazinatorMemory, out List<MemoryChunk> chunks, out List<bool> actuallyPersisted, out BinaryBufferWriter writer);

            if (ContainedInSingleBlob)
            {
                BlobManager.OpenForWriting(BlobPath);
                await BlobManager.AppendAsync(BlobPath, writer.ActiveMemoryWritten);
            }
            else
                await BlobManager.WriteAsync(BlobPath, writer.ActiveMemoryWritten);

            try
            {
                long totalBytes = writer.ActiveMemoryPosition;
                for (int i = 0; i < chunks.Count; i++)
                {
                    MemoryChunk chunk = chunks[i];
                    bool mustWriteChunk = ContainedInSingleBlob || !actuallyPersisted[i];
                    if (mustWriteChunk)
                    {
                        if (!chunk.IsLoaded)
                            await chunk.LoadMemoryAsync();
                        GetBlobMemoryChunkAndInfo(chunk, totalBytes, i, !ContainedInSingleBlob, out Memory<byte> memory, out string revisedPath, out BlobMemoryChunk blobMemoryChunk);
                        if (ContainedInSingleBlob)
                            await BlobManager.AppendAsync(revisedPath, memory);
                        else if (!blobMemoryChunk.IsPersisted)
                            await BlobManager.WriteAsync(revisedPath, memory);
                        if (ContainedInSingleBlob && i == chunks.Count - 1)
                            BlobManager.CloseAfterWriting(revisedPath);
                    }
                    totalBytes += chunk.ReferenceOnceLoaded.Length;
                    actuallyPersisted[i] = true;
                }
            }
            catch
            {
                for (int i = 0; i < chunks.Count; i++)
                {
                    MemoryChunk chunk = chunks[i];
                    chunk.IsPersisted = actuallyPersisted[i];
                }
                throw;
            }
            return this;
        }

        /// <summary>
        /// Gets memory chunks all set so that they are persisted, but remembering their actual persistence state
        /// in case an error occurs. 
        /// </summary>
        private void GetChunksAndWriterAssumingSuccessfulPersistence(LazinatorMemory lazinatorMemory, out List<MemoryChunk> chunks, out List<bool> actuallyPersisted, out BinaryBufferWriter writer)
        {
            chunks = GetMemoryChunksAndSetReferences(lazinatorMemory);
            actuallyPersisted = chunks.Select(x => x.IsPersisted).ToList();
            foreach (var chunk in chunks)
                chunk.IsPersisted = true;
            writer = GetBinaryBufferWriterWithIndex();
            IsPersisted = true;
        }


        private List<MemoryChunk> GetMemoryChunksAndSetReferences(LazinatorMemory lazinatorMemory)
        {
            var chunks = lazinatorMemory.EnumerateMemoryChunks().ToList();
            MemoryChunkReferences = chunks.Select(x => x.ReferenceOnceLoaded).ToList();
            return chunks;
        }

        private void GetBlobMemoryChunkAndInfo(MemoryChunk chunk, long numBytesWritten, int i, bool includeMemoryContainingChunk, out Memory<byte> memory, out string revisedPath, out BlobMemoryChunk blobMemoryChunk)
        {
            MemoryChunkReference reference = chunk.ReferenceOnceLoaded;
            if (chunk.IsLoaded)
                memory = includeMemoryContainingChunk ? chunk.MemoryContainingChunk.Memory : chunk.Memory ;
            else
                memory = null;
            revisedPath = ContainedInSingleBlob ? BlobPath : GetPathForMemoryChunk(BlobPath, reference.MemoryChunkID);
            blobMemoryChunk = new BlobMemoryChunk(revisedPath, (IBlobManager)this.BlobManager, this.ContainedInSingleBlob ? new MemoryChunkReference(reference.MemoryChunkID, (int)numBytesWritten, chunk.ReferenceOnceLoaded.Length) : reference, null);
        }

        private BinaryBufferWriter GetBinaryBufferWriterWithIndex()
        {

            BinaryBufferWriter writer = new BinaryBufferWriter();
            writer.SetLengthsPosition(0);
            writer.Skip(4);
            SerializeToExistingBuffer(ref writer, LazinatorSerializationOptions.Default);
            int memoryUsed = writer.ActiveMemoryPosition - 4;
            writer.RecordLength(memoryUsed);
            _BytesUsedForIndexWithPrefix = writer.ActiveMemoryPosition;

            return writer;
        }

        private BinaryBufferWriter RecordIndexInformation(PersistentIndex list, bool containedInSingleBlob)
        {
            BinaryBufferWriter writer = new BinaryBufferWriter();
            writer.SetLengthsPosition(0);
            writer.Skip(4);
            if (containedInSingleBlob)
                list.CombineToSameChunk(0, true); // writing everything to a single blob, so always use MemoryChunkID of 0, but with multiple references to the blob
            list.SerializeToExistingBuffer(ref writer, LazinatorSerializationOptions.Default);
            int memoryUsed = writer.ActiveMemoryPosition - 4;
            writer.RecordLength(memoryUsed);
            return writer;
        }

    }
}
