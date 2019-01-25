using LazinatorCollections.Interfaces;
using LazinatorCollections.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Tree
{
    // DEBUG -- not in use yet. We really also need IsBeforeLocation to be part of interface to immplement this. If we do this, we should also constrain IContainerLocation to struct

    public struct BinaryTreeLocation<T> : IContainerLocation where T : ILazinator
    {
        public readonly BinaryNode<T> BinaryNode;
        public readonly bool IsBefore;

        public BinaryTreeLocation(bool isBeforeCollection)
        {
            IsBefore = isBeforeCollection;
            BinaryNode = null;
        }

        public BinaryTreeLocation(BinaryNode<T> binaryNode)
        {
            IsBefore = false;
            BinaryNode = binaryNode;
        }

        public bool IsBeforeContainer => IsBefore;
        public bool IsAfterContainer => BinaryNode == null && !IsBeforeContainer;

        public IContainerLocation GetNextLocation()
        {
            BinaryNode<T> nextNode = BinaryNode.GetNextNode();
            if (nextNode == null)
                return new AfterContainerLocation();
            return new BinaryTreeLocation<T>(BinaryNode.GetNextNode());
        }

        public IContainerLocation GetPreviousLocation()
        {
            BinaryNode<T> previousNode = BinaryNode.GetPreviousNode();
            if (previousNode == null)
                return new BeforeContainerLocation();
            return new BinaryTreeLocation<T>(previousNode);
        }
    }
}
