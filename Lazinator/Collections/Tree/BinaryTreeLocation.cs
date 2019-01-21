using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Tree
{
    // DEBUG -- not in use yet. We really also need IsBeforeLocation to be part of interface to immplement this. If we do this, we should also constrain IContainerLocation to struct

    public struct BinaryTreeLocation<T> : IContainerLocation where T : ILazinator
    {
        public readonly BinaryNode<T> BinaryNode;

        public BinaryTreeLocation(BinaryNode<T> binaryNode)
        {
            BinaryNode = binaryNode;
        }

        public bool IsAfterCollection => BinaryNode != null;

        public IContainerLocation GetNextLocation() => new BinaryTreeLocation<T>(BinaryNode.GetNextNode());

        public IContainerLocation GetPreviousLocation() => new BinaryTreeLocation<T>(BinaryNode.GetNextNode());
    }
}
