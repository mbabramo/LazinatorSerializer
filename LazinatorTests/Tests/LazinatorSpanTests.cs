using System;
using System.Linq;
using FluentAssertions;
using LazinatorTests.Examples.Collections;
using Lazinator.Core;
using Xunit;
using Lazinator.Spans;

namespace LazinatorTests.Tests
{
    public class LazinatorSpanTests
    {

        static char[] chars = "Hello, world".ToCharArray();

        public static SpanAndMemory GetSpanAndMemory(bool emptySpans)
        {
            if (emptySpans)
                return new SpanAndMemory
                {
                    MyReadOnlyMemoryByte = new Memory<byte>(new byte[] { }),
                    MyReadOnlyMemoryChar = new ReadOnlyMemory<char>(new char[] { }),
                    MyReadOnlyMemoryInt = new ReadOnlyMemory<int>(new int[] { }),
                    MyReadOnlySpanByte = new Span<byte>(new byte[] { }),
                    MyReadOnlySpanChar = new ReadOnlySpan<char>(new char[] { }),
                    MyReadOnlySpanLong = new Span<long>(new long[] { }),
                    MyMemoryByte = new Memory<byte>(new byte[] { }),
                    MyMemoryInt = new Memory<int>(new int[] { }),
                    MyNullableMemoryByte = new Memory<byte>(new byte[] { }),
                    MyNullableMemoryInt = new Memory<int>(new int[] { }),
                    MyNullableReadOnlyMemoryInt = new ReadOnlyMemory<int>(new int[] { })

                };
            return new SpanAndMemory
            {
                MyReadOnlyMemoryByte = new Memory<byte>(new byte[] { 3, 4, 5 }),
                MyReadOnlyMemoryChar = new ReadOnlyMemory<char>(chars),
                MyReadOnlyMemoryInt = new ReadOnlyMemory<int>(new int[] { 3, 4, 5 }),
                MyReadOnlySpanByte = new Span<byte>(new byte[] { 3, 4, 5 }),
                MyReadOnlySpanChar = new ReadOnlySpan<char>(chars),
                MyReadOnlySpanLong = new Span<long>(new long[] { -234234, long.MaxValue }),
                MyMemoryByte = new Memory<byte>(new byte[] { 3, 4, 5 }),
                MyMemoryInt = new Memory<int>(new int[] { 3, 4, 5 }),
                MyNullableMemoryByte = new Memory<byte>(new byte[] { 3, 4, 5 }),
                MyNullableMemoryInt = new Memory<int>(new int[] { 3, 4, 5 }),
                MyNullableReadOnlyMemoryInt = new ReadOnlyMemory<int>(new int[] { 3, 4, 5 })
            };
        }

        [Fact]
        public void LazinatorSpansAndMemory()
        {
            

            var original = GetSpanAndMemory(false);
            var copy = GetSpanAndMemory(false);
            for (int i = 0; i < 3; i++)
            {
                var result = copy.CloneLazinatorTyped();
                result.MyReadOnlyMemoryByte.Span[1].Should().Be(4);
                result.MyReadOnlyMemoryByte.Length.Should().Be(3);
                new string(result.MyReadOnlyMemoryChar.Span).Equals("Hello, world").Should().BeTrue();
                new string(result.MyReadOnlyMemoryChar.Span.Slice(0, 5)).Equals("Hello").Should().BeTrue();
                result.MyReadOnlyMemoryInt.Span[1].Should().Be(4);
                result.MyReadOnlyMemoryInt.Length.Should().Be(3);
                result.MyReadOnlySpanByte.Length.Should().Be(3);
                result.MyReadOnlySpanByte[1].Should().Be(4);
                new string(result.MyReadOnlySpanChar).Equals("Hello, world").Should().BeTrue();
                new string(result.MyReadOnlySpanChar.Slice(0, 5)).Equals("Hello").Should().BeTrue();
                result.MyReadOnlySpanLong.Length.Should().Be(2);
                result.MyReadOnlySpanLong[1].Should().Be(long.MaxValue);
                result.MyMemoryByte.Span[1].Should().Be(4);
                result.MyMemoryByte.Length.Should().Be(3);
                result.MyMemoryInt.Span[1].Should().Be(4);
                result.MyMemoryInt.Length.Should().Be(3);
                result.MyNullableMemoryByte.Value.Span[1].Should().Be(4);
                result.MyNullableMemoryByte.Value.Length.Should().Be(3);
                result.MyNullableMemoryInt.Value.Span[1].Should().Be(4);
                result.MyNullableMemoryInt.Value.Length.Should().Be(3);
                result.MyNullableReadOnlyMemoryInt.Value.Span[1].Should().Be(4);
                result.MyNullableReadOnlyMemoryInt.Value.Length.Should().Be(3);
                copy = result;
            }

            original = GetSpanAndMemory(true);
            copy = GetSpanAndMemory(true);
            for (int i = 0; i < 3; i++)
            {
                var result = copy.CloneLazinatorTyped();
                result.MyReadOnlyMemoryByte.Length.Should().Be(0);
                result.MyReadOnlyMemoryChar.Length.Should().Be(0);
                result.MyReadOnlyMemoryInt.Length.Should().Be(0);
                result.MyReadOnlySpanByte.Length.Should().Be(0);
                result.MyReadOnlySpanChar.Length.Should().Be(0);
                result.MyReadOnlySpanLong.Length.Should().Be(0);
                result.MyMemoryByte.Length.Should().Be(0);
                result.MyMemoryInt.Length.Should().Be(0);
                result.MyNullableMemoryByte.Value.Length.Should().Be(0);
                result.MyNullableMemoryInt.Value.Length.Should().Be(0);
                result.MyNullableReadOnlyMemoryInt.Value.Length.Should().Be(0);
                copy = result;
            }
        }

        [Fact]
        public void LazinatorByteSpan()
        {
            byte[] originalBytes = new byte[] { 1, 2, 3 };
            LazinatorByteSpan lazinatorBytes = new LazinatorByteSpan(originalBytes);
            lazinatorBytes.GetIsReadOnlyMode().Should().BeFalse();
            LazinatorByteSpan clone = lazinatorBytes.CloneLazinatorTyped();
            byte[] bytesConverted = clone.GetSpanToReadOnly().ToArray();
            clone.GetIsReadOnlyMode().Should().BeTrue();
            bytesConverted.SequenceEqual(originalBytes).Should().BeTrue();
            clone.GetSpanToReadOrWrite()[0] = 4;
            clone.GetIsReadOnlyMode().Should().BeFalse();
            LazinatorByteSpan clone2 = clone.CloneLazinatorTyped();
            clone2.GetIsReadOnlyMode().Should().BeTrue();
            byte[] bytesConverted2 = clone2.GetSpanToReadOnly().ToArray();
            clone2.GetIsReadOnlyMode().Should().BeTrue();
            byte[] expectedBytes = new byte[] { 4, 2, 3 };
            bytesConverted2.SequenceEqual(expectedBytes).Should().BeTrue();

            byte[] anotherSequence = new byte[] { 10, 11, 12, 13 };
            clone2.SetMemory(anotherSequence);
            clone2.GetIsReadOnlyMode().Should().BeFalse();
            LazinatorByteSpan clone3 = clone2.CloneLazinatorTyped();
            clone3.GetIsReadOnlyMode().Should().BeTrue();
            byte[] bytesConverted3 = clone3.GetSpanToReadOnly().ToArray();
            bytesConverted3.SequenceEqual(anotherSequence).Should().BeTrue();

            byte[] lastSequence = new byte[] { 20, 21, 22, 23, 24, 25 };
            clone3.SetReadOnlySpan(lastSequence);
            clone3.GetIsReadOnlyMode().Should().BeTrue();
            LazinatorByteSpan clone4 = clone3.CloneLazinatorTyped();
            clone4.GetIsReadOnlyMode().Should().BeTrue();
            byte[] bytesConverted4 = clone4.GetSpanToReadOnly().ToArray();
            bytesConverted4.SequenceEqual(lastSequence).Should().BeTrue();
        }

        [Fact]
        public void LazinatorByteSpan_EnsureUpToDate()
        {
            byte[] originalBytes = new byte[] { 1, 2, 3 };
            LazinatorByteSpan lazinatorBytes = new LazinatorByteSpan(originalBytes);
            lazinatorBytes.GetIsReadOnlyMode().Should().BeFalse();
            LazinatorByteSpan clone = lazinatorBytes.CloneLazinatorTyped();
            var byteSpan = clone.GetSpanToReadOrWrite();
            clone.IsDirty = true;
            clone.UpdateStoredBuffer();
            var x = byteSpan[0];
        }

        [Fact]
        public void SpanAndMemory_EnsureUpToDate()
        {
            byte[] originalBytes = new byte[] { 1, 2, 3 };
            SpanAndMemory spanAndMemory = new SpanAndMemory();
            spanAndMemory.MyMemoryByte = new Memory<byte>(originalBytes);
            SpanAndMemory clone = spanAndMemory.CloneLazinatorTyped();
            spanAndMemory.MyMemoryByte.Span[0] = 5;
            var byteSpan = clone.MyMemoryByte;
            clone.IsDirty = true; // trigger replacement of buffer
            clone.UpdateStoredBuffer();
            var x = byteSpan.Span[0];
            x.Should().Be(1);
        }

        [Fact]
        public void LazinatorBitArrayWorks()
        {
            LazinatorBitArray reservedArray = new LazinatorBitArray(100);
            reservedArray.Length.Should().Be(100);

            bool[] values1 = new bool[]
                {true, false, true, false, true, false, true, false, true};
            bool[] values2 = new bool[]
                {true, true, true, true, true, false, false, false, false};
            LazinatorBitArray bits1 = new LazinatorBitArray(values1);
            LazinatorBitArray bits2 = new LazinatorBitArray(values2);
            bits1 = bits1.CloneLazinatorTyped();
            bits2 = bits2.CloneLazinatorTyped();
            bits1.Count.Should().Be(9);
            bits2.Count.Should().Be(9);
            var not = new LazinatorBitArray(bits1).Not();
            for (int i = 0; i < values1.Length; i++)
                not[i].Should().Be(!values1[i]);
            var cleared = new LazinatorBitArray(bits1);
            cleared.SetAll(false);
            for (int i = 0; i < values1.Length; i++)
                cleared[i].Should().Be(false);
            cleared.SetAll(true);
            for (int i = 0; i < values1.Length; i++)
                cleared[i].Should().Be(true);
            var and = new LazinatorBitArray(bits1).And(bits2);
            for (int i = 0; i < values1.Length; i++)
                and[i].Should().Be(values1[i] & values2[i]);
            var or = new LazinatorBitArray(bits1).Or(bits2);
            for (int i = 0; i < values1.Length; i++)
                or[i].Should().Be(values1[i] | values2[i]);
            var xor = new LazinatorBitArray(bits1).Xor(bits2);
            for (int i = 0; i < values1.Length; i++)
                xor[i].Should().Be(values1[i] ^ values2[i]);
        }

        [Fact]
        public void LazinatorMemoryInt()
        {
            SpanAndMemory GetObject(int thirdItem)
            {
                return new SpanAndMemory
                {
                    MyMemoryInt = new int[] { 3, 4, thirdItem }
                };
            }

            void SetIndex(Memory<int> source, int index, int value)
            {
                // note: this seems to be a way to write into Memory<T>. I think that since Span lives only on the stack, the memory is guaranteed not to move. So, this works. I'm not clear why there isn't a direct way to index into Memory<T>, without getting a span. Maybe the reason is that they want to encourage you to get a Span if you are going to do a large number of writes. It's awkward that you can't say source.Span[index] = value; 
                var sourceSpan = source.Span;
                sourceSpan[index] = value;
            }
            bool SequenceEqual(Memory<int> a, Memory<int> b)
            {
                if (a.Length != b.Length)
                    return false;
                Span<int> aSpan = a.Span;
                Span<int> bSpan = b.Span;
                for (int i = 0; i < a.Length; i++)
                    if (aSpan[i] != bSpan[i])
                        return false;
                return true;
            }

            var original = GetObject(5);
            var copy = GetObject(5);
            SetIndex(copy.MyMemoryInt, 2, 6);
            var span = copy.MyMemoryInt.Span;
            span[2].Should().Be(6);
            var result = copy.CloneLazinatorTyped();
            SequenceEqual(copy.MyMemoryInt, result.MyMemoryInt).Should().BeTrue();
        }

        [Fact]
        public void LazinatorNullableMemoryAsNull()
        {
            var l = new SpanAndMemory
            {
                MyNullableMemoryInt = null,
                MyNullableMemoryByte = null,
                MyNullableReadOnlyMemoryInt = null
            };
            var c = l.CloneLazinatorTyped();
            c.MyNullableReadOnlyMemoryInt.Should().Be(null);
            c.MyNullableMemoryByte.Should().Be(null);
            c.MyNullableReadOnlyMemoryInt.Should().Be(null);
        }

        [Fact]
        public void LazinatorNullableMemoryInt()
        {
            SpanAndMemory GetObject()
            {
                return new SpanAndMemory
                {
                    MyNullableMemoryInt = new int[] { 3, 4, 5 }
                };
            }

            SpanAndMemory GetObject2()
            {
                return new SpanAndMemory
                {
                    MyNullableMemoryInt = new int[] { 0, 0, 0 }
                };
            }

            SpanAndMemory GetEmptyMemoryObject()
            {
                return new SpanAndMemory
                {
                    MyNullableMemoryInt = new int[] { }
                };
            }

            void SetIndex(Memory<int> source, int index, int value)
            {
                // note: this seems to be a way to write into Memory<T>. I think that since Span lives only on the stack, the memory is guaranteed not to move. So, this works. I'm not clear why there isn't a direct way to index into Memory<T>, without getting a span. Maybe the reason is that they want to encourage you to get a Span if you are going to do a large number of writes. It's awkward that you can't say source.Span[index] = value; 
                var sourceSpan = source.Span;
                sourceSpan[index] = value;
            }
            bool SequenceEqual(Memory<int> a, Memory<int> b)
            {
                if (a.Length != b.Length)
                    return false;
                Span<int> aSpan = a.Span;
                Span<int> bSpan = b.Span;
                for (int i = 0; i < a.Length; i++)
                    if (aSpan[i] != bSpan[i])
                        return false;
                return true;
            }

            // first, we'll do the same thing we did for the non-nullable field

            var original = GetObject();
            var copy = GetObject();
            SetIndex(copy.MyNullableMemoryInt.Value, 2, 6);
            var span = copy.MyNullableMemoryInt.Value.Span;
            span[2].Should().Be(6);
            var result = copy.CloneLazinatorTyped();
            SequenceEqual(copy.MyNullableMemoryInt.Value, result.MyNullableMemoryInt.Value).Should().BeTrue();
            result.MyMemoryInt.Length.Should().Be(0);

            // now, see if the second object works
            original = GetObject2();
            result = original.CloneLazinatorTyped();
            SequenceEqual(result.MyNullableMemoryInt.Value, original.MyNullableMemoryInt.Value).Should().BeTrue();

            // now, let's make sure that null serializes correctly
            original = new SpanAndMemory();
            result = original.CloneLazinatorTyped();
            result.MyNullableMemoryInt.Should().Be(null);
            result.MyMemoryInt.Length.Should().Be(0);

            // and empty list must serialize correctly too
            original = GetEmptyMemoryObject();
            result = original.CloneLazinatorTyped();
            result.MyNullableMemoryInt.Should().NotBeNull();
            result.MyNullableMemoryInt.Value.Length.Should().Be(0);
            result.MyMemoryInt.Length.Should().Be(0);
        }



        [Fact]
        public void DisposalOfBufferInvalidatesReadOnlySpan()
        {
            SpanAndMemory s = new SpanAndMemory()
            {
                MyReadOnlySpanByte = new Memory<byte>(new byte[] { 3, 4, 5 }).Span
            };
            s = s.CloneLazinatorTyped();
            var memory = s.MyReadOnlySpanByte;
            s.LazinatorMemoryStorage.Dispose();
            s.LazinatorMemoryStorage.Disposed.Should().BeTrue();
            Action a = () =>
            {
                var m2 = s.MyReadOnlySpanByte;
            };
            a.Should().Throw<ObjectDisposedException>();
        }
    }
}
