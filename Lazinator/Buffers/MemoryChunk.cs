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
        static int DEBUGCount = 0;
        public int DEBUGCount2 = 0;
        public IMemoryOwner<byte> MemoryContainingChunk { get; set; }

        /// <summary>
        /// A reference to the memory being referred to once the memory is loaded.
        /// </summary>
        public virtual MemoryChunkReference ReferenceOnceLoaded { get; set; }

        /// <summary>
        /// A reference to the memory to be loaded. 
        /// </summary>
        public virtual MemoryChunkReference ReferenceForLoading => ReferenceOnceLoaded;

        public bool IsLoaded => MemoryContainingChunk != null;

        public virtual bool IsPersisted { get; set; }

        public MemoryChunk()
        {

        }

        public MemoryChunk(IMemoryOwner<byte> referencedMemory, MemoryChunkReference reference)
        {
            DEBUGCount2 = DEBUGCount++;
            if (DEBUGCount2 == 6)
            {
                var DEBUGSDF = 23;
            }
            MemoryContainingChunk = referencedMemory;
            ReferenceOnceLoaded = reference;
        }

        public virtual Memory<byte> Memory => MemoryContainingChunk == null ? LazinatorMemory.EmptyMemory : MemoryContainingChunk.Memory.Slice(ReferenceOnceLoaded.Offset, ReferenceOnceLoaded.Length);

        public virtual MemoryChunk SliceReferenceForLoading(int startIndexRelativeToBroaderMemory, int length) => new MemoryChunk(MemoryContainingChunk, new MemoryChunkReference(ReferenceForLoading.MemoryChunkID, startIndexRelativeToBroaderMemory, length));

        /// <summary>
        /// This method should be overridden for a MemoryReference subclass that loads memory lazily. The subclass method
        /// should set ReferencedMemory. The base class always has memory available and thus this method does nothing.
        /// </summary>
        /// <returns></returns>
        public virtual void LoadMemory()
        {
            if (IsLoaded)
                return;
            var task = Task.Run(async () => await LoadMemoryAsync());
            task.Wait();
        }

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
        public virtual void ConsiderUnloadMemory()
        {
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

        public void Dispose()
        {
        }
    }
}
