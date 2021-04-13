using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public class MemoryChunkCollection : IMemoryChunkCollection
    {
        public MemoryChunkCollection()
        {

        }

        public MemoryChunkCollection(List<MemoryChunk> memoryChunks)
        {
            MemoryChunks = memoryChunks;
            MaxMemoryChunkID = MemoryChunks.Any() ? MemoryChunks.Max(x => x.MemoryChunkID) : 0;
        }

        List<MemoryChunk> MemoryChunks = new List<MemoryChunk>();
        public MemoryChunkCollection DeepCopy()
        {
            var collection = new MemoryChunkCollection();
            collection.SetContents(MemoryChunks);
            return collection;
        }
        public MemoryChunkCollection WithAppendedMemoryChunk(MemoryChunk memoryChunk)
        {
            List<MemoryChunk> memoryChunks = MemoryChunks.Select(x => x.WithPreTruncationLengthIncreasedIfNecessary(memoryChunk)).ToList();
            memoryChunks.Add(memoryChunk);
            var collection = new MemoryChunkCollection();
            collection.SetContents(memoryChunks);
            return collection;
        }

        public int MaxMemoryChunkID { get; private set; }

        public IEnumerable<MemoryChunk> EnumerateMemoryChunks()
        {
            if (MemoryChunks is not null)
                foreach (var chunk in MemoryChunks)
                    yield return chunk;
        }

        public MemoryChunk MemoryAtIndex(int i)
        {
            return MemoryChunks[i];
        }

        public void SetContents(IEnumerable<MemoryChunk> chunks)
        {
            MemoryChunks = new List<MemoryChunk>();
            int i = 0;
            if (chunks == null)
            {
                MaxMemoryChunkID = -1;
                return;
            }
            foreach (var chunk in chunks)
            {
                if (i == 0 || chunk.MemoryChunkID > MaxMemoryChunkID)
                    MaxMemoryChunkID = chunk.MemoryChunkID;
                MemoryChunks.Add(chunk);
                i++;
            }

        }

        public IEnumerator<MemoryChunk> GetEnumerator()
        {
            if (MemoryChunks != null)
                foreach (var memoryChunk in MemoryChunks)
                    yield return memoryChunk;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns the first index within the memory chunk collection of the specified memory chunk ID, or null if not found. This will be one less than the index within a LazinatorMemory containing this collection.
        /// </summary>
        /// <param name="memoryChunkID"></param>
        /// <returns></returns>
        public int? GetFirstIndexOfMemoryChunkID(int memoryChunkID)
        {
            int count = Count;
            for (int i = 0; i < count; i++)
            {
                var memoryChunk = MemoryAtIndex(i);
                if (memoryChunk.Reference.MemoryChunkID == memoryChunkID)
                    return i;
            }
            return null;
        }

        public int Count => MemoryChunks?.Count ?? 0;
    }
}
