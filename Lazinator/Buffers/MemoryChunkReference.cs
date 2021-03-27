using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    /// <summary>
    /// A reference to a range of bytes, identified by a memory chunk number, the index of the first byte within that memory chunk, and the number of bytes.
    /// </summary>
    public partial struct MemoryChunkReference : IMemoryChunkReference
    {
        public MemoryChunkReference(int memoryChunkID, int offset, int length) : this()
        {
            MemoryChunkID = memoryChunkID;
            Offset = offset;
            Length = length;
        }

        public override string ToString()
        {
            return $"MemoryChunkID: {MemoryChunkID}; Offset: {Offset}; Length: {Length}";
        }

        /// <summary>
        /// Extends a memory chunk references list by adding a new reference. If the new reference is contiguous to the last existing reference,
        /// then the list size remains constant. 
        /// </summary>
        /// <param name="memoryChunkReferences"></param>
        /// <param name="newSegment"></param>
        public static void ExtendMemoryChunkReferencesList(List<MemoryChunkReference> memoryChunkReferences, MemoryChunkReference newSegment)
        {
            if (memoryChunkReferences.Any())
            {
                MemoryChunkReference last = memoryChunkReferences.Last();
                if (newSegment.MemoryChunkID == last.MemoryChunkID && newSegment.Offset == last.Offset + last.Length)
                {
                    last.Length += newSegment.Length;
                    return;
                }
            }
            memoryChunkReferences.Add(newSegment);
        }

        /// <summary>
        /// Extends a memory chunk references list by adding new segments. The list is consolidated to avoid having consecutive entries for contiguous ranges.
        /// </summary>
        /// <param name="memoryChunkReferences"></param>
        /// <param name="newSegments"></param>
        public static void ExtendMemoryChunkReferencesList(List<MemoryChunkReference> memoryChunkReferences, IEnumerable<MemoryChunkReference> newSegments)
        {
            foreach (var newSegment in newSegments)
                ExtendMemoryChunkReferencesList(memoryChunkReferences, newSegment);
        }
    }
}
