using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;
using LazinatorAvlCollections.Avl.BinaryTree;
using Lazinator.Collections.Location;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    public sealed partial class AvlCountedNode<T> : IAvlCountedNode<T>, IIndexableNode<T, AvlCountedNode<T>> where T : ILazinator
    {
        public long SelfCount => 1;
        public long LongCount => LeftCount + SelfCount + RightCount;
        
        public long Index { get; set; }

        public void OnRootAccessed()
        {
            // We set the index of the root every time it is accessed by the tree.
            Index = LeftCount;
        }

        public void OnChildNodeAccessed(AvlCountedNode<T> childNode, bool isLeft)
        {
            // Note: We set the index of a node every time it is accessed, since its index might have changed since the last access.
            if (childNode == null)
                return;
            AvlCountedNode<T> countedChildNode = (AvlCountedNode<T>)childNode;
            if (isLeft)
                countedChildNode.Index = Index - countedChildNode.RightCount - 1;
            else
                countedChildNode.Index = Index + countedChildNode.LeftCount + 1;
        }
        

        public void UpdateFollowingTreeChange()
        {
            if (Left != null && Left.NodeVisitedDuringChange)
            {
                Left.UpdateFollowingTreeChange();
            }
            if (Right != null && Right.NodeVisitedDuringChange)
            {
                Right.UpdateFollowingTreeChange();
            }
            LeftCount = Left?.LongCount ?? 0;
            RightCount = Right?.LongCount ?? 0;
            if (NodeVisitedDuringChange)
                NodeVisitedDuringChange = false;
        }


        public override string ToString()
        {
            return $"Index {Index}: {Value} (Count {LongCount}: Left {LeftCount} Self {SelfCount} Right {RightCount}) (visited {NodeVisitedDuringChange}) (ParentIndex {(Parent is AvlCountedNode<T> p ? p.Index.ToString() : "N/A")})";
        }

        public bool NodeVisitedDuringChange { get; set; }

        public AvlCountedNode<T> LeftBackingField => _Left;
        public AvlCountedNode<T> RightBackingField => _Right;

        // Following is boilerplate, copied with changes in type from AvlNode<T>. We are avoiding inheritance to improve performance.

        // We can't serialize the Parent, because an item can't appear multiple times in a hierarchy, so we use the Lazinator built-in parent as a substitute.
        [NonSerialized]
        private AvlCountedNode<T> _Parent;
        public AvlCountedNode<T> Parent
        {
            get
            {
                if (_Parent == null)
                {
                    if (LazinatorParents.LastAdded is AvlCountedNode<T> p)
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

        /// <summary>
        /// Returns either the right or left child, preferring already allocated to nonallocated child and right to left.
        /// </summary>
        public AvlCountedNode<T> SomeChild => _Right ?? _Left ?? Right ?? Left;

        public AvlCountedNode()
        {

        }

        public AvlCountedNode<T> CreateNode(T value, AvlCountedNode<T> parent = null)
        {
            return new AvlCountedNode<T>()
            {
                Value = value,
                Parent = parent
            };
        }

        public IContainerLocation GetLocation() => new TreeLocation<T, AvlCountedNode<T>>(this);

        public string ToTreeString() => NodeHelpers.ToTreeString<T, AvlCountedNode<T>>(this);

    }
}