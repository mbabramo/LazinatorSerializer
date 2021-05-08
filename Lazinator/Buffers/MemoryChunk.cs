using Lazinator.Exceptions;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public class MemoryChunk
    {

        public IReadableBytes MemoryAsLoaded { get; set; }

        public IMemoryOwner<byte> MemoryOwner => MemoryAsLoaded?.MemoryOwner;
        public bool Disposed => (MemoryOwner as IMemoryAllocationInfo)?.Disposed ?? false;

        public bool IsReadOnly => MemoryAsLoaded is not ReadWriteBytes;

        public ReadWriteBytes WritableMemory
        {
            get
            {
                if (IsReadOnly)
                    ThrowHelper.ThrowMemoryNotWritableException();
                return (ReadWriteBytes)MemoryAsLoaded;
            }
        }

        public MemoryBlockLoadingInfo LoadingInfo { get; set; }

        public int MemoryBlockID => LoadingInfo.MemoryBlockID;
        public int Length => LoadingInfo.PreTruncationLength;

        public bool IsLoaded => MemoryAsLoaded != null; // DEBUG -- when loading later

        public virtual bool IsPersisted { get; set; }

        public long AllocationID => MemoryAsLoaded switch
        {
            ReadWriteBytes rw => rw.AllocationID,
            ReadOnlyBytes ro => ro.AllocationID,
            _ => throw new NotImplementedException()
        };

        public MemoryChunk()
        {

        }

        public MemoryChunk(IReadableBytes memoryAsLoaded) : this(memoryAsLoaded, null, false)
        {

        }

        public MemoryChunk(ReadWriteBytes memoryAsLoaded) : this(memoryAsLoaded, null,  false)
        {

        }

        public MemoryChunk(IReadableBytes memoryAsLoaded, MemoryBlockLoadingInfo loadingInfo, bool isPersisted)
        {
            MemoryAsLoaded = memoryAsLoaded;
            LoadingInfo = loadingInfo ?? new MemoryBlockLoadingInfo(0, memoryAsLoaded.ReadOnlyMemory.Length);
            IsPersisted = isPersisted;
        }

        public virtual MemoryChunk DeepCopy() => new MemoryChunk(MemoryAsLoaded, LoadingInfo, IsPersisted);

        /// <summary>
        /// Returns the memory being referred to, taking into account the additional offset to be applied after loading.
        /// If the memory hasn't been loaded, empty memory will be returned.
        /// </summary>
        public virtual ReadOnlyMemory<byte> ReadOnlyMemory => MemoryAsLoaded == null ? LazinatorMemory.EmptyReadOnlyMemory : MemoryAsLoaded.ReadOnlyMemory.Slice(0, Length);

        public IEnumerable<byte> EnumerateBytes()
        {
            int length = ReadOnlyMemory.Length;
            for (int i = 0; i < length; i++)
                yield return ReadOnlyMemory.Span[i];
        }

        public virtual Memory<byte> ReadWriteMemory
        {
            get
            {
                if (IsReadOnly)
                {
                    ThrowHelper.ThrowMemoryNotWritableException();
                    return null; // will not execute
                }
                else
                    return WritableMemory.Memory.Slice(0, Length);
            }
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
            var bytes = ReadOnlyMemory.ToArray();
            string result = String.Join(",", bytes.Select(x => x.ToString().PadLeft(3, '0')));
            return $"Chunk {MemoryBlockID.ToString().PadLeft(3, '0')}: {result}";
        }
    }
}
