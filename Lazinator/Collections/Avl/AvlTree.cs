using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public class AvlTree<T> : IOrderableContainer<T> where T : ILazinator
    {
        public virtual AvlOldNode<T> CreateNode(T value, AvlOldNode<T> parent = null)
        {
            return new AvlOldNode<T>()
            {
                Value = value,
                Parent = parent
            };
        }

        /// <summary>
        /// Gets the node that either contains the key or the next node (which would contain the key if inserted).
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A node or null, if the key is after all keys in the tree</returns>
        public (AvlOldNode<T> node, long index, bool found) GetMatchingOrNextNode(TKey key, IComparer<TKey> comparer)
        {
            if (comparer == null)
                comparer = Comparer<TKey>.Default;
            var result = GetMatchingOrNextNodeHelper(key, comparer);
            if (result.found && AllowDuplicates)
            {
                bool matches = true;
                do
                { // make sure we have the first key match
                    result.index--;
                    matches = result.index >= 0 && KeyAtIndex(result.index).Equals(key);
                    if (!matches)
                        result.index++;
                }
                while (matches);
            }
            return result;
        }


        private (AvlOldNode<T> node, long index, bool found) GetMatchingOrNextNodeHelper(TKey key, IComparer<TKey> comparer)
        {
            AvlOldNode<T> node = Root;
            if (node == null)
                return (null, 0, false);
            long index = node?.LeftCount ?? 0;
            while (true)
            {
                int comparison = comparer.Compare(key, node.Key);
                if (comparison < 0)
                {
                    if (node.Left == null)
                        return (node, index, false);
                    node = node.Left;
                    index -= 1 + node.RightCount;
                }
                else if (comparison > 0)
                {
                    if (node.Right == null)
                    {
                        var next = node.GetNextNode();
                        index++;
                        return (next, index, false);
                    }
                    node = node.Right;
                    index += 1 + node.LeftCount;
                }
                else
                {
                    return (node, index, true);
                }
            }
        }

        /// <summary>
        /// Gets the node containing the key, or which would contain the key if the key were inserted, or the last
        /// node if there is no such node.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AvlOldNode<T> NodeForKey(TKey key, IComparer<TKey> comparer)
        {
            return GetMatchingOrNextNode(key, comparer).node ?? LastNode();
        }

        /// <summary>
        /// Gets the last node.
        /// </summary>
        /// <returns></returns>
        public AvlOldNode<T> LastNode()
        {
            var x = Root;
            while (x.Right != null)
                x = x.Right;
            return x;
        }
        private (bool inserted, long index) InsertByKeyOrIndex(TKey key, IComparer<TKey> comparer, TValue value, long? nodeIndex = null)
        {
            var result = InsertHelper(AllowDuplicates, key, comparer, value, nodeIndex);
            if (Root != null)
            {
                Root.RecalculateCount();
                //Root.Print("", false);
            }
            return result;
        }

        /// <summary>
        /// Helps complete the insert by key or by node.
        /// </summary>
        /// <param name="key">The key to insert.</param>
        /// <param name="value">The value to insert</param>
        /// <param name="nodeIndex">If the insertion point is based on an index, the index at which to insert. Null if the insertion point is to be found from the key.</param>
        /// <returns></returns>
        private (bool inserted, long index) InsertHelper(bool skipDuplicateKeys, TKey key, IComparer<TKey> comparer, TValue value, long? nodeIndex = null)
        {
            AvlOldNode<T> node = Root;
            long index = node?.LeftCount ?? 0;
            while (node != null)
            {
                node.NodeVisitedDuringChange = true;

                int compare = CompareKeyOrIndexToNode(key, comparer, skipDuplicateKeys, nodeIndex, index, node);

                if (compare < 0 || (compare == 0 && nodeIndex != null))
                {
                    AvlOldNode<T> left = node.Left;

                    if (left == null)
                    {
                        var childNode = CreateNode(key, value, node);
                        childNode.NodeVisitedDuringChange = true;
                        node.Left = childNode;
                        // index is same as node
                        InsertBalance(node, 1);

                        return (true, index);
                    }
                    else
                    {
                        node = left;
                        index -= 1 + (node?.RightCount ?? 0);
                    }
                }
                else if (compare > 0)
                {
                    AvlOldNode<T> right = node.Right;

                    if (right == null)
                    {
                        var childNode = CreateNode(key, value, node);
                        childNode.NodeVisitedDuringChange = true;
                        node.Right = childNode;

                        InsertBalance(node, -1);

                        index += 1;
                        return (true, index);
                    }
                    else
                    {
                        node = right;
                        index += 1 + (node?.LeftCount ?? 0);
                    }
                }
                else
                {
                    node.Value = value;

                    return (false, index);
                }
            }

            Root = CreateNode(key, value);
            Root.NodeVisitedDuringChange = true;

            return (true, 0);
        }

        private int CompareKeyOrIndexToNode(TKey key, IComparer<TKey> comparer, bool skipDuplicateKeys, long? desiredNodeIndex, long actualNodeIndex, AvlOldNode<T> node)
        {
            int compare;
            if (desiredNodeIndex is long index)
            {
                if (index == actualNodeIndex)
                {
                    compare = 0;
                }
                else if (index < actualNodeIndex)
                    compare = -1;
                else
                    compare = 1;
            }
            else
                compare = comparer.Compare(key, node.Key);

            if (compare == 0 && skipDuplicateKeys && desiredNodeIndex == null)
            {
                compare = 1;
            }
            return compare;
        }

        private bool Remove(TKey key, IComparer<TKey> comparer, long? nodeIndex = null)
        {
            bool returnVal = RemoveHelper(key, comparer, nodeIndex);
            if (Root != null)
            {
                Root.RecalculateCount();
                //Root.Print("", false);
            }
            return returnVal;
        }

        private bool RemoveHelper(TKey key, IComparer<TKey> comparer, long? nodeIndex)
        {
            AvlOldNode<T> node = Root;

            long index = node?.LeftCount ?? 0;
            while (node != null)
            {
                node.NodeVisitedDuringChange = true;

                int compare = CompareKeyOrIndexToNode(key, comparer, false, nodeIndex, index, node);
                if (compare < 0)
                {
                    node = node.Left;
                    index -= 1 + (node?.RightCount ?? 0);
                }
                else if (compare > 0)
                {
                    node = node.Right;
                    index += 1 + (node?.LeftCount ?? 0);
                }
                else
                {
                    AvlOldNode<T> left = node.Left;
                    AvlOldNode<T> right = node.Right;

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
                                AvlOldNode<T> parent = node.Parent;

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
                        AvlOldNode<T> successor = right;
                        successor.NodeVisitedDuringChange = true;

                        if (successor.Left == null)
                        {
                            AvlOldNode<T> parent = node.Parent;

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

                            AvlOldNode<T> parent = node.Parent;
                            AvlOldNode<T> successorParent = successor.Parent;
                            AvlOldNode<T> successorRight = successor.Right;
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

                    return true;
                }
            }

            return false;
        }

        #region Balancing

        private void InsertBalance(AvlOldNode<T> node, int balance)
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

                AvlOldNode<T> parent = node.Parent;

                if (parent != null)
                {
                    balance = parent.Left == node ? 1 : -1;
                }

                node = parent;
            }
        }

        private AvlOldNode<T> RotateLeft(AvlOldNode<T> node)
        {
            AvlOldNode<T> right = node.Right;
            AvlOldNode<T> rightLeft = right.Left;
            AvlOldNode<T> parent = node.Parent;
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

        private AvlOldNode<T> RotateRight(AvlOldNode<T> node)
        {
            AvlOldNode<T> left = node.Left;
            AvlOldNode<T> leftRight = left.Right;
            AvlOldNode<T> parent = node.Parent;
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

        private AvlOldNode<T> RotateLeftRight(AvlOldNode<T> node)
        {
            AvlOldNode<T> left = node.Left;
            AvlOldNode<T> leftRight = left.Right;
            AvlOldNode<T> parent = node.Parent;
            AvlOldNode<T> leftRightRight = leftRight.Right;
            AvlOldNode<T> leftRightLeft = leftRight.Left;
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

        private AvlOldNode<T> RotateRightLeft(AvlOldNode<T> node)
        {
            AvlOldNode<T> right = node.Right;
            AvlOldNode<T> rightLeft = right.Left;
            AvlOldNode<T> parent = node.Parent;
            AvlOldNode<T> rightLeftLeft = rightLeft.Left;
            AvlOldNode<T> rightLeftRight = rightLeft.Right;
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
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryInsertSorted(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryInsertSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryRemoveSorted(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryRemoveSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }


        private void DeleteBalance(AvlOldNode<T> node, int balance)
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

                AvlOldNode<T> parent = node.Parent;

                if (parent != null)
                {
                    balance = parent.Left == node ? -1 : 1;
                }

                node = parent;
            }
        }
        #endregion

        private static void Replace(AvlOldNode<T> target, AvlOldNode<T> source)
        {
            AvlOldNode<T> left = source.Left;
            AvlOldNode<T> right = source.Right;

            target.Balance = source.Balance;
            target.Key = (TKey)source.Key.CloneLazinator();
            target.Value = (TValue)source.Value.CloneLazinator();
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
