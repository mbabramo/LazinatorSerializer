using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public class MultipleBufferInfo
    {


        /// <summary>
        /// Bytes that were previously written. They may have been written in the same serialization pass (created when ExpandableBytes became full) or 
        /// in a previous serialization pass (when serializing diffs).
        /// </summary>
        public LazinatorMemory CompletedMemory { get; set; }

        /// <summary>
        /// When serializing diffs, these are non-null and will refer to various segments in CompletedMemory and ActiveMemory in order.
        /// </summary>
        internal List<MemoryChunkReference> RecycledMemoryChunkReferences;

        /// <summary>
        /// When serializing diffs, when a section of ActiveMemory is added to RecycledMemoryChunkReferences, this will equal the index
        /// of the last byte added plus 1. 
        /// </summary>
        internal int NumActiveMemoryBytesAddedToRecycling;
    }
}
