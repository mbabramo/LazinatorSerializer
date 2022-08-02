using System;
using FluentAssertions;
using Lazinator.Buffers;
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
        public void SevenBitEncodingULongWorks(ulong valueToWrite)
        {
            ulong valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            BufferWriter writer = new BufferWriter();
            numBytesWritten = CompressedIntegralTypes.WriteCompressedULong(ref writer, valueToWrite);
            numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
            bytes = writer.LazinatorMemory.GetConsolidatedMemory();
            valueRead = bytes.Span.ToDecompressedUInt64(ref numBytesRead);
            valueRead.Should().Be(valueToWrite);
            numBytesRead.Should().Be(numBytesWritten);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(127)]
        [InlineData(128)]
        [InlineData(234231123)]
        [InlineData(uint.MaxValue)]
        [InlineData(uint.MaxValue / 2)]
        public void SevenBitEncodingUIntWorks(uint valueToWrite)
        {
            uint valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedUInt(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
                valueRead = bytes.Span.ToDecompressedUInt32(ref numBytesRead);
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
        [InlineData(1000000224)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void SevenBitEncodingIntWorks(int valueToWrite)
        {
            int valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedInt(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
                valueRead = bytes.Span.ToDecompressedInt32(ref numBytesRead);
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
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedShort(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
                valueRead = bytes.Span.ToDecompressedInt16(ref numBytesRead);
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
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableShort(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
                valueRead = bytes.Span.ToDecompressedNullableInt16(ref numBytesRead);
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
        public void SevenBitEncodingNullableUShortWorks(ushort? valueToWrite)
        {
            ushort? valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableUShort(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
                valueRead = bytes.Span.ToDecompressedNullableUInt16(ref numBytesRead);
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
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableByte(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
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
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableBool(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
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
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableInt(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
                valueRead = bytes.Span.ToDecompressedNullableInt32(ref numBytesRead);
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
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedLong(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
                valueRead = bytes.Span.ToDecompressedInt64(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Theory]
        [InlineData((long) 0)]
        [InlineData((long)-1)]
        [InlineData((long)1)]
        [InlineData((long)127)]
        [InlineData((long)128)]
        [InlineData((long)-356)]
        [InlineData((long)234231)]
        [InlineData((long)-345732)]
        [InlineData((long) int.MaxValue)]
        [InlineData((long) int.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MaxValue / 2)]
        [InlineData(long.MinValue)]
        [InlineData(null)]
        public void SevenBitEncodingNullableLongWorks(long? valueToWrite)
        {
            long? valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableLong(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
                valueRead = bytes.Span.ToDecompressedNullableInt64(ref numBytesRead);
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
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedTimeSpan(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
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
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableTimeSpan(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
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
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedDateTime(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
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
            BufferWriter writer = new BufferWriter();
            {
                numBytesWritten = CompressedIntegralTypes.WriteCompressedNullableDateTime(ref writer, valueToWrite);
                numBytesWritten.Should().Be((int)writer.ActiveMemoryPosition);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
                valueRead = bytes.Span.ToDecompressedNullableDateTime(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

    }
}