using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using Lazinator.Buffers; 
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;
using Lazinator.Support;
using Xunit;

namespace LazinatorTests.Tests
{
    public class EncodePrimitivesTest
    {
        [Fact]
        public void ReadOnlySpan_CanEncodeByte()
        {
            byte valueToWrite = 179;
            byte valueRead = 0;
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                writer.Write(valueToWrite);
                numBytesExpected = 1;
                writer.Position.Should().Be(numBytesExpected);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToByte(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeSByte()
        {
            sbyte valueToWrite = -111;
            sbyte valueRead = 0;
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                writer.Write(valueToWrite);
                numBytesExpected = 1;
                writer.Position.Should().Be(numBytesExpected);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToSByte(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeChar_WritingBytesIndividually()
        {
            char valueToWrite = '\u2342'; // encoding will be Unicode because we break down the bytes
            char valueRead = 'X';
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                writer.WriteCharInTwoBytes(valueToWrite);
                numBytesExpected = 2;
                writer.Position.Should().Be(numBytesExpected);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToChar(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeChar_SpecifyingEncodingInBinaryBufferWriter()
        {
            char valueToWrite = '\u2342'; // encoding will be Unicode because we explicitly select it
            char valueRead = 'X';
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                writer.Write(valueToWrite);
                numBytesExpected = 2;
                writer.Position.Should().Be(numBytesExpected);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToChar(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeInt16()
        {
            Int16 valueToWrite = 1790;
            Int16 valueRead = 0;
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                writer.Write(valueToWrite);
                numBytesExpected = 2;
                writer.Position.Should().Be(numBytesExpected);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToInt16(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeInt32()
        {
            Int32 valueToWrite = 179021;
            Int32 valueRead = 0;
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                writer.Write(valueToWrite);
                numBytesExpected = 4;
                writer.Position.Should().Be(numBytesExpected);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToInt32(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeInt64()
        {
            Int64 valueToWrite = 179021235;
            Int64 valueRead = 0;
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                writer.Write(valueToWrite);
                numBytesExpected = 8;
                writer.Position.Should().Be(numBytesExpected);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToInt64(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeUInt16()
        {
            UInt16 valueToWrite = 1790;
            UInt16 valueRead = 0;
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                writer.Write(valueToWrite);
                numBytesExpected = 2;
                writer.Position.Should().Be(numBytesExpected);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToUInt16(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeUInt32()
        {
            UInt32 valueToWrite = 179021;
            UInt32 valueRead = 0;
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                writer.Write(valueToWrite);
                numBytesExpected = 4;
                writer.Position.Should().Be(numBytesExpected);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToUInt32(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeUInt64()
        {
            UInt64 valueToWrite = 179021235;
            UInt64 valueRead = 0;
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                writer.Write(valueToWrite);
                numBytesExpected = 8;
                writer.Position.Should().Be(numBytesExpected);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToUInt64(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeSingle()
        {
            float valueToWrite = 3.4F;
            float valueRead = 0;
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                writer.Write(valueToWrite);
                numBytesExpected = 4;
                writer.Position.Should().Be(numBytesExpected);
                bytes = writer.MemoryInBuffer.FilledMemory;
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToSingle(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeDouble()
        {
            double valueToWrite = 3.4;
            double valueRead = 0;
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                writer.Write(valueToWrite);
                numBytesExpected = 8;
                writer.Position.Should().Be(numBytesExpected);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToDouble(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeString()
        {
            string valueToWrite = "Hello, World";
            CanEncodeShortStringHelper(valueToWrite);
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeString_Null()
        {
            string valueToWrite = null;
            CanEncodeShortStringHelper(valueToWrite);
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeString_Empty()
        {
            string valueToWrite = "";
            CanEncodeShortStringHelper(valueToWrite);
        }

        private static void CanEncodeShortStringHelper(string valueToWrite)
        {
            string valueRead = "";
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(writer, valueToWrite);
                numBytesExpected = (valueToWrite?.Length ?? 0) + 1; // 12 characters (1 byte each) + 1 byte for length
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToString_VarIntLengthUtf8(ref numBytesRead);
                valueRead.Should().Be(valueToWrite);
                numBytesRead.Should().Be(numBytesExpected);
            }
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeStringBrotli()
        {
            string valueToWrite = "Hello, World";
            CanEncodeStringBrotliHelper(valueToWrite);
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeStringBrotli_Null()
        {
            string valueToWrite = null;
            CanEncodeStringBrotliHelper(valueToWrite);
        }

        [Fact]
        public void ReadOnlySpan_CanEncodeStringBrotli_Empty()
        {
            string valueToWrite = "";
            CanEncodeStringBrotliHelper(valueToWrite);
        }

        private static void CanEncodeStringBrotliHelper(string valueToWrite)
        {
            string valueRead = "";
            int numBytesExpected = 0, numBytesRead = 0;
            ReadOnlyMemory<byte> bytes;
            using (BinaryBufferWriter writer = new BinaryBufferWriter())
            {
                EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(writer, valueToWrite);
                bytes = writer.MemoryInBuffer.FilledMemory;
                valueRead = bytes.Span.ToString_BrotliCompressedWithLength(ref numBytesRead);
                numBytesRead.Should().Be(bytes.Length);
                valueRead.Should().Be(valueToWrite);
            }
        }
    }
}
