using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Tree
{
    public partial class BinaryNode<T> : IBinaryNode<T>, IContainerLocation where T : ILazinator
    {
        // We can't serialize the Parent, because an item can't appear multiple times in a hierarchy, so we use the Lazinator built-in parent as a substitute.
        private BinaryNode<T> _Parent;
        public BinaryNode<T> Parent
        {
            get
            {
                if (_Parent == null)
                {
                    if (LazinatorParents.LastAdded is BinaryTree<T> p)
                        _Parent = null;
                    else
                        _Parent = (BinaryNode<T>)LazinatorParents.LastAdded;
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

        /// <summary>
        /// Returns either the right or left child, preferring already allocated to nonallocated child and right to left.
        /// </summary>
        public BinaryNode<T> SomeChild => _Right ?? _Left ?? Right ?? Left;


        protected virtual BinaryNode<T> CreateNode(T value, BinaryNode<T> parent = null)
        {
            return new BinaryNode<T>()
            {
                Value = value,
                Parent = parent
            };
        }

        public BinaryNode<T> GetNextNode()
        {
            // All the nodes to the left are complete. Therefore, if there is a node to the right, we move to the right and then as far to the left as possible. Otherwise, we move to the first parent where this is on the left; if there is no such parent, we return null, because there is no last node.
            BinaryNode<T> current = this;
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

        public BinaryNode<T> GetPreviousNode()
        {
            // If there is a left node, then we just came from its rightmost descendant. 
            BinaryNode<T> current = this;
            if (current.Left != null)
            {
                current = current.Left;
                while (current.Right != null)
                    current = current.Right;
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

        public IContainerLocation GetNextLocation() => GetNextNode();

        public IContainerLocation GetPreviousLocation() => GetPreviousNode();

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
