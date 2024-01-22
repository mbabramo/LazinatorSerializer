using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using LazinatorAvlCollections.Avl.BinaryTree;
using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    /// <summary>
    /// A basic AvlTree class, adding balancing to the BinaryTree class.
    /// </summary>
    /// <typeparam name="T">The type of the object to be stored on each node of the tree</typeparam>
    public partial class AvlTreeWithNodeType<T, N> : BinaryTreeWithNodeType<T, N>, IAvlTreeWithNodeType<T, N>, IEnumerable<T> where T : ILazinator where N : class, ILazinator, IUpdatableNode<T, N>, new()
    {
        public N AvlRoot => (N)Root;

        #region Construction

        public AvlTreeWithNodeType(bool allowDuplicates, bool unbalanced, bool cacheEnds) : base(allowDuplicates, unbalanced, cacheEnds)
        {
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlTreeWithNodeType<T, N>(AllowDuplicates, Unbalanced, CacheEnds);
        }

        protected override N CreateNode(T value, N parent = null)
        {
            return new N()
            {
                Value = value,
                Parent = parent
            };
        }

        #endregion

        #region Insertion

        public override void InsertAt(IContainerLocation location, T item)
        {
            var node = ((TreeLocation<T, N>)location).Node;
            Func<N, int> comparisonFunc;
            if (node == null)
                comparisonFunc = n => 1; // insert after last node
            else
            {
                MiniBoolStack pathFromRoot = GetCompactPathToNode(node);
                comparisonFunc = ComparisonToFollowCompactPath(pathFromRoot);
            }
            InsertOrReplaceReturningNode(item, comparisonFunc, true);
        }

        protected override (N node, bool insertedNotReplaced) InsertOrReplaceReturningNode(T item, Func<N, int> comparisonFunc, bool alwaysInsert = false)
        {
            N node = AvlRoot;
            bool allMovesLeft = true, allMovesRight = true;
            bool alwaysInsertTargetFound = false;
            while (node != null)
            {
                node.NodeVisitedDuringChange = true;

                int compare = alwaysInsertTargetFound ? 0 : comparisonFunc(node);

                if (compare == 0 && alwaysInsert)
                {
                    if (!alwaysInsertTargetFound)
                    {
                        compare = -1; // move to left ...
                        alwaysInsertTargetFound = true;
                    }
                    else
                        compare = 1; // .. then keep moving to right
                }

                if (compare < 0)
                {
                    N left = node.Left;
                    allMovesRight = false;

                    if (left == null)
                    {
                        var childNode = (N)CreateNode(item, node);
                        childNode.NodeVisitedDuringChange = true;
                        node.Left = childNode;
                        // index is same as node
                        if (!Unbalanced)
                            InsertBalance(node, 1);

                        AvlRoot.UpdateFollowingTreeChange();
                        if (allMovesLeft)
                            ConsiderCacheUpdateAfterChange(childNode);
                        return (childNode, true);
                    }
                    else
                    {
                        node = left;
                    }
                }
                else if (compare > 0)
                {
                    N right = node.Right;
                    allMovesLeft = false;

                    if (right == null)
                    {
                        var childNode = (N)CreateNode(item, node);
                        childNode.NodeVisitedDuringChange = true;
                        node.Right = childNode;

                        if (!Unbalanced)
                            InsertBalance(node, -1);

                        AvlRoot.UpdateFollowingTreeChange();
                        if (allMovesRight)
                            ConsiderCacheUpdateAfterChange(childNode);
                        return (childNode, true);
                    }
                    else
                    {
                        node = right;
                    }
                }
                else
                {
                    node.Value = item;

                    if (allMovesLeft || allMovesRight)
                        ConsiderCacheUpdateAfterChange(node);
                    AvlRoot.UpdateFollowingTreeChange();
                    return (node, false);
                }
            }

            Root = CreateNode(item);
            AvlRoot.NodeVisitedDuringChange = true;
            AvlRoot.UpdateFollowingTreeChange();
            ConsiderCacheUpdateAfterChange(Root);
            return (Root, true);
        }

        #endregion

        #region Removal

        public override void RemoveNode(N node)
        {
            MiniBoolStack pathFromRoot = GetCompactPathToNode(node);
            N result = TryRemoveReturningNode(MultivalueLocationOptions.Any, ComparisonToFollowCompactPath(pathFromRoot));
            if (result != node)
                throw new Exception("Internal exception on RemoveNode.");
        }

        protected override N TryRemoveReturningNode(MultivalueLocationOptions whichOne, Func<N, int> comparisonFunc)
        {
            N node = AvlRoot;

            bool allMovesLeft = true, allMovesRight = true;
            while (node != null)
            {
                node.NodeVisitedDuringChange = true;

                int compare = comparisonFunc(node);
                if (compare < 0)
                {
                    node = node.Left;
                    allMovesRight = false;
                }
                else if (compare > 0)
                {
                    node = node.Right;
                    allMovesLeft = false;
                }
                else
                {
                    N left = node.Left;
                    N right = node.Right;

                    if (left == null)
                    {
                        if (right == null)
                        {
                            if (node == AvlRoot)
                            {
                                Root = null;
                            }
                            else
                            {
                                N parent = node.Parent;

                                if (parent.Left == node)
                                {
                                    parent.Left = null;

                                    if (!Unbalanced)
                                        DeleteBalance(parent, -1);
                                }
                                else
                                {
                                    parent.Right = null;

                                    if (!Unbalanced)
                                        DeleteBalance(parent, 1);
                                }
                            }
                        }
                        else
                        {
                            T value = node.Value;
                            Replace(node, right);
                            right.NodeVisitedDuringChange = true;

                            if (!Unbalanced)
                                DeleteBalance(node, 0);
                            if (allMovesLeft || allMovesRight)
                                ConsiderCacheUpdateAfterRemoval(value);
                        }
                    }
                    else if (right == null)
                    {
                        T value = node.Value;
                        Replace(node, left);
                        left.NodeVisitedDuringChange = true;

                        if (!Unbalanced)
                            DeleteBalance(node, 0);
                        if (allMovesLeft || allMovesRight)
                            ConsiderCacheUpdateAfterRemoval(value);
                    }
                    else
                    {
                        N successor = right;
                        successor.NodeVisitedDuringChange = true;

                        if (successor.Left == null)
                        {
                            N parent = node.Parent;

                            successor.Parent = parent;
                            successor.Left = left;
                            successor.Balance = node.Balance;
                            left.Parent = successor;

                            if (node == AvlRoot)
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

                            if (!Unbalanced)
                                DeleteBalance(successor, 1);
                        }
                        else
                        {
                            while (successor.Left != null)
                            {
                                successor = successor.Left;
                                successor.NodeVisitedDuringChange = true;
                            }

                            N parent = node.Parent;
                            N successorParent = successor.Parent;
                            N successorRight = successor.Right;
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

                            if (node == AvlRoot)
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

                            if (!Unbalanced)
                                DeleteBalance(successorParent, -1);
                        }
                    }

                    AvlRoot?.UpdateFollowingTreeChange();
                    if (allMovesLeft || allMovesRight)
                        ConsiderCacheUpdateAfterRemoval(node.Value);
                    return node;
                }
            }

            AvlRoot?.UpdateFollowingTreeChange();
            return null;
        }

        #endregion

        #region Balancing

        private void InsertBalance(N node, int balance)
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

                N parent = node.Parent;

                if (parent != null)
                {
                    balance = parent.Left == node ? 1 : -1;
                }

                node = parent;
            }
        }

        private N RotateLeft(N node)
        {
            N right = node.Right;
            N rightLeft = right.Left;
            N parent = node.Parent;
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

            if (node == AvlRoot)
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

        private N RotateRight(N node)
        {
            N left = node.Left;
            N leftRight = left.Right;
            N parent = node.Parent;
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

            if (node == AvlRoot)
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

        private N RotateLeftRight(N node)
        {
            N left = node.Left;
            N leftRight = left.Right;
            N parent = node.Parent;
            N leftRightRight = leftRight.Right;
            N leftRightLeft = leftRight.Left;
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

            if (node == AvlRoot)
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

        private N RotateRightLeft(N node)
        {
            N right = node.Right;
            N rightLeft = right.Left;
            N parent = node.Parent;
            N rightLeftLeft = rightLeft.Left;
            N rightLeftRight = rightLeft.Right;
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

            if (node == AvlRoot)
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

        private void DeleteBalance(N node, int balance)
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

                N parent = node.Parent;

                if (parent != null)
                {
                    balance = parent.Left == node ? -1 : 1;
                }

                node = parent;
            }
        }

        protected internal override void Replace(N target, N source)
        {
            base.Replace(target, source);
            ((N)target).Balance = ((N)source).Balance;
        }

        #endregion

        #region Enumeration

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator<T>)this;
        }

        #endregion


    }
}
