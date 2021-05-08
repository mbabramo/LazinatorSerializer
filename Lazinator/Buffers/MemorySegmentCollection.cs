using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class MemorySegmentCollection : MemoryChunkCollection, IMemorySegmentCollection
    {
        public MemorySegmentCollection(LazinatorMemory lazinatorMemory, bool recycle) : this(lazinatorMemory.EnumerateMemoryChunks().ToList(), recycle)
        {
        }

        public MemorySegmentCollection(MemoryChunk chunk, bool recycle) : this(new List<MemoryChunk> { chunk }, recycle)
        {
        }

        public MemorySegmentCollection(List<MemoryChunk> memoryChunks, bool recycle) : base(memoryChunks)
        {
            if (recycle)
                Segments = new List<MemoryBlockIDAndSlice>();
        }


        public override MemorySegmentCollection WithAppendedMemoryChunk(MemoryChunk memoryChunk)
        {
            List<MemoryChunk> memoryChunks = MemoryChunks.Select(x => x.WithPreTruncationLengthIncreasedIfNecessary(memoryChunk)).ToList();
            var collection = new MemorySegmentCollection(memoryChunks, Recycling);
            collection.AppendMemoryChunk(memoryChunk);
            return collection;
        }

        public void SetFromLazinatorMemory(LazinatorMemory lazinatorMemory)
        {
            SetChunks(lazinatorMemory.EnumerateMemoryChunks());
            SetSegments(lazinatorMemory.EnumerateMemoryBlockIDsAndSlices().ToList());
        }

        private void SetSegments(List<MemoryBlockIDAndSlice> segments)
        {
            Segments = segments.ToList();
        }

        public bool Recycling => Segments != null;

        internal long RecycledTotalLength { get; set; }

        /// <summary>
        /// When serializing diffs, when a section of ActiveMemory is added to RecycledMemoryChunkReferences, this will equal the index
        /// of the last byte added plus 1. 
        /// </summary>
        internal int NumActiveMemoryBytesAddedToRecycling;

        /// <summary>
        /// Extends a memory chunk references list by adding a new reference. If the new reference is contiguous to the last existing reference,
        /// then the list size remains constant. 
        /// </summary>
        /// <param name="memoryChunkReferences"></param>
        /// <param name="newSegment"></param>
        public void ExtendMemoryChunkReferencesList(MemoryBlockIDAndSlice blockAndSlice, bool extendEarlierReferencesForSameChunk)
        {
            if (Segments.Any())
            {
                if (extendEarlierReferencesForSameChunk)
                {
                    for (int i = 0; i < Segments.Count; i++)
                    {
                        var segment = Segments[i];
                        if (segment.MemoryBlockID == blockAndSlice.MemoryBlockID) 
                        {
                            MemoryChunk existingChunk = GetMemoryChunkByMemoryBlockID(segment.MemoryBlockID);
                            if (existingChunk != null)
                            {
                                existingChunk.LoadingInfo.PreTruncationLength = blockAndSlice.Length;
                            }
                        }
                    }
                }
                var last = Segments.Last();
                if (last.MemoryBlockID == blockAndSlice.MemoryBlockID && blockAndSlice.Offset == last.Offset + last.Length)
                {
                    last = new MemoryBlockIDAndSlice(last.MemoryBlockID, last.Offset, last.Length + blockAndSlice.Length);
                    Segments[Segments.Count - 1] = last;
                    RecycledTotalLength += blockAndSlice.Length;
                    return;
                }
            }
            Segments.Add(new MemoryBlockIDAndSlice(blockAndSlice.MemoryBlockID, blockAndSlice.Offset, blockAndSlice.Length));
            RecycledTotalLength += blockAndSlice.Length;
        }

        /// <summary>
        /// Extends a memory chunk references list by adding new segments. The list is consolidated to avoid having consecutive entries for contiguous ranges.
        /// </summary>
        /// <param name="memoryChunkReferences"></param>
        /// <param name="newSegments"></param>
        public void ExtendMemoryChunkReferencesList(IEnumerable<MemoryBlockIDAndSlice> newSegments)
        {
            foreach (var newSegment in newSegments)
                ExtendMemoryChunkReferencesList(newSegment, false);
        }


        /// <summary>
        /// Writes from CompletedMemory. Instead of copying the bytes, it simply adds a BytesSegment reference to where those bytes are in the overall sets of bytes.
        /// </summary>
        /// <param name="memoryChunkIndex">The index of the memory chunk</param>
        /// <param name="startPosition">The position of the first byte of the memory within the indexed memory chunk</param>
        /// <param name="numBytes"></param>
        internal void InsertReferenceToCompletedMemory(int memoryChunkIndex, int startPosition, long numBytes, int activeMemoryPosition)
        {
            RecordLastActiveMemoryChunkReference(activeMemoryPosition);
            IEnumerable<MemoryBlockIDAndSlice> segmentsToAdd = EnumerateMemoryChunkReferences(memoryChunkIndex, startPosition, numBytes);
            ExtendMemoryChunkReferencesList(segmentsToAdd);
            // Debug.WriteLine($"Reference to completed memory added. References are {String.Join(", ", RecycledMemoryChunkReferences)}");
        }


        /// <summary>
        /// Enumerates memory chunk references based on an initial memory chunk index (not an ID), an offset into that memory chunk, and a length.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MemoryBlockIDAndSlice> EnumerateMemoryChunkReferences(int initialMemoryChunkIndex, int offset, long length)
        {
            int memoryChunkIndex = initialMemoryChunkIndex;
            long numBytesOfLengthRemaining = length;
            while (numBytesOfLengthRemaining > 0)
            {
                var memoryChunk = MemoryAtIndex(memoryChunkIndex);

                int numBytesThisChunk = memoryChunk.Length;
                int bytesToUseThisChunk = (int)Math.Min(numBytesThisChunk - offset, numBytesOfLengthRemaining);
                var sliceInfo = memoryChunk.SliceInfo.Slice(offset, bytesToUseThisChunk);
                yield return new MemoryBlockIDAndSlice(memoryChunk.MemoryBlockID, sliceInfo.Offset, sliceInfo.Length);

                numBytesOfLengthRemaining -= bytesToUseThisChunk;
                memoryChunkIndex++;
                offset = 0;
            }
        }

        /// <summary>
        /// Extends the bytes segment list to include the portion of active memory that is not included in active memory. 
        /// </summary>
        internal void RecordLastActiveMemoryChunkReference(int activeMemoryPosition)
        {
            if (activeMemoryPosition > NumActiveMemoryBytesAddedToRecycling)
            {
                int activeMemoryBlockID = GetNextMemoryBlockID();
                ExtendMemoryChunkReferencesList(new MemoryBlockIDAndSlice(activeMemoryBlockID, NumActiveMemoryBytesAddedToRecycling, activeMemoryPosition - NumActiveMemoryBytesAddedToRecycling), true);
                NumActiveMemoryBytesAddedToRecycling = activeMemoryPosition;
            }
        }

        public int GetIndexFromMemoryBlockID(int memoryBlockID)
        {
            for (int i = 0; i < MemoryChunks.Count; i++)
            {
                if (MemoryChunks[i].MemoryBlockID == memoryBlockID)
                    return i;
            }
            return -1;
        }
    }
}
