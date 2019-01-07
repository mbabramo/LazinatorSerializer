using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Lazinator.Collections.Tuples;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    public partial class AvlOldNode<TKey, TValue> : IAvlOldNode<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public KeyValuePair<TKey, TValue> KeyValuePair => new KeyValuePair<TKey, TValue>(Key, Value);
        public LazinatorKeyValue<TKey, TValue> LazinatorKeyValue => new LazinatorKeyValue<TKey, TValue>(Key, Value);

        // We can't serialize the Parent, because an item can't appear multiple times in a hierarchy, so we use the Lazinator built-in parent as a substitute.
        private AvlOldNode<TKey, TValue> _Parent;
        public AvlOldNode<TKey, TValue> Parent
        {
            get
            {
                if (_Parent == null)
                {
                    if (LazinatorParents.LastAdded is AvlOldTree<TKey, TValue> p)
                        _Parent = null;
                    else
                        _Parent = (AvlOldNode<TKey, TValue>) LazinatorParents.LastAdded;
                }

                return _Parent;
            }
            set
            {
                _Parent = value;
                LazinatorParents = new LazinatorParentsCollection(value);
            }
        }

        public AvlOldNode<TKey, TValue> GetNextNode()
        {
            // All the nodes to the left are complete. Therefore, if there is a node to the right, we move to the right and then as far to the left as possible. Otherwise, we move to the first parent where this is on the left; if there is no such parent, we return null, because there is no last node.
            AvlOldNode<TKey, TValue> current = this;
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

        public AvlOldNode<TKey, TValue> GetPreviousNode()
        {
            // If there is a left node, then we just came from there. 
            AvlOldNode<TKey, TValue> current = this;
            if (current.Left != null)
            {
                current = current.Left;
                return current;
            }
            var p = current.Parent;
            // If there is no parent, then this is a root node with no children, and thus the first node.
            if (p == null)
                return null;
            // If the parent is to the left (i.e., this is the right child), then that was previous.
            if (p.Right == current)
            {
                current = p;
                return current;
            }
            // Otherwise, go up to the right as far as possible and then one up to the left.
            while (p != null && p.Left == current)
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

        public string ToTreeString()
        {
            StringBuilder sb = new StringBuilder();
            ToTreeStringHelper(sb, "", false);
            return sb.ToString();
        }

        private void ToTreeStringHelper(StringBuilder sb, string indent, bool right)
        {
            sb.Append(indent);
            if (right)
            {
                sb.Append("\\-");
                indent += "  ";
            }
            else
            {
                sb.Append("|-");
                indent += "| ";
            }

            sb.AppendLine($"Index {Index}: ({Key}, {Value}) (Count {Count}, Visited {NodeVisitedDuringChange})");

            Left?.ToTreeStringHelper(sb, indent, false);
            Right?.ToTreeStringHelper(sb, indent, true);
        }
    }
}
