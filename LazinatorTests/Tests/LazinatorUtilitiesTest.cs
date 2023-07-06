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
            int numBlocks = 5;
            int memoryPerBlock =   100; 
            // build a single combined block and many individual blocks -- then see if the whole and slices match
            byte[] c = new byte[memoryPerBlock * numBlocks];
            LazinatorMemory m = default;
            for (int i = 0; i < numBlocks; i++)
            {
                byte[] b = new byte[memoryPerBlock];
                for (int j = 0; j < memoryPerBlock; j++)
                {
                    int overallIndex = i * memoryPerBlock + j;
                    b[j] = (byte)(overallIndex % 255);
                    c[overallIndex] = b[j];
                }
                if (i == 0)
                    m = new LazinatorMemory(b);
                else 
                    m = m.WithAppendedBlock(new MemoryBlock(new ReadOnlyBytes(b), new MemoryBlockLoadingInfo(new MemoryBlockID(i), b.Length), false)); 
            }

            const int numChecks = 15;
            Random r = new Random(0);
            for (int i = 0; i < numChecks; i++)
            {
                int startPosition = r.Next(0, c.Length);
                int numBytes = r.Next(0, c.Length - startPosition);
                var s = m.Slice(startPosition, numBytes);
                var consolidated = s.GetConsolidatedMemory().ToArray(); 
                var comparison = c.Skip(startPosition).Take(numBytes).ToArray();
                if (!comparison.SequenceEqual(consolidated))
                    throw new Exception("LazinatorMemory did not match");
            }
        }

        [Fact]
        public void LazinatorMemorySubranges()
        {
            Random r = new Random(1);
            int numMainBlocks = 5;
            int bytesPerBlock = 100;
            byte[][] mainBlocks = new byte[numMainBlocks][];
            byte[] continuousUnderlying = new byte[numMainBlocks * bytesPerBlock];
            List<ReadOnlyBytes> overallMemoryOwners = new List<ReadOnlyBytes>();
            List<MemoryBlock> overallMemoryBlocks = new List<MemoryBlock>();
            int overallIndex = 0;
            // record some values (it doesn't really matter what) in mainBlocks and in continuousUnderlying,
            // which contains the same bytes but arranged in one dimension
            // also create memory owners for the main blocks and references to those memory owners
            for (int i = 0; i < numMainBlocks; i++)
            {
                mainBlocks[i] = new byte[bytesPerBlock];
                for (int j = 0; j < bytesPerBlock; j++)
                {
                    mainBlocks[i][j] = (byte) (overallIndex % 256);
                    continuousUnderlying[overallIndex++] = mainBlocks[i][j];
                }
                overallMemoryOwners.Add(new ReadOnlyBytes(mainBlocks[i]));
                overallMemoryBlocks.Add(new MemoryBlock(overallMemoryOwners[i], new MemoryBlockLoadingInfo(new MemoryBlockID(i), bytesPerBlock), false));
            }
            LazinatorMemory overallLazinatorMemory = new LazinatorMemory(overallMemoryBlocks.ToList(), 0, 0, continuousUnderlying.Length);
            const int numRepetitions = 100;
            for (int rep = 0; rep < numRepetitions; rep++)
            {
                // Let's build a cobbled together set of references to the overall memory, cobbling together several ranges of bytes (as might appear in a LazinatorMemory after multiple versions)
                // We'll copy this byte range (which may not be continuous in the original) to referencedBytes. 
                //Debug.WriteLine($"Repetition: {rep}");
                List<byte> referencedBytes = new List<byte>();
                int maxNumReferenceBlocks = numMainBlocks;
                int numReferenceBlocks = r.Next(1, maxNumReferenceBlocks); // CompletedMemory will always have at least one block
                List<MemoryBlock> memoryBlocks = new List<MemoryBlock>();
                HashSet<int> blocksReferenced = new HashSet<int>();
                for (int i = 0; i < numReferenceBlocks; i++)
                {
                    int mainBlockIndex = -1;
                    while (mainBlockIndex == -1 || blocksReferenced.Contains(mainBlockIndex))
                        mainBlockIndex = r.Next(0, numMainBlocks);
                    blocksReferenced.Add(mainBlockIndex);
                    int startPosition = 0; // Note: No longer allowing the start position to vary (we can do that with segments) r.Next(0, bytesPerBlock);
                    int numBytes = r.Next(0, bytesPerBlock - startPosition);
                    var overallMemoryOwner = overallMemoryOwners[mainBlockIndex];
                    var overallMemoryOwnerLoaded = new ReadOnlyBytes(overallMemoryOwner.ReadOnlyMemory);
                    memoryBlocks.Add(new MemoryBlock(overallMemoryOwnerLoaded, new MemoryBlockLoadingInfo(new MemoryBlockID(mainBlockIndex), numBytes), false));
                    IEnumerable<byte> bytesToAdd = overallMemoryOwners[mainBlockIndex].ReadOnlyMemory.ToArray().Skip(startPosition).Take(numBytes);
                    referencedBytes.AddRange(bytesToAdd);
                    // Debug.WriteLine($"Main block {mainBlockIndex} start {startPosition} numBytes {numBytes} bytes {String.Join(",", bytesToAdd)}");
                    // Debug.WriteLine($"Overall referenced bytes {String.Join(",", referencedBytes)}");
                }
                int totalBytesReferredTo = memoryBlocks.Sum(x => x.LoadingInfo.MemoryBlockLength);
                referencedBytes.Count().Should().Be(totalBytesReferredTo);
                LazinatorMemory cobbledMemory = new LazinatorMemory(memoryBlocks.ToList(), 0, 0, totalBytesReferredTo);

                // Now, we are going to index into this range, first just by using LINQ, and then by getting a bytes segment, which should give us a pointer into overallLazinatorMemory.
                int startingPositionWithinLazinatorMemorySubrange = r.Next(0, totalBytesReferredTo);
                int numBytesWithinLazinatorMemorySubrange = r.Next(0, totalBytesReferredTo - startingPositionWithinLazinatorMemorySubrange);
                referencedBytes = referencedBytes.Skip(startingPositionWithinLazinatorMemorySubrange).Take(numBytesWithinLazinatorMemorySubrange).ToList();
                // Debug.WriteLine($"startingPositionWithinLazinatorMemorySubrange {startingPositionWithinLazinatorMemorySubrange } numBytesWithinLazinatorMemorySubrange {numBytesWithinLazinatorMemorySubrange}");

                List<MemoryRangeByBlockIndex> memorySegmentIndexAndSlices = cobbledMemory.Slice((long) startingPositionWithinLazinatorMemorySubrange, (long) numBytesWithinLazinatorMemorySubrange).EnumerateMemoryRangesByBlockIndex().ToList();
                memorySegmentIndexAndSlices.Sum(x => x.Length).Should().Be(numBytesWithinLazinatorMemorySubrange);
                List<byte> bytesFound = new List<byte>();
                foreach (var memorySegmentIndexAndSlice in memorySegmentIndexAndSlices)
                    bytesFound.AddRange(GetMemoryAtBlockAndOffset(cobbledMemory, memorySegmentIndexAndSlice).ToArray());
                bytesFound.SequenceEqual(referencedBytes).Should().BeTrue();
            }
        }

        [Fact]
        public void MemoryRangeCollectionSubranges()
        {
            // Note that the LoadingInfos should be irrelevant. The blocks consist of the memory as loaded.
            MemoryRangeCollection c = new MemoryRangeCollection(new List<MemoryBlock>
            {
                new MemoryBlock(new ReadOnlyBytes(new byte[] { 1, 2, 3 })) { LoadingInfo = new MemoryBlockLoadingInfo(new MemoryBlockID(0), 409 /* should't matter that pretruncation length is large */) },
                new MemoryBlock(new ReadOnlyBytes(new byte[] { 200, 200, 4, 5, 6, 200, 200 })) { LoadingInfo = new MemoryBlockLoadingInfo(new MemoryBlockID(1), 7) },
                new MemoryBlock(new ReadOnlyBytes(new byte[] { 7, 8, 9, 200 }))  { LoadingInfo = new MemoryBlockLoadingInfo(new MemoryBlockID(2), 5 ) },
                new MemoryBlock(new ReadOnlyBytes(new byte[] { 10, 11, 12 })) { LoadingInfo = new MemoryBlockLoadingInfo(new MemoryBlockID(3), 3) },
            }, new List<MemoryRangeByBlockID>()
            {
                new MemoryRangeByBlockID(new MemoryBlockID(2), 1, 2), // 8, 9
                new MemoryRangeByBlockID(new MemoryBlockID(2), 0, 3), // 7, 8, 9
                new MemoryRangeByBlockID(new MemoryBlockID(3), 0, 2), // 10, 11
                new MemoryRangeByBlockID(new MemoryBlockID(1), 1, 1) // 200
            });
            LazinatorMemory memory = new LazinatorMemory(c);
            var result = memory.GetConsolidatedMemory().ToArray();
            result.Should().BeEquivalentTo(new byte[] { 8, 9, 7, 8, 9, 10, 11, 200 });
        }

        /// <summary>
        /// Returns the Memory block of bytes corresponding to a memory block reference. It is required that each memory owner be a MemoryBlock.
        /// </summary>
        /// <param name="memoryBlockInfo">The memory block reference</param>
        /// <returns></returns>
        private ReadOnlyMemory<byte> GetMemoryAtBlockAndOffset(LazinatorMemory lazinatorMemory, MemoryRangeByBlockIndex memoryBlockInfo)
        {
            var memoryBlock = lazinatorMemory.MemoryRangeAtIndex(memoryBlockInfo.MemoryBlockIndex);
            var underlyingReadOnlyMemory = memoryBlock.ReadOnlyMemory.Slice(memoryBlockInfo.OffsetIntoMemoryBlock, memoryBlockInfo.Length);
            return underlyingReadOnlyMemory;
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