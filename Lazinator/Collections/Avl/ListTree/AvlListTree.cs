using Lazinator.Buffers;
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
        public IAvlListTreeInteriorCollectionFactory<T> InteriorCollectionFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public AvlListTree(bool allowDuplicates, bool unbalanced, IAvlListTreeInteriorCollectionFactory<T> interiorCollectionFactory)
        {
            AllowDuplicates = allowDuplicates;
            Unbalanced = unbalanced;
            InteriorCollectionFactory = interiorCollectionFactory;
            UnderlyingTree2 = new AvlIndexableTree<IMultivalueContainer<T>>() { Unbalanced = Unbalanced, AllowDuplicates = AllowDuplicates };
        }

        public IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlListTree<T>()
            {
                AllowDuplicates = AllowDuplicates,
                Unbalanced = Unbalanced,
                InteriorCollectionFactory = InteriorCollectionFactory
            };
        }

        private int CompareBasedOnEndItems(BinaryNode<IMultivalueContainer<T>> node, T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
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

        protected AvlCountedNode<IMultivalueContainer<T>> GetNodeForValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, bool chooseShorterIfInBetween)
        {
            var matchInfo = UnderlyingTree2.GetMatchingOrNextNode(whichOne, n => CompareBasedOnEndItems((AvlCountedNode<IMultivalueContainer<T>>)n, item, whichOne, comparer));
            var node = matchInfo.found ? (AvlCountedNode<IMultivalueContainer<T>>)matchInfo.node : (AvlCountedNode<IMultivalueContainer<T>>)UnderlyingTree2.LastNode();
            if (node == null || !chooseShorterIfInBetween)
                return node;
            bool isBeforeThis = comparer.Compare(item, node.Value.First()) == -1;
            if (isBeforeThis)
            {
                AvlCountedNode<IMultivalueContainer<T>> previousNode = (AvlCountedNode<IMultivalueContainer<T>>)node.GetPreviousNode();
                bool inBetweenThisAndPrevious = previousNode != null && comparer.Compare(item, previousNode.Value.Last()) == 1;
                if (inBetweenThisAndPrevious)
                {
                    if (InteriorCollectionFactory.FirstIsShorter(previousNode.Value, node.Value))
                        return previousNode;
                }
            }
            return node;
        }

        private IMultivalueContainer<T> GetMultivalueContainer(AvlCountedNode<IMultivalueContainer<T>> node)
        {
            if (node == null)
                return null;
            var result = node.Value as IMultivalueContainer<T>;
            return result;
        }

        private IMultivalueContainer<T> GetMultivalueContainerForValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, bool chooseShorterIfInBetween) => GetMultivalueContainer(GetNodeForValue(item, whichOne, comparer, chooseShorterIfInBetween));

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
            foreach (var multivalueContainer in UnderlyingTree2.AsEnumerable(reverse, 0))
            {
                if (multivalueContainer is ICountableContainer countable)
                { // enumerate by skipping entire containers
                    if (skip >= countable.LongCount)
                    {
                        skip -= countable.LongCount;
                        continue;
                    }
                    else
                    {
                        foreach (T t in multivalueContainer.AsEnumerable(reverse, skip))
                            yield return t;
                    }
                }
                else
                { // enumerate each item one by one
                    foreach (T t in multivalueContainer.AsEnumerable(reverse, 0))
                    {
                        if (skip > 0)
                            skip--;
                        else
                            yield return t;
                    }
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
            var multivalueContainer = GetMultivalueContainerForValue(item, whichOne, comparer, false);
            if (multivalueContainer == null)
            {
                match = default;
                return false;
            }
            bool result = multivalueContainer.GetValue(item, whichOne, comparer, out match);
            return result;
        }

        public bool TryInsert(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var node = GetNodeForValue(item, whichOne, comparer, false);
            var multivalueContainer = GetMultivalueContainer(node);
            var result = multivalueContainer.TryInsert(item, whichOne, comparer);
            if (InteriorCollectionFactory.RequiresSplitting(multivalueContainer))
            {
                var splitOff = multivalueContainer.Splitt
            }
            return result;
        }

        public bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var node = GetNodeForValue(item, whichOne, comparer, false);
            var multivalueContainer = GetMultivalueContainer(node);
            bool result = multivalueContainer.TryRemove(item, whichOne, comparer);
            if (result && multivalueContainer.Any() == false)
            {
                // Remove the node, since nothing is left in it.
                UnderlyingTree2.RemoveAt(node.Index);
            }
            return result;
        }

        public bool TryRemoveAll(T item, IComparer<T> comparer)
        {
            var multivalueContainer = GetMultivalueContainerForValue(item, MultivalueLocationOptions.Any, comparer, false);
            bool any = multivalueContainer.TryRemove(item, comparer);
            if (any)
            {
                do
                {
                } while (multivalueContainer.TryRemove(item, comparer));
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
                count += node.Value.Count(item, comparer);
                if (count == 0)
                    keepGoing = false;
                else
                {
                    node = (AvlCountedNode<IMultivalueContainer<T>>) node.GetNextNode();
                    keepGoing = node != null;
                }
            }
            return count;
        }

        public ILazinatorSplittable SplitOff()
        {
            throw new NotImplementedException();
        }
    }

}