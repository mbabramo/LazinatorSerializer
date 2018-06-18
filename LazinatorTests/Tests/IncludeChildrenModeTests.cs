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
    public class IncludeChildrenModeTests : SerializationDeserializationTestBase
    {
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

    }
}
