
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;
using LazinatorAvlCollections.Avl.BinaryTree;
using Lazinator.Collections.Location;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    public sealed partial class AvlNode<T> : IAvlNode<T>, ITreeString, IUpdatableNode<T, AvlNode<T>> where T : ILazinator
    {
        public bool NodeVisitedDuringChange { get; set; }

        public void UpdateFollowingTreeChange()
        {
        }

        public AvlNode<T> LeftBackingField => _Left;
        public AvlNode<T> RightBackingField => _Right;

        // Following is boilerplate, copied with changes in type from BinaryNode<T>. We are avoiding inheritance to improve performance.

        // We can't serialize the Parent, because an item can't appear multiple times in a hierarchy, so we use the Lazinator built-in parent as a substitute.
        [NonSerialized]
        private AvlNode<T> _Parent;
        public AvlNode<T> Parent
        {
            get
            {
                if (_Parent == null)
                {
                    if (LazinatorParents.LastAdded is AvlNode<T> p)
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

        /// <summary>
        /// Returns either the right or left child, preferring already allocated to nonallocated child and right to left.
        /// </summary>
        public AvlNode<T> SomeChild => _Right ?? _Left ?? Right ?? Left;

        public AvlNode()
        {

        }

        public AvlNode<T> CreateNode(T value, AvlNode<T> parent = null)
        {
            return new AvlNode<T>()
            {
                Value = value,
                Parent = parent
            };
        }

        public IContainerLocation GetLocation() => new TreeLocation<T, AvlNode<T>>(this);

        public string ToTreeString() => NodeHelpers.ToTreeString<T, AvlNode<T>>(this);

        public override string ToString()
        {
            return $"{Value}";
        }

    }
}
