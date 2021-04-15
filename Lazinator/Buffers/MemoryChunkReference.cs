using System;
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
        public MemoryChunkReference(int memoryChunkID, long offsetForLoading, int lengthAsLoaded, int additionalOffset, int finalLength) : this()
        {
            MemoryChunkID = memoryChunkID;
            OffsetForLoading = offsetForLoading;
            PreTruncationLength = lengthAsLoaded;
            AdditionalOffset = additionalOffset;
            FinalLength = finalLength;
        }

        public MemoryChunkReference(int memoryChunkID, long offsetForLoading, int lengthAsLoaded) : this()
        {
            MemoryChunkID = memoryChunkID;
            OffsetForLoading = offsetForLoading;
            PreTruncationLength = lengthAsLoaded;
            AdditionalOffset = 0;
            FinalLength = lengthAsLoaded;
        }

        public override bool Equals(object obj)
        {
            if (obj is not MemoryChunkReference other)
                return false;
            return MemoryChunkID == other.MemoryChunkID && OffsetForLoading == other.OffsetForLoading && PreTruncationLength == other.PreTruncationLength && AdditionalOffset == other.AdditionalOffset && FinalLength == other.FinalLength;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MemoryChunkID, OffsetForLoading, PreTruncationLength, AdditionalOffset, FinalLength);
        }

        public override string ToString()
        {
            return $"MemoryChunkID: {MemoryChunkID}; OffsetForLoading: {OffsetForLoading}; LengthAsLoaded: {PreTruncationLength}; AdditionalOffset: {AdditionalOffset}; FinalLength {FinalLength}";
        }

        /// <summary>
        /// Slices the reference relative to the existing offsets. The loading offset and length remain the same, but the additional offset
        /// is incremented, and the length provided becomes the final length.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public MemoryChunkReference Slice(int offset, int length) => new MemoryChunkReference(MemoryChunkID, OffsetForLoading, PreTruncationLength, AdditionalOffset + offset, length);

        /// <summary>
        /// Slices the reference relative to the existing offset. 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public MemoryChunkReference Slice(int offset) => new MemoryChunkReference(MemoryChunkID, OffsetForLoading, PreTruncationLength, AdditionalOffset + offset, FinalLength - offset);

        /// <summary>
        /// Slices the reference relative to the memory as originally loaded. The original additional offset and final length are ignored.
        /// </summary>
        /// <param name="replacementAdditionalOffset"></param>
        /// <param name="finalLength"></param>
        /// <returns></returns>
        public MemoryChunkReference Resliced(int replacementAdditionalOffset, int finalLength) => new MemoryChunkReference(MemoryChunkID, OffsetForLoading, PreTruncationLength, replacementAdditionalOffset, finalLength);

        public bool SameLoadingInformation(in MemoryChunkReference other) => MemoryChunkID == other.MemoryChunkID && OffsetForLoading == other.OffsetForLoading && PreTruncationLength == other.PreTruncationLength;

        internal MemoryChunkReference WithMemoryChunkID(int memoryChunkID)
        {
            return new MemoryChunkReference(memoryChunkID, OffsetForLoading, PreTruncationLength, AdditionalOffset, FinalLength);
        }

        internal MemoryChunkReference WithLoadingOffset(long offset)
        {
            return new MemoryChunkReference(MemoryChunkID, offset, PreTruncationLength, AdditionalOffset, FinalLength);
        }

        internal MemoryChunkReference WithPreTruncationLength(int preTruncationLength)
        {
            return new MemoryChunkReference(MemoryChunkID, OffsetForLoading, preTruncationLength, AdditionalOffset, FinalLength);
        }

        internal MemoryChunkReference WithAdditionalOffsetAndFinalLength(int additionalOffset, int finalLength)
        {
            return new MemoryChunkReference(MemoryChunkID, OffsetForLoading, PreTruncationLength, additionalOffset, finalLength);
        }
    }
}
