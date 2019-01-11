using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections.Avl;
using Lazinator.Support;
using Lazinator.Wrappers;

namespace LazinatorTests.AVL
{
    public class AvlSetTests
    {

        [Fact]
        public void AvlTreeSearchWorks()
        {
            var tree = new AvlSortedIndexableKeyValueTree<WInt, WString>();
            int[] ints = new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 13, 14 };
            foreach (int x in ints)
                tree.SetValueForKey(x, x.ToString());
            foreach (int x in ints)
            {
                bool found = tree.GetValueForKey(x, out WString value);
                found.Should().BeTrue();
                value.WrappedValue.Should().Be(x.ToString());
            }
            for (int x = 0; x < 16; x++)
                if (!ints.Contains(x))
                {
                    bool found = tree.ValueAtKey(x, out WString value);
                    found.Should().BeFalse();
                }
        }

        [Fact]
        public void SearchMatchOrNextWorks()
        {
            var tree = new AvlOldTree<WInt, WString>();
            int[] ints = new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 13, 14 };
            foreach (int x in ints)
                tree.Insert(x, x.ToString());
            foreach (int x in ints)
            {
                (AvlOldNode<WInt, WString> node, long index, bool found) = tree.GetMatchingOrNextNode(x);
                node.Value.WrappedValue.Should().Be(x.ToString());
            }
            for (int x = 0; x < 16; x++)
                if (!ints.Contains(x))
                {
                    var result = tree.GetMatchingOrNextNode(x);
                    if (x > ints.Max())
                        result.found.Should().BeFalse();
                    else
                    {
                        result.node.Key.Should().Be(ints.First(y => y > x));
                    }
                }
        }

    }
}