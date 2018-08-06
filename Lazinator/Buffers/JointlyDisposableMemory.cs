using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Buffers
{
    public abstract class JointlyDisposableMemory : IMemoryOwner<byte>
    {

        private JointlyDisposableMemory _OriginalSource;
        public JointlyDisposableMemory OriginalSource
        {
            get => _OriginalSource ?? this;
            set { _OriginalSource = value; }
        }

        private HashSet<IMemoryOwner<byte>> DisposeTogether = null;

        public abstract Memory<byte> Memory { get; }


        #region Memory management

        /// <summary>
        /// Remembers an additional buffer that should be disposed when this is disposed. 
        /// </summary>
        /// <param name="additionalBuffer">The buffer to dispose with this buffer.</param>
        public void DisposeWhenOriginalSourceDisposed(IMemoryOwner<byte> additionalBuffer)
        {
            if (OriginalSource != this)
                OriginalSource.DisposeWhenOriginalSourceDisposed(additionalBuffer);
            else
            {
                if (DisposeTogether == null)
                    DisposeTogether = new HashSet<IMemoryOwner<byte>>();
                DisposeTogether.Add(additionalBuffer);
            }
        }

        /// <summary>
        /// Disposes of the owned memory, thus allowing it to be reused without garbage collection. Memory can be reclaimed
        /// without calling this, but it will be less efficient.
        /// </summary>
        public virtual void Dispose()
        {
            if (OriginalSource != this)
                OriginalSource.Dispose();
            if (DisposeTogether != null)
                foreach (JointlyDisposableMemory m in DisposeTogether)
                    m.Dispose();
        }

        #endregion
    }
}
