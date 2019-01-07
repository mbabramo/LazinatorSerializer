using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlNode<T> : IAvlNode<T> where T : ILazinator
    {
        [NonSerialized]
        protected bool _NodeVisitedDuringChange;
        internal virtual bool NodeVisitedDuringChange { get; set; }

        public T Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Balance { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public AvlNode<T> _Left = null, _Right = null;
        public AvlNode<T> Left { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public AvlNode<T> Right { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public LazinatorParentsCollection LazinatorParents { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        // We can't serialize the Parent, because an item can't appear multiple times in a hierarchy, so we use the Lazinator built-in parent as a substitute.
        private AvlNode<T> _Parent;
        public AvlNode<T> Parent
        {
            get
            {
                if (_Parent == null)
                {
                    if (LazinatorParents.LastAdded is AvlTree<T> p)
                        _Parent = null;
                    else
                        _Parent = (AvlNode<T>)LazinatorParents.LastAdded;
                }

                return _Parent;
            }
            set
            {
                _Parent = value;
                LazinatorParents = new LazinatorParentsCollection(value as ILazinator);
            }
        }

        public bool IsLeftNode => Parent != null && this == Parent._Left;
        public bool IsRightNode => Parent != null && this == Parent._Right;

        public AvlNode<T> GetNextNode()
        {
            // All the nodes to the left are complete. Therefore, if there is a node to the right, we move to the right and then as far to the left as possible. Otherwise, we move to the first parent where this is on the left; if there is no such parent, we return null, because there is no last node.
            AvlNode<T> current = this;
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

        public AvlNode<T> GetPreviousNode()
        {
            // If there is a left node, then we just came from there. 
            AvlNode<T> current = this;
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

            sb.AppendLine(ToString());

            Left?.ToTreeStringHelper(sb, indent, false);
            Right?.ToTreeStringHelper(sb, indent, true);
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
