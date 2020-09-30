using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using Lazinator.Exceptions;
using Lazinator.Core;
using LazinatorTests.Examples.Tuples;
using Xunit;
using Lazinator.Wrappers;
using Lazinator.Buffers;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.Subclasses;

namespace LazinatorTests.Tests
{
    public class SupportedCollectionsTest : SerializationDeserializationTestBase
    {
        [Fact]
        public void LazinatorDotNetListInt()
        {
            DotNetList_Values GetObject(int thirdItem)
            {
                return new DotNetList_Values()
                {
                    MyListInt = new List<int>() { 3, 4, thirdItem }
                };
            }

            var original = GetObject(5);
            var copy = GetObject(5);
            var copyWithGoal = GetObject(5);
            copyWithGoal.MyListInt[2] = 6;
            var result = original.CloneLazinatorTyped();
            copy.MyListInt.SequenceEqual(result.MyListInt).Should().BeTrue();
            // make sure that updates are registered when dirty flag is set
            result.MyListInt[2] = 6;
            result.MyListInt_Dirty = true;
            var result2 = result.CloneLazinatorTyped();
            copyWithGoal.MyListInt.SequenceEqual(result2.MyListInt).Should().BeTrue();
            // if we make a change but don't set dirty, nothing happens if we don't verify cleanliness
            result2.MyListInt[2] = 7;
            var result3 = result2.CloneLazinatorTyped();
            result3.MyListInt[2].Should().Be(6); // the change is ignored, since dirtiness flag wasn't set
            // make sure that error is thrown if we do verify cleanliness
            Action attemptToVerifyCleanlinessWithoutSettingDirtyFlag = () => CloneWithOptionalVerification(result2, true, true); // now, verifying cleanliness
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().Throw<UnexpectedDirtinessException>();
        }


        [Fact]
        public void LazinatorDotNetListWrappedInts()
        {
            DotNetList_Wrapper GetObject(int thirdItem)
            {
                return new DotNetList_Wrapper()
                {
                    MyListInt = new List<WInt32>() { 3, 4, thirdItem }
                };
            }

            var original = GetObject(5);
            var copy = GetObject(5);
            var copyWithGoal = GetObject(5);
            copyWithGoal.MyListInt[2] = 6;
            var result = original.CloneLazinatorTyped();
            copy.MyListInt.SequenceEqual(result.MyListInt).Should().BeTrue();
            // make sure that updates are registered when dirty flag is set
            result.MyListInt[2] = 6;
            result.MyListInt_Dirty = true;
            var result2 = result.CloneLazinatorTyped();
            copyWithGoal.MyListInt.SequenceEqual(result2.MyListInt).Should().BeTrue();
            // if we make a change but don't set dirty, nothing happens if we don't verify cleanliness
            result2.MyListInt[2] = 7;
            var result3 = result2.CloneLazinatorTyped();
            result3.MyListInt[2].Should().Be(6); // the change is ignored, since dirtiness flag wasn't set
            // make sure that error is thrown if we do verify cleanliness
            Action attemptToVerifyCleanlinessWithoutSettingDirtyFlag = () => CloneWithOptionalVerification(result2, true, true); // now, verifying cleanliness
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().Throw<UnexpectedDirtinessException>();
        }

        [Fact]
        public void LazinatorDotNetListWrappedNullableLazinators()
        {
            DotNetList_Wrapper w = new DotNetList_Wrapper()
            {
                MyListNullableByte = new List<WNullableByte>() { 3, 4, 249, null },
                MyListNullableInt = new List<WNullableInt32>() { 3, 16000, 249, null, 1000000000 }
            };
            var c = w.CloneLazinatorTyped();
            c.MyListNullableByte[0].WrappedValue.Should().Be((byte)3);
            c.MyListNullableByte[1].WrappedValue.Should().Be((byte)4);
            c.MyListNullableByte[2].WrappedValue.Should().Be((byte)249);
            c.MyListNullableByte[3].WrappedValue.Should().Be(null);
            c.MyListNullableInt[0].WrappedValue.Should().Be(3);
            c.MyListNullableInt[1].WrappedValue.Should().Be(16000);
            c.MyListNullableInt[2].WrappedValue.Should().Be(249);
            c.MyListNullableInt[3].WrappedValue.Should().Be(null);
            c.MyListNullableInt[4].WrappedValue.Should().Be(1000000000);
        }

        [Fact]
        public void LazinatorDotNetListInt_Null()
        {
            DotNetList_Values GetObject()
            {
                return new DotNetList_Values()
                {
                    MyListInt = null
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var copyWithGoal = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyListInt.Should().BeNull();
        }


        [Fact]
        public void LazinatorDotNetListInt_Empty()
        {
            DotNetList_Values GetObject()
            {
                return new DotNetList_Values()
                {
                    MyListInt = new List<int>()
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var copyWithGoal = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyListInt.Count().Should().Be(0);
        }

        [Fact]
        public void LazinatorDotNetListLazinatorStructs_Null()
        {
            ExampleContainerContainingClassesStructContainingClasses GetObject()
            {
                return new ExampleContainerContainingClassesStructContainingClasses()
                {
                    MyListExampleStruct = null,
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var copyWithGoal = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyListExampleStruct.Should().BeNull();
        }

        [Fact]
        public void LazinatorDotNetListWrappedNullableStructs()
        {
            ExampleContainerContainingClassesStructContainingClasses GetObject()
            {
                var returnObj = new ExampleContainerContainingClassesStructContainingClasses()
                {
                    MyListNullableExampleStruct = new List<WNullableStruct<ExampleStructContainingClasses>>()
                    {
                        new WNullableStruct<ExampleStructContainingClasses>() { AsNullableStruct = new ExampleStructContainingClasses() { MyChar = 'd' } },
                        new WNullableStruct<ExampleStructContainingClasses>() { AsNullableStruct = null },
                    },
                };
                return returnObj;
            }

            var original = GetObject();
            var copy = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyListNullableExampleStruct.Count().Should().Be(2);
            result.MyListNullableExampleStruct[0].AsNullableStruct.Value.MyChar.Should().Be('d');
            result.MyListNullableExampleStruct[1].AsNullableStruct.Should().BeNull();
        }

        [Fact]
        public void LazinatorDotNetListUnwrappedNullableStructs()
        {
            ExampleContainerContainingClassesStructContainingClasses GetObject()
            {
                var returnObj = new ExampleContainerContainingClassesStructContainingClasses()
                {
                    MyListUnwrappedNullableExampleStruct = new List<ExampleStructContainingClasses?>()
                    {
                        new ExampleStructContainingClasses() { MyChar = 'd' },
                        null
                    },
                };
                return returnObj;
            }

            var original = GetObject();
            var copy = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyListUnwrappedNullableExampleStruct.Count().Should().Be(2);
            result.MyListUnwrappedNullableExampleStruct[0].Value.MyChar.Should().Be('d');
            result.MyListUnwrappedNullableExampleStruct[1].Should().BeNull();
        }

        [Fact]
        public void LazinatorDotNetListLazinatorStructs_Empty()
        {
            ExampleContainerContainingClassesStructContainingClasses GetObject()
            {
                return new ExampleContainerContainingClassesStructContainingClasses()
                {
                    MyListExampleStruct = new List<ExampleStructContainingClasses>(),
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var copyWithGoal = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyListExampleStruct.Count().Should().Be(0);
        }

        [Fact]
        public void LazinatorDotNetListLazinatorStructs_Filled()
        {
            ExampleContainerContainingClassesStructContainingClasses GetObject()
            {
                var returnObj = new ExampleContainerContainingClassesStructContainingClasses()
                {
                    MyListExampleStruct = new List<ExampleStructContainingClasses>()
                    {
                        new ExampleStructContainingClasses() { MyChar = 'd'},
                        new ExampleStructContainingClasses() { MyChar = 'e'},
                    },
                };
                return returnObj;
            }

            var original = GetObject();
            var copy = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyListExampleStruct.Count().Should().Be(2);
            result.MyListExampleStruct[0].MyChar.Should().Be('d');
            result.MyListExampleStruct[1].MyChar.Should().Be('e');
        }

        [Fact]
        public void LazinatorDotNetListLazinatorNullableStructs()
        {
            ExampleContainerContainingClassesStructContainingClasses GetObject()
            {
                var returnObj = new ExampleContainerContainingClassesStructContainingClasses()
                {
                    MyListUnwrappedNullableExampleStruct = new List<ExampleStructContainingClasses?>()
                    {
                        new ExampleStructContainingClasses() { MyChar = 'd'},
                        null,
                        new ExampleStructContainingClasses() { MyChar = 'e'},
                    },
                };
                return returnObj;
            }

            var original = GetObject();
            var copy = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyListUnwrappedNullableExampleStruct.Count().Should().Be(3);
            result.MyListUnwrappedNullableExampleStruct[0].Value.MyChar.Should().Be('d');
            result.MyListUnwrappedNullableExampleStruct[1].HasValue.Should().BeFalse();
            result.MyListUnwrappedNullableExampleStruct[2].Value.MyChar.Should().Be('e');
        }
        

        [Fact]
        public void LazinatorDotNetListLazinator_Null()
        {
            DotNetList_Lazinator GetObject()
            {
                return new DotNetList_Lazinator()
                {
                    MyListSerialized = null,
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var copyWithGoal = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyListSerialized.Should().BeNull();
        }

        [Fact]
        public void LazinatorDotNetListLazinator_Empty()
        {
            DotNetList_Lazinator GetObject()
            {
                return new DotNetList_Lazinator()
                {
                    MyListSerialized = new List<ExampleChild>()
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var copyWithGoal = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyListSerialized.Count().Should().Be(0);
        }

        [Fact]
        public void Lazinator_NullableRegularTuple_Null()
        {
            RegularTuple GetObject()
            {
                return new RegularTuple()
                {
                    MyTupleSerialized = null
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var copyWithGoal = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyTupleSerialized.Should().BeNull();
        }

        [Fact]
        public void Lazinator_NullableRegularTuple_NonNullableStruct()
        {
            RegularTuple GetObject()
            {
                return new RegularTuple()
                {
                    MyTupleSerialized4 = new Tuple<int, ExampleStructContainingClasses>(3, new ExampleStructContainingClasses() { MyChar = '5' })
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var copyWithGoal = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyTupleSerialized4.Item2.MyChar.Should().Be('5');
        }

        [Fact]
        public void Lazinator_NullableRegularTuple_NullableStruct()
        {
            RegularTuple GetObject()
            {
                return new RegularTuple()
                {
                    MyTupleSerialized5 = new Tuple<int, ExampleStructContainingClasses?>(3, new ExampleStructContainingClasses() { MyChar = '5' })
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var copyWithGoal = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyTupleSerialized5.Item2.Value.MyChar.Should().Be('5');
        }

        [Fact]
        public void Lazinator_NullableRegularTuple_NullableStructIsNull()
        {
            RegularTuple GetObject()
            {
                return new RegularTuple()
                {
                    MyTupleSerialized5 = new Tuple<int, ExampleStructContainingClasses?>(3, null)
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var copyWithGoal = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyTupleSerialized5.Item2.HasValue.Should().BeFalse();
        }

        [Fact]
        public void Lazinator_NullableValueTuple_Null()
        {
            StructTuple GetObject()
            {
                return new StructTuple()
                {
                    MyNullableTuple = null
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var copyWithGoal = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyNullableTuple.Should().BeNull();
        }


        [Fact]
        public void LazinatorDotNetLinkedListInt()
        {
            DotNetList_Values GetObject(int thirdItem)
            {
                return new DotNetList_Values()
                {
                    MyLinkedListInt = new LinkedList<int>(new[] { 3, 4, thirdItem })
                };
            }

            var original = GetObject(5);
            var copy = GetObject(5);
            var result = original.CloneLazinatorTyped();
            copy.MyLinkedListInt.SequenceEqual(result.MyLinkedListInt).Should().BeTrue();
        }

        [Fact]
        public void LazinatorDotNetSortedSetInt()
        {
            DotNetList_Values GetObject(int thirdItem)
            {
                return new DotNetList_Values()
                {
                    MySortedSetInt = new SortedSet<int>(new[] { 3, 4, thirdItem })
                };
            }

            var original = GetObject(5);
            var copy = GetObject(5);
            var result = original.CloneLazinatorTyped();
            copy.MySortedSetInt.SequenceEqual(result.MySortedSetInt).Should().BeTrue();
        }


        [Fact]
        public void LazinatorDotNetQueueInt()
        {
            DotNetQueue_Values GetObject(int thirdItem)
            {
                return new DotNetQueue_Values()
                {
                    MyQueueInt = new Queue<int>(new[] { 3, 4, thirdItem })
                };
            }

            var original = GetObject(5);
            var copy = GetObject(5);
            var copyWithGoal = GetObject(5);
            copyWithGoal.MyQueueInt.Enqueue(6);
            var result = original.CloneLazinatorTyped();
            copy.MyQueueInt.SequenceEqual(result.MyQueueInt).Should().BeTrue();
            // make sure that updates are registered when dirty flag is set
            result.MyQueueInt.Enqueue(6);
            result.MyQueueInt_Dirty = true;
            var result2 = result.CloneLazinatorTyped();
            copyWithGoal.MyQueueInt.SequenceEqual(result2.MyQueueInt).Should().BeTrue();
            result.MyQueueInt.SequenceEqual(result2.MyQueueInt).Should().BeTrue();
            // if we make a change but don't set dirty, nothing happens if we don't verify cleanliness
            result2.MyQueueInt.Enqueue(7);
            var result3 = result2.CloneLazinatorTyped();
            result3.MyQueueInt.Count().Should().Be(4); // the change is ignored, since dirtiness flag wasn't set
            // make sure that error is thrown if we do verify cleanliness
            Action attemptToVerifyCleanlinessWithoutSettingDirtyFlag = () => CloneWithOptionalVerification(result2, true, true); // now, verifying cleanliness
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().Throw<UnexpectedDirtinessException>();
        }

        [Fact]
        public void LazinatorDotNetQueueLazinator()
        {
            DotNetQueue_Lazinator GetObject()
            {
                return new DotNetQueue_Lazinator()
                {
                    MyQueueSerialized = new Queue<ExampleChild>(new[] { new ExampleChild() { MyLong = 3 }, new ExampleChild() { MyLong = 4 }, new ExampleChild() { MyLong = 5 } })
                };
            }

            var original = GetObject();
            var copy = GetObject();

            var result = original.CloneLazinatorTyped();
            copy.MyQueueSerialized.Select(x => x.MyLong).SequenceEqual(result.MyQueueSerialized.Select(x => x.MyLong)).Should().BeTrue();
        }

        [Fact]
        public void LazinatorDotNetQueueLazinatorEmpty()
        {
            DotNetQueue_Lazinator GetObject()
            {
                return new DotNetQueue_Lazinator()
                {
                    MyQueueSerialized = new Queue<ExampleChild>()
                };
            }

            var original = GetObject();
            var copy = GetObject();

            var result = original.CloneLazinatorTyped();
            copy.MyQueueSerialized.Count().Should().Be(0);
        }

        [Fact]
        public void LazinatorDotNetStackInt()
        {
            DotNetStack_Values GetObject(int thirdItem)
            {
                return new DotNetStack_Values()
                {
                    MyStackInt = new Stack<int>(new[] { 3, 4, thirdItem })
                };
            }

            var original = GetObject(5);
            var copy = GetObject(5);
            var copyWithGoal = GetObject(5);
            copyWithGoal.MyStackInt.Push(6);
            var result = original.CloneLazinatorTyped();
            copy.MyStackInt.SequenceEqual(result.MyStackInt).Should().BeTrue();
            // make sure that updates are registered when dirty flag is set
            result.MyStackInt.Push(6);
            result.MyStackInt_Dirty = true;
            var result2 = result.CloneLazinatorTyped();
            copyWithGoal.MyStackInt.SequenceEqual(result2.MyStackInt).Should().BeTrue();
            result.MyStackInt.SequenceEqual(result2.MyStackInt).Should().BeTrue();
            // if we make a change but don't set dirty, nothing happens if we don't verify cleanliness
            result2.MyStackInt.Push(7);
            var result3 = result2.CloneLazinatorTyped();
            result3.MyStackInt.Count().Should().Be(4); // the change is ignored, since dirtiness flag wasn't set
            // make sure that error is thrown if we do verify cleanliness
            Action attemptToVerifyCleanlinessWithoutSettingDirtyFlag = () => CloneWithOptionalVerification(result2, true, true); // now, verifying cleanliness
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().Throw<UnexpectedDirtinessException>();
        }

        [Fact]
        public void ArrayInt()
        {
            Array_Values GetObject(int thirdItem)
            {
                return new Array_Values()
                {
                    MyArrayInt = new int[] { 3, 4, thirdItem }
                };
            }

            var original = GetObject(5);
            var copy = GetObject(5);
            var copyWithGoal = GetObject(5);
            copyWithGoal.MyArrayInt[2] = 6;
            var result = original.CloneLazinatorTyped();
            copy.MyArrayInt.SequenceEqual(result.MyArrayInt).Should().BeTrue();
            // make sure that updates are registered when dirty flag is set
            result.MyArrayInt[2] = 6;
            result.MyArrayInt_Dirty = true;
            var result2 = result.CloneLazinatorTyped();
            result2.MyArrayInt_Dirty.Should().BeFalse();
            copyWithGoal.MyArrayInt.SequenceEqual(result2.MyArrayInt).Should().BeTrue();
            // if we make a change but don't set dirty, nothing happens if we don't verify cleanliness
            result2.MyArrayInt[2] = 7;
            var result3 = result2.CloneLazinatorTyped();
            result3.MyArrayInt[2].Should().Be(6); // the change is ignored, since dirtiness flag wasn't set
            // make sure that error is thrown if we do verify cleanliness
            Action attemptToVerifyCleanlinessWithoutSettingDirtyFlag = () => CloneWithOptionalVerification(result2, true, true); // now, verifying cleanliness
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().Throw<UnexpectedDirtinessException>();
        }

        [Fact]
        public void ArrayNullableInt()
        {
            int?[] array = new int?[] { 3, 4, 5, 0, 1, 2, null, 6 };
            Array_Values a = new Array_Values() { MyArrayNullableInt = array };
            var c = a.CloneLazinatorTyped();
            c.MyArrayNullableInt.SequenceEqual(array).Should().BeTrue();
            array = new int?[] { null, 0, (int) ((uint) 0) };
            a = new Array_Values() { MyArrayNullableInt = array };
            c = a.CloneLazinatorTyped();
            c.MyArrayNullableInt.SequenceEqual(array).Should().BeTrue();
        }

        [Fact]
        public void LazinatorDerivedArrayInt_UsingDerivedProperty()
        {
            Array_Values GetObject(int thirdItem)
            {
                return new Array_Values()
                {
                    MyArrayInt = new int[] { 3, 4, thirdItem }
                };
            }

            var original = GetObject(5);
            var copy = GetObject(5);
            var copyWithGoal = GetObject(5);
            copyWithGoal.MyArrayInt[2] = 6;
            var result = original.CloneLazinatorTyped();
            copy.MyArrayInt.SequenceEqual(result.MyArrayInt).Should().BeTrue();
            // make sure that updates are registered when dirty flag is set
            result.MyArrayInt[2] = 6;
            result.MyArrayInt_Dirty = true;
            var result2 = result.CloneLazinatorTyped();
            copyWithGoal.MyArrayInt.SequenceEqual(result2.MyArrayInt).Should().BeTrue();
            // if we make a change but don't set dirty, nothing happens if we don't verify cleanliness
            result2.MyArrayInt[2] = 7;
            var result3 = result2.CloneLazinatorTyped();
            result3.MyArrayInt[2].Should().Be(6); // the change is ignored, since dirtiness flag wasn't set
            // make sure that error is thrown if we do verify cleanliness
            Action attemptToVerifyCleanlinessWithoutSettingDirtyFlag = () => CloneWithOptionalVerification(result2, true, true); // now, verifying cleanliness
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().Throw<UnexpectedDirtinessException>();
        }

        [Fact]
        public void LazinatorDerivedArrayInt_UsingNewProperty()
        {
            DerivedArray_Values GetObject(int thirdItem)
            {
                return new DerivedArray_Values()
                {
                    MyArrayInt_DerivedLevel = new int[] { 3, 4, thirdItem }
                };
            }

            var original = GetObject(5);
            var copy = GetObject(5);
            var copyWithGoal = GetObject(5);
            copyWithGoal.MyArrayInt_DerivedLevel[2] = 6;
            var result = original.CloneLazinatorTyped();
            copy.MyArrayInt_DerivedLevel.SequenceEqual(result.MyArrayInt_DerivedLevel).Should().BeTrue();
            // make sure that updates are registered when dirty flag is set
            result.MyArrayInt_DerivedLevel[2] = 6;
            result.MyArrayInt_DerivedLevel_Dirty = true;
            var result2 = result.CloneLazinatorTyped();
            copyWithGoal.MyArrayInt_DerivedLevel.SequenceEqual(result2.MyArrayInt_DerivedLevel).Should().BeTrue();
            // if we make a change but don't set dirty, nothing happens if we don't verify cleanliness
            result2.MyArrayInt_DerivedLevel[2] = 7;
            var result3 = result2.CloneLazinatorTyped();
            result3.MyArrayInt_DerivedLevel[2].Should().Be(6); // the change is ignored, since dirtiness flag wasn't set
            // make sure that error is thrown if we do verify cleanliness
            Action attemptToVerifyCleanlinessWithoutSettingDirtyFlag = () => CloneWithOptionalVerification(result2, true, true); // now, verifying cleanliness
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().Throw<UnexpectedDirtinessException>();
        }

        [Fact]
        public void LazinatorMultidimensionalArray()
        {
            MultidimensionalArray GetObject()
            {
                return new MultidimensionalArray()
                {
                    MyArrayInt = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } }
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var result = original.CloneLazinatorTyped();
            for (int i0 = 0; i0 < 2; i0++)
                for (int i1 = 0; i1 < 3; i1++)
                    result.MyArrayInt[i0, i1].Equals(copy.MyArrayInt[i0, i1]).Should().BeTrue();
        }


        [Fact]
        public void LazinatorJaggedArray()
        {
            Array_Values GetObject()
            {
                return new Array_Values()
                {
                    MyJaggedArrayInt = new int[][] { new int[] { 1, 2, 3 }, new int[] { 4, 5, 6 } }
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var result = original.CloneLazinatorTyped();
            for (int i0 = 0; i0 < 2; i0++)
                for (int i1 = 0; i1 < 3; i1++)
                    result.MyJaggedArrayInt[i0][i1].Equals(copy.MyJaggedArrayInt[i0][i1]).Should().BeTrue();
        }



        [Fact]
        public void LazinatorDotNetHashSetOfSerializedItems()
        {
            DotNetHashSet_Lazinator GetObject(int thirdItemIndex)
            {
                return new DotNetHashSet_Lazinator()
                {
                    MyHashSetSerialized = new HashSet<ExampleChild>()
                    {
                        GetExampleChild(1),
                        GetExampleChild(3), // inherited item
                        GetExampleChild(thirdItemIndex), // could be just like first item, but could be different
                        null // null item
                    }
                };
            }

            bool HashSetsEqual(HashSet<ExampleChild> one, HashSet<ExampleChild> two)
            {
                var oneList = one.ToList();
                var twoList = two.ToList();
                for (int i = 0; i < Math.Max(oneList.Count, twoList.Count); i++)
                    if (!ExampleChildEqual(oneList[i], twoList[i]))
                        return false;
                return true;
            }

            var original = GetObject(1);
            var copy = GetObject(1);
            var copyWithGoal = GetObject(2);
            var result = original.CloneLazinatorTyped();
            HashSetsEqual(result.MyHashSetSerialized, copy.MyHashSetSerialized).Should().BeTrue();
        }

        [Fact]
        public void LazinatorDotNetListOfSerializedItems()
        {
            DotNetList_Lazinator GetObject(int thirdItemIndex)
            {
                return new DotNetList_Lazinator()
                {
                    MyListSerialized = new List<ExampleChild>()
                    {
                        GetExampleChild(1),
                        GetExampleChild(3), // inherited item
                        GetExampleChild(thirdItemIndex), // could be just like first item, but could be different
                        null // null item
                    },

                };
            }

            bool ListsEqual(List<ExampleChild> one, List<ExampleChild> two)
            {
                for (int i = 0; i < Math.Max(one.Count, two.Count); i++)
                    if (!ExampleChildEqual(one[i], two[i]))
                        return false;
                return true;
            }

            var original = GetObject(1);
            var copy = GetObject(1);
            var copyWithGoal = GetObject(2);
            var result = original.CloneLazinatorTyped();
            ListsEqual(result.MyListSerialized, copy.MyListSerialized).Should().BeTrue();
            // make sure that updates are registered when dirty flag is set
            result.MyListSerialized[2] = GetExampleChild(2);
            result.MyListSerialized_Dirty = true;
            var result2 = result.CloneLazinatorTyped();
            ListsEqual(result2.MyListSerialized, copyWithGoal.MyListSerialized).Should().BeTrue();
            // if we make a change but don't set dirty, nothing happens if we don't verify cleanliness
            var itemRemoved = result2.MyListSerialized[2];
            result2.MyListSerialized.Add(null);
            var result3 = result2.CloneLazinatorTyped();
            ListsEqual(result3.MyListSerialized, copyWithGoal.MyListSerialized).Should().BeTrue(); // the change is ignored, since dirtiness flag wasn't set
            // make sure that error is thrown if we do verify cleanliness
            Action attemptToVerifyCleanlinessWithoutSettingDirtyFlag = () => CloneWithOptionalVerification(result2, true, true); // now, verifying cleanliness
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().Throw<UnexpectedDirtinessException>();
        }

        [Fact]
        public void LazinatorDotNetDictionaryOfSerializedItems()
        {
            Dictionary_Values_Lazinator GetObject()
            {
                return new Dictionary_Values_Lazinator()
                {
                    MyDictionary = new Dictionary<int, ExampleChild>()
                    {
                        { 100, GetExampleChild(1) },
                        { 101, GetExampleChild(2) }, // inherited item
                        { 102, GetExampleChild(2) } // could be just like first item, but could be different
                    }
                };
            }

            bool DictionariesEqual(Dictionary<int, ExampleChild> one, Dictionary<int, ExampleChild> two)
            {
                foreach (KeyValuePair<int, ExampleChild> oneElement in one.AsEnumerable())
                {
                    ExampleChild oneChild = oneElement.Value;
                    ExampleChild twoChild = two[oneElement.Key];
                    bool match = ExampleChildEqual(oneChild, twoChild);
                }
                return true;
            }

            var original = GetObject();
            var copy = GetObject();
            var result = original.CloneLazinatorTyped();
            DictionariesEqual(result.MyDictionary, copy.MyDictionary).Should().BeTrue();
        }

        [Fact]
        public void LazinatorDotNetSortedDictionaryOfSerializedItems()
        {
            Dictionary_Values_Lazinator GetObject()
            {
                return new Dictionary_Values_Lazinator()
                {
                    MySortedDictionary = new SortedDictionary<int, ExampleChild>()
                    {
                        { 100, GetExampleChild(1) },
                        { 101, GetExampleChild(2) }, // inherited item
                        { 102, GetExampleChild(2) } // could be just like first item, but could be different
                    }
                };
            }

            bool DictionariesEqual(SortedDictionary<int, ExampleChild> one, SortedDictionary<int, ExampleChild> two)
            {
                foreach (KeyValuePair<int, ExampleChild> oneElement in one.AsEnumerable())
                {
                    ExampleChild oneChild = oneElement.Value;
                    ExampleChild twoChild = two[oneElement.Key];
                    bool match = ExampleChildEqual(oneChild, twoChild);
                }
                return true;
            }

            var original = GetObject();
            var copy = GetObject();
            var result = original.CloneLazinatorTyped();
            DictionariesEqual(result.MySortedDictionary, copy.MySortedDictionary).Should().BeTrue();
        }


        [Fact]
        public void LazinatorDotNetSortedListOfSerializedItems()
        {
            Dictionary_Values_Lazinator GetObject()
            {
                return new Dictionary_Values_Lazinator()
                {
                    MySortedList = new SortedList<int, ExampleChild>()
                    {
                        { 100, GetExampleChild(1) },
                        { 101, GetExampleChild(2) }, // inherited item
                        { 102, GetExampleChild(2) } // could be just like first item, but could be different
                    }
                };
            }

            bool DictionariesEqual(SortedList<int, ExampleChild> one, SortedList<int, ExampleChild> two)
            {
                foreach (KeyValuePair<int, ExampleChild> oneElement in one.AsEnumerable())
                {
                    ExampleChild oneChild = oneElement.Value;
                    ExampleChild twoChild = two[oneElement.Key];
                    bool match = ExampleChildEqual(oneChild, twoChild);
                }
                return true;
            }

            var original = GetObject();
            var copy = GetObject();
            var result = original.CloneLazinatorTyped();
            DictionariesEqual(result.MySortedList, copy.MySortedList).Should().BeTrue();
        }

        [Fact]
        public void LazinatorValueTuple()
        {

            void ConfirmSerializeAndDeserialize(int secondIndex, int thirdIndex)
            {
                StructTuple GetObject()
                {
                    return new StructTuple()
                    {
                        MyValueTupleSerialized = (3, GetExampleChild(secondIndex),
                            GetNonLazinatorType(thirdIndex))
                    };
                }

                var original = GetObject();
                var result = original.CloneLazinatorTyped();
                result.MyValueTupleSerialized.Item1.Should().Be(original.MyValueTupleSerialized.Item1);
                ExampleChildEqual(result.MyValueTupleSerialized.Item2, original.MyValueTupleSerialized.Item2)
                    .Should().BeTrue();
                NonLazinatorTypeEqual(result.MyValueTupleSerialized.Item3, original.MyValueTupleSerialized.Item3)
                    .Should().BeTrue();
            }

            for (int a = 0; a <= 3; a++)
                for (int b = 0; b <= 3; b++)
                    ConfirmSerializeAndDeserialize(a, b);
        }

        [Fact]
        public void LazinatorValueTuple_LazinatorStruct()
        {
            StructTuple GetObject()
            {
                return new StructTuple()
                {
                    MyValueTupleStructs = (3, 4) // WInt32s
                };
            }

            var original = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyValueTupleStructs.Item1.WrappedValue.Should().Be(3);
            result.MyValueTupleStructs.Item2.WrappedValue.Should().Be(4);
        }

        [Fact]
        public void LazinatorValueTuple_LazinatorNullableStructs()
        {
            StructTuple GetObject()
            {
                return new StructTuple()
                {
                    MyValueTupleNullableStructs = (new ExampleStructContainingClasses() { MyChar = 'a' }, null, new ExampleStructContainingClasses() { MyChar = 'b' })
                };
            }

            var original = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyValueTupleNullableStructs.Item1.Value.MyChar.Should().Be('a');
            result.MyValueTupleNullableStructs.Item2.HasValue.Should().BeFalse();
            result.MyValueTupleNullableStructs.Item3.Value.MyChar.Should().Be('b');
        }

        [Fact]
        public void LazinatorRegularTuple()
        {

            void ConfirmSerializeAndDeserialize(int secondIndex, int thirdIndex)
            {
                RegularTuple GetObject()
                {
                    return new RegularTuple()
                    {
                        MyTupleSerialized = new Tuple<uint, ExampleChild, NonLazinatorClass>(3, GetExampleChild(secondIndex),
                            GetNonLazinatorType(thirdIndex))
                    };
                }

                var original = GetObject();
                var result = original.CloneLazinatorTyped();
                result.MyTupleSerialized.Item1.Should().Be(original.MyTupleSerialized.Item1);
                ExampleChildEqual(result.MyTupleSerialized.Item2, original.MyTupleSerialized.Item2)
                    .Should().BeTrue();
                NonLazinatorTypeEqual(result.MyTupleSerialized.Item3, original.MyTupleSerialized.Item3)
                    .Should().BeTrue();
            }

            for (int a = 0; a <= 3; a++)
                for (int b = 0; b <= 3; b++)
                    ConfirmSerializeAndDeserialize(a, b);
        }


        [Fact]
        public void ListRegularTuple()
        {
            RegularTuple GetObject()
            {
                return new RegularTuple()
                {
                    MyListTuple = new List<Tuple<uint, ExampleChild, NonLazinatorClass>>()
                    {
                        new Tuple<uint, ExampleChild, NonLazinatorClass>(3, GetExampleChild(0),
                        GetNonLazinatorType(0)),
                        new Tuple<uint, ExampleChild, NonLazinatorClass>(4, GetExampleChild(1),
                        GetNonLazinatorType(1)),
                        new Tuple<uint, ExampleChild, NonLazinatorClass>(4, GetExampleChild(2),
                        GetNonLazinatorType(2)),
                    }
                };
            }

            var original = GetObject();
            var result = original.CloneLazinatorTyped();

            for (int i = 0; i < 3; i++)
            {
                result.MyListTuple[i].Item1.Should().Be(original.MyListTuple[i].Item1);
                ExampleChildEqual(result.MyListTuple[i].Item2, original.MyListTuple[i].Item2)
                    .Should().BeTrue();
                NonLazinatorTypeEqual(result.MyListTuple[i].Item3, original.MyListTuple[i].Item3)
                    .Should().BeTrue();
            }
        }


        [Fact]
        public void LazinatorDotNetListOfNonSerializedItems()
        {
            DotNetList_NonLazinator GetNonObject(int thirdItemIndex)
            {
                return new DotNetList_NonLazinator()
                {
                    MyListNonLazinatorType = new List<NonLazinatorClass>()
                    {
                        GetNonLazinatorType(1),
                        GetNonLazinatorType(3),
                        GetNonLazinatorType(thirdItemIndex), // could be just like first item, but could be different
                        null // null item
                    }
                };
            }

            bool ListsEqual(List<NonLazinatorClass> one, List<NonLazinatorClass> two)
            {
                for (int i = 0; i < Math.Max(one.Count, two.Count); i++)
                    if (!NonLazinatorTypeEqual(one[i], two[i]))
                        return false;
                return true;
            }

            var original = GetNonObject(1);
            var copy = GetNonObject(1);
            var copyWithGoal = GetNonObject(2);
            var result = original.CloneLazinatorTyped();
            ListsEqual(result.MyListNonLazinatorType, copy.MyListNonLazinatorType).Should().BeTrue();
            // make sure that updates are registered when dirty flag is set
            result.MyListNonLazinatorType[2] = GetNonLazinatorType(2);
            result.MyListNonLazinatorType_Dirty = true;
            var result2 = result.CloneLazinatorTyped();
            ListsEqual(result2.MyListNonLazinatorType, copyWithGoal.MyListNonLazinatorType).Should().BeTrue();
            // if we make a change but don't set dirty, nothing happens if we don't verify cleanliness
            var itemRemoved = result2.MyListNonLazinatorType[2];
            result2.MyListNonLazinatorType[2] = null;
            var result3 = result2.CloneLazinatorTyped();
            ListsEqual(result3.MyListNonLazinatorType, copyWithGoal.MyListNonLazinatorType).Should().BeTrue(); // the change is ignored, since dirtiness flag wasn't set
            // make sure that error is thrown if we do verify cleanliness
            Action attemptToVerifyCleanlinessWithoutSettingDirtyFlag = () => CloneWithOptionalVerification(result2, true, true); // now, verifying cleanliness
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().Throw<UnexpectedDirtinessException>();
            // also make sure that error is thrown if we verify cleanliness when change is to individual item in list
            result2.MyListNonLazinatorType[2] = itemRemoved;
            itemRemoved.MyInt = -987;
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().Throw<UnexpectedDirtinessException>();
        }


        [Fact]
        public void LazinatorDotNetNestedListOfNonSerializedItems()
        {
            DotNetList_Nested_NonLazinator GetNestedList(int index)
            {
                return new DotNetList_Nested_NonLazinator()
                {
                    MyListNestedNonLazinatorType = new List<List<NonLazinatorClass>>()
                    {
                        new List<NonLazinatorClass>()
                        {
                            GetNonLazinatorType(1),
                            null,
                            GetNonLazinatorType(3),
                        },
                        new List<NonLazinatorClass>()
                        {
                            GetNonLazinatorType(2),
                            GetNonLazinatorType(index),
                        },
                        new List<NonLazinatorClass>()
                        {
                        },
                        null // null item
                    }
                };
            }

            bool ListsEqual(List<List<NonLazinatorClass>> one, List<List<NonLazinatorClass>> two)
            {
                for (int i = 0; i < Math.Max(one.Count, two.Count); i++)
                    if (one[i] != null || two[i] != null)
                        for (int j = 0; j < Math.Max(one[i].Count, two[i].Count); j++)
                            if (!NonLazinatorTypeEqual(one[i][j], two[i][j]))
                                return false;
                return true;
            }

            var original = GetNestedList(2);
            var copy = GetNestedList(2);
            var copyWithGoal = GetNestedList(1);
            var result = original.CloneLazinatorTyped();
            ListsEqual(result.MyListNestedNonLazinatorType, copy.MyListNestedNonLazinatorType).Should().BeTrue();
            // make sure that updates serialize; because we're not tracking dirtiness, the reserialization should occur simply because this was deserialized
            result.MyListNestedNonLazinatorType[1][1] = GetNonLazinatorType(1);
            var result2 = result.CloneLazinatorTyped();
            ListsEqual(result2.MyListNestedNonLazinatorType, copyWithGoal.MyListNestedNonLazinatorType).Should().BeTrue();
        }

        [Fact]
        public void LazinatorDerivedDotNetNestedListOfNonSerializedItems()
        {
            Derived_DotNetList_Nested_NonLazinator GetNestedList(int index)
            {
                return new Derived_DotNetList_Nested_NonLazinator()
                {
                    MyLevel2ListNestedNonLazinatorType = new List<List<NonLazinatorClass>>()
                    {
                        new List<NonLazinatorClass>()
                        {
                            GetNonLazinatorType(1),
                            null,
                            GetNonLazinatorType(3),
                        },
                        new List<NonLazinatorClass>()
                        {
                            GetNonLazinatorType(2),
                            GetNonLazinatorType(index),
                        },
                        new List<NonLazinatorClass>()
                        {
                        },
                        null // null item
                    }
                };
            }

            bool ListsEqual(List<List<NonLazinatorClass>> one, List<List<NonLazinatorClass>> two)
            {
                for (int i = 0; i < Math.Max(one.Count, two.Count); i++)
                    if (one[i] != null || two[i] != null)
                        for (int j = 0; j < Math.Max(one[i].Count, two[i].Count); j++)
                            if (!NonLazinatorTypeEqual(one[i][j], two[i][j]))
                                return false;
                return true;
            }

            var original = GetNestedList(2);
            var copy = GetNestedList(2);
            var copyWithGoal = GetNestedList(1);
            var result = original.CloneLazinatorTyped();
            ListsEqual(result.MyLevel2ListNestedNonLazinatorType, copy.MyLevel2ListNestedNonLazinatorType).Should().BeTrue();
            // make sure that updates serialize; because we're not tracking dirtiness, the reserialization should occur simply because this was deserialized
            result.MyLevel2ListNestedNonLazinatorType[1][1] = GetNonLazinatorType(1);
            var result2 = result.CloneLazinatorTyped();
            ListsEqual(result2.MyLevel2ListNestedNonLazinatorType, copyWithGoal.MyLevel2ListNestedNonLazinatorType).Should().BeTrue();
        }

        [Fact]
        public void RecordLikeTypes_DefaultValues()
        {
            RecordLikeContainer original = new RecordLikeContainer()
            {
            };
            var clone = original.CloneLazinatorTyped();
            clone.MyRecordLikeClass.Should().BeNull();
            clone.MyRecordLikeStruct.Age.Should().Be(0); // default value in struct
        }

        [Fact]
        public void RecordLikeTypes()
        {
            RecordLikeContainer original = new RecordLikeContainer()
            {
                MyRecordLikeClass = new RecordLikeClass(23, new Example() { MyChar = 'q' }),
                MyRecordLikeStruct = new RecordLikeStruct(12, "Sam")
            };
            LazinatorMemory serialized = original.SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
            RecordLikeContainer s2 = new RecordLikeContainer()
            {
            };
            s2.DeserializeLazinator(serialized);
            s2.MyRecordLikeClass.Age.Should().Be(23);
            s2.MyRecordLikeClass.Example.MyChar.Should().Be('q');
            s2.MyRecordLikeStruct.Age.Should().Be(12);
            s2.MyRecordLikeStruct.Name.Should().Be("Sam");
            s2.DescendantIsDirty.Should().BeFalse(); // no automatic dirtiness tracking
            s2.IsDirty.Should().BeTrue(); // since no automatic dirtiness tracking, assumed dirty on first access
        }




        [Fact]
        public void NonLazinatorRecordTypes()
        {
            RecordLikeContainer original = new RecordLikeContainer()
            {
                MyNonLazinatorRecordWithConstructor = new NonLazinatorRecordWithConstructor(20, new Example() { MyChar = 'q' }, 23.4, 7),
                MyNonLazinatorRecordWithoutConstructor = new NonLazinatorRecordWithoutConstructor()
                {
                    Age = 21,
                    Example = new Example() { MyChar = 'w' },
                    DoubleValue = 23.5,
                    NullableInt = 8
                }
            };
            LazinatorMemory serialized = original.SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
            RecordLikeContainer s2 = new RecordLikeContainer()
            {
            };
            s2.DeserializeLazinator(serialized);
            s2.MyNonLazinatorRecordWithConstructor.Age.Should().Be(20);
            s2.MyNonLazinatorRecordWithConstructor.Example.MyChar.Should().Be('q');
            s2.MyNonLazinatorRecordWithConstructor.DoubleValue.Should().Be(23.4);
            s2.MyNonLazinatorRecordWithConstructor.NullableInt.Should().Be(7);

            s2.MyNonLazinatorRecordWithoutConstructor.Age.Should().Be(21);
            s2.MyNonLazinatorRecordWithoutConstructor.Example.MyChar.Should().Be('w');
            s2.MyNonLazinatorRecordWithoutConstructor.DoubleValue.Should().Be(23.5);
            s2.MyNonLazinatorRecordWithoutConstructor.NullableInt.Should().Be(8);


            s2.DescendantIsDirty.Should().BeFalse(); // no automatic dirtiness tracking
            s2.IsDirty.Should().BeTrue(); // since no automatic dirtiness tracking, assumed dirty on first access
        }
    }
}
