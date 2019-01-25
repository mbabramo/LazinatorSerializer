using FluentAssertions;
using Lazinator.Core;
using LazinatorCollections.BitArray;
using LazinatorCollections.ByteSpan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LazinatorCollectionsTests
{
    public class BitArrayTests
    {

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
    }
}
