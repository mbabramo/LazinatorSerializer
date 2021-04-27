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

        List<MemoryChunk> MemoryChunks = new List<MemoryChunk>();
        public long Length { get; private set; }

        public MemoryChunkCollection()
        {

        }

        public MemoryChunkCollection(MemoryChunk memoryChunk)
        {
            MemoryChunks = new List<MemoryChunk>() { memoryChunk };
            MaxMemoryChunkID = memoryChunk.MemoryChunkID;
            Length = memoryChunk.Length;
        }

        public MemoryChunkCollection(List<MemoryChunk> memoryChunks)
        {
            MemoryChunks = memoryChunks;
            MaxMemoryChunkID = MemoryChunks.Any() ? MemoryChunks.Max(x => x.MemoryChunkID) : 0;
            Length = MemoryChunks.Sum(x => (long) x.Length);
        }

        public MemoryChunkCollection(LazinatorMemory lazinatorMemory) : this(lazinatorMemory.EnumerateMemoryChunks().ToList())
        {
        }

        public LazinatorMemory ToLazinatorMemory()
        {
            return new LazinatorMemory(this);
        }

        public MemoryChunkCollection DeepCopy()
        {
            var collection = new MemoryChunkCollection();
            collection.SetContents(MemoryChunks);
            return collection;
        }

        public void AppendMemoryChunk(MemoryChunk memoryChunk)
        {
            MemoryChunks.Add(memoryChunk);
            if (memoryChunk.MemoryChunkID > MaxMemoryChunkID)
                MaxMemoryChunkID = memoryChunk.MemoryChunkID;
            Length += (long)memoryChunk.Length;
        }

        public MemoryChunkCollection WithAppendedMemoryChunk(MemoryChunk memoryChunk)
        {
            List<MemoryChunk> memoryChunks = MemoryChunks.Select(x => x.WithPreTruncationLengthIncreasedIfNecessary(memoryChunk)).ToList();
            var collection = new MemoryChunkCollection(memoryChunks);
            collection.AppendMemoryChunk(memoryChunk);
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
            Length = 0;
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
                Length += (long)chunk.Length;
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

        public Dictionary<int, MemoryChunk> GetMemoryChunksByID()
        {
            Dictionary<int, MemoryChunk> d = new Dictionary<int, MemoryChunk>();
            foreach (MemoryChunk memoryChunk in MemoryChunks)
            {
                int chunkID = memoryChunk.Reference.MemoryChunkID;
                if (!d.ContainsKey(chunkID))
                    d[chunkID] = memoryChunk;
            }
            return d;
        }

        public int GetNextMemoryChunkID() => MaxMemoryChunkID + 1;

        public int NumMemoryChunks => MemoryChunks?.Count ?? 0;
    }
}
