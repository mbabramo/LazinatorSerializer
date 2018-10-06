using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Buffers
{
    public abstract class JointlyDisposableMemory : IMemoryOwner<byte>
    {
        public string N; // DEBUG
        public static int Round = 0; // DEBUG

        private JointlyDisposableMemory _OriginalSource;
        public JointlyDisposableMemory OriginalSource
        {
            get => _OriginalSource;
            set
            {
                if (value == this)
                    _OriginalSource = null;
                else
                    _OriginalSource = value;
                while (_OriginalSource?.OriginalSource != null)
                    _OriginalSource = _OriginalSource.OriginalSource;
            }
        }

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
                if (buffer != this)
                {
                    if (DisposeTogether != null)
                        DisposeTogether.Remove(buffer);
                    if (disposeBufferIfNotOriginalSource)
                    {
                        if (buffer is JointlyDisposableMemory j)
                            j.OriginalSource = null; // don't dispose original source -- just dispose the buffer itself
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
            }
        }

        /// <summary>
        /// Indicate whether disposing has been initiating to avoid infinite loops.
        /// </summary>
        bool _disposingInitiated = false;

        /// <summary>
        /// Disposes of the owned memory, thus allowing it to be reused without garbage collection. Memory can be reclaimed
        /// without calling this, but it will be less efficient.
        /// </summary>
        public virtual void Dispose()
        {
            if (!_disposingInitiated)
            {
                _disposingInitiated = true;
                if (OriginalSource != null)
                {
                    OriginalSource.Dispose();
                }
                if (DisposeTogether != null)
                    foreach (JointlyDisposableMemory m in DisposeTogether)
                        m.Dispose();
            }
        }

        #endregion
    }
}
