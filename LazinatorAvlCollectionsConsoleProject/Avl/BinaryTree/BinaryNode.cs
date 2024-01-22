using Lazinator.Collections.Location;
using Lazinator.Core;
using System.Text;
using System;
using System.Linq;
using System.Collections.Generic;

// DEBUG // must check all tree types to make sure that their interfaces inherit from corresponding interfaces

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    public sealed partial class BinaryNode<T> : IBinaryNode<T>, ITreeString, INode<T, BinaryNode<T>>, ILazinator where T : ILazinator
    { 
        // We can't serialize the Parent, because an item can't appear multiple times in a hierarchy, so we use the Lazinator built-in parent as a substitute.
        [NonSerialized]
        private BinaryNode<T> _Parent;
        public BinaryNode<T> Parent
        {
            get
            {
                if (_Parent == null)
                {
                    if (LazinatorParents.LastAdded is BinaryNode<T> p)
                        _Parent = p;
                }

                return _Parent;
            }
            set
            {
                _Parent = value;
                LazinatorParents = new LazinatorParentsCollection(value as ILazinator);
            }
        }

        public bool IsAfterCollection => false;

        public bool IsLeftNode => Parent != null && this == Parent._Left;
        public bool IsRightNode => Parent != null && this == Parent._Right;

        public void OnRootAccessed()
        {

        }

        public void OnChildNodeAccessed(BinaryNode<T> childNode, bool isLeft)
        {

        }

        /// <summary>
        /// Returns either the right or left child, preferring already allocated to nonallocated child and right to left.
        /// </summary>
        public BinaryNode<T> SomeChild => _Right ?? _Left ?? Right ?? Left;

        public BinaryNode()
        {

        }

        public BinaryNode<T> CreateNode(T value, BinaryNode<T> parent = null)
        {
            return new BinaryNode<T>()
            {
                Value = value,
                Parent = parent
            };
        }

        public IContainerLocation GetLocation() => new TreeLocation<T, BinaryNode<T>>(this);

        public string ToTreeString() => NodeHelpers.ToTreeString<T, BinaryNode<T>>(this);

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
