using FluentAssertions;
using Lazinator.Buffers;
using Lazinator.Core;
using Xunit;
using System.Buffers;
using System;
using System.Linq;

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
                    e.Memory.Span[bufferSize - 1] = 1;
                }
            }
        }

        [Fact]
        public void LazinatorMemoryAggregationAndSlicing()
        {
            int numChunks = 5;
            int memoryPerChunk = 100;
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
                else m = m.WithAppendedChunk(new SimpleMemoryOwner<byte>(b));
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
        public void UsingReturnedMemoryTriggersException()
        {
            const int bufferSize = 64 * 1024;
            IMemoryOwner<byte> rentedMemory = LazinatorUtilities.GetRentedMemory(bufferSize);
            rentedMemory.Dispose();
            Action a = () => { var m = rentedMemory.Memory.Span; m[0] = 1; };
            a.Should().Throw<ObjectDisposedException>();
        }

        [Fact]
        public void BinaryBufferWriterCanBeCreated()
        {
            const int bufferSize = 64 * 1024;
            for (int i = 0; i < 1; i++)
            { // same as above; higher iterations causes no memory leak
                BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
                var rented = writer.LazinatorMemory;
                rented.InitialOwnedMemory.Memory.Length.Should().BeGreaterOrEqualTo(bufferSize);
            }
        }

        [Fact]
        public void CanWriteBeyondInitialBufferSize()
        {
            const int bufferSize = 1024;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
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