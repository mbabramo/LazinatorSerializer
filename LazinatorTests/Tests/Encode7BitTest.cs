using System;
using System.IO;
using FluentAssertions;
using Lazinator.Support;
using Lazinator.Buffers; 
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;
using Xunit;

namespace LazinatorTests.Tests
{
    public class Encode7BitTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(127)]
        [InlineData(128)]
        [InlineData(2342311232342)]
        [InlineData(ulong.MaxValue)]
        [InlineData(ulong.MaxValue / 2)]
        public void SevenBitEncodingUlongWorks(ulong valueToWrite)
        {
            ulong valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedUlong(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedUlong(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(127)]
        [InlineData(128)]
        [InlineData(234231123)]
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MaxValue / 2)]
        public void SevenBitEncodingUintWorks(uint valueToWrite)
        {
            uint valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedUint(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedUint(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(127)]
        [InlineData(128)]
        [InlineData(-356)]
        [InlineData(234231)]
        [InlineData(-345732)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void SevenBitEncodingIntWorks(int valueToWrite)
        {
            int valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedInt(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedInt(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(127)]
        [InlineData(128)]
        [InlineData(-356)]
        [InlineData(-4000)]
        [InlineData(16342)]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        public void SevenBitEncodingShortWorks(short valueToWrite)
        {
            short valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedShort(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedShort(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Theory]
        [InlineData((short)0)]
        [InlineData((short)-1)]
        [InlineData((short)1)]
        [InlineData((short)127)]
        [InlineData((short)128)]
        [InlineData((short)-356)]
        [InlineData((short)-4000)]
        [InlineData((short)16342)]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        [InlineData(null)]
        public void SevenBitEncodingNullableShortWorks(short? valueToWrite)
        {
            short? valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableShort(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedNullableShort(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Theory]
        [InlineData((ushort)0)]
        [InlineData((ushort)1)]
        [InlineData((ushort)127)]
        [InlineData((ushort)128)]
        [InlineData((ushort)45000)]
        [InlineData(ushort.MaxValue)]
        [InlineData(null)]
        public void SevenBitEncodingNullableUshortWorks(ushort? valueToWrite)
        {
            ushort? valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableUshort(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedNullableUshort(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Theory]
        [InlineData((byte)0)]
        [InlineData((byte)1)]
        [InlineData((byte)127)]
        [InlineData((byte)128)]
        [InlineData((byte)255)]
        [InlineData(null)]
        public void SevenBitEncodingNullableByteWorks(byte? valueToWrite)
        {
            byte? valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableByte(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedNullableByte(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public void SevenBitEncodingNullableBoolWorks(bool? valueToWrite)
        {
            bool? valueRead = null;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableBool(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedNullableBool(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(127)]
        [InlineData(128)]
        [InlineData(-356)]
        [InlineData(234231)]
        [InlineData(-345732)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(null)]
        public void SevenBitEncodingNullableIntWorks(int? valueToWrite)
        {
            int? valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableInt(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedNullableInt(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(127)]
        [InlineData(128)]
        [InlineData(-356)]
        [InlineData(234231)]
        [InlineData(-345732)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MaxValue / 2)]
        [InlineData(long.MinValue)]
        public void SevenBitEncodingLongWorks(long valueToWrite)
        {
            long valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedLong(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedLong(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(127)]
        [InlineData(128)]
        [InlineData(-356)]
        [InlineData(234231)]
        [InlineData(-345732)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MaxValue / 2)]
        [InlineData(long.MinValue)]
        [InlineData(null)]
        public void SevenBitEncodingNullableLongWorks(long? valueToWrite)
        {
            long? valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableLong(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedNullableLong(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Fact]
        public void SevenBitEncodingTimeSpanWorks()
        {
            SevenBitEncodingTimeSpanWorks_Helper(TimeSpan.FromMilliseconds(1));
            SevenBitEncodingTimeSpanWorks_Helper(TimeSpan.FromDays(99999));
            SevenBitEncodingTimeSpanWorks_Helper(TimeSpan.Zero);
        }

        private void SevenBitEncodingTimeSpanWorks_Helper(TimeSpan valueToWrite)
        {
            TimeSpan valueRead;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedTimeSpan(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedTimeSpan(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Fact]
        public void SevenBitEncodingNullableTimeSpanWorks()
        {
            SevenBitEncodingNullableTimeSpanWorks_Helper(TimeSpan.FromMilliseconds(1));
            SevenBitEncodingNullableTimeSpanWorks_Helper(TimeSpan.FromDays(99999));
            SevenBitEncodingNullableTimeSpanWorks_Helper(TimeSpan.Zero);
            SevenBitEncodingNullableTimeSpanWorks_Helper(null);
        }

        private void SevenBitEncodingNullableTimeSpanWorks_Helper(TimeSpan? valueToWrite)
        {
            TimeSpan? valueRead;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableTimeSpan(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedNullableTimeSpan(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Fact]
        public void SevenBitEncodingDateTimeWorks()
        {
            SevenBitEncodingDateTimeWorks_Helper(DateTime.MinValue);
            SevenBitEncodingDateTimeWorks_Helper(DateTime.MaxValue);
            SevenBitEncodingDateTimeWorks_Helper(DateTime.Now);
        }

        private void SevenBitEncodingDateTimeWorks_Helper(DateTime valueToWrite)
        {
            DateTime valueRead;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedDateTime(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedDateTime(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Fact]
        public void SevenBitEncodingNullableDateTimeWorks()
        {
            SevenBitEncodingNullableDateTimeWorks_Helper(DateTime.MinValue);
            SevenBitEncodingNullableDateTimeWorks_Helper(DateTime.MaxValue);
            SevenBitEncodingNullableDateTimeWorks_Helper(DateTime.Now);
            SevenBitEncodingNullableDateTimeWorks_Helper(null);
        }

        private void SevenBitEncodingNullableDateTimeWorks_Helper(DateTime? valueToWrite)
        {
            DateTime? valueRead;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableDateTime(writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.EndPosition);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDecompressedNullableDateTime(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

    }
}