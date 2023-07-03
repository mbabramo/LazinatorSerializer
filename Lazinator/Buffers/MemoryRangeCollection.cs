using Microsoft.CodeAnalysis.Host;
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

        public MemoryRangeCollection(MemoryBlock block, bool initiatePatching) : this(new List<MemoryBlock> { block }, initiatePatching)
        {
        }

        public MemoryRangeCollection(List<MemoryBlock> memoryBlocks, bool initiatePatching) : base(memoryBlocks)
        {
            if (initiatePatching)
            {
                Ranges = new List<MemoryRangeByBlockID>(); // We don't actually create the ranges yet
            }
        }

        public MemoryRangeCollection(List<MemoryBlock> memoryBlocks, List<MemoryRangeByBlockID> memoryRanges) : base(memoryBlocks)
        {
            SetRanges(memoryRanges);
        }

        public override MemoryBlockCollection DeepCopy()
        {
            var collection = new MemoryRangeCollection(MemoryBlocks, Ranges);
            return collection;
        }

        public void SetFromLazinatorMemory(LazinatorMemory lazinatorMemory)
        {
            SetBlocks(lazinatorMemory.EnumerateMemoryBlocks());
            SetRanges(lazinatorMemory.EnumerateMemoryRangesByID().ToList());
        }

        private void SetRanges(List<MemoryRangeByBlockID> ranges)
        {
            Ranges = ranges.ToList();
            PatchesTotalLength = Ranges.Sum(x => (long) x.Length);
        }

        /// <summary>
        /// Whether this MemoryRangeCollection uses patching (i.e., includes a list of MemoryRanges). If false, this is functioning just like a MemoryBlockCollection, but one that can be converted to a MemoryRangeCollection if necessary.
        /// </summary>
        public bool Patching => Ranges != null;

        public override long LengthReferenced => Patching ? PatchesTotalLength : base.LengthReferenced;

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
        public void AddRange(MemoryRangeByBlockID range, bool extendEarlierReferencesForSameBlock)
        {
            if (!Patching)
            {
                SetRanges(EnumerateMemoryBlocks().Select(x => new MemoryRangeByBlockID(x.MemoryBlockID, 0, x.Length)).ToList());
            }
            if (Ranges.Any())
            {
                if (extendEarlierReferencesForSameBlock)
                {
                    for (int i = 0; i < Ranges.Count; i++)
                    {
                        var rangeI = Ranges[i];
                        if (rangeI.GetMemoryBlockID() == range.GetMemoryBlockID()) 
                        {
                            MemoryBlock existingBlock = GetMemoryBlockByBlockID(rangeI.GetMemoryBlockID());
                            if (existingBlock != null)
                            {
                                throw new Exception("DEBUGCHECK"); // is this really correct?
                                existingBlock.LoadingInfo.MemoryBlockLength = range.Length;
                            }
                        }
                    }
                }
                var last = Ranges.Last();
                if (last.GetMemoryBlockID() == range.GetMemoryBlockID() && range.OffsetIntoMemoryBlock == last.OffsetIntoMemoryBlock + last.Length)
                {
                    var replacementLast = new MemoryRangeByBlockID(last.GetMemoryBlockID(), last.OffsetIntoMemoryBlock, last.Length + range.Length);
                    Ranges[Ranges.Count - 1] = replacementLast;
                    PatchesTotalLength += range.Length; // i.e., we're replacing last.Length with last>Length + blockAndSlice.Length, so this is the increment.
                    return;
                }
            }
            Ranges.Add(new MemoryRangeByBlockID(range.GetMemoryBlockID(), range.OffsetIntoMemoryBlock, range.Length));
            PatchesTotalLength += range.Length;
        }

        /// <summary>
        /// Adds new ranges. The list is consolidated to avoid having consecutive entries for contiguous ranges.
        /// </summary>
        /// <param name="newRanges"></param>
        public void AddRanges(IEnumerable<MemoryRangeByBlockID> newRanges)
        {
            foreach (var newRange in newRanges)
                AddRange(newRange, false);
        }


        /// <summary>
        /// Writes from previous version. Instead of copying the bytes, it simply adds one or more ranges referring to where those bytes are in the overall sets of bytes.
        /// </summary>
        /// <param name="rangeIndex">The index of the memory block</param>
        /// <param name="startPosition">The position of the first byte of the memory within the indexed memory block</param>
        /// <param name="numBytes"></param>
        internal void InsertReferenceToPreviousVersion(MemoryBlockCollection previousVersion, int rangeIndex, int startPosition, long numBytes, int activeMemoryPosition)
        {
            RecordLastActiveMemoryBlockReference(activeMemoryPosition);
            IEnumerable<MemoryRangeByBlockID> rangesToAdd = previousVersion.EnumerateMemoryRangesWithBlockID(rangeIndex, startPosition, numBytes);
            AddRanges(rangesToAdd);
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
                AddRange(new MemoryRangeByBlockID(activeMemoryBlockID, NumActiveMemoryBytesReferenced, activeMemoryPosition - NumActiveMemoryBytesReferenced), true);
                NumActiveMemoryBytesReferenced = activeMemoryPosition;
            }
        }

        public override void AppendMemoryBlock(MemoryBlock memoryBlock)
        {
            base.AppendMemoryBlock(memoryBlock);
            if (Ranges != null)
            {
                // See if we have already been adding ranges referring to this memory block.
                int numBytesAlreadyReferredTo = 0;
                if (Ranges.Any())
                {
                    var previousRange = Ranges.Last();
                    if (previousRange.MemoryBlockIntID == memoryBlock.MemoryBlockID.GetIntID())
                        numBytesAlreadyReferredTo = previousRange.Length;
                }
                // Now add a range referring to the memory block or what hasn't been referred to yet
                if (numBytesAlreadyReferredTo < memoryBlock.Length)
                    AddRange(new MemoryRangeByBlockID(memoryBlock.MemoryBlockID, numBytesAlreadyReferredTo, memoryBlock.Length - numBytesAlreadyReferredTo), false);
            }
        }

        public override int NumMemoryRanges => Patching ? Ranges.Count : base.NumMemoryRanges;

        protected override int GetRangeLength(int rangeIndex) => Patching ? Ranges[rangeIndex].Length : base.GetRangeLength(rangeIndex); 
        protected override int GetOffsetIntoBlockForRange(int rangeIndex) => Patching ? Ranges[rangeIndex].OffsetIntoMemoryBlock : 0;
        
        public override MemoryRange MemoryRangeAtIndex(int i)
        {
            if (i == -1)
                return default;
            if (Ranges == null)
                return base.MemoryRangeAtIndex(i);
            if (i >= Ranges.Count)
                return default;
            InitializeMemoryBlocksInformationIfNecessary();
            int memoryBlockIndex = GetMemoryBlockIndexFromMemoryRangeIndex(i);
            var range = Ranges[i];
            if (memoryBlockIndex == -1)
            {
                MemoryBlockID memoryBlockID = range.GetMemoryBlockID();
                var match = MemoryBlocksLoadingInfo.Select((loadingInfo, index) => (loadingInfo, index)).FirstOrDefault(x => x.loadingInfo.MemoryBlockID == memoryBlockID);
                if (match.loadingInfo == null)
                    return default;
                memoryBlockIndex = match.index;
            }
            var block = MemoryBlockAtIndex(memoryBlockIndex);
            return new MemoryRange(block, new MemoryBlockSlice(range.OffsetIntoMemoryBlock, range.Length)); 
        }

        public async override ValueTask<MemoryRange> MemoryRangeAtIndexAsync(int i)
        {
            if (i == -1)
                return default;
            if (Ranges == null)
                return await base.MemoryRangeAtIndexAsync(i);
            if (i >= Ranges.Count)
                return default;
            InitializeMemoryBlocksInformationIfNecessary();
            int memoryBlockIndex = GetMemoryBlockIndexFromMemoryRangeIndex(i);
            var range = Ranges[i];
            if (memoryBlockIndex == -1)
            {
                MemoryBlockID memoryBlockID = range.GetMemoryBlockID();
                var match = MemoryBlocksLoadingInfo.Select((loadingInfo, index) => (loadingInfo, index)).FirstOrDefault(x => x.loadingInfo.MemoryBlockID == memoryBlockID);
                if (match.loadingInfo == null)
                    return default;
                memoryBlockIndex = match.index;
            }
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
            int index = GetMemoryBlockIndexFromBlockID(GetMemoryBlockIndexFromMemoryRange(range));
            return index;
        }

        protected MemoryBlockID? GetMemoryBlockIDFromMemoryRangeIndex(int i)
        {
            if (Ranges == null || !Ranges.Any())
                return MemoryBlockAtIndex(i).MemoryBlockID;
            if (Ranges.Count < i + 1)
                return null;
            var range = Ranges[i];
            return new MemoryBlockID(range.MemoryBlockIntID);
        }

        private static MemoryBlockID GetMemoryBlockIndexFromMemoryRange(MemoryRangeByBlockID range)
        {
            return range.GetMemoryBlockID();
        }

        public override string ToString()
        {
            return $"Blocks: {String.Join(",", MemoryBlocks.Select(x => x.MemoryBlockID.GetIntID()))} Ranges: {String.Join("; ", Ranges)}";
        }
    }
}
