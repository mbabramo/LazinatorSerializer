using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Hierarchy;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.NonAbstractGenerics;

namespace LazinatorTests.Tests
{
    public class ParentsTests : SerializationDeserializationTestBase
    {
        [Fact]
        public void LazinatorParentsCollectionWorks()
        {
            ExampleChild e1 = new ExampleChild();
            Example e2 = new Example();
            ExampleChild e3 = new ExampleChild();

            LazinatorParentsCollection c = new LazinatorParentsCollection();
            c.Count.Should().Be(0);
            c.Any().Should().BeFalse();
            c.LastAdded.Should().BeNull();
            c.EnumerateParents().Count().Should().Be(0);

            c = c.WithAdded(e1);
            c.Count.Should().Be(1);
            c.Any().Should().BeTrue();
            c.LastAdded.Should().Be(e1);
            c.EnumerateParents().SequenceEqual(new ILazinator[] { e1 }).Should().BeTrue();

            c = c.WithAdded(e1); // added a second time
            c.Count.Should().Be(1);
            c.Any().Should().BeTrue();
            c.LastAdded.Should().Be(e1);
            c.EnumerateParents().SequenceEqual(new ILazinator[] { e1 }).Should().BeTrue();

            c = c.WithRemoved(e1);
            c.Count.Should().Be(1);
            c.Any().Should().BeTrue();
            c.LastAdded.Should().Be(e1);
            c.EnumerateParents().SequenceEqual(new ILazinator[] { e1 }).Should().BeTrue();

            c = c.WithRemoved(e1);
            c.Count.Should().Be(0);
            c.Any().Should().BeFalse();
            c.LastAdded.Should().BeNull();
            c.EnumerateParents().Count().Should().Be(0);

            c = c.WithAdded(e1).WithAdded(e2);
            c.Count.Should().Be(2);
            c.Any().Should().BeTrue();
            c.LastAdded.Should().Be(e2);
            c.EnumerateParents().SequenceEqual(new ILazinator[] { e2, e1 }).Should().BeTrue();

            c = c.WithAdded(e3);
            c.Count.Should().Be(3);
            c.Any().Should().BeTrue();
            c.LastAdded.Should().Be(e3);
            c.EnumerateParents().Count().Should().Be(3);

            c = c.WithRemoved(e3);
            c.Count.Should().Be(2);
            c.Any().Should().BeTrue();
            c.LastAdded.Should().Be(null);
            c.EnumerateParents().Count().Should().Be(2);

            c = c.WithAdded(e2);
            c.Count.Should().Be(2); // e2 only counts 1 time
            c.Any().Should().BeTrue();
            c.LastAdded.Should().Be(e2);
            c.EnumerateParents().SequenceEqual(new ILazinator[] { e2, e1 }).Should().BeTrue();

            c = c.WithRemoved(e2);
            c.Count.Should().Be(2); // e2 only counts 1 time
            c.Any().Should().BeTrue();
            c.LastAdded.Should().Be(e2); // still listed as LastAdded
            c.EnumerateParents().Count().Should().Be(2);

            c = c.WithRemoved(e2).WithRemoved(e1);
            c.Count.Should().Be(0);
            c.Any().Should().BeFalse();
            c.LastAdded.Should().Be(null);
            c.EnumerateParents().Count().Should().Be(0);

        }


        [Fact]
        public void SameObjectCanAppearTwice()
        {
            LazinatorTuple<ExampleChild, ExampleChild> e = new LazinatorTuple<ExampleChild, ExampleChild>()
            {
                Item1 = new ExampleChild(),
                Item2 = new ExampleChild()
            };
            var c = e.CloneLazinatorTyped();
            c.Item2.MyLong = -123456;
            c.Item1 = c.Item2;
            var c2 = c.CloneLazinatorTyped();
            c2.Item1.MyLong.Should().Be(-123456);
            c2.Item2.MyLong.Should().Be(-123456);
        }

        [Fact]
        public void ChangeToObjectAppearingTwiceAffectsBoth()
        {
            LazinatorTuple<ExampleChild, ExampleChild> e = new LazinatorTuple<ExampleChild, ExampleChild>()
            {
                Item1 = new ExampleChild(),
                Item2 = new ExampleChild() { MyLong = -123456 }
            };
            var c = e.CloneLazinatorTyped();
            c.Item1 = c.Item2;
            c.Item1.MyLong = -987;
            var c2 = c.CloneLazinatorTyped();
            c2.Item1.MyLong.Should().Be(-987);
            c2.Item2.MyLong.Should().Be(-987);
        }

        [Fact]
        public void ChangeAfterCopyingAffectsSource()
        {
            LazinatorTuple<ExampleChild, ExampleChild> e = new LazinatorTuple<ExampleChild, ExampleChild>()
            {
                Item1 = new ExampleChild(),
                Item2 = new ExampleChild() { MyLong = -123456 }
            };
            var c = e.CloneLazinatorTyped();
            var c2 = e.CloneLazinatorTyped();
            c.Item1 = c2.Item2;
            c2.Item2.MyLong = 543;
            var c3 = c2.CloneLazinatorTyped();
            c3.Item2.MyLong.Should().Be(543);
        }

    }
}
