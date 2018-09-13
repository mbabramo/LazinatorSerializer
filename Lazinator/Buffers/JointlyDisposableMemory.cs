using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Buffers
{
    public abstract class JointlyDisposableMemory : IMemoryOwner<byte>
    {

        static int DEBUG_Count = 0;
        public int DEBUG_ID;

        public JointlyDisposableMemory()
        {
            DEBUG_ID = DEBUG_Count;
            System.Diagnostics.Debug.WriteLine($"JointlyDisposableMemory {DEBUG_Count++}");
        }

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
        /// Indicate whether this class has triggered disposal at the original source. In that case, we avoid an infinite loop by not doing so again.
        /// </summary>
        bool _disposingOriginalSource = false;

        /// <summary>
        /// Disposes of the owned memory, thus allowing it to be reused without garbage collection. Memory can be reclaimed
        /// without calling this, but it will be less efficient.
        /// </summary>
        public virtual void Dispose()
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG Disposing {DEBUG_ID}");
            if (!_disposingOriginalSource)
            {
                if (OriginalSource != this)
                {
                    _disposingOriginalSource = true;
                    OriginalSource.Dispose();
                }
                if (DisposeTogether != null)
                    foreach (JointlyDisposableMemory m in DisposeTogether)
                        m.Dispose();
                _disposingOriginalSource = false;
            }
        }

        #endregion
    }
}
