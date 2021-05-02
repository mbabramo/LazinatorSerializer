using FluentAssertions;
using Lazinator.Buffers;
using Lazinator.Core;
using Xunit;
using System.Buffers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

namespace LazinatorTests.Tests
{
    public class LazinatorUtilitiesTest
    {
        [Fact]
        public void MemoryPoolWorks()
        {
            const int bufferSize = 64 * 1024;
            for (int i = 0; i < 10; i++)
            { // note: If we set this to a very large number of iterations, we can confirm that there is no memory leak here. If, on the other hand, we add each rented memory to a list that stays in memory, then we quickly use all available system memory. But we will do neither for the purpose of general testing.
                IMemoryOwner<byte> rentedMemory = LazinatorUtilities.GetRentedMemory(bufferSize);
                var bytes = rentedMemory.Memory;
                bytes.Length.Should().BeGreaterOrEqualTo(bufferSize);
            }
        }

        [Fact]
        public void ExpandableBytesWorks()
        {
            ExpandableBytes e = null;
            for (int i = 0; i < 10; i++)
            {
                int bufferSize = 100;
                e = new ExpandableBytes();
                for (int j = 0; j < 10; j++)
                {
                    bufferSize *= 2;
                    e.EnsureMinBufferSize(bufferSize);
                    e.UsedBytesInCurrentBuffer = bufferSize;
                    e.Memory.Span[bufferSize - 1] = 1;
                }
            }
        }

        [Fact]
        public void LazinatorMemoryAggregationAndSlicing()
        {
            int numChunks = 5;
            int memoryPerChunk =   100; 
            // build a single combined chunk and many individual chunks -- then see if the whole and slices match
            byte[] c = new byte[memoryPerChunk * numChunks];
            LazinatorMemory m = default;
            for (int i = 0; i < numChunks; i++)
            {
                byte[] b = new byte[memoryPerChunk];
                for (int j = 0; j < memoryPerChunk; j++)
                {
                    int overallIndex = i * memoryPerChunk + j;
                    b[j] = (byte)(overallIndex % 255);
                    c[overallIndex] = b[j];
                }
                if (i == 0)
                    m = new LazinatorMemory(b);
                else 
                    m = m.WithAppendedChunk(new MemoryChunk(new ReadOnlyBytes(b), new MemoryBlockLoadingInfo(i, b.Length), new MemoryBlockSlice(0, b.Length), false));
            }

            const int numChecks = 15;
            Random r = new Random(0);
            for (int i = 0; i < numChecks; i++)
            {
                int startPosition = r.Next(0, c.Length);
                int numBytes = r.Next(0, c.Length - startPosition);
                var s = m.Slice(startPosition, numBytes);
                var consolidated = s.GetConsolidatedMemory(false).ToArray();
                var comparison = c.Skip(startPosition).Take(numBytes).ToArray();
                if (!comparison.SequenceEqual(consolidated))
                    throw new Exception("LazinatorMemory did not match");
            }
        }

        [Fact]
        public void LazinatorMemorySubranges()
        {
            // See LazinatorMemory for an explanation of how delta serialization works. This test ignores BufferWriter and makes sure that LazinatorMemory returns the correct data.

            Random r = new Random(1);
            int numMainChunks = 5;
            int bytesPerChunk = 100;
            byte[][] mainChunks = new byte[numMainChunks][];
            byte[] continuousUnderlying = new byte[numMainChunks * bytesPerChunk];
            List<ReadOnlyBytes> overallMemoryOwners = new List<ReadOnlyBytes>();
            List<MemoryChunk> overallMemoryChunks = new List<MemoryChunk>();
            int overallIndex = 0;
            // record some values (it doesn't really matter what) in mainChunks and in continuousUnderlying,
            // which contains the same bytes but arranged in one dimension
            // also create memory owners for the main chunks and references to those memory owners
            for (int i = 0; i < numMainChunks; i++)
            {
                mainChunks[i] = new byte[bytesPerChunk];
                for (int j = 0; j < bytesPerChunk; j++)
                {
                    mainChunks[i][j] = (byte) (overallIndex % 256);
                    continuousUnderlying[overallIndex++] = mainChunks[i][j];
                }
                overallMemoryOwners.Add(new ReadOnlyBytes(mainChunks[i]));
                overallMemoryChunks.Add(new MemoryChunk(overallMemoryOwners[i], new MemoryBlockLoadingInfo(i, bytesPerChunk), new MemoryBlockSlice(0, bytesPerChunk), false));
            }
            LazinatorMemory overallLazinatorMemory = new LazinatorMemory(overallMemoryChunks.ToList(), 0, 0, continuousUnderlying.Length);
            const int numRepetitions = 100;
            for (int rep = 0; rep < numRepetitions; rep++)
            {
                // Let's build a cobbled together set of references to the overall memory, cobbling together several ranges of bytes (as might appear in a LazinatorMemory after multiple versions)
                // We'll copy this byte range (which may not be continuous in the original) to referencedBytes. 
                //Debug.WriteLine($"Repetition: {rep}");
                List<byte> referencedBytes = new List<byte>();
                const int maxNumReferenceChunks = 10;
                int numReferenceChunks = r.Next(1, maxNumReferenceChunks); // CompletedMemory will always have at least one chunk
                List<MemoryChunk> memoryChunks = new List<MemoryChunk>();
                for (int i = 0; i < numReferenceChunks; i++)
                {
                    int mainChunkIndex = r.Next(0, numMainChunks);
                    int startPosition = r.Next(0, bytesPerChunk);
                    int numBytes = r.Next(0, bytesPerChunk - startPosition);
                    var overallMemoryOwner = overallMemoryOwners[mainChunkIndex];
                    var overallMemoryOwnerLoaded = new ReadOnlyBytes(overallMemoryOwner.ReadOnlyMemory);
                    memoryChunks.Add(new MemoryChunk(overallMemoryOwnerLoaded, new MemoryBlockLoadingInfo(mainChunkIndex, overallMemoryOwner.ReadOnlyMemory.Length), new MemoryBlockSlice(startPosition, numBytes), false));
                    IEnumerable<byte> bytesToAdd = overallMemoryOwners[mainChunkIndex].ReadOnlyMemory.ToArray().Skip(startPosition).Take(numBytes);
                    referencedBytes.AddRange(bytesToAdd);
                    // Debug.WriteLine($"Main chunk {mainChunkIndex} start {startPosition} numBytes {numBytes} bytes {String.Join(",", bytesToAdd)}");
                    // Debug.WriteLine($"Overall referenced bytes {String.Join(",", referencedBytes)}");
                }
                int totalBytesReferredTo = memoryChunks.Sum(x => x.LoadingInfo.PreTruncationLength);
                referencedBytes.Count().Should().Equals(totalBytesReferredTo);
                LazinatorMemory cobbledMemory = new LazinatorMemory(memoryChunks.ToList(), 0, 0, totalBytesReferredTo);

                // Now, we are going to index into this range, first just by using LINQ, and then by getting a bytes segment, which should give us a pointer into overallLazinatorMemory.
                int startingPositionWithinLazinatorMemorySubrange = r.Next(0, totalBytesReferredTo);
                int numBytesWithinLazinatorMemorySubrange = r.Next(0, totalBytesReferredTo - startingPositionWithinLazinatorMemorySubrange);
                referencedBytes = referencedBytes.Skip(startingPositionWithinLazinatorMemorySubrange).Take(numBytesWithinLazinatorMemorySubrange).ToList();
                // Debug.WriteLine($"startingPositionWithinLazinatorMemorySubrange {startingPositionWithinLazinatorMemorySubrange } numBytesWithinLazinatorMemorySubrange {numBytesWithinLazinatorMemorySubrange}");

                List<MemoryChunkReference> memoryChunkReferences = cobbledMemory.EnumerateMemoryChunkReferences(startingPositionWithinLazinatorMemorySubrange, numBytesWithinLazinatorMemorySubrange).ToList();
                memoryChunkReferences.Sum(x => x.PreTruncationLength).Should().Equals(numBytesWithinLazinatorMemorySubrange);
                List<byte> bytesFound = new List<byte>();
                foreach (var memoryChunkReference in memoryChunkReferences)
                    bytesFound.AddRange(GetMemoryAtMemoryChunkReference(cobbledMemory, memoryChunkReference).ToArray());
                bytesFound.SequenceEqual(referencedBytes).Should().BeTrue();
            }
        }

        /// <summary>
        /// Returns the Memory block of bytes corresponding to a memory chunk reference. It is required that each memory owner be a MemoryChunk.
        /// </summary>
        /// <param name="memoryChunkReference">The memory chunk reference</param>
        /// <returns></returns>
        private ReadOnlyMemory<byte> GetMemoryAtMemoryChunkReference(LazinatorMemory lazinatorMemory, MemoryChunkReference memoryChunkReference)
        {
            var memoryChunk = GetFirstMemoryChunkWithID(lazinatorMemory, memoryChunkReference.MemoryBlockID);
            memoryChunk.LoadMemory();
            var underlyingReadOnlyMemory = memoryChunk.MemoryAsLoaded.ReadOnlyMemory.Slice(memoryChunkReference.AdditionalOffset, memoryChunkReference.FinalLength);
            return underlyingReadOnlyMemory;
        }

        private MemoryChunk GetFirstMemoryChunkWithID(LazinatorMemory lazinatorMemory, int memoryBlockID)
        {
            int? index = GetFirstIndexOfMemoryBlockID(lazinatorMemory, memoryBlockID);
            if (index == null)
                return null;
            return (MemoryChunk)lazinatorMemory.MemoryAtIndex((int)index);
        }

        private int? GetFirstIndexOfMemoryBlockID(LazinatorMemory lazinatorMemory, int memoryBlockID)
        {
            if (lazinatorMemory.MemoryAtIndex(0).LoadingInfo.MemoryBlockID == memoryBlockID)
                return 0;
            if (lazinatorMemory.MultipleMemoryChunks == null)
                return null;
            var index = GetFirstIndexOfMemoryBlockID(lazinatorMemory.MultipleMemoryChunks, memoryBlockID);
            if (index == null)
                return null;
            return index;
        }

        /// <summary>
        /// Returns the first index within the memory chunk collection of the specified memory chunk ID, or null if not found. This will be one less than the index within a LazinatorMemory containing this collection.
        /// </summary>
        /// <param name="memoryBlockID"></param>
        /// <returns></returns>
        private int? GetFirstIndexOfMemoryBlockID(MemoryChunkCollection memoryChunkCollection, int memoryBlockID)
        {
            int count = memoryChunkCollection.NumMemoryChunks;
            for (int i = 0; i < count; i++)
            {
                var memoryChunk = memoryChunkCollection.MemoryAtIndex(i);
                if (memoryChunk.LoadingInfo.MemoryBlockID == memoryBlockID)
                    return i;
            }
            return null;
        }

        [Fact]
        public void UsingReturnedMemoryTriggersException()
        {
            const int bufferSize = 64 * 1024;
            IMemoryOwner<byte> rentedMemory = LazinatorUtilities.GetRentedMemory(bufferSize);
            rentedMemory.Dispose();
            Action a = () => { var m = rentedMemory.Memory.Span; m[0] = 1; };
            a.Should().Throw<ObjectDisposedException>();
        }

        [Fact]
        public void BufferWriterCanBeCreated()
        {
            const int bufferSize = 64 * 1024;
            for (int i = 0; i < 1; i++)
            { // same as above; higher iterations causes no memory leak
                BufferWriter writer = new BufferWriter(bufferSize);
                writer.ActiveMemory.CurrentBuffer.Memory.Length.Should().BeGreaterOrEqualTo(bufferSize);
            }
        }

        [Fact]
        public void CanWriteBeyondInitialBufferSize()
        {
            const int bufferSize = 1024;
            BufferWriter writer = new BufferWriter(bufferSize);
            for (int j = 0; j < 5000; j++)
                writer.Write(j);
            var written = writer.ActiveMemoryWrittenSpan;
            for (int j = 0; j < 5000; j++)
            {
                int index = j * sizeof(int);
                ReadUncompressedPrimitives.ToInt32(written, ref index).Should().Be(j);
            }
        }
    }
}