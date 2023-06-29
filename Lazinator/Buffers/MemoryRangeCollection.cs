using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class MemoryRangeCollection : MemoryChunkCollection, IMemoryRangeCollection
    {
        public MemoryRangeCollection(LazinatorMemory lazinatorMemory, bool recycle) : this(lazinatorMemory.EnumerateMemoryChunks().ToList(), recycle)
        {
        }

        public MemoryRangeCollection(MemoryChunk chunk, bool recycle) : this(new List<MemoryChunk> { chunk }, recycle)
        {
        }

        public MemoryRangeCollection(List<MemoryChunk> memoryChunks, bool recycle) : base(memoryChunks)
        {
            if (recycle)
                Ranges = new List<MemoryRangeByID>();
        }

        public void SetFromLazinatorMemory(LazinatorMemory lazinatorMemory)
        {
            SetChunks(lazinatorMemory.EnumerateMemoryChunks());
            SetRanges(lazinatorMemory.EnumerateMemoryRangesByID().ToList());
        }

        private void SetRanges(List<MemoryRangeByID> ranges)
        {
            Ranges = ranges.ToList();
            PatchesTotalLength = Ranges.Sum(x => (long) x.Length);
        }

        public bool Patching => Ranges != null;

        internal long PatchesTotalLength 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// The number of bytes of active memory that have already been referenced by ranges.
        /// </summary>
        internal int NumActiveMemoryBytesReferenced;

        /// <summary>
        /// Extends a memory chunk references list by adding a new reference. If the new reference is contiguous to the last existing reference,
        /// then the list size remains constant. 
        /// </summary>
        /// <param name="memoryChunkReferences"></param>
        /// <param name="newRange"></param>
        public void ExtendRanges(MemoryRangeByID range, bool extendEarlierReferencesForSameChunk)
        {
            if (!Patching)
                throw new Exception("Internal error."); // DEBUG -- should be able to delete.
            if (Ranges.Any())
            {
                if (extendEarlierReferencesForSameChunk)
                {
                    for (int i = 0; i < Ranges.Count; i++)
                    {
                        var rangeI = Ranges[i];
                        if (rangeI.GetMemoryBlockID() == range.GetMemoryBlockID()) 
                        {
                            MemoryChunk existingChunk = GetMemoryChunkByMemoryBlockID(rangeI.GetMemoryBlockID());
                            if (existingChunk != null)
                            {
                                existingChunk.LoadingInfo.MemoryBlockLength = range.Length;
                            }
                        }
                    }
                }
                var last = Ranges.Last();
                if (last.GetMemoryBlockID() == range.GetMemoryBlockID() && range.OffsetIntoMemoryChunk == last.OffsetIntoMemoryChunk + last.Length)
                {
                    var replacementLast = new MemoryRangeByID(last.GetMemoryBlockID(), last.OffsetIntoMemoryChunk, last.Length + range.Length);
                    Ranges[Ranges.Count - 1] = replacementLast;
                    PatchesTotalLength += range.Length; // i.e., we're replacing last.Length with last>Length + blockAndSlice.Length, so this is the increment.
                    return;
                }
            }
            Ranges.Add(new MemoryRangeByID(range.GetMemoryBlockID(), range.OffsetIntoMemoryChunk, range.Length));
            PatchesTotalLength += range.Length;
        }

        /// <summary>
        /// Adds new ranges. The list is consolidated to avoid having consecutive entries for contiguous ranges.
        /// </summary>
        /// <param name="newRanges"></param>
        public void ExtendRanges(IEnumerable<MemoryRangeByID> newRanges)
        {
            foreach (var newRange in newRanges)
                ExtendRanges(newRange, false);
        }


        /// <summary>
        /// Writes from CompletedMemory. Instead of copying the bytes, it simply adds one or more ranges referring to where those bytes are in the overall sets of bytes.
        /// </summary>
        /// <param name="rangeIndex">The index of the memory chunk</param>
        /// <param name="startPosition">The position of the first byte of the memory within the indexed memory chunk</param>
        /// <param name="numBytes"></param>
        internal void InsertReferenceToCompletedMemory(MemoryRangeCollection completedMemoryRangeCollection, int rangeIndex, int startPosition, long numBytes, int activeMemoryPosition)
        {
            RecordLastActiveMemoryChunkReference(activeMemoryPosition);
            IEnumerable<MemoryRangeByID> rangesToAdd = completedMemoryRangeCollection.EnumerateMemoryRangesByID(rangeIndex, startPosition, numBytes);
            ExtendRanges(rangesToAdd);
            // Debug.WriteLine($"Reference to completed memory added. References are {String.Join(", ", RecycledMemoryChunkReferences)}");
        }

        /// <summary>
        /// Extends the ranges list to include the portion of active memory that is not included in active memory. 
        /// </summary>
        internal void RecordLastActiveMemoryChunkReference(int activeMemoryPosition)
        {
            if (activeMemoryPosition > NumActiveMemoryBytesReferenced)
            {
                MemoryBlockID activeMemoryBlockID = GetNextMemoryBlockID();
                ExtendRanges(new MemoryRangeByID(activeMemoryBlockID, NumActiveMemoryBytesReferenced, activeMemoryPosition - NumActiveMemoryBytesReferenced), true);
                NumActiveMemoryBytesReferenced = activeMemoryPosition;
            }
        }

        public override void AppendMemoryChunk(MemoryChunk memoryChunk)
        {
            base.AppendMemoryChunk(memoryChunk);
            if (Ranges != null)
            {
                Ranges.Add(new MemoryRangeByID(memoryChunk.MemoryBlockID, 0, memoryChunk.Length));
                PatchesTotalLength += memoryChunk.Length;
            }
        }

        public override int NumMemoryRanges => Patching ? Ranges.Count : base.NumMemoryRanges;

        protected override int GetRangeLength(int rangeIndex) => Patching ? Ranges[rangeIndex].Length : base.GetRangeLength(rangeIndex); 
        protected override int GetOffsetIntoChunkForRange(int rangeIndex) => Patching ? Ranges[rangeIndex].OffsetIntoMemoryChunk : 0;
        
        public override MemoryRange MemoryRangeAtIndex(int i)
        {
            if (Ranges == null)
                return base.MemoryRangeAtIndex(i);
            int memoryChunkIndex = GetMemoryChunkIndexFromMemoryRangeIndex(i);
            if (memoryChunkIndex == -1)
                return default;
            var range = Ranges[i];
            var chunk = MemoryChunkAtIndex(memoryChunkIndex);
            return new MemoryRange(chunk, new MemoryChunkSlice(range.OffsetIntoMemoryChunk, range.Length)); 
        }

        public async override ValueTask<MemoryRange> MemoryRangeAtIndexAsync(int i)
        {
            if (Ranges == null)
                return await base.MemoryRangeAtIndexAsync(i);
            int memoryChunkIndex = GetMemoryChunkIndexFromMemoryRangeIndex(i);
            if (memoryChunkIndex == -1)
                return default;
            var range = Ranges[i];
            var chunk = await MemoryChunkAtIndexAsync(memoryChunkIndex);
            return new MemoryRange(chunk, new MemoryChunkSlice(range.OffsetIntoMemoryChunk, range.Length));
        }

        protected override int GetMemoryChunkIndexFromMemoryRangeIndex(int i)
        {
            if (Ranges == null || !Ranges.Any())
                return i;
            if (Ranges.Count < i + 1)
                return -1;
            var range = Ranges[i];
            int index = GetMemoryChunkIndexFromBlockID(range.GetMemoryBlockID());
            return index;
        }

        public override string ToString()
        {
            return $"Chunks: {String.Join(",", MemoryChunks.Select(x => x.MemoryBlockID.GetIntID()))} Ranges: {String.Join("; ", Ranges)}";
        }
    }
}
