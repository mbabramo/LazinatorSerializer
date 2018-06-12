using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using Lazinator.Exceptions;
using Lazinator.Support;
using Lazinator.Buffers; 
using Lazinator.Core; 
using LazinatorTests.Examples.Tuples;
using Xunit;
using ExampleNonexclusiveInterfaceImplementer = LazinatorTests.Examples.ExampleNonexclusiveInterfaceImplementer;
using Lazinator.Wrappers;
using System.Buffers;
using System.Reflection;
using Lazinator.Spans;
using System.Collections;
using LazinatorTests.Examples.Abstract;
using LazinatorTests.Examples.Hierarchy;
using LazinatorTests.Examples.NonLazinator;
using LazinatorTests.Examples.Structs;
using LazinatorTests.Examples.Subclasses;
using LazinatorTests.Examples.NonAbstractGenerics;

namespace LazinatorTests.Tests
{
    public class SerializationDeserializationTest
    {
        [Fact]
        public void MemoryPoolWorks()
        {
            const int bufferSize = 64 * 1024;
            for (int i = 0; i < 10; i++)
            { // note: If we set this to a very large number of iterations, we can confirm that there is no memory leak here. If, on the other hand, we add each rented memory to a list that stays in memory, then we quickly use all available system memory. But we will do neither for the purpose of general testing.
                IMemoryOwner<byte> rentedMemory = LazinatorUtilities.GetRentedMemory(bufferSize);
                var bytes = rentedMemory.Memory;
                bytes.Length.Should().BeGreaterOrEqualTo(bufferSize);
            }
        }

        [Fact]
        public void BinaryBufferWriterCanBeCreated()
        {
            const int bufferSize = 64 * 1024;
            for (int i = 0; i < 1; i++)
            { // same as above; higher iterations causes no memory leak
                BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
                var rented = writer.MemoryInBuffer;
                rented.OwnedMemory.Memory.Length.Should().BeGreaterOrEqualTo(bufferSize);
            }
        }

        [Fact]
        public void CanWriteBeyondInitialBufferSize()
        {
            const int bufferSize = 1024;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
            for (int j = 0; j < 5000; j++)
                writer.Write(j);
            var written = writer.Written;
            for (int j = 0; j < 5000; j++)
            {
                int index = j * sizeof(int);
                ReadUncompressedPrimitives.ToInt32(written, ref index).Should().Be(j);
            }
        }

        [Fact]
        public void BasicSelfSerializationWorks()
        {
            var original = GetHierarchy(1, 1, 1, 1, 0);
            var copy = GetHierarchy(1, 1, 1, 1, 0);
            var result = original.CloneLazinatorTyped(); 
            ExampleEqual(copy, result).Should().BeTrue();
        }

        [Fact]
        public void SelfSerializationCanSetChildToNull()
        {
            var original = GetHierarchy(1, 1, 1, 1, 0);
            var result = original.CloneLazinatorTyped();
            result.MyChild1 = null;
            var result2 = result.CloneLazinatorTyped();
            result2.MyChild1.Should().Be(null);
        }

        [Fact]
        public void SelfSerializationVersionUpgradeWorks()
        {
            var original = GetHierarchy(1, 1, 1, 1, 0);
            // Set to old version number. This should serialize as the old version number.
            original.LazinatorObjectVersion = 2;
            original.MyOldString = "Old string";
            var bytes = original.SerializeNewBuffer(IncludeChildrenMode.ExcludeAllChildren, false); // serializes as version 3
            // Now, deserializing, but again setting to the old version.
            var stillOldVersion = new Example
            {
                LazinatorObjectVersion = 2,
                HierarchyBytes = bytes,
            };
            stillOldVersion.MyOldString.Should().Be("Old string");
            stillOldVersion.MyNewString.Should().Be(null);

            var upgraded = new Example
            {
                LazinatorObjectVersion = 3,
                HierarchyBytes = bytes,
            };
            upgraded.LazinatorObjectVersion.Should().Be(3);
            upgraded.MyOldString.Should().Be(null);
            upgraded.MyNewString.Should().Be("NEW Old string");
        }

        [Fact]
        public void SelfSerializationRecordLikeTypes()
        {
            RecordLikeContainer original = new RecordLikeContainer()
            {
                MyRecordLikeClass = new RecordLikeClass(23, new Example() { MyChar = 'q'}),
                MyRecordLikeType = new RecordLikeType(12, "Sam")
            };
            MemoryInBuffer serialized = original.SerializeNewBuffer(IncludeChildrenMode.IncludeAllChildren, false);
            RecordLikeContainer s2 = new RecordLikeContainer()
            {
                HierarchyBytes = serialized
            };
            s2.MyRecordLikeClass.Age.Should().Be(23);
            s2.MyRecordLikeClass.Example.MyChar.Should().Be('q');
            s2.MyRecordLikeType.Age.Should().Be(12);
            s2.MyRecordLikeType.Name.Should().Be("Sam");
            s2.DescendantIsDirty.Should().BeFalse(); // no automatic dirtiness tracking
            s2.IsDirty.Should().BeTrue(); // since no automatic dirtiness tracking, assumed dirty on first access
        }


        [Fact]
        public void ExampleInterfaceContainerWorks()
        {
            ExampleInterfaceContainer c = new ExampleInterfaceContainer()
            {
                ExampleByInterface = GetExample(2),
            };

            var c2 = c.CloneLazinatorTyped();
            ExampleEqual((Example) c.ExampleByInterface, (Example) c2.ExampleByInterface).Should().BeTrue();
        }

        [Fact]
        public void RecursiveExampleWorks()
        {
            // the Recursive example allows us to build a tree
            RecursiveExample r = new RecursiveExample()
            {
                RecursiveClass = new RecursiveExample(),
                RecursiveInterface = new RecursiveExample()
                {
                    RecursiveClass = new RecursiveExample()
                },
            };
            var r2 = r.CloneLazinatorTyped();
            r2.RecursiveClass.Should().NotBeNull();
            r2.RecursiveInterface.Should().NotBeNull();
            r2.RecursiveInterface.RecursiveClass.Should().NotBeNull();
        }

        [Fact]
        public void ClassContainingStructWorks()
        {
            ContainerForExampleStructWithoutClass c = new ContainerForExampleStructWithoutClass()
            {
                ExampleStructWithoutClass = new ExampleStructWithoutClass() { MyInt = 3 }
            };

            var c2 = c.CloneLazinatorTyped();
            c2.ExampleStructWithoutClass.MyInt.Should().Be(3);
        }

        [Fact]
        public void ClassContainingStructContainingClassThrows()
        {
            ExampleStructContainer c = new ExampleStructContainer()
            {
                MyExampleStruct = new ExampleStruct() { MyChar = 'z', MyLazinatorList = new List<Example>() }
            };
            var c2 = c.CloneLazinatorTyped();

            Action a = () => { var result = c2.MyExampleStruct; };
            a.Should().Throw<LazinatorDeserializationException>();
        }

        [Fact]
        public void CopyPropertyForStructWorks()
        {
            ContainerForExampleStructWithoutClass c = new ContainerForExampleStructWithoutClass()
            {
                ExampleStructWithoutClass = new ExampleStructWithoutClass() { MyInt = 3 }
            };

            var c2 = c.CloneLazinatorTyped();
            var copyOfStruct = c2.ExampleStructWithoutClass;
            copyOfStruct.MyInt.Should().Be(3);
            copyOfStruct.MyInt = 4;
            c2.IsDirty.Should().BeFalse(); // no effect of change on copy
            c2.ExampleStructWithoutClass.MyInt.Should().Be(3);

            // let's confirm that this has been created on the stack (which is the purpose of the property)
            bool IsBoxed<T>(T value)
            {
                return
                    (typeof(T).IsInterface || typeof(T) == typeof(object)) &&
                    value != null &&
                    value.GetType().IsValueType;
            }
            bool IsReferenceType<T>(T input)
            {
                object surelyBoxed = input;
                return object.ReferenceEquals(surelyBoxed, input);
            }
            IsBoxed(copyOfStruct).Should().BeFalse();
            IsReferenceType(copyOfStruct).Should().BeFalse();
        }

        [Fact]
        public void StructContainingStructWorks()
        {
            ExampleStructContainingStruct c = new ExampleStructContainingStruct()
            {
                MyExampleStruct = new ExampleStruct() { MyChar = 'z', MyLazinatorList = new List<Example>() }
            };

            var c2 = CloneWithOptionalVerification(c, true, false);
            c2.MyExampleStruct.MyChar.Should().Be('z');
        }

        [Fact]
        public void StructLazinatorWorks()
        {
            ExampleStruct s = new ExampleStruct();
            s.MyBool = true;
            s.MyChar = 'x';
            s.MyChild1 = new ExampleChildInherited()
            {
                MyInt = 34,
                MyLong = 341341
            };
            s.MyListValues = new List<int>() { 3, 4 };
            s.MyTuple = (new NonLazinatorClass() { MyInt = 5 }, 4);
            s.MyLazinatorList = new List<Example>() { new Example() };

            var s2 = s.CloneLazinatorTyped();
            s2.MyBool.Should().BeTrue();
            s2.MyChar.Should().Be('x');
            s2.MyChild1.Should().NotBeNull();
            var child1 = (ExampleChildInherited)s2.MyChild1;
            child1.Should().NotBeNull();
            child1.MyInt.Should().Be(34);
            child1.MyLong.Should().Be(341341);
            s2.IsDirty.Should().BeFalse();
            s2.MyListValues.Should().NotBeNull(); // will cause IsDirty to be false
            s2.MyListValues.Count().Should().Be(2);
            s2.MyTuple.Item1.MyInt.Should().Be(5);
            s2.MyTuple.Item2.Should().Be(4);
            s2.MyLazinatorList.Count().Should().Be(1);
            // make sure that parent knows of descendant dirtiness (remember that the technique is different with structs)
            s2.IsDirty.Should().BeTrue(); // as a result of access above
            s2.DescendantIsDirty.Should().BeFalse();
            child1.MyLong = 17;
            s2.DescendantIsDirty.Should().BeTrue();

            var s3 = s.CloneLazinatorTyped();
            s3.IsDirty.Should().Be(false);
            s3.MyLazinatorList[0] = new Example() { MyChar = 'y' };
            s3.IsDirty.Should().Be(false);
            s3.DescendantIsDirty.Should().Be(false); // struct can't be informed about this kind of change
            Action reserializationAction = () => s3.SerializeNewBuffer(IncludeChildrenMode.IncludeAllChildren, true);
            reserializationAction.Should().Throw<UnexpectedDirtinessException>();
            s3.MyLazinatorList_Dirty = true;
            var s3Serialized = s3.SerializeNewBuffer(IncludeChildrenMode.IncludeAllChildren, true);
            ExampleStruct s3b = new ExampleStruct()
            {
                HierarchyBytes = s3Serialized
            };
            s3b.MyLazinatorList[0].MyChar.Should().Be('y');

            var s4 = s.CloneLazinatorTyped();
            s4.IsDirty.Should().Be(false);
            s4.MyListValues[0] = -12345;
            s4.IsDirty.Should().Be(true); // just accessing should set IsDirty to true
            s4.DescendantIsDirty.Should().Be(false); // struct can't be informed about this kind of change

            var s5 = s4.CloneLazinatorTyped();
            s5.MyListValues[0].Should().Be(-12345); // confirm proper serialization
        }

        [Fact]
        public void SerializationWithoutChildrenWorks()
        {
            var original = GetHierarchy(1, 1, 1, 1, 1);
            // make a copy, but manually delete all children -- i.e., lazinator types including interface implementers (open generics also count)
            var copy = GetHierarchy(1, 1, 1, 1, 1);
            copy.MyChild1 = null;
            copy.MyChild2 = null;
            copy.MyInterfaceImplementer = null;
            /* now, clone the original, automatically deleting the children */
            var result = original.CloneLazinatorTyped(IncludeChildrenMode.ExcludeAllChildren);
            ExampleEqual(copy, result).Should().BeTrue();
            // now, serialize again
            var result2 = result.CloneLazinatorTyped(IncludeChildrenMode.ExcludeAllChildren);
            ExampleEqual(copy, result2).Should().BeTrue();
            // and again -- this time include children but they should be null
            var result3 = result2.CloneLazinatorTyped();
            ExampleEqual(copy, result3).Should().BeTrue();
        }

        [Fact]
        public void ChildInclusionOptionsWork_IncludeAllChildren()
        {
            var original = GetHierarchy(1, 1, 1, 1, 1);
            original.IncludableChild = GetExampleChild(1);
            original.ExcludableChild = GetExampleChild(1);

            var result = original.CloneLazinatorTyped(IncludeChildrenMode.ExcludeAllChildren);
            result.IncludableChild.Should().BeNull();
            result.ExcludableChild.Should().BeNull();
            result.MyChild1.Should().BeNull();

            result = original.CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren);
            result.IncludableChild.Should().NotBeNull();
            result.ExcludableChild.Should().NotBeNull();
            result.MyChild1.Should().NotBeNull();

            result = original.CloneLazinatorTyped(IncludeChildrenMode.ExcludeOnlyExcludableChildren);
            result.IncludableChild.Should().NotBeNull();
            result.ExcludableChild.Should().BeNull();
            result.MyChild1.Should().NotBeNull();

            result = original.CloneLazinatorTyped(IncludeChildrenMode.IncludeOnlyIncludableChildren);
            result.IncludableChild.Should().NotBeNull();
            result.ExcludableChild.Should().BeNull();
            result.MyChild1.Should().BeNull();
        }

        [Fact]
        public void LazinatorDotNetListInt()
        {
            DotNetList_Values GetObject(int thirdItem)
            {
                return new DotNetList_Values()
                {
                    MyListInt = new List<int>() {3, 4, thirdItem}
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
                    MyListInt = new List<WInt>() { 3, 4, thirdItem }
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
                MyListNullableInt = new List<WNullableInt>() { 3, 16000, 249, null, 1000000000 }
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
            ExampleStructContainer GetObject()
            {
                return new ExampleStructContainer()
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
        public void LazinatorDotNetListLazinatorNullableStructs()
        {
            ExampleStructContainer GetObject()
            {
                var returnObj = new ExampleStructContainer()
                {
                    MyListNullableExampleStruct = new List<WNullableStruct<ExampleStruct>>()
                    {
                        new WNullableStruct<ExampleStruct>() { AsNullableStruct = new ExampleStruct() { MyChar = 'd' } },
                        new WNullableStruct<ExampleStruct>() { AsNullableStruct = null },
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
        public void LazinatorDotNetListLazinatorStructs_Empty()
        {
            ExampleStructContainer GetObject()
            {
                return new ExampleStructContainer()
                {
                    MyListExampleStruct = new List<ExampleStruct>(),
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
            ExampleStructContainer GetObject()
            {
                var returnObj = new ExampleStructContainer()
                {
                    MyListExampleStruct = new List<ExampleStruct>()
                    {
                        new ExampleStruct() { MyChar = 'd'},
                        new ExampleStruct() { MyChar = 'e'},
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
        public void LazinatorDotNetListLazinator_Null()
        {
            DotNetList_SelfSerialized GetObject()
            {
                return new DotNetList_SelfSerialized()
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
            DotNetList_SelfSerialized GetObject()
            {
                return new DotNetList_SelfSerialized()
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
                    MyTupleSerialized4 = new Tuple<int, ExampleStruct>(3, new ExampleStruct() { MyChar = '5' })
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var copyWithGoal = GetObject();
            var result = original.CloneLazinatorTyped();
            result.MyTupleSerialized4.Item2.MyChar.Should().Be('5');
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
        public void WrapperNullableStructWorks()
        {
            WNullableStruct<ExampleStruct> w = new WNullableStruct<ExampleStruct>()
            {
                AsNullableStruct = new ExampleStruct()
                {
                    MyChar = 'q'
                }
            };
            var w2 = CloneWithOptionalVerification(w, true, false);
            w2.AsNullableStruct.Value.MyChar.Should().Be('q');
            w2.AsNullableStruct = null;
            var w3 = CloneWithOptionalVerification(w2, true, false);
            w3.AsNullableStruct.Should().Be(null);
        }

        [Fact]
        public void WrapperNullableGuidWorks()
        {
            Guid g = Guid.NewGuid();
            WNullableGuid w = new WNullableGuid(g);
            var w2 = CloneWithOptionalVerification(w, true, false);
            w2.WrappedValue.Should().Be(g);
        }

        [Fact]
        public void WrapperIntWorks()
        {
            ExampleStructContainer w = new ExampleStructContainer()
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
            a.CompareTo((string) null).Should().Be(1);
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
            c.WrappedByte.Should().Be((byte) 1);
            c.WrappedSByte.Should().Be((sbyte) -2);
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
            wrappedInt.WrappedValue.Should().Be(0);
            var clone = e.CloneLazinatorTyped();
            clone.WrappedInt.WrappedValue.Should().Be(0);
        }

        [Fact]
        public void NonLazinatorContainerWorks()
        {
            NonLazinatorContainer c = new NonLazinatorContainer()
            {
                NonLazinatorClass = new NonLazinatorClass() { MyInt = 5, MyString = "hi" },
                NonLazinatorStruct = new NonLazinatorStruct() { MyInt = 6, MyString = null }
            };
            var c2 = CloneWithOptionalVerification(c, true, false);
            c2.NonLazinatorClass.MyInt.Should().Be(5);
            c2.NonLazinatorClass.MyString.Should().Be("hi");
            c2.NonLazinatorStruct.MyInt.Should().Be(6);
            c2.NonLazinatorStruct.MyString.Should().Be(null);
        }


        [Fact]
        public void NonLazinatorInterchangeWorks()
        {
            NonLazinatorContainer c = new NonLazinatorContainer()
            {
                NonLazinatorClass = null,
                NonLazinatorStruct = new NonLazinatorStruct(),
                NonLazinatorInterchangeableClass = new NonLazinatorInterchangeableClass("hi", 5)
            };
            var c2 = CloneWithOptionalVerification(c, true, false);
            c2.NonLazinatorStruct.MyInt.Should().Be(0);
            c2.NonLazinatorStruct.MyString.Should().Be(null);
            // read a private field and a public field
            typeof(NonLazinatorInterchangeableClass).GetField("MyInt", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(c2.NonLazinatorInterchangeableClass).Should().Be(5);
            c2.NonLazinatorInterchangeableClass.MyString.Should().Be("hi");
        }

        [Fact]
        public void ClassWithNonLazinatorBaseWorks()
        {
            FromNonLazinatorBase b = new FromNonLazinatorBase()
            {
                MyInt = 3 // property defined virtually in base class
            };
            b.CloneLazinatorTyped().MyInt.Should().Be(3);
        }

        [Fact]
        public void ChildOfLazinatorWithoutAttributeWorks()
        {
            ChildOfLazinatorWithoutAttribute b = new ChildOfLazinatorWithoutAttribute()
            {
                MyInt = 3 // property defined virtually in base class
            };
            Action a = () =>
            {
                var typedClone = b.CloneLazinatorTyped();
            };
            a.Should().Throw<Exception>(); // we can't return an item of same type as ChildOfLazinatorWithoutAttribute, since Lazinator doesn't know about that type

            ILazinator untypedClone = b.CloneLazinator();
            (untypedClone as ChildOfLazinatorWithoutAttribute).Should().BeNull();
            (untypedClone as FromNonLazinatorBase).MyInt.Should().Be(3);
        }

        [Fact]
        public void LazinatorDotNetLinkedListInt()
        {
            DotNetList_Values GetObject(int thirdItem)
            {
                return new DotNetList_Values()
                {
                    MyLinkedListInt = new LinkedList<int>(new[]{ 3, 4, thirdItem })
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
            // if we make a change but don't set dirty, nothing happens if we don't verify cleanliness
            result2.MyQueueInt.Enqueue(7);
            var result3 = result2.CloneLazinatorTyped();
            result3.MyQueueInt.Count().Should().Be(4); // the change is ignored, since dirtiness flag wasn't set
            // make sure that error is thrown if we do verify cleanliness
            Action attemptToVerifyCleanlinessWithoutSettingDirtyFlag = () => CloneWithOptionalVerification(result2, true, true); // now, verifying cleanliness
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().Throw<UnexpectedDirtinessException>();
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
            // if we make a change but don't set dirty, nothing happens if we don't verify cleanliness
            result2.MyStackInt.Push(7);
            var result3 = result2.CloneLazinatorTyped();
            result3.MyStackInt.Count().Should().Be(4); // the change is ignored, since dirtiness flag wasn't set
            // make sure that error is thrown if we do verify cleanliness
            Action attemptToVerifyCleanlinessWithoutSettingDirtyFlag = () => CloneWithOptionalVerification(result2, true, true); // now, verifying cleanliness
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().Throw<UnexpectedDirtinessException>();
        }

        [Fact]
        public void LazinatorArrayInt()
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
            ArrayMultidimensional_Values GetObject()
            {
                return new ArrayMultidimensional_Values()
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
                    MyJaggedArrayInt = new int[][] { new int[] { 1, 2, 3 }, new int[]{ 4, 5, 6 } }
                };
            }

            var original = GetObject();
            var copy = GetObject();
            var result = original.CloneLazinatorTyped();
            for (int i0 = 0; i0 < 2; i0++)
                for (int i1 = 0; i1 < 3; i1++)
                    result.MyJaggedArrayInt[i0][i1].Equals(copy.MyJaggedArrayInt[i0] [i1]).Should().BeTrue();
        }


        [Fact]
        public void LazinatorReadOnlySpanInStruct()
        {
            // we have special code to deal with ReadOnlySpans within structs. So, here is a test to make sure that it works.
            WReadOnlySpanChar w = new WReadOnlySpanChar();
            w.Value = new Span<char>("mystring".ToArray());
            var result = CloneWithOptionalVerification(w, false, false);
            new string(result.Value).Should().Be("mystring");
        }

        [Fact]
        public void LazinatorReadOnlySpans()
        {
            var chars = "Hello, world".ToCharArray();

            var now = DateTime.Now;
            SpanAndMemory GetObject(bool emptySpans)
            {
                if (emptySpans)
                    return new SpanAndMemory
                    {
                        MyReadOnlySpanByte = new Span<byte>(new byte[] { }),
                        MyReadOnlyMemoryByte = new Memory<byte>(new byte[] { }),
                        MyReadOnlySpanDateTime = new Span<DateTime>(), // should also work with no array
                        MyReadOnlySpanLong = new Span<long>(new long[] { }),
                        
                    };
                return new SpanAndMemory
                {
                    MyReadOnlySpanByte = new Span<byte>(new byte[] { 3, 4, 5}),
                    MyReadOnlyMemoryByte = new Memory<byte>(new byte[] { 3, 4, 5 }),
                    MyReadOnlySpanDateTime = new Span<DateTime>(new DateTime[] { now }),
                    MyReadOnlySpanLong = new Span<long>(new long[] { -234234, long.MaxValue}),
                    MyReadOnlySpanChar = new ReadOnlySpan<char>(chars)
                };
            }

            var original = GetObject(false);
            var copy = GetObject(false);
            for (int i = 0; i < 3; i++)
            {
                var result = copy.CloneLazinatorTyped();
                result.MyReadOnlySpanByte.Length.Should().Be(3);
                result.MyReadOnlySpanByte[1].Should().Be(4);
                result.MyReadOnlyMemoryByte.Span[1].Should().Be(4);
                result.MyReadOnlyMemoryByte.Length.Should().Be(3);
                result.MyReadOnlySpanDateTime.Length.Should().Be(1);
                result.MyReadOnlySpanDateTime[0].Should().Be(now);
                result.MyReadOnlySpanLong.Length.Should().Be(2);
                result.MyReadOnlySpanLong[1].Should().Be(long.MaxValue);
                new string(result.MyReadOnlySpanChar).Equals("Hello, world").Should().BeTrue();
                new string(result.MyReadOnlySpanChar.Slice(0, 5)).Equals("Hello").Should().BeTrue();
                copy = result;
            }

            original = GetObject(true);
            copy = GetObject(true);
            for (int i = 0; i < 3; i++)
            {
                var result = copy.CloneLazinatorTyped();
                result.MyReadOnlySpanByte.Length.Should().Be(0);
                result.MyReadOnlyMemoryByte.Length.Should().Be(0);
                result.MyReadOnlySpanDateTime.Length.Should().Be(0);
                result.MyReadOnlySpanLong.Length.Should().Be(0);
                copy = result;
            }
        }

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

        [Fact]
        public void LazinatorMemoryInt()
        {
            SpanAndMemory GetObject(int thirdItem)
            {
                return new SpanAndMemory             {
                    MyMemoryInt = new int[] { 3, 4, thirdItem }
                };
            }

            void SetIndex(Memory<int> source, int index, int value)
            {
                // note: this seems to be a way to write into Memory<T>. I think that since Span lives only on the stack, the memory is guaranteed not to move. So, this works. I'm not clear why there isn't a direct way to index into Memory<T>, without getting a span. Maybe the reason is that they want to encourage you to get a Span if you are going to do a large number of writes. It's awkward that you can't say source.Span[index] = value; 
                var sourceSpan = source.Span;
                sourceSpan[index] = value;
            }
            bool SequenceEqual(Memory<int> a, Memory<int> b)
            {
                if (a.Length != b.Length)
                    return false;
                Span<int> aSpan = a.Span;
                Span<int> bSpan = b.Span;
                for (int i = 0; i < a.Length; i++)
                    if (aSpan[i] != bSpan[i])
                        return false;
                return true;
            }

            var original = GetObject(5);
            var copy = GetObject(5);
            SetIndex(copy.MyMemoryInt, 2, 6);
            var span = copy.MyMemoryInt.Span;
            span[2].Should().Be(6);
            var result = copy.CloneLazinatorTyped();
            SequenceEqual(copy.MyMemoryInt, result.MyMemoryInt).Should().BeTrue();
        }


        [Fact]
        public void LazinatorNullableMemoryInt()
        {
            SpanAndMemory GetObject()
            {
                return new SpanAndMemory
                {
                    MyNullableMemoryInt = new int[] { 3, 4, 5 }
                };
            }

            SpanAndMemory GetEmptyMemoryObject()
            {
                return new SpanAndMemory
                {
                    MyNullableMemoryInt = new int[] { }
                };
            }

            void SetIndex(Memory<int> source, int index, int value)
            {
                // note: this seems to be a way to write into Memory<T>. I think that since Span lives only on the stack, the memory is guaranteed not to move. So, this works. I'm not clear why there isn't a direct way to index into Memory<T>, without getting a span. Maybe the reason is that they want to encourage you to get a Span if you are going to do a large number of writes. It's awkward that you can't say source.Span[index] = value; 
                var sourceSpan = source.Span;
                sourceSpan[index] = value;
            }
            bool SequenceEqual(Memory<int> a, Memory<int> b)
            {
                if (a.Length != b.Length)
                    return false;
                Span<int> aSpan = a.Span;
                Span<int> bSpan = b.Span;
                for (int i = 0; i < a.Length; i++)
                    if (aSpan[i] != bSpan[i])
                        return false;
                return true;
            }

            // first, we'll do the same thing we did for the non-nullable field

            var original = GetObject();
            var copy = GetObject();
            SetIndex(copy.MyNullableMemoryInt.Value, 2, 6);
            var span = copy.MyNullableMemoryInt.Value.Span;
            span[2].Should().Be(6);
            var result = copy.CloneLazinatorTyped();
            SequenceEqual(copy.MyNullableMemoryInt.Value, result.MyNullableMemoryInt.Value).Should().BeTrue();
            result.MyMemoryInt.Length.Should().Be(0);

            // now, let's make sure that null serializes correctly
            original = new SpanAndMemory();
            result = original.CloneLazinatorTyped();
            result.MyNullableMemoryInt.Should().Be(null);
            result.MyMemoryInt.Length.Should().Be(0);

            // and empty list must serialize correctly too
            original = GetEmptyMemoryObject();
            result = original.CloneLazinatorTyped();
            result.MyNullableMemoryInt.Should().NotBeNull();
            result.MyNullableMemoryInt.Value.Length.Should().Be(0);
            result.MyMemoryInt.Length.Should().Be(0);
        }

        [Fact]
        public void LazinatorDotNetHashSetOfSerializedItems()
        {
            DotNetHashSet_SelfSerialized GetObject(int thirdItemIndex)
            {
                return new DotNetHashSet_SelfSerialized()
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
            DotNetList_SelfSerialized GetObject(int thirdItemIndex)
            {
                return new DotNetList_SelfSerialized()
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
            // also make sure that no error is thrown if we verify cleanliness when change is to individual item in list, because the parent will know that it's dirty
            result2.MyListSerialized[2] = itemRemoved;
            itemRemoved.MyShort = -987;
            attemptToVerifyCleanlinessWithoutSettingDirtyFlag.Should().NotThrow();
        }

        [Fact]
        public void LazinatorDotNetDictionaryOfSerializedItems()
        {
            Dictionary_Values_SelfSerialized GetObject()
            {
                return new Dictionary_Values_SelfSerialized()
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
            Dictionary_Values_SelfSerialized GetObject()
            {
                return new Dictionary_Values_SelfSerialized()
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
            Dictionary_Values_SelfSerialized GetObject()
            {
                return new Dictionary_Values_SelfSerialized()
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
        public void LazinatorTuple()
        {
            LazinatorTuple<WInt, WString> item =
                new LazinatorTuple<WInt, WString>(5, "hello");
            var clone = item.CloneLazinatorTyped();
            clone.Item1.WrappedValue.Should().Be(5);
            clone.Item2.WrappedValue.Should().Be("hello");
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
        public void LazinatorTupleWithLazinatorStruct()
        {

            void ConfirmSerializeAndDeserialize()
            {
                RegularTuple GetObject()
                {
                    return new RegularTuple()
                    {
                        MyTupleSerialized4 = new Tuple<int, ExampleStruct>(2, new ExampleStruct() { MyChar = '1' })
                    };
                }

                var original = GetObject();
                var result = original.CloneLazinatorTyped();
                result.MyTupleSerialized4.Item1.Should().Be(original.MyTupleSerialized4.Item1);
                result.MyTupleSerialized4.Item2.MyChar.Should().Be(original.MyTupleSerialized4.Item2.MyChar);
            }

            ConfirmSerializeAndDeserialize();
        }

        [Fact]
        public void LazinatorDotNetListOfNonSerializedItems()
        {
            DotNetList_NonSelfSerializable GetNonObject(int thirdItemIndex)
            {
                return new DotNetList_NonSelfSerializable()
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
            DotNetList_Nested_NonSelfSerializable GetNestedList(int index)
            {
                return new DotNetList_Nested_NonSelfSerializable()
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
            Derived_DotNetList_Nested_NonSelfSerializable GetNestedList(int index)
            {
                return new Derived_DotNetList_Nested_NonSelfSerializable()
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
        public void LazinatorInheritanceWorks()
        {
            var original = GetHierarchy(1, 1, 3, 1, 2);
            var copy = GetHierarchy(1, 1, 3, 1, 2);
            var result = original.CloneLazinatorTyped();
            ExampleEqual(copy, result).Should().BeTrue();
        }

        [Fact]
        public void LazinatorListWithObjectsWorks()
        {
            var original = new LazinatorListContainer()
            {
                MyList = new LazinatorList<ExampleChild>()
                {
                    new ExampleChild() { MyShort = 22 },
                    new ExampleChildInherited() { MyShort = 21, MyInt = 23 }
                }
            };
            var clone = original.CloneLazinatorTyped(); // second clone
            var list = clone.MyList;
            list.Should().NotBeNull();
            list[0].MyShort.Should().Be(22);
            var innerDerived = list[1] as ExampleChildInherited;
            innerDerived.Should().NotBeNull();
            innerDerived.MyShort.Should().Be(21);
            innerDerived.MyInt.Should().Be(23);
        }

        [Fact]
        public void DerivedLazinatorListWithObjectsWorks()
        {
            var original = new LazinatorListContainer()
            {
                MyList = new DerivedLazinatorList<ExampleChild>()
                {
                    new ExampleChild() { MyShort = 22 },
                    new ExampleChildInherited() { MyShort = 21, MyInt = 23 }
                }
            };
            var clone = original.CloneLazinatorTyped(); // second clone
            var list = clone.MyList;
            list.Should().NotBeNull();
            list[0].MyShort.Should().Be(22);
            var innerDerived = list[1] as ExampleChildInherited;
            innerDerived.Should().NotBeNull();
            innerDerived.MyShort.Should().Be(21);
            innerDerived.MyInt.Should().Be(23);
        }

        [Fact]
        public void ConcreteClassesInheritingFromAbstractSerialize()
        {
            // serialize the concrete classes inheriting from the abstract ones
            Concrete5 c = new Concrete5()
            {
                String1 = "1",
                String2 = "2",
                String3 = "3",
                String4 = "4",
                String5 = "5"
            };
            var c2 = c.CloneLazinatorTyped();
            c2.String1.Should().Be("1");
            c2.String2.Should().Be("2");
            c2.String3.Should().Be("3");
            c2.String4.Should().Be("4");
            c2.String5.Should().Be("5");
        }

        [Fact]
        public void AbstractPropertySerializes()
        {
            ContainerWithAbstract1 c = new ContainerWithAbstract1()
            {
                AbstractProperty = new Concrete3() { String1 = "1", String2 = "2", String3 = "3"  }
            };
            var c2 = c.CloneLazinatorTyped();
            var c2_abstractProperty = (c2.AbstractProperty as Concrete3);
            c2_abstractProperty.String1.Should().Be("1");
            c2_abstractProperty.String2.Should().Be("2");
            c2_abstractProperty.String3.Should().Be("3");
        }

        [Fact]
        public void PropertyChangedInConstructorSerializes()
        {
            // Concrete3's constructor sets Example2 and Example3 to null. This means that their _Accessed fields will be true.
            // When we deserialize ContainerWithAbstract1, we also deserialize Concrete3. If we did not call ResetAccessedProperties,
            // then when we access Example2, it would appear that no deserialization is necessary, and Example2 will stay at its null value.
            // Both of the following examples fail without ResetAccessedProperties().

            var concrete = new Concrete3()
            {
                Example2 = GetHierarchy(1, 1, 1, 1, 0),
                Example3 = GetHierarchy(1, 1, 1, 1, 0)
            };
            var concrete2 = concrete.CloneLazinatorTyped();
            concrete2.Example2.Should().NotBeNull();
            concrete2.Example3.Should().NotBeNull();

            ContainerWithAbstract1 c = new ContainerWithAbstract1()
            {
                AbstractProperty = new Concrete3() { Example2 = GetHierarchy(1, 1, 1, 1, 0), Example3 = GetHierarchy(1, 1, 1, 1, 0) }
            };
            var c2 = c.CloneLazinatorTyped();
            var c2_abstractProperty = (c2.AbstractProperty as Concrete3);
            c2_abstractProperty.Example2.Should().NotBeNull();
            c2_abstractProperty.Example3.Should().NotBeNull();
        }

        [Fact]
        public void OpenGenericWithConcreteTypeSerializes()
        {
            OpenGenericStayingOpenContainer x = new OpenGenericStayingOpenContainer()
            {
                ClosedGenericFloat = new OpenGeneric<WFloat>()
                {
                    MyT = new WFloat(3.4F)
                }
            };
            var c = x.CloneLazinatorTyped();
            c.ClosedGenericFloat.MyT.WrappedValue.Should().Be(3.4F);
        }

        [Fact]
        public void OpenGenericWithInterfaceTypeSerializes()
        {
            OpenGenericStayingOpenContainer x = new OpenGenericStayingOpenContainer()
            {
                ClosedGenericInterface = new OpenGeneric<IExampleChild>()
                {
                    MyT = new ExampleChild() { MyShort = 45 }
                }
            };
            var c = x.CloneLazinatorTyped();
            ((ExampleChild)c.ClosedGenericInterface.MyT).MyShort.Should().Be(45);
        }

        [Fact]
        public void ConcreteGenericClassesSerialize()
        {
            ConcreteGeneric2a cg2a = new ConcreteGeneric2a()
            {
                AnotherProperty = "hi",
                MyT = 5, // now is an int
                LazinatorExample = GetExample(2),
            };
            var cg2a_clone = cg2a.CloneLazinatorTyped();
            cg2a_clone.AnotherProperty.Should().Be("hi");
            cg2a_clone.MyT.Should().Be(5);
            cg2a_clone.LazinatorExample.Should().NotBeNull();

            ConcreteGeneric2b cg2b = new ConcreteGeneric2b()
            {
                AnotherProperty = "hi",
                MyT = GetExample(2),
                LazinatorExample = GetExample(1),
            };
            var cg2b_clone = cg2b.CloneLazinatorTyped();
            cg2b_clone.AnotherProperty.Should().Be("hi");
            cg2b_clone.MyT.Should().NotBeNull();
            cg2b_clone.LazinatorExample.Should().NotBeNull();
        }

        [Fact]
        public void ConcreteGenericContainerWorks()
        {
            ConcreteGenericContainer c = new ConcreteGenericContainer()
            {
                Item = new ConcreteGeneric2a()
                {
                    AnotherProperty = "hi",
                    MyT = 5, // now is an int
                    LazinatorExample = GetExample(2),
                },
            };
            var c2 = c.CloneLazinatorTyped();
            ExampleEqual(((ConcreteGeneric2a) c.Item).LazinatorExample, ((ConcreteGeneric2a) c2.Item).LazinatorExample)
                .Should().BeTrue();
        }

        [Fact]
        public void ConcreteGenericContainerWithDerivedGeneric()
        {
            DerivedGenericContainer<WInt> c = new DerivedGenericContainer<WInt>()
            {
                Item = new DerivedGeneric2c<WInt>()
                {
                    MyT = 5 // now is a wrapped int -- note that Item is defined as being IAbstract<T>
                },
            };
            var c2 = c.CloneLazinatorTyped();
            var item = c2.Item;
            ((DerivedGeneric2c<WInt>)item).MyT.WrappedValue.Should().Be(5);
        }

        [Fact]
        public void GenericFromBase()
        {
            GenericFromBase<WInt> c = new GenericFromBase<WInt>()
            {
                MyT = 5
            };
            var c2 = c.CloneLazinatorTyped();
            var item = c2.MyT;
            item.WrappedValue.Should().Be(5);
        }

        [Fact]
        public void GenericFromBaseInContainer()
        {
            GenericFromBase<WInt> g = new GenericFromBase<WInt>()
            {
                MyT = 5
            };
            BaseContainer c = new BaseContainer()
            {
                MyBase = g
            };
            var c2 = c.CloneLazinatorTyped();
            var item = ((GenericFromBase<WInt>)c2.MyBase);
            item.MyT.WrappedValue.Should().Be(5);
        }

        [Fact]
        public void DirtinessSetsCorrectly()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy.IsDirty.Should().BeTrue();
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy.MyDateTime = DateTime.Now - TimeSpan.FromHours(1);
            hierarchy.IsDirty.Should().BeTrue();
            hierarchy.MyChild1.IsDirty.Should().BeFalse();
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy.MyChild1 = new ExampleChild() {MyLong = 232344};
            hierarchy.IsDirty.Should().BeTrue();
        }

        [Fact]
        public void OnDirtyCalledWhereImplemented()
        {
            // The Example.cs method includes an _OnDirtyCalled nonserialized flag that is set to true when its OnDirty() method (not generally required) is called.
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy._OnDirtyCalled.Should().BeTrue();
            hierarchy._OnDirtyCalled = false; // reset flag
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy.MyDateTime = DateTime.Now - TimeSpan.FromHours(1);
            hierarchy.IsDirty.Should().BeTrue();
            hierarchy._OnDirtyCalled.Should().BeTrue();
            hierarchy._OnDirtyCalled = false; // reset flag
            hierarchy.MyChild1.IsDirty.Should().BeFalse();
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy._OnDirtyCalled.Should().BeFalse();
            hierarchy.MyChild1 = new ExampleChild() { MyLong = 232344 };
            hierarchy.IsDirty.Should().BeTrue();
            hierarchy._OnDirtyCalled.Should().BeTrue();
        }

        [Fact]
        public void DescendantDirtinessSetsCorrectly()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy.IsDirty.Should().BeTrue();
            hierarchy.MyChild1.MyShort = 5234; 
            hierarchy.DescendantIsDirty.Should().BeFalse(); // not affected by change to new child when the child was already marked as dirty
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy.MyChild1.MyShort.Should().Be(5234);
            hierarchy.MyChild1.IsDirty.Should().BeFalse();
            hierarchy.MyChild1.MyLong = 987654;
            hierarchy.MyChild1.IsDirty.Should().BeTrue();
            hierarchy.IsDirty.Should().BeFalse();
            hierarchy.DescendantIsDirty.Should().BeTrue();
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.MyChild1.MyLong.Should().Be(987654);
        }


        [Fact]
        public void DistantPropertiesSerialized()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy.MyChild1.MyWrapperContainer = new WrapperContainer() { WrappedInt = 17 };
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.MyChild1.MyWrapperContainer.WrappedInt.Should().Be(17);
            hierarchy.MyChild1.MyWrapperContainer.WrappedInt = 19;
            hierarchy = hierarchy.CloneLazinatorTyped();
            hierarchy.MyChild1.MyWrapperContainer.WrappedInt.Should().Be(19);
        }

        [Fact]
        public void DirtinessWorksAfterConvertToBytes()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy.MyChild1.MyWrapperContainer = new WrapperContainer() { WrappedInt = 17 };
            hierarchy.MyChild1.IsDirty.Should().BeTrue();
            hierarchy.MyChild1.DescendantIsDirty.Should().BeFalse(); // false because MyWrapperContainer is dirty and thus doesn't notify new parent
            hierarchy.MyChild1.MyWrapperContainer.LazinatorConvertToBytes();
            hierarchy.MyChild1.IsDirty.Should().BeTrue();
            hierarchy.MyChild1.DescendantIsDirty.Should().BeFalse();
            hierarchy.MyChild1.MyWrapperContainer.IsDirty.Should().BeFalse();
            hierarchy.MyChild1.MyWrapperContainer.WrappedInt = 18;
            hierarchy.MyChild1.MyWrapperContainer.IsDirty.Should().BeTrue(); 
            hierarchy.MyChild1.IsDirty.Should().BeTrue(); // hasn't changed
            hierarchy.MyChild1.DescendantIsDirty.Should().BeTrue(); // this time, wrapper's dirtiness change, so MyChild1 is notified
            var clone = hierarchy.CloneLazinatorTyped();
            // The following is the tricky part. We must make sure that LazinatorConvertToBytes doesn't cause MyChild1 to think that no serialization is necessary.
            clone.MyChild1.MyWrapperContainer.WrappedInt.Should().Be(18);
        }

        [Fact]
        public void DirtinessWorksAfterDotNetCollectionConverted()
        {
            DotNetHashSet_SelfSerialized l = new DotNetHashSet_SelfSerialized();
            l.MyHashSetSerialized = new HashSet<ExampleChild>();
            l.MyHashSetSerialized.Add(new ExampleChild());
            l.LazinatorConvertToBytes();
            l.IsDirty.Should().BeFalse();
            l.DescendantIsDirty.Should().BeFalse();
            var firstItem = l.MyHashSetSerialized.First();
            l.IsDirty.Should().BeTrue(); // should be true because .Net collection without special _Dirty property has been accessed
            firstItem.MyLong = 54321;
            var c = l.CloneLazinatorTyped();
            c.MyHashSetSerialized.First().MyLong.Should().Be(54321);
        }

        [Fact]
        public void PropertiesWorkAfterConvertToBytes()
        {
            // Suppose we have two properties, property A and property B. After deserialization, the byte points are set. Suppose that property A is accessed, but property B is not. If we then convert to bytes but continue to use the old object, then the old byte points will be out of date. Property A will be good; since it has been accessed, it still has the object. But unless we update the byte points, property B will now be pointing to a spot on the old set of bytes.

            LazinatorTriple<WString, WString, WString> x = new LazinatorTriple<WString, WString, WString>("one", "andtwo", "andthree");
            var c3 = x.CloneLazinatorTyped(); // c3 has LazinatorObjectBytes set
            c3.Item1 = "another";
            var c4 = c3.CloneLazinatorTyped(); // LazinatorObjectBytes are now updated in C3
            c3.Item1.Should().Be("another");
            c3.Item2.Should().Be("andtwo"); // this will cause a problem if the byte indides are not set correctly
        }

        [Fact]
        public void SelfSerializationRecognizesUpdates()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            var original = GetHierarchy(0, 1, 2, 0, 0);
            var withChildrenReversed = GetHierarchy(0, 2, 1, 0, 0);

            foreach (bool explicitlyCallConvertFromBytes in new bool[] { true, false })
                foreach (bool verifyCleanness in new bool[] { true, false })
                    hierarchy = ReverseChildrenAndBack(hierarchy, original, withChildrenReversed, explicitlyCallConvertFromBytes, verifyCleanness);
        }

        private Example ReverseChildrenAndBack(Example hierarchy, Example original, Example withChildrenReversed, bool explicitlyCallConvertFromBytes, bool verifyCleanness)
        {
            ReverseChildrenMembers(hierarchy);
            ExampleEqual(hierarchy, withChildrenReversed).Should().Be(true);
            hierarchy = CloneWithOptionalVerification(hierarchy, true, verifyCleanness);
            ExampleEqual(hierarchy, withChildrenReversed).Should().Be(true);
            ReverseChildrenMembers(hierarchy);
            hierarchy = CloneWithOptionalVerification(hierarchy, true, verifyCleanness);
            ExampleEqual(hierarchy, original).Should().Be(true);
            return hierarchy;
        }

        private void ReverseChildrenMembers(Example original)
        {
            long myLong1 = original.MyChild1.MyLong;
            short myShort1 = original.MyChild1.MyShort;
            long myLong2 = original.MyChild2.MyLong;
            short myShort2 = original.MyChild2.MyShort;
            original.MyChild1.MyLong = myLong2;
            original.MyChild1.MyShort = myShort2;
            original.MyChild2.MyLong = myLong1;
            original.MyChild2.MyShort = myShort1;
        }

        [Fact]
        public void SelfSerializationUpdatesNonSerializedTypeProperly()
        {
            var hierarchy = GetHierarchy(1, 1, 1, 1, 0);
            var copy = GetHierarchy(1, 1, 1, 1, 0);
            var goal = GetHierarchy(1, 1, 1, 2, 0);
            ChangeHierarchyToGoal(copy, goal, serializeAndDeserializeFirst: true, setDirtyFlag: true, verifyCleanliness: true);
            ChangeHierarchyToGoal(copy, goal, serializeAndDeserializeFirst: true, setDirtyFlag: true, verifyCleanliness: false);
            ChangeHierarchyToGoal(copy, goal, serializeAndDeserializeFirst: true, setDirtyFlag: false, verifyCleanliness: true);
            ChangeHierarchyToGoal(copy, goal, serializeAndDeserializeFirst: true, setDirtyFlag: false, verifyCleanliness: false);
            ChangeHierarchyToGoal(copy, goal, serializeAndDeserializeFirst: false, setDirtyFlag: true, verifyCleanliness: true);
            ChangeHierarchyToGoal(copy, goal, serializeAndDeserializeFirst: false, setDirtyFlag: true, verifyCleanliness: false);
            ChangeHierarchyToGoal(copy, goal, serializeAndDeserializeFirst: false, setDirtyFlag: false, verifyCleanliness: true);
            ChangeHierarchyToGoal(copy, goal, serializeAndDeserializeFirst: false, setDirtyFlag: false, verifyCleanliness: false);
        }

        [Fact]
        public void FastReadListIntWorks()
        {
            LazinatorFastReadList<int> r = new LazinatorFastReadList<int>();
            r.AsList = new List<int>() { 3, 4, 5 };
            r.IsDirty.Should().BeTrue();
            LazinatorFastReadList<int> r2 = r.CloneLazinatorTyped();
            r2[0].Should().Be(3);
            r2.IsDirty.Should().BeFalse();
            r2.AsList.Add(6);
            r2.IsDirty.Should().BeTrue();
            LazinatorFastReadList<int> r3 = r2.CloneLazinatorTyped();
            r3.AsList.Count().Should().Be(4);
        }
        

        [Fact]
        public void OffsetListWorks()
        {
            LazinatorOffsetList x = new LazinatorOffsetList();
            List<int> list = new List<int>() {3, 4, 1600, 234234234, 234234345};
            void CheckList()
            {
                for (int i = 0; i < list.Count; i++)
                    x[i].Should().Be(list[i]);
            }
            void CheckBeforeAndAfterSerialization()
            {
                CheckList();
                x = x.CloneLazinatorTyped();
                CheckList();
                x = x.CloneLazinatorTyped();
                CheckList();
            }
            // set the list
            foreach (int i in list)
                x.AddOffset(i);
            CheckBeforeAndAfterSerialization();
            // set the list a diferent way
            x.SetOffsets(list);
            CheckBeforeAndAfterSerialization();
            // change the list
            list.Add(234234346);
            x.AddOffset(234234346);
            CheckBeforeAndAfterSerialization();
            // change the list again
            x.SetOffsets(list);
            CheckBeforeAndAfterSerialization();
            // change the list again, with only large numbers
            list = new List<int>() { 234234234, 234234345, 999999999 };
            x.SetOffsets(list);
            CheckBeforeAndAfterSerialization();
            // change the list again, with only a small number
            list = new List<int>() { 0 };
            x.SetOffsets(list);
            CheckBeforeAndAfterSerialization();
            // change the list again, with a few small numbers
            list = new List<int>() { 2, 280, 9000 };
            x.SetOffsets(list);
            CheckBeforeAndAfterSerialization();
            // now try an empty list
            x = new LazinatorOffsetList();
            list = new List<int>();
            CheckBeforeAndAfterSerialization();
        }

        public enum ContainerForLazinatorList
        {
            GenericContainer,
            NonGenericContainer,
            NoContainer
        }

        [InlineData(ContainerForLazinatorList.GenericContainer)]
        [InlineData(ContainerForLazinatorList.NonGenericContainer)]
        [InlineData(ContainerForLazinatorList.NoContainer)]
        [Theory]
        public void LazinatorListWorks(ContainerForLazinatorList containerOption)
        {
            LazinatorListContainer nonGenericContainer = new LazinatorListContainer()
            {
            };
            LazinatorListContainerGeneric<ExampleChild> genericContainer = new LazinatorListContainerGeneric<ExampleChild>()
            {
            };
            LazinatorList<ExampleChild> withoutContainer = null;
            LazinatorList<ExampleChild> GetList()
            {
                switch (containerOption)
                {
                    case ContainerForLazinatorList.NoContainer:
                        return withoutContainer;
                    case ContainerForLazinatorList.NonGenericContainer:
                        return nonGenericContainer.MyList;
                    case ContainerForLazinatorList.GenericContainer:
                        return genericContainer.MyList;
                }
                throw new NotImplementedException();
            }
            void SetList(LazinatorList<ExampleChild> value)
            {
                switch (containerOption)
                {
                    case ContainerForLazinatorList.NoContainer:
                        withoutContainer = value;
                        break;
                    case ContainerForLazinatorList.NonGenericContainer:
                        nonGenericContainer.MyList = value;
                        break;
                    case ContainerForLazinatorList.GenericContainer:
                        genericContainer.MyList = value;
                        break;
                }
            }

            SetList(new LazinatorList<ExampleChild>()
            {
            });

            LazinatorList<ExampleChild> list = new LazinatorList<ExampleChild>();
            for (int i = 0; i <= 3; i++)
            {
                AddItem(i);
                list.IsDirty.Should().BeTrue();

            }
            void AddItem(int i, int? insertIndex = null)
            {
                if (insertIndex is int insertIndexInt)
                {
                    GetList().Insert(insertIndexInt, GetExampleChild(i));
                    list.Insert(insertIndexInt, GetExampleChild(i));
                }
                else
                {
                    GetList().Add(GetExampleChild(i));
                    list.Add(GetExampleChild(i));
                }
            }
            void RemoveItem(int i, bool useRemoveAt)
            {
                if (useRemoveAt)
                {
                    GetList().RemoveAt(i);
                    list.RemoveAt(i);
                }
                else
                {
                    var item = GetList()[i];
                    GetList().Remove(item);
                    list.RemoveAt(i); // not testing this, so can just do this
                }
            }
            void Clear()
            {
                GetList().Clear();
                list.Clear();
                list.IsDirty.Should().BeTrue();
            }
            void CheckList()
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (GetList()[i] == null)
                        list[i].Should().Be(null);
                    else
                        ExampleChildEqual(GetList()[i], list[i]).Should().BeTrue();
                }
                // now check another way, using enumerables
                var zipped = GetList().Zip(list, (a, b) => (a, b));
                foreach (var zip in zipped)
                    ExampleChildEqual(zip.a, zip.b).Should().BeTrue();
            }
            void CheckBeforeAndAfterSerialization()
            {
                CheckList();

                switch (containerOption)
                {
                    case ContainerForLazinatorList.NoContainer:
                        SetList(GetList().CloneLazinatorTyped());
                        break;
                    case ContainerForLazinatorList.NonGenericContainer:
                        nonGenericContainer = nonGenericContainer.CloneLazinatorTyped();
                        break;
                    case ContainerForLazinatorList.GenericContainer:
                        genericContainer = genericContainer.CloneLazinatorTyped();
                        break;
                }

                CheckList();
            }

            CheckBeforeAndAfterSerialization();
            CheckBeforeAndAfterSerialization(); // do it again
            AddItem(1);
            CheckBeforeAndAfterSerialization();
            AddItem(0);
            CheckBeforeAndAfterSerialization();
            AddItem(2, 1);
            CheckBeforeAndAfterSerialization();
            AddItem(0, 0);
            CheckBeforeAndAfterSerialization();
            RemoveItem(3, true);
            CheckBeforeAndAfterSerialization();
            RemoveItem(3, false);
            CheckBeforeAndAfterSerialization();
            RemoveItem(0, false);
            CheckBeforeAndAfterSerialization();
            RemoveItem(0, true);
            CheckBeforeAndAfterSerialization();
            Clear();
            CheckBeforeAndAfterSerialization();
        }

        [Fact]
        public void LazinatorListEmptyWorks()
        {
            LazinatorListContainer nonGenericContainer = new LazinatorListContainer()
            {
            };
            nonGenericContainer.MyList = new LazinatorList<ExampleChild>();
            var clone = nonGenericContainer.CloneLazinatorTyped();
            var listInClone = clone.MyList;
            listInClone.Should().NotBeNull();

            // and again
            clone = nonGenericContainer.CloneLazinatorTyped();
            listInClone = clone.MyList;
            listInClone.Should().NotBeNull();
        }

        [Fact]
        public void LazinatorListNullWorks()
        {
            LazinatorListContainer nonGenericContainer = new LazinatorListContainer()
            {
            };
            nonGenericContainer.MyList = null;
            var clone = nonGenericContainer.CloneLazinatorTyped();
            var listInClone = clone.MyList;
            listInClone.Should().BeNull();

            // and again
            clone = nonGenericContainer.CloneLazinatorTyped();
            listInClone = clone.MyList;
            listInClone.Should().BeNull();
        }

        [Fact]
        public void ShortLazinatorListWorks()
        {
            LazinatorList<WInt> l = new LazinatorList<WInt>();
            l.Add(3);
            var c = l.CloneLazinatorTyped();
            var result = c[0];
            result.Should().Be(3);
        }

        [Fact]
        public void LazinatorListDirtinessWorks()
        {
            LazinatorListContainer nonGenericContainer = new LazinatorListContainer()
            {
            };
            nonGenericContainer.MyList = new LazinatorList<ExampleChild>();
            nonGenericContainer.MyList.IsDirty.Should().BeTrue();
            nonGenericContainer.IsDirty.Should().BeTrue();
            nonGenericContainer.DescendantIsDirty.Should().BeTrue(); 

            var v2 = nonGenericContainer.CloneLazinatorTyped();
            v2.IsDirty.Should().BeFalse();
            v2.DescendantIsDirty.Should().BeFalse();
            v2.MyList.IsDirty.Should().BeFalse();
            v2.MyList.Add(GetExampleChild(1));
            v2.MyList.IsDirty.Should().BeTrue();
            v2.IsDirty.Should().BeFalse();
            v2.DescendantIsDirty.Should().BeTrue();

            var v3 = v2.CloneLazinatorTyped();
            v3.MyList.IsDirty.Should().BeFalse();
            v3.MyList.DescendantIsDirty.Should().BeFalse();
            v3.MyList[0].MyLong = 987654321;
            v3.MyList.IsDirty.Should().BeFalse();
            v3.MyList.DescendantIsDirty.Should().BeTrue();
            var v4 = v3.CloneLazinatorTyped();
            v4.MyList[0].MyLong.Should().Be(987654321);

            // now, back to prior list
            v2.MyList.Add(GetExampleChild(1));
            v2.MyList.Add(GetExampleChild(1));
            v2.MyList.Add(GetExampleChild(1));
            var v5 = v2.CloneLazinatorTyped();
            v5.IsDirty.Should().BeFalse();
            v5.DescendantIsDirty.Should().BeFalse();
            v5.MyList.IsDirty.Should().BeFalse();
            var x = v5.MyList[2];
            v5.MyList.IsDirty.Should().BeFalse();
            v5.MyList.DescendantIsDirty.Should().BeFalse();
            x.MyLong = 25;
            v5.MyList.IsDirty.Should().BeFalse();
            v5.MyList.DescendantIsDirty.Should().BeTrue();
            v5.IsDirty.Should().BeFalse();
            v5.DescendantIsDirty.Should().BeTrue();
        }

        private void ChangeHierarchyToGoal(Example copy, Example goal, bool serializeAndDeserializeFirst, bool setDirtyFlag, bool verifyCleanliness)
        {
            var hierarchy = GetHierarchy(1, 1, 1, 1, 0);
            if (serializeAndDeserializeFirst)
            {
                hierarchy = hierarchy.CloneLazinatorTyped();
                ExampleEqual(hierarchy, copy).Should().BeTrue();
            }
            hierarchy.MyNonLazinatorChild.MyString = goal.MyNonLazinatorChild.MyString;
            hierarchy.MyNonLazinatorChild.MyInt = goal.MyNonLazinatorChild.MyInt;
            if (setDirtyFlag)
                hierarchy.MyNonLazinatorChild_Dirty = true;
            bool shouldThrow = serializeAndDeserializeFirst && !setDirtyFlag && verifyCleanliness; // note: If it has not been deserialized first, then it will definitely have to serialize, and so there is no comparison to make
            if (shouldThrow)
            {
                Action act = () => CloneWithOptionalVerification(hierarchy, true, verifyCleanliness);
                act.Should().Throw<UnexpectedDirtinessException>();
            }
            else
            {
                hierarchy = CloneWithOptionalVerification(hierarchy, true, verifyCleanliness);
                bool shouldBeEqual = !serializeAndDeserializeFirst || setDirtyFlag; // if we don't set dirty flag, then either we get an exception or we get the wrong data, unless the data has never been serialized and deserialized
                ExampleEqual(hierarchy, goal).Should().Be(shouldBeEqual);
            }
        }

        [Fact]
        public void BinaryHashCodesWork()
        {
            var example = GetHierarchy(1, 1, 1, 1, 0);
            var hash32 = example.GetBinaryHashCode32();
            var hash64 = example.GetBinaryHashCode64();
            var clone = example.CloneLazinatorTyped();
            clone.GetBinaryHashCode32().Should().Be(hash32);
            clone.GetBinaryHashCode64().Should().Be(hash64);
            var anotherExample = GetHierarchy(1, 1, 1, 1, 0);
            anotherExample.GetBinaryHashCode64().Should().Be(hash64);

            example.MyBool = !example.MyBool;
            var hash32b = example.GetBinaryHashCode32();
            var hash64b = example.GetBinaryHashCode64();
            hash32b.Should().NotBe(hash32);
            hash64b.Should().NotBe(hash64);
            var clone2 = example.CloneLazinatorTyped();
            clone2.GetBinaryHashCode32().Should().Be(hash32b);
            clone2.GetBinaryHashCode64().Should().Be(hash64b);

            example.MyChild1.MyShort = (short) 999;
            var hash32c = example.GetBinaryHashCode32();
            var hash64c = example.GetBinaryHashCode64();
            hash32c.Should().NotBe(hash32);
            hash64c.Should().NotBe(hash64);
            hash32c.Should().NotBe(hash32b);
            hash64c.Should().NotBe(hash64b);
            var clone3 = example.CloneLazinatorTyped();
            clone3.GetBinaryHashCode32().Should().Be(hash32c);
            clone3.GetBinaryHashCode64().Should().Be(hash64c);
        }

        [Fact]
        void BinaryHashInList()
        {
            var wrapped = new WInt(1);
            var wrapped2 = new WInt(1);
            LazinatorList<WInt> x = new LazinatorList<WInt>();
            x.Add(wrapped2);
            x.GetListMemberHash32(0).Should().Be(wrapped.GetBinaryHashCode32());
            var clone = x.CloneLazinatorTyped();
            clone.GetListMemberHash32(0).Should().Be(wrapped.GetBinaryHashCode32());
        }

        [Fact]
        void BinaryHashCanBeAssignedToPropertyOfItem()
        {
            // the challenge here is that the call to GetBinaryHashCode results in a ConvertToBytes. Meanwhile, the object may have been accessed already on the left side of the assignment. We want to make sure this doesn't cause any problems.
            Example e = GetHierarchy(1, 1, 1, 1, 0);
            e.MyChild1.MyLong = (long) e.MyChild1.GetBinaryHashCode64();
            e.MyChild1.Should().NotBeNull();
            e.MyChild1.MyLong.Should().NotBe(0);
            var c = e.CloneLazinatorTyped();
            c.MyChild1.MyLong = 0;
            c.MyChild1.MyLong = (long)e.MyChild1.GetBinaryHashCode64();
            c.MyChild1.Should().NotBeNull();
            c.MyChild1.MyLong.Should().NotBe(0);
            Example e2 = GetHierarchy(1, 1, 1, 1, 0);
            e2.MyChild1 = new ExampleChild();
            e2.MyChild1.MyLong = 0;
            e2.MyChild1.MyLong = (long)e.MyChild1.GetBinaryHashCode64();
            e2.MyChild1.Should().NotBeNull();
            e2.MyChild1.MyLong.Should().NotBe(0);
        }

        [Fact]
        public void SubclassesWork()
        {
            ClassWithSubclass outer = new ClassWithSubclass()
            {
                IntWithinSuperclass = 5,
                SubclassInstance1 = new ClassWithSubclass.SubclassWithinClass()
                {
                    StringWithinSubclass = "within1"
                },
                SubclassInstance2 = new ClassWithSubclass.SubclassWithinClass()
                {
                    StringWithinSubclass = "within2"
                }
            };
            var clone = outer.CloneLazinatorTyped();
            clone.IntWithinSuperclass.Should().Be(5);
            clone.SubclassInstance1.StringWithinSubclass.Should().Be("within1");
            clone.SubclassInstance2.StringWithinSubclass.Should().Be("within2");
        }

        [Fact]
        public void DeserializeMultipleWorks()
        {
            Example e = GetHierarchy(1, 1, 1, 1, 0);
            OpenGeneric<WFloat> o = new OpenGeneric<WFloat>()
            {
                MyT = 3.0F,
                MyListT = new List<WFloat>()
                    {
                        1.0F,
                        2.0F
                    }
            };
            int lengthExample = e.CloneLazinator().LazinatorObjectBytes.Length;
            int lengthOpenGeneric = o.CloneLazinator().LazinatorObjectBytes.Length;
            int lengthSoFar = 0;
            const int numPairs = 5;
            Memory<byte> memory = new Memory<byte>(new byte[numPairs * (lengthExample + lengthOpenGeneric)]);
            for (int i = 0; i < numPairs; i++)
            {
                e.LazinatorObjectBytes.CopyTo(memory.Slice(lengthSoFar));
                lengthSoFar += lengthExample;
                o.LazinatorObjectBytes.CopyTo(memory.Slice(lengthSoFar));
                lengthSoFar += lengthOpenGeneric;
            }
            var results = DeserializationFactory.Instance
                .CreateMultiple(memory, null)
                .Select(x => (ILazinator)x)
                .Where(x => x != null).ToList();
            results.Count().Should().Be(10);
            for (int i = 0; i < numPairs; i++)
            {
                (results[2 * i] as Example).Should().NotBeNull();
                (results[2 * i + 1] as OpenGeneric<WFloat>).Should().NotBeNull();
                (results[2 * i + 1] as OpenGeneric<WInt>).Should().BeNull();
            }
        }

        [Fact]
        public void AutocloneWorks()
        {
            var original = GetHierarchy(1, 1, 1, 1, 0);
            var another = GetHierarchy(1, 1, 1, 1, 0);
            original.MyAutocloneChild = new ExampleChild();
            original.MyAutocloneChild.LazinatorParentClass.Should().Be(original);
            another.MyAutocloneChild = original.MyAutocloneChild;
            original.MyAutocloneChild.LazinatorParentClass.Should().Be(original);
            another.MyAutocloneChild.LazinatorParentClass.Should().Be(another);

            original.MyAutocloneChildStruct = new ExampleStruct();
            original.MyAutocloneChildStruct.LazinatorParentClass.Should().Be(original);
            another.MyAutocloneChildStruct = original.MyAutocloneChildStruct;
            original.MyAutocloneChildStruct.LazinatorParentClass.Should().Be(original);
            another.MyAutocloneChildStruct.LazinatorParentClass.Should().Be(another);

            original.MyAutocloneChild = null;
            original.MyAutocloneChild.Should().BeNull();
        }

        [Fact]
        public void ConfirmSerialization()
        {
            for (int i = 0; i <= 2; i++)
                for (int i2 = 0; i2 <= 3; i2++)
                    for (int i3 = 0; i3 <= 3; i3++)
                        for (int i4 = 0; i4 <= 3; i4++)
                            for (int i5 = 0; i5 <= 2; i5++)
                            {
                                ConfirmHierarchySerialization(i, i2, i3, i4, i5);
                            }
        }

        private void ConfirmHierarchySerialization(int indexUpTo2, int indexUpTo3a, int indexUpTo3b,
            int indexUpTo3c, int indexUpTo2b)
        {
            var original = GetHierarchy(indexUpTo2, indexUpTo3a, indexUpTo3b, indexUpTo3c, indexUpTo2b);
            var copy = GetHierarchy(indexUpTo2, indexUpTo3a, indexUpTo3b, indexUpTo3c, indexUpTo2b);
            var anotherCopyForReference = GetHierarchy(indexUpTo2, indexUpTo3a, indexUpTo3b, indexUpTo3c, indexUpTo2b);
            // do an initial serialization / deserialization cycle
            var result = original.CloneLazinatorTyped();
            ExampleEqual(copy, result).Should().BeTrue();
            result.MyChild1?.LazinatorParentClass.Should().Be(result);
            result.MyChild2?.LazinatorParentClass.Should().Be(result);
            // repeat the cycle
            var result2 = result.CloneLazinatorTyped();
            ExampleEqual(copy, result2).Should().BeTrue();
            // and now again, verifying cleanness
            var result3 = CloneWithOptionalVerification(result2, true, true);
            ExampleEqual(copy, result3).Should().BeTrue();
            ExampleEqual(original, result3).Should().BeTrue();
        }

        private T CloneWithOptionalVerification<T>(T original, bool includeChildren, bool verifyCleanness) where T : ILazinator, new()
        {
            var bytes = original.SerializeNewBuffer(includeChildren ? IncludeChildrenMode.IncludeAllChildren : IncludeChildrenMode.ExcludeAllChildren, verifyCleanness);
            var result = new T
            {
                HierarchyBytes = bytes,
            };

            return result;
        }


        public Example GetHierarchy(int indexUpTo2, int indexUpTo3a, int indexUpTo3b, int indexUpTo3c, int indexUpTo2b)
        {
            var parent = GetExample(indexUpTo2);
            parent.MyChild1 = GetExampleChild(indexUpTo3a);
            parent.MyChild2 = GetExampleChild(indexUpTo3b);
            parent.MyNonLazinatorChild = GetNonLazinatorType(indexUpTo3c);
            parent.MyInterfaceImplementer = GetExampleInterfaceImplementer(indexUpTo2b);
            return parent;
        }

        public Example GetExample(int index)
        {
            if (index == 0)
                return new Example()
                {
                    MyBool = true,
                    MyChar = 'b',
                    MyString = "hello, world",
                    MyUint = (uint) 2342343242,
                    MyNullableDouble = (double)3.5,
                    MyNullableDecimal = (decimal?)-2341.5212352,
                    MyNullableTimeSpan = TimeSpan.FromHours(3),
                    MyDateTime = new DateTime(2000, 1, 1),
                    MyTestEnum = TestEnum.MyTestValue2,
                    MyTestEnumByteNullable = null,
                    WrappedInt = 5

                };
            else if (index == 1)
                return new Example()
                {
                    MyBool = false,
                    MyChar = '\u2342',
                    MyString = "",
                    MyUint = (uint) 1235,
                    MyNullableDouble = (double?)4.2,
                    MyNullableDecimal = null,
                    MyNullableTimeSpan = TimeSpan.FromDays(4),
                    MyDateTime = new DateTime(1972, 10, 22, 17, 36, 0),
                    MyTestEnum = TestEnum.MyTestValue3,
                    MyTestEnumByteNullable = TestEnumByte.MyTestValue,
                    WrappedInt = 2
                };
            else if (index == 2)
                return new Example()
                {
                    MyBool = true,
                    MyChar = '\n',
                    MyString = null,
                    MyUint = (uint) 3127,
                    MyNullableDouble = null,
                    MyNullableDecimal = (decimal?)234243252341,
                    MyTestEnum = TestEnum.MyTestValue3,
                    MyTestEnumByteNullable = TestEnumByte.MyTestValue3
                };
            throw new NotSupportedException();
        }

        public ExampleChild GetExampleChild(int index)
        {
            if (index == 0)
                return null;
            else if (index == 1)
                return new ExampleChild() {MyLong = 123123, MyShort = 543};
            else if (index == 2)
                return new ExampleChild() {MyLong = 999888, MyShort = -23};
            else if (index == 3)
                return new ExampleChildInherited() {MyLong = 234123, MyInt = 5432, MyShort = 2341}; // the subtype
            throw new NotImplementedException();
        }

        public IExampleNonexclusiveInterface GetExampleInterfaceImplementer(int index)
        {
            if (index == 0)
                return null;
            else if (index == 1)
                return new ExampleChildInherited() { MyLong = 234123, MyInt = 5432, MyShort = 2341 }; // the subtype
            else if (index == 2)
                return new ExampleNonexclusiveInterfaceImplementer() { MyInt = 27 };
            throw new NotImplementedException();
        }

        public NonLazinatorClass GetNonLazinatorType(int index)
        {
            if (index == 0)
                return null;
            else if (index == 1)
                return new NonLazinatorClass() {MyInt = 31, MyString = "ok now"};
            else if (index == 2)
                return new NonLazinatorClass() { MyInt = 345, MyString = "" };
            else if (index == 3)
                return new NonLazinatorClass() {MyInt = 876, MyString = "dokey\r\n"};
            throw new NotImplementedException();
        }


        public bool ExampleEqual(Example example1, Example example2)
        {
            if (example1 == null && example2 == null)
                return true;
            if ((example1 == null) != (example2 == null))
                return false;
            return example1.MyBool == example2.MyBool && example1.MyString == example2.MyString && example1.MyNewString == example2.MyNewString && example1.MyOldString == example2.MyOldString && example1.MyUint == example2.MyUint && example1.MyNullableDouble == example2.MyNullableDouble && example1.MyNullableDecimal == example2.MyNullableDecimal && example1.MyDateTime == example2.MyDateTime && example1.MyNullableTimeSpan == example2.MyNullableTimeSpan && ExampleChildEqual(example1.MyChild1, example2.MyChild1) && ExampleChildEqual(example1.MyChild2, example2.MyChild2) && InterfaceImplementerEqual(example1.MyInterfaceImplementer, example2.MyInterfaceImplementer) && NonLazinatorTypeEqual(example1.MyNonLazinatorChild, example2.MyNonLazinatorChild);
        }

        public bool ExampleChildEqual(ExampleChild child1, ExampleChild child2)
        {
            if (child1 == null && child2 == null)
                return true;
            if ((child1 == null) != (child2 == null))
                return false;
            if ((child1 is ExampleChildInherited) != (child2 is ExampleChildInherited))
                return false;
            bool basicFieldsEqual = (child1.MyLong == child2.MyLong && child1.MyShort == child2.MyShort && child1.ByteSpan.ToArray().SequenceEqual(child2.ByteSpan.ToArray()));
            bool inheritedFieldsEqual = !(child1 is ExampleChildInherited child1Inherited) || child1Inherited.MyInt == ((ExampleChildInherited) child2).MyInt;
            return basicFieldsEqual && inheritedFieldsEqual;
        }

        public bool InterfaceImplementerEqual(IExampleNonexclusiveInterface child1, IExampleNonexclusiveInterface child2)
        {
            if (child1 == null && child2 == null)
                return true;
            if ((child1 == null) != (child2 == null))
                return false;
            if ((child1 is ExampleChildInherited) != (child2 is ExampleChildInherited))
                return false;
            bool sharedFieldEqual = (child1.MyInt == child2.MyInt);
            bool otherFieldsEqual = !(child1 is ExampleChildInherited child1Inherited) || (child1Inherited.MyLong == ((ExampleChildInherited)child2).MyLong && child1Inherited.MyShort == ((ExampleChildInherited)child2).MyShort);
            return sharedFieldEqual && otherFieldsEqual;
        }

        public bool NonLazinatorTypeEqual(NonLazinatorClass instance1, NonLazinatorClass instance2)
        {
            if (instance1 == null && instance2 == null)
                return true;
            if ((instance1 == null) != (instance2 == null))
                return false;
            return instance1.MyString == instance2.MyString && instance1.MyInt == instance2.MyInt;
        }
    }
}
