using Lazinator.Core;
using LazinatorCollections;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    public partial class AvlAggregatedNode<T> : AvlCountedNode<T>, IAvlAggregatedNode<T> where T : ILazinator, ICountableContainer
    {
        public long LongAggregatedCount => LeftAggregatedCount + SelfAggregatedCount + RightAggregatedCount;

        public AvlAggregatedNode<T> ParentAggregatedNode => (AvlAggregatedNode<T>)Parent;
        public AvlAggregatedNode<T> LeftAggregatedNode => (AvlAggregatedNode<T>)Left;
        public AvlAggregatedNode<T> RightAggregatedNode => (AvlAggregatedNode<T>)Right;

        public long? _FirstAggregatedIndex;
        public long FirstAggregatedIndex
        {
            get
            {
                if (_FirstAggregatedIndex != null)
                    return (long)_FirstAggregatedIndex;
                if (Parent == null)
                    return LeftAggregatedCount;
                if (IsLeftNode)
                    return ParentAggregatedNode.FirstAggregatedIndex - RightAggregatedCount - SelfAggregatedCount;
                else if (IsRightNode)
                    return ParentAggregatedNode.FirstAggregatedIndex + LeftAggregatedCount + ParentAggregatedNode.SelfAggregatedCount;
                throw new Exception("Malformed AvlTree.");
            }
        }

        public long LastAggregatedIndex => FirstAggregatedIndex + SelfAggregatedCount - 1;

        public bool ContainsAggregatedIndex(long aggregatedIndex)
        {
            if (SelfAggregatedCount == 0)
                return false;
            return aggregatedIndex >= FirstAggregatedIndex && aggregatedIndex < FirstAggregatedIndex + SelfAggregatedCount;
        }

        public override void UpdateFollowingTreeChange()
        {
            base.UpdateFollowingTreeChange();
            UpdateCountsForThisNode();
        }

        public void UpdateFollowingNodeChange()
        {
            UpdateCountsForThisNode();
            if (ParentAggregatedNode != null)
                ParentAggregatedNode.UpdateFollowingNodeChange();
        }

        private void UpdateCountsForThisNode()
        {
            LeftAggregatedCount = LeftAggregatedNode?.LongAggregatedCount ?? 0;
            SelfAggregatedCount = Value.LongCount;
            RightAggregatedCount = RightAggregatedNode?.LongAggregatedCount ?? 0;
            _FirstAggregatedIndex = null;
        }

        public override string ToString()
        {
            return $"Index {Index} (items {FirstAggregatedIndex}-{LastAggregatedIndex}): {Value} Node count (total {LongCount} left {LeftCount} self {SelfCount} right {RightCount}) Aggregated items count (total {LongAggregatedCount} left {LeftAggregatedCount} self {SelfAggregatedCount} right {RightAggregatedCount})  (visited {NodeVisitedDuringChange}) (ParentIndex {(Parent is AvlCountedNode<T> p ? p.Index.ToString() : "N/A")})";
        }
    }
}
