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
        bool ContainedInSingleBlob;

        MemoryChunkReference OriginalReference;
        public override MemoryChunkReference Reference
        {
            get => ReferencedMemory == null ? OriginalReference : new MemoryChunkReference(OriginalReference.MemoryChunkID, 0, ReferencedMemory.Memory.Length);
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
    }
}
