﻿using System;
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
using Lazinator.Support;

namespace LazinatorTests.Tests
{
    public class NodeEnumerationTests : SerializationDeserializationTestBase
    {


        [Fact]
        public void DirtinessEnumerationWorks()
        {
            var hierarchy = GetHierarchy(1, 1, 1, 1, 0);
            var results = hierarchy.GetDirtyNodes(true).ToList();
            results.Count().Should().BeGreaterThan(1);

            var c = hierarchy.CloneLazinatorTyped();
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(0);

            var accessed = c.MyChild1;
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(0);

            c.MyUint = 12345;
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(1);
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(1); // still 1
            c.LazinatorConvertToBytes();
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(0); // nothing is dirty now

            c = c.CloneLazinatorTyped();
            c.MyChild1 = new ExampleChild();
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(2); // parent and MyChild1

            c = c.CloneLazinatorTyped();
            c.MyChild1.MyLong = 565656;
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(1); // child only is dirty
            (results[0] is ExampleChild).Should().BeTrue();

            c = c.CloneLazinatorTyped();
            c.MyChild1 = new ExampleChild();
            c = c.CloneLazinatorTyped();
            c.MyChild1 = new ExampleChild();
            c.MyChild1.MyLong = 7878;
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(2); // now, parent and child again

            c = c.CloneLazinatorTyped();
            c.MyChild1.MyWrapperContainer = new WrapperContainer() { WrappedInt = 8 };
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(3);
            (results[1] is WrapperContainer).Should().BeTrue();
            (results[2] is WInt).Should().BeTrue();

            c = c.CloneLazinatorTyped();
            c.MyChar = 'R';
            c.MyChild1.MyWrapperContainer.WrappedInt = 9;
            c.MyChild2 = new ExampleChild();
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(4);
            (results[0] is Example).Should().BeTrue();
            (results[1] is WrapperContainer).Should().BeTrue(); // wrapped int is a property of the container itself
            (results[2] is WInt).Should().BeTrue();
            (results[3] is ExampleChild).Should().BeTrue(); // i.e., MyChild2
            results = c.GetDirtyNodes(true).ToList();
            results.Count().Should().Be(4); // no change

            results = c.EnumerateLazinatorNodes(null, true, null, true).ToList();
            results.Count().Should().Be(1); // only highest dirty returned 
            (results[0] is Example).Should().BeTrue();

            results = c.EnumerateLazinatorNodes(null, false, x => !(x is WrapperContainer), true).ToList();
            results.Count().Should().Be(4); // WrapperContainer is yielded but not further explored; dirtiness is not a consideration

            results = c.EnumerateLazinatorNodes(x => x is WInt, false, null, true).ToList();
            results.Count().Should().Be(1); // full exploration, but only WInt returned 
            (results[0] is WInt).Should().BeTrue();
        }

        [Fact]
        public void LazinatorListDirtinessEnumerationWorks()
        {
            LazinatorListContainer nonGenericContainer = new LazinatorListContainer()
            {
            };
            nonGenericContainer.MyList = new LazinatorList<ExampleChild>();

            var v2 = nonGenericContainer.CloneLazinatorTyped();
            v2.MyList.Add(GetExampleChild(1));
            v2.MyList.Add(GetExampleChild(1));
            v2.MyList.Add(GetExampleChild(1));

            var results = v2.GetDirtyNodes(true);
            results.Count().Should().Be(4);

            var v5 = v2.CloneLazinatorTyped();
            v5.MyList[1].MyLong = -98765;
            results = v5.GetDirtyNodes(true);
            results.Count().Should().Be(1);
        }

        [Fact]
        public void LazinatorListEnumerateNodesWorks()
        {
            LazinatorList<Example> l = new LazinatorList<Example>() { GetExample(1), GetExample(1) };
            var c = l.CloneLazinatorTyped();
            var results2 = c.GetAllNodes().ToList();
            results2[0].Should().Be(c);
            results2[1].LazinatorParentClass.Should().Be(c);
        }

        [Fact]
        public void HierarchyTreeWorks_TwoLevel()
        {
            var hierarchy = GetHierarchy(1, 1, 1, 1, 0);
            HierarchyTree tree = hierarchy.GetAllNodesAsHierarchy();
            string result = tree.ToString();
            string expected = $@"LazinatorTests.Examples.Example
    LazinatorTests.Examples.ExampleChild
    LazinatorTests.Examples.ExampleChild
    2
";
            result.Should().Be(expected);
        }

        [Fact]
        public void HierarchyTreeWorks_Larger()
        {
            var hierarchy = GetHierarchy(0, 1, 2, 0, 0);
            hierarchy.MyChild1.MyWrapperContainer = new WrapperContainer() { WrappedInt = 17 };
            HierarchyTree tree = hierarchy.GetAllNodesAsHierarchy();
            string result = tree.ToString();
            string expected = 
$@"LazinatorTests.Examples.Example
    LazinatorTests.Examples.ExampleChild
        LazinatorTests.Examples.Structs.WrapperContainer
            17
    LazinatorTests.Examples.ExampleChild
    5
";
            result.Should().Be(expected);
        }
    }
}
