using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            GetTreeAndItems(out DeserializationFactory deserializationFactory, out AvlTree<LazinatorWrapperInt, LazinatorWrapperInt> tree, out Dictionary<int, int> items, out int firstKey);

            var enumerated = tree.Select(x => x.Value.WrappedValue).ToList();
            AvlTree<LazinatorWrapperInt, LazinatorWrapperInt> clone = tree.CloneLazinatorTyped();
            var enumerated2 = clone.Select(x => x.Value.WrappedValue).ToList();
            var correctOrder = items.OrderBy(x => x.Key).Select(x => x.Value).ToList();
            enumerated.SequenceEqual(correctOrder).Should().BeTrue();
            enumerated2.SequenceEqual(correctOrder).Should().BeTrue();
        }

        [Fact]
        public void AvlTreeAllowsSearchAfterDeserialization()
        {
            GetTreeAndItems(out DeserializationFactory deserializationFactory, out AvlTree<LazinatorWrapperInt, LazinatorWrapperInt> tree, out Dictionary<int, int> items, out int firstKey);

            MemoryInBuffer b = tree.SerializeNewBuffer(IncludeChildrenMode.IncludeAllChildren, false);
            //const int repetitions = 10000;
            //for (int i = 0; i < repetitions; i++)
            //{
                var tree2 = new AvlTree<LazinatorWrapperInt, LazinatorWrapperInt>()
                {
                    DeserializationFactory = deserializationFactory,
                    HierarchyBytes = b
                };
                bool found = tree2.Search(firstKey, out LazinatorWrapperInt value);
                found.Should().BeTrue();
            //}
        }

        private AvlTree<LazinatorWrapperInt, LazinatorWrapperInt> GetTree()
        {
            GetTreeAndItems(out _, out AvlTree<LazinatorWrapperInt, LazinatorWrapperInt> t, out _, out _ );
            return t;
        }

        private void GetTreeAndItems(out DeserializationFactory deserializationFactory, out AvlTree<LazinatorWrapperInt, LazinatorWrapperInt> tree, out Dictionary<int, int> items, out int firstKey)
        {
            deserializationFactory = new DeserializationFactory(typeof(AvlTree<,>));
            tree = new AvlTree<LazinatorWrapperInt, LazinatorWrapperInt>();
            tree.DeserializationFactory = deserializationFactory;
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
