using System;
using static System.Buffers.Binary.BinaryPrimitives;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Buffers;
using Lazinator.Core;
using Newtonsoft.Json.Serialization;
using System.Runtime.CompilerServices;
using System.Text;
using Lazinator.Support;
using Newtonsoft.Json.Linq;

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
            return ActiveMemoryString() + " >>> " + MemoryRangeCollection?.ToStringByBlock();
        }

        private string ActiveMemoryString()
        {
            return ActiveMemory == null ? "" : String.Join(",", ActiveMemoryWrittenSpan.ToArray().Select(x => x.ToString().PadLeft(3, '0')));
        }

        /// <summary>
        /// Bytes that have been written by the current BufferWriter. These bytes do not necessarily occur logically after the CompletedMemory bytes.
        /// When diffs are serialized, the ActiveMemory may consist of bytes that will replace bytes in CompletedMemory. The BytesRanges are then used
        /// to indicate the order of reference of bytes in ActiveMemory and bytes in CompletedMemory. However, when serializing without diffs, ActiveMemory
        /// does contain bytes that occur logically after the bytes in CompletedMemory. 
        /// </summary>
        public ExpandableBytes ActiveMemory { get; set; }

        /// <summary>
        /// When storing multiple buffers, this is used to store them.
        /// </summary>
        public MemoryRangeCollection MemoryRangeCollection { get; set; }

        /// <summary>
        /// When a BufferWriter is based on a previous version, it will be stored here,
        /// so that we can begin writing on a clean slate, while still being able
        /// to refer to completed memory.
        /// </summary>
        public MemoryBlockCollection PreviousVersion { get; set; }

        private bool Patching => MemoryRangeCollection?.Patching ?? false;

        internal int NumActiveMemoryBytesReferenced => MemoryRangeCollection?.NumActiveMemoryBytesReferenced ?? 0;
        
        MemoryBlockID GetActiveMemoryBlockID() =>
            MemoryRangeCollection?.GetNextMemoryBlockID() ?? PreviousVersion?.GetNextMemoryBlockID() ?? new MemoryBlockID(0);

        #region Construction and initialization

        public BufferWriter(int minimumSize)
        {
            if (minimumSize == 0)
                minimumSize = ExpandableBytes.DefaultMinBufferSize;
            ActiveMemory = new ExpandableBytes(minimumSize);
            ActiveMemory.UsedBytesInCurrentBuffer = 0;
            _LengthsPosition = (new MemoryBlockID(0), 0);
            MemoryRangeCollection = null;
            PreviousVersion = null;
        }

        public BufferWriter(int minimumSize, LazinatorMemory previousVersion)
        {
            if (minimumSize == 0)
                minimumSize = ExpandableBytes.DefaultMinBufferSize;
            ActiveMemory = new ExpandableBytes(minimumSize);
            ActiveMemory.UsedBytesInCurrentBuffer = 0;
            if (previousVersion.MultipleMemoryBlocks == null)
            {
                if (previousVersion.SingleMemoryBlock != null)
                    PreviousVersion = new MemoryBlockCollection(new List<MemoryBlock>() { previousVersion.SingleMemoryBlock });
                else
                    PreviousVersion = default;
            }
            else
                PreviousVersion = previousVersion.MultipleMemoryBlocks.DeepCopy();
            _LengthsPosition = (PreviousVersion?.GetNextMemoryBlockID() ?? new MemoryBlockID(0), 0);
            MemoryRangeCollection = null;
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
                if (MemoryRangeCollection != null)
                {
                    if (ActiveMemoryPosition == 0)
                        return MemoryRangeCollection.ToLazinatorMemory();
                    var withAppended = MemoryRangeCollection.DeepCopy();
                    withAppended.AppendMemoryBlock(new MemoryBlock(ActiveMemory.ReadOnlyBytes, new MemoryBlockLoadingInfo(withAppended.GetNextMemoryBlockID(), ActiveMemoryPosition), false));
                    return withAppended.ToLazinatorMemory();
                }
                return new LazinatorMemory(new MemoryBlock(ActiveMemory.ReadOnlyBytes), 0, ActiveMemoryPosition);
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
                if (MemoryRangeCollection is null)
                    return ActiveMemoryPosition;
                if (!Patching)
                {
                    return ActiveMemoryPosition + (MemoryRangeCollection?.LengthOfMemoryBlocks ?? 0);
                }
                else
                {
                    return ActiveMemoryPosition - MemoryRangeCollection.NumActiveMemoryBytesReferenced + MemoryRangeCollection.PatchesTotalLength;
                }
            }
        }

        public (MemoryBlockID id, int offset) IndexedMemoryPosition
        {
            get
            {
                return (GetActiveMemoryBlockID(), ActiveMemoryPosition);
            }
        }

        private (MemoryBlockID id, int offset) _LengthsPosition;
        private (MemoryBlockID id, int offset) LengthsPosition
        {
            get => _LengthsPosition;
            set
            {
                _LengthsPosition = value;
            }
        }

        public string ToLocationString()
        {
            return $"Overall {OverallMemoryPosition} Indexed {IndexedMemoryPosition} (length {LengthsPosition})"; 
        }

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
                MoveActiveToCompletedMemory((int)(options.NextBufferThreshold * 1.2));
            }
        }

        public void InsertReferenceToPreviousVersion(int memoryRangeIndex, int startPosition, long numBytes)
        {
            if (MemoryRangeCollection == null)
            {
                MemoryRangeCollection = new MemoryRangeCollection(PreviousVersion.EnumerateMemoryBlocks().ToList(), true); // load all the memory block from previous version into the new memory range collection, and prepare to add references
                MoveActiveToCompletedMemory();
            }
            MemoryRangeCollection.InsertReferenceToPreviousVersion(PreviousVersion, memoryRangeIndex, startPosition, numBytes, ActiveMemoryPosition);
        }

        /// <summary>
        /// Extends the bytes range list to include the portion of active memory (which has not been added to completed memory yet) that has not yet been referred to. 
        /// </summary>
        internal void RecordLastActiveMemoryBlockReferenceIfAny()
        {
            MemoryRangeCollection?.RecordLastActiveMemoryBlockReference(ActiveMemoryPosition);
        }

        /// <summary>
        /// Moves the active memory to completed memory
        /// </summary>
        /// <param name="minSizeofNewBuffer"></param>
        public void MoveActiveToCompletedMemory(int minSizeofNewBuffer = ExpandableBytes.DefaultMinBufferSize)
        {
            if (ActiveMemoryPosition > 0)
            {
#if TRACING
                TabbedText.WriteLine($"Moving active memory to completed memory, block {GetActiveMemoryBlockID()} ({ActiveMemoryPosition} bytes)");
#endif

                if (GetActiveMemoryBlockID().AsInt == 6)
                {
                    var DEBUG = 0;
                }
                bool memoryBlockAlreadyIncluded = false;
                if (MemoryRangeCollection != null)
                {
                    memoryBlockAlreadyIncluded = MemoryRangeCollection.ContainsMemoryBlockID(GetActiveMemoryBlockID());
                }
                if (!memoryBlockAlreadyIncluded)
                {
                    var block = new MemoryBlock(ActiveMemory.ReadWriteBytes, new MemoryBlockLoadingInfo(GetActiveMemoryBlockID(), ActiveMemoryPosition), false);
                    if (MemoryRangeCollection == null)
                        MemoryRangeCollection = new MemoryRangeCollection(block, false);
                    else
                    {
                        MemoryRangeCollection.AppendMemoryBlock(block); // Append the memory block (and any part of the range of the block not already referred to)
                    }
                }
                ActiveMemory = new ExpandableBytes(minSizeofNewBuffer);
                ActiveMemoryPosition = 0;
                MemoryRangeCollection.NumActiveMemoryBytesReferenced = 0;
            }
        }

        private LazinatorMemory GetLazinatorMemoryWithPatches()
        {
            if (MemoryRangeCollection is null)
                throw new Exception("No LazinatorMemory to patch");
            MemoryRangeCollection.RecordLastActiveMemoryBlockReference(ActiveMemoryPosition);
            int activeLength = NumActiveMemoryBytesReferenced;
            if (activeLength > 0)
            {
                MoveActiveToCompletedMemory();
            }
            return MemoryRangeCollection.ToLazinatorMemory();
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
                if (LengthsPosition.id == GetActiveMemoryBlockID())
                    return ActiveSpan.Slice(LengthsPosition.offset);
                MemoryBlock block = MemoryRangeCollection.GetMemoryBlockByBlockID(LengthsPosition.id);
                return block.ReadWriteMemory.Slice(LengthsPosition.offset).Span;
            }
        }

        /// <summary>
        /// Designates the current active memory position as the position at which to store length information.
        /// If there are multiple child objects, the lengths will be stored consecutively.
        /// </summary>
        /// <param name="bytesToReserve">The number of bytes to reserve</param>
        public (MemoryBlockID id, int offset) SetLengthsPosition(int bytesToReserve)
        {
            (MemoryBlockID id, int offset) previousPosition = LengthsPosition;
            LengthsPosition = IndexedMemoryPosition;
#if TRACING
            TabbedText.WriteLine($"Setting lengths position to {LengthsPosition} and then skipping {bytesToReserve} reserved bytes.");
#endif
            Skip(bytesToReserve);
            return previousPosition;
        }

        public void AdvanceLengthsPosition(int bytesToAdvance)
        {
            LengthsPosition = (LengthsPosition.id, LengthsPosition.offset + bytesToAdvance);
#if TRACING
            TabbedText.WriteLine($"Advancing lengths position {bytesToAdvance} bytes to {LengthsPosition} (while writing at {IndexedMemoryPosition}).");
#endif
        }

        /// <summary>
        /// Resets the lengths position to the previous position. This is called after writing the child properties,
        /// since that would result in changing the LengthsPosition to the appropriate value for the child.
        /// </summary>
        /// <param name="previousPosition"></param>
        public void ResetLengthsPosition((MemoryBlockID id, int offset) indexedPosition)
        {
            LengthsPosition = indexedPosition;
#if TRACING
            TabbedText.WriteLine($"Returning to write length at {LengthsPosition}.");
#endif
        }

        public void RecordLength(byte length)
        {
#if TRACING
            TabbedText.WriteLine($"Setting lengths value to {length} at {LengthsPosition}. Currently writing otherwise at {IndexedMemoryPosition}");
#endif
            LengthsSpan[0] = length;
            AdvanceLengthsPosition(1);
#if TRACING_DETAILED
            WriteTrace();
#endif
        }

        public void RecordLength(Int16 length)
        {
#if TRACING
            TabbedText.WriteLine($"Setting lengths value to {length} at {LengthsPosition}. Currently writing otherwise at {IndexedMemoryPosition}");
#endif
            if (BufferWriter.LittleEndianStorage)
                WriteInt16LittleEndian(LengthsSpan, length);
            else
                WriteInt16BigEndian(LengthsSpan, length);
#if TRACING_DETAILED
            WriteTrace();
#endif
            AdvanceLengthsPosition(sizeof(Int16));
        }

        public void RecordLength(int length)
        {
#if TRACING
            TabbedText.WriteLine($"Setting lengths value to {length} at {LengthsPosition}. Currently writing otherwise at {IndexedMemoryPosition}");
#endif
            if (BufferWriter.LittleEndianStorage)
                WriteInt32LittleEndian(LengthsSpan, length);
            else
                WriteInt32BigEndian(LengthsSpan, length);
#if TRACING_DETAILED
            WriteTrace();
#endif
            AdvanceLengthsPosition(sizeof(int));
        }
        public void RecordLength(Int64 length)
        {
#if TRACING
            TabbedText.WriteLine($"Setting lengths value to {length} at {LengthsPosition}. Currently writing otherwise at {IndexedMemoryPosition}");
#endif
            if (BufferWriter.LittleEndianStorage)
                WriteInt64LittleEndian(LengthsSpan, length);
            else
                WriteInt64BigEndian(LengthsSpan, length);
#if TRACING_DETAILED
            WriteTrace();
#endif

            AdvanceLengthsPosition(sizeof(Int64));
        }

#endregion

        #region Tracewriting

#if TRACING_DETAILED

        // TRACING_DETAILED can be defined to allow visibility into each step of a buffer change.

        // Note that TRACING_DETAILED may be defined in the Lazinator project file as follows:
        //<PropertyGroup Condition = "'$(Configuration)|$(Platform)'=='Debug|AnyCPU'" >
        //  <AllowUnsafeBlocks> false </AllowUnsafeBlocks >
        //  <DefineConstants>$(DefineConstants);TRACING_DETAILED</DefineConstants>
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

            TabbedText.WriteLine($"Step {stepNum:D3}: {highlighted}");

            TraceWritingStep++;
        }

        public static string GetHighlightedDifference(string s1, string s2)
        {
            if (s1 == null || s2.StartsWith(s1))
                return s2; // no highlighting needed

            StringBuilder highlightedDifference = new StringBuilder();
            highlightedDifference.AppendLine(s2);
            highlightedDifference.Append("          "); // width of "Step XXX: "

            for (int i = 0; i < s2.Length; i++)
            {
                if (s1.Length < i + 1 || s1[i] != s2[i])
                {
                    highlightedDifference.Append("*");
                }
                else
                    highlightedDifference.Append(" ");
            }

            return highlightedDifference.ToString();
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
