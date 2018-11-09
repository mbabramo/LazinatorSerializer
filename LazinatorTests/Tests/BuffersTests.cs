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
    public class BuffersTests : SerializationDeserializationTestBase
    {

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
            int repetitions = 4; // DEBUG 8;
            Random r = new Random();
            long randLong = 0;
            short randShort = 0;
            for (int i = 0; i < repetitions; i++)
            {
                if (doNotAutomaticallyReturnToPool && e.LazinatorMemoryStorage != null)
                    e.LazinatorMemoryStorage.LazinatorShouldNotReturnToPool();
                if (i == 0)
                {
                    e.MyChild1.MyLong = 0;
                    e.MyChild1.MyShort = 0;
                    ((ExampleChildInherited)e.MyChild1).MyInt = 0;
                }
                if (i == 3) // DEBUG 5)
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
                        e.MyChild1.LazinatorMemoryStorage.LazinatorShouldNotReturnToPool();
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

        [Fact]
        public void DEBUG()
        {
            CanRepeatedlyEnsureMemoryUpToDate(true, false, RepetitionsToMutate.All, RepetitionsToMutate.All, true);
            GC.Collect();
            System.Diagnostics.Debug.WriteLine(ExpandableBytes.PoolTrackerSummary());
        }

        [Fact]
        public void EnsureLazinatorSimplifiedTest()
        {
            Example e = GetTypicalExample();
            e.MyChild1 = new ExampleChildInherited() { MyInt = 25 };
            Example c1 = null; // early clones -- make sure unaffected
            int repetitions = 4; 
            e.MyChild1.MyLong = 0;
            e.MyChild1.MyShort = 0;
            ((ExampleChildInherited)e.MyChild1).MyInt = 0;
            for (int i = 0; i < repetitions; i++)
            {
                if (i == 3)
                {
                    c1 = e.CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.LinkedBuffer);
                }
                e.MyBool = !e.MyBool;
                e.EnsureLazinatorMemoryUpToDate();
            }
            c1.MyChild2.MyLong = -3; // make sure we can access it
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
        public void RemoveBufferWorks_Example()
        {
            Example e = GetTypicalExample();
            e.EnsureLazinatorMemoryUpToDate();
            e.LazinatorMemoryStorage.Should().NotBeNull();
            var x = e.MyChild1.MyExampleGrandchild.MyInt;
            e.MyChild1.MyExampleGrandchild.MyInt++;
            e.MyChild1.MyExampleGrandchild.LazinatorMemoryStorage.Should().NotBeNull();
            e.RemoveBufferInHierarchy();
            e.LazinatorMemoryStorage.Should().BeNull();
            e.MyChild1.MyExampleGrandchild.LazinatorMemoryStorage.Should().BeNull();
            e.MyChild1.MyExampleGrandchild.MyInt.Should().Be(x + 1);

            e.EnsureLazinatorMemoryUpToDate();
            e.MyChild1.MyExampleGrandchild.LazinatorMemoryStorage.Should().NotBeNull();
            e.MyChild1.MyExampleGrandchild.MyInt.Should().Be(x + 1);
        }

        [Fact]
        public void RemoveBufferWorks_ExampleStructContainer()
        {
            ExampleStructContainerContainingClasses e = new ExampleStructContainerContainingClasses();
            e.IntWrapper++;
            e.EnsureLazinatorMemoryUpToDate();
            e.IntWrapper.LazinatorMemoryStorage.Should().NotBeNull();
            var x = e.IntWrapper;
            e.IntWrapper++;
            e.IntWrapper.LazinatorMemoryStorage.Should().BeNull();
            e.RemoveBufferInHierarchy();
            e.LazinatorMemoryStorage.Should().BeNull();
            e.IntWrapper.LazinatorMemoryStorage.Should().BeNull();
            e.IntWrapper.WrappedValue.Should().Be(x + 1);

            e.EnsureLazinatorMemoryUpToDate();
            e.IntWrapper.LazinatorMemoryStorage.Should().NotBeNull();
            e.IntWrapper.WrappedValue.Should().Be(x + 1);
        }

        [Fact]
        public void RemoveBufferWorks_DotNetList()
        {
            DotNetList_Lazinator lazinator = new DotNetList_Lazinator()
            {
                MyListSerialized = new List<ExampleChild>()
                {
                    GetExampleChild(1),
                    GetExampleChild(2),
                    null
                }
            };
            lazinator.EnsureLazinatorMemoryUpToDate();
            lazinator.LazinatorMemoryStorage.Should().NotBeNull();
            var x = lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt;
            lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt++;
            lazinator.MyListSerialized_Dirty = true;
            lazinator.MyListSerialized[0].MyExampleGrandchild.LazinatorMemoryStorage.Should().NotBeNull();

            lazinator.RemoveBufferInHierarchy();
            lazinator.LazinatorMemoryStorage.Should().BeNull();
            lazinator.MyListSerialized[0].MyExampleGrandchild.LazinatorMemoryStorage.Should().BeNull();
            lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt.Should().Be(x + 1);

            lazinator.EnsureLazinatorMemoryUpToDate();
            lazinator.MyListSerialized[0].MyExampleGrandchild.LazinatorMemoryStorage.Should().NotBeNull();
            lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt.Should().Be(x + 1);

            lazinator.EnsureLazinatorMemoryUpToDate();
            LazinatorMemory lazinatorMemoryStorage = lazinator.MyListSerialized[0].MyExampleGrandchild.LazinatorMemoryStorage;
            lazinatorMemoryStorage.Should().NotBeNull();
            lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt.Should().Be(x + 1);

            lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt++;
            lazinator.MyListSerialized_Dirty = true;
            lazinator.EnsureLazinatorMemoryUpToDate();
            lazinator.MyListSerialized[0].MyExampleGrandchild.LazinatorMemoryStorage.Should().NotBeNull();
            (lazinator.MyListSerialized[0].MyExampleGrandchild.LazinatorMemoryStorage == lazinatorMemoryStorage).Should()
                .BeFalse();
            lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt.Should().Be(x + 2);
        }

        [Fact]
        public void RemoveBufferWorks_DotNetHashSet()
        {
            DotNetHashSet_Lazinator lazinator = new DotNetHashSet_Lazinator()
            {
                MyHashSetSerialized = new HashSet<ExampleChild>()
                {
                    GetExampleChild(1),
                    GetExampleChild(2),
                }
            };
            lazinator.EnsureLazinatorMemoryUpToDate();
            lazinator.LazinatorMemoryStorage.Should().NotBeNull();
            var x = lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt;
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt++;
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.LazinatorMemoryStorage.Should().NotBeNull();

            lazinator.RemoveBufferInHierarchy();
            lazinator.LazinatorMemoryStorage.Should().BeNull();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.LazinatorMemoryStorage.Should().BeNull();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt.Should().Be(x + 1);

            lazinator.EnsureLazinatorMemoryUpToDate();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.LazinatorMemoryStorage.Should().NotBeNull();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt.Should().Be(x + 1);

            lazinator.EnsureLazinatorMemoryUpToDate();
            LazinatorMemory lazinatorMemoryStorage = lazinator.MyHashSetSerialized.First().MyExampleGrandchild.LazinatorMemoryStorage;
            lazinatorMemoryStorage.Should().NotBeNull();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt.Should().Be(x + 1);

            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt++;
            lazinator.EnsureLazinatorMemoryUpToDate();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.LazinatorMemoryStorage.Should().NotBeNull();
            (lazinator.MyHashSetSerialized.First().MyExampleGrandchild.LazinatorMemoryStorage == lazinatorMemoryStorage).Should()
                .BeFalse();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt.Should().Be(x + 2);
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
        public void CloneMiddleOfHierarchy()
        {
            Example e = GetTypicalExample();
            e.MyChild1.MyShort = 5;
            e = e.CloneLazinatorTyped();
            e.MyChild1.MyShort = 6;
            e.MyChild1.EnsureLazinatorMemoryUpToDate();
            var f = e.MyChild1.CloneLazinatorTyped();
            e.MyChar = '5';
            var e2 = e.CloneLazinatorTyped();
            var x = e2.MyChild1.MyShort;
            x.Should().Be(6);
        }

        [Fact]
        public void UpdateBufferForDeserialized()
        {
            Example e = GetTypicalExample();
            e.MyChild1.MyShort = 5;
            e = e.CloneLazinatorTyped();
            e.MyChild1.MyShort = 6;
            UpdateStoredBufferFromExisting(e);
            e.MyChar = '5';
            var e2 = e.CloneLazinatorTyped();
            var x = e2.MyChild1.MyShort;
            x.Should().Be(6);
        }

        [Fact]
        public void UpdateBufferForDeserialized_LazinatorList()
        {
            LazinatorListContainer c = GetLazinatorListContainer();
            c.MyList[0].MyExampleGrandchild.MyInt = 200;
            UpdateStoredBufferFromExisting(c);
            var item = c.MyList[0].CloneLazinatorTyped();
            var c2 = c.CloneLazinatorTyped();
            c.MyList[0].MyExampleGrandchild.MyInt.Should().Be(200);
        }


        [Fact]
        public void UpdateBufferForDeserialized_LazinatorList_Struct()
        {
            LazinatorListContainer c = new LazinatorListContainer() { MyStructList = new LazinatorList<WByte>() };
            c.MyStructList.Add(3);
            c.MyStructList.Add(4);
            c = c.CloneLazinatorTyped();
            var x = c.MyStructList[0];
            c.MyInt = -234;
            UpdateStoredBufferFromExisting(c);
            var storageOverall = c.LazinatorMemoryStorage.OwnedMemory as ExpandableBytes;
            var storageItem = c.MyStructList[0].LazinatorMemoryStorage.OwnedMemory as ExpandableBytes;
            storageOverall.AllocationID.Should().Be(storageItem.AllocationID);
            var item = c.MyStructList[0].CloneLazinatorTyped();
            var c2 = c.CloneLazinatorTyped();
            item.WrappedValue.Should().Be(3);
        }

        private static void UpdateStoredBufferFromExisting(ILazinator e)
        {
            e.EnsureLazinatorMemoryUpToDate();
            var buffer = new Memory<byte>(e.LazinatorMemoryStorage.Memory.Span.ToArray());
            BinaryBufferWriter b = new BinaryBufferWriter();
            b.Write(buffer.Span);
            e.UpdateStoredBuffer(ref b, 0, buffer.Span.Length /* DEBUG -- check */, IncludeChildrenMode.IncludeAllChildren, true);
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
            LazinatorListContainer e = GetLazinatorListContainer();
            e.MyList[1].MyExampleGrandchild.MyInt = 6;
            e.MyList[1].EnsureLazinatorMemoryUpToDate(); // generate a new buffer in a list member
            ConfirmBuffersUpdateInTandem(e);
            e.MyInt = 17; // keep list clean while making container dirty
            ConfirmBuffersUpdateInTandem(e);
        }

        private LazinatorListContainer GetLazinatorListContainer()
        {
            LazinatorListContainer container = new LazinatorListContainer();
            container.MyList = new LazinatorList<ExampleChild>();
            container.MyList.Add(GetExampleChild(1));
            container.MyList[0].MyExampleGrandchild = new ExampleGrandchild() { MyInt = 5 };
            container.MyList.Add(GetExampleChild(1));
            container.MyList[1].MyExampleGrandchild = new ExampleGrandchild() { MyInt = 5 };
            container = container.CloneLazinatorTyped();
            return container;
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

        [Fact]
        public void UpdateBufferDoesntAffectHash()
        {
            Example e = GetTypicalExample();
            e.MyChild1.MyExampleGrandchild = new ExampleGrandchild() { MyInt = 17 };
            e = e.CloneLazinatorTyped();
            e.MyChar = 'b';
            var x = e.MyChild1.MyExampleGrandchild.MyInt;
            e.EnsureLazinatorMemoryUpToDate();
            uint h = e.MyChild1.MyExampleGrandchild.GetBinaryHashCode32();
            uint h2 = (new ExampleGrandchild() { MyInt = 17 }).GetBinaryHashCode32();
            h.Should().Be(h2);

        }
    }
}
