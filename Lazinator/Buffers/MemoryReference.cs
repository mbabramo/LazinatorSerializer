using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public class MemoryReference : IMemoryOwner<byte>
    {
        public IMemoryOwner<byte> ReferencedMemory { get; set; }

        public int VersionOfReferencedMemory { get; set; }

        public int StartIndex;

        public int Length;

        public bool IsLoaded => ReferencedMemory != null;

        public MemoryReference(IMemoryOwner<byte> referencedMemory, int versionOfReferencedMemory, int startIndex, int length)
        {
            ReferencedMemory = referencedMemory;
            VersionOfReferencedMemory = versionOfReferencedMemory;
            StartIndex = startIndex;
            Length = length;
        }

        public Memory<byte> Memory => ReferencedMemory.Memory.Slice(StartIndex, Length);

        /// <summary>
        /// This method should be overriden for a MemoryReference subclass that loads memory lazily. The subclass method
        /// should set ReferencedMemory.
        /// </summary>
        /// <returns></returns>
        public virtual ValueTask LoadMemoryAsync()
        {
            return ValueTask.CompletedTask;
        }

        public virtual async ValueTask<Memory<byte>> GetMemoryAsync()
        {
            if (!IsLoaded)
                await LoadMemoryAsync();

            return Memory;
        }

        public MemoryReference Slice(int startIndex, int length) => new MemoryReference(ReferencedMemory, VersionOfReferencedMemory, StartIndex + startIndex, length);

        public void Dispose()
        {
        }
    }
}
