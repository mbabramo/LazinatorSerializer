using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lazinator.Buffers;
using Lazinator.Collections.Avl;
using Lazinator.Collections.Avl.KeyValueTree;
using Lazinator.Core;
using Lazinator.Wrappers;
using Xunit;

namespace LazinatorTests.AVL
{
    public class AvlSerializationTest
    {
        private Random r = new Random();

        [Fact]
        public void DeserializedAvlTreeOrdersItemsCorrectly()
        {
            GetTreeAndItems(out AvlKeyValueTree<WInt, WInt> tree, out Dictionary<int, int> items, out int firstKey);

            var enumerated = tree.Select(x => x.Value.WrappedValue).ToList();
            AvlKeyValueTree<WInt, WInt> clone = tree.CloneLazinatorTyped();
            var enumerated2 = clone.Select(x => x.Value.WrappedValue).ToList();
            var correctOrder = items.OrderBy(x => x.Key).Select(x => x.Value).ToList();
            enumerated.SequenceEqual(correctOrder).Should().BeTrue();
            enumerated2.SequenceEqual(correctOrder).Should().BeTrue();
        }

        [Fact]
        public void AvlTreeAllowsSearchAfterDeserialization()
        {
            GetTreeAndItems(out AvlKeyValueTree<WInt, WInt> tree, out Dictionary<int, int> items, out int firstKey);

            LazinatorMemory b = tree.SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
            //const int repetitions = 10000;
            //for (int i = 0; i < repetitions; i++)
            //{
                var tree2 = new AvlKeyValueTree<WInt, WInt>(false, false)
                {
                };
                tree2.DeserializeLazinator(b);
                bool found = tree2.ContainsKey(firstKey, Comparer<WInt>.Default);
                found.Should().BeTrue();
            //}
        }

        private AvlKeyValueTree<WInt, WInt> GetTree()
        {
            GetTreeAndItems(out AvlKeyValueTree<WInt, WInt> t, out _, out _ );
            return t;
        }

        private void GetTreeAndItems(out AvlKeyValueTree<WInt, WInt> tree, out Dictionary<int, int> items, out int firstKey)
        {
            tree = new AvlKeyValueTree<WInt, WInt>(false, false);
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
                tree.SetValueForKey(k, v, Comparer<WInt>.Default);
            }
        }
    }
}
