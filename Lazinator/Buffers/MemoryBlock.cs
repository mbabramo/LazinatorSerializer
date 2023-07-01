﻿using Lazinator.Exceptions;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    // DEBUG -- maybe MemoryBlock should be LoadableMemoryBlock or just MemoryBlock

    public class MemoryBlock
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

        public MemoryBlockID MemoryBlockID => LoadingInfo.MemoryBlockID;

        private int _Length = 0;
        public int Length { get => _Length; set => _Length = value; }
        public int MemoryBlockLength => LoadingInfo.MemoryBlockLength;

        public bool IsLoaded => MemoryAsLoaded != null; // DEBUG -- when loading later

        public virtual bool IsPersisted { get; set; }

        public long AllocationID => MemoryAsLoaded switch
        {
            ReadWriteBytes rw => rw.AllocationID,
            ReadOnlyBytes ro => ro.AllocationID,
            _ => throw new NotImplementedException()
        };

        public MemoryBlock()
        {

        }

        public MemoryBlock(IReadableBytes memoryAsLoaded) : this(memoryAsLoaded, null, false)
        {

        }

        public MemoryBlock(ReadWriteBytes memoryAsLoaded) : this(memoryAsLoaded, null,  false)
        {

        }

        public MemoryBlock(IReadableBytes memoryAsLoaded, MemoryBlockLoadingInfo loadingInfo, bool isPersisted)
        {
            MemoryAsLoaded = memoryAsLoaded;
            Length = memoryAsLoaded.ReadOnlyMemory.Length;
            LoadingInfo = loadingInfo ?? new MemoryBlockLoadingInfo(new MemoryBlockID(0), Length);
            IsPersisted = isPersisted;
        }

        public virtual MemoryBlock DeepCopy() => new MemoryBlock(MemoryAsLoaded, LoadingInfo, IsPersisted);

        /// <summary>
        /// Returns the memory being referred to, taking into account the additional offset to be applied after loading.
        /// If the memory hasn't been loaded, empty memory will be returned.
        /// </summary>
        public virtual ReadOnlyMemory<byte> ReadOnlyMemory => MemoryAsLoaded == null ? LazinatorMemory.EmptyReadOnlyMemory : MemoryAsLoaded.ReadOnlyMemory.Slice(0, Length);

        public IEnumerable<byte> EnumerateBytes()
        {
            int length = Length;
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
        /// This method should be overridden for a MemoryBlock subclass that loads memory lazily. The subclass method
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
        /// This method should be overridden for a MemoryBlock subclass that loads memory lazily. The subclass method
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
            string result = ContentToString();
            return $"Block {MemoryBlockID.ToString().PadLeft(3, '0')}:\n{result}";
        }

        public string ContentToString()
        {
            LoadMemory();
            var bytes = ReadOnlyMemory.ToArray();
            string result = String.Join(",", bytes.Select(x => x.ToString().PadLeft(3, '0')));
            return result;
        }
    }
}