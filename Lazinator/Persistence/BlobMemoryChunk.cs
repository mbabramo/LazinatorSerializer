using System;
using System.Buffers;
using System.Threading.Tasks;
using Lazinator.Buffers;

namespace Lazinator.Persistence
{
    /// <summary>
    /// Saves or loads memory references referring either to parts of a single file or to consecutively numbered files. The initial file contains information on all the other components, so that these can be asynchronously loaded if necessary.
    /// </summary>
    public class BlobMemoryChunk : MemoryChunk
    {
        string BlobPath;
        IBlobManager BlobManager;

        /// <summary>
        /// Creates a reference to an existing blob. This is called internally by GetAdditionalReferences(Async), after a call to MemoryReferenceInFile.
        /// </summary>
        public BlobMemoryChunk(string path, IBlobManager blobManager, MemoryChunkReference reference) : base(null, reference, true)
        {
            BlobPath = path;
            BlobManager = blobManager;
        }

        #region Memory loading and unloading

        public override void LoadMemory()
        {
            if (IsLoaded)
                return;
            ReadOnlyMemory<byte> bytes = BlobManager.Read(BlobPath, Reference.OffsetForLoading, Reference.PreTruncationLength);
            ReadOnlyLoadedMemory = new ReadOnlyBytes(bytes);
        }

        public async override ValueTask LoadMemoryAsync()
        {
            if (IsLoaded)
                return;
            ReadOnlyMemory<byte> bytes = await BlobManager.ReadAsync(BlobPath, Reference.OffsetForLoading, Reference.PreTruncationLength);
            ReadOnlyLoadedMemory = new ReadOnlyBytes(bytes);
        }

        public override void ConsiderUnloadMemory()
        {
            ReadOnlyLoadedMemory = null; // Reference will now point to OriginalReference
        }

        public override ValueTask ConsiderUnloadMemoryAsync()
        {
            ReadOnlyLoadedMemory = null; // Reference will now point to OriginalReference
            return ValueTask.CompletedTask;
        }

        #endregion
    }
}
