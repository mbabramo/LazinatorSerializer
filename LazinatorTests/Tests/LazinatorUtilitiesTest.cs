using FluentAssertions;
using Lazinator.Buffers;
using Lazinator.Core;
using Xunit;
using System.Buffers;
using System;

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
                BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize, null);
                var rented = writer.LazinatorMemory;
                rented.OwnedMemory.Memory.Length.Should().BeGreaterOrEqualTo(bufferSize);
            }
        }

        [Fact]
        public void CanWriteBeyondInitialBufferSize()
        {
            const int bufferSize = 1024;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize, null);
            for (int j = 0; j < 5000; j++)
                writer.Write(j);
            var written = writer.Written;
            for (int j = 0; j < 5000; j++)
            {
                int index = j * sizeof(int);
                ReadUncompressedPrimitives.ToInt32(written, ref index).Should().Be(j);
            }
        }
    }
}