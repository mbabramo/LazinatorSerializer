using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                SegmentInfos = new List<MemorySegmentLocationByID>();
        }

        public void SetFromLazinatorMemory(LazinatorMemory lazinatorMemory)
        {
            SetChunks(lazinatorMemory.EnumerateMemoryChunks());
            SetSegments(lazinatorMemory.EnumerateMemoryBlockIDsAndSlices().ToList());
        }

        private void SetSegments(List<MemorySegmentLocationByID> segments)
        {
            SegmentInfos = segments.ToList();
            PatchesTotalLength = SegmentInfos.Sum(x => (long) x.Length);
        }

        public bool Patching => SegmentInfos != null;

        internal long PatchesTotalLength 
        { 
            get; 
            set; 
        }

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
        public void ExtendSegments(MemorySegmentLocationByID blockAndSlice, bool extendEarlierReferencesForSameChunk)
        {
            if (!Patching)
                throw new Exception("Internal error."); // DEBUG -- should be able to delete.
            if (SegmentInfos.Any())
            {
                if (extendEarlierReferencesForSameChunk)
                {
                    for (int i = 0; i < SegmentInfos.Count; i++)
                    {
                        var segment = SegmentInfos[i];
                        if (segment.GetMemoryBlockID() == blockAndSlice.GetMemoryBlockID()) 
                        {
                            MemoryChunk existingChunk = GetMemoryChunkByMemoryBlockID(segment.GetMemoryBlockID());
                            if (existingChunk != null)
                            {
                                existingChunk.LoadingInfo.MemoryBlockLength = blockAndSlice.Length;
                            }
                        }
                    }
                }
                var last = SegmentInfos.Last();
                if (last.GetMemoryBlockID() == blockAndSlice.GetMemoryBlockID() && blockAndSlice.OffsetIntoMemoryChunk == last.OffsetIntoMemoryChunk + last.Length)
                {
                    var replacementLast = new MemorySegmentLocationByID(last.GetMemoryBlockID(), last.OffsetIntoMemoryChunk, last.Length + blockAndSlice.Length);
                    SegmentInfos[SegmentInfos.Count - 1] = replacementLast;
                    PatchesTotalLength += blockAndSlice.Length; // i.e., we're replacing last.Length with last>Length + blockAndSlice.Length, so this is the increment.
                    return;
                }
            }
            SegmentInfos.Add(new MemorySegmentLocationByID(blockAndSlice.GetMemoryBlockID(), blockAndSlice.OffsetIntoMemoryChunk, blockAndSlice.Length));
            PatchesTotalLength += blockAndSlice.Length;
        }

        /// <summary>
        /// Adds new segments. The list is consolidated to avoid having consecutive entries for contiguous ranges.
        /// </summary>
        /// <param name="memoryChunkReferences"></param>
        /// <param name="newSegments"></param>
        public void ExtendSegments(IEnumerable<MemorySegmentLocationByID> newSegments)
        {
            foreach (var newSegment in newSegments)
                ExtendSegments(newSegment, false);
        }


        /// <summary>
        /// Writes from CompletedMemory. Instead of copying the bytes, it simply adds one or more segments referring to where those bytes are in the overall sets of bytes.
        /// </summary>
        /// <param name="memoryChunkIndex">The index of the memory chunk</param>
        /// <param name="startPosition">The position of the first byte of the memory within the indexed memory chunk</param>
        /// <param name="numBytes"></param>
        internal void InsertReferenceToCompletedMemory(int memoryChunkIndex, int startPosition, long numBytes, int activeMemoryPosition)
        {
            RecordLastActiveMemoryChunkReference(activeMemoryPosition);
            IEnumerable<MemorySegmentLocationByID> segmentsToAdd = EnumerateMemorySegmentLocationsByID(memoryChunkIndex, startPosition, numBytes);
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
                MemoryBlockID activeMemoryBlockID = GetNextMemoryBlockID();
                ExtendSegments(new MemorySegmentLocationByID(activeMemoryBlockID, NumActiveMemoryBytesReferenced, activeMemoryPosition - NumActiveMemoryBytesReferenced), true);
                NumActiveMemoryBytesReferenced = activeMemoryPosition;
            }
        }

        public override void AppendMemoryChunk(MemoryChunk memoryChunk)
        {
            base.AppendMemoryChunk(memoryChunk);
            if (SegmentInfos != null)
            {
                SegmentInfos.Add(new MemorySegmentLocationByID(memoryChunk.MemoryBlockID, 0, memoryChunk.Length));
                PatchesTotalLength += memoryChunk.Length;
            }
        }

        public override int NumMemorySegments => Patching ? SegmentInfos.Count : base.NumMemorySegments;

        protected override int GetLengthOfSegment(int segmentIndex) => Patching ? SegmentInfos[segmentIndex].Length : base.GetLengthOfSegment(segmentIndex); 
        protected override int GetOffsetIntoChunkForSegment(int segmentIndex) => Patching ? SegmentInfos[segmentIndex].OffsetIntoMemoryChunk : 0;
        
        public override MemorySegment MemorySegmentAtIndex(int i)
        {
            if (SegmentInfos == null)
                return base.MemorySegmentAtIndex(i);
            int memoryChunkIndex = GetMemoryChunkIndexFromMemorySegmentIndex(i);
            if (memoryChunkIndex == -1)
                return default;
            var segmentInfo = SegmentInfos[i];
            var chunk = MemoryChunkAtIndex(memoryChunkIndex);
            return new MemorySegment(chunk, new MemoryChunkSlice(segmentInfo.OffsetIntoMemoryChunk, segmentInfo.Length)); 
        }

        public async override ValueTask<MemorySegment> MemorySegmentAtIndexAsync(int i)
        {
            if (SegmentInfos == null)
                return await base.MemorySegmentAtIndexAsync(i);
            int memoryChunkIndex = GetMemoryChunkIndexFromMemorySegmentIndex(i);
            if (memoryChunkIndex == -1)
                return default;
            var segmentInfo = SegmentInfos[i];
            var chunk = await MemoryChunkAtIndexAsync(memoryChunkIndex);
            return new MemorySegment(chunk, new MemoryChunkSlice(segmentInfo.OffsetIntoMemoryChunk, segmentInfo.Length));
        }

        protected override int GetMemoryChunkIndexFromMemorySegmentIndex(int i)
        {
            if (SegmentInfos == null || SegmentInfos.Count < i + 1)
                return -1;
            var segment = SegmentInfos[i];
            int index = GetMemoryChunkIndexFromBlockID(segment.GetMemoryBlockID());
            return index;
        }

        public override string ToString()
        {
            return $"Chunks: {String.Join(",", MemoryChunks.Select(x => x.MemoryBlockID.GetIntID()))} Segments: {String.Join("; ", SegmentInfos)}";
        }
    }
}
