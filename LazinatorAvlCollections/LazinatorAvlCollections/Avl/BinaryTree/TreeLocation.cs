﻿using Lazinator.Collections.Location;
using Lazinator.Core;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    public struct TreeLocation<T, N> : IContainerLocation where T : ILazinator where N : class, ILazinator, INode<T, N>, new()
    {
        public readonly N Node;
        public readonly bool IsBefore;

        public TreeLocation(bool isBeforeCollection)
        {
            IsBefore = isBeforeCollection;
            Node = default;
        }

        public TreeLocation(N node)
        {
            IsBefore = false;
            Node = node;
        }

        public bool IsBeforeContainer => IsBefore;
        public bool IsAfterContainer => Node == null && !IsBeforeContainer;

        public IContainerLocation GetNextLocation()
        {
            N nextNode = Node.GetNextNode<T, N>();
            if (nextNode == null)
                return new AfterContainerLocation();
            return new TreeLocation<T, N>(Node.GetNextNode<T, N>());
        }

        public IContainerLocation GetPreviousLocation()
        {
            N previousNode = Node.GetPreviousNode<T, N>();
            if (previousNode == null)
                return new BeforeContainerLocation();
            return new TreeLocation<T, N>(previousNode);
        }
    }
}
