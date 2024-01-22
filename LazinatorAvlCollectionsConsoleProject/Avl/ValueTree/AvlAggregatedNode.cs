using Lazinator.Core;
using LazinatorAvlCollections.Avl.BinaryTree;
using Lazinator.Collections;
using Lazinator.Collections.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    public sealed partial class AvlAggregatedNode<T> : IAvlAggregatedNode<T>, IAggregatedNode<T, AvlAggregatedNode<T>>, ILazinator where T : ILazinator, ICountableContainer
    { 
        public AvlAggregatedNode()
        {

        }

        public AvlAggregatedNode<T> CreateNode(T value, AvlAggregatedNode<T> parent = null)
        {
            return new AvlAggregatedNode<T>()
            {
                Value = value,
                Parent = parent
            };
        }

        public long LongAggregatedCount => LeftAggregatedCount + SelfAggregatedCount + RightAggregatedCount;
        
        public long FirstAggregatedIndex { get; set; }

        public long LastAggregatedIndex => FirstAggregatedIndex + SelfAggregatedCount - 1;

        public void OnRootAccessed()
        {
            Index = LeftCount;
            FirstAggregatedIndex = LeftAggregatedCount;
        }

        public void OnChildNodeAccessed(AvlAggregatedNode<T> childNode, bool isLeft)
        {
            // Note: We set the index of a node every time it is accessed, since its index might have changed since the last access.
            if (childNode == null)
                return;
            AvlAggregatedNode<T> aggregatedChildNode = (AvlAggregatedNode<T>)childNode;
            if (isLeft)
            {
                aggregatedChildNode.Index = Index - aggregatedChildNode.RightCount - 1;
                aggregatedChildNode.FirstAggregatedIndex = FirstAggregatedIndex - aggregatedChildNode.RightAggregatedCount - aggregatedChildNode.SelfAggregatedCount;
            }
            else
            {
                aggregatedChildNode.Index = Index + aggregatedChildNode.LeftCount + 1;
                aggregatedChildNode.FirstAggregatedIndex = FirstAggregatedIndex + aggregatedChildNode.LeftAggregatedCount + SelfAggregatedCount;
            }
        }

        public bool ContainsAggregatedIndex(long aggregatedIndex)
        {
            if (SelfAggregatedCount == 0)
                return false;
            return aggregatedIndex >= FirstAggregatedIndex && aggregatedIndex < FirstAggregatedIndex + SelfAggregatedCount;
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
            UpdateCountsForThisNode();
        }
        
        public void UpdateFollowingNodeChange()
        {
            UpdateCountsForThisNode();
            if (Parent != null)
                Parent.UpdateFollowingNodeChange();
        }

        private void UpdateCountsForThisNode()
        {
            LeftAggregatedCount = Left?.LongAggregatedCount ?? 0;
            SelfAggregatedCount = Value.LongCount;
            RightAggregatedCount = Right?.LongAggregatedCount ?? 0;
        }

        public override string ToString()
        {
            return $"Index {Index} (items {FirstAggregatedIndex}-{LastAggregatedIndex}): {Value} Node count (total {LongCount} left {LeftCount} self {SelfCount} right {RightCount}) Aggregated items count (total {LongAggregatedCount} left {LeftAggregatedCount} self {SelfAggregatedCount} right {RightAggregatedCount})  (visited {NodeVisitedDuringChange}) (ParentIndex {(Parent is AvlAggregatedNode<T> p ? p.Index.ToString() : "N/A")})";
        }


        // Following is boilerplate, copied with changes in type from AvlAggregatedNode<T>. We are avoiding inheritance to improve performance.

        public long SelfCount => 1;
        public long LongCount => LeftCount + SelfCount + RightCount;

        public long Index { get; set; }

        public bool NodeVisitedDuringChange { get; set; }

        public AvlAggregatedNode<T> LeftBackingField => _Left;
        public AvlAggregatedNode<T> RightBackingField => _Right;

        // We can't serialize the Parent, because an item can't appear multiple times in a hierarchy, so we use the Lazinator built-in parent as a substitute.
        [NonSerialized]
        private AvlAggregatedNode<T> _Parent;
        public AvlAggregatedNode<T> Parent
        {
            get
            {
                if (_Parent == null)
                {
                    if (LazinatorParents.LastAdded is AvlAggregatedNode<T> p)
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
        public AvlAggregatedNode<T> SomeChild => _Right ?? _Left ?? Right ?? Left;

        public IContainerLocation GetLocation() => new TreeLocation<T, AvlAggregatedNode<T>>(this);

        public string ToTreeString() => NodeHelpers.ToTreeString<T, AvlAggregatedNode<T>>(this);
    }
}
