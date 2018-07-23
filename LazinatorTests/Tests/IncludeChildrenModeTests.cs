using FluentAssertions;
using Lazinator.Core;
using LazinatorTests.Examples.Hierarchy;
using Xunit;

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
        public void IncludeChildrenModeWorks()
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
        public void IncludeChildrenModeWorks_UnaccessedProperties()
        {
            // same as previous test, except that put everything in a container, and we make sure that the Example object 
            // itself is unaccessed at the beginning of the clone. Thus, the question is whether we'll exclude children
            // appropriately when those children were originally contained within an unaccessed property. We'll repeatedly
            // clone the container (to make sure all properties are unaccessed) and then clone again into a different
            // variable (to test the effect of IncludeChildrenMode).

            ExampleInterfaceContainer container = new ExampleInterfaceContainer();
            container.ExampleByInterface = GetHierarchy(1, 1, 1, 1, 1);
            container.ExampleByInterface.IncludableChild = GetExampleChild(1);
            container.ExampleByInterface.ExcludableChild = GetExampleChild(1);

            container = container.CloneLazinatorTyped();
            var result = container.CloneLazinatorTyped(IncludeChildrenMode.ExcludeAllChildren);
            result.ExampleByInterface.Should().BeNull();

            container = container.CloneLazinatorTyped();
            result = container.CloneLazinatorTyped(IncludeChildrenMode.IncludeAllChildren);
            result.ExampleByInterface.IncludableChild.Should().NotBeNull();
            result.ExampleByInterface.ExcludableChild.Should().NotBeNull();
            result.ExampleByInterface.MyChild1.Should().NotBeNull();

            container = container.CloneLazinatorTyped();
            result = container.CloneLazinatorTyped(IncludeChildrenMode.ExcludeOnlyExcludableChildren);
            result.ExampleByInterface.IncludableChild.Should().NotBeNull();
            result.ExampleByInterface.ExcludableChild.Should().BeNull();
            result.ExampleByInterface.MyChild1.Should().NotBeNull();

            container = container.CloneLazinatorTyped();
            result = container.CloneLazinatorTyped(IncludeChildrenMode.IncludeOnlyIncludableChildren);
            result.ExampleByInterface.Should().NotBeNull(); // because ExampleByInterface is defined as an includable child
            result.ExampleByInterface.IncludableChild.Should().NotBeNull();
            result.ExampleByInterface.ExcludableChild.Should().BeNull();
            result.ExampleByInterface.MyChild1.Should().BeNull();
        }

    }
}
