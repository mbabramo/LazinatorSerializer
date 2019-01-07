using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public class AvlTree<T> : IAvlTree<T>, IOrderableContainer<T>, IEnumerable<T> where T : ILazinator
    {
        public AvlNode<T> Root { get => throw new NotImplementedException(); set => throw new NotImplementedException(); } // DEBUG

        protected virtual AvlNode<T> CreateNode(T value, AvlNode<T> parent = null)
        {
            return new AvlNode<T>()
            {
                Value = value,
                Parent = parent
            };
        }

        public string ToTreeString() => Root?.ToTreeString() ?? "";

        protected AvlNode<T> GetMatchingNode(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer) => GetMatchingNode(whichOne, node => CompareValueToNode(value, node, whichOne, comparer));

        protected AvlNode<T> GetMatchingNode(MultivalueLocationOptions whichOne, Func<AvlNode<T>, int> comparisonFunc)
        {
            var result = GetMatchingOrNextNode(whichOne, comparisonFunc);
            if (result.found)
                return result.node;
            return null;
        }

        protected (AvlNode<T> node, bool found) GetMatchingOrNextNode(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer) => GetMatchingOrNextNode(whichOne, node => CompareValueToNode(value, node, whichOne, comparer));

        protected (AvlNode<T> node, bool found) GetMatchingOrNextNode(MultivalueLocationOptions whichOne, Func<AvlNode<T>, int> comparisonFunc)
        {
            AvlNode<T> node = Root;
            if (node == null)
                return (null, false);
            while (true)
            {
                int comparison = comparisonFunc(node);
                if (comparison < 0)
                {
                    if (node.Left == null)
                        return (node, false);
                    node = node.Left;
                }
                else if (comparison > 0)
                {
                    if (node.Right == null)
                    {
                        var next = node.GetNextNode();
                        return (next, false);
                    }
                    node = node.Right;
                }
                else
                {
                    return (node, true);
                }
            }
        }

        private int CompareValueToNode(T value, AvlNode<T> node, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            int compare = comparer.Compare(value, node.Value);
            if (compare == 0)
            {
                // Even though value is equal, we don't calculate it as equal if, for example, we're at the second value and the request is for the first.
                if (whichOne == MultivalueLocationOptions.BeforeFirst)
                    compare = 1;
                else if (whichOne == MultivalueLocationOptions.AfterLast)
                    compare = -1;
                else if (whichOne == MultivalueLocationOptions.First)
                {
                    var previousNode = node.GetPreviousNode();
                    if (previousNode != null && comparer.Compare(value, previousNode.Value) == 0)
                        compare = 1;
                }
                else
                {
                    var nextNode = node.GetNextNode();
                    if (nextNode != null && comparer.Compare(value, nextNode.Value) == 0)
                        compare = -1;
                }
            }

            return compare;
        }

        public bool Contains(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public virtual void Clear()
        {
            Root = null;
        }

        /// <summary>
        /// Gets the last node.
        /// </summary>
        /// <returns></returns>
        protected AvlNode<T> FirstNode()
        {
            var x = Root;
            while (x.Left != null)
                x = x.Left;
            return x;
        }

        /// <summary>
        /// Gets the last node.
        /// </summary>
        /// <returns></returns>
        protected AvlNode<T> LastNode()
        {
            var x = Root;
            while (x.Right != null)
                x = x.Right;
            return x;
        }

        public bool TryInsertSorted(T item, IComparer<T> comparer) => TryInsertSorted(item, MultivalueLocationOptions.Any, comparer);

        public bool TryInsertSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => TryInsertSorted(item, whichOne, node => CompareValueToNode(item, node, whichOne, comparer));

        protected bool TryInsertSorted(T item, MultivalueLocationOptions whichOne, Func<AvlNode<T>, int> comparisonFunc)
        {
            var result = TryInsertSortedReturningNode(item, whichOne, comparisonFunc);
            return result.insertionNotReplacement;
        }

        public (AvlNode<T> node, bool insertionNotReplacement) TryInsertSortedReturningNode(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => TryInsertSortedReturningNode(item, whichOne, node => CompareValueToNode(item, node, whichOne, comparer));

        protected (AvlNode<T> node, bool insertionNotReplacement) TryInsertSortedReturningNode(T item, MultivalueLocationOptions whichOne, Func<AvlNode<T>, int> comparisonFunc)
        {
            AvlNode<T> node = Root;
            while (node != null)
            {
                node.NodeVisitedDuringChange = true;

                int compare = comparisonFunc(node);

                if (compare < 0)
                {
                    AvlNode<T> left = node.Left;

                    if (left == null)
                    {
                        var childNode = CreateNode(item, node);
                        childNode.NodeVisitedDuringChange = true;
                        node.Left = childNode;
                        // index is same as node
                        InsertBalance(node, 1);

                        return (node, true);
                    }
                    else
                    {
                        node = left;
                    }
                }
                else if (compare > 0)
                {
                    AvlNode<T> right = node.Right;

                    if (right == null)
                    {
                        var childNode = CreateNode(item, node);
                        childNode.NodeVisitedDuringChange = true;
                        node.Right = childNode;

                        InsertBalance(node, -1);

                        return (node, true);
                    }
                    else
                    {
                        node = right;
                    }
                }
                else
                {
                    node.Value = item;

                    return (node, false);
                }
            }

            Root = CreateNode(item);
            Root.NodeVisitedDuringChange = true;

            return (node, true);
        }

        public bool TryRemoveSorted(T item, IComparer<T> comparer) => TryRemoveSorted(item, MultivalueLocationOptions.Any, comparer);

        public bool TryRemoveSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => TryRemoveSorted(whichOne, node => CompareValueToNode(item, node, whichOne, comparer));

        protected bool TryRemoveSorted(MultivalueLocationOptions whichOne, Func<AvlNode<T>, int> comparisonFunc) => TryRemoveSortedReturningNode(whichOne, comparisonFunc) != null;

        public AvlNode<T> TryRemoveSortedReturningNode(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => TryRemoveSortedReturningNode(whichOne, node => CompareValueToNode(item, node, whichOne, comparer));

        protected AvlNode<T> TryRemoveSortedReturningNode(MultivalueLocationOptions whichOne, Func<AvlNode<T>, int> comparisonFunc)
        {
            AvlNode<T> node = Root;
            
            while (node != null)
            {
                node.NodeVisitedDuringChange = true;

                int compare = comparisonFunc(node);
                if (compare < 0)
                {
                    node = node.Left;
                }
                else if (compare > 0)
                {
                    node = node.Right;
                }
                else
                {
                    AvlNode<T> left = node.Left;
                    AvlNode<T> right = node.Right;

                    if (left == null)
                    {
                        if (right == null)
                        {
                            if (node == Root)
                            {
                                Root = null;
                            }
                            else
                            {
                                AvlNode<T> parent = node.Parent;

                                if (parent.Left == node)
                                {
                                    parent.Left = null;

                                    DeleteBalance(parent, -1);
                                }
                                else
                                {
                                    parent.Right = null;

                                    DeleteBalance(parent, 1);
                                }
                            }
                        }
                        else
                        {
                            Replace(node, right);
                            right.NodeVisitedDuringChange = true;

                            DeleteBalance(node, 0);
                        }
                    }
                    else if (right == null)
                    {
                        Replace(node, left);
                        left.NodeVisitedDuringChange = true;

                        DeleteBalance(node, 0);
                    }
                    else
                    {
                        AvlNode<T> successor = right;
                        successor.NodeVisitedDuringChange = true;

                        if (successor.Left == null)
                        {
                            AvlNode<T> parent = node.Parent;

                            successor.Parent = parent;
                            successor.Left = left;
                            successor.Balance = node.Balance;
                            left.Parent = successor;

                            if (node == Root)
                            {
                                Root = successor;
                            }
                            else
                            {
                                if (parent.Left == node)
                                {
                                    parent.Left = successor;
                                }
                                else
                                {
                                    parent.Right = successor;
                                }
                            }

                            DeleteBalance(successor, 1);
                        }
                        else
                        {
                            while (successor.Left != null)
                            {
                                successor = successor.Left;
                                successor.NodeVisitedDuringChange = true;
                            }

                            AvlNode<T> parent = node.Parent;
                            AvlNode<T> successorParent = successor.Parent;
                            AvlNode<T> successorRight = successor.Right;
                            if (successorRight != null)
                                successorRight.NodeVisitedDuringChange = true;

                            if (successorParent.Left == successor)
                            {
                                successorParent.Left = successorRight;
                            }
                            else
                            {
                                successorParent.Right = successorRight;
                            }

                            if (successorRight != null)
                            {
                                successorRight.Parent = successorParent;
                            }

                            successor.Parent = parent;
                            successor.Left = left;
                            successor.Balance = node.Balance;
                            successor.Right = right;
                            right.Parent = successor;
                            left.Parent = successor;

                            if (node == Root)
                            {
                                Root = successor;
                            }
                            else
                            {
                                if (parent.Left == node)
                                {
                                    parent.Left = successor;
                                }
                                else
                                {
                                    parent.Right = successor;
                                }
                            }

                            DeleteBalance(successorParent, -1);
                        }
                    }

                    return node;
                }
            }

            return null;
        }

        #region Balancing

        private void InsertBalance(AvlNode<T> node, int balance)
        {
            while (node != null)
            {
                balance = (node.Balance += balance);

                if (balance == 0)
                {
                    return;
                }
                else if (balance == 2)
                {
                    if (node.Left.Balance == 1)
                    {
                        RotateRight(node);
                    }
                    else
                    {
                        RotateLeftRight(node);
                    }

                    return;
                }
                else if (balance == -2)
                {
                    if (node.Right.Balance == -1)
                    {
                        RotateLeft(node);
                    }
                    else
                    {
                        RotateRightLeft(node);
                    }

                    return;
                }

                AvlNode<T> parent = node.Parent;

                if (parent != null)
                {
                    balance = parent.Left == node ? 1 : -1;
                }

                node = parent;
            }
        }

        private AvlNode<T> RotateLeft(AvlNode<T> node)
        {
            AvlNode<T> right = node.Right;
            AvlNode<T> rightLeft = right.Left;
            AvlNode<T> parent = node.Parent;
            right.NodeVisitedDuringChange = true;
            if (rightLeft != null)
                rightLeft.NodeVisitedDuringChange = true;

            right.Parent = parent;
            right.Left = node;
            node.Right = rightLeft;
            node.Parent = right;

            if (rightLeft != null)
            {
                rightLeft.Parent = node;
            }

            if (node == Root)
            {
                Root = right;
            }
            else if (parent.Right == node)
            {
                parent.Right = right;
            }
            else
            {
                parent.Left = right;
            }

            right.Balance++;
            node.Balance = -right.Balance;

            return right;
        }

        private AvlNode<T> RotateRight(AvlNode<T> node)
        {
            AvlNode<T> left = node.Left;
            AvlNode<T> leftRight = left.Right;
            AvlNode<T> parent = node.Parent;
            left.NodeVisitedDuringChange = true;
            if (leftRight != null)
                leftRight.NodeVisitedDuringChange = true;

            left.Parent = parent;
            left.Right = node;
            node.Left = leftRight;
            node.Parent = left;

            if (leftRight != null)
            {
                leftRight.Parent = node;
            }

            if (node == Root)
            {
                Root = left;
            }
            else if (parent.Left == node)
            {
                parent.Left = left;
            }
            else
            {
                parent.Right = left;
            }

            left.Balance--;
            node.Balance = -left.Balance;

            return left;
        }

        private AvlNode<T> RotateLeftRight(AvlNode<T> node)
        {
            AvlNode<T> left = node.Left;
            AvlNode<T> leftRight = left.Right;
            AvlNode<T> parent = node.Parent;
            AvlNode<T> leftRightRight = leftRight.Right;
            AvlNode<T> leftRightLeft = leftRight.Left;
            left.NodeVisitedDuringChange = true;
            leftRight.NodeVisitedDuringChange = true;
            if (leftRightRight != null)
                leftRightRight.NodeVisitedDuringChange = true;
            if (leftRightLeft != null)
                leftRightLeft.NodeVisitedDuringChange = true;

            leftRight.Parent = parent;
            node.Left = leftRightRight;
            left.Right = leftRightLeft;
            leftRight.Left = left;
            leftRight.Right = node;
            left.Parent = leftRight;
            node.Parent = leftRight;

            if (leftRightRight != null)
            {
                leftRightRight.Parent = node;
            }

            if (leftRightLeft != null)
            {
                leftRightLeft.Parent = left;
            }

            if (node == Root)
            {
                Root = leftRight;
            }
            else if (parent.Left == node)
            {
                parent.Left = leftRight;
            }
            else
            {
                parent.Right = leftRight;
            }

            if (leftRight.Balance == -1)
            {
                node.Balance = 0;
                left.Balance = 1;
            }
            else if (leftRight.Balance == 0)
            {
                node.Balance = 0;
                left.Balance = 0;
            }
            else
            {
                node.Balance = -1;
                left.Balance = 0;
            }

            leftRight.Balance = 0;

            return leftRight;
        }

        private AvlNode<T> RotateRightLeft(AvlNode<T> node)
        {
            AvlNode<T> right = node.Right;
            AvlNode<T> rightLeft = right.Left;
            AvlNode<T> parent = node.Parent;
            AvlNode<T> rightLeftLeft = rightLeft.Left;
            AvlNode<T> rightLeftRight = rightLeft.Right;
            right.NodeVisitedDuringChange = true;
            rightLeft.NodeVisitedDuringChange = true;
            if (rightLeftLeft != null)
                rightLeftLeft.NodeVisitedDuringChange = true;
            if (rightLeftRight != null)
                rightLeftRight.NodeVisitedDuringChange = true;

            rightLeft.Parent = parent;
            node.Right = rightLeftLeft;
            right.Left = rightLeftRight;
            rightLeft.Right = right;
            rightLeft.Left = node;
            right.Parent = rightLeft;
            node.Parent = rightLeft;

            if (rightLeftLeft != null)
            {
                rightLeftLeft.Parent = node;
            }

            if (rightLeftRight != null)
            {
                rightLeftRight.Parent = right;
            }

            if (node == Root)
            {
                Root = rightLeft;
            }
            else if (parent.Right == node)
            {
                parent.Right = rightLeft;
            }
            else
            {
                parent.Left = rightLeft;
            }

            if (rightLeft.Balance == 1)
            {
                node.Balance = 0;
                right.Balance = -1;
            }
            else if (rightLeft.Balance == 0)
            {
                node.Balance = 0;
                right.Balance = 0;
            }
            else
            {
                node.Balance = 1;
                right.Balance = 0;
            }

            rightLeft.Balance = 0;

            return rightLeft;
        }

        private void DeleteBalance(AvlNode<T> node, int balance)
        {
            while (node != null)
            {
                balance = (node.Balance += balance);

                if (balance == 2)
                {
                    if (node.Left.Balance >= 0)
                    {
                        node = RotateRight(node);

                        if (node.Balance == -1)
                        {
                            return;
                        }
                    }
                    else
                    {
                        node = RotateLeftRight(node);
                    }
                }
                else if (balance == -2)
                {
                    if (node.Right.Balance <= 0)
                    {
                        node = RotateLeft(node);

                        if (node.Balance == 1)
                        {
                            return;
                        }
                    }
                    else
                    {
                        node = RotateRightLeft(node);
                    }
                }
                else if (balance != 0)
                {
                    return;
                }

                AvlNode<T> parent = node.Parent;

                if (parent != null)
                {
                    balance = parent.Left == node ? -1 : 1;
                }

                node = parent;
            }
        }

        internal static void Replace(AvlNode<T> target, AvlNode<T> source)
        {
            AvlNode<T> left = source.Left;
            AvlNode<T> right = source.Right;

            target.Balance = source.Balance;
            target.Value = (T)source.Value.CloneLazinator();
            target.Left = left;
            target.Right = right;

            if (left != null)
            {
                left.Parent = target;
            }

            if (right != null)
            {
                right.Parent = target;
            }
        }

        #endregion

        #region Enumeration

        public IEnumerable<T> AsEnumerable(long skip = 0)
        {
            var enumerator = new AvlNodeEnumerator<T>(FirstNode());
            long i = skip;
            while (i > 0)
                enumerator.MoveNext();
            while (enumerator.MoveNext())
                yield return enumerator.Current.Value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator<T>)this;
        }

        #endregion
    }
}
