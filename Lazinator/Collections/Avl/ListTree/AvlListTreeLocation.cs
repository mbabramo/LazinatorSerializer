using Lazinator.Buffers;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ListTree
{
    public struct AvlListTreeLocation<T> : IContainerLocation where T : ILazinator
    {
        public readonly IMultivalueContainer<IMultivalueContainer<T>> OuterContainer;
        public readonly IContainerLocation LocationOfInnerContainer;
        public readonly IMultivalueContainer<T> InnerContainer;
        public readonly IContainerLocation LocationInInnerContainer;

        public AvlListTreeLocation(IMultivalueContainer<IMultivalueContainer<T>> outerContainer, IContainerLocation locationOfInnerContainer, IMultivalueContainer<T> innerContainer, IContainerLocation locationInInnerContainer)
        {
            OuterContainer = outerContainer;
            LocationOfInnerContainer = locationOfInnerContainer;
            InnerContainer = innerContainer;
            LocationInInnerContainer = locationInInnerContainer;
        }

        public IContainerLocation GetNextLocation()
        {
            var nextInner = LocationInInnerContainer.GetNextLocation();
            if (nextInner != null)
                return new AvlListTreeLocation<T>(OuterContainer, LocationOfInnerContainer, InnerContainer, nextInner);
            var nextOuterLocation = LocationOfInnerContainer.GetNextLocation();
            if (nextOuterLocation == null)
                return null;
            var nextInnerContainer = OuterContainer.GetAt(nextOuterLocation);
            var nextInnerLocation = nextInnerContainer.FirstLocation();
            return new AvlListTreeLocation<T>(OuterContainer, nextOuterLocation, nextInnerContainer, nextInnerLocation);
        }

        public IContainerLocation GetPreviousLocation()
        {
            var previousInner = LocationInInnerContainer.GetPreviousLocation();
            if (previousInner != null)
                return new AvlListTreeLocation<T>(OuterContainer, LocationOfInnerContainer, InnerContainer, previousInner);
            var previousOuterLocation = LocationOfInnerContainer.GetPreviousLocation();
            if (previousOuterLocation == null)
                return null;
            var previousInnerContainer = OuterContainer.GetAt(previousOuterLocation);
            var previousInnerLocation = previousInnerContainer.LastLocation();
            return new AvlListTreeLocation<T>(OuterContainer, previousOuterLocation, previousInnerContainer, previousInnerLocation);
        }
    }
}
