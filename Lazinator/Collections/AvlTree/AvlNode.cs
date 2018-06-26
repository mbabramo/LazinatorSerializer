using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    public partial class AvlNode<TKey, TValue> : IAvlNode<TKey, TValue>
        where TKey : ILazinator, new() where TValue : ILazinator, new()
    {
        // We can't serialize the Parent, because an item can't appear multiple times in a hierarchy, so we use the Lazinator built-in parent as a substitute.
        private AvlNode<TKey, TValue> _Parent;
        public AvlNode<TKey, TValue> Parent
        {
            get
            {
                if (_Parent == null)
                {
                    if (LazinatorParentsReference.LastAdded is AvlTree<TKey, TValue> p)
                        _Parent = null;
                    else
                        _Parent = (AvlNode<TKey, TValue>) LazinatorParentsReference.LastAdded;
                }

                return _Parent;
            }
            set
            {
                _Parent = value;
                LazinatorParentsReference = new LazinatorParentsCollection(value);
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

        public bool NodeVisitedDuringChange { get; set; }

        public void RecalculateCount()
        {
            // The approach here is to recursively visit all nodes visited during a change
            if (NodeVisitedDuringChange)
            {
                Left?.RecalculateCount();
                Right?.RecalculateCount();
                Count = (Left?.Count ?? 0) + (Right?.Count ?? 0) + 1;
                NodeVisitedDuringChange = false;
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
