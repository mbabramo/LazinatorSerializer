using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlCountedNode<T> : AvlNode<T>, IAvlCountedNode<T> where T : ILazinator
    {
        public long LeftCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long SelfCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long RightCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public long LongCount => LeftCount + SelfCount + RightCount;

        public AvlCountedNode<T> ParentCountedNode => (AvlCountedNode<T>)Parent;

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

        internal override bool NodeVisitedDuringChange
        {
            set
            {
                _NodeVisitedDuringChange = value;
                _Index = null;
            }
        }


        public override string ToString()
        {
            return $"Index {Index}: {Value} (Count {LongCount})";
        }

    }
}