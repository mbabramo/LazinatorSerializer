using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ValueTree
{
    public partial class AvlAggregatedNode<T> : AvlCountedNode<T>, IAvlAggregatedNode<T> where T : ILazinator
    {
        public long LongAggregatedCount => LeftAggregatedCount + SelfAggregatedCount + RightAggregatedCount;

        public AvlAggregatedNode<T> ParentAggregatedNode => (AvlAggregatedNode<T>)Parent;
        public AvlAggregatedNode<T> LeftAggregatedNode => (AvlAggregatedNode<T>)Left;
        public AvlAggregatedNode<T> RightAggregatedNode => (AvlAggregatedNode<T>)Right;

        public long? _AggregatedIndex;
        public long AggregatedIndex
        {
            get
            {
                if (_AggregatedIndex != null)
                    return (long)_AggregatedIndex;
                if (Parent == null)
                    return LeftAggregatedCount;
                if (IsLeftNode)
                    return ParentAggregatedNode.Index - RightAggregatedCount - 1;
                else if (IsRightNode)
                    return ParentAggregatedNode.Index + LeftAggregatedCount + 1;
                throw new Exception("Malformed AvlTree.");
            }
        }

        public override void UpdateFollowingTreeChange()
        {
            base.UpdateFollowingTreeChange();
            LeftAggregatedCount = LeftAggregatedNode?.LongAggregatedCount ?? 0;
            RightAggregatedCount = RightAggregatedNode?.LongAggregatedCount ?? 0;
        }


        public override string ToString()
        {
            return $"Index {Index}: {Value} (Count {LongCount} ({LongAggregatedCount}): Left {LeftCount} ({LeftAggregatedCount}) Self {SelfCount} ({SelfAggregatedCount}) Right {RightCount} {RightAggregatedCount}) (visited {NodeVisitedDuringChange}) (ParentIndex {(Parent is AvlCountedNode<T> p ? p.Index.ToString() : "N/A")})";
        }
    }
}
