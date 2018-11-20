using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Collections;

namespace LazinatorTests.Tests
{
    public class SimplifiableTest : SerializationDeserializationTestBase
    {
        [Fact]
        public void SimplifiableWorks()
        {
            Simplifiable s = new Simplifiable()
            {
                ANonSkippableEarlierExample = new Example() { MyChar = 'A' }
            };
            var c = s.CloneLazinatorTyped();
            s.MyInt.Should().Be(0);
            s.MyOtherInt.Should().Be(0);
            s.Example.Should().BeNull();
            s.Example2.Should().BeNull();
            s.Example3.Should().BeNull();
            var originalbytes = s.SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);


            // take advantage of simplifications
            s = new Simplifiable()
            {
                MyInt = 3,
                MyOtherInt = 3,
                ANonSkippableEarlierExample = new Example() { MyChar = 'A' },
                Example = new Example() { MyChar = 'X', MyString = Simplifiable.LongString },
                Example2 = new Example() { MyChar = 'Z' },
                Example3 = null
            };
            c = s.CloneLazinatorTyped();
            s.MyInt.Should().Be(3);
            s.MyOtherInt.Should().Be(3);
            s.ANonSkippableEarlierExample.MyChar.Should().Be('A');
            s.Example.MyChar.Should().Be('X');
            s.Example.MyString.Should().Be(Simplifiable.LongString);
            s.Example2.MyChar.Should().Be('Z');
            s.Example3.Should().BeNull();
            var d = s.SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
            d.Memory.Length.Should().BeLessOrEqualTo(originalbytes.Memory.Length); // shorter even though it is more complex once constructed

            // now use the values that are not simplified
            s = new Simplifiable()
            {
                MyInt = 4,
                MyOtherInt = 5,
                Example = new Example() { MyChar = 'A', MyString = "Hello" },
                Example2 = null,
                Example3 = new Example() { MyDateTime = new System.DateTime(123456) }
            };
            c = s.CloneLazinatorTyped();
            s.MyInt.Should().Be(4);
            s.MyOtherInt.Should().Be(5);
            s.Example.MyChar.Should().Be('A');
            s.Example.MyString.Should().Be("Hello");
            s.Example2.Should().BeNull();
            s.Example3.MyDateTime.Ticks.Should().Be(123456);

            var l = new LazinatorList<Simplifiable>()
            {
                new Simplifiable() {MyInt = 3, MyOtherInt = 3}
            };
            var cl = l.CloneLazinatorTyped();
            cl[0].MyIntsAre3.Should().BeTrue();
            cl[0].MyInt.Should().Be(3);
            cl[0].MyOtherInt.Should().Be(3);

            cl[0].MyInt = 2;
            cl = cl.CloneLazinatorTyped();
            cl[0].MyIntsAre3.Should().BeFalse();

            cl[0].MyInt = 3;
            cl = cl.CloneLazinatorTyped();
            cl[0].MyIntsAre3.Should().BeTrue();

            cl[0].MyInt = 2;
            cl.UpdateStoredBuffer();
            cl[0].MyIntsAre3.Should().BeFalse();
        }
    }
}