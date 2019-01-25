using FluentAssertions;
using Lazinator.Core;
using LazinatorCollections.ByteSpan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LazinatorCollectionsTests
{
    public class ByteSpanTests
    {

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
    }
}
