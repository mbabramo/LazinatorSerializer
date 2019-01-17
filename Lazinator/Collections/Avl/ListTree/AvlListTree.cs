using Lazinator.Buffers;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tree;
using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ListTree
{
    public partial class AvlListTree<T> : IAvlListTree<T>, IValueContainer<T>, IMultivalueContainer<T>, ILazinatorSplittable where T : ILazinator
    {
        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private int CompareBasedOnEndItems(BinaryNode<ILazinatorListable<T>> node, T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            T last = node.Value.Last();
            var lastComparison = comparer.Compare(item, last);
            if (lastComparison >= 0)
                return lastComparison; // item is last or after
            T first = node.Value.First();
            var firstComparison = comparer.Compare(item, first);
            if (firstComparison <= 0)
                return firstComparison; // item is first or before
            return 0; // item is between first and last
        }

        protected int Foo(int? x)
        {
            if (x is int y)
                return y;
            return default;
        }

        protected AvlCountedNode<ILazinatorListable<T>> GetNodeForValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, bool chooseShorterIfInBetween)
        {
            var matchInfo = UnderlyingTree.GetMatchingOrNextNode(whichOne, n => CompareBasedOnEndItems((AvlCountedNode<ILazinatorListable<T>>)n, item, whichOne, comparer));
            var node = matchInfo.found ? (AvlCountedNode<ILazinatorListable<T>>)matchInfo.node : (AvlCountedNode<ILazinatorListable<T>>)UnderlyingTree.LastNode();
            if (node == null || !chooseShorterIfInBetween)
                return node;
            bool isBeforeThis = comparer.Compare(item, node.Value.First()) == -1;
            if (isBeforeThis)
            {
                AvlCountedNode<ILazinatorListable<T>> previousNode = (AvlCountedNode<ILazinatorListable<T>>)node.GetPreviousNode();
                bool inBetweenThisAndPrevious = previousNode != null && comparer.Compare(item, previousNode.Value.Last()) == 1;
                if (inBetweenThisAndPrevious)
                {
                    long previousCount = previousNode.Value.LongCount;
                    long currentCount = node.Value.LongCount;
                    if (previousCount < currentCount)
                        return previousNode;
                }
            }
            return node;
        }
    }

}