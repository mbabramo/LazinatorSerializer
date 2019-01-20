using Lazinator.Buffers;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Extensions;
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
    public partial class AvlListTree<T> : IAvlListTree<T>, IValueContainer<T>, IMultivalueContainer<T> where T : ILazinator
    {
        public AvlListTree(bool allowDuplicates, bool unbalanced, ContainerFactory<T> interiorCollectionFactory)
        {
            AllowDuplicates = allowDuplicates;
            Unbalanced = unbalanced;
            InteriorContainerFactory = interiorCollectionFactory;
            UnderlyingTree = new AvlIndexableTree<IMultivalueContainer<T>>(AllowDuplicates, Unbalanced);
        }

        private void AllowDuplicatesChanged(bool value)
        {
            // DEBUG -- delete this
        }

        public IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlListTree<T>(AllowDuplicates, Unbalanced, InteriorContainerFactory);
        }

        private static int CompareBasedOnEndItems(IMultivalueContainer<T> container, T item, IComparer<T> comparer)
        {
            T last = container.Last();
            var lastComparison = comparer.Compare(item, last);
            if (lastComparison >= 0)
                return lastComparison; // item is last or after
            T first = container.First();
            var firstComparison = comparer.Compare(item, first);
            if (firstComparison <= 0)
                return firstComparison; // item is first or before
            return 0; // item is between first and last
        }

        /// <summary>
        /// Returns a comparer to compare an item to an interior collection. Usually, the custom comparer can only compare like objects, so null should be passed as the interior collection being compared; this custom comparer then substitutes the item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        private CustomComparer<IMultivalueContainer<T>> GetItemToInteriorCollectionComparer(T item, IComparer<T> comparer)
        {
            return new CustomComparer<IMultivalueContainer<T>>((a, b) =>
            {
                if (a == null)
                    return CompareBasedOnEndItems(b, item, comparer);
                return 0 - CompareBasedOnEndItems(a, item, comparer);
            });
        }

        private CustomComparer<IMultivalueContainer<T>> GetInteriorCollectionsComparer(IComparer<T> comparer)
        {
            return new CustomComparer<IMultivalueContainer<T>>((a, b) =>
            {
                // compare based on firsts
                var aFirst = a.First();
                var bFirst = b.First();
                int firstComparison = comparer.Compare(aFirst, bFirst);
                if (firstComparison != 0)
                    return firstComparison;
                // compare based on lasts
                var aLast = a.Last();
                var bLast = b.Last();
                int lastComparison = comparer.Compare(aLast, bLast);
                return lastComparison;
            });
        }

        protected AvlCountedNode<IMultivalueContainer<T>> GetNodeForValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, bool chooseShorterIfInBetween)
        {
            // If inserting before the first or after the last, we still want the node containing the first or last.
            MultivalueLocationOptions whichOneModified = whichOne;
            if (whichOne == MultivalueLocationOptions.InsertAfterLast)
                whichOneModified = MultivalueLocationOptions.Last;
            else if (whichOne == MultivalueLocationOptions.InsertBeforeFirst)
                whichOneModified = MultivalueLocationOptions.First;
            var matchInfo = UnderlyingTree.GetMatchingOrNextNode(null, whichOneModified, GetItemToInteriorCollectionComparer(item, comparer));
            var node = (AvlCountedNode<IMultivalueContainer<T>>)matchInfo.node ?? (AvlCountedNode<IMultivalueContainer<T>>)UnderlyingTree.LastNode();
            if (node == null || !chooseShorterIfInBetween)
                return node;
            bool isBeforeThis = comparer.Compare(item, node.Value.First()) == -1;
            if (isBeforeThis)
            {
                AvlCountedNode<IMultivalueContainer<T>> previousNode = (AvlCountedNode<IMultivalueContainer<T>>)node.GetPreviousNode();
                bool inBetweenThisAndPrevious = previousNode != null && comparer.Compare(item, previousNode.Value.Last()) == 1;
                if (inBetweenThisAndPrevious)
                {
                    if (InteriorContainerFactory.FirstIsShorter(previousNode.Value, node.Value))
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


        public IContainerLocation FirstLocation() => new AvlListTreeLocation<T>() { OuterNode = (AvlCountedNode<IMultivalueContainer<T>>) UnderlyingTree.FirstNode(), InnerLocation = UnderlyingTree.First().FirstLocation() };
        public IContainerLocation LastLocation() => new AvlListTreeLocation<T>() { OuterNode = (AvlCountedNode<IMultivalueContainer<T>>)UnderlyingTree.LastNode(), InnerLocation = UnderlyingTree.Last().LastLocation() };

        public void Clear()
        {
            UnderlyingTree.Clear();
        }

        public IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            // Because this tree doesn't store indexing, we still have to go through container by container, though this should be a little faster than going through item by item.
            foreach (var multivalueContainer in UnderlyingTree.AsEnumerable(reverse, 0))
            {
                if (skip > 0 && multivalueContainer is ICountableContainer countable)
                {
                    if (skip >= countable.LongCount)
                    { // enumerate by skipping entire containers
                        skip -= countable.LongCount;
                        continue;
                    }
                    else
                    { // skip all that we have left to skip, then enumerate
                        foreach (T t in multivalueContainer.AsEnumerable(reverse, skip))
                        {
                            yield return t;
                        }
                        skip = 0;
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
            return AsEnumerable(reverse, skip).GetEnumerator();
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

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, IComparer<T> comparer) => InsertOrReplace(item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any, comparer);

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

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var node = GetNodeForValue(item, whichOne, comparer, true);
            if (node == null)
            {
                return InsertInitialNode(item, comparer);
            }
            var multivalueContainer = GetMultivalueContainer(node);
            var result = multivalueContainer.InsertOrReplace(item, whichOne, comparer);
            if (InteriorContainerFactory.RequiresSplitting(multivalueContainer))
            {
                IMultivalueContainer<T> splitOff = (IMultivalueContainer<T>)multivalueContainer.SplitOff(comparer);
                UnderlyingTree.InsertOrReplace(splitOff, AllowDuplicates ? MultivalueLocationOptions.InsertBeforeFirst : MultivalueLocationOptions.Any, GetInteriorCollectionsComparer(comparer)); // note: a duplicate here would be a duplicate of the entire inner node, meaning that all items are the same according to the comparer. But they may not always be exactly identical, if the comparer is a key-only comparer. We always split off the left in our multivalue containers, so this ensures consistency.
            }
            return result;
        }

        private (IContainerLocation location, bool insertedNotReplaced) InsertInitialNode(T item, IComparer<T> comparer)
        {
            IMultivalueContainer<T> initialContainer = (IMultivalueContainer<T>)InteriorContainerFactory.CreateValueContainer();
            if (initialContainer.AllowDuplicates != AllowDuplicates)
                throw new Exception("AllowDuplicates must be same for interior container.");
            initialContainer.InsertOrReplace(item, comparer);
            var result = UnderlyingTree.InsertOrReplace(initialContainer, GetInteriorCollectionsComparer(comparer));
            return result;
        }

        public bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var node = GetNodeForValue(item, whichOne, comparer, false);
            if (node == null)
                return false;
            var multivalueContainer = GetMultivalueContainer(node);
            bool result = multivalueContainer.TryRemove(item, whichOne, comparer);
            if (result && multivalueContainer.Any() == false)
            {
                // Remove the node, since nothing is left in it.
                UnderlyingTree.RemoveAtIndex(node.Index);
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
                    node = (AvlCountedNode<IMultivalueContainer<T>>)node.GetNextNode();
                    keepGoing = node != null;
                }
            }
            return count;
        }

        public IValueContainer<T> SplitOff(IComparer<T> comparer)
        {
            var splitOffUnderlying = (AvlIndexableTree<IMultivalueContainer<T>>)UnderlyingTree.SplitOff(GetInteriorCollectionsComparer(comparer));
            var splitOff = (AvlListTree<T>)CreateNewWithSameSettings();
            splitOff.UnderlyingTree = splitOffUnderlying;
            return splitOff;
        }

        public (IContainerLocation location, bool found) FindContainerLocation(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer) => UnderlyingTree.GetMatchingOrNextNode(null, whichOne, GetItemToInteriorCollectionComparer(value, comparer));
        public (IContainerLocation location, bool found) FindContainerLocation(T value, IComparer<T> comparer) => UnderlyingTree.GetMatchingOrNextNode(null, MultivalueLocationOptions.Any, GetItemToInteriorCollectionComparer(value, comparer));

    }

}