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
                Segments = new List<MemorySegmentIDAndSlice>();
        }

        public void SetFromLazinatorMemory(LazinatorMemory lazinatorMemory)
        {
            SetChunks(lazinatorMemory.EnumerateMemoryChunks());
            SetSegments(lazinatorMemory.EnumerateMemoryBlockIDsAndSlices().ToList());
        }

        private void SetSegments(List<MemorySegmentIDAndSlice> segments)
        {
            Segments = segments.ToList();
        }

        public bool Patching => Segments != null;

        internal long PatchesTotalLength { get; set; }

        /// <summary>
        /// The number of bytes of active memory that have already been referenced by MemorySegments.
        /// </summary>
        internal int NumActiveMemoryBytesReferenced;

        /// <summary>
        /// Extends a memory chunk references list by adding a new reference. If the new reference is contiguous to the last existing reference,
        /// then the list size remains constant. 
        /// </summary>
        /// <param name="memoryChunkReferences"></param>
        /// <param name="newSegment"></param>
        public void ExtendSegments(MemorySegmentIDAndSlice blockAndSlice, bool extendEarlierReferencesForSameChunk)
        {
            if (!Patching)
                throw new Exception("DEBUG");
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
                if (last.MemoryBlockID == blockAndSlice.MemoryBlockID && blockAndSlice.OffsetIntoMemoryChunk == last.OffsetIntoMemoryChunk + last.Length)
                {
                    last = new MemorySegmentIDAndSlice(last.MemoryBlockID, last.OffsetIntoMemoryChunk, last.Length + blockAndSlice.Length);
                    Segments[Segments.Count - 1] = last;
                    PatchesTotalLength += blockAndSlice.Length;
                    return;
                }
            }
            Segments.Add(new MemorySegmentIDAndSlice(blockAndSlice.MemoryBlockID, blockAndSlice.OffsetIntoMemoryChunk, blockAndSlice.Length));
            PatchesTotalLength += blockAndSlice.Length;
        }

        /// <summary>
        /// Adds new segments. The list is consolidated to avoid having consecutive entries for contiguous ranges.
        /// </summary>
        /// <param name="memoryChunkReferences"></param>
        /// <param name="newSegments"></param>
        public void ExtendSegments(IEnumerable<MemorySegmentIDAndSlice> newSegments)
        {
            foreach (var newSegment in newSegments)
                ExtendSegments(newSegment, false);
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
            IEnumerable<MemorySegmentIDAndSlice> segmentsToAdd = EnumerateMemoryBlockIDAndSlices(memoryChunkIndex, startPosition, numBytes);
            ExtendSegments(segmentsToAdd);
            // Debug.WriteLine($"Reference to completed memory added. References are {String.Join(", ", RecycledMemoryChunkReferences)}");
        }

        /// <summary>
        /// Extends the bytes segment list to include the portion of active memory that is not included in active memory. 
        /// </summary>
        internal void RecordLastActiveMemoryChunkReference(int activeMemoryPosition)
        {
            if (activeMemoryPosition > NumActiveMemoryBytesReferenced)
            {
                int activeMemoryBlockID = GetNextMemoryBlockID();
                ExtendSegments(new MemorySegmentIDAndSlice(activeMemoryBlockID, NumActiveMemoryBytesReferenced, activeMemoryPosition - NumActiveMemoryBytesReferenced), true);
                NumActiveMemoryBytesReferenced = activeMemoryPosition;
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

        public override void AppendMemoryChunk(MemoryChunk memoryChunk)
        {
            base.AppendMemoryChunk(memoryChunk);
            if (Segments != null)
                Segments.Add(new MemorySegmentIDAndSlice(memoryChunk.MemoryBlockID, 0, memoryChunk.Length));
        }

        public override int NumMemorySegments => Patching ? Segments.Count : base.NumMemorySegments;

        protected override int GetLengthOfSegment(int segmentIndex) => Patching ? Segments[segmentIndex].Length : base.GetLengthOfSegment(segmentIndex); 
        
        public override MemorySegment MemorySegmentAtIndex(int i)
        {
            if (Segments == null)
                return base.MemorySegmentAtIndex(i);
            var segment = Segments[i];
            var chunk = MemoryChunks[GetIndexFromMemoryBlockID(segment.MemoryBlockID)];
            chunk.LoadMemory();
            return new MemorySegment(chunk, new MemoryChunkSlice(0, chunk.Length));
        }

        public async override ValueTask<MemorySegment> MemorySegmentAtIndexAsync(int i)
        {
            if (Segments == null)
                return await base.MemorySegmentAtIndexAsync(i);
            var segment = Segments[i];
            var chunk = MemoryChunks[GetIndexFromMemoryBlockID(segment.MemoryBlockID)];
            await chunk.LoadMemoryAsync();
            return new MemorySegment(chunk, new MemoryChunkSlice(0, chunk.Length));
        }

        public override IEnumerable<MemorySegment> EnumerateMemorySegments()
        {
            if (Segments == null)
            {
                foreach (var segment in base.EnumerateMemorySegments())
                    yield return segment;
            }
            else
            {
                for (int i = 0; i < NumMemorySegments; i++)
                    yield return MemorySegmentAtIndex(i);
            }
        }
    }
}
