﻿namespace Lazinator.Core
{
    /// <summary>
    /// Options to determine the relationship between the buffers of the original object and the cloned object
    /// </summary>
    public enum CloneBufferOptions
    {
        /// <summary>
        /// A new buffer is created for the clone, and the original's buffer remains the same. Disposing one buffer will not affect the other. Note that disposing need not be done manually, as garbage collection will eventually return memory to the pool, but manual disposing improves performance.
        /// </summary>
        IndependentBuffers,
        /// <summary>
        /// Clone without using buffers. That is, the cloned objection will not have a buffer until and if it is serialized. This requires a deep clone of all Lazinator objects and thus may be slower than other methods, but it is useful when a large number of small Lazinator objects must be cloned.
        /// </summary>
        NoBuffer
    }
}
