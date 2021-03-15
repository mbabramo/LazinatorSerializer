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
        /// Extends a byte segment list by adding a new segment. If the new segment is contiguous to the last existing segment,
        /// then the list size remains constant. 
        /// </summary>
        /// <param name="bytesSegmentList"></param>
        /// <param name="newSegment"></param>
        public static void ExtendBytesSegmentList(List<MemoryChunkReference> bytesSegmentList, MemoryChunkReference newSegment)
        {
            if (bytesSegmentList.Any())
            {
                MemoryChunkReference last = bytesSegmentList.Last();
                if (newSegment.MemoryChunkID == last.MemoryChunkID && newSegment.Offset == last.Offset + last.Length)
                {
                    last.Length += newSegment.Length;
                    return;
                }
            }
            bytesSegmentList.Add(newSegment);
        }

        /// <summary>
        /// Extends a byte segments list by adding new segments. The list is consolidated to avoid having consecutive entries for contiguous ranges.
        /// </summary>
        /// <param name="bytesSegmentList"></param>
        /// <param name="newSegments"></param>
        public static void ExtendBytesSegmentList(List<MemoryChunkReference> bytesSegmentList, IEnumerable<MemoryChunkReference> newSegments)
        {
            foreach (var newSegment in bytesSegmentList)
                ExtendBytesSegmentList(bytesSegmentList, newSegment);
        }
    }
}
