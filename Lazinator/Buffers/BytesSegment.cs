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
    public struct BytesSegment
    {
        public int MemoryChunkNumber { get; private set; }
        public int IndexWithinMemoryChunk { get; private set; }
        public int NumBytes { get; private set; }

        public BytesSegment(int memoryChunkNumber, int indexWithinMemoryChunk, int numBytes)
        {
            MemoryChunkNumber = memoryChunkNumber;
            IndexWithinMemoryChunk = indexWithinMemoryChunk;
            NumBytes = numBytes;
        }

        /// <summary>
        /// Extends a byte segment list by adding a new segment. If the new segment is contiguous to the last existing segment,
        /// then the list size remains constant. 
        /// </summary>
        /// <param name="bytesSegmentList"></param>
        /// <param name="newSegment"></param>
        public static void ExtendBytesSegmentList(List<BytesSegment> bytesSegmentList, BytesSegment newSegment)
        {
            if (bytesSegmentList.Any())
            {
                BytesSegment last = bytesSegmentList.Last();
                if (newSegment.MemoryChunkNumber == last.MemoryChunkNumber && newSegment.IndexWithinMemoryChunk == last.IndexWithinMemoryChunk + last.NumBytes)
                {
                    last.NumBytes += newSegment.NumBytes;
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
        public static void ExtendBytesSegmentList(List<BytesSegment> bytesSegmentList, IEnumerable<BytesSegment> newSegments)
        {
            foreach (var newSegment in bytesSegmentList)
                ExtendBytesSegmentList(bytesSegmentList, newSegment);
        }
    }
}
