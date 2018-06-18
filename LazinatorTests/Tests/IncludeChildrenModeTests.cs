using FluentAssertions;
using Lazinator.Core;
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
