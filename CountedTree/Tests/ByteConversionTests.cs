using System;
using System.Linq;
using System.Collections.Generic;
using CountedTree.Core;
using Xunit;
using FluentAssertions;
using CountedTree.ByteUtilities;
using Lazinator.Wrappers;
using Lazinator.Core;
using Lazinator.Buffers;

namespace CountedTree.Tests
{
    public class ByteConversionTests
    {
        [Fact]
        public void KeyAndIDConversionsWork_float()
        {
            KeyAndID<WFloat> f = new KeyAndID<WFloat>(3.5F, 3);
            byte[] b = f.SerializeToArray();
            KeyAndID<WFloat> d = default(KeyAndID<WFloat>);
            d.DeserializeLazinator(new LazinatorMemory(b));
            f.Equals(d).Should().BeTrue();
        }

        [Fact]
        public void KeyAndIDConversionsWork_ulong()
        {
            KeyAndID<WUInt64> f = new KeyAndID<WUInt64>(17, 3);
            byte[] b = f.SerializeToArray();
            KeyAndID<WUInt64> d = default(KeyAndID<WUInt64>);
            d.DeserializeLazinator(new LazinatorMemory(b));
            f.Equals(d).Should().BeTrue();
        }

        [Fact]
        public void KeyAndIDConversionsWork_uint()
        {
            KeyAndID<WUInt32> f = new KeyAndID<WUInt32>(17, 3);
            byte[] b = f.SerializeToArray();
            KeyAndID<WUInt32> d = default(KeyAndID<WUInt32>);
            d.DeserializeLazinator(new LazinatorMemory(b));
            f.Equals(d).Should().BeTrue();
        }

        [Fact]
        public void KeyAndIDConversionsWork_decimal()
        {
            KeyAndID<WDecimal> f = new KeyAndID<WDecimal>(17, 3);
            byte[] b = f.SerializeToArray();
            KeyAndID<WDecimal> d = default(KeyAndID<WDecimal>);
            d.DeserializeLazinator(new LazinatorMemory(b));
            f.Equals(d).Should().BeTrue();
        }

        [Fact]
        public void HalfByteCompressionWorks()
        {
            byte compressed = HalfByteCompression.Compress(15, 13);
            byte byteA, byteB;
            HalfByteCompression.Decompress(compressed, out byteA, out byteB);
            byteA.Should().Be(15);
            byteB.Should().Be(13);
        }

        [Fact]
        public void HalfByteArrayCompressionWorks_OddNumberOfItems()
        { 
            byte[] uncompressedArray = new byte[] { 13, 12, 11, 10, 9 };
            byte[] compressedArray = HalfByteCompression.Compress(uncompressedArray);
            byte[] uncompressedArray2 = HalfByteCompression.Decompress(compressedArray, uncompressedArray.Length % 2 == 0);
            uncompressedArray.SequenceEqual(uncompressedArray2).Should().BeTrue();
        }

        [Fact]
        public void HalfByteArrayCompressionWorks_EvenNumberOfItems()
        {
            byte[] uncompressedArray = new byte[] { 13, 12, 11, 10, 9, 8 };
            byte[] compressedArray = HalfByteCompression.Compress(uncompressedArray);
            byte[] uncompressedArray2 = HalfByteCompression.Decompress(compressedArray, uncompressedArray.Length % 2 == 0);
            uncompressedArray.SequenceEqual(uncompressedArray2).Should().BeTrue();
        }
    }
}
