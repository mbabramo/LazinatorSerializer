using System;
using System.Diagnostics;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    public partial class AvlNode<TKey, TValue> : IAvlNode<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        // We can't serialize the Parent, because an item can't appear multiple times in a hierarchy, so we use the Lazinator built-in parent as a substitute.
        private AvlNode<TKey, TValue> _Parent;
        public AvlNode<TKey, TValue> Parent
        {
            get
            {
                if (_Parent == null)
                {
                    if (LazinatorParents.LastAdded is AvlTree<TKey, TValue> p)
                        _Parent = null;
                    else
                        _Parent = (AvlNode<TKey, TValue>) LazinatorParents.LastAdded;
                }

                return _Parent;
            }
            set
            {
                _Parent = value;
                LazinatorParents = new LazinatorParentsCollection(value);
            }
        }

        public AvlNode<TKey, TValue> GetNextNode()
        {
            AvlNode<TKey, TValue> current = this;
            while (true)
            {
                var p = current.Parent;
                if (p == null)
                    return null;
                if (p.Left == current)
                    return p;
                current = p;
            }
        }

        [NonSerialized]
        internal bool NodeVisitedDuringChange;

        public void RecalculateCount()
        {
            // The approach here is to recursively visit all nodes visited during a change
            if (NodeVisitedDuringChange)
            {
                Left?.RecalculateCount();
                Right?.RecalculateCount();
                Count = LeftCount + RightCount + 1;
                NodeVisitedDuringChange = false;
            }
        }

        public int LeftCount => Left?.Count ?? 0;
        public int RightCount => Right?.Count ?? 0;
        public bool IsLeftNode => Parent != null && this == Parent._Left;
        public bool IsRightNode => Parent != null && this == Parent._Right;
        public int Index
        {
            get
            {
                if (Parent == null)
                    return LeftCount;
                if (IsLeftNode)
                    return Parent.Index - RightCount - 1;
                else if (IsRightNode)
                    return Parent.Index + LeftCount + 1;
                throw new Exception("Malformed AvlTree.");
            }
        }

        public void Print(string indent, bool right)
        {
            Debug.Write(indent);
            if (right)
            {
                Debug.Write("\\-");
                indent += "  ";
            }
            else
            {
                Debug.Write("|-");
                indent += "| ";
            }

            Debug.WriteLine($"{Key} ({Count}, {NodeVisitedDuringChange})");

            Left?.Print(indent, false);
            Right?.Print(indent, true);
        }
    }
}
