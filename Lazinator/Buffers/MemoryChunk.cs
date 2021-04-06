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
        public IMemoryOwner<byte> MemoryAsLoaded { get; set; }

        public MemoryChunkReference Reference { get; set; }

        public int MemoryChunkID => Reference.MemoryChunkID;
        public int Length => Reference.FinalLength;

        public bool IsLoaded => MemoryAsLoaded != null;

        public virtual bool IsPersisted { get; set; }

        public MemoryChunk()
        {

        }

        public MemoryChunk(IMemoryOwner<byte> memoryAsLoaded) : this(memoryAsLoaded, new MemoryChunkReference(0, 0, memoryAsLoaded.Memory.Length, 0, memoryAsLoaded.Memory.Length), false)
        {

        }

        public MemoryChunk(IMemoryOwner<byte> memoryAsLoaded, MemoryChunkReference reference, bool isPersisted)
        {
            MemoryAsLoaded = memoryAsLoaded;
            Reference = reference;
            IsPersisted = isPersisted;
        }

        /// <summary>
        /// Returns the memory being referred to, taking into account the additional offset to be applied after loading.
        /// If the memory hasn't been loaded, empty memory will be returned.
        /// </summary>
        public virtual Memory<byte> Memory => MemoryAsLoaded == null ? LazinatorMemory.EmptyMemory : MemoryAsLoaded.Memory.Slice(Reference.AdditionalOffset, Reference.FinalLength);

        /// <summary>
        /// Slices the memory being referred to. The information for loading remains the same.
        /// </summary>
        /// <param name="offset">An additional offset to be applied, in addition to the existing additional offset</param>
        /// <param name="length">The final length of the slice</param>
        /// <returns></returns>
        public virtual MemoryChunk Slice(int offset, int length)
        {
            var chunk = new MemoryChunk(MemoryAsLoaded, Reference.Slice(offset, length), IsPersisted);
            return chunk;
        }

        /// <summary>
        /// Returns a MemoryChunk with the specified reference.
        /// </summary>
        /// <param name="replacementReference"></param>
        /// <returns></returns>
        public virtual MemoryChunk WithReference(MemoryChunkReference replacementReference) => new MemoryChunk(MemoryAsLoaded, replacementReference, IsPersisted);

        internal MemoryChunk WithPreTruncationLengthIncreasedIfNecessary(MemoryChunk otherMemoryChunk)
        {
            if ((otherMemoryChunk.MemoryChunkID == MemoryChunkID) && Reference.PreTruncationLength < otherMemoryChunk.Reference.PreTruncationLength)
            {
                Reference = Reference.WithPreTruncationLength(otherMemoryChunk.Reference.PreTruncationLength);
                MemoryAsLoaded = otherMemoryChunk.MemoryAsLoaded;
            }
            return this;
        }

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
            MemoryAsLoaded?.Dispose();
        }

        public override string ToString()
        {
            LoadMemory();
            var bytes = Memory.ToArray();
            string result = String.Join(",", bytes.Select(x => x.ToString().PadLeft(3, '0')));
            return $"Chunk {MemoryChunkID.ToString().PadLeft(3, '0')}: {result}";
        }
    }
}
