﻿using System;
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
        /// This method should be overridden for a MemoryReference subclass that loads memory lazily. The subclass method
        /// should set ReferencedMemory. The base class always has memory available and thus this method does nothing.
        /// </summary>
        /// <returns></returns>
        public virtual ValueTask LoadMemoryAsync()
        {
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// This method should be overridden for a MemoryReference subclass that saves memory lazily. This will be called
        /// during the serialization of a splittable object after a page of memory has been completed. A subclass may 
        /// choose not to unload memory, as this base class does.
        /// </summary>
        /// <returns></returns>
        public virtual ValueTask UnloadMemoryAsync()
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