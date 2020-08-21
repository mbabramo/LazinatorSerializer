using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LazinatorCollections;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Structs;
using LazinatorCollections.Dictionary;
using Lazinator.Buffers;
using LazinatorTests.Examples.NonAbstractGenerics;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.Collections;
using LazinatorTests.Examples.Tuples;

namespace LazinatorTests.Tests
{
    public class BuffersTests : SerializationDeserializationTestBase
    {

        [Fact]
        public void BufferOfCloneIsIndependent()
        {
            Example e = GetTypicalExample(); // no memory backing yet
            e = e.CloneLazinatorTyped(); // now there is a memory buffer
            e.MyChild1.MyLong = -342356;
            e.LazinatorMemoryStorage.OwnedMemory.Should().Be(e.MyChild1.LazinatorMemoryStorage.OwnedMemory);
            e.MyChild1.UpdateStoredBuffer();
            e.LazinatorMemoryStorage.OwnedMemory.Should().NotBe(e.MyChild1.LazinatorMemoryStorage.OwnedMemory);
            e.LazinatorMemoryStorage.Dispose();
            Action a = () =>
            {
                var m = e.MyChild1.LazinatorMemoryStorage.Memory;
                m.Span[0] = 1;
            };
            a.Should().NotThrow<ObjectDisposedException>();
        }

        [Fact]
        public void BuffersDisposedJointly_UpdatingDoesntDisposeClone()
        {
            Example e = GetTypicalExample(); // no memory backing yet
            e = e.CloneLazinatorTyped(); // now there is a memory buffer
            var e2 = e.CloneLazinatorTyped();
            e.MyChild1.MyShort = 52;
            e.MyChild1.UpdateStoredBuffer();
            var f = e.MyChild1.CloneLazinatorTyped();
            e.MyChild1.MyShort = 53;
            e.MyChild1.UpdateStoredBuffer();
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
            e.LazinatorMemoryStorage.Dispose();
            Action a = () =>
            {
                var m = e2.LazinatorMemoryStorage.Memory;
                m.Span[0] = 1;
            };
            a.Should().NotThrow<ObjectDisposedException>();
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
            Example c1 = null, c2 = null; // early clones -- make sure unaffected
            int repetitions = 8;
            Random r = new Random();
            long randLong = 0;
            short randShort = 0;
            for (int i = 0; i < repetitions; i++)
            {
                if (doNotAutomaticallyReturnToPool && e.LazinatorMemoryStorage.IsEmpty == false)
                    e.LazinatorMemoryStorage.LazinatorShouldNotReturnToPool();
                if (i == 0)
                {
                    e.MyChild1.MyLong = 0;
                    e.MyChild1.MyShort = 0;
                    ((ExampleChildInherited)e.MyChild1).MyInt = 0;
                }
                if (i == 5)
                {
                    c1 = e.CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.IndependentBuffers);
                    c2 = e.CloneNoBuffer();
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
                    if (doNotAutomaticallyReturnToPool && e.MyChild1.LazinatorMemoryStorage.IsEmpty == false)
                        e.MyChild1.LazinatorMemoryStorage.LazinatorShouldNotReturnToPool();
                    e.MyChild1.UpdateStoredBuffer();
                }
                if (makeParentUpToDate)
                    e.UpdateStoredBuffer();
            }
            foreach (Example c in new Example[] { c1, c2 })
            {
                c.MyChild2.MyLong = -3; // make sure early clone still works
            }
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
                    c1 = e.CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.IndependentBuffers);
                }
                e.MyBool = !e.MyBool;
                e.UpdateStoredBuffer();
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
            l.UpdateStoredBuffer();
        }

        [Fact]
        public void CanCloneListOfLazinatorsAndThenEnsureUpToDate()
        {
            var l = new LazinatorList<Example>() { GetTypicalExample(), GetTypicalExample() };
            var l2 = l.CloneLazinatorTyped();
            var l3 = l.CloneLazinatorTyped();
            l.UpdateStoredBuffer();
        }

        [Fact]
        public void CanCloneDictionaryAndThenEnsureUpToDate()
        {
            // Note: This is a sequence that proved problematic in the next test
            var d = GetDictionary();
            var d2 = d.CloneLazinatorTyped();
            var d3 = d.CloneLazinatorTyped();
            d.UpdateStoredBuffer();
        }

        [Fact]
        public void ObjectDisposedExceptionThrownOnItemRemovedFromHierarchy()
        {
            LazinatorDictionary<WInt, Example> d = GetDictionary();
            //same effect if both of the following lines are included
            //d.UpdateStoredBuffer();
            //d[0].MyChar = 'q';
            d[0].UpdateStoredBuffer(); // OwnedMemory has allocation ID of 0. 
            d.UpdateStoredBuffer(); // OwnedMemory for this and d[0] share allocation ID of 1
            Example e = d[0];
            d[0] = GetTypicalExample();
            d.LazinatorMemoryStorage.Dispose();
            Action a = () => { var x = e.MyChild1.LazinatorMemoryStorage.OwnedMemory.Memory; }; // note that error occurs only when looking at underlying memory
            a.Should().Throw<ObjectDisposedException>();
        }

        [Fact]
        public void ObjectDisposedExceptionAvoidedByCloneToIndependentBuffer()
        {
            LazinatorDictionary<WInt, Example> d = GetDictionary();
            d[0].UpdateStoredBuffer(); // OwnedMemory has allocation ID of 0. 
            d.UpdateStoredBuffer(); // OwnedMemory for this and d[0] share allocation ID of 1
            Example e = d[0].CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.IndependentBuffers);
            d[0] = GetTypicalExample();
            d.UpdateStoredBuffer(); // allocation ID 1 disposed.
            Action a = () => { var x = e.MyChild1; };
            a.Should().NotThrow<ObjectDisposedException>();
        }

        [Fact]
        public void CanAccessCopiedItemAfterEnsureUpToDate()
        {
            LazinatorDictionary<WInt, Example> d = GetDictionary();
            d.UpdateStoredBuffer(); // OwnedMemory for this and d[0] share allocation ID of 0. As the original source, this will not be automatically disposed. 
            Example e = d[0];
            d[0] = GetTypicalExample();
            d.UpdateStoredBuffer(); // allocation ID 0 is not disposed.
            var x = e.MyChild1;
        }

        [Fact]
        public void RemoveBufferWorks_Example()
        {
            Example e = GetTypicalExample();
            e.UpdateStoredBuffer();
            e.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            var x = e.MyChild1.MyExampleGrandchild.MyInt;
            e.MyChild1.MyExampleGrandchild.MyInt++;
            e.MyChild1.MyExampleGrandchild.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            e.RemoveBufferInHierarchy();
            e.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            e.MyChild1.MyExampleGrandchild.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            e.MyChild1.MyExampleGrandchild.MyInt.Should().Be(x + 1);

            var original = e.LazinatorMemoryStorage;
            e.UpdateStoredBuffer();
            original.Dispose(); // make sure that attempting to access original will cause problems

            e.MyChild1.MyExampleGrandchild.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            e.MyChild1.MyExampleGrandchild.MyInt.Should().Be(x + 1);
        }

        [Theory]
        [InlineData(true, false, false, true, true)]
        [InlineData(true, false, false, true, false)]
        [InlineData(true, false, false, false, true)]
        [InlineData(true, false, false, false, false)]
        [InlineData(false, true, false, true, true)]
        [InlineData(false, true, false, true, false)]
        [InlineData(false, true, false, false, true)]
        [InlineData(false, true, false, false, false)]
        [InlineData(false, false, true, true, true)]
        [InlineData(false, false, true, true, false)]
        [InlineData(false, false, true, false, true)]
        [InlineData(false, false, true, false, false)]
        public void RemoveBufferWorks_SpanAndMemory(bool readOnlySpan, bool memory, bool readOnlyMemory, bool readBeforeUpdate, bool setBeforeUpdate)
        {
            SpanAndMemory duplicate = LazinatorSpanTests.GetSpanAndMemory(false);
            SpanAndMemory main = LazinatorSpanTests.GetSpanAndMemory(false);
            main = main.CloneLazinatorTyped();
            if (readBeforeUpdate)
            {
                if (readOnlySpan)
                {
                    var read = main.MyReadOnlySpanChar;
                }
                else if (memory)
                {
                    var read = main.MyMemoryByte;
                }
                else if (readOnlyMemory)
                {
                    var read = main.MyReadOnlyMemoryChar;
                }
            }

            if (setBeforeUpdate)
            {
                if (readOnlySpan)
                {
                    main.MyReadOnlySpanChar = duplicate.MyReadOnlySpanChar;
                }
                else if (memory)
                {
                    main.MyMemoryByte = duplicate.MyMemoryByte;
                }
                else if (readOnlyMemory)
                {
                    main.MyReadOnlyMemoryChar = duplicate.MyReadOnlyMemoryChar;
                }
            }
            var original = main.LazinatorMemoryStorage;
            main.RemoveBufferInHierarchy();
            original.Dispose(); // make sure that attempting to access original will cause problems
            if (readOnlySpan)
            {
                main.MyReadOnlySpanChar.ToArray().SequenceEqual(duplicate.MyReadOnlySpanChar.ToArray()).Should().BeTrue();
            }
            else if (memory)
            {
                main.MyMemoryByte.ToArray().SequenceEqual(duplicate.MyMemoryByte.ToArray()).Should().BeTrue();
            }
            else if (readOnlyMemory)
            {
                main.MyReadOnlyMemoryChar.ToArray().SequenceEqual(duplicate.MyReadOnlyMemoryChar.ToArray()).Should().BeTrue();
            }
        }

        [Fact]
        public void RemoveBufferWorks_RecordLikeContainer()
        {
            RecordLikeContainer recordLikeContainer = new RecordLikeContainer
            {
                MyRecordLikeTypeWithLazinator = new RecordLikeTypeWithLazinator(5, "May", GetTypicalExample(), new ExampleStructWithoutClass() { MyInt = 17 })
            };

            recordLikeContainer = recordLikeContainer.CloneLazinatorTyped();
            recordLikeContainer.MyRecordLikeClass = new RecordLikeClass(3, GetTypicalExample()); // make outer class dirty
            recordLikeContainer.RemoveBufferInHierarchy();
            recordLikeContainer.MyRecordLikeTypeWithLazinator.Example.MyDateTime.Should().Be(new DateTime(1972, 10, 22, 17, 36, 0));
            recordLikeContainer.MyRecordLikeTypeWithLazinator.ExampleStruct.MyInt.Should().Be(17);
            recordLikeContainer.MyRecordLikeTypeWithLazinator.Example.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            recordLikeContainer.MyRecordLikeTypeWithLazinator.ExampleStruct.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            var x = recordLikeContainer.MyRecordLikeTypeWithLazinator.Example.MyChar;
            recordLikeContainer.UpdateStoredBuffer(); // should remove the buffer within the struct
            recordLikeContainer.MyRecordLikeTypeWithLazinator.Example.MyDateTime.Should().Be(new DateTime(1972, 10, 22, 17, 36, 0));
            recordLikeContainer.MyRecordLikeTypeWithLazinator.ExampleStruct.MyInt.Should().Be(17);
            recordLikeContainer.LazinatorMemoryStorage.IsEmpty.Should().BeFalse(); 
            recordLikeContainer.MyRecordLikeTypeWithLazinator.Example.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            recordLikeContainer.MyRecordLikeTypeWithLazinator.ExampleStruct.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            recordLikeContainer = recordLikeContainer.CloneLazinatorTyped();
            recordLikeContainer.MyRecordLikeClass = new RecordLikeClass(3, GetTypicalExample()); // make outer class dirty
            recordLikeContainer.UpdateStoredBuffer();
            recordLikeContainer.MyRecordLikeTypeWithLazinator.Example.MyDateTime.Should().Be(new DateTime(1972, 10, 22, 17, 36, 0));
            recordLikeContainer.MyRecordLikeTypeWithLazinator.ExampleStruct.MyInt.Should().Be(17);
            recordLikeContainer.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            recordLikeContainer.MyRecordLikeTypeWithLazinator.Example.LazinatorMemoryStorage.IsEmpty.Should().BeFalse(); // this will be first access to MyRecordLikeTypeWithLazinator.Example, so that will lead to the memory allocation
            recordLikeContainer.MyRecordLikeTypeWithLazinator.ExampleStruct.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            recordLikeContainer.MyRecordLikeTypeWithLazinator.Example.LazinatorMemoryStorage.AllocationID.Should().Be(
                recordLikeContainer.LazinatorMemoryStorage.AllocationID);
        }

        [Fact]
        public void RemoveBufferWorks_ExampleStructContainingStructContainer()
        {
            ContainerForExampleStructWithoutClass e = new ContainerForExampleStructWithoutClass();
            var x = e.ExampleStructWithoutClass;
            x.MyInt++;
            e.ExampleStructWithoutClass = x;
            e.UpdateStoredBuffer();
            e.ExampleStructWithoutClass.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            x = e.ExampleStructWithoutClass;
            x.MyInt++;
            var y = x.MyInt;
            e.ExampleStructWithoutClass = x;
            e.ExampleStructWithoutClass.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            e.RemoveBufferInHierarchy();
            e.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            e.ExampleStructWithoutClass.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            e.ExampleStructWithoutClass.MyInt.Should().Be(y);

            e.UpdateStoredBuffer();
            e.ExampleStructWithoutClass.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            e.ExampleStructWithoutClass.MyInt.Should().Be(y);
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
            lazinator.UpdateStoredBuffer();
            lazinator.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            var x = lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt;
            lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt++;
            lazinator.MyListSerialized_Dirty = true;
            lazinator.MyListSerialized[0].MyExampleGrandchild.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();

            lazinator.RemoveBufferInHierarchy();
            lazinator.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            lazinator.MyListSerialized[0].MyExampleGrandchild.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt.Should().Be(x + 1);

            lazinator.UpdateStoredBuffer();
            lazinator.MyListSerialized[0].MyExampleGrandchild.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt.Should().Be(x + 1);

            lazinator.UpdateStoredBuffer();
            LazinatorMemory lazinatorMemoryStorage = lazinator.MyListSerialized[0].MyExampleGrandchild.LazinatorMemoryStorage;
            lazinatorMemoryStorage.Should().NotBeNull();
            lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt.Should().Be(x + 1);

            lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt++;
            lazinator.MyListSerialized_Dirty = true;
            lazinator.UpdateStoredBuffer();
            lazinator.MyListSerialized[0].MyExampleGrandchild.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            (lazinator.MyListSerialized[0].MyExampleGrandchild.LazinatorMemoryStorage.Equals(lazinatorMemoryStorage)).Should()
                .BeFalse();
            lazinator.MyListSerialized[0].MyExampleGrandchild.MyInt.Should().Be(x + 2);
        }


        [Fact]
        public void RemoveBufferWorks_LazinatorList()
        {
            // when we remove the buffer from a lazinatorlist, it completely deserializes.

            LazinatorListContainer lazinator = new LazinatorListContainer()
            {
                MyStructList = new LazinatorList<WByte>()
                {
                    3,
                    4,
                    5
                }
            };
            lazinator.UpdateStoredBuffer();
            lazinator.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            var x = lazinator.MyStructList[0].WrappedValue;
            lazinator.MyStructList[0] = 6;
            lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();

            lazinator.RemoveBufferInHierarchy();
            lazinator.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            lazinator.MyStructList[0].WrappedValue.Should().Be(6);

            lazinator.UpdateStoredBuffer();
            lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            lazinator.MyStructList[0].WrappedValue.Should().Be(6);

            lazinator.UpdateStoredBuffer();
            lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            lazinator.MyStructList[0].WrappedValue.Should().Be(6);

            lazinator.MyStructList[0] = 7;
            lazinator.UpdateStoredBuffer();
            lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            lazinator.MyStructList[0].WrappedValue.Should().Be(7);

            WByte w = new WByte(8).CloneLazinatorTyped(); // make 
            lazinator.MyStructList[0] = w;
            lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            lazinator.RemoveBufferInHierarchy();
            lazinator.MyStructList[0].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();

            lazinator.MyStructList[1] = w;
            lazinator.MyStructList[1].LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            lazinator.UpdateStoredBuffer();
            lazinator.MyStructList[1].LazinatorMemoryStorage.IsEmpty.Should().BeTrue();

            lazinator = new LazinatorListContainer()
            {
                MyStructList = new LazinatorList<WByte>()
                {
                    3,
                    4,
                    5
                }
            };
            lazinator = lazinator.CloneLazinatorTyped();
            var list = lazinator.MyStructList;
            lazinator.RemoveBufferInHierarchy();
            lazinator.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            lazinator.MyStructList.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            lazinator.MyStructList[0].WrappedValue.Should().Be(3);
        }

        [Fact]
        public void RemoveBufferWorks_SpanInDotNetList()
        {
            SpanInDotNetList lazinator = new SpanInDotNetList()
            {
                SpanList = new List<SpanAndMemory>()
                {
                    new SpanAndMemory()
                    {
                        MyReadOnlySpanByte = new byte[] { 1, 2, 3 }
                    }
                }
            };
            var x = lazinator.SpanList[0].MyReadOnlySpanByte[1];
            x.Should().Be(2);
            lazinator.UpdateStoredBuffer();
            var memoryStorage = lazinator.LazinatorMemoryStorage;
            lazinator.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            x = lazinator.SpanList[0].MyReadOnlySpanByte[1];
            x.Should().Be(2);

            lazinator.RemoveBufferInHierarchy();
            memoryStorage.Dispose();
            memoryStorage.Disposed.Should().BeTrue();
            lazinator.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            x = lazinator.SpanList[0].MyReadOnlySpanByte[1]; // this should be in memory storage that was copied at the time of RemoveBufferInHierarchy
            x.Should().Be(2);
            lazinator.SpanList[0].MyReadOnlySpanChar = new ReadOnlySpan<char>(new char[2] {'a', 'b'});

            // check works on first access
            lazinator = lazinator.CloneLazinatorTyped();
            x = lazinator.SpanList[0].MyReadOnlySpanByte[1];
            x.Should().Be(2);

            // check works after removal of buffer before access
            lazinator = lazinator.CloneLazinatorTyped();
            memoryStorage = lazinator.LazinatorMemoryStorage;
            lazinator.RemoveBufferInHierarchy();
            memoryStorage.Dispose();
            memoryStorage.Disposed.Should().BeTrue();
            x = lazinator.SpanList[0].MyReadOnlySpanByte[1];
            x.Should().Be(2);
            var y = lazinator.SpanList[0].MyReadOnlySpanChar[1];
            y.Should().Be('b');
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
            lazinator.UpdateStoredBuffer();
            lazinator.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            var x = lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt;
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt++;
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();

            lazinator.RemoveBufferInHierarchy();
            lazinator.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.LazinatorMemoryStorage.IsEmpty.Should().BeTrue();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt.Should().Be(x + 1);

            lazinator.UpdateStoredBuffer();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt.Should().Be(x + 1);

            lazinator.UpdateStoredBuffer();
            LazinatorMemory lazinatorMemoryStorage = lazinator.MyHashSetSerialized.First().MyExampleGrandchild.LazinatorMemoryStorage;
            lazinatorMemoryStorage.Should().NotBeNull();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt.Should().Be(x + 1);

            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.MyInt++;
            lazinator.UpdateStoredBuffer();
            lazinator.MyHashSetSerialized.First().MyExampleGrandchild.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
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
            e.MyChild1.UpdateStoredBuffer();
            e.UpdateStoredBuffer();
            ((ExampleChildInherited)e.MyChild1).MyInt.Should().Be(137);
            ((ExampleChildInherited)e.MyChild1).MyGrandchildInInherited.MyInt.Should().Be(141);
        }

        [Fact]
        public void UpdateStoredBuffer_Struct()
        {
            WInt w = 3;
            w.UpdateStoredBuffer();
            w.LazinatorMemoryStorage.IsEmpty.Should().BeFalse();
            w.WrappedValue.Should().Be(3);
        }

        [Fact]
        public void CloneMiddleOfHierarchy()
        {
            Example e = GetTypicalExample();
            e.MyChild1.MyShort = 5;
            e = e.CloneLazinatorTyped();
            e.MyChild1.MyShort = 6;
            e.MyChild1.UpdateStoredBuffer();
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
            e.UpdateStoredBuffer();
            var buffer = new Memory<byte>(e.LazinatorMemoryStorage.Memory.Span.ToArray());
            BinaryBufferWriter b = new BinaryBufferWriter();
            b.Write(buffer.Span);
            e.UpdateStoredBuffer(ref b, 0, buffer.Span.Length, IncludeChildrenMode.IncludeAllChildren, true);
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
            e.MyChild1.UpdateStoredBuffer();
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
            e.MyList[1].UpdateStoredBuffer(); // generate a new buffer in a list member
            ConfirmBuffersUpdateInTandem(e);
            e.MyInt = 17; // keep list clean while making container dirty
            ConfirmBuffersUpdateInTandem(e);
        }

        [Fact]
        public void BuffersUpdateInTandem_LazinatorList_Struct()
        {
            LazinatorListContainer e = GetLazinatorListContainer();
            e.MyStructList[1] = 6;
            e.MyStructList[1] = e.MyStructList[1].CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.IndependentBuffers); // generate a new buffer in a list member
            ConfirmBuffersUpdateInTandem(e);
            e.MyInt = 17; // keep list clean while making container dirty
            ConfirmBuffersUpdateInTandem(e);
        }

        [Fact]
        public void BuffersUpdateInTandem_List_Struct()
        {
            ExampleContainerContainingClassesStructContainingClasses e = new ExampleContainerContainingClassesStructContainingClasses()
            {
                MyListExampleStruct = new List<ExampleStructContainingClasses>()
                {
                    new ExampleStructContainingClasses()
                    {
                        MyChar = 'h'
                    }
                }
            };

            e.MyListExampleStruct[0] = new ExampleStructContainingClasses()
            {
                MyChar = 'i'
            };
            e.MyListExampleStruct[0] = e.MyListExampleStruct[0].CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.IndependentBuffers); // generate a new buffer in a list member
            ConfirmBuffersUpdateInTandem(e);
            e.MyExampleStructContainingClasses = new ExampleStructContainingClasses()
            {
                MyChar = 'j'
            }; // keep list clean while making container dirty
            ConfirmBuffersUpdateInTandem(e);
        }

        [Fact]
        public void BuffersUpdateInTandem_List_OpenGeneric_Struct()
        {
            OpenGenericStayingOpenContainer e = new OpenGenericStayingOpenContainer()
            {
                ClosedGenericFloat = new OpenGeneric<WFloat>()
                {
                    MyListT = new List<WFloat>()
                    {
                        3.4F
                    }
                }
            };

            e.ClosedGenericFloat.MyListT[0] = new WFloat(4.0F);
            e.ClosedGenericFloat.MyListT[0] = e.ClosedGenericFloat.MyListT[0].CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions.IndependentBuffers); // generate a new buffer in a list member
            ConfirmBuffersUpdateInTandem(e);
            e.ClosedGenericFloat.MyT = 10.0F; // keep list clean while making container dirty
            ConfirmBuffersUpdateInTandem(e);
        }

        [Fact]
        public void BuffersUpdateInTandem_StructTuple()
        {
            StructTuple e = new StructTuple
            {
                MyValueTupleStructs = (3, 4), // WInts
                MyValueTupleSerialized = (4, GetExampleChild(0), GetNonLazinatorType(1))
            };

            e = e.CloneLazinatorTyped();
            ConfirmBuffersUpdateInTandem(e);
            e.MyNamedTuple = (3, 4.0);
            ConfirmBuffersUpdateInTandem(e);
        }

        [Fact]
        public void BuffersUpdateInTandem_RecordLikeCollections()
        {
            RecordLikeCollections e = new RecordLikeCollections()
            {
                MyDictionaryWithRecordLikeTypeValues = new Dictionary<int, RecordLikeTypeWithLazinator>() { { 0, new RecordLikeTypeWithLazinator(12, "Sam", GetTypicalExample(), new ExampleStructWithoutClass() { MyInt = 18 }) } },
                MyDictionaryWithRecordLikeContainers = new Dictionary<int, RecordLikeContainer>() { { 0, new RecordLikeContainer() { MyRecordLikeTypeWithLazinator = new RecordLikeTypeWithLazinator(12, "Sam", GetTypicalExample(), new ExampleStructWithoutClass() { MyInt = 18 } ) } } }
            };
            DateTime expectedDateTime = new DateTime(1972, 10, 22, 17, 36, 0);
            e.MyDictionaryWithRecordLikeTypeValues[0].Example.MyDateTime.Should().Be(expectedDateTime);
            e.MyDictionaryWithRecordLikeTypeValues[0].ExampleStruct.MyInt.Should().Be(18);
            e.MyDictionaryWithRecordLikeContainers[0].MyRecordLikeTypeWithLazinator.Example.MyDateTime.Should().Be(expectedDateTime);

            e = e.CloneLazinatorTyped();
            e.MyInt = 30;
            var deserialized = e.MyDictionaryWithRecordLikeContainers;
            e.UpdateStoredBuffer();
            e = e.CloneLazinatorTyped();
            ConfirmBuffersUpdateInTandem(e);
            e.MyDictionaryWithRecordLikeTypeValues[0].Example.MyDateTime.Should().Be(expectedDateTime);
            e.MyDictionaryWithRecordLikeTypeValues[0].ExampleStruct.MyInt.Should().Be(18);
            e.MyDictionaryWithRecordLikeContainers[0].MyRecordLikeTypeWithLazinator.Example.MyDateTime.Should().Be(expectedDateTime);
        }

        [Fact]
        public void BuffersUpdateInTandem_RecordLikeContainer()
        {
            RecordLikeContainer e = new RecordLikeContainer
            {
                MyRecordLikeTypeWithLazinator = new RecordLikeTypeWithLazinator(5, "May", GetTypicalExample(), new ExampleStructWithoutClass() { MyInt = 17 }),
            };

            e = e.CloneLazinatorTyped();
            ConfirmBuffersUpdateInTandem(e);

            e = e.CloneLazinatorTyped();
            e.MyInt = 25; // make container dirty
            ConfirmBuffersUpdateInTandem(e);
        }

        [Fact]
        public void BuffersUpdateInTandem_HashSet_Struct()
        {
            ExampleContainerContainingClassesStructContainingClasses e = new ExampleContainerContainingClassesStructContainingClasses()
            {
                MyHashSetExampleStruct = new HashSet<ExampleStructContainingClasses>()
                {
                    new ExampleStructContainingClasses()
                    {
                        MyChar = 'h'
                    }
                }
            };

            e.MyHashSetExampleStruct.Add(new ExampleStructContainingClasses()
            {
                MyChar = 'i'
            }.CloneLazinatorTyped());
            ConfirmBuffersUpdateInTandem(e);
            e.MyExampleStructContainingClasses = new ExampleStructContainingClasses()
            {
                MyChar = 'j'
            }; // keep list clean while making container dirty
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
            container.MyStructList = new LazinatorList<WByte>();
            container.MyStructList.Add(1);
            container.MyStructList.Add(2);
            container = container.CloneLazinatorTyped();
            return container;
        }

        private static void ConfirmBuffersUpdateInTandem(ILazinator itemToUpdate)
        {
            itemToUpdate.UpdateStoredBuffer();
            var allocationID = ((ExpandableBytes)itemToUpdate.LazinatorMemoryStorage.OwnedMemory).AllocationID;
            itemToUpdate.ForEachLazinator(x => 
            {
                if (x.LazinatorMemoryStorage.IsEmpty == false)
                {
                    ExpandableBytes b = x.LazinatorMemoryStorage.OwnedMemory as ExpandableBytes;
                    if (b != null)
                        b.AllocationID.Should().Be(allocationID);
                }
                return x;
            }, true, true);
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
            e.UpdateStoredBuffer();
            uint h = e.MyChild1.MyExampleGrandchild.GetBinaryHashCode32();
            uint h2 = (new ExampleGrandchild() { MyInt = 17 }).GetBinaryHashCode32();
            h.Should().Be(h2);
        }
    }
}
