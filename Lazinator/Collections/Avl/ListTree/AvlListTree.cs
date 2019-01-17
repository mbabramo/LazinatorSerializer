﻿using Lazinator.Buffers;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tree;
using Lazinator.Core;
using Lazinator.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ListTree
{
    public partial class AvlListTree<T> : IAvlListTree<T>, IValueContainer<T>, IMultivalueContainer<T>, ILazinatorSplittable where T : ILazinator
    {

        public AvlListTree(bool allowDuplicates, bool unbalanced, ILazinatorListableFactory<T> listableFactory)
        {
            AllowDuplicates = allowDuplicates;
            Unbalanced = unbalanced;
            ListableFactory = listableFactory;
            UnderlyingTree2 = new AvlIndexableTree<ILazinatorListable<T>>() { Unbalanced = Unbalanced, AllowDuplicates = AllowDuplicates };
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
            var matchInfo = UnderlyingTree2.GetMatchingOrNextNode(whichOne, n => CompareBasedOnEndItems((AvlCountedNode<ILazinatorListable<T>>)n, item, whichOne, comparer));
            var node = matchInfo.found ? (AvlCountedNode<ILazinatorListable<T>>)matchInfo.node : (AvlCountedNode<ILazinatorListable<T>>)UnderlyingTree2.LastNode();
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

        private ILazinatorSortable<T> GetSortable(AvlCountedNode<ILazinatorListable<T>> node)
        {
            if (node == null)
                return null;
            var result = node.Value as ILazinatorSortable<T>;
            if (result == null)
                throw new Exception("The list in the AvlListTree must be sortable to use this method.");
            return result;
        }

        private ILazinatorSortable<T> GetSortableForValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, bool chooseShorterIfInBetween) => GetSortable(GetNodeForValue(item, whichOne, comparer, chooseShorterIfInBetween));

        public bool Any()
        {
            return UnderlyingTree2.Any();
        }

        public T First()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return UnderlyingTree2.First().First();
        }

        public T FirstOrDefault()
        {
            if (!Any())
                return default;
            return UnderlyingTree2.First().First();
        }

        public T Last()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return UnderlyingTree2.Last().Last();
        }

        public T LastOrDefault()
        {
            if (!Any())
                return default;
            return UnderlyingTree2.Last().Last();
        }

        public void Clear()
        {
            UnderlyingTree2.Clear();
        }

        public IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            foreach (var sortable in UnderlyingTree2.AsEnumerable(reverse, 0))
            {
                if (skip >= sortable.LongCount)
                {
                    skip -= sortable.LongCount;
                    continue;
                }
                else
                {
                    foreach (T t in sortable.AsEnumerable(reverse, skip))
                        yield return t;
                }
            }
        }

        public IEnumerator<T> GetEnumerator(bool reverse = false, long skip = 0)
        {
            return (IEnumerator<T>)AsEnumerable(reverse, skip);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(false, 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(false, 0);
        }


        public bool GetValue(T item, IComparer<T> comparer, out T match) => GetValue(item, MultivalueLocationOptions.Any, comparer, out match);

        public bool TryInsert(T item, IComparer<T> comparer) => TryInsert(item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any, comparer);

        public bool TryRemove(T item, IComparer<T> comparer) => TryRemove(item, MultivalueLocationOptions.Any, comparer);

        public bool GetValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, out T match)
        {
            var sortable = GetSortableForValue(item, whichOne, comparer, false);
            if (sortable == null)
            {
                match = default;
                return false;
            }
            var result = sortable.Find(item, whichOne, comparer);
            if (result.exists)
            {
                match = sortable.GetAt(result.index);
                return true;
            }
            else
            {
                match = default;
                return false;
            }
        }

        public bool TryInsert(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var node = GetNodeForValue(item, whichOne, comparer, false);
            var sortable = GetSortable(node);
            (long index, bool insertedNotReplaced) = sortable.InsertGetIndex(item, whichOne, comparer);
            // var DEBUG
            return insertedNotReplaced;
        }

        public bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var node = GetNodeForValue(item, whichOne, comparer, false);
            var sortable = GetSortable(node);
            bool result = sortable.TryRemove(item, whichOne, comparer);
            if (result && sortable.Any() == false)
            {
                // Remove the node, since nothing is left in it.
                UnderlyingTree2.RemoveAt(node.Index);
            }
            return result;
        }

        public bool TryRemoveAll(T item, IComparer<T> comparer)
        {
            var sortable = GetSortableForValue(item, MultivalueLocationOptions.Any, comparer, false);
            bool any = sortable.TryRemove(item, comparer);
            if (any)
            {
                do
                {
                } while (sortable.TryRemove(item, comparer));
            }
            return any;
        }

        public long Count(T item, IComparer<T> comparer)
        {
            var node = GetNodeForValue(item, MultivalueLocationOptions.First, comparer, false);
            if (node == null)
                return 0;
            // The item might appear in multiple nodes, so we need to check this node and subsequent nodes.
            bool keepGoing = true;
            long count = 0;
            while (keepGoing)
            {
                count += CountInNode(item, comparer, node);
                if (count == 0)
                    keepGoing = false;
                else
                {
                    node = (AvlCountedNode<ILazinatorListable<T>>) node.GetNextNode();
                    keepGoing = node != null;
                }
            }
            return count;
        }

        private long CountInNode(T item, IComparer<T> comparer, AvlCountedNode<ILazinatorListable<T>> node)
        {
            ILazinatorSortable<T> sortable = GetSortable(node);
            var first = sortable.Find(item, MultivalueLocationOptions.First, comparer);
            if (first.exists == false)
                return 0;
            var last = sortable.Find(item, MultivalueLocationOptions.Last, comparer);
            return (last.index - first.index + 1);
        }

        public ILazinatorSplittable SplitOff()
        {
            throw new NotImplementedException();
        }
    }

}