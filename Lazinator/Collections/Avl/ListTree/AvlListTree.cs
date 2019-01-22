using Lazinator.Buffers;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Extensions;
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
    public partial class AvlListTree<T> : IAvlListTree<T>, IValueContainer<T>, IMultivalueContainer<T> where T : ILazinator
    {

        public AvlListTree(bool allowDuplicates, bool unbalanced, ContainerFactory innerContainerFactory)
        {
            AllowDuplicates = allowDuplicates;
            Unbalanced = unbalanced;
            InnerContainerFactory = innerContainerFactory;
            CreateUnderlyingTree(innerContainerFactory);
        }

        protected virtual void CreateUnderlyingTree(ContainerFactory innerContainerFactory)
        {
            UnderlyingTree = (IMultivalueContainer<IMultivalueContainer<T>>)innerContainerFactory.CreateValueContainer<IMultivalueContainer<T>>();
        }

        public virtual IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlListTree<T>(AllowDuplicates, Unbalanced, InnerContainerFactory);
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
        /// Returns a comparer to compare an item to an inner collection. Usually, the custom comparer can only compare like objects, so null should be passed as the inner collection being compared; this custom comparer then substitutes the item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        private CustomComparer<IMultivalueContainer<T>> GetItemToInnerContainerComparer(T item, IComparer<T> comparer)
        {
            return new CustomComparer<IMultivalueContainer<T>>((a, b) =>
            {
                if (a == null)
                    return CompareBasedOnEndItems(b, item, comparer);
                return 0 - CompareBasedOnEndItems(a, item, comparer);
            });
        }

        private CustomComparer<IMultivalueContainer<T>> GetInnerContainersComparer(IComparer<T> comparer)
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


        private IMultivalueContainer<T> GetInnerContainer(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, bool chooseShorterIfInBetween) => GetInnerLocationAndContainer(item, whichOne, comparer, chooseShorterIfInBetween).container;

        private IContainerLocation GetInnerLocation(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, bool chooseShorterIfInBetween) => GetInnerLocationAndContainer(item, whichOne, comparer, chooseShorterIfInBetween).location;

        protected (IContainerLocation location, IMultivalueContainer<T> container) GetInnerLocationAndContainer(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, bool chooseShorterIfInBetween)
        {
            if (UnderlyingTree == null || !UnderlyingTree.Any())
                return default;
            // If inserting before the first or after the last, we still want the node containing the first or last.
            MultivalueLocationOptions whichOneModified = FirstOrLastFromBeforeOrAfter(whichOne);
            var matchInfo = UnderlyingTree.FindContainerLocation(null, whichOneModified, GetItemToInnerContainerComparer(item, comparer)); // Note: GetItemToInnerContainerComparer will result in comparing the item to the inner containers, so the "null" is a placeholder
            var locationOfInitialInnerContainer = matchInfo.location ?? UnderlyingTree.LastLocation();
            if (locationOfInitialInnerContainer.IsAfterContainer)
                return default;
            var initialInnerContainer = UnderlyingTree.GetAt(locationOfInitialInnerContainer);
            if (locationOfInitialInnerContainer == null || !chooseShorterIfInBetween)
                return (locationOfInitialInnerContainer, initialInnerContainer);
            return ChooseBetweenInnerContainerAndPrevious(item, comparer, locationOfInitialInnerContainer, initialInnerContainer);
        }

        private (IContainerLocation location, IMultivalueContainer<T> container) ChooseBetweenInnerContainerAndPrevious(T item, IComparer<T> comparer, IContainerLocation locationOfInitialInnerContainer, IMultivalueContainer<T> initialInnerContainer)
        {
            bool isBeforeThis = comparer.Compare(item, initialInnerContainer.First()) == -1;
            if (isBeforeThis)
            {
                IContainerLocation previousInnerContainerLocation = locationOfInitialInnerContainer.GetPreviousLocation();
                if (!previousInnerContainerLocation.IsBeforeContainer)
                {
                    IMultivalueContainer<T> previousInnerContainer = UnderlyingTree.GetAt(previousInnerContainerLocation);
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
            return new AvlListTreeLocation<T>(outerContainer, locationOfInnerContainer, innerContainer, locationInInnerContainer);
        }

        public IContainerLocation LastLocation()
        {
            if (UnderlyingTree == null || !UnderlyingTree.Any())
                return null;
            var outerContainer = UnderlyingTree;
            var locationOfInnerContainer = UnderlyingTree.LastLocation();
            var innerContainer = UnderlyingTree.GetAt(locationOfInnerContainer);
            var locationInInnerContainer = innerContainer.LastLocation();
            return new AvlListTreeLocation<T>(outerContainer, locationOfInnerContainer, innerContainer, locationInInnerContainer);
        }

        public T GetAt(IContainerLocation location)
        {
            AvlListTreeLocation<T> avlLocation = (AvlListTreeLocation<T>)location;
            return avlLocation.InnerContainer.GetAt(avlLocation.LocationInInnerContainer);
        }

        public void SetAt(IContainerLocation location, T value)
        {
            AvlListTreeLocation<T> avlLocation = (AvlListTreeLocation<T>)location;
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
            if (location.IsAfterContainer)
            {
                if (UnderlyingTree == null || !UnderlyingTree.Any())
                {
                    InsertInitialNode(item, Comparer<T>.Default);
                    return;
                }
                var innerContainer = UnderlyingTree.Last();
                innerContainer.InsertAt(new AfterContainerLocation(), item);
                return;
            }
            AvlListTreeLocation<T> listTreeLocation = (AvlListTreeLocation<T>)(location);
            listTreeLocation.InnerContainer.InsertAt(listTreeLocation.LocationInInnerContainer, item);
            if (InnerContainerFactory.ShouldSplit(listTreeLocation.InnerContainer))
            {
                IMultivalueContainer<T> splitOff = (IMultivalueContainer<T>) listTreeLocation.InnerContainer.SplitOff();
                UnderlyingTree.InsertAt(listTreeLocation.LocationOfInnerContainer, splitOff);
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
            if (InnerContainerFactory.ShouldSplit(innerContainer))
            {
                IMultivalueContainer<T> splitOff = (IMultivalueContainer<T>)innerContainer.SplitOff();
                UnderlyingTree.InsertAt(innerContainerLocation, splitOff);
                // The splitting has changed the location, so we need to find the item, using the same comparer, but we modify the location if we were inserting before or after. Note that if we were inserting at ANY location, this could return a different result.
                var revisedLocation = FindContainerLocation(item, FirstOrLastFromBeforeOrAfter(whichOne), comparer);
                return (revisedLocation.location, resultWithinContainer.insertedNotReplaced);
            }
            else
                return (new AvlListTreeLocation<T>(UnderlyingTree, innerContainerLocation, innerContainer, resultWithinContainer.location), resultWithinContainer.insertedNotReplaced);
        }

        private (IContainerLocation location, bool insertedNotReplaced) InsertInitialNode(T item, IComparer<T> comparer)
        {
            IMultivalueContainer<T> onlyInnerContainer = (IMultivalueContainer<T>)InnerContainerFactory.CreateValueContainer<T>();
            if (onlyInnerContainer.AllowDuplicates != AllowDuplicates)
                throw new Exception("AllowDuplicates must be same for inner container.");
            onlyInnerContainer.InsertOrReplace(item, comparer);
            var resultWithinContainer = UnderlyingTree.InsertOrReplace(onlyInnerContainer, GetInnerContainersComparer(comparer));
            return (new AvlListTreeLocation<T>(UnderlyingTree, UnderlyingTree.FirstLocation(), onlyInnerContainer, onlyInnerContainer.FirstLocation()), resultWithinContainer.insertedNotReplaced);
        }

        public void RemoveAt(IContainerLocation location)
        {
            AvlListTreeLocation<T> listTreeLocation = (AvlListTreeLocation<T>)location;
            listTreeLocation.InnerContainer.RemoveAt(listTreeLocation.LocationInInnerContainer);
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
            if (result && !innerContainer.Any())
            {
                // Remove the node, since nothing is left in it.
                UnderlyingTree.RemoveAt(innerContainerLocation);
            }
            return result;
        }

        public bool TryRemoveAll(T item, IComparer<T> comparer)
        {
            var innerContainer = GetInnerContainer(item, MultivalueLocationOptions.Any, comparer, false);
            bool any = innerContainer.TryRemove(item, comparer);
            if (any)
            {
                do
                {
                } while (innerContainer.TryRemove(item, comparer));
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
            return UnderlyingTree.IsShorterThan(((AvlListTree<T>)second).UnderlyingTree);
        }

        public IValueContainer<T> SplitOff()
        {
            var splitOffUnderlying = UnderlyingTree.SplitOff();
            var splitOff = (AvlListTree<T>)CreateNewWithSameSettings();
            splitOff.UnderlyingTree = (IMultivalueContainer<IMultivalueContainer<T>>) splitOffUnderlying;
            return splitOff;
        }

        public (IContainerLocation location, bool found) FindContainerLocation(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var matchInfo = UnderlyingTree.FindContainerLocation(null, whichOne, GetItemToInnerContainerComparer(value, comparer)); // Note: GetItemToInnerContainerComparer will result in comparing the item to the inner containers, so the "null" is a placeholder
            if (matchInfo.location.IsAfterContainer)
                return (matchInfo.location, false);
            var innerContainer = UnderlyingTree.GetAt(matchInfo.location);
            var innerContainerResult = innerContainer.FindContainerLocation(value, whichOne, comparer);
            return (new AvlListTreeLocation<T>(UnderlyingTree, matchInfo.location, innerContainer, innerContainerResult.location), innerContainerResult.found);
        }
        public (IContainerLocation location, bool found) FindContainerLocation(T value, IComparer<T> comparer) => FindContainerLocation(value, MultivalueLocationOptions.Any, comparer);

        public bool Contains(T item, IComparer<T> comparer)
        {
            var result = FindContainerLocation(item, comparer);
            return result.found;
        }

    }

}