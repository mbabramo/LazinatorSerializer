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
    public class BlobMemoryChunk : MemoryChunk
    {
        string BlobPath;
        IBlobManager BlobManager;

        MemoryChunkReference OriginalReference;
        public override MemoryChunkReference Reference
        {
            get => MemoryContainingChunk == null ? OriginalReference : new MemoryChunkReference(OriginalReference.MemoryChunkID, 0, MemoryContainingChunk.Memory.Length);
            set => base.Reference = value;
        }

        /// <summary>
        /// Creates a reference to an existing file other than the index file. This is called internally by GetAdditionalReferences(Async), after a call to MemoryReferenceInFile.
        /// </summary>
        /// <param name="path">The path, including a number referring to the specific file</param>
        /// <param name="length"></param>
        public BlobMemoryChunk(string path, IBlobManager blobManager, MemoryChunkReference reference)
        {
            BlobPath = path;
            BlobManager = blobManager;
            OriginalReference = reference;
        }

        #region Memory loading and unloading

        public override void LoadMemory()
        {
            Memory<byte> bytes = BlobManager.Read(BlobPath, Reference.Offset, Reference.Length);
            // DEBUG -- what we really need to do here (and in file manager) is cache this in the blob manager. That way, there is just one memory blob for a memory chunk, even if we have references to many pieces of that chunk. Then, it can be unloaded or not. 
            MemoryContainingChunk = new SimpleMemoryOwner<byte>(bytes);
        }

        public async override ValueTask LoadMemoryAsync()
        {
            Memory<byte> bytes = await BlobManager.ReadAsync(BlobPath, Reference.Offset, Reference.Length);
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
