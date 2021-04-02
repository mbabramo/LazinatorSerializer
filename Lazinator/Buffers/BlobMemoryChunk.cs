using System;
using System.Buffers;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Saves or loads memory references referring either to parts of a single file or to consecutively numbered files. The initial file contains information on all the other components, so that these can be asynchronously loaded if necessary.
    /// </summary>
    public class BlobMemoryChunk : MemoryChunk
    {
        string BlobPath;
        IBlobManager BlobManager;

        private MemoryChunkReference _ReferenceForLoading;
        public override MemoryChunkReference ReferenceForLoading => _ReferenceForLoading;
        public override MemoryChunkReference ReferenceOnceLoaded
        {
            get => new MemoryChunkReference(ReferenceForLoading.MemoryChunkID, 0, MemoryContainingChunk.Memory.Length);
            set => base.ReferenceOnceLoaded = value;
        }

        /// <summary>
        /// Creates a reference to an existing file other than the index file. This is called internally by GetAdditionalReferences(Async), after a call to MemoryReferenceInFile.
        /// </summary>
        /// <param name="path">The path, including a number referring to the specific file</param>
        /// <param name="length"></param>
        public BlobMemoryChunk(string path, IBlobManager blobManager, MemoryChunkReference reference, IMemoryOwner<byte> memoryContainingChunk)
        {
            BlobPath = path;
            BlobManager = blobManager;
            _ReferenceForLoading = reference;
            MemoryContainingChunk = memoryContainingChunk;
        }
        public override MemoryChunk SliceReferenceForLoading(int startIndexRelativeToBroaderMemory, int length) => new BlobMemoryChunk(BlobPath, BlobManager, new MemoryChunkReference(ReferenceForLoading.MemoryChunkID, startIndexRelativeToBroaderMemory, length), MemoryContainingChunk);

        #region Memory loading and unloading

        public override void LoadMemory()
        {
            if (IsLoaded)
                return;
            Memory<byte> bytes = BlobManager.Read(BlobPath, ReferenceForLoading.Offset, ReferenceForLoading.Length);
            // DEBUG -- what we really need to do here (and in file manager) is cache this in the blob manager. That way, there is just one memory blob for a memory chunk, even if we have references to many pieces of that chunk. Then, it can be unloaded or not. 
            MemoryContainingChunk = new SimpleMemoryOwner<byte>(bytes);
        }

        public async override ValueTask LoadMemoryAsync()
        {
            if (IsLoaded)
                return;
            Memory<byte> bytes = await BlobManager.ReadAsync(BlobPath, ReferenceForLoading.Offset, ReferenceForLoading.Length);
            MemoryContainingChunk = new SimpleMemoryOwner<byte>(bytes);
        }

        public override void ConsiderUnloadMemory()
        {
            MemoryContainingChunk = null; // Reference will now point to OriginalReference
        }

        public override ValueTask ConsiderUnloadMemoryAsync()
        {
            MemoryContainingChunk = null; // Reference will now point to OriginalReference
            return ValueTask.CompletedTask;
        }

        #endregion
    }
}
