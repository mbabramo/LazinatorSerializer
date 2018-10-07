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
using Lazinator.Buffers;

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
        public void ParentsWorksWithGenericStruct()
        {
            LazinatorTuple<WInt, WInt> e = new LazinatorTuple<WInt, WInt>()
            {
                Item1 = new WInt(1),
                Item2 = new WInt(2)
            };
            e.Item1.LazinatorParents.LastAdded.Should().Be(e);
            var c = e.CloneLazinatorTyped();
            var d = e.CloneLazinatorTyped();
            d.Item1 = c.Item2;
            d.Item1.LazinatorParents.Count.Should().Be(1);
            d.Item1.LazinatorParents.LastAdded.Should().Be(d);
            d.Item2.LazinatorParents.Count.Should().Be(1);
            d.Item2.LazinatorParents.LastAdded.Should().Be(d);
            c.Item1.LazinatorParents.Count.Should().Be(1);
            c.Item1.LazinatorParents.LastAdded.Should().Be(c);
            c.Item2.LazinatorParents.Count.Should().Be(1);
            c.Item2.LazinatorParents.LastAdded.Should().Be(c);
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
        public void ChangeToObjectInTwoHierarchiesAffectsBoth()
        {
            LazinatorTuple<ExampleChild, ExampleChild> e = new LazinatorTuple<ExampleChild, ExampleChild>()
            {
                Item1 = new ExampleChild(),
                Item2 = new ExampleChild() { MyLong = -123456 }
            };
            var c = e.CloneLazinatorTyped();
            c.Item1 = e.Item1;
            c.Item1.LazinatorParents.Count.Should().Be(2);
            c.Item1.MyLong = 101;
            var c2 = c.CloneLazinatorTyped();
            var c3 = e.CloneLazinatorTyped();
            c2.Item1.MyLong.Should().Be(101);
            c3.Item1.MyLong.Should().Be(101);
        }

        [Fact]
        public void ParentRemovedWhenObjectDetached()
        {
            LazinatorTuple<ExampleChild, ExampleChild> e = new LazinatorTuple<ExampleChild, ExampleChild>()
            {
                Item1 = new ExampleChild(),
                Item2 = new ExampleChild() { MyLong = -123456 }
            };
            var c = e.CloneLazinatorTyped();
            var item1Orig = c.Item1;
            item1Orig.LazinatorParents.Count.Should().Be(1);
            c.Item1 = new ExampleChild();
            item1Orig.LazinatorParents.Count.Should().Be(0);

            // same thing, but going from two parents to 1
            e = new LazinatorTuple<ExampleChild, ExampleChild>()
            {
                Item1 = new ExampleChild(),
                Item2 = new ExampleChild() { MyLong = -123456 }
            };
            var e2 = new LazinatorTuple<ExampleChild, ExampleChild>()
            {
                Item1 = new ExampleChild(),
                Item2 = new ExampleChild() { MyLong = -123456 }
            };
            c = e.CloneLazinatorTyped();
            e2.Item1 = c.Item1;
            item1Orig = c.Item1;
            item1Orig.LazinatorParents.Count.Should().Be(2);
            c.Item1 = new ExampleChild();
            item1Orig.LazinatorParents.Count.Should().Be(1);
        }

        [Fact]
        public void ParentSetForDefaultItem()
        {
            LazinatorTuple<WInt, WInt> e = new LazinatorTuple<WInt, WInt>()
            {
                Item1 = 0
            };
            e.Item1.LazinatorParents.LastAdded.Should().Be(e);
            e.Item2.LazinatorParents.LastAdded.Should().Be(e);
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

        [Fact]
        public void GetRootsAndAncestors()
        {
            LazinatorTuple<ExampleChild, ExampleChild> e1 = new LazinatorTuple<ExampleChild, ExampleChild>()
            {
                Item1 = new ExampleChild() { MyWrapperContainer = new WrapperContainer() { WrappedInt = 3 } },
                Item2 = null
            };
            var startingPoint = e1.Item1.MyWrapperContainer.WrappedInt;
            startingPoint.GetSoleRoot().Should().Be(e1);
            startingPoint.GetPrincipalRoot().Should().Be(e1);
            startingPoint.GetSoleClosestAncestorOfType<LazinatorTuple<ExampleChild, ExampleChild>>().Should().Be(e1);
            var closestAncestors = startingPoint.GetAllClosestAncestorsOfType<LazinatorTuple<ExampleChild, ExampleChild>>().ToList();
            closestAncestors.Count().Should().Be(1);
            closestAncestors[0].Should().Be(e1);

            LazinatorTuple<ExampleChild, ExampleChild> e2 = new LazinatorTuple<ExampleChild, ExampleChild>()
            {
                Item1 = null,
                Item2 = e1.Item1
            };
            startingPoint = e2.Item2.MyWrapperContainer.WrappedInt;
            var roots = startingPoint.GetAllRoots().ToList();
            roots.Count().Should().Be(2);
            roots[0].Should().Be(e2);
            roots[1].Should().Be(e1);
            startingPoint.GetAllClosestAncestorsOfType<LazinatorTuple<ExampleChild, ExampleChild>>();
            closestAncestors = startingPoint.GetAllClosestAncestorsOfType<LazinatorTuple<ExampleChild, ExampleChild>>().ToList();
            closestAncestors.Count().Should().Be(2);
            closestAncestors[0].Should().Be(e2);
            closestAncestors[1].Should().Be(e1);
            Action a = () => { var result = startingPoint.GetSoleClosestAncestorOfType<LazinatorTuple<ExampleChild, ExampleChild>>(); };
            a.Should().Throw<Exception>();

            e1.GetSoleClosestAncestorOfType<LazinatorTuple<ExampleChild, ExampleChild>>().Should().Be(null); // don't count node itself
        }

        [Fact]
        public void TopNodesComparisonWorks()
        {
            ExampleChild c1 = new ExampleChild() { MyShort = 3 };
            ExampleChild c2 = new ExampleChild() { MyShort = 3 };
            LazinatorUtilities.TopNodesOfHierarchyEqual(c1, c2, out string comparison).Should().BeTrue();
            c2.MyShort = 5;
            LazinatorUtilities.TopNodesOfHierarchyEqual(c1, c2, out comparison).Should().BeFalse();

            LazinatorTuple<ExampleChild, ExampleChild> e1 = new LazinatorTuple<ExampleChild, ExampleChild>()
            {
                Item1 = new ExampleChild() { MyWrapperContainer = new WrapperContainer() { WrappedInt = 3 } },
                Item2 = null
            };
            var e2 = e1.CloneLazinatorTyped();
            LazinatorUtilities.TopNodesOfHierarchyEqual(e1, e2, out comparison).Should().BeTrue();
            e2.Item1.MyWrapperContainer.WrappedInt = 5;
            LazinatorUtilities.TopNodesOfHierarchyEqual(e1, e2, out comparison).Should().BeTrue(); // top node is still equal

            LazinatorList<ExampleChild> l1 = new LazinatorList<ExampleChild>()
            {
                new ExampleChild() { MyWrapperContainer = new WrapperContainer() { WrappedInt = 3 } }
            };
            LazinatorList<ExampleChild> l2 = new LazinatorList<ExampleChild>()
            {
                new ExampleChild() { MyWrapperContainer = new WrapperContainer() { WrappedInt = 6 } }
            };
            LazinatorUtilities.TopNodesOfHierarchyEqual(l1, l2, out comparison).Should().BeTrue();
            l2.Add(null);
            LazinatorUtilities.TopNodesOfHierarchyEqual(l1, l2, out comparison).Should().BeFalse(); // number of elements differs
        }

        [Fact]
        public void BuffersDisposedJointly()
        {
            Example e = GetTypicalExample(); // no memory backing yet
            e = e.CloneLazinatorTyped(); // now there is a memory buffer
            e.MyChild1.MyLong = -342356;
            e.LazinatorMemoryStorage.OwnedMemory.Should().Be(e.MyChild1.LazinatorMemoryStorage.OwnedMemory);
            e.MyChild1.EnsureLazinatorMemoryUpToDate();
            e.LazinatorMemoryStorage.OwnedMemory.Should().NotBe(e.MyChild1.LazinatorMemoryStorage.OwnedMemory);
            e.LazinatorMemoryStorage.Dispose();
            Action a = () =>
            {
                var m = e.MyChild1.LazinatorMemoryStorage.Memory;
                m.Span[0] = 1;
            };
            a.Should().Throw<ObjectDisposedException>();
        }

        [Fact]
        public void BuffersDisposedJointly_WhenChildDisposed()
        {
            Example e = GetTypicalExample(); // no memory backing yet
            e = e.CloneLazinatorTyped(); // now there is a memory buffer
            e.MyChild1.MyLong = -342356;
            e.LazinatorMemoryStorage.OwnedMemory.Should().Be(e.MyChild1.LazinatorMemoryStorage.OwnedMemory);
            JointlyDisposableMemory.Round = 1; // DEBUG
            e.MyChild1.EnsureLazinatorMemoryUpToDate();
            e.LazinatorMemoryStorage.OwnedMemory.Should().NotBe(e.MyChild1.LazinatorMemoryStorage.OwnedMemory);
            e.MyChild1.LazinatorMemoryStorage.Dispose();
            Action a = () =>
            {
                var m = e.LazinatorMemoryStorage.Memory;
                m.Span[0] = 1;
            };
            a.Should().Throw<ObjectDisposedException>();
        }

        [Fact]
        public void BuffersDisposedJointly_DisposingOriginalDisposesClone()
        {
            Example e = GetTypicalExample(); // no memory backing yet
            e = e.CloneLazinatorTyped(); // now there is a memory buffer
            var e2 = e.CloneLazinatorTyped();
            e.LazinatorMemoryStorage.Dispose();
            Action a = () =>
            {
                var m = e2.LazinatorMemoryStorage.Memory;
                m.Span[0] = 1;
            };
            a.Should().Throw<ObjectDisposedException>();
        }

        [Fact]
        public void BuffersDisposedJointly_DisposeCloneIndependently()
        {
            Example e = GetTypicalExample(); // no memory backing yet
            e = e.CloneLazinatorTyped(); // now there is a memory buffer
            var e2 = e.CloneLazinatorTyped();
            e2.LazinatorMemoryStorage.DisposeIndependently();
            e.LazinatorMemoryStorage.Dispose();
            Action a = () =>
            {
                var m = e2.LazinatorMemoryStorage.Memory;
                m.Span[0] = 1;
            };
            a.Should().NotThrow<ObjectDisposedException>();
        }

        [Fact]
        public void BuffersDisposedJointly_DisposingCloneDisposesOriginal()
        {
            Example e = GetTypicalExample(); // no memory backing yet
            e = e.CloneLazinatorTyped(); // now there is a memory buffer
            var e2 = e.CloneLazinatorTyped();
            e2.LazinatorMemoryStorage.Dispose();
            Action a = () =>
            {
                var m = e.LazinatorMemoryStorage.Memory;
                m.Span[0] = 1;
            };
            a.Should().Throw<ObjectDisposedException>();
        }

        [Fact]
        public void CanRepeatedlyEnsureMemoryUpToDate()
        {
            Example e = GetTypicalExample();
            int repetitions = 10000;
            for (int i = 0; i < repetitions; i++)
            {
                JointlyDisposableMemory.Round = i;
                e.MyChild1.MyLong = i;
                e.EnsureLazinatorMemoryUpToDate();
            }
        }

    }
}
