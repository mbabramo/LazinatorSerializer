using Lazinator.Buffers;
using LazinatorAvlCollections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.ListTree
{
    /// <summary>
    /// A means of referencing an item within an Avl list tree, specifying the location of the inner container
    /// in the outer Avl list tree as well as the location in the inner container, while also providing references
    /// to both the outer underlying tree and the inner container.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct AvlListTreeLocation<T> : IContainerLocation where T : ILazinator
    {
        public readonly IMultivalueContainer<IMultivalueContainer<T>> UnderlyingTree;
        public readonly IContainerLocation LocationOfInnerContainer;
        public readonly IMultivalueContainer<T> InnerContainer;
        public readonly IContainerLocation LocationInInnerContainer;

        public AvlListTreeLocation(IMultivalueContainer<IMultivalueContainer<T>> outerContainer, IContainerLocation locationOfInnerContainer, IMultivalueContainer<T> innerContainer, IContainerLocation locationInInnerContainer)
        {
            UnderlyingTree = outerContainer;
            LocationOfInnerContainer = locationOfInnerContainer;
            InnerContainer = innerContainer;
            LocationInInnerContainer = locationInInnerContainer;
        }

        public bool IsBeforeContainer => InnerContainer == null;

        public bool IsAfterContainer => (LocationOfInnerContainer == null || LocationOfInnerContainer.IsAfterContainer ||  LocationInInnerContainer.IsAfterContainer);

        public IContainerLocation GetNextLocation()
        {
            var nextInner = LocationInInnerContainer.GetNextLocation();
            if (!nextInner.IsAfterContainer)
                return new AvlListTreeLocation<T>(UnderlyingTree, LocationOfInnerContainer, InnerContainer, nextInner);
            var nextOuterLocation = LocationOfInnerContainer.GetNextLocation();
            if (nextOuterLocation.IsAfterContainer)
                return new AfterContainerLocation();
            var nextInnerContainer = UnderlyingTree.GetAt(nextOuterLocation);
            var nextInnerLocation = nextInnerContainer.FirstLocation();
            return new AvlListTreeLocation<T>(UnderlyingTree, nextOuterLocation, nextInnerContainer, nextInnerLocation);
        }

        public IContainerLocation GetPreviousLocation()
        {
            var previousInner = LocationInInnerContainer.GetPreviousLocation();
            if (!previousInner.IsBeforeContainer)
                return new AvlListTreeLocation<T>(UnderlyingTree, LocationOfInnerContainer, InnerContainer, previousInner);
            var previousOuterLocation = LocationOfInnerContainer.GetPreviousLocation();
            if (previousOuterLocation.IsBeforeContainer)
                return new BeforeContainerLocation();
            var previousInnerContainer = UnderlyingTree.GetAt(previousOuterLocation);
            var previousInnerLocation = previousInnerContainer.LastLocation();
            return new AvlListTreeLocation<T>(UnderlyingTree, previousOuterLocation, previousInnerContainer, previousInnerLocation);
        }
    }
}
