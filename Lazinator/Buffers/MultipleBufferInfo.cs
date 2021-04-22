using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public class MultipleBufferInfo
    {
        public MultipleBufferInfo(LazinatorMemory completedMemory, bool recycle)
        {
            CompletedMemory = completedMemory;
            if (recycle)
                RecycledMemoryChunkReferences = new List<MemoryChunkReference>();
        }

        /// <summary>
        /// Bytes that were previously written. They may have been written in the same serialization pass (created when ExpandableBytes became full) or 
        /// in a previous serialization pass (when serializing diffs).
        /// </summary>
        public LazinatorMemory CompletedMemory { get; internal set; }

        /// <summary>
        /// When serializing diffs, these are non-null and will refer to various segments in CompletedMemory and ActiveMemory in order.
        /// </summary>
        private List<MemoryChunkReference> RecycledMemoryChunkReferences;

        public bool Recycling => RecycledMemoryChunkReferences != null;

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
        public void ExtendMemoryChunkReferencesList(MemoryChunkReference newSegment, bool extendEarlierReferencesForSameChunk)
        {
            if (RecycledMemoryChunkReferences.Any())
            {
                if (extendEarlierReferencesForSameChunk)
                {
                    for (int i = 0; i < RecycledMemoryChunkReferences.Count; i++)
                    {
                        MemoryChunkReference memoryChunkReference = RecycledMemoryChunkReferences[i];
                        if (memoryChunkReference.MemoryChunkID == newSegment.MemoryChunkID && memoryChunkReference.PreTruncationLength != newSegment.PreTruncationLength)
                        {
                            RecycledMemoryChunkReferences[i] = memoryChunkReference.WithPreTruncationLength(newSegment.PreTruncationLength);
                        }
                    }
                }
                MemoryChunkReference last = RecycledMemoryChunkReferences.Last();
                if (last.SameLoadingInformation(newSegment) && newSegment.AdditionalOffset == last.AdditionalOffset + last.FinalLength)
                {
                    last.FinalLength += newSegment.FinalLength;
                    RecycledMemoryChunkReferences[RecycledMemoryChunkReferences.Count - 1] = last;
                    RecycledTotalLength += newSegment.FinalLength;
                    return;
                }
            }
            RecycledMemoryChunkReferences.Add(newSegment);
            RecycledTotalLength += newSegment.FinalLength;
        }

        /// <summary>
        /// Extends a memory chunk references list by adding new segments. The list is consolidated to avoid having consecutive entries for contiguous ranges.
        /// </summary>
        /// <param name="memoryChunkReferences"></param>
        /// <param name="newSegments"></param>
        public void ExtendMemoryChunkReferencesList(IEnumerable<MemoryChunkReference> newSegments)
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
            IEnumerable<MemoryChunkReference> segmentsToAdd = CompletedMemory.EnumerateMemoryChunkReferences(memoryChunkIndex, startPosition, numBytes);
            ExtendMemoryChunkReferencesList(segmentsToAdd);
            // Debug.WriteLine($"Reference to completed memory added. References are {String.Join(", ", RecycledMemoryChunkReferences)}");
        }

        /// <summary>
        /// Extends the bytes segment list to include the portion of active memory that is not included in active memory. 
        /// </summary>
        internal void RecordLastActiveMemoryChunkReference(int activeMemoryPosition)
        {
            if (activeMemoryPosition > NumActiveMemoryBytesAddedToRecycling)
            {
                int activeMemoryChunkID = GetActiveMemoryChunkID();
                ExtendMemoryChunkReferencesList(new MemoryChunkReference(activeMemoryChunkID, 0, activeMemoryPosition, NumActiveMemoryBytesAddedToRecycling, activeMemoryPosition - NumActiveMemoryBytesAddedToRecycling), true);
                NumActiveMemoryBytesAddedToRecycling = activeMemoryPosition;
            }
        }

        internal LazinatorMemory CompletePatchLazinatorMemory(int activeLength, int activeMemoryChunkID)
        {
            var byID = CompletedMemory.GetMemoryChunksByID();
            if (activeLength > 0)
            {
                var activeMemoryChunk = byID[activeMemoryChunkID];
                activeMemoryChunk.Reference = new MemoryChunkReference(activeMemoryChunkID, 0, activeLength, 0, 0);
                byID[activeMemoryChunkID] = activeMemoryChunk;
            }
            MemoryChunk initialMemoryChunk = null;
            List<MemoryChunk> moreMemory = null;
            long length = 0;
            for (int i = 0; i < RecycledMemoryChunkReferences.Count; i++)
            {
                if (i == 1)
                    moreMemory = new List<MemoryChunk>();
                MemoryChunkReference reference = RecycledMemoryChunkReferences[i];
                MemoryChunk memoryChunk = byID[reference.MemoryChunkID];
                MemoryChunk resliced = memoryChunk.WithReference(reference.WithAdditionalOffsetAndFinalLength(reference.AdditionalOffset, reference.FinalLength));
                length += reference.FinalLength;
                if (i == 0)
                    initialMemoryChunk = resliced;
                else
                    moreMemory.Add(resliced);
            }
            return new LazinatorMemory(initialMemoryChunk, moreMemory, 0, 0, length);
        }

        /// <summary>
        /// Returns the version number for active memory (equal to the last version number for CompletedMemory plus one). 
        /// </summary>
        /// <returns></returns>
        internal int GetActiveMemoryChunkID()
        {
            return CompletedMemory.GetNextMemoryChunkID();
        }


    }
}
