using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Lazinator.Collections.Avl;
using Lazinator.Wrappers;

namespace LazinatorTests.AVL
{
    public class AvlTreeSearchTests
    {
        [Fact]
        public void SearchWorks()
        {
            var tree = new AvlTree<LazinatorWrapperInt, LazinatorWrapperString>();
            int[] ints = new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 13, 14 };
            foreach (int x in ints)
                tree.Insert(x, x.ToString());
            foreach (int x in ints)
            {
                bool found = tree.Search(x, out LazinatorWrapperString value);
                found.Should().BeTrue();
                value.Value.Should().Be(x.ToString());
            }
            for (int x = 0; x < 16; x++)
                if (!ints.Contains(x))
                {
                    bool found = tree.Search(x, out LazinatorWrapperString value);
                    found.Should().BeFalse();
                }
        }

        [Fact]
        public void SearchMatchOrNextWorks()
        {
            var tree = new AvlTree<LazinatorWrapperInt, LazinatorWrapperString>();
            int[] ints = new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 13, 14 };
            foreach (int x in ints)
                tree.Insert(x, x.ToString());
            foreach (int x in ints)
            {
                var node = tree.SearchMatchOrNext(x);
                node.Value.Value.Should().Be(x.ToString());
            }
            for (int x = 0; x < 16; x++)
                if (!ints.Contains(x))
                {
                    var node = tree.SearchMatchOrNext(x);
                    if (x == ints.Max())
                        node.Should().BeNull();
                    else
                    {
                        node.Key.Should().Be(ints.First(y => y > x));
                    }
                }
        }

    }
}