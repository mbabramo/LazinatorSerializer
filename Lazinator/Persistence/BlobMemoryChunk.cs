using System;
using System.Buffers;
using System.Threading.Tasks;
using Lazinator.Buffers;

namespace Lazinator.Persistence
{
    // DEBUG -- are we using this?
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
        public BlobMemoryChunk(string path, IBlobManager blobManager, MemoryBlockLoadingInfo loadingInfo) : base(null, loadingInfo, true)
        {
            BlobPath = path;
            BlobManager = blobManager;
        }

        #region Memory loading and unloading

        public override void LoadMemory()
        {
            if (IsLoaded)
                return;
            
            ReadOnlyMemory<byte> bytes = BlobManager.Read(BlobPath, LoadingOffset, LoadingInfo.PreTruncationLength);
            MemoryAsLoaded = new ReadOnlyBytes(bytes);
        }

        public async override ValueTask LoadMemoryAsync()
        {
            if (IsLoaded)
                return;
            ReadOnlyMemory<byte> bytes = await BlobManager.ReadAsync(BlobPath, LoadingOffset, LoadingInfo.PreTruncationLength);
            MemoryAsLoaded = new ReadOnlyBytes(bytes);
        }

        private long LoadingOffset
        {
            get
            {
                long loadingOffset = 0;
                if (LoadingInfo is MemoryBlockInsetLoadingInfo insetLoadingInfo)
                    loadingOffset = insetLoadingInfo.LoadingOffset;
                return loadingOffset;
            }
        }

        public override void ConsiderUnloadMemory()
        {
            MemoryAsLoaded = null; // Reference will now point to OriginalReference
        }

        public override ValueTask ConsiderUnloadMemoryAsync()
        {
            MemoryAsLoaded = null; // Reference will now point to OriginalReference
            return ValueTask.CompletedTask;
        }

        #endregion
    }
}
