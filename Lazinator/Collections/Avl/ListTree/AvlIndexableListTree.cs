using Lazinator.Buffers;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Collections.Tree;
using Lazinator.Core;
using Lazinator.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ListTree
{
    public partial class AvlIndexableListTree<T> : IAvlIndexableListTree<T>, IIndexableMultivalueContainer<T> where T : ILazinator
    {
        public AvlIndexableListTree(bool allowDuplicates, bool unbalanced, ContainerFactory innerContainerFactory)
        {
            AllowDuplicates = allowDuplicates;
            Unbalanced = unbalanced;
            InnerContainerFactory = innerContainerFactory;
            CreateUnderlyingTree(innerContainerFactory);
        }

        protected virtual void CreateUnderlyingTree(ContainerFactory innerContainerFactory)
        {
            UnderlyingTree = new AvlAggregatedTree<IIndexableMultivalueContainer<T>>(AllowDuplicates, Unbalanced);
        }

        public virtual IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlIndexableListTree<T>(AllowDuplicates, Unbalanced, InnerContainerFactory);
        }

        public long LongCount => UnderlyingTree.LongAggregatedCount;

        public T GetAtIndex(long index)
        {
            var location = GetAvlLocationForIndex(index);
            return location.InnerContainer.GetAt(location.LocationInInnerContainer);
        }

        public void InsertAtIndex(long index, T item)
        {
            IContainerLocation location = GetLocationForIndex(index);
            InsertAt(location, item);
        }

        public void RemoveAt(long index)
        {
            var location = GetAvlLocationForIndex(index);
            location.InnerContainer.RemoveAt(location.LocationInInnerContainer);
            location.InnerContainerNode.UpdateFollowingNodeChange();
            if (!location.InnerContainer.Any())
                UnderlyingTree.RemoveAt(location.LocationOfInnerContainer);
        }

        public void SetAtIndex(long index, T value)
        {
            var location = GetAvlLocationForIndex(index);
            location.InnerContainer.SetAt(location.LocationInInnerContainer, value);
        }

        public (long index, bool exists) FindIndex(T target, IComparer<T> comparer) => FindIndex(target, MultivalueLocationOptions.Any, comparer);
        public (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var outerResult = GetInnerLocationAndContainer(target, whichOne, comparer, false);
            if (outerResult.location.IsAfterContainer)
                return (LongCount, false);
            AvlAggregatedNode<IIndexableMultivalueContainer<T>> aggregatedNode = GetAggregatedNodeAtLocation(outerResult.location);
            var innerResult = outerResult.container.FindIndex(target, comparer);
            return (aggregatedNode.FirstAggregatedIndex + innerResult.index, innerResult.exists);
        }

        private (AvlAggregatedNode<IIndexableMultivalueContainer<T>> innerContainerNode, long indexInInnerContainer) GetInnerContainerNodeAndIndexWithin(long index)
        {
            var indexInfo = UnderlyingTree.GetAggregatedIndexInfo(index);
            var innerContainerNode = UnderlyingTree.GetNodeAtNonaggregatedIndex(indexInfo.nonaggregatedIndex);
            var indexInInnerContainer = index - indexInfo.firstAggregatedIndex;
            return (innerContainerNode, indexInInnerContainer);
        }

        protected internal AvlIndexableListTreeLocation<T> GetAvlLocationForIndex(long index)
        {
            if (index >= LongCount || index < 0)
                throw new ArgumentException();
            var result = (AvlIndexableListTreeLocation<T>)GetLocationForIndex(index);
            return result;
        }

        protected internal IContainerLocation GetLocationForIndex(long index)
        {
            if (index == LongCount)
                return new AfterContainerLocation();
            var node = UnderlyingTree.GetNodeAtAggregatedIndex(index);
            AvlIndexableListTreeLocation<T> location = new AvlIndexableListTreeLocation<T>(UnderlyingTree, node, new IndexLocation(index - node.FirstAggregatedIndex, node.SelfAggregatedCount));
            return location;
        }


        private AvlAggregatedNode<IIndexableMultivalueContainer<T>> GetAggregatedNodeAtLocation(IContainerLocation innerContainerLocation)
        {
            AvlAggregatedNode<IIndexableMultivalueContainer<T>> aggregatedNode;
            switch (innerContainerLocation)
            {
                case AvlIndexableListTreeLocation<T> listTreeLocation:
                    aggregatedNode = listTreeLocation.InnerContainerNode;
                    break;
                case IndexLocation indexLocation:
                    aggregatedNode = UnderlyingTree.GetNodeAtNonaggregatedIndex(indexLocation.Index);
                    break;
                case BinaryTreeLocation<IIndexableMultivalueContainer<T>> binaryTreeLocation:
                    aggregatedNode = (AvlAggregatedNode<IIndexableMultivalueContainer<T>>)binaryTreeLocation.BinaryNode;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return aggregatedNode;
        }

        private IIndexableMultivalueContainer<T> GetInnerContainer(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, bool chooseShorterIfInBetween) => GetInnerLocationAndContainer(item, whichOne, comparer, chooseShorterIfInBetween).container;

        private IContainerLocation GetInnerLocation(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, bool chooseShorterIfInBetween) => GetInnerLocationAndContainer(item, whichOne, comparer, chooseShorterIfInBetween).location;

        protected internal (IContainerLocation location, IIndexableMultivalueContainer<T> container) GetInnerLocationAndContainer(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, bool chooseShorterIfInBetween)
        {
            if (UnderlyingTree == null || !UnderlyingTree.Any())
                return default;
            // If inserting before the first or after the last, we still want the node containing the first or last.
            MultivalueLocationOptions whichOneModified = FirstOrLastFromBeforeOrAfter(whichOne);
            var matchInfo = UnderlyingTree.FindContainerLocation(null, whichOneModified, ListTreeComparisons<T>.GetItemToInnerContainerComparer(item, comparer)); // Note: GetItemToInnerContainerComparer will result in comparing the item to the inner containers, so the "null" is a placeholder
            var locationOfInitialInnerContainer = matchInfo.location ?? UnderlyingTree.LastLocation();
            if (locationOfInitialInnerContainer.IsAfterContainer)
                return (new AfterContainerLocation(), default);
            var initialInnerContainer = UnderlyingTree.GetAt(locationOfInitialInnerContainer);
            if (locationOfInitialInnerContainer == null || !chooseShorterIfInBetween)
                return (locationOfInitialInnerContainer, initialInnerContainer);
            return ChooseBetweenInnerContainerAndPrevious(item, comparer, locationOfInitialInnerContainer, initialInnerContainer);
        }

        private (IContainerLocation location, IIndexableMultivalueContainer<T> container) ChooseBetweenInnerContainerAndPrevious(T item, IComparer<T> comparer, IContainerLocation locationOfInitialInnerContainer, IIndexableMultivalueContainer<T> initialInnerContainer)
        {
            bool isBeforeThis = comparer.Compare(item, initialInnerContainer.First()) == -1;
            if (isBeforeThis)
            {
                IContainerLocation previousInnerContainerLocation = locationOfInitialInnerContainer.GetPreviousLocation();
                if (!previousInnerContainerLocation.IsBeforeContainer)
                {
                    IIndexableMultivalueContainer<T> previousInnerContainer = UnderlyingTree.GetAt(previousInnerContainerLocation);
                    bool inBetweenThisAndPrevious = previousInnerContainer != null && comparer.Compare(item, previousInnerContainer.Last()) == 1;
                    if (inBetweenThisAndPrevious)
                    {
                        if (previousInnerContainer.IsShorterThan(initialInnerContainer))
                            return (previousInnerContainerLocation, previousInnerContainer);
                    }
                }
            }
            return (locationOfInitialInnerContainer, initialInnerContainer);
        }

        private static MultivalueLocationOptions FirstOrLastFromBeforeOrAfter(MultivalueLocationOptions whichOne)
        {
            MultivalueLocationOptions whichOneModified = whichOne;
            if (whichOne == MultivalueLocationOptions.InsertAfterLast)
                whichOneModified = MultivalueLocationOptions.Last;
            else if (whichOne == MultivalueLocationOptions.InsertBeforeFirst)
                whichOneModified = MultivalueLocationOptions.First;
            return whichOneModified;
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

        public IContainerLocation FirstLocation()
        {
            if (UnderlyingTree == null || !UnderlyingTree.Any())
                return null;
            var outerContainer = UnderlyingTree;
            var locationOfInnerContainer = UnderlyingTree.FirstLocation();
            var innerContainer = UnderlyingTree.GetAt(locationOfInnerContainer);
            var locationInInnerContainer = innerContainer.FirstLocation();
            return new AvlIndexableListTreeLocation<T>(outerContainer, GetAggregatedNodeAtLocation(locationOfInnerContainer),  locationInInnerContainer);
        }

        public IContainerLocation LastLocation()
        {
            if (UnderlyingTree == null || !UnderlyingTree.Any())
                return null;
            var outerContainer = UnderlyingTree;
            var locationOfInnerContainer = UnderlyingTree.LastLocation();
            var innerContainer = UnderlyingTree.GetAt(locationOfInnerContainer);
            var locationInInnerContainer = innerContainer.LastLocation();
            return new AvlIndexableListTreeLocation<T>(outerContainer, GetAggregatedNodeAtLocation(locationOfInnerContainer),  locationInInnerContainer);
        }

        public T GetAt(IContainerLocation location)
        {
            AvlIndexableListTreeLocation<T> avlLocation = (AvlIndexableListTreeLocation<T>)location;
            return avlLocation.InnerContainer.GetAt(avlLocation.LocationInInnerContainer);
        }

        public void SetAt(IContainerLocation location, T value)
        {
            AvlIndexableListTreeLocation<T> avlLocation = (AvlIndexableListTreeLocation<T>)location;
            avlLocation.InnerContainer.SetAt(avlLocation.LocationInInnerContainer, value);
        }

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

        public void InsertAt(IContainerLocation location, T item)
        {
            if (UnderlyingTree.Any() == false)
            {
                InsertInitialNode(item, Comparer<T>.Default);
                return;
            }
            IIndexableMultivalueContainer<T> innerContainer;
            AvlAggregatedNode<IIndexableMultivalueContainer<T>> innerNode;
            if (location.IsAfterContainer)
            {
                if (UnderlyingTree == null || !UnderlyingTree.Any())
                {
                    InsertInitialNode(item, Comparer<T>.Default);
                    return;
                }
                innerContainer = UnderlyingTree.Last();
                innerContainer.InsertAt(new AfterContainerLocation(), item);
                innerNode = UnderlyingTree.LastAggregatedNode;
            }
            else
            {
                var listTreeLocation = (AvlIndexableListTreeLocation<T>)location;
                innerContainer = listTreeLocation.InnerContainer;
                innerNode = listTreeLocation.InnerContainerNode;
                innerContainer.InsertAt(listTreeLocation.LocationInInnerContainer, item);
            }
            innerNode.UpdateFollowingNodeChange();
            if (InnerContainerFactory.ShouldSplit(innerContainer))
            {
                IIndexableMultivalueContainer<T> splitOff = (IIndexableMultivalueContainer<T>)innerContainer.SplitOff();
                IContainerLocation innerNodeLocation = new BinaryTreeLocation<IIndexableMultivalueContainer<T>>(innerNode);
                UnderlyingTree.InsertAt(innerNodeLocation, splitOff);
            }
        }

        

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, IComparer<T> comparer) => InsertOrReplace(item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any, comparer);

        public bool TryRemove(T item, IComparer<T> comparer) => TryRemove(item, MultivalueLocationOptions.Any, comparer);

        public bool GetValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, out T match)
        {
            var multivalueContainer = GetInnerContainer(item, whichOne, comparer, false);
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
            (var innerContainerLocation, var innerContainer) = GetInnerLocationAndContainer(item, whichOne, comparer, true);
            if (innerContainer == null)
            {
                return InsertInitialNode(item, comparer);
            }
            var resultWithinContainer = innerContainer.InsertOrReplace(item, whichOne, comparer);
            UpdateAfterChange(innerContainerLocation);
            if (InnerContainerFactory.ShouldSplit(innerContainer))
            {
                IIndexableMultivalueContainer<T> splitOff = (IIndexableMultivalueContainer<T>)innerContainer.SplitOff();
                UnderlyingTree.InsertAt(innerContainerLocation, splitOff);
                // The splitting has changed the location, so we need to find the item, using the same comparer, but we modify the location if we were inserting before or after. Note that if we were inserting at ANY location, this could return a different result.
                var revisedLocation = FindContainerLocation(item, FirstOrLastFromBeforeOrAfter(whichOne), comparer);

                return (revisedLocation.location, resultWithinContainer.insertedNotReplaced);
            }
            else
                return (new AvlIndexableListTreeLocation<T>(UnderlyingTree, GetAggregatedNodeAtLocation(innerContainerLocation),  resultWithinContainer.location), resultWithinContainer.insertedNotReplaced);
        }

        private (IContainerLocation location, bool insertedNotReplaced) InsertInitialNode(T item, IComparer<T> comparer)
        {
            IIndexableMultivalueContainer<T> onlyInnerContainer = CreateInnerContainer();
            if (onlyInnerContainer.AllowDuplicates != AllowDuplicates)
                throw new Exception("AllowDuplicates must be same for inner container.");
            onlyInnerContainer.InsertOrReplace(item, comparer);
            var resultWithinContainer = UnderlyingTree.InsertOrReplace(onlyInnerContainer, ListTreeComparisons<T>.GetInnerContainersComparer(comparer));
            return (new AvlIndexableListTreeLocation<T>(UnderlyingTree, GetAggregatedNodeAtLocation(UnderlyingTree.FirstLocation()), onlyInnerContainer.FirstLocation()), resultWithinContainer.insertedNotReplaced);
        }

        protected virtual IIndexableMultivalueContainer<T> CreateInnerContainer()
        {
            return (IIndexableMultivalueContainer<T>)InnerContainerFactory.CreateValueContainer<T>();
        }

        public void RemoveAt(IContainerLocation location)
        {
            AvlIndexableListTreeLocation<T> listTreeLocation = (AvlIndexableListTreeLocation<T>)location;
            listTreeLocation.InnerContainer.RemoveAt(listTreeLocation.LocationInInnerContainer);
            listTreeLocation.InnerContainerNode.UpdateFollowingNodeChange();
            if (listTreeLocation.InnerContainer.Any() == false)
            {
                UnderlyingTree.RemoveAt(listTreeLocation.LocationOfInnerContainer);
            }
        }

        public bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            (var innerContainerLocation, var innerContainer) = GetInnerLocationAndContainer(item, whichOne, comparer, true);
            if (innerContainer == null)
                return false;
            bool result = innerContainer.TryRemove(item, whichOne, comparer);
            UpdateAfterChange(innerContainerLocation);
            if (result && !innerContainer.Any())
            {
                // Remove the node, since nothing is left in it.
                UnderlyingTree.RemoveAt(innerContainerLocation);
            }
            return result;
        }

        public bool TryRemoveAll(T item, IComparer<T> comparer)
        {
            bool any = TryRemove(item, comparer);
            if (any)
            {
                do
                {
                } while (TryRemove(item, comparer));
            }
            return any;
        }

        public long Count(T item, IComparer<T> comparer)
        {
            (var innerContainerLocation, var innerContainer) = GetInnerLocationAndContainer(item, MultivalueLocationOptions.First, comparer, true);
            if (innerContainerLocation.IsBeforeContainer)
                return 0;
            // The item might appear in multiple inner containers
            long count = 0;
            while (true)
            {
                count += innerContainer.Count(item, comparer);
                if (count == 0)
                    return 0;
                else
                {
                    innerContainerLocation = innerContainerLocation.GetNextLocation();
                    if (innerContainerLocation.IsAfterContainer)
                        return count;
                    innerContainer = UnderlyingTree.GetAt(innerContainerLocation);
                }
            }
        }

        public bool ShouldSplit(long splitThreshold)
        {
            return UnderlyingTree.ShouldSplit(splitThreshold);
        }

        public bool IsShorterThan(IValueContainer<T> second)
        {
            return UnderlyingTree.IsShorterThan(((AvlIndexableListTree<T>)second).UnderlyingTree);
        }

        public IValueContainer<T> SplitOff()
        {
            var splitOffUnderlying = UnderlyingTree.SplitOff();
            var splitOff = (AvlIndexableListTree<T>)CreateNewWithSameSettings();
            splitOff.UnderlyingTree = (AvlAggregatedTree<IIndexableMultivalueContainer<T>>)splitOffUnderlying;
            return splitOff;
        }

        public (IContainerLocation location, bool found) FindContainerLocation(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var matchInfo = UnderlyingTree.FindContainerLocation(null, whichOne, ListTreeComparisons<T>.GetItemToInnerContainerComparer(value, comparer)); // Note: GetItemToInnerContainerComparer will result in comparing the item to the inner containers, so the "null" is a placeholder
            if (matchInfo.location.IsAfterContainer)
                return (matchInfo.location, false);
            var innerContainer = UnderlyingTree.GetAt(matchInfo.location);
            var innerContainerResult = innerContainer.FindContainerLocation(value, whichOne, comparer);
            return (new AvlIndexableListTreeLocation<T>(UnderlyingTree, GetAggregatedNodeAtLocation(matchInfo.location), innerContainerResult.location), innerContainerResult.found);
        }
        public (IContainerLocation location, bool found) FindContainerLocation(T value, IComparer<T> comparer) => FindContainerLocation(value, MultivalueLocationOptions.Any, comparer);

        public bool Contains(T item, IComparer<T> comparer)
        {
            var result = FindContainerLocation(item, comparer);
            return result.found;
        }


        private void UpdateAfterChange(IContainerLocation locationOfInnerContainer)
        {
            var innerContainer = UnderlyingTree.GetAt(locationOfInnerContainer);
            var node = GetAggregatedNodeAtLocation(locationOfInnerContainer);
            node.UpdateFollowingNodeChange();
        }
    }
}
