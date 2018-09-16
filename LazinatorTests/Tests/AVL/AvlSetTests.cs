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

        private static void GetAvlSet(out AvlSet<WInt> set, out int[] ints)
        {
            set = new AvlSet<WInt>();
            ints = new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 13, 14 };
            foreach (int x in ints)
                set.Insert(x);
        }

        [Fact]
        public void AvlSetSkipWorks()
        {
            GetAvlSet(out var set, out var ints);
            set.Skip(2).Skip(2).First().WrappedValue.Should().Be(6);
        }

        [Fact]
        public void AvlMultisetWorks()
        {
            AvlMultiset<WInt> s = new AvlMultiset<WInt>();
            s.Insert(3);
            s.Insert(5);
            s.Insert(5);
            s.Contains(2).Should().BeFalse();
            s.Contains(3).Should().BeTrue();
            s.Contains(4).Should().BeFalse();
            s.Count.Should().Be(3);
            s.Contains(5).Should().BeTrue();
            s.Contains(6).Should().BeFalse();
            s.NumItemsAdded.Should().Be(3);
            s.ToList().Select(x => x.WrappedValue).SequenceEqual(new int[] {3, 5, 5}).Should().BeTrue();
            s.RemoveFirstMatchIfExists(5);
            s.Count.Should().Be(2);
            s.Contains(5).Should().BeTrue();
            s.RemoveFirstMatchIfExists(5);
            s.Contains(5).Should().BeFalse();
            s.Count.Should().Be(1);
            s.RemoveFirstMatchIfExists(4);
            s.Count.Should().Be(1);
        }

        [Fact]
        public void AvlMultisetSkipWorks()
        {
            AvlMultiset<WInt> s = new AvlMultiset<WInt>();
            s.Insert(3);
            s.Insert(5);
            s.Insert(5);
            s.Skip(2).First().WrappedValue.Should().Be(5);
        }

        [Fact]
        public void AvlMultisetComparerWorks()
        {
            // The AvlMultiset comparer sets the comparer of the underlying set, which sets the comparer of the underlying tree, so this serves to check all their functionality. 
            CustomComparer<WInt> reverseComparer =
                new CustomComparer<WInt>((x, y) => 0 - x.CompareTo(y));
            AvlMultiset<WInt> s = new AvlMultiset<WInt>(reverseComparer);
            s.Insert(3);
            s.Insert(5);
            s.Insert(5);
            List<WInt> list = s.ToList();
            list.Select(x => x.WrappedValue).SequenceEqual(new int[] {5, 5, 3}).Should().BeTrue();
        }

        [Fact]
        public void AvlSetGetMatchOrNextWorks()
        {
            GetAvlSet(out var set, out var ints);
            foreach (int x in ints)
            {
                (bool valueFound, WInt valueIfFound) = set.GetMatchOrNext(x);
                valueFound.Should().BeTrue();
                valueIfFound.WrappedValue.Should().Be(x);
            }
            for (int x = 0; x < 16; x++)
                if (!ints.Contains(x))
                {
                    (bool valueFound, WInt valueIfFound) = set.GetMatchOrNext(x);
                    if (x > ints.Max())
                        valueFound.Should().BeFalse();
                    else
                    {
                        valueIfFound.WrappedValue.Should().Be(ints.First(y => y > x));
                    }
                }
        }

        [Fact]
        public void AvlSetEnumeratorWorks()
        {
            GetAvlSet(out var set, out var ints);
            var list = set.ToList();
            list.Select(x => x.WrappedValue).SequenceEqual(ints).Should().BeTrue();
        }

        [Fact]
        public void SearchWorks()
        {
            var tree = new AvlTree<WInt, WString>();
            int[] ints = new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 13, 14 };
            foreach (int x in ints)
                tree.Insert(x, x.ToString());
            foreach (int x in ints)
            {
                bool found = tree.Search(x, out WString value);
                found.Should().BeTrue();
                value.WrappedValue.Should().Be(x.ToString());
            }
            for (int x = 0; x < 16; x++)
                if (!ints.Contains(x))
                {
                    bool found = tree.Search(x, out WString value);
                    found.Should().BeFalse();
                }
        }

        [Fact]
        public void SearchMatchOrNextWorks()
        {
            var tree = new AvlTree<WInt, WString>();
            int[] ints = new int[] { 1, 2, 3, 4, 6, 7, 8, 10, 13, 14 };
            foreach (int x in ints)
                tree.Insert(x, x.ToString());
            foreach (int x in ints)
            {
                var node = tree.SearchMatchOrNext(x);
                node.Value.WrappedValue.Should().Be(x.ToString());
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