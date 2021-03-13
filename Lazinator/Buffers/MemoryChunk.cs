using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public class MemoryChunk : IMemoryOwner<byte>
    {
        public IMemoryOwner<byte> ReferencedMemory { get; set; }

        public MemoryChunkReference Reference { get; set; }

        public bool IsLoaded => ReferencedMemory != null;

        public MemoryChunk()
        {

        }

        public MemoryChunk(IMemoryOwner<byte> referencedMemory, MemoryChunkReference reference)
        {
            ReferencedMemory = referencedMemory;
            Reference = reference;
        }

        public Memory<byte> Memory => ReferencedMemory.Memory.Slice(Reference.IndexWithinMemoryChunk, Reference.Length);

        public MemoryChunk Slice(int startIndex, int length) => new MemoryChunk(ReferencedMemory, new MemoryChunkReference(Reference.MemoryChunkID, Reference.IndexWithinMemoryChunk + startIndex, length));

        /// <summary>
        /// This method should be overridden for a MemoryReference subclass that loads memory lazily. The subclass method
        /// should set ReferencedMemory. The base class always has memory available and thus this method does nothing.
        /// </summary>
        /// <returns></returns>
        public virtual ValueTask LoadMemoryAsync()
        {
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// This method may be overridden for a MemoryReference subclass that saves memory lazily. This will be called
        /// after memory that is loaded is read and may no longer be necessary.  A subclass may, like this base class, 
        /// choose not to unload memory.
        /// </summary>
        public virtual ValueTask ConsiderUnloadMemoryAsync()
        {
            return ValueTask.CompletedTask;
        }

        public virtual async ValueTask<Memory<byte>> GetMemoryAsync()
        {
            if (!IsLoaded)
                await LoadMemoryAsync();

            return Memory;
        }

        public void Dispose()
        {
        }
    }
}
