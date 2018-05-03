using System;
using System.Collections.Generic;
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
        public void CreateAndSerializeTree()
        {
            AvlTree<LazinatorWrapperInt, LazinatorWrapperInt> tree = new AvlTree<LazinatorWrapperInt, LazinatorWrapperInt>();
            tree.DeserializationFactory = new DeserializationFactory(new Type[] { typeof(AvlTree<,>) }, true);
            Dictionary<int, int> items = new Dictionary<int, int>();
            for (int i = 0; i < 100; i++)
            {
                int k;
                do
                {
                    k = r.Next();
                } while (items.ContainsKey(k));
                int v = r.Next();
                items.Add(k, v);
                tree.Insert(k, v);
            }

            var enumerated = tree.Select(x => x.Value.Value).ToList();
            AvlTree<LazinatorWrapperInt, LazinatorWrapperInt> clone = tree.CloneLazinatorTyped();
            var enumerated2 = clone.Select(x => x.Value.Value).ToList();
            var correctOrder = items.OrderBy(x => x.Key).Select(x => x.Value).ToList();
            enumerated.SequenceEqual(correctOrder).Should().BeTrue();
            enumerated2.SequenceEqual(correctOrder).Should().BeTrue();
        }
    }
}
