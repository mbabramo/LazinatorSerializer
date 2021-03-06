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
        public MemoryChunkReference(int memoryBlockID, long offsetForLoading, int preTruncationLength, int additionalOffset, int finalLength) : this()
        {
            MemoryBlockID = memoryBlockID;
            LoadingOffset = offsetForLoading;
            PreTruncationLength = preTruncationLength;
            AdditionalOffset = additionalOffset;
            FinalLength = finalLength;
        }

        public MemoryChunkReference(int memoryBlockID, long offsetForLoading, int preTruncationLength) : this()
        {
            MemoryBlockID = memoryBlockID;
            LoadingOffset = offsetForLoading;
            PreTruncationLength = preTruncationLength;
            AdditionalOffset = 0;
            FinalLength = preTruncationLength;
        }

        public override bool Equals(object obj)
        {
            if (obj is not MemoryChunkReference other)
                return false;
            return MemoryBlockID == other.MemoryBlockID && LoadingOffset == other.LoadingOffset && PreTruncationLength == other.PreTruncationLength && AdditionalOffset == other.AdditionalOffset && FinalLength == other.FinalLength;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MemoryBlockID, LoadingOffset, PreTruncationLength, AdditionalOffset, FinalLength);
        }

        public override string ToString()
        {
            return $"MemoryBlockID: {MemoryBlockID}; OffsetForLoading: {LoadingOffset}; LengthAsLoaded: {PreTruncationLength}; AdditionalOffset: {AdditionalOffset}; FinalLength {FinalLength}";
        }

        /// <summary>
        /// Slices the reference relative to the existing offsets. The loading offset and length remain the same, but the additional offset
        /// is incremented, and the length provided becomes the final length.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public MemoryChunkReference Slice(int offset, int length) => new MemoryChunkReference(MemoryBlockID, LoadingOffset, PreTruncationLength, AdditionalOffset + offset, length);

        /// <summary>
        /// Slices the reference relative to the existing offset. 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public MemoryChunkReference Slice(int offset) => new MemoryChunkReference(MemoryBlockID, LoadingOffset, PreTruncationLength, AdditionalOffset + offset, FinalLength - offset);

        /// <summary>
        /// Slices the reference relative to the memory as originally loaded. The original additional offset and final length are ignored.
        /// </summary>
        /// <param name="replacementAdditionalOffset"></param>
        /// <param name="finalLength"></param>
        /// <returns></returns>
        public MemoryChunkReference Resliced(int replacementAdditionalOffset, int finalLength) => new MemoryChunkReference(MemoryBlockID, LoadingOffset, PreTruncationLength, replacementAdditionalOffset, finalLength);

        public bool SameLoadingInformation(in MemoryChunkReference other) => MemoryBlockID == other.MemoryBlockID && LoadingOffset == other.LoadingOffset && PreTruncationLength == other.PreTruncationLength;

        internal MemoryChunkReference WithMemoryBlockID(int memoryBlockID)
        {
            return new MemoryChunkReference(memoryBlockID, LoadingOffset, PreTruncationLength, AdditionalOffset, FinalLength);
        }

        internal MemoryChunkReference WithLoadingOffset(long offset)
        {
            return new MemoryChunkReference(MemoryBlockID, offset, PreTruncationLength, AdditionalOffset, FinalLength);
        }

        internal MemoryChunkReference WithPreTruncationLength(int preTruncationLength)
        {
            return new MemoryChunkReference(MemoryBlockID, LoadingOffset, preTruncationLength, AdditionalOffset, FinalLength);
        }

        internal MemoryChunkReference WithAdditionalOffsetAndFinalLength(int additionalOffset, int finalLength)
        {
            return new MemoryChunkReference(MemoryBlockID, LoadingOffset, PreTruncationLength, additionalOffset, finalLength);
        }

        internal MemoryBlockLoadingInfo LoadingInfo => LoadingOffset == 0 ? new MemoryBlockLoadingInfo(MemoryBlockID, PreTruncationLength) : new MemoryBlockInsetLoadingInfo(MemoryBlockID, PreTruncationLength, LoadingOffset);

        internal MemoryBlockSlice SliceInfo => new MemoryBlockSlice(AdditionalOffset, FinalLength);
    }
}
