using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Structs;
using Lazinator.Collections.Dictionary;
using Lazinator.Buffers;
using LazinatorTests.Examples.NonAbstractGenerics;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.Collections;

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
        public void BuffersDisposedJointly_UpdatingDoesntDisposeClone()
        {
            Example e = GetTypicalExample(); // no memory backing yet
            e = e.CloneLazinatorTyped(); // now there is a memory buffer
            var e2 = e.CloneLazinatorTyped();
            e.MyChild1.MyShort = 52;
            e.MyChild1.EnsureLazinatorMemoryUpToDate();
            var f = e.MyChild1.CloneLazinatorTyped();
            e.MyChild1.MyShort = 53;
            e.MyChild1.EnsureLazinatorMemoryUpToDate();
            Action a = () =>
            {
                var m = f.LazinatorMemoryStorage.Memory;
                m.Span[0] = 1;
            };
            a.Should().NotThrow<ObjectDisposedException>();
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

        public enum RepetitionsToMutate
        {
            All,
            None,
            Even,
            Odd
        }

        private bool MutateThisRepetition(int i, RepetitionsToMutate o)
        {
            switch (o)
            {
                case RepetitionsToMutate.All:
                    return true;
                case RepetitionsToMutate.None:
                    return false;
                case RepetitionsToMutate.Even:
                    return i % 2 == 0;
                case RepetitionsToMutate.Odd:
                    return i % 2 == 1;
                default:
                    throw new NotSupportedException();
            }
        }

        public static IEnumerable<object[]> CanRepeatedlyData()
        {
            foreach (bool makeChildUpToDate in new bool[] { true, false })
                foreach (bool makeParentUpToDate in new bool[] { true, false })
                    foreach (RepetitionsToMutate mutateParent in new RepetitionsToMutate[] { RepetitionsToMutate.All, RepetitionsToMutate.None, RepetitionsToMutate.Even, RepetitionsToMutate.Odd })
                        foreach (RepetitionsToMutate mutateChild in new RepetitionsToMutate[] { RepetitionsToMutate.All, RepetitionsToMutate.None, RepetitionsToMutate.Even, RepetitionsToMutate.Odd })
                            foreach (bool doNotAutomaticallyReturnToPool in new bool[] { true, false })
                                yield return new object[] { makeChildUpToDate, makeParentUpToDate, mutateParent, mutateChild, doNotAutomaticallyReturnToPool };
        }

        [Theory]
        [MemberData(nameof(CanRepeatedlyData))]
        public void CanRepeatedlyEnsureMemoryUpToDate(bool makeChildUpToDate, bool makeParentUpToDate, RepetitionsToMutate mutateParent, RepetitionsToMutate mutateChild, bool doNotAutomaticallyReturnToPool)
        {
            Example e = GetTypicalExample();
            e.MyChild1 = new ExampleChildInherited() { MyInt = 25 };
            Example c1 = null, c2 = null, c3 = null, c4 = null; // early clones -- make sure unaffected
            int repetitions = 8;
            Random r = new Random();
            long randLong = 0;
            short randShort = 0;
            for (int i = 0; i < repetitions; i++)
            {
                if (doNotAutomaticallyReturnToPool && e.LazinatorMemoryStorage != null)
                    e.LazinatorMemoryStorage.DoNotAutomaticallyReturnToPool();
                if (i == 0)
                {
                    e.MyChild1.MyLong = 0;
                    e.MyChild1.MyShort = 0;
                    ((ExampleChildInherited)e.MyChild1).MyInt = 0;
                }
                if (i == 5)
                {
                    c1 = e.CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.LinkedBuffer);
                    c2 = e.CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.IndependentBuffers);
                    c3 = e.CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.SharedBuffer);
                    c4 = e.CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.NoBuffer);
                }
                if (MutateThisRepetition(i, mutateParent))
                    e.MyBool = !e.MyBool;
                if (MutateThisRepetition(i, mutateChild))
                {
                    if (i > 2)
                    {
                        e.MyChild1.MyLong.Should().Be(randLong);
                        e.MyChild1.MyShort.Should().Be(randShort);
                        ((ExampleChildInherited)e.MyChild1).MyInt.Should().Be(randShort);
                    }
                    unchecked
                    {
                        randLong = r.Next(0, 2) == 0 ? r.Next(1, 100) : r.Next();
                        randShort = (short)(r.Next(0, 2) == 0 ? r.Next(1, 100) : r.Next());
                    }
                    e.MyChild1.MyLong = randLong;
                    e.MyChild1.MyShort = randShort;
                    ((ExampleChildInherited)e.MyChild1).MyInt = randShort;
                }
                if (makeChildUpToDate)
                {
                    if (doNotAutomaticallyReturnToPool && e.MyChild1.LazinatorMemoryStorage != null)
                        e.MyChild1.LazinatorMemoryStorage.DoNotAutomaticallyReturnToPool();
                    e.MyChild1.EnsureLazinatorMemoryUpToDate();
                }
                if (makeParentUpToDate)
                    e.EnsureLazinatorMemoryUpToDate();
            }
            foreach (Example c in new Example[] { c1, c2, c3, c4 })
            {
                c.MyChild2.MyLong = -3; // make sure early clone still works
            }
        }


        const int dictsize = 5;


        [Fact]
        public void CanCloneListOfStructsAndThenEnsureUpToDate()
        {
            // Note: This works because of IsStruct parameter in ReplaceBuffer. Without that parameter, the last call would lead to disposal of memory still needed.
            var l = new LazinatorList<WInt>() { 1 };
            var l2 = l.CloneLazinatorTyped();
            var l3 = l.CloneLazinatorTyped();
            l.EnsureLazinatorMemoryUpToDate();
        }

        [Fact]
        public void CanCloneListOfLazinatorsAndThenEnsureUpToDate()
        {
            var l = new LazinatorList<Example>() { GetTypicalExample(), GetTypicalExample() };
            var l2 = l.CloneLazinatorTyped();
            var l3 = l.CloneLazinatorTyped();
            l.EnsureLazinatorMemoryUpToDate();
        }

        [Fact]
        public void CanCloneDictionaryAndThenEnsureUpToDate()
        {
            // Note: This is a sequence that proved problematic in the next test
            var d = GetDictionary();
            var d2 = d.CloneLazinatorTyped();
            var d3 = d.CloneLazinatorTyped();
            d.EnsureLazinatorMemoryUpToDate();
        }

        [Fact]
        public void ObjectDisposedExceptionThrownOnItemRemovedFromHierarchy()
        {
            LazinatorDictionary<WInt, Example> d = GetDictionary();
            //same effect if both of the following lines are included
            //d.EnsureLazinatorMemoryUpToDate();
            //d[0].MyChar = 'q';
            d[0].EnsureLazinatorMemoryUpToDate(); // OwnedMemory has allocation ID of 0. 
            d.EnsureLazinatorMemoryUpToDate(); // OwnedMemory for this and d[0] share allocation ID of 1
            Example e = d[0];
            d[0] = GetTypicalExample();
            d.EnsureLazinatorMemoryUpToDate(); // allocation ID 1 disposed.
            Action a = () => { var x = e.MyChild1.LazinatorMemoryStorage.OwnedMemory.Memory; }; // note that error occurs only when looking at underlying memory
            a.Should().Throw<ObjectDisposedException>();
        }

        [Fact]
        public void ObjectDisposedExceptionAvoidedByCloneToIndependentBuffer()
        {
            LazinatorDictionary<WInt, Example> d = GetDictionary();
            d[0].EnsureLazinatorMemoryUpToDate(); // OwnedMemory has allocation ID of 0. 
            d.EnsureLazinatorMemoryUpToDate(); // OwnedMemory for this and d[0] share allocation ID of 1
            Example e = d[0].CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.IndependentBuffers);
            d[0] = GetTypicalExample();
            d.EnsureLazinatorMemoryUpToDate(); // allocation ID 1 disposed.
            Action a = () => { var x = e.MyChild1; };
            a.Should().NotThrow<ObjectDisposedException>();
        }

        [Fact]
        public void CanAccessCopiedItemAfterEnsureUpToDate()
        {
            LazinatorDictionary<WInt, Example> d = GetDictionary();
            d.EnsureLazinatorMemoryUpToDate(); // OwnedMemory for this and d[0] share allocation ID of 0. As the original source, this will not be automatically disposed. 
            Example e = d[0];
            d[0] = GetTypicalExample();
            d.EnsureLazinatorMemoryUpToDate(); // allocation ID 0 is not disposed.
            var x = e.MyChild1;
        }

        [Fact]
        public void UpdateStoredBufferOnInherited()
        {
            Example e = GetTypicalExample();
            e.MyChild1 = GetExampleChild(3); // inherited child
            ((ExampleChildInherited)e.MyChild1).MyGrandchildInInherited = new ExampleGrandchild() { MyInt = 139 };
            e = e.CloneLazinatorTyped();
            ((ExampleChildInherited)e.MyChild1).MyInt = 137;
            ((ExampleChildInherited)e.MyChild1).MyGrandchildInInherited.MyInt = 141;
            e.MyChild1.EnsureLazinatorMemoryUpToDate();
            e.EnsureLazinatorMemoryUpToDate();
            ((ExampleChildInherited)e.MyChild1).MyInt.Should().Be(137);
            ((ExampleChildInherited)e.MyChild1).MyGrandchildInInherited.MyInt.Should().Be(141);
        }

        [Fact]
        public void BuffersUpdateInTandem()
        {
            Example e = GetTypicalExample().CloneLazinatorTyped();
            e.MyChild1.MyLong = 3;
            var y = e.MyChild1.MyExampleGrandchild;
            e.MyChild2.MyExampleGrandchild.MyInt = 6;
            ConfirmBuffersUpdateInTandem(e);
            ConfirmBuffersUpdateInTandem(e);
        }

        [Fact]
        public void BuffersUpdateInTandem_Inherited()
        {

            Example e = GetTypicalExample().CloneLazinatorTyped();
            e.MyChild1 = GetExampleChild(3); // inherited child
            ((ExampleChildInherited)e.MyChild1).MyGrandchildInInherited = new ExampleGrandchild() { MyInt = 139 };
            e.MyChild1.EnsureLazinatorMemoryUpToDate();
            ConfirmBuffersUpdateInTandem(e);
            ConfirmBuffersUpdateInTandem(e);
        }

        [Fact]
        public void BuffersUpdateInTandem_Struct()
        {
            ContainerForExampleStructWithoutClass e = new ContainerForExampleStructWithoutClass() { ExampleStructWithoutClass = new ExampleStructWithoutClass() { MyInt = 19 } };
            e = e.CloneLazinatorTyped();
            ConfirmBuffersUpdateInTandem(e);
            e.MyInt = 29; // make it dirty but leave child struct clean
            ConfirmBuffersUpdateInTandem(e);
        }

        [Fact]
        public void BuffersUpdateInTandem_OpenGeneric_Struct()
        {
            OpenGenericStayingOpenContainer e = new OpenGenericStayingOpenContainer();
            e.ClosedGenericFloat = new OpenGeneric<WFloat>() { MyT = 3.45F };
            e.ClosedGenericInterface = new OpenGeneric<IExampleChild>() { MyT = GetExampleChild(1) };
            e = e.CloneLazinatorTyped();
            ConfirmBuffersUpdateInTandem(e);
            e.ClosedGenericInterface.MyT.MyLong = 29; // make it dirty but leave child struct clean
            ConfirmBuffersUpdateInTandem(e);
        }

        [Fact]
        public void BuffersUpdateInTandem_GenericFromBase_Struct()
        {
            GenericFromBase<WFloat> e = new GenericFromBase<WFloat>();
            e.MyT = new WFloat(8.65F);
            e = e.CloneLazinatorTyped();
            ConfirmBuffersUpdateInTandem(e);
            e.MyInt = 29; // make it dirty but leave child struct clean
            ConfirmBuffersUpdateInTandem(e);
        }

        [Fact]
        public void BuffersUpdateInTandem_LazinatorList()
        {
            LazinatorListContainer e = new LazinatorListContainer();
            e.MyList = new LazinatorList<ExampleChild>();
            e.MyList.Add(GetExampleChild(1));
            e.MyList[0].MyExampleGrandchild = new ExampleGrandchild() { MyInt = 5 };
            e.MyList.Add(GetExampleChild(1));
            e.MyList[1].MyExampleGrandchild = new ExampleGrandchild() { MyInt = 5 };
            e = e.CloneLazinatorTyped();
            e.MyList[1].MyExampleGrandchild.MyInt = 6;
            e.MyList[1].EnsureLazinatorMemoryUpToDate(); // generate a new buffer in a list member
            ConfirmBuffersUpdateInTandem(e);
            e.MyInt = 17; // keep list clean while making container dirty
            ConfirmBuffersUpdateInTandem(e);
        }

        private static void ConfirmBuffersUpdateInTandem(ILazinator itemToUpdate)
        {
            itemToUpdate.EnsureLazinatorMemoryUpToDate();
            var allocationID = ((ExpandableBytes)itemToUpdate.LazinatorMemoryStorage.OwnedMemory).AllocationID;
            List<ILazinator> descendants = itemToUpdate.EnumerateAllNodes().ToList();
            for (int i = 0; i < descendants.Count; i++) 
            {
                ILazinator lazinator = descendants[i];
                ExpandableBytes b = lazinator.LazinatorMemoryStorage.OwnedMemory as ExpandableBytes;
                if (b != null)
                    b.AllocationID.Should().Be(allocationID);
            }
        }

        private LazinatorDictionary<WInt, Example> GetDictionary()
        {
            LazinatorDictionary<WInt, Example> d = new LazinatorDictionary<WInt, Example>();
            for (int i = 0; i < dictsize; i++)
            {
                d[i] = GetTypicalExample();
            }
            return d;
        }
    }
}
