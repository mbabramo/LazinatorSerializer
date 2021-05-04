using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class MemoryChunkCollection : IMemoryChunkCollection, IEnumerable<MemoryChunk>
    {

        protected List<MemoryChunk> MemoryChunks = new List<MemoryChunk>();
        protected Dictionary<int, MemoryChunk> MemoryChunksByID = null;
        public long Length { get; private set; }

        public MemoryChunkCollection()
        {

        }

        public MemoryChunkCollection(List<MemoryChunk> memoryChunks)
        {
            MemoryChunks = memoryChunks;
            MaxMemoryBlockID = MemoryChunks.Any() ? MemoryChunks.Max(x => x.MemoryBlockID) : 0;
            Length = MemoryChunks.Sum(x => (long) x.Length);
        }

        public virtual MemoryChunkCollection WithAppendedMemoryChunk(MemoryChunk memoryChunk)
        {
            List<MemoryChunk> memoryChunks = MemoryChunks.Select(x => x.WithPreTruncationLengthIncreasedIfNecessary(memoryChunk)).ToList();
            var collection = new MemoryChunkCollection(memoryChunks);
            collection.MemoryChunksByID = null;
            collection.AppendMemoryChunk(memoryChunk);
            return collection;
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
            if (MemoryChunksByID != null)
                MemoryChunksByID[memoryChunk.MemoryBlockID] = memoryChunk;
            MemoryChunks.Add(memoryChunk);
            if (memoryChunk.MemoryBlockID > MaxMemoryBlockID)
                MaxMemoryBlockID = memoryChunk.MemoryBlockID;
            Length += (long)memoryChunk.Length;
        }

        public int MaxMemoryBlockID { get; private set; }

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
                MaxMemoryBlockID = -1;
                return;
            }
            foreach (var chunk in chunks)
            {
                if (i == 0 || chunk.MemoryBlockID > MaxMemoryBlockID)
                    MaxMemoryBlockID = chunk.MemoryBlockID;
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

        public MemoryChunk GetMemoryChunkByMemoryBlockID(int memoryBlockID)
        {
            var d = GetMemoryChunksByMemoryBlockID();
            if (!d.ContainsKey(memoryBlockID))
                return null;
            return d[memoryBlockID];
        }

        public Dictionary<int, MemoryChunk> GetMemoryChunksByMemoryBlockID()
        {
            if (MemoryChunksByID == null)
                LoadMemoryChunksByMemoryBlockID();
            return MemoryChunksByID;
        }

        private void LoadMemoryChunksByMemoryBlockID()
        {
            Dictionary<int, MemoryChunk> d = new Dictionary<int, MemoryChunk>();
            foreach (MemoryChunk memoryChunk in MemoryChunks)
            {
                int chunkID = memoryChunk.MemoryBlockID;
                if (!d.ContainsKey(chunkID))
                    d[chunkID] = memoryChunk;
            }
            MemoryChunksByID = d;
        }

        public int GetNextMemoryBlockID() => MaxMemoryBlockID + 1;

        public int NumMemoryChunks => MemoryChunks?.Count ?? 0;
    }
}
