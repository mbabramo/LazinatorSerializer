using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Core
{
    /// <summary>
    /// Options to determine the relationship between the buffers of the original object and the cloned object
    /// </summary>
    public enum CloneBufferOptions
    {
        /// <summary>
        /// A new buffer is created for the clone, and the original's buffer remains the same. However, the buffers are linked, so that if either is disposed and thus returned to the memory pool, the other is as well. Note that disposing need not be done manually, as garbage collection will also eventually return memory to the pool, but manual disposing improves performance.
        /// </summary>
        LinkedBuffer,
        /// <summary>
        /// A new buffer is created for the clone, and the original is updated to use the same buffer if all children are being cloned. The two objects are still independent, so if one is mutated and then serialized, a new buffer will then be created. Disposing the memory buffer makes both objects unusable. Note that disposing need not be done manually, as garbage collection will also eventually return memory to the pool, but manual disposing improves performance.
        /// </summary>
        SharedBuffer,
        /// <summary>
        /// A new buffer is created for the clone, and the original's buffer remains the same. Disposing one buffer will not affect the other. Note that disposing need not be done manually, as garbage collection will eventually return memory to the pool, but manual disposing improves performance.
        /// </summary>
        IndependentBuffers,
    }
}
