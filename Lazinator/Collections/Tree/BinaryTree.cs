﻿using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Tree
{
    public partial class BinaryTree<T> : IBinaryTree<T> where T : ILazinator
    {
        public BinaryNode<T> Root { get => throw new NotImplementedException(); set => throw new NotImplementedException(); } // DEBUG

        protected virtual BinaryNode<T> CreateNode(T value, BinaryNode<T> parent = null)
        {
            return new BinaryNode<T>()
            {
                Value = value,
                Parent = parent
            };
        }

        public string ToTreeString() => Root?.ToTreeString() ?? "";

        protected BinaryNode<T> GetMatchingNode(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer) => GetMatchingNode(whichOne, node => CompareValueToNode(value, node, whichOne, comparer));

        protected BinaryNode<T> GetMatchingNode(MultivalueLocationOptions whichOne, Func<BinaryNode<T>, int> comparisonFunc)
        {
            var result = GetMatchingOrNextNode(whichOne, comparisonFunc);
            if (result.found)
                return result.node;
            return null;
        }

        protected (BinaryNode<T> node, bool found) GetMatchingOrNextNode(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer) => GetMatchingOrNextNode(whichOne, node => CompareValueToNode(value, node, whichOne, comparer));

        protected (BinaryNode<T> node, bool found) GetMatchingOrNextNode(MultivalueLocationOptions whichOne, Func<BinaryNode<T>, int> comparisonFunc)
        {
            BinaryNode<T> node = Root;
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

        private int CompareValueToNode(T value, BinaryNode<T> node, MultivalueLocationOptions whichOne, IComparer<T> comparer)
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
        protected BinaryNode<T> FirstNode()
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
        protected BinaryNode<T> LastNode()
        {
            var x = Root;
            while (x.Right != null)
                x = x.Right;
            return x;
        }

        public bool TryInsertSorted(T item, IComparer<T> comparer) => TryInsertSorted(item, MultivalueLocationOptions.Any, comparer);

        public bool TryInsertSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => TryInsertSorted(item, whichOne, node => CompareValueToNode(item, node, whichOne, comparer));

        protected bool TryInsertSorted(T item, MultivalueLocationOptions whichOne, Func<BinaryNode<T>, int> comparisonFunc)
        {
            var result = TryInsertSortedReturningNode(item, whichOne, comparisonFunc);
            return result.insertionNotReplacement;
        }

        public (BinaryNode<T> node, bool insertionNotReplacement) TryInsertSortedReturningNode(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => TryInsertSortedReturningNode(item, whichOne, node => CompareValueToNode(item, node, whichOne, comparer));

        protected (BinaryNode<T> node, bool insertionNotReplacement) TryInsertSortedReturningNode(T item, MultivalueLocationOptions whichOne, Func<BinaryNode<T>, int> comparisonFunc)
        {
            BinaryNode<T> node = Root;
            while (node != null)
            {
                int compare = comparisonFunc(node);

                if (compare < 0)
                {
                    BinaryNode<T> left = node.Left;

                    if (left == null)
                    {
                        var childNode = CreateNode(item, node);
                        node.Left = childNode;

                        return (node, true);
                    }
                    else
                    {
                        node = left;
                    }
                }
                else if (compare > 0)
                {
                    BinaryNode<T> right = node.Right;

                    if (right == null)
                    {
                        var childNode = CreateNode(item, node);
                        node.Right = childNode;

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

            return (node, true);
        }

        public bool TryRemoveSorted(T item, IComparer<T> comparer) => TryRemoveSorted(item, MultivalueLocationOptions.Any, comparer);

        public bool TryRemoveSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => TryRemoveSorted(whichOne, node => CompareValueToNode(item, node, whichOne, comparer));

        protected bool TryRemoveSorted(MultivalueLocationOptions whichOne, Func<BinaryNode<T>, int> comparisonFunc) => TryRemoveSortedReturningNode(whichOne, comparisonFunc) != null;

        public BinaryNode<T> TryRemoveSortedReturningNode(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => TryRemoveSortedReturningNode(whichOne, node => CompareValueToNode(item, node, whichOne, comparer));

        protected BinaryNode<T> TryRemoveSortedReturningNode(MultivalueLocationOptions whichOne, Func<BinaryNode<T>, int> comparisonFunc)
        {
            BinaryNode<T> node = Root;

            while (node != null)
            {
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
                    BinaryNode<T> left = node.Left;
                    BinaryNode<T> right = node.Right;

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
                                BinaryNode<T> parent = node.Parent;

                                if (parent.Left == node)
                                {
                                    parent.Left = null;
                                }
                                else
                                {
                                    parent.Right = null;
                                }
                            }
                        }
                        else
                        {
                            Replace(node, right);
                        }
                    }
                    else if (right == null)
                    {
                        Replace(node, left);
                    }
                    else
                    {
                        BinaryNode<T> successor = right;

                        if (successor.Left == null)
                        {
                            BinaryNode<T> parent = node.Parent;

                            successor.Parent = parent;
                            successor.Left = left;
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
                        }
                        else
                        {
                            while (successor.Left != null)
                            {
                                successor = successor.Left;
                            }

                            BinaryNode<T> parent = node.Parent;
                            BinaryNode<T> successorParent = successor.Parent;
                            BinaryNode<T> successorRight = successor.Right;

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
                        }
                    }

                    return node;
                }
            }

            return null;
        }

        protected internal void Replace(BinaryNode<T> target, BinaryNode<T> source)
        {
            BinaryNode<T> left = source.Left;
            BinaryNode<T> right = source.Right;
            
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

    }
}
