using System;
using static System.Buffers.Binary.BinaryPrimitives;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Buffers;
using Lazinator.Core;
using Newtonsoft.Json.Serialization;

namespace Lazinator.Buffers
{
    /// <summary>
    /// Used internally by Lazinator to write data into a binary buffer.
    /// </summary>
    public struct BufferWriter
    {
        /// <summary>
        /// Indicates whether storage should be in little Endian format. This should be true unless the intent is to use software primarily on big Endian computers, which are comparatively rarer.
        /// </summary>
        public static bool LittleEndianStorage = true;

        public override string ToString()
        {
            return ActiveMemoryString() + " " + MemorySegmentCollection?.ToStringByChunk();
        }

        private string ActiveMemoryString()
        {
            return ActiveMemory == null ? "" : String.Join(",", ActiveMemoryWrittenSpan.ToArray());
        }

        /// <summary>
        /// Bytes that have been written by the current BufferWriter. These bytes do not necessarily occur logically after the CompletedMemory bytes.
        /// When diffs are serialized, the ActiveMemory may consist of bytes that will replace bytes in CompletedMemory. The BytesSegments are then used
        /// to indicate the order of reference of bytes in ActiveMemory and bytes in CompletedMemory. However, when serializing without diffs, ActiveMemory
        /// does contain bytes that occur logically after the bytes in CompletedMemory. 
        /// </summary>
        public ExpandableBytes ActiveMemory { get; set; }

        /// <summary>
        /// Information related to writing multiple buffers.
        /// </summary>
        public MemorySegmentCollection MemorySegmentCollection { get; set; }

        private bool Patching => MemorySegmentCollection?.Patching ?? false;

        internal int NumActiveMemoryBytesReferenced => MemorySegmentCollection?.NumActiveMemoryBytesReferenced ?? 0;

        #region Construction and initialization

        public BufferWriter(int minimumSize)
        {
            if (minimumSize == 0)
                minimumSize = ExpandableBytes.DefaultMinBufferSize;
            ActiveMemory = new ExpandableBytes(minimumSize);
            ActiveMemory.UsedBytesInCurrentBuffer = 0;
            LengthsPosition = (0, 0);
            MemorySegmentCollection = null;
        }

        public BufferWriter(int minimumSize, LazinatorMemory completedMemory)
        {
            if (minimumSize == 0)
                minimumSize = ExpandableBytes.DefaultMinBufferSize;
            ActiveMemory = new ExpandableBytes(minimumSize);
            ActiveMemory.UsedBytesInCurrentBuffer = 0;
            LengthsPosition = (0, 0);
            MemorySegmentCollection = new MemorySegmentCollection(completedMemory, true);
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
                if (Patching)
                {
                    return GetLazinatorMemoryWithPatches();
                }
                ActiveMemory.UsedBytesInCurrentBuffer = ActiveMemoryPosition;
                if (MemorySegmentCollection != null)
                {
                    if (ActiveMemoryPosition == 0)
                        return MemorySegmentCollection.ToLazinatorMemory();
                    var withAppended = MemorySegmentCollection.DeepCopy();
                    withAppended.AppendMemoryChunk(new MemoryChunk(ActiveMemory.ReadOnlyBytes, new MemoryBlockLoadingInfo(MemorySegmentCollection.GetNextMemoryBlockID(), ActiveMemoryPosition), false));
                    return withAppended.ToLazinatorMemory();
                }
                return new LazinatorMemory(new MemoryChunk(ActiveMemory.ReadOnlyBytes), 0, ActiveMemoryPosition);
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
        public int ActiveMemoryPosition
        {
            get => ActiveMemory?.UsedBytesInCurrentBuffer ?? 0;
            set
            {
                EnsureMinBufferSize(value);
                ActiveMemory.UsedBytesInCurrentBuffer = value;
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
                if (MemorySegmentCollection is null)
                    return ActiveMemoryPosition;
                if (!Patching)
                {
                    return ActiveMemoryPosition + (MemorySegmentCollection?.Length ?? 0);
                }
                else
                {
                    return ActiveMemoryPosition - MemorySegmentCollection.NumActiveMemoryBytesReferenced + MemorySegmentCollection.PatchesTotalLength;
                }
            }
        }

        public (int index, int offset) IndexedMemoryPosition
        {
            get
            {
                if (MemorySegmentCollection is null)
                    return (0, ActiveMemoryPosition);
                int completedMemoryChunks = MemorySegmentCollection.NumMemorySegments;
                return (completedMemoryChunks, ActiveMemoryPosition);
            }
        }

        private (int index, int offset) LengthsPosition;

        Span<byte> ActiveSpan => ActiveMemory == null ? new Span<byte>() : ActiveMemory.CurrentBuffer.Memory.Span;

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
            if (ActiveMemoryPosition - NumActiveMemoryBytesReferenced >= options.NextBufferThreshold)
            {
                // DEBUG -- commented out code not needed?
                //if (options.SerializeDiffs)
                //    RecordLastActiveMemoryChunkReferenceIfAny();
                MoveActiveToCompletedMemory((int)(options.NextBufferThreshold * 1.2));
            }
        }

        public void InsertReferenceToCompletedMemory(int memoryChunkIndex, int startPosition, long numBytes)
        {
            MemorySegmentCollection?.InsertReferenceToCompletedMemory(memoryChunkIndex, startPosition, numBytes, ActiveMemoryPosition);
        }

        /// <summary>
        /// Extends the bytes segment list to include the portion of active memory that has not yet been referred to. 
        /// </summary>
        internal void RecordLastActiveMemoryChunkReferenceIfAny()
        {
            MemorySegmentCollection?.RecordLastActiveMemoryChunkReference(ActiveMemoryPosition);
        }

        /// <summary>
        /// Moves the active memory to completed memory
        /// </summary>
        /// <param name="minSizeofNewBuffer"></param>
        public void MoveActiveToCompletedMemory(int minSizeofNewBuffer = ExpandableBytes.DefaultMinBufferSize)
        {
            if (ActiveMemoryPosition > 0)
            {
                var chunk = new MemoryChunk(ActiveMemory.ReadWriteBytes, new MemoryBlockLoadingInfo(MemorySegmentCollection?.GetNextMemoryBlockID() ?? new MemoryBlockID(0), ActiveMemoryPosition), false);
                if (MemorySegmentCollection == null)
                    MemorySegmentCollection = new MemorySegmentCollection(chunk, false);
                else
                    MemorySegmentCollection.AppendMemoryChunk(chunk);
                ActiveMemory = new ExpandableBytes(minSizeofNewBuffer);
                ActiveMemoryPosition = 0;
                MemorySegmentCollection.NumActiveMemoryBytesReferenced = 0;
            }
        }

        private LazinatorMemory GetLazinatorMemoryWithPatches()
        {
            if (MemorySegmentCollection is null)
                throw new Exception("No LazinatorMemory to patch");
            MemorySegmentCollection.RecordLastActiveMemoryChunkReference(ActiveMemoryPosition);
            MemoryBlockID activeMemoryBlockID = MemorySegmentCollection.GetNextMemoryBlockID();
            int activeLength = NumActiveMemoryBytesReferenced;
            if (activeLength > 0)
            {
                MoveActiveToCompletedMemory();
            }
            return MemorySegmentCollection.ToLazinatorMemory();
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
        /// 
        private Span<byte> LengthsSpan
        {
            get
            {
                if (MemorySegmentCollection == null || LengthsPosition.index == (MemorySegmentCollection?.NumMemorySegments ?? 0))
                    return ActiveSpan.Slice(LengthsPosition.offset);
                return MemorySegmentCollection.MemorySegmentAtIndex(LengthsPosition.index).Memory.Slice(LengthsPosition.offset).Span;
            }
        }

        /// <summary>
        /// Designates the current active memory position as the position at which to store length information.
        /// If there are multiple child objects, the lengths will be stored consecutively.
        /// </summary>
        /// <param name="bytesToReserve">The number of bytes to reserve</param>
        public (int index, int offset) SetLengthsPosition(int bytesToReserve)
        {
            (int index, int offset) previousPosition = LengthsPosition;
            LengthsPosition = IndexedMemoryPosition;
            Skip(bytesToReserve);
            return previousPosition;
        }

        /// <summary>
        /// Resets the lengths position to the previous position. This is called after writing the child properties,
        /// since that would result in changing the LengthsPosition to the appropriate value for the child.
        /// </summary>
        /// <param name="previousPosition"></param>
        public void ResetLengthsPosition((int index, int offset) indexedPosition)
        {
            LengthsPosition = indexedPosition;
        }

        public void RecordLength(byte length)
        {
            LengthsSpan[0] = length;
            LengthsPosition = (LengthsPosition.index, LengthsPosition.offset + 1);
            WriteTrace();
        }

        public void RecordLength(Int16 length)
        {
            if (BufferWriter.LittleEndianStorage)
                WriteInt16LittleEndian(LengthsSpan, length);
            else
                WriteInt16BigEndian(LengthsSpan, length);
            WriteTrace();
            LengthsPosition = (LengthsPosition.index, LengthsPosition.offset + sizeof(Int16));
        }

        public void RecordLength(int length)
        {
            if (BufferWriter.LittleEndianStorage)
                WriteInt32LittleEndian(LengthsSpan, length);
            else
                WriteInt32BigEndian(LengthsSpan, length);
            WriteTrace();
            LengthsPosition = (LengthsPosition.index, LengthsPosition.offset + sizeof(int));
        }
        public void RecordLength(Int64 length)
        {
            if (BufferWriter.LittleEndianStorage)
                WriteInt64LittleEndian(LengthsSpan, length);
            else
                WriteInt64BigEndian(LengthsSpan, length);
            WriteTrace();
            LengthsPosition = (LengthsPosition.index, LengthsPosition.offset + sizeof(Int64));
        }

        #endregion

        #region Tracewriting

#if TRACEWRITING

        // TRACEWRITING can be defined to allow visibility into each step of a buffer change.

        // Note that TRACEWRITING may be defined in the Lazinator project file as follows:
        //<PropertyGroup Condition = "'$(Configuration)|$(Platform)'=='Debug|AnyCPU'" >
        //  <AllowUnsafeBlocks> false </AllowUnsafeBlocks >
        //  <DefineConstants>$(DefineConstants);TRACEWRITING</DefineConstants>
        //</PropertyGroup>

        public static int _TraceWritingStep;
        public static int TraceWritingStep
        {
            get => _TraceWritingStep;
            set => _TraceWritingStep = value;
        }

        static string PreviousString = "";

        public void WriteTrace()
        {
            string s = ToString().Trim();

            string highlighted = GetHighlightedDifference(PreviousString, s);

            PreviousString = s;

            int stepNum = TraceWritingStep;

            Debug.WriteLine($"Step {stepNum:D3}: {highlighted}");

            TraceWritingStep++;
        }

        public static string GetHighlightedDifference(string s1, string s2)
        {
            if (s1 == null || s2.StartsWith(s1))
                return s2; // no highlighting needed

            int startIndex = -1;
            int endIndex = -1;

            for (int i = 0; i < s2.Length; i++)
            {
                if (i >= s1.Length || s1[i] != s2[i])
                {
                    if (startIndex == -1)
                    {
                        startIndex = i;
                        endIndex = i;
                    }
                    else
                    {
                        endIndex = i;
                    }
                }
            }

            string highlightedDifference = "";

            for (int i = 0; i < s2.Length; i++)
            {
                if (i == startIndex)
                {
                    highlightedDifference += "***";
                }

                highlightedDifference += s2[i];

                if (i == endIndex)
                {
                    highlightedDifference += "***";
                }
            }

            return highlightedDifference;
        }

        private static void PrintHighlightedDifference_Console(string s1, string s2)
        {
            for (int i = 0; i < s2.Length; i++)
            {
                if (i >= s1.Length || s1[i] != s2[i])
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(s2[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(s2[i]);
                    Console.ResetColor();
                }
            }
        }
#endif
    #endregion
    }
}
