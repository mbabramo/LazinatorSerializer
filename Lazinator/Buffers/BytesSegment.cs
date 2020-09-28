using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public struct BytesSegment
    {
        public int MemoryChunkVersion { get; private set; }
        public int IndexWithinMemoryChunk { get; private set; }
        public int NumBytes { get; private set; }

        public BytesSegment(int memoryChunkVersion, int indexWithinMemoryChunk, int numBytes)
        {
            MemoryChunkVersion = memoryChunkVersion;
            IndexWithinMemoryChunk = indexWithinMemoryChunk;
            NumBytes = numBytes;
        }

        public static void ExtendBytesSegmentList(List<BytesSegment> bytesSegmentList, BytesSegment newSegment)
        {
            if (bytesSegmentList.Any())
            {
                BytesSegment last = bytesSegmentList.Last();
                if (newSegment.MemoryChunkVersion == last.MemoryChunkVersion && newSegment.IndexWithinMemoryChunk == last.IndexWithinMemoryChunk + last.NumBytes)
                {
                    last.NumBytes += newSegment.NumBytes;
                    return;
                }
            }
            bytesSegmentList.Add(newSegment);
        }

        public static void ExtendBytesSegmentList(List<BytesSegment> bytesSegmentList, List<BytesSegment> newSegments)
        {
            foreach (var newSegment in bytesSegmentList)
                ExtendBytesSegmentList(bytesSegmentList, newSegment);
        }
    }
}
