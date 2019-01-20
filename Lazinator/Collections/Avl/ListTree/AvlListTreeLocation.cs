﻿using Lazinator.Buffers;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ListTree
{
    public partial class AvlListTreeLocation<T> : IAvlListTreeLocation<T>, IContainerLocation where T : ILazinator
    {
        public IContainerLocation GetNextLocation()
        {
            var nextInner = InnerLocation.GetNextLocation();
            if (nextInner != null)
                return new AvlListTreeLocation<T>() { OuterNode = OuterNode, InnerLocation = nextInner };
            var nextOuter = (AvlCountedNode<IMultivalueContainer<T>>) OuterNode.GetNextNode();
            if (nextOuter == null)
                return null;
            return new AvlListTreeLocation<T>() { OuterNode = nextOuter, InnerLocation = nextOuter.Value.FirstLocation() };
        }

        public IContainerLocation GetPreviousLocation()
        {
            var nextInner = InnerLocation.GetPreviousLocation();
            if (nextInner != null)
                return new AvlListTreeLocation<T>() { OuterNode = OuterNode, InnerLocation = nextInner };
            var nextOuter = (AvlCountedNode<IMultivalueContainer<T>>)OuterNode.GetPreviousNode();
            if (nextOuter == null)
                return null;
            return new AvlListTreeLocation<T>() { OuterNode = nextOuter, InnerLocation = nextOuter.Value.LastLocation() };
        }
    }
}
