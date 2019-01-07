using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlCountedNode<T> : AvlNode<T>, IAvlCountedNode<T> where T : ILazinator
    {
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
            LeftCount = LeftCountedNode?.LongCount ?? 0;
            if (RightCountedNode != null && RightCountedNode.NodeVisitedDuringChange)
            {
                RightCountedNode.UpdateFollowingTreeChange();
            }
            RightCount = RightCountedNode?.LongCount ?? 0;
            _Index = null;
            if (NodeVisitedDuringChange)
                NodeVisitedDuringChange = false;
        }


        public override string ToString()
        {
            return $"Index {Index}: {Value} (Count {LongCount}: Left {LeftCount} Self {SelfCount} Right {RightCount}) (visited {NodeVisitedDuringChange})";
        }

    }
}