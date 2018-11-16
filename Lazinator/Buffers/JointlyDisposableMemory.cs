﻿using System;
using System.Buffers;
using System.Collections.Generic;

namespace Lazinator.Buffers
{
    public abstract class JointlyDisposableMemory : IMemoryOwner<byte>
    {
        private JointlyDisposableMemory _OriginalSource;
        public JointlyDisposableMemory OriginalSource
        {
            get => _OriginalSource;
            set
            {
                if (value == this)
                    _OriginalSource = null;
                else
                {
                    if (_OriginalSource != null && _OriginalSource != value)
                    {
                        _OriginalSource.DoNotDisposeWithThis(this, false);
                    }
                    _OriginalSource = value;
                }
                while (_OriginalSource?.OriginalSource != null)
                    _OriginalSource = _OriginalSource.OriginalSource;
            }
        }

        public bool Disposed { get; protected internal set; }

        private HashSet<IMemoryOwner<byte>> DisposeTogether = null;

        public abstract Memory<byte> Memory { get; }


        #region Memory management

        /// <summary>
        /// Remembers an additional buffer that should be disposed when this is disposed. 
        /// </summary>
        /// <param name="additionalBuffer">The buffer to dispose with this buffer.</param>
        public void DisposeWithThis(IMemoryOwner<byte> additionalBuffer)
        {
            if (additionalBuffer is LazinatorMemory lazinatorMemory && lazinatorMemory.OwnedMemory != null)
                additionalBuffer = lazinatorMemory.OwnedMemory;
            if (OriginalSource != null)
                OriginalSource.DisposeWithThis(additionalBuffer);
            else
            {
                if (DisposeTogether == null)
                    DisposeTogether = new HashSet<IMemoryOwner<byte>>();
                DisposeTogether.Add(additionalBuffer);
            }
        }

        /// <summary>
        /// Specifies that an old buffer is being replaced with a new buffer. Thus, the old buffer should be disposed now if it is not the original source. The new buffer should be disposed together with the original source.
        /// </summary>
        /// <param name="newBuffer"></param>
        public virtual void ReplaceWithNewBuffer(IMemoryOwner<byte> newBuffer)
        {
            DoNotDisposeWithThis(this, true);
            DisposeWithThis(newBuffer);
        }

        
        public void CopyOriginalSourceToNewBuffer(IMemoryOwner<byte> newBuffer)
        {
            if (newBuffer is JointlyDisposableMemory j)
            {
                j.OriginalSource = this.OriginalSource ?? this;
                if (newBuffer is LazinatorMemory m && m.OwnedMemory is JointlyDisposableMemory m2)
                    m2.OriginalSource = this.OriginalSource ?? this;
            }
        }

        /// <summary>
        /// Specifies that when this is disposed, the buffer should not be disposed with it. 
        /// </summary>
        /// <param name="buffer"></param>
        public void DoNotDisposeWithThis(IMemoryOwner<byte> buffer, bool disposeBufferIfNotOriginalSource)
        {
            if (OriginalSource != null)
                OriginalSource.DoNotDisposeWithThis(buffer, disposeBufferIfNotOriginalSource);
            else
            {
                bool bufferIsThis = buffer == this;
                if (!bufferIsThis)
                {
                    if (DisposeTogether != null)
                        DisposeTogether.Remove(buffer);
                    if (disposeBufferIfNotOriginalSource)
                    {
                        if (buffer is JointlyDisposableMemory j2)
                            j2.OriginalSource = null; // don't dispose original source -- just dispose the buffer itself
                        if (!(buffer is SimpleMemoryOwner<byte>))
                            buffer.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Indicates that memory disposal for this object should be handled independently from any object from which it was created.
        /// This is useful when cloning a Lazinator object, if the clone should survive the disposal of the object
        /// from which it is cloned.
        /// </summary>
        public virtual void DisposeIndependently()
        {
            if (OriginalSource != null)
            {
                IMemoryOwner<byte> buffer;
                if (this is LazinatorMemory lazinatorMemory)
                    buffer = lazinatorMemory.OwnedMemory ?? this;
                else
                    buffer = this;
                OriginalSource.DoNotDisposeWithThis(buffer, false);
                OriginalSource = null;
            }
        }

        /// <summary>
        /// Disposes of the owned memory, thus allowing it to be reused without garbage collection. Memory can be reclaimed
        /// without calling this, but it will be less efficient.
        /// </summary>
        public virtual void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
                if (OriginalSource != null)
                {
                    OriginalSource.Dispose();
                }
                if (DisposeTogether != null)
                    foreach (IMemoryOwner<byte> m in DisposeTogether)
                        if (!(m is SimpleMemoryOwner<byte>))
                            m.Dispose();
            }
        }

        #endregion
    }
}
