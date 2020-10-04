using System;
using FluentAssertions;
using Lazinator.Buffers;
using Xunit;

namespace LazinatorTests.Tests
{
    public class EncodeDecimalTest
    {
        [Theory]
        [InlineData(null)]
        public void DecimalEncodingWorks(decimal? valueToWrite)
        {
            decimal? valueRead = 0;
            int numBytesWritten = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            BinaryBufferWriter writer = new BinaryBufferWriter();
            {
                numBytesWritten = CompressedDecimal.WriteCompressedNullableDecimal(writer, valueToWrite);
                numBytesWritten.Should().Be((int) writer.Position);
                bytes = writer.LazinatorMemory.GetConsolidatedMemory();
                valueRead = CompressedDecimal.ToDecompressedNullableDecimal(bytes.Span, ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesWritten);
            }
        }

        [Theory]
        [InlineData("0.0")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("127")]
        [InlineData("128")]
        [InlineData("255")]
        [InlineData("1.23")]
        [InlineData("-1.23")]
        [InlineData("2.55")]
        [InlineData("-2.55")]
        [InlineData("327000")]
        [InlineData("-327000")]
        [InlineData(".035")]
        [InlineData("-0.234")]
        [InlineData("3590")]
        [InlineData("-45678")]
        [InlineData("1.2345678")]
        [InlineData("-1.2345678")]
        [InlineData("1.234567823423482340")]
        [InlineData("-23423048293408234234")]
        public void DecimalEncodingWorks_String(string decimalValueAsString)
        {
            // This is necessary because we can't use decimals in InlineData in xunit.
            decimal converted = Convert.ToDecimal(decimalValueAsString);
            DecimalEncodingWorks(converted);
        }


        [Theory]
        [InlineData("0.0")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("127")]
        [InlineData("128")]
        [InlineData("255")]
        [InlineData("1.23")]
        [InlineData("-1.23")]
        [InlineData("2.55")]
        [InlineData("-2.55")]
        [InlineData("327000")]
        [InlineData("-327000")]
        [InlineData(".035")]
        [InlineData("-0.234")]
        [InlineData("3590")]
        [InlineData("-45678")]
        [InlineData("1.2345678")]
        [InlineData("-1.2345678")]
        [InlineData("1.234567823423482340")]
        [InlineData("-23423048293408234234")]
        public void CanDecomposeDecimal(string decimalValueAsString)
        {
            decimal converted = Convert.ToDecimal(decimalValueAsString);
            CompressedDecimal.DecomposableDecimal d = new CompressedDecimal.DecomposableDecimal(converted);
            CompressedDecimal.DecomposedDecimal d2 = d.DecomposedDecimal;
            CompressedDecimal.DecomposableDecimal d3 = new CompressedDecimal.DecomposableDecimal(new CompressedDecimal.DecomposedDecimal(d2.negative, d2.scale, d2.hi, d2.mid, d2.lo));
            d3.Decimal.Should().Be(converted);
        }
        
    }
}