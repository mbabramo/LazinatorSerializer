using System;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Structs;

namespace LazinatorTests.Tests
{
    public class WrappersTests : SerializationDeserializationTestBase
    {
        [Fact]
        public void WrapperNullableStructWorks()
        {
            WNullableStruct<ExampleStructContainingClasses> wNullableStruct = new WNullableStruct<ExampleStructContainingClasses>()
            {
                AsNullableStruct = new ExampleStructContainingClasses()
                {
                    MyChar = 'q'
                }
            };
            var w2 = CloneWithOptionalVerification(wNullableStruct, true, false);
            w2.AsNullableStruct.Value.MyChar.Should().Be('q');
            w2.AsNullableStruct = null;
            var w3 = CloneWithOptionalVerification(w2, true, false);
            w3.AsNullableStruct.Should().Be(null);
        }

        [Fact]
        public void WrapperNullableGuidWorks()
        {
            Guid g = Guid.NewGuid();
            WNullableGuid wNullableGuid = new WNullableGuid(g);
            var w2 = CloneWithOptionalVerification(wNullableGuid, true, false);
            w2.WrappedValue.Should().Be(g);
        }

        [Fact]
        public void WrapperIntWorks()
        {
            ExampleContainerStructContainingClasses w = new ExampleContainerStructContainingClasses()
            {
                IntWrapper = 5
            };
            w.IntWrapper = 6;
            w.IntWrapper.Should().Be(6);
        }

        [Fact]
        public void WrapperStringCompareWorks()
        {
            WString a = "a";
            WString b = "b";
            WString n = null; // wrapped value is null
            a.CompareTo(b).Should().Be(-1);
            b.CompareTo(a).Should().Be(1);
            a.CompareTo((string)null).Should().Be(1);
            a.CompareTo(n).Should().Be(1);
            n.CompareTo(a).Should().Be(-1);
            n.CompareTo(n).Should().Be(0);
        }

        [Fact]
        public void WrapperStringEqualsWorks()
        {
            WString a = "a";
            WString b = "b";
            WString n = null; // wrapped value is null
            a.Equals(b).Should().Be(false);
            b.Equals(a).Should().Be(false);
            a.Equals((string)null).Should().Be(false);
            a.Equals(n).Should().Be(false);
            n.Equals(a).Should().Be(false);
            n.Equals(n).Should().Be(true);
            a.Equals(a).Should().Be(true);
        }

        [Fact]
        public void WrappersWithFixedLengthWork()
        {
            // some of these small wrappers have fixed length. The nullable wrappers (other than nullable bool) don't, because if it's null, then we don't include the rest. This means that we need a length byte. 
            SmallWrappersContainer w = new SmallWrappersContainer()
            {
                WrappedBool = true,
                WrappedByte = 1,
                WrappedSByte = -2,
                WrappedChar = 'x',
                WrappedNullableBool = false,
                WrappedNullableByte = 3,
                WrappedNullableSByte = 4,
                WrappedNullableChar = 'Y'
            };
            var c = w.CloneLazinatorTyped();
            c.WrappedBool.Should().Be(true);
            c.WrappedByte.Should().Be((byte)1);
            c.WrappedSByte.Should().Be((sbyte)-2);
            c.WrappedChar.Should().Be('x');
            c.WrappedNullableBool.Should().Be(false);
            c.WrappedNullableByte.Should().Be((byte)3);
            c.WrappedNullableSByte.Should().Be((sbyte)4);
            c.WrappedNullableChar.Should().Be('Y');
            c.WrappedNullableBool = null;
            c.WrappedNullableByte = null;
            c.WrappedNullableSByte = null;
            c.WrappedNullableChar = null;
            var c2 = c.CloneLazinatorTyped();
            c.WrappedNullableBool.HasValue.Should().BeFalse();
            c.WrappedNullableByte.HasValue.Should().BeFalse();
            c.WrappedNullableSByte.HasValue.Should().BeFalse();
            c.WrappedNullableChar.HasValue.Should().BeFalse();
        }

        [Fact]
        public void ListOfWrapperWithFixedLengthWorks()
        {
            SmallWrappersContainer w = new SmallWrappersContainer()
            {
                ListWrappedBytes = new LazinatorList<WByte>()
                {
                    0, 1, 2, 3, 255
                }
            };
            var c = w.CloneLazinatorTyped();
            c.ListWrappedBytes[0].WrappedValue.Should().Be(0);
            c.ListWrappedBytes[1].WrappedValue.Should().Be(1);
            c.ListWrappedBytes[2].WrappedValue.Should().Be(2);
            c.ListWrappedBytes[3].WrappedValue.Should().Be(3);
            c.ListWrappedBytes[4].WrappedValue.Should().Be(255);
        }

        [Fact]
        public void WrapperHasDefaultValue()
        {
            WrapperContainer e = new WrapperContainer();
            var wrappedInt = e.WrappedInt;
            wrappedInt.WrappedValue.Should().Be(1000);
            var clone = e.CloneLazinatorTyped();
            clone.IsDirty.Should().BeFalse(); // even though WrapperContainer constructor executes, the FreeInMemoryObjects method will set IsDirty = false
            clone.WrappedInt.WrappedValue.Should().Be(1000);

        }

        [Fact]
        public void WrappersAndStructsHashProperly()
        {
            WString a = "a";
            WString b = "b";
            uint aHash = a.GetBinaryHashCode32();
            uint bHash = b.GetBinaryHashCode32();
            aHash.Should().NotBe(bHash);

            a = a.CloneLazinatorTyped();
            b = b.CloneLazinatorTyped();
            uint aHash2 = a.GetBinaryHashCode32();
            uint bHash2 = b.GetBinaryHashCode32();
            aHash2.Should().Be(aHash);
            bHash2.Should().Be(bHash);
        }

    }
}
