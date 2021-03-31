using System;
using static System.Buffers.Binary.BinaryPrimitives;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Buffers;
using Lazinator.Core;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Used internally by Lazinator to write data into a binary buffer.
    /// </summary>
    public struct BinaryBufferWriter
    {
        /// <summary>
        /// Indicates whether storage should be in little Endian format. This should be true unless the intent is to use software primarily on big Endian computers, which are comparatively rarer.
        /// </summary>
        public static bool LittleEndianStorage = true;

        public override string ToString()
        {
            return ActiveMemory == null ? "" : String.Join(",", ActiveMemoryWrittenSpan.ToArray()) + " " + CompletedMemory.ToString();
        }

        /// <summary>
        /// Bytes that have been written by the current BinaryBufferWriter. These bytes do not necessarily occur logically after the CompletedMemory bytes.
        /// When diffs are serialized, the ActiveMemory may consist of bytes that will replace bytes in CompletedMemory. The BytesSegments are then used
        /// to indicate the order of reference of bytes in ActiveMemory and bytes in CompletedMemory. However, when serializing without diffs, ActiveMemory
        /// does contain bytes that occur logically after the bytes in CompletedMemory. 
        /// </summary>
        public ExpandableBytes ActiveMemory { get; set; }

        /// <summary>
        /// Bytes that were previously written. They may have been written in the same serialization pass (created when ExpandableBytes became full) or 
        /// in a previous serialization pass (when serializing diffs).
        /// </summary>
        public LazinatorMemory CompletedMemory { get; set; }

        /// <summary>
        /// When serializing diffs, these are non-null and will refer to various segments in CompletedMemory and ActiveMemory in order.
        /// </summary>
        internal List<MemoryChunkReference> RecycledMemoryChunkReferences;

        /// <summary>
        /// When serializing diffs, when a section of ActiveMemory is added to RecycledMemoryChunkReferences, this will equal the index
        /// of the last byte added plus 1. 
        /// </summary>
        private int NumActiveMemoryBytesAddedToRecycling;

        #region Construction and initialization

        public BinaryBufferWriter(int minimumSize, LazinatorMemory? completedMemory = null)
        {
            if (minimumSize == 0)
                minimumSize = ExpandableBytes.DefaultMinBufferSize;
            ActiveMemory = new ExpandableBytes(minimumSize);
            if (completedMemory == null)
            {
                CompletedMemory = default;
                RecycledMemoryChunkReferences = null;
            }
            else
            {
                RecycledMemoryChunkReferences = new List<MemoryChunkReference>();
                CompletedMemory = completedMemory.Value;
            }
            _ActiveMemoryPosition = 0;
            LengthsPosition = 0;
            NumActiveMemoryBytesAddedToRecycling = 0;
        }

        private void InitializeIfNecessary()
        {
            if (ActiveMemory == null)
                ActiveMemory = new ExpandableBytes();
        }

        #endregion

        #region LazinatorMemory access

        /// <summary>
        /// Creates LazinatorMemory equal to the underlying memory through the current position.
        /// </summary>
        public LazinatorMemory LazinatorMemory
        {
            get
            {
                InitializeIfNecessary();
                if (RecycledMemoryChunkReferences is not null)
                {
                    return PatchLazinatorMemoryFromRecycled();
                }
                if (!CompletedMemory.IsEmpty)
                {
                    if (ActiveMemoryPosition == 0)
                        return CompletedMemory;
                    Debug.WriteLine($"Appending {ActiveMemoryPosition} bytes to {CompletedMemory}"); // DEBUG
                    return CompletedMemory.WithAppendedChunk(new MemoryChunk(ActiveMemory, new MemoryChunkReference(GetActiveMemoryChunkID(), 0, ActiveMemoryPosition)));
                }
                return new LazinatorMemory(ActiveMemory, 0, ActiveMemoryPosition);
            }
        }

        /// <summary>
        /// Returns the memory in the buffer beginning at the specified position.
        /// </summary>
        /// <param name="position">The start position</param>
        /// <returns></returns>
        public LazinatorMemory Slice(long position) => LazinatorMemory.Slice(position);


        /// <summary>
        /// Returns the memory in the buffer beginning at the specified position and with the specified length
        /// </summary>
        /// <param name="position">The start position</param>
        /// <param name="position">The number of bytes to include</param>
        /// <returns></returns>
        public LazinatorMemory Slice(long position, long length) => LazinatorMemory.Slice(position, length);

        #endregion

        #region Active and completed memory


        /// <summary>
        /// The position within the active memory buffer. This is changed by the client after writing to the buffer.
        /// </summary>
        private int _ActiveMemoryPosition;
        public int ActiveMemoryPosition
        {
            get => _ActiveMemoryPosition;
            set
            {
                EnsureMinBufferSize(value);
                _ActiveMemoryPosition = value;
            }
        }

        /// <summary>
        /// Skips ahead the specified number of bytes
        /// </summary>
        /// <param name="length"></param>
        public void Skip(int length)
        {
            ActiveMemoryPosition += length;
        }

        public long OverallMemoryPosition
        {
            get
            {
                if (RecycledMemoryChunkReferences is null)
                {
                    return ActiveMemoryPosition + (CompletedMemory.IsEmpty ? 0 : CompletedMemory.Length);
                }
                else
                {
                    return ActiveMemoryPosition - NumActiveMemoryBytesAddedToRecycling + RecycledMemoryChunkReferences.Sum(x => x.Length);
                }
            }
        }

        /// <summary>
        /// An earlier position in the buffer, to which we can write information on the lengths that we are writing later in the buffer.
        /// </summary>
        private long LengthsPosition;

        Span<byte> ActiveSpan => ActiveMemory == null ? new Span<byte>() : ActiveMemory.Memory.Span;

        /// <summary>
        /// Free bytes that have not been written to. The client can attempt to write to these bytes directly, calling EnsureMinBufferSize if the operation fails and trying again. Then, the client must update the position.
        /// </summary>
        public Span<byte> FreeSpan => ActiveSpan.Slice(ActiveMemoryPosition);

        /// <summary>
        /// The bytes written through the current position. Note that the client can change the position within the buffer.
        /// </summary>
        public Span<byte> ActiveMemoryWrittenSpan => ActiveSpan.Slice(0, ActiveMemoryPosition);


        /// <summary>
        /// The bytes written through the current position. Note that the client can change the position within the buffer.
        /// </summary>
        public Memory<byte> ActiveMemoryWritten => ActiveMemory.Memory.Slice(0, ActiveMemoryPosition);

        /// <summary>
        /// Ensures that the active memory is at least a specified size, copying the current memory if needed.
        /// </summary>
        /// <param name="desiredBufferSize"></param>
        public void EnsureMinBufferSize(int desiredBufferSize = 0)
        {
            InitializeIfNecessary();
            ActiveMemory.EnsureMinBufferSize(desiredBufferSize);
        }

        /// <summary>
        /// Ensures that the active memory contains at least the specified free size.
        /// </summary>
        /// <param name="desiredFreeSize"></param>
        public void EnsureMinFreeSize(int desiredFreeSize)
        {
            if (FreeSpan.Length < desiredFreeSize)
                EnsureMinBufferSize(ActiveMemoryPosition + desiredFreeSize);
        }

        /// <summary>
        /// Returns a span containing the free bytes of the active memory span. 
        /// </summary>
        /// <param name="desiredSize"></param>
        /// <returns></returns>
        public Span<byte> GetFreeBytes(int desiredSize)
        {
            EnsureMinFreeSize(desiredSize);
            return FreeSpan.Slice(0, desiredSize);
        }

        #endregion

        #region Multiple buffers management

        public void ConsiderSwitchToNextBuffer(ref LazinatorSerializationOptions options)
        {
            if (ActiveMemoryPosition - NumActiveMemoryBytesAddedToRecycling >= options.NextBufferThreshold)
            {
                if (options.SerializeDiffs)
                    RecordLastActiveMemoryChunkReference();
                MoveActiveToCompletedMemory((int)(options.NextBufferThreshold * 1.2));
            }
        }

        /// <summary>
        /// Moves the active memory to completed memory
        /// </summary>
        /// <param name="minSizeofNewBuffer"></param>
        public void MoveActiveToCompletedMemory(int minSizeofNewBuffer = ExpandableBytes.DefaultMinBufferSize)
        {
            if (ActiveMemoryPosition > 0)
            {
                CompletedMemory = CompletedMemory.WithAppendedChunk(new MemoryChunk(ActiveMemory, new MemoryChunkReference(GetActiveMemoryChunkID(), 0, ActiveMemoryPosition)));
                ActiveMemory = new ExpandableBytes(minSizeofNewBuffer);
                ActiveMemoryPosition = 0;
                NumActiveMemoryBytesAddedToRecycling = 0;
                Debug.WriteLine($"Active memory moved to completed memory. Completed now: {CompletedMemory}"); // DEBUG
            }
        }

        /// <summary>
        /// Writes from CompletedMemory. Instead of copying the bytes, it simply adds a BytesSegment reference to where those bytes are in the overall sets of bytes.
        /// </summary>
        /// <param name="memoryChunkIndex">The index of the memory chunk</param>
        /// <param name="startPosition">The position of the first byte of the memory within the indexed memory chunk</param>
        /// <param name="numBytes"></param>
        public void InsertReferenceToCompletedMemory(int memoryChunkIndex, int startPosition, long numBytes)
        {
            RecordLastActiveMemoryChunkReference();
            IEnumerable<MemoryChunkReference> segmentsToAdd = CompletedMemory.EnumerateMemoryChunkReferences(memoryChunkIndex, startPosition, numBytes).ToList(); // DEBUG -- remove ToList()
            MemoryChunkReference.ExtendMemoryChunkReferencesList(RecycledMemoryChunkReferences, segmentsToAdd);
            Debug.WriteLine($"Reference to completed memory added. Last reference is {RecycledMemoryChunkReferences.Last()}"); // DEBUG
        }

        /// <summary>
        /// Extends the bytes segment list to include the portion of active memory that is not included in active memory. 
        /// </summary>
        internal void RecordLastActiveMemoryChunkReference()
        {
            int activeMemoryChunkID = GetActiveMemoryChunkID();
            if (ActiveMemoryPosition > NumActiveMemoryBytesAddedToRecycling)
            {
                MemoryChunkReference.ExtendMemoryChunkReferencesList(RecycledMemoryChunkReferences, new MemoryChunkReference(activeMemoryChunkID, NumActiveMemoryBytesAddedToRecycling, ActiveMemoryPosition - NumActiveMemoryBytesAddedToRecycling));
                NumActiveMemoryBytesAddedToRecycling = ActiveMemoryPosition;
            }
        }

        internal LazinatorMemory PatchLazinatorMemoryFromRecycled()
        {
            if (RecycledMemoryChunkReferences == null || !RecycledMemoryChunkReferences.Any())
                return LazinatorMemory.EmptyLazinatorMemory;
            RecordLastActiveMemoryChunkReference();
            MoveActiveToCompletedMemory();
            var byID = CompletedMemory.GetMemoryChunksByID();
            MemoryChunk initialMemoryChunk = null;
            List<MemoryChunk> moreMemory = null;
            long length = 0;
            for (int i = 0; i < RecycledMemoryChunkReferences.Count; i++)
            {
                if (i == 1)
                    moreMemory = new List<MemoryChunk>();
                MemoryChunkReference reference = RecycledMemoryChunkReferences[i];
                length += reference.Length;
                MemoryChunk memoryChunk = byID[reference.MemoryChunkID];
                MemoryChunk resliced = memoryChunk.SliceReferencedMemory(reference.Offset, reference.Length);
                if (i == 0)
                    initialMemoryChunk = resliced;
                else
                    moreMemory.Add(resliced);
            }
            return new LazinatorMemory(initialMemoryChunk, moreMemory, 0, 0, length);
        }

        /// <summary>
        /// Returns the Span beginning at position LengthsPosition, when recycled memory chunk references are being recorded.
        /// </summary>
        /// <returns></returns>
        internal Span<byte> GetLengthsSpanWithinRecycled()
        {
            MemoryChunkReference? lengthsSpanMemoryChunkReference = null;
            long lengthPositionRemaining = LengthsPosition;
            if (RecycledMemoryChunkReferences.Any())
            {
                int i = 0;
                int numRecycledMemoryChunkReferencesCount = RecycledMemoryChunkReferences.Count;
                while (lengthPositionRemaining > 0 && numRecycledMemoryChunkReferencesCount > i)
                {
                    MemoryChunkReference reference = RecycledMemoryChunkReferences[i];
                    if (lengthPositionRemaining < reference.Length)
                    {
                        lengthsSpanMemoryChunkReference = new MemoryChunkReference(reference.MemoryChunkID, (int)(reference.Offset + lengthPositionRemaining), (int)(reference.Length - lengthPositionRemaining));
                        lengthPositionRemaining = 0;
                    }
                    else
                        lengthPositionRemaining -= reference.Length;
                    i++;
                }
            }
            if (lengthsSpanMemoryChunkReference == null)
            {
                // We've exhausted the recycled memory chunk references. So, it must be within the non-recycled portion of active memory.
                return ActiveMemory.Memory.Slice((int)(NumActiveMemoryBytesAddedToRecycling + lengthPositionRemaining)).Span;
            }
            else
            {
                // We need to find the MemoryChunkID.
                int memoryChunkID = lengthsSpanMemoryChunkReference.Value.MemoryChunkID;
                if (GetActiveMemoryChunkID() == memoryChunkID)
                {
                    return ActiveMemoryWritten.Slice(lengthsSpanMemoryChunkReference.Value.Offset).Span;
                }
                else
                {
                    IMemoryOwner<byte> memoryOwner = CompletedMemory.GetMemoryChunkWithID(memoryChunkID);
                    return memoryOwner.Memory.Slice(lengthsSpanMemoryChunkReference.Value.Offset).Span;
                }
            }
        }

        /// <summary>
        /// Returns the version number for active memory (equal to the last version number for CompletedMemory plus one). 
        /// </summary>
        /// <returns></returns>
        private int GetActiveMemoryChunkID()
        {
            return CompletedMemory.GetNextMemoryChunkID();
        }

        #endregion

        #region Writing to active buffer

        public void Write(bool value)
        {
            WriteEnlargingIfNecessary(ref value);
            ActiveMemoryPosition += sizeof(byte);
        }

        public void Write(byte value)
        {
            if (ActiveSpan.Length > ActiveMemoryPosition)
                ActiveSpan[ActiveMemoryPosition++] = value;
            else
            {
                WriteEnlargingIfNecessary(ref value);
                ActiveMemoryPosition++;
            }
        }

        public void Write(Span<byte> value)
        {
            int originalPosition = ActiveMemoryPosition;
            if (originalPosition + value.Length > ActiveSpan.Length)
                EnsureMinBufferSize((originalPosition + value.Length) * 2);
            value.CopyTo(FreeSpan);
            ActiveMemoryPosition += value.Length;
        }

        public void Write(sbyte value)
        {
            WriteEnlargingIfNecessary(ref value);
            ActiveMemoryPosition += sizeof(sbyte);
        }

        public void Write(char value)
        {
            WriteEnlargingIfNecessary(ref value);
            ActiveMemoryPosition += sizeof(char);
        }

        public void Write(float value)
        {
            Write(BitConverter.SingleToInt32Bits(value));
        }

        public void Write(double value)
        {
            Write(BitConverter.DoubleToInt64Bits(value));
        }

        public void Write(decimal value)
        {
            // Note: Decimal.GetBits allocates an array in the heap. To avoid this, we use our CompressedDecimal class.
            CompressedDecimal.DecomposableDecimal cd = new CompressedDecimal.DecomposableDecimal(value);
            Write(cd.DecomposedDecimal.lo);
            Write(cd.DecomposedDecimal.mid);
            Write(cd.DecomposedDecimal.hi);
            WriteAlwaysLittleEndian(cd.DecomposedDecimal.flags); // we always write this in little endian since our read method looks specifically at bits, assuming little endianness
            // The above is equivalent to the following:
            //int[] integerComponents = Decimal.GetBits(value);
            //foreach (var integerComponent in integerComponents)
            //    Write(integerComponent); // advances EndPosition
        }

        public void Write(Guid value)
        {
            bool success = false;
            while (!success)
            {
                success = value.TryWriteBytes(FreeSpan);
                if (!success)
                    EnsureMinBufferSize();
            }
            ActiveMemoryPosition += 16; // trywritebytes always writes exactly 16 bytes even though sizeof(Guid) is not defined
        }

        private void WriteEnlargingIfNecessary<T>(ref T value) where T : struct
        {
            InitializeIfNecessary();
            bool success = false;
            while (!success)
            {
                success = MemoryMarshal.TryWrite<T>(FreeSpan, ref value);
                if (!success)
                    EnsureMinBufferSize();
            }
        }

        public void Write(ReadOnlySpan<byte> value)
        {
            bool success = false;
            while (!success)
            {
                success = value.TryCopyTo(FreeSpan);
                if (!success)
                    EnsureMinBufferSize();
            }
            ActiveMemoryPosition += value.Length;
        }

        public void Write(short value)
        {
            bool success = false;
            while (!success)
            {
                success = LittleEndianStorage ? TryWriteInt16LittleEndian(FreeSpan, value) : TryWriteInt16BigEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            ActiveMemoryPosition += sizeof(short);
        }

        public void Write(ushort value)
        {
            bool success = false;
            while (!success)
            {
                success = LittleEndianStorage ? TryWriteUInt16LittleEndian(FreeSpan, value) : TryWriteUInt16BigEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            ActiveMemoryPosition += sizeof(ushort);
        }

        public void Write(int value)
        {
            bool success = false;
            while (!success)
            {
                success = LittleEndianStorage ? TryWriteInt32LittleEndian(FreeSpan, value) : TryWriteInt32BigEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            ActiveMemoryPosition += sizeof(int);
        }

        public void WriteAlwaysLittleEndian(int value)
        {
            bool success = false;
            while (!success)
            {
                success = TryWriteInt32LittleEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            ActiveMemoryPosition += sizeof(int);
        }

        public void Write(uint value)
        {
            bool success = false;
            while (!success)
            {
                success = LittleEndianStorage ? TryWriteUInt32LittleEndian(FreeSpan, value) : TryWriteUInt32BigEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            ActiveMemoryPosition += sizeof(uint);
        }

        public void Write(long value)
        {
            bool success = false;
            while (!success)
            {
                success = LittleEndianStorage ? TryWriteInt64LittleEndian(FreeSpan, value) : TryWriteInt64BigEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            ActiveMemoryPosition += sizeof(long);
        }

        public void Write(ulong value)
        {
            bool success = false;
            while (!success)
            {
                success = LittleEndianStorage ? TryWriteUInt64LittleEndian(FreeSpan, value) : TryWriteUInt64BigEndian(FreeSpan, value);
                if (!success)
                    EnsureMinBufferSize();
            }
            ActiveMemoryPosition += sizeof(ulong);
        }

        #endregion

        #region Recording lengths

        /// <summary>
        /// A span containing space reserved to write length values of what is written later in the buffer.
        /// </summary>
        private Span<byte> LengthsSpan
        {
            get
            {
                if (RecycledMemoryChunkReferences != null)
                    return GetLengthsSpanWithinRecycled();
                if (CompletedMemory.IsEmpty)
                    return ActiveSpan.Slice((int)LengthsPosition);
                if (LengthsPosition >= CompletedMemory.Length)
                    return ActiveSpan.Slice((int)(LengthsPosition - CompletedMemory.Length));
                return CompletedMemory.Slice(LengthsPosition).InitialMemory.Span;
            }
        }

        /// <summary>
        /// Designates the current active memory position as the position at which to store length information. 
        /// </summary>
        /// <param name="bytesToReserve">The number of bytes to reserve</param>
        public long SetLengthsPosition(int bytesToReserve)
        {
            long previousPosition = LengthsPosition;
            LengthsPosition = OverallMemoryPosition;
            Skip(bytesToReserve);
            return previousPosition;
        }

        /// <summary>
        /// Resets the lengths position to the previous position.
        /// </summary>
        /// <param name="previousPosition"></param>
        public void ResetLengthsPosition(long previousPosition)
        {
            LengthsPosition = previousPosition;
        }

        public void RecordLength(byte length)
        {
            LengthsSpan[0] = length;
            LengthsPosition++;
        }

        public void RecordLength(Int16 length)
        {
            if (BinaryBufferWriter.LittleEndianStorage)
                WriteInt16LittleEndian(LengthsSpan, length);
            else
                WriteInt16BigEndian(LengthsSpan, length);
            LengthsPosition += sizeof(Int16);
        }

        public void RecordLength(int length)
        {
            if (BinaryBufferWriter.LittleEndianStorage)
                WriteInt32LittleEndian(LengthsSpan, length);
            else
                WriteInt32BigEndian(LengthsSpan, length);
            LengthsPosition += sizeof(int);
        }
        public void RecordLength(Int64 length)
        {
            if (BinaryBufferWriter.LittleEndianStorage)
                WriteInt64LittleEndian(LengthsSpan, length);
            else
                WriteInt64BigEndian(LengthsSpan, length);
            LengthsPosition += sizeof(Int64);
        }

        #endregion
    }
}
