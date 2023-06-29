using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public partial class MemoryRangeCollection : MemoryBlockCollection, IMemoryRangeCollection
    {
        public MemoryRangeCollection(LazinatorMemory lazinatorMemory, bool recycle) : this(lazinatorMemory.EnumerateMemoryBlocks().ToList(), recycle)
        {
        }

        public MemoryRangeCollection(MemoryBlock block, bool recycle) : this(new List<MemoryBlock> { block }, recycle)
        {
        }

        public MemoryRangeCollection(List<MemoryBlock> memoryBlocks, bool recycle) : base(memoryBlocks)
        {
            if (recycle)
                Ranges = new List<MemoryRangeByID>();
        }

        public void SetFromLazinatorMemory(LazinatorMemory lazinatorMemory)
        {
            SetBlocks(lazinatorMemory.EnumerateMemoryBlocks());
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
        /// Extends a memory block references list by adding a new reference. If the new reference is contiguous to the last existing reference,
        /// then the list size remains constant. 
        /// </summary>
        /// <param name="memoryBlockReferences"></param>
        /// <param name="newRange"></param>
        public void ExtendRanges(MemoryRangeByID range, bool extendEarlierReferencesForSameBlock)
        {
            if (!Patching)
                throw new Exception("Internal error."); // DEBUG -- should be able to delete.
            if (Ranges.Any())
            {
                if (extendEarlierReferencesForSameBlock)
                {
                    for (int i = 0; i < Ranges.Count; i++)
                    {
                        var rangeI = Ranges[i];
                        if (rangeI.GetMemoryBlockID() == range.GetMemoryBlockID()) 
                        {
                            MemoryBlock existingBlock = GetMemoryBlockByMemoryBlockID(rangeI.GetMemoryBlockID());
                            if (existingBlock != null)
                            {
                                existingBlock.LoadingInfo.MemoryBlockLength = range.Length;
                            }
                        }
                    }
                }
                var last = Ranges.Last();
                if (last.GetMemoryBlockID() == range.GetMemoryBlockID() && range.OffsetIntoMemoryBlock == last.OffsetIntoMemoryBlock + last.Length)
                {
                    var replacementLast = new MemoryRangeByID(last.GetMemoryBlockID(), last.OffsetIntoMemoryBlock, last.Length + range.Length);
                    Ranges[Ranges.Count - 1] = replacementLast;
                    PatchesTotalLength += range.Length; // i.e., we're replacing last.Length with last>Length + blockAndSlice.Length, so this is the increment.
                    return;
                }
            }
            Ranges.Add(new MemoryRangeByID(range.GetMemoryBlockID(), range.OffsetIntoMemoryBlock, range.Length));
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
        /// <param name="rangeIndex">The index of the memory block</param>
        /// <param name="startPosition">The position of the first byte of the memory within the indexed memory block</param>
        /// <param name="numBytes"></param>
        internal void InsertReferenceToCompletedMemory(MemoryRangeCollection completedMemoryRangeCollection, int rangeIndex, int startPosition, long numBytes, int activeMemoryPosition)
        {
            RecordLastActiveMemoryBlockReference(activeMemoryPosition);
            IEnumerable<MemoryRangeByID> rangesToAdd = completedMemoryRangeCollection.EnumerateMemoryRangesByID(rangeIndex, startPosition, numBytes);
            ExtendRanges(rangesToAdd);
            // Debug.WriteLine($"Reference to completed memory added. References are {String.Join(", ", RecycledMemoryBlockReferences)}");
        }

        /// <summary>
        /// Extends the ranges list to include the portion of active memory that is not included in active memory. 
        /// </summary>
        internal void RecordLastActiveMemoryBlockReference(int activeMemoryPosition)
        {
            if (activeMemoryPosition > NumActiveMemoryBytesReferenced)
            {
                MemoryBlockID activeMemoryBlockID = GetNextMemoryBlockID();
                ExtendRanges(new MemoryRangeByID(activeMemoryBlockID, NumActiveMemoryBytesReferenced, activeMemoryPosition - NumActiveMemoryBytesReferenced), true);
                NumActiveMemoryBytesReferenced = activeMemoryPosition;
            }
        }

        public override void AppendMemoryBlock(MemoryBlock memoryBlock)
        {
            base.AppendMemoryBlock(memoryBlock);
            if (Ranges != null)
            {
                Ranges.Add(new MemoryRangeByID(memoryBlock.MemoryBlockID, 0, memoryBlock.Length));
                PatchesTotalLength += memoryBlock.Length;
            }
        }

        public override int NumMemoryRanges => Patching ? Ranges.Count : base.NumMemoryRanges;

        protected override int GetRangeLength(int rangeIndex) => Patching ? Ranges[rangeIndex].Length : base.GetRangeLength(rangeIndex); 
        protected override int GetOffsetIntoBlockForRange(int rangeIndex) => Patching ? Ranges[rangeIndex].OffsetIntoMemoryBlock : 0;
        
        public override MemoryRange MemoryRangeAtIndex(int i)
        {
            if (Ranges == null)
                return base.MemoryRangeAtIndex(i);
            int memoryBlockIndex = GetMemoryBlockIndexFromMemoryRangeIndex(i);
            if (memoryBlockIndex == -1)
                return default;
            var range = Ranges[i];
            var block = MemoryBlockAtIndex(memoryBlockIndex);
            return new MemoryRange(block, new MemoryBlockSlice(range.OffsetIntoMemoryBlock, range.Length)); 
        }

        public async override ValueTask<MemoryRange> MemoryRangeAtIndexAsync(int i)
        {
            if (Ranges == null)
                return await base.MemoryRangeAtIndexAsync(i);
            int memoryBlockIndex = GetMemoryBlockIndexFromMemoryRangeIndex(i);
            if (memoryBlockIndex == -1)
                return default;
            var range = Ranges[i];
            var block = await MemoryBlockAtIndexAsync(memoryBlockIndex);
            return new MemoryRange(block, new MemoryBlockSlice(range.OffsetIntoMemoryBlock, range.Length));
        }

        protected override int GetMemoryBlockIndexFromMemoryRangeIndex(int i)
        {
            if (Ranges == null || !Ranges.Any())
                return i;
            if (Ranges.Count < i + 1)
                return -1;
            var range = Ranges[i];
            int index = GetMemoryBlockIndexFromBlockID(range.GetMemoryBlockID());
            return index;
        }

        public override string ToString()
        {
            return $"Blocks: {String.Join(",", MemoryBlocks.Select(x => x.MemoryBlockID.GetIntID()))} Ranges: {String.Join("; ", Ranges)}";
        }
    }
}
