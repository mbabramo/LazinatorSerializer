using Lazinator.Buffers;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Factories;
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
        public bool Unbalanced { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILazinatorListableFactory<T> ListableFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public AvlListTree(bool allowDuplicates, bool unbalanced, ILazinatorListableFactory<T> listableFactory)
        {
            AllowDuplicates = allowDuplicates;
            Unbalanced = unbalanced;
            ListableFactory = listableFactory;
            UnderlyingTree = new AvlIndexableTree<ILazinatorListable<T>>() { Unbalanced = Unbalanced, AllowDuplicates = AllowDuplicates };
        }

        public IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlListTree<T>()
            {
                AllowDuplicates = AllowDuplicates,
                Unbalanced = Unbalanced,
                ListableFactory = ListableFactory
            };
        }

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

        public bool Any()
        {
            return UnderlyingTree.Any();
        }

        public T First()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return UnderlyingTree.First().First();
        }

        public T FirstOrDefault()
        {
            if (!Any())
                return default;
            return UnderlyingTree.First().First();
        }

        public T Last()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return UnderlyingTree.Last().Last();
        }

        public T LastOrDefault()
        {
            if (!Any())
                return default;
            return UnderlyingTree.Last().Last();
        }

        public bool GetValue(T item, IComparer<T> comparer, out T match)
        {
            throw new NotImplementedException();
        }

        public bool TryInsert(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryRemove(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator(bool reverse = false, long skip = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool GetValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, out T match)
        {
            throw new NotImplementedException();
        }

        public bool TryInsert(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryRemoveAll(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public long Count(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public ILazinatorSplittable SplitOff()
        {
            throw new NotImplementedException();
        }
    }

}