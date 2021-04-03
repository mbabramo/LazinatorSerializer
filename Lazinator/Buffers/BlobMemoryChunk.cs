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

        private MemoryChunkReference LoadingReference;
        private bool MemoryAlreadyTruncated;
        public override MemoryChunkReference ReferenceForLoading => LoadingReference;
        public int MemoryChunkID => LoadingReference.MemoryChunkID;
        public int Length => LoadingReference.Length;
        public override MemoryChunkReference ReferenceOnceLoaded
        {
            get => new MemoryChunkReference(MemoryChunkID, 0, Length);
            set => base.ReferenceOnceLoaded = value;
        }

        public override Memory<byte> Memory
        {
            get
            {
                if (MemoryContainingChunk == null)
                    return LazinatorMemory.EmptyMemory;
                if (MemoryAlreadyTruncated)
                    return MemoryContainingChunk.Memory;
                return MemoryContainingChunk.Memory.Slice(ReferenceForLoading.Offset, ReferenceForLoading.Length);
            }
        }

        static int DEBUGQQ = 0;

        /// <summary>
        /// Creates a reference to an existing file other than the index file. This is called internally by GetAdditionalReferences(Async), after a call to MemoryReferenceInFile.
        /// </summary>
        public BlobMemoryChunk(string path, IBlobManager blobManager, MemoryChunkReference referenceForLoading, IMemoryOwner<byte> memoryContainingChunk, bool memoryAlreadyTruncated)
        {
            DEBUGQ = DEBUGQQ++;
            BlobPath = path;
            BlobManager = blobManager;
            LoadingReference = referenceForLoading;
            MemoryContainingChunk = memoryContainingChunk;
            MemoryAlreadyTruncated = memoryAlreadyTruncated;
            if (DEBUGQ == 5)
            {
                var DEBUGL = 0;
            }
        }
        public override MemoryChunk Slice(int startPosition, int length)
        {
            SimpleMemoryOwner<byte> memoryOwner;
            if (MemoryAlreadyTruncated)
                memoryOwner = new SimpleMemoryOwner<byte>(MemoryContainingChunk.Memory.Slice(startPosition, length));
            else
                memoryOwner = new SimpleMemoryOwner<byte>(MemoryContainingChunk.Memory.Slice(startPosition + ReferenceForLoading.Offset, length));
            var revisedReferenceForLoading = new MemoryChunkReference(ReferenceForLoading.MemoryChunkID, ReferenceForLoading.Offset + startPosition, length);
            return new BlobMemoryChunk(BlobPath, BlobManager, revisedReferenceForLoading, memoryOwner, true);
        }

        #region Memory loading and unloading

        public override void LoadMemory()
        {
            if (IsLoaded)
                return;
            Memory<byte> bytes = BlobManager.Read(BlobPath, ReferenceForLoading.Offset, ReferenceForLoading.Length);
            // DEBUG -- what we really need to do here (and in file manager) is cache this in the blob manager. That way, there is just one memory blob for a memory chunk, even if we have references to many pieces of that chunk. Then, it can be unloaded or not. 
            MemoryContainingChunk = new SimpleMemoryOwner<byte>(bytes);
            MemoryAlreadyTruncated = true;
        }

        public async override ValueTask LoadMemoryAsync()
        {
            if (IsLoaded)
                return;
            Memory<byte> bytes = await BlobManager.ReadAsync(BlobPath, ReferenceForLoading.Offset, ReferenceForLoading.Length);
            MemoryContainingChunk = new SimpleMemoryOwner<byte>(bytes);
            MemoryAlreadyTruncated = true;
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
