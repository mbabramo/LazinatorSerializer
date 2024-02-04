using Lazinator.Buffers;
using LazinatorAvlCollections.Avl.ValueTree;
using Lazinator.Collections.Extensions;
using LazinatorAvlCollections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using LazinatorAvlCollections.Avl.BinaryTree;
using Lazinator.Core;
using Lazinator.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;
using System.Diagnostics;

namespace LazinatorAvlCollections.Avl.ListTree
{
    /// <summary>
    /// An Avl list tree (an Avl tree of lists) that uses an Avl aggregated tree as the underlying outer tree to allow for overall indexing of items within the tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class AvlIndexableListTree<T> : IAvlIndexableListTree<T>, IIndexableMultivalueContainer<T>, IMultilevelReportReceiver, ITreeString where T : ILazinator
    {
        #region Construction

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
            UnderlyingTree.MultilevelReporterParent = this;
        }

        public virtual IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlIndexableListTree<T>(AllowDuplicates, Unbalanced, InnerContainerFactory);
        }

        protected virtual IIndexableMultivalueContainer<T> CreateInnerContainer()
        {
            var result = (IIndexableMultivalueContainer<T>)InnerContainerFactory.CreateValueContainer<T>();
            ((IMultilevelReporter)result).MultilevelReporterParent = UnderlyingTree;
            return result;
        }

        #endregion

        #region Find items and locations

        public (long index, bool exists) FindIndex(T target, IComparer<T> comparer) => FindIndex(target, MultivalueLocationOptions.Any, comparer);
        public (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var outerResult = GetInnerLocationAndContainer(target, whichOne, comparer, false);
            if (outerResult.location.IsAfterContainer)
                return (LongCount, false);
            AvlAggregatedNode<IIndexableMultivalueContainer<T>> aggregatedNode = GetAggregatedNodeAtLocation(outerResult.location);
            var innerResult = outerResult.container.FindIndex(target, whichOne, comparer);
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

        private AvlIndexableListTreeLocation<T> GetAvlIndexableListTreeLocation(IContainerLocation innerContainerLocation)
        {
            switch (innerContainerLocation)
            {
                case AvlIndexableListTreeLocation<T> listTreeLocation:
                    return listTreeLocation;
                case IndexLocation indexLocation:
                    var result = GetInnerContainerNodeAndIndexWithin(indexLocation.Index);
                    return new AvlIndexableListTreeLocation<T>(UnderlyingTree, result.innerContainerNode, new IndexLocation(result.indexInInnerContainer, LongCount));
                case TreeLocation<IIndexableMultivalueContainer<T>, AvlAggregatedNode<IIndexableMultivalueContainer<T>>> binaryTreeLocation:
                    AvlAggregatedNode<IIndexableMultivalueContainer<T>> avlAggregatedNode = GetAggregatedNodeAtLocation(binaryTreeLocation);
                    return new AvlIndexableListTreeLocation<T>(UnderlyingTree, avlAggregatedNode, avlAggregatedNode.Value.FirstLocation());
                default:
                    throw new NotImplementedException();
            }
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
                case TreeLocation<IIndexableMultivalueContainer<T>, AvlAggregatedNode<IIndexableMultivalueContainer<T>>> binaryTreeLocation:
                    aggregatedNode = (AvlAggregatedNode<IIndexableMultivalueContainer<T>>)binaryTreeLocation.Node;
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
            MultivalueLocationOptions whichOneModified = FirstOrLastFromBeforeFirstOrAfterLast(whichOne);
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

        #endregion

        #region First and last

        private static MultivalueLocationOptions FirstOrLastFromBeforeFirstOrAfterLast(MultivalueLocationOptions whichOne)
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
            return InnermostFirst;
        }

        public T FirstOrDefault()
        {
            if (!Any())
                return default;
            return InnermostFirst;
        }

        public T Last()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return InnermostLast;
        }

        public T LastOrDefault()
        {
            if (!Any())
                return default;
            return InnermostLast;
        }

        public IContainerLocation FirstLocation()
        {
            if (UnderlyingTree == null || !UnderlyingTree.Any())
                return null;
            var outerContainer = UnderlyingTree;
            var locationOfInnerContainer = UnderlyingTree.FirstLocation();
            var innerContainer = UnderlyingTree.GetAt(locationOfInnerContainer);
            var locationInInnerContainer = innerContainer.FirstLocation();
            return new AvlIndexableListTreeLocation<T>(outerContainer, GetAggregatedNodeAtLocation(locationOfInnerContainer), locationInInnerContainer);
        }

        public IContainerLocation LastLocation()
        {
            if (UnderlyingTree == null || !UnderlyingTree.Any())
                return null;
            var outerContainer = UnderlyingTree;
            var locationOfInnerContainer = UnderlyingTree.LastLocation();
            var innerContainer = UnderlyingTree.GetAt(locationOfInnerContainer);
            var locationInInnerContainer = innerContainer.LastLocation();
            return new AvlIndexableListTreeLocation<T>(outerContainer, GetAggregatedNodeAtLocation(locationOfInnerContainer), locationInInnerContainer);
        }


        private void ConsiderUpdateInnermostEnds(bool firstChanged, bool lastChanged)
        {
            if (firstChanged)
            {
                AvlAggregatedNode<IIndexableMultivalueContainer<T>> firstNode = UnderlyingTree.FirstNode();
                if (firstNode != null)
                {
                    InnermostFirst = firstNode.Value.First();
                    MultilevelReporterParent?.EndItemChanged(true, InnermostFirst, this);
                }
            }
            if (lastChanged)
            {
                AvlAggregatedNode<IIndexableMultivalueContainer<T>> lastNode = UnderlyingTree.LastNode();
                if (lastNode != null)
                {
                    InnermostLast = lastNode.Value.Last();
                    MultilevelReporterParent?.EndItemChanged(true, InnermostFirst, this);
                }
            }
        }

        private void ChangeMultilevelReporterParent(AvlAggregatedNode<IIndexableMultivalueContainer<T>> node, IMultilevelReportReceiver reportTo)
        {
            ((IMultilevelReporter)node.Value).MultilevelReporterParent = reportTo;
            if (node.LeftBackingField is AvlAggregatedNode<IIndexableMultivalueContainer<T>> left)
                ChangeMultilevelReporterParent(left, reportTo);
            if (node.RightBackingField is AvlAggregatedNode<IIndexableMultivalueContainer<T>> right)
                ChangeMultilevelReporterParent(right, reportTo);
        }

        public IMultilevelReportReceiver MultilevelReporterParent { get; set; }

        protected void ReportFirstChanged()
        {
            MultilevelReporterParent?.EndItemChanged(true, First(), this);
        }

        protected void ReportLastChanged()
        {
            MultilevelReporterParent?.EndItemChanged(false, Last(), this);
        }

        public void EndItemChanged(bool isFirstItem, ILazinator revisedValue, IMultilevelReporter reporter)
        {
            ReactToEndItemChanged(isFirstItem, revisedValue);
            MultilevelReporterParent?.EndItemChanged(isFirstItem, revisedValue, this);
        }

        public void EndItemRemoved(bool wasFirstItem, IMultilevelReporter reporter)
        {
            ReactToEndItemRemoved(wasFirstItem);
            MultilevelReporterParent?.EndItemRemoved(wasFirstItem, this);
        }

        protected virtual void ReactToEndItemChanged(bool isFirstItem, ILazinator revisedValue)
        {
            T t = (T)revisedValue.CloneNoBuffer();
            if (isFirstItem)
                InnermostFirst = t;
            else
                InnermostLast = t;
        }

        protected virtual void ReactToEndItemRemoved(bool wasFirstItem)
        {
            if (wasFirstItem)
            {
                bool anyLeft = false;
                var node = UnderlyingTree.FirstNode();
                while (node != null && node.Value.Any() == false)
                {
                    node = node.GetNextNode<IIndexableMultivalueContainer<T>, AvlAggregatedNode<IIndexableMultivalueContainer<T>>>();
                    if (node != null)
                        anyLeft = true;
                }
                if (node != null)
                    InnermostFirst = node.Value.First().CloneNoBuffer();
                else if (!anyLeft)
                    MultilevelReporterParent?.EndItemRemoved(true, this);
            }
            else
            {
                bool anyLeft = false;
                var node = UnderlyingTree.LastNode();
                while (node != null && node.Value.Any() == false)
                {
                    node = node.GetPreviousNode<IIndexableMultivalueContainer<T>, AvlAggregatedNode<IIndexableMultivalueContainer<T>>>();
                    if (node != null)
                        anyLeft = true;
                }
                if (node != null)
                    InnermostLast = node.Value.Last().CloneNoBuffer();
                else if (!anyLeft)
                    MultilevelReporterParent?.EndItemRemoved(false, this);
            }
        }

        #endregion

        #region Item access

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

        public bool GetValue(T item, IComparer<T> comparer, out T match) => GetValue(item, MultivalueLocationOptions.Any, comparer, out match);

        public T GetAt(IContainerLocation location)
        {
            AvlIndexableListTreeLocation<T> avlLocation = GetAvlIndexableListTreeLocation(location);
            return avlLocation.InnerContainer.GetAt(avlLocation.LocationInInnerContainer);
        }

        public void SetAt(IContainerLocation location, T value)
        {
            AvlIndexableListTreeLocation<T> avlLocation = GetAvlIndexableListTreeLocation(location);
            avlLocation.InnerContainer.SetAt(avlLocation.LocationInInnerContainer, value);
        }

        public void Clear()
        {
            UnderlyingTree.Clear();
        }

        public long LongCount => UnderlyingTree.LongAggregatedCount;

        public T GetAtIndex(long index)
        {
            if (index == 0)
                return First();
            else if (index == LongCount - 1)
                return Last();
            else
            {
                var location = GetAvlLocationForIndex(index);
                return location.InnerContainer.GetAt(location.LocationInInnerContainer);
            }
        }

        public void InsertAtIndex(long index, T item)
        {
            IContainerLocation location = GetLocationForIndex(index);
            InsertAt(location, item);
        }

        public void SetAtIndex(long index, T value)
        {
            var location = GetAvlLocationForIndex(index);
            location.InnerContainer.SetAt(location.LocationInInnerContainer, value);
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

        #endregion

        #region Enumeration

        public IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            if (LongCount == 0 || skip >= LongCount)
                yield break;
            long startIndex = reverse ? LongCount - skip - 1 : skip;
            var node = UnderlyingTree.GetNodeAtAggregatedIndex(startIndex);
            long skipWithinNode = reverse ? node.LastAggregatedIndex - startIndex : startIndex - node.FirstAggregatedIndex;
            while (node != null)
            {
                foreach (T t in node.Value.AsEnumerable(reverse, skipWithinNode))
                {
                    if (skip > 0)
                        skip--;
                    else
                        yield return t;
                }
                skipWithinNode = 0;
                node = (AvlAggregatedNode<IIndexableMultivalueContainer<T>>)(reverse ? node.GetPreviousNode<IIndexableMultivalueContainer<T>, AvlAggregatedNode<IIndexableMultivalueContainer<T>>>() : node.GetNextNode<IIndexableMultivalueContainer<T>, AvlAggregatedNode<IIndexableMultivalueContainer<T>>>());
            }
        }

        public IEnumerable<T> AsEnumerable(bool reverse, T startValue, IComparer<T> comparer) => this.MultivalueAsEnumerable<AvlIndexableListTree<T>, T>(reverse, startValue, comparer);

        public IEnumerator<T> GetEnumerator(bool reverse, T startValue, IComparer<T> comparer) => this.MultivalueAsEnumerable<AvlIndexableListTree<T>, T>(reverse, startValue, comparer).GetEnumerator();

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


        #endregion

        #region Insertion

        public void InsertAt(IContainerLocation location, T item)
        {
            if (UnderlyingTree == null || UnderlyingTree.Any() == false)
            {
                InsertInitialNode(item, Comparer<T>.Default);
                InnermostFirst = item.CloneNoBuffer();
                InnermostLast = item.CloneNoBuffer();
                return;
            }
            IIndexableMultivalueContainer<T> innerContainer;
            AvlAggregatedNode<IIndexableMultivalueContainer<T>> innerNode;
            if (location.IsAfterContainer)
            {
                innerContainer = UnderlyingTree.Last();
                innerContainer.InsertAt(new AfterContainerLocation(), item);
                innerNode = UnderlyingTree.LastNode();
            }
            else
            {
                var listTreeLocation = GetAvlIndexableListTreeLocation(location);
                innerContainer = listTreeLocation.InnerContainer;
                innerNode = listTreeLocation.InnerContainerNode;
                innerContainer.InsertAt(listTreeLocation.LocationInInnerContainer, item);
            }
            innerNode = (AvlAggregatedNode<IIndexableMultivalueContainer<T>>) UnderlyingTree.ReloadNodeWithUpdatedIndex(innerNode);
            innerNode.UpdateFollowingNodeChange();
            if (InnerContainerFactory.ShouldSplit(innerContainer))
            {
                SplitInnerContainer(innerContainer, innerNode);
            }
        }

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, IComparer<T> comparer) => InsertOrReplace(item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any, comparer);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            (var locationOfInnerContainer, var innerContainer) = GetInnerLocationAndContainer(item, whichOne, comparer, true);
            if (innerContainer == null)
            {
                if (UnderlyingTree.Any())
                {
                    locationOfInnerContainer = UnderlyingTree.LastLocation();
                    innerContainer = UnderlyingTree.Last();
                }
                else
                    return InsertInitialNode(item, comparer);
            }
            var resultWithinContainer = innerContainer.InsertOrReplace(item, whichOne, comparer);
            UpdateAfterChange(locationOfInnerContainer);
            if (InnerContainerFactory.ShouldSplit(innerContainer))
            {
                SplitInnerContainer(innerContainer, GetAggregatedNodeAtLocation(locationOfInnerContainer));
                // The splitting has changed the location, so we need to find the item, using the same comparer, but we modify the location if we were inserting before or after. Note that if we were inserting at ANY location, this could return a different result.
                var revisedLocation = FindContainerLocation(item, FirstOrLastFromBeforeFirstOrAfterLast(whichOne), comparer);
                return (revisedLocation.location, resultWithinContainer.insertedNotReplaced);
            }
            else
                return (new AvlIndexableListTreeLocation<T>(UnderlyingTree, GetAggregatedNodeAtLocation(locationOfInnerContainer), resultWithinContainer.location), resultWithinContainer.insertedNotReplaced);
        }

        private (IContainerLocation location, bool insertedNotReplaced) InsertInitialNode(T item, IComparer<T> comparer)
        {
            IIndexableMultivalueContainer<T> onlyInnerContainer = CreateInnerContainer();
            if (onlyInnerContainer.AllowDuplicates != AllowDuplicates)
                throw new Exception("AllowDuplicates must be same for inner container.");
            var resultWithinContainer = UnderlyingTree.InsertOrReplace(onlyInnerContainer, ListTreeComparisons<T>.GetInnerContainersComparer(comparer));
            onlyInnerContainer.InsertOrReplace(item, comparer);
            UnderlyingTree.FirstNode().UpdateFollowingNodeChange();
            return (new AvlIndexableListTreeLocation<T>(UnderlyingTree, GetAggregatedNodeAtLocation(UnderlyingTree.FirstLocation()), onlyInnerContainer.FirstLocation()), resultWithinContainer.insertedNotReplaced);
        }

        #endregion

        #region Splitting

        public bool ShouldSplit(int splitThreshold)
        {
            return UnderlyingTree.ShouldSplit(splitThreshold);
        }

        public bool IsShorterThan(IValueContainer<T> second)
        {
            return UnderlyingTree.IsShorterThan(((AvlIndexableListTree<T>)second).UnderlyingTree);
        }

        private void SplitInnerContainer(IIndexableMultivalueContainer<T> innerContainer, AvlAggregatedNode<IIndexableMultivalueContainer<T>> innerNode)
        {
            var innerLocation = new TreeLocation<IIndexableMultivalueContainer<T>, AvlAggregatedNode<IIndexableMultivalueContainer<T>>>(innerNode);
            bool isFirst = innerLocation.GetPreviousLocation().IsBeforeContainer;
            IIndexableMultivalueContainer<T> splitOff = (IIndexableMultivalueContainer<T>)innerContainer.SplitOff();
            ((IMultilevelReporter)splitOff).MultilevelReporterParent = UnderlyingTree;
            if (splitOff is AvlIndexableListTree<T> t)
            {
                t.UnderlyingTree.MultilevelReporterParent = t;
                ChangeMultilevelReporterParent((AvlAggregatedNode<IIndexableMultivalueContainer<T>>)t.UnderlyingTree.Root, t.UnderlyingTree);
            }
            UnderlyingTree.InsertAt(innerLocation, splitOff);
            ConsiderUpdateInnermostEnds(isFirst, false);
        }

        public IValueContainer<T> SplitOff()
        {
            var splitOffUnderlying = UnderlyingTree.SplitOff();
            InnermostFirst = UnderlyingTree.FirstNode().Value.First().CloneNoBuffer();
            InnermostLast = UnderlyingTree.LastNode().Value.Last().CloneNoBuffer();
            var splitOff = (AvlIndexableListTree<T>)CreateNewWithSameSettings();
            splitOff.UnderlyingTree = (AvlAggregatedTree<IIndexableMultivalueContainer<T>>)splitOffUnderlying;
            splitOff.UnderlyingTree.MultilevelReporterParent = this;
            splitOff.InnermostFirst = splitOff.UnderlyingTree.FirstNode().Value.First().CloneNoBuffer();
            splitOff.InnermostLast = splitOff.UnderlyingTree.LastNode().Value.Last().CloneNoBuffer();
            return splitOff;
        }

        #endregion

        #region Removal

        public bool TryRemove(T item, IComparer<T> comparer) => TryRemove(item, MultivalueLocationOptions.Any, comparer);

        public void RemoveAt(long index)
        {
            var location = GetAvlLocationForIndex(index);
            location.InnerContainer.RemoveAt(location.LocationInInnerContainer);
            location.InnerContainerNode.UpdateFollowingNodeChange();
            if (!location.InnerContainer.Any())
                UnderlyingTree.RemoveAt(location.LocationOfInnerContainer);
        }

        public void RemoveAt(IContainerLocation location)
        {
            AvlIndexableListTreeLocation<T> avlLocation = GetAvlIndexableListTreeLocation(location);
            avlLocation.InnerContainer.RemoveAt(avlLocation.LocationInInnerContainer);
            avlLocation.InnerContainerNode.UpdateFollowingNodeChange();
            if (avlLocation.InnerContainer.Any() == false)
            {
                RemoveEmptyNode(avlLocation.LocationOfInnerContainer);
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
                RemoveEmptyNode(innerContainerLocation);
            }
            return result;
        }

        private void RemoveEmptyNode(IContainerLocation innerContainerLocation)
        {
            bool isFirst = innerContainerLocation.GetPreviousLocation().IsBeforeContainer;
            bool isLast = innerContainerLocation.GetNextLocation().IsAfterContainer;
            // Remove the node, since nothing is left in it.
            UnderlyingTree.RemoveAt(innerContainerLocation);
            if (Any())
            {
                ConsiderUpdateInnermostEnds(isFirst, isLast);
            }
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

        #endregion

        #region Tree string

        private int CountTreeLevel()
        {
            int level = 0;
            IMultilevelReporter r = this;
            while (r.MultilevelReporterParent != null)
            {
                r = r.MultilevelReporterParent;
                if (r is AvlIndexableListTree<T>)
                    level++;
            }
            return level;
        }

        public string ToTreeString()
        {
            string prelim = Any() ? $"[{InnermostFirst}-{InnermostLast}]" : "[Empty]";
            string treeString = UnderlyingTree.ToTreeString();
            return $"Tree (Level {CountTreeLevel()}) (Aggregated count: {LongCount}): {prelim}\n{treeString}";
        }

        #endregion
    }
}
