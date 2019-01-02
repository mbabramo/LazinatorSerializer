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
            // All the nodes to the left are complete. Therefore, if there is a node to the right, we move to the right and then as far to the left as possible. Otherwise, we move to the first parent where this is on the left; if there is no such parent, we return null, because there is no last node.
            AvlNode<TKey, TValue> current = this;
            if (current.Right != null)
            {
                current = current.Right;
                while (current.Left != null)
                {
                    current = current.Left;
                }
                return current;
            }
            while (true)
            {
                var p = current.Parent;
                if (p == null)
                {
                    return null;
                }
                if (p.Left == current)
                    return p;
                current = p;
            }
        }

        public AvlNode<TKey, TValue> GetPreviousNode()
        {
            // If there is a left node, then we just came from there. 
            AvlNode<TKey, TValue> current = this;
            if (current.Left != null)
            {
                current = current.Left;
                return current;
            }
            // If the parent is to the left, then that was previous.
            var p = current.Parent;
            if (p.Left == current)
            {
                current = p;
                return current;
            }
            // Otherwise, go up to the right as far as possible and then one up to the left.
            while (p.Right == current)
            {
                current = p;
                p = current.Parent;
            }
            return p;
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

        public long LeftCount => Left?.Count ?? 0;
        public long RightCount => Right?.Count ?? 0;
        public bool IsLeftNode => Parent != null && this == Parent._Left;
        public bool IsRightNode => Parent != null && this == Parent._Right;
        public long Index
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

            Debug.WriteLine($"Index {Index}: ({Key}, {Value}) (Count {Count}, Visited {NodeVisitedDuringChange})");

            Left?.Print(indent, false);
            Right?.Print(indent, true);
        }
    }
}
