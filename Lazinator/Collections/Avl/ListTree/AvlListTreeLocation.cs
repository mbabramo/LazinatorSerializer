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
        public readonly AvlCountedNode<IMultivalueContainer<T>> OuterNode;
        public readonly IContainerLocation InnerLocation;

        public AvlListTreeLocation(AvlCountedNode<IMultivalueContainer<T>> outerNode, IContainerLocation innerLocation)
        {
            OuterNode = outerNode;
            InnerLocation = innerLocation;
        }

        public IContainerLocation GetNextLocation()
        {
            var nextInner = InnerLocation.GetNextLocation();
            if (nextInner != null)
                return new AvlListTreeLocation<T>(OuterNode, nextInner);
            var nextOuter = (AvlCountedNode<IMultivalueContainer<T>>) OuterNode.GetNextNode();
            if (nextOuter == null)
                return null;
            return new AvlListTreeLocation<T>(nextOuter, nextOuter.Value.FirstLocation());
        }

        public IContainerLocation GetPreviousLocation()
        {
            var nextInner = InnerLocation.GetPreviousLocation();
            if (nextInner != null)
                return new AvlListTreeLocation<T>(OuterNode, nextInner);
            var nextOuter = (AvlCountedNode<IMultivalueContainer<T>>)OuterNode.GetPreviousNode();
            if (nextOuter == null)
                return null;
            return new AvlListTreeLocation<T>(nextOuter, nextOuter.Value.LastLocation());
        }
    }
}
