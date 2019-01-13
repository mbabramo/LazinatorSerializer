using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tree;
using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections.Avl
{
    /// <summary>
    /// A basic AvlTree class, adding balancing to the BinaryTree class.
    /// </summary>
    /// <typeparam name="T">The type of the object to be stored on each node of the tree</typeparam>
    public partial class AvlTree<T> : BinaryTree<T>, IAvlTree<T>, IEnumerable<T> where T : ILazinator
    {
        public AvlNode<T> AvlRoot => (AvlNode<T>)Root;

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlTree<T>()
            {
                AllowDuplicates = AllowDuplicates
            };
        }

        protected override BinaryNode<T> CreateNode(T value, BinaryNode<T> parent = null)
        {
            return new AvlNode<T>()
            {
                Value = value,
                Parent = parent
            };
        }

        protected override (BinaryNode<T> node, bool insertedNotReplaced) TryInsertReturningNode(T item, Func<BinaryNode<T>, int> comparisonFunc)
        {
            AvlNode<T> node = AvlRoot;
            while (node != null)
            {
                node.NodeVisitedDuringChange = true;

                int compare = comparisonFunc(node);

                if (compare < 0)
                {
                    AvlNode<T> left = node.AvlLeft;

                    if (left == null)
                    {
                        var childNode = (AvlNode<T>)CreateNode(item, node);
                        childNode.NodeVisitedDuringChange = true;
                        node.Left = childNode;
                        // index is same as node
                        if (!Unbalanced)
                            InsertBalance(node, 1);

                        AvlRoot.UpdateFollowingTreeChange();
                        return (childNode, true);
                    }
                    else
                    {
                        node = left;
                    }
                }
                else if (compare > 0)
                {
                    AvlNode<T> right = node.AvlRight;

                    if (right == null)
                    {
                        var childNode = (AvlNode<T>)CreateNode(item, node);
                        childNode.NodeVisitedDuringChange = true;
                        node.Right = childNode;

                        if (!Unbalanced)
                            InsertBalance(node, -1);

                        AvlRoot.UpdateFollowingTreeChange();
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

                    AvlRoot.UpdateFollowingTreeChange();
                    return (node, false);
                }
            }

            Root = CreateNode(item);
            AvlRoot.NodeVisitedDuringChange = true;
            AvlRoot.UpdateFollowingTreeChange();

            return (Root, true);
        }

        private struct MiniBoolStack
        {
            ulong storage;
            byte index;

            public void Push(bool value)
            {
                if (value)
                    storage = storage | ((ulong) 1 << index);
                else
                    storage &= ~((ulong) 1 << index);
                index++;
            }

            public bool Pop()
            {
                if (index == 0)
                    throw new Exception();
                index--;
                bool set = (storage & ((ulong)1 << index)) != 0;
                return set;
            }

            public bool Any()
            {
                return index > 0;
            }
        }


        public void RemoveNode(BinaryNode<T> node)
        {
            MiniBoolStack pathIsLeft = new MiniBoolStack();
            var onPathToNode = node;
            while (onPathToNode.Parent != null)
            {
                pathIsLeft.Push(onPathToNode.Parent.Left == onPathToNode);
                onPathToNode = onPathToNode.Parent;
            }
            BinaryNode<T> result = TryRemoveReturningNode(MultivalueLocationOptions.Any, x =>
                {
                    AvlNode<T> avlNode = (AvlNode<T>)x;
                    if (!pathIsLeft.Any())
                        return 0;
                    // DEBUG -- must be tested. Could be the opposite order.
                    // TODO: Use a long as a bitset instead of a stack to avoid allocation
                    if (pathIsLeft.Pop())
                        return -1;
                    else
                        return 1;

                });
            if (result != node)
                throw new Exception("Internal exception on RemoveNode.");
        }

        protected override BinaryNode<T> TryRemoveReturningNode(MultivalueLocationOptions whichOne, Func<BinaryNode<T>, int> comparisonFunc)
        {
            AvlNode<T> node = AvlRoot;

            while (node != null)
            {
                node.NodeVisitedDuringChange = true;

                int compare = comparisonFunc(node);
                if (compare < 0)
                {
                    node = node.AvlLeft;
                }
                else if (compare > 0)
                {
                    node = node.AvlRight;
                }
                else
                {
                    AvlNode<T> left = node.AvlLeft;
                    AvlNode<T> right = node.AvlRight;

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
                                AvlNode<T> parent = node.AvlParent;

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
                            Replace(node, right);
                            right.NodeVisitedDuringChange = true;

                            if (!Unbalanced)
                                DeleteBalance(node, 0);
                        }
                    }
                    else if (right == null)
                    {
                        Replace(node, left);
                        left.NodeVisitedDuringChange = true;

                        if (!Unbalanced)
                            DeleteBalance(node, 0);
                    }
                    else
                    {
                        AvlNode<T> successor = right;
                        successor.NodeVisitedDuringChange = true;

                        if (successor.Left == null)
                        {
                            AvlNode<T> parent = node.AvlParent;

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
                            while (successor.AvlLeft != null)
                            {
                                successor = successor.AvlLeft;
                                successor.NodeVisitedDuringChange = true;
                            }

                            AvlNode<T> parent = node.AvlParent;
                            AvlNode<T> successorParent = successor.AvlParent;
                            AvlNode<T> successorRight = successor.AvlRight;
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
                    return node;
                }
            }

            AvlRoot?.UpdateFollowingTreeChange();
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
                    if (node.AvlLeft.Balance == 1)
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
                    if (node.AvlRight.Balance == -1)
                    {
                        RotateLeft(node);
                    }
                    else
                    {
                        RotateRightLeft(node);
                    }

                    return;
                }

                AvlNode<T> parent = node.AvlParent;

                if (parent != null)
                {
                    balance = parent.Left == node ? 1 : -1;
                }

                node = parent;
            }
        }

        private AvlNode<T> RotateLeft(AvlNode<T> node)
        {
            AvlNode<T> right = node.AvlRight;
            AvlNode<T> rightLeft = right.AvlLeft;
            AvlNode<T> parent = node.AvlParent;
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

        private AvlNode<T> RotateRight(AvlNode<T> node)
        {
            AvlNode<T> left = node.AvlLeft;
            AvlNode<T> leftRight = left.AvlRight;
            AvlNode<T> parent = node.AvlParent;
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

        private AvlNode<T> RotateLeftRight(AvlNode<T> node)
        {
            AvlNode<T> left = node.AvlLeft;
            AvlNode<T> leftRight = left.AvlRight;
            AvlNode<T> parent = node.AvlParent;
            AvlNode<T> leftRightRight = leftRight.AvlRight;
            AvlNode<T> leftRightLeft = leftRight.AvlLeft;
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

        private AvlNode<T> RotateRightLeft(AvlNode<T> node)
        {
            AvlNode<T> right = node.AvlRight;
            AvlNode<T> rightLeft = right.AvlLeft;
            AvlNode<T> parent = node.AvlParent;
            AvlNode<T> rightLeftLeft = rightLeft.AvlLeft;
            AvlNode<T> rightLeftRight = rightLeft.AvlRight;
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

        private void DeleteBalance(AvlNode<T> node, int balance)
        {
            while (node != null)
            {
                balance = (node.Balance += balance);

                if (balance == 2)
                {
                    if (node.AvlLeft.Balance >= 0)
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
                    if (node.AvlRight.Balance <= 0)
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

                AvlNode<T> parent = node.AvlParent;

                if (parent != null)
                {
                    balance = parent.Left == node ? -1 : 1;
                }

                node = parent;
            }
        }

        protected internal override void Replace(BinaryNode<T> target, BinaryNode<T> source)
        {
            base.Replace(target, source);
            ((AvlNode<T>)target).Balance = ((AvlNode<T>)source).Balance;
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
