using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Exceptions;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using System.Buffers;
using LazinatorTests.Examples.Structs;

namespace LazinatorTests.Tests
{
    public class StructTests : SerializationDeserializationTestBase
    {

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
        public void LazinatorReadOnlySpanInStruct()
        {
            // we have special code to deal with ReadOnlySpans within structs. So, here is a test to make sure that it works.
            WReadOnlySpanChar w = new WReadOnlySpanChar();
            w.Value = new Span<char>("mystring".ToArray());
            var result = CloneWithOptionalVerification(w, false, false);
            new string(result.Value).Should().Be("mystring");
        }

    }
}
