using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Lazinator.Collections.Avl;
using Lazinator.Collections.AvlTree;
using Lazinator.Wrappers;

namespace LazinatorTests.AVL
{
    public class AvlSetTests
    {
        [Fact]
        public void AvlSetSearchWorks()
        {
            GetAvlSet(out var set, out var ints);
            foreach (int x in ints)
            {
                bool found = set.Contains(x);
                found.Should().BeTrue();
            }
            for (int x = 0; x < 20; x++)
                if (!ints.Contains(x))
                {
                    bool found = set.Contains(x);
                    found.Should().BeFalse();
                }
        }

        private static void GetAvlSet(out AvlSet<LazinatorWrapperInt> set, out int[] ints)
        {
            set = new AvlSet<LazinatorWrapperInt>();
            ints = new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 13, 14 };
            foreach (int x in ints)
                set.Insert(x);
        }

        [Fact]
        public void AvlMultisetWorks()
        {
            AvlMultiset<LazinatorWrapperInt> s = new AvlMultiset<LazinatorWrapperInt>();
            s.Insert(3);
            s.Insert(5);
            s.Insert(5);
            s.Contains(2).Should().BeFalse();
            s.Contains(3).Should().BeTrue();
            s.Contains(4).Should().BeFalse();
            s.Contains(5).Should().BeTrue();
            s.Contains(6).Should().BeFalse();
            s.NumItemsAdded.Should().Be(3);
            s.RemoveFirstMatchIfExists(5);
            s.Contains(5).Should().BeTrue();
            s.RemoveFirstMatchIfExists(5);
            s.Contains(5).Should().BeFalse();
            s.RemoveFirstMatchIfExists(4);
            // DEBUG -- must implement Count (also for AvlSet) and GetEnumerator().
            
        }

        [Fact]
        public void AvlSetGetMatchOrNextWorks()
        {
            GetAvlSet(out var set, out var ints);
            foreach (int x in ints)
            {
                (bool valueFound, LazinatorWrapperInt valueIfFound) = set.GetMatchOrNext(x);
                valueFound.Should().BeTrue();
                valueIfFound.Value.Should().Be(x);
            }
            for (int x = 0; x < 16; x++)
                if (!ints.Contains(x))
                {
                    (bool valueFound, LazinatorWrapperInt valueIfFound) = set.GetMatchOrNext(x);
                    if (x > ints.Max())
                        valueFound.Should().BeFalse();
                    else
                    {
                        valueIfFound.Value.Should().Be(ints.First(y => y > x));
                    }
                }
        }

        [Fact]
        public void AvlSetEnumeratorWorks()
        {
            GetAvlSet(out var set, out var ints);
            var list = set.ToList();
            list.Select(x => x.Value).SequenceEqual(ints).Should().BeTrue();
        }

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
                    if (x > ints.Max())
                        node.Should().BeNull();
                    else
                    {
                        node.Key.Should().Be(ints.First(y => y > x));
                    }
                }
        }

    }
}