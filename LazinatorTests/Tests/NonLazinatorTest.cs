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

    }
}
