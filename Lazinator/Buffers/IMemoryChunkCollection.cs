using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public interface IMemoryChunkCollection : IEnumerable<MemoryChunk>
    {
        MemoryChunk MemoryAtIndex(int i);
        MemoryChunkCollection DeepCopy();
        void AppendMemoryChunk(MemoryChunk memoryChunk);
        MemoryChunkCollection WithAppendedMemoryChunk(MemoryChunk memoryChunk);
        void SetContents(IEnumerable<MemoryChunk> chunks);
        Dictionary<int, MemoryChunk> GetMemoryChunksByID();
        int GetNextMemoryBlockID();
        public int NumMemoryChunks { get; }
        int MaxMemoryBlockID { get; }
        public long Length { get; }
    }
}
