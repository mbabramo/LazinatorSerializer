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
        public MultipleBufferInfo(LazinatorMemory lazinatorMemory, bool recycle) : this(new MemoryChunkCollection(lazinatorMemory), recycle)
        {
        }

        public MultipleBufferInfo(MemoryChunk chunk, bool recycle) : this(new MemoryChunkCollection(chunk), recycle)
        {
        }

        public MultipleBufferInfo(IMemoryChunkCollection memoryChunkCollection, bool recycle)
        {
            MemoryChunks = memoryChunkCollection;
            if (recycle)
                RecycledMemoryChunkReferences = new List<MemoryChunkReference>();
        }

        public MultipleBufferInfo WithAppendedMemoryChunk(MemoryChunk memoryChunk) => new MultipleBufferInfo(MemoryChunks.WithAppendedMemoryChunk(memoryChunk), RecycledMemoryChunkReferences != null);

        /// <summary>
        /// Bytes that were previously written. They may have been written in the same serialization pass (created when ExpandableBytes became full) or 
        /// in a previous serialization pass (when serializing diffs).
        /// </summary>
        public IMemoryChunkCollection MemoryChunks { get; internal set; }

        public int NumMemoryChunks => MemoryChunks.NumMemoryChunks;

        public MemoryChunk MemoryAtIndex(int i) => MemoryChunks.MemoryAtIndex(i);
        public long Length => MemoryChunks.Length;

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

        public void AppendChunk(MemoryChunk chunk)
        {
            MemoryChunks.AppendMemoryChunk(chunk);
        }

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
                        if (memoryChunkReference.MemoryBlockID == newSegment.MemoryBlockID && memoryChunkReference.PreTruncationLength != newSegment.PreTruncationLength)
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
            IEnumerable<MemoryChunkReference> segmentsToAdd = EnumerateMemoryChunkReferences(memoryChunkIndex, startPosition, numBytes);
            ExtendMemoryChunkReferencesList(segmentsToAdd);
            // Debug.WriteLine($"Reference to completed memory added. References are {String.Join(", ", RecycledMemoryChunkReferences)}");
        }


        /// <summary>
        /// Enumerates memory chunk references based on an initial memory chunk index (not an ID), an offset into that memory chunk, and a length.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MemoryChunkReference> EnumerateMemoryChunkReferences(int initialMemoryChunkIndex, int offset, long length)
        {
            int memoryChunkIndex = initialMemoryChunkIndex;
            long numBytesOfLengthRemaining = length;
            while (numBytesOfLengthRemaining > 0)
            {
                var memoryChunk = MemoryAtIndex(memoryChunkIndex);

                int numBytesThisChunk = memoryChunk.Length;
                int bytesToUseThisChunk = (int)Math.Min(numBytesThisChunk - offset, numBytesOfLengthRemaining);
                yield return memoryChunk.Reference.Slice(offset, bytesToUseThisChunk);

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
                int activeMemoryBlockID = GetActiveMemoryBlockID();
                ExtendMemoryChunkReferencesList(new MemoryChunkReference(activeMemoryBlockID, 0, activeMemoryPosition, NumActiveMemoryBytesAddedToRecycling, activeMemoryPosition - NumActiveMemoryBytesAddedToRecycling), true);
                NumActiveMemoryBytesAddedToRecycling = activeMemoryPosition;
            }
        }

        internal LazinatorMemory CompletePatchLazinatorMemory(int activeLength, int activeMemoryBlockID)
        {
            var byID = MemoryChunks.GetMemoryChunksByID();
            if (activeLength > 0)
            {
                var activeMemoryChunk = byID[activeMemoryBlockID];
                activeMemoryChunk.SliceInfo = new MemoryBlockSlice(0, activeLength);
                byID[activeMemoryBlockID] = activeMemoryChunk;
            }
            List<MemoryChunk> memoryChunks = new List<MemoryChunk>(); 
            long length = 0;
            for (int i = 0; i < RecycledMemoryChunkReferences.Count; i++)
            {
                MemoryChunkReference reference = RecycledMemoryChunkReferences[i];
                MemoryChunk memoryChunk = byID[reference.MemoryBlockID];
                MemoryChunk resliced = memoryChunk.DeepCopy();
                resliced.SliceInfo = new MemoryBlockSlice(reference.AdditionalOffset, reference.FinalLength);
                length += reference.FinalLength;
                memoryChunks.Add(resliced);
            }
            return new LazinatorMemory(memoryChunks, 0, 0, length);
        }

        /// <summary>
        /// Returns the version number for active memory (equal to the last version number for CompletedMemory plus one). 
        /// </summary>
        /// <returns></returns>
        internal int GetActiveMemoryBlockID()
        {
            return MemoryChunks.GetNextMemoryBlockID();
        }

        public LazinatorMemory ToLazinatorMemory() => new LazinatorMemory(MemoryChunks);


    }
}
