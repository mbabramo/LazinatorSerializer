using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ValueTree
{
    public partial class AvlCountedNode<T> : AvlNode<T>, IAvlCountedNode<T> where T : ILazinator
    {
        public long SelfCount => 1;
        public long LongCount => LeftCount + SelfCount + RightCount;

        public AvlCountedNode<T> ParentCountedNode => (AvlCountedNode<T>)Parent;
        public AvlCountedNode<T> LeftCountedNode => (AvlCountedNode<T>)Left;
        public AvlCountedNode<T> RightCountedNode => (AvlCountedNode<T>)Right;

        public long? _Index;
        public long Index
        {
            get
            {
                if (_Index != null)
                    return (long) _Index;
                if (Parent == null)
                    return LeftCount;
                if (IsLeftNode)
                    return ParentCountedNode.Index - RightCount - 1;
                else if (IsRightNode)
                    return ParentCountedNode.Index + LeftCount + 1;
                throw new Exception("Malformed AvlTree.");
            }
        }

        public override void UpdateFollowingTreeChange()
        {
            if (LeftCountedNode != null && LeftCountedNode.NodeVisitedDuringChange)
            {
                LeftCountedNode.UpdateFollowingTreeChange();
            }
            if (RightCountedNode != null && RightCountedNode.NodeVisitedDuringChange)
            {
                RightCountedNode.UpdateFollowingTreeChange();
            }
            LeftCount = LeftCountedNode?.LongCount ?? 0;
            RightCount = RightCountedNode?.LongCount ?? 0;
            _Index = null;
            if (NodeVisitedDuringChange)
                NodeVisitedDuringChange = false;
        }

        internal void ResetIndicesFollowingTreeSplit()
        {
            _Index = null;
            if (_Left != null)
                LeftCountedNode.ResetIndicesFollowingTreeSplit();
            if (_Right != null)
                RightCountedNode.ResetIndicesFollowingTreeSplit();
        }


        public override string ToString()
        {
            return $"Index {Index}: {Value} (Count {LongCount}: Left {LeftCount} Self {SelfCount} Right {RightCount}) (visited {NodeVisitedDuringChange}) (ParentIndex {(Parent is AvlCountedNode<T> p ? p.Index.ToString() : "N/A")})";
        }

    }
}