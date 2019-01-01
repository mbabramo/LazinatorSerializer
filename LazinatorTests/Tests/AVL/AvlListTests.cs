using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace LazinatorTests.AVL
{
    public class AvlListTests
    {
        [Fact]
        public void AvlListAddWorks()
        {
            AvlList<WInt> list = new AvlList<WInt>();
            const int numItems = 100;
            for (int i = 0; i < numItems; i++)
                list.Add(i);
            for (int i = 0; i < numItems; i++)
                list[i].WrappedValue.Should().Be(i);
        }

        [Fact]
        public void AvlListCloneDoesntRewriteEverything()
        {
            int DeserializedNodesCount(AvlList<WInt> avlList)
            {
                return avlList.EnumerateLazinatorNodes(x => true, false, x => true, true, false).Where(x => x is AvlNode<WByte, WInt>).Count();
            }

            AvlList<WInt> list = new AvlList<WInt>();
            const int numItems = 1000;
            for (int i = 0; i < numItems; i++)
                list.Add(i);
            var list2 = list.CloneLazinatorTyped();
            int deserializedNodesCount = DeserializedNodesCount(list2);
            list2.Insert(0, -1);
            deserializedNodesCount = DeserializedNodesCount(list2);
            var list3 = list2.CloneLazinatorTyped();
            deserializedNodesCount = DeserializedNodesCount(list2);
            deserializedNodesCount.Should().BeLessThan(25); // works out to exactly 20 -- note that this is because we're always looking right and left at each node to determine where to go, so a fair number of nodes are deserialized.
        }

        [Fact]
        public void AvlListIndexOfWorks()
        {
            AvlList<WInt> avlList = new AvlList<WInt>() { 0, 1, 2 };
            avlList.IndexOf(0).Should().Be(0);
            avlList.IndexOf(1).Should().Be(1);
            avlList.IndexOf(2).Should().Be(2);
            avlList.IndexOf(3).Should().Be(-1);
        }

        [Fact]
        public void AvlListCopyToWorks()
        {
            AvlList<WInt> avlList = new AvlList<WInt>() { 0, 1, 2 };
            WInt[] array = new WInt[6];
            avlList.CopyTo(array, 2);
            array[2].WrappedValue.Should().Be(0);
            array[3].WrappedValue.Should().Be(1);
            array[4].WrappedValue.Should().Be(2);
        }

        [Fact]
        public void AvlListCountWorks()
        {
            AvlList<WInt> avlList = new AvlList<WInt>() { 0, 1, 2 };
            avlList.Count.Should().Be(3);
        }

        [Fact]
        public void AvlListClearWorks()
        {
            AvlList<WInt> avlList = new AvlList<WInt>() { 0, 1, 2 };
            avlList.Clear();
            avlList.Count.Should().Be(0);
        }
    }
}
