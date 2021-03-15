using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lazinator.Exceptions;
using System.Reflection.Metadata;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Saves or loads memory references referring either to parts of a single file or to consecutively numbered files. The initial file contains information on all the other components, so that these can be asynchronously loaded if necessary.
    /// </summary>
    public class BlobMemoryReference : MemoryChunk
    {
        string BlobPath;
        IBlobManager BlobManager;
        bool ContainedInSingleBlob;
        MemoryChunkReference OriginalReference;
        public override MemoryChunkReference Reference 
        { 
            get => ReferencedMemory == null ? OriginalReference : new MemoryChunkReference(OriginalReference.MemoryChunkID, 0, ReferencedMemory.Memory.Length); 
            set => base.Reference = value; 
        }

        // DEBUG -- maybe index file should not be a BlobMemoryReference, but a BlobIndexReference.

        /// <summary>
        /// Creates a reference to the index file (which may or may not contain all of the remaining data). This should be followed by a call to GetLazinatorMemory or GetLazinatorMemoryAsync.
        /// </summary>
        /// <param name="path"></param>
        public BlobMemoryReference(string path, IBlobManager blobManager, bool containedInSingleBlob)
        {
            BlobPath = path;
            BlobManager = blobManager;
            ContainedInSingleBlob = containedInSingleBlob;
        }

        /// <summary>
        /// Creates a reference to an existing file other than the index file. This is called internally by GetAdditionalReferences(Async), after a call to MemoryReferenceInFile.
        /// </summary>
        /// <param name="path">The path, including a number referring to the specific file</param>
        /// <param name="length"></param>
        public BlobMemoryReference(string path, IBlobManager blobManager, MemoryChunkReference reference)
        {
            BlobPath = path;
            BlobManager = blobManager;
            OriginalReference = reference;
        }

        #region Memory loading and unloading

        public async override ValueTask LoadMemoryAsync()
        {
            Memory<byte> bytes = await BlobManager.ReadAsync(BlobPath, Reference.Offset, Reference.Length);
            ReferencedMemory = new SimpleMemoryOwner<byte>(bytes);
        }

        public override ValueTask ConsiderUnloadMemoryAsync()
        {
            ReferencedMemory = null; // Reference will now point to OriginalReference
            return ValueTask.CompletedTask;
        }

        #endregion

        #region Index file management

        public LazinatorMemory GetLazinatorMemory()
        {
            var references = GetAdditionalReferences(ContainedInSingleBlob);
            var firstAfterIndex = references.First();
            LazinatorMemory lazinatorMemory = new LazinatorMemory(firstAfterIndex, references.Skip(1).ToList(), 0, 0, references.Sum(x => x.Reference.Length));
            lazinatorMemory.LoadInitialMemory();
            return lazinatorMemory;
        }

        public async ValueTask<LazinatorMemory> GetLazinatorMemoryAsync()
        {
            var references = await GetAdditionalReferencesAsync(ContainedInSingleBlob);
            var firstAfterIndex = references.First();
            LazinatorMemory lazinatorMemory = new LazinatorMemory(firstAfterIndex, references.Skip(1).ToList(), 0, 0, references.Sum(x => x.Reference.Length));
            await lazinatorMemory.LoadInitialMemoryAsync();
            return lazinatorMemory;
        }

        /// <summary>
        /// Uses information in an initial MemoryReferenceInFile to generate information on other MemoryReferenceInFiles. 
        /// This should be called immediately after the constructor. 
        /// </summary>
        /// <returns></returns>
        private async Task<List<MemoryChunk>> GetAdditionalReferencesAsync(bool containedInSingleBlob)
        {
            Memory<byte> intHolder = await BlobManager.ReadAsync(BlobPath, 0, 4);
            int numBytesRead = 0;
            int numBytes = ReadUncompressedPrimitives.ToInt32(intHolder.Span, ref numBytesRead);
            Memory<byte> bytesForLengths = await BlobManager.ReadAsync(BlobPath, 4, numBytes);
            return GetChunksFromChunkReferencesStorage(containedInSingleBlob, bytesForLengths);
        }

        /// <summary>
        /// Uses information in an initial MemoryReferenceInFile to generate information on other MemoryReferenceInFiles. 
        /// This should be called immediately after the constructor. 
        /// </summary>
        /// <returns></returns>
        private List<MemoryChunk> GetAdditionalReferences(bool containedInSingleBlob)
        {
            const int bytesForLength = sizeof(int);
            Memory<byte> intHolder = BlobManager.Read(BlobPath, 0, bytesForLength);
            int numBytesRead = 0;
            int numBytes = ReadUncompressedPrimitives.ToInt32(intHolder.Span, ref numBytesRead);
            Memory<byte> memoryChunkReferencesStorage = BlobManager.Read(BlobPath, numBytesRead, numBytes);
            var chunks = GetChunksFromChunkReferencesStorage(containedInSingleBlob, memoryChunkReferencesStorage);
            return chunks;
        }

        private List<MemoryChunk> GetChunksFromChunkReferencesStorage(bool containedInSingleBlob, Memory<byte> memoryChunkReferencesStorage)
        {
            List<MemoryChunkReference> memoryChunkReferences = GetMemoryChunkReferences(memoryChunkReferencesStorage);
            List<MemoryChunk> memoryChunks = GetMemoryChunks(memoryChunkReferences, containedInSingleBlob, memoryChunkReferencesStorage.Length);
            return memoryChunks;
        }

        private List<MemoryChunkReference> GetMemoryChunkReferences(Memory<byte> memoryChunkReferencesStorage)
        {
            MemoryChunkReferenceList list = new MemoryChunkReferenceList(memoryChunkReferencesStorage);
            return list.MemoryChunkReferences;
        }

        private List<MemoryChunk> GetMemoryChunks(List<MemoryChunkReference> memoryChunkReferences, bool containedInSingleBlob, int bytesForMemoryChunkReferencesStorage)
        {
            List<MemoryChunk> memoryReferences = new List<MemoryChunk>();
            long numBytesProcessed = sizeof(int) + bytesForMemoryChunkReferencesStorage;
            for (int i = 0; i < memoryChunkReferences.Count; i++)
            {
                MemoryChunkReference memoryChunkReference = memoryChunkReferences[i];
                if (containedInSingleBlob)
                {
                    if (numBytesProcessed > int.MaxValue)
                        ThrowHelper.ThrowTooLargeException(int.MaxValue);
                    memoryChunkReference = new MemoryChunkReference(memoryChunkReferences[0].MemoryChunkID, (int)numBytesProcessed, memoryChunkReference.Length);
                }
                string referencePath = containedInSingleBlob ? BlobPath : GetPathWithNumber(BlobPath, memoryChunkReference.MemoryChunkID);
                memoryReferences.Add(new BlobMemoryReference(referencePath, BlobManager, memoryChunkReference));
                numBytesProcessed += memoryChunkReference.Length;
            }
            return memoryReferences;
        }

        public static string GetPathWithNumber(string originalPath, int i)
        {
            string withoutExtension = Path.GetFileNameWithoutExtension(originalPath);
            string withoutNumber = withoutExtension.EndsWith("-0") ? withoutExtension.Substring(0, withoutExtension.Length - 2) : withoutExtension;
            string withNumber = withoutNumber + "-" + i.ToString();
            string withExtension = withNumber + Path.GetExtension(originalPath);
            return withExtension;
        }

        #endregion
    }
}
