using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections;
using LazinatorTests.Examples;
using LazinatorTests.Examples.Collections;
using Lazinator.Core;
using LazinatorTests.Examples.Tuples;
using Xunit;
using Lazinator.Wrappers;
using LazinatorTests.Examples.Structs;

namespace LazinatorTests.Tests
{
    public class LazinatorTreeTests
    {
        [Fact]
        public void CanCreateLazinatorTree()
        {
            LazinatorGeneralTree<WString> root = new LazinatorGeneralTree<WString>("Root");
            root.GetLocationInTree().SequenceEqual(new List<int>() { }).Should().BeTrue();
            var root_0 = root.AddChild("Root-0");
            root_0.GetLocationInTree().SequenceEqual(new List<int>() {0}).Should().BeTrue();
            var root_1 = root.AddChild("Root-1");
            root_1.GetLocationInTree().SequenceEqual(new List<int>() { 1 }).Should().BeTrue();
            var root_2 = root.AddChild("Root-2");
            root_2.GetLocationInTree().SequenceEqual(new List<int>() { 2 }).Should().BeTrue();
            var root_1_0 = root_1.AddChild("Root-1-0");
            root_1_0.GetLocationInTree().SequenceEqual(new List<int>() { 1, 0 }).Should().BeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CanTraverseLazinatorTree(bool clone)
        {
            LazinatorGeneralTree<WString> root = GetTree(clone);
            var traversed = root.Traverse().ToList();
            var expected = new List<(WString, int)>()
            {
                ("Root", 0),
                ("Root-0", 1),
                ("Root-1", 1),
                ("Root-1-0", 2),
                ("Root-1-1", 2),
                ("Root-1-1-0", 3),
                ("Root-2", 1),
                ("Root-2-0", 2)
            };
            traversed.SequenceEqual(expected).Should().BeTrue();
        }
        
        private static LazinatorGeneralTree<WString> GetTree(bool clone)
        {
            LazinatorGeneralTree<WString> root = new LazinatorGeneralTree<WString>("Root");
            var root_0 = root.AddChild("Root-0");
            var root_1 = root.AddChild("Root-1");
            var root_2 = root.AddChild("Root-2");
            var root_1_0 = root_1.AddChild("Root-1-0");
            var root_2_0 = root_2.AddChild("Root-2-0");
            var root_1_1 = root_1.AddChild("Root-1-1");
            var root_1_1_0 = root_1_1.AddChild("Root-1-1-0");
            if (clone)
                root = (LazinatorGeneralTree<WString>) root.CloneLazinator();
            return root;
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CanFindLocationInLazinatorTree(bool clone)
        {
            LazinatorGeneralTree<WString> root = GetTree(clone);
            var root_1 = root.GetTreeAtLocation(new List<int> {1});
            root_1.GetLocationInTree().SequenceEqual(new List<int> { 1 }).Should().BeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LazinatorTreeNonserializedProperties(bool clone)
        {
            LazinatorGeneralTree<WString> root = GetTree(clone);
            root.Index.Should().Be(0);
            root.Level.Should().Be(0);
            root.ParentTree.Should().Be(null);
            var root_1 = root.GetTreeAtLocation(new List<int> { 1 });
            root_1.Index.Should().Be(1);
            root_1.Level.Should().Be(1);
            root_1.ParentTree.Should().Be(root);
            var root_1_0 = root_1.GetTreeAtIndex(0);
            root_1_0.Index.Should().Be(0);
            root_1_0.Level.Should().Be(2);
            root_1_0.ParentTree.Should().Be(root_1);
            var root_1_1 = root_1.GetTreeAtIndex(1);
            root_1_1.Index.Should().Be(1);
            root_1_1.Level.Should().Be(2);
            root_1_1.ParentTree.Should().Be(root_1);
            var root_1_1_0 = root_1_1.GetTreeAtIndex(0);
            root_1_1_0.Index.Should().Be(0);
            root_1_1_0.Level.Should().Be(3);
            root_1_1_0.ParentTree.Should().Be(root_1_1);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LazinatorTree_CanInsert(bool clone)
        {
            LazinatorGeneralTree<WString> root = GetTree(clone);
            var root_2 = root.GetTreeAtLocation(new List<int> { 2 }); // original root 2
            var root_1A = root.InsertChild("Root-1A", 2);
            root_1A.Index.Should().Be(2);
            root_1A.Level.Should().Be(1);
            root_1A.ParentTree.Should().Be(root);
            var root_1 = root.GetTreeAtLocation(new List<int> { 1 });
            root_1.Index.Should().Be(1);
            root_1.Level.Should().Be(1);
            root_1.ParentTree.Should().Be(root);
            var root_1_1 = root.GetTreeAtLocation(new List<int> { 1, 1 });
            root_1_1.Index.Should().Be(1);
            root_1_1.Level.Should().Be(2);
            root_1_1.ParentTree.Should().Be(root_1);
            var root_3 = root.GetTreeAtLocation(new List<int> { 3 });
            root_3.Should().Be(root_2);
            root_3.Item.WrappedValue.Should().Be("Root-2"); // name is same, but
            root_3.Index.Should().Be(3); // position is changed
            root_3.Level.Should().Be(1);
            root_3.ParentTree.Should().Be(root);
            var root_1_0A = root_1.InsertChild("Root-1-0A", 1);
            root_1_0A.Index.Should().Be(1);
            root_1_0A.Level.Should().Be(2);
            root_1_0A.ParentTree.Should().Be(root_1);
            root_1_1.Index.Should().Be(2);
            root_1_1.Level.Should().Be(2);
            root_1_1.ParentTree.Should().Be(root_1);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LazinatorTree_CanRemove(bool clone)
        {
            LazinatorGeneralTree<WString> root = GetTree(clone);
            int childrenCount = root.GetChildren().Count();
            var root_0 = root.GetTreeAtLocation(new List<int> { 0 });
            var root_1 = root.GetTreeAtLocation(new List<int> { 1 });
            root_1.Index.Should().Be(1);
            var root_1_1 = root.GetTreeAtLocation(new List<int> { 1, 1 });
            bool successful = root.RemoveChild(root_0.Item);
            successful.Should().BeTrue();
            root_1.Index.Should().Be(0);
        }
    }
}
