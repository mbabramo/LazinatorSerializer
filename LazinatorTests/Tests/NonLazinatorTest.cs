using FluentAssertions;
using LazinatorTests.Examples;
using Lazinator.Core;
using Xunit;
using System.Reflection;
using LazinatorTests.Examples.NonLazinator;

namespace LazinatorTests.Tests
{
    public class NonLazinatorTest : SerializationDeserializationTestBase
    {

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
            var c2 = c.CloneLazinatorTyped();
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
        public void LazinatorSerializationUpdatesNonSerializedTypeProperly()
        {
            var hierarchy = GetTypicalExample();
            var copy = GetTypicalExample();
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

    }
}
