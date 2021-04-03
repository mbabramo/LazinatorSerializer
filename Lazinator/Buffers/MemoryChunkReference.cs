﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    /// <summary>
    /// A reference to a range of bytes, identified by a memory chunk number, the index of the first byte within that memory chunk, and the number of bytes.
    /// </summary>
    public partial struct MemoryChunkReference : IMemoryChunkReference
    {

        public MemoryChunkReference(int memoryChunkID, int offsetForLoading, int lengthAsLoaded, int additionalOffset, int finalLength) : this()
        {
            MemoryChunkID = memoryChunkID;
            OffsetForLoading = offsetForLoading;
            LengthAsLoaded = lengthAsLoaded;
            AdditionalOffset = additionalOffset;
            FinalLength = finalLength;
        }

        public MemoryChunkReference(int memoryChunkID, int offsetForLoading, int lengthAsLoaded) : this()
        {
            debug; // check all references to this
            MemoryChunkID = memoryChunkID;
            OffsetForLoading = offsetForLoading;
            LengthAsLoaded = lengthAsLoaded;
            AdditionalOffset = 0;
            FinalLength = lengthAsLoaded;
        }

        public override string ToString()
        {
            return $"MemoryChunkID: {MemoryChunkID}; OffsetForLoading: {OffsetForLoading}; LengthAsLoaded: {LengthAsLoaded}; AdditionalOffset: {AdditionalOffset}; FinalLength {FinalLength}";
        }

        /// <summary>
        /// Slices the reference relative to the existing offsets. The loading offset and length remain the same, but the additional offset
        /// is incremented, and the length provided becomes the final length.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public MemoryChunkReference Slice(int offset, int length) => new MemoryChunkReference(MemoryChunkID, OffsetForLoading, LengthAsLoaded, AdditionalOffset + offset, length);

        /// <summary>
        /// Slices the reference relative to the memory as originally loaded. The original additional offset and final length are ignored.
        /// </summary>
        /// <param name="replacementAdditionalOffset"></param>
        /// <param name="finalLength"></param>
        /// <returns></returns>
        public MemoryChunkReference Resliced(int replacementAdditionalOffset, int finalLength) => new MemoryChunkReference(MemoryChunkID, OffsetForLoading, LengthAsLoaded, replacementAdditionalOffset, finalLength);

        public bool SameLoadingInformation(in MemoryChunkReference other) => MemoryChunkID == other.MemoryChunkID && OffsetForLoading == other.OffsetForLoading && LengthAsLoaded == other.LengthAsLoaded;

        /// <summary>
        /// Extends a memory chunk references list by adding a new reference. If the new reference is contiguous to the last existing reference,
        /// then the list size remains constant. 
        /// </summary>
        /// <param name="memoryChunkReferences"></param>
        /// <param name="newSegment"></param>
        public static void ExtendMemoryChunkReferencesList(List<MemoryChunkReference> memoryChunkReferences, MemoryChunkReference newSegment)
        {
            if (memoryChunkReferences.Any())
            {
                MemoryChunkReference last = memoryChunkReferences.Last();
                if (last.SameLoadingInformation(newSegment) && newSegment.AdditionalOffset == last.AdditionalOffset + last.FinalLength)
                {
                    last.FinalLength += newSegment.FinalLength;
                    return;
                }
            }
            memoryChunkReferences.Add(newSegment);
        }

        /// <summary>
        /// Extends a memory chunk references list by adding new segments. The list is consolidated to avoid having consecutive entries for contiguous ranges.
        /// </summary>
        /// <param name="memoryChunkReferences"></param>
        /// <param name="newSegments"></param>
        public static void ExtendMemoryChunkReferencesList(List<MemoryChunkReference> memoryChunkReferences, IEnumerable<MemoryChunkReference> newSegments)
        {
            foreach (var newSegment in newSegments)
                ExtendMemoryChunkReferencesList(memoryChunkReferences, newSegment);
        }
    }
}
