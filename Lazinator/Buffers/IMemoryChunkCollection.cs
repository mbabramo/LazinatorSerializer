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
        void SetContents(IEnumerable<MemoryChunk> chunks);
        public int Count { get; }
        int? GetFirstIndexOfMemoryChunkID(int memoryChunkID); 
        int MaxMemoryChunkID { get; }
    }
}
