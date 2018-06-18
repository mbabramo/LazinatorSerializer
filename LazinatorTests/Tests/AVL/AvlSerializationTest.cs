using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;
using Xunit;

namespace LazinatorTests.Tests.AVL
{
    public class AvlSerializationTest
    {
        private Random r = new Random();

        [Fact]
        public void DeserializedAvlTreeOrdersItemsCorrectly()
        {
            GetTreeAndItems(out AvlTree<WInt, WInt> tree, out Dictionary<int, int> items, out int firstKey);

            var enumerated = tree.Select(x => x.Value.WrappedValue).ToList();
            AvlTree<WInt, WInt> clone = tree.CloneLazinatorTyped();
            var enumerated2 = clone.Select(x => x.Value.WrappedValue).ToList();
            var correctOrder = items.OrderBy(x => x.Key).Select(x => x.Value).ToList();
            enumerated.SequenceEqual(correctOrder).Should().BeTrue();
            enumerated2.SequenceEqual(correctOrder).Should().BeTrue();
        }

        [Fact]
        public void AvlTreeAllowsSearchAfterDeserialization()
        {
            GetTreeAndItems(out AvlTree<WInt, WInt> tree, out Dictionary<int, int> items, out int firstKey);

            MemoryInBuffer b = tree.SerializeNewBuffer(IncludeChildrenMode.IncludeAllChildren, false);
            //const int repetitions = 10000;
            //for (int i = 0; i < repetitions; i++)
            //{
                var tree2 = new AvlTree<WInt, WInt>()
                {
                    HierarchyBytes = b
                };
                bool found = tree2.Search(firstKey, out WInt value);
                found.Should().BeTrue();
            //}
        }

        private AvlTree<WInt, WInt> GetTree()
        {
            GetTreeAndItems(out AvlTree<WInt, WInt> t, out _, out _ );
            return t;
        }

        private void GetTreeAndItems(out AvlTree<WInt, WInt> tree, out Dictionary<int, int> items, out int firstKey)
        {
            tree = new AvlTree<WInt, WInt>();
            items = new Dictionary<int, int>();
            firstKey = 0;
            for (int i = 0; i < 100; i++)
            {
                int k;
                do
                {
                    k = r.Next();
                } while (items.ContainsKey(k));

                if (i == 0)
                    firstKey = k;
                int v = r.Next();
                items.Add(k, v);
                tree.Insert(k, v);
            }
        }
    }
}
