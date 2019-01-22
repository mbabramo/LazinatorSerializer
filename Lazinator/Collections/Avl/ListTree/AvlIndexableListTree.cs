using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ListTree
{
    public partial class AvlIndexableListTree<T> : AvlListTree<T>, IAvlIndexableListTree<T>, IIndexableMultivalueContainer<T> where T : ILazinator
    {
        IAggregatedMultivalueContainer<IIndexableMultivalueContainer<T>> UnderlyingAggregatedContainer => (IAggregatedMultivalueContainer<IIndexableMultivalueContainer<T>>)UnderlyingTree;

        public long LongCount => UnderlyingAggregatedContainer.LongAggregatedCount;

        public T GetAtIndex(long index)
        {
            (IIndexableMultivalueContainer<T> innerContainer, long indexInInnerContainer) = GetInnerContainerAndIndexWithin(index);
            return innerContainer.GetAtIndex(indexInInnerContainer);
        }

        public void InsertAtIndex(long index, T item)
        {
            if (index == LongCount)
                InsertAt(new AfterContainerLocation(), item);
            else
            {
                (IIndexableMultivalueContainer<T> innerContainer, long indexInInnerContainer) = GetInnerContainerAndIndexWithin(index);
                innerContainer.InsertAtIndex(indexInInnerContainer, item);
            }
        }

        public void RemoveAt(long index)
        {
            (IIndexableMultivalueContainer<T> innerContainer, long indexInInnerContainer) = GetInnerContainerAndIndexWithin(index);
            innerContainer.RemoveAt(indexInInnerContainer);
        }

        public void SetAtIndex(long index, T value)
        {
            (IIndexableMultivalueContainer<T> innerContainer, long indexInInnerContainer) = GetInnerContainerAndIndexWithin(index);
            innerContainer.SetAtIndex(indexInInnerContainer, value);
        }

        public (long index, bool exists) FindIndex(T target, IComparer<T> comparer) => FindIndex(target, MultivalueLocationOptions.Any, comparer);
        public (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var outerResult = GetInnerLocationAndIndexedContainer(target, whichOne, comparer, false);
            var locationOfInnerContainer = outerResult.location.LocationOfInnerContainer;
            (long firstAggregatedIndex, long lastAggregatedIndex) = UnderlyingAggregatedContainer.GetAggregatedIndexRange(locationOfInnerContainer);
            var innerResult = outerResult.container.FindIndex(target, comparer);
            return (firstAggregatedIndex + innerResult.index, innerResult.exists);
        }

        private (IIndexableMultivalueContainer<T> innerContainer, long indexInInnerContainer) GetInnerContainerAndIndexWithin(long index)
        {
            var indexInfo = UnderlyingAggregatedContainer.GetAggregatedIndexInfo(index);
            var innerContainer = UnderlyingAggregatedContainer.GetAtIndex(indexInfo.nonaggregatedIndex);
            var indexInInnerContainer = index - indexInfo.firstAggregatedIndex;
            return (innerContainer, indexInInnerContainer);
        }

        protected internal (AvlListTreeLocation<T> location, IIndexableMultivalueContainer<T> container) GetInnerLocationAndIndexedContainer(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, bool chooseShorterIfInBetween)
        {
            (IContainerLocation location, IMultivalueContainer<T> container) = GetInnerLocationAndContainer(item, whichOne, comparer, chooseShorterIfInBetween);
            var listTreeLocation = (AvlListTreeLocation<T>)location;
            var indexedContainer = (IIndexableMultivalueContainer<T>)container;
            return (listTreeLocation, indexedContainer);
        }
    }
}
