﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using Lazinator.Support;

namespace Lazinator.Collections.Avl
{
    public partial class AvlOldTree<TKey, TValue> : IEnumerable<AvlOldNode<TKey, TValue>>, IAvlOldTree<TKey, TValue>, ILazinatorOrderedKeyable<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
		public AvlOldTree()
		{
		}

        public AvlOldNode<TKey, TValue> NodeAtIndex(long i)
        {
            ConfirmInRange(i);
            
			AvlOldNode<TKey, TValue> node = Root;

            long index = node?.LeftCount ?? 0;
            while (node != null)
			{
			    node.NodeVisitedDuringChange = true;

                if (i == index)
                    return node;
                else if (i < index)
                {
                    node = node.Left;
                    index -= 1 + (node?.RightCount ?? 0);
                }
                else
                {
                    node = node.Right;
                    index += 1 + (node?.LeftCount ?? 0);
                }
			}
            throw new InvalidOperationException(); // should not be reached
        }

        public KeyValuePair<TKey, TValue> KeyValuePairAtIndex(long i) => NodeAtIndex(i).KeyValuePair;
        public TKey KeyAtIndex(long i) => NodeAtIndex(i).Key;
        public void SetKeyAtIndex(long i, TKey key)
        {
            NodeAtIndex(i).Key = key;
        }
        public TValue ValueAtIndex(long i) => NodeAtIndex(i).Value;
        public void SetValueAtIndex(long i, TValue value)
        {
            NodeAtIndex(i).Value = value;
        }

        /// <summary>
        /// Creates a node for the tree.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public virtual AvlOldNode<TKey, TValue> CreateNode(TKey key, TValue value, AvlOldNode<TKey, TValue> parent = null)
        {
            return new AvlOldNode<TKey, TValue>()
            {
                Key = key,
                Value = value,
                Parent = parent
            };
        }

        private void ConfirmInRange(long i)
        {
            if (i < 0 || i >= Count)
                throw new ArgumentOutOfRangeException();
        }

        public long Count => Root?.Count ?? 0;

        public bool ValueAtKey(TKey key, out TValue value) => ValueAtKey(key, Comparer<TKey>.Default, out value);

        /// <summary>
        /// Returns the value at the first node matching the key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
		public bool ValueAtKey(TKey key, IComparer<TKey> comparer, out TValue value)
		{
			AvlOldNode<TKey, TValue> node = Root;

			while (node != null)
			{
                int comparison = comparer.Compare(key, node.Key);
                if (comparison < 0)
				{
					node = node.Left;
				}
				else if (comparison > 0)
				{
					node = node.Right;
				}
				else
				{
					value = node.Value;

					return true;
				}
			}

			value = default(TValue);

			return false;
		}

        public (TValue valueIfFound, long index, bool found) GetMatchingOrNext(TKey key) => GetMatchingOrNext(key, Comparer<TKey>.Default);

        public (TValue valueIfFound, long index, bool found) GetMatchingOrNext(TKey key, IComparer<TKey> comparer)
        {
            var result = GetMatchingOrNextNode(key);
            if (result.found)
                return (result.node.Value, result.index, true);
            else
                return (default, result.index, false);
        }

        public (AvlOldNode<TKey, TValue> node, long index, bool found) GetMatchingOrNextNode(TKey key) => GetMatchingOrNextNode(key, Comparer<TKey>.Default);

        /// <summary>
        /// Gets the node that either contains the key or the next node (which would contain the key if inserted).
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A node or null, if the key is after all keys in the tree</returns>
        public (AvlOldNode<TKey, TValue> node, long index, bool found) GetMatchingOrNextNode(TKey key, IComparer<TKey> comparer)
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

        private (AvlOldNode<TKey, TValue> node, long index, bool found) GetMatchingOrNextNodeHelper(TKey key, IComparer<TKey> comparer)
        {
            AvlOldNode<TKey, TValue> node = Root;
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

        public AvlOldNode<TKey, TValue> NodeForKey(TKey key) => NodeForKey(key, Comparer<TKey>.Default);

        /// <summary>
        /// Gets the node containing the key, or which would contain the key if the key were inserted, or the last
        /// node if there is no such node.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AvlOldNode<TKey, TValue> NodeForKey(TKey key, IComparer<TKey> comparer)
        {
            return GetMatchingOrNextNode(key, comparer).node ?? LastNode();
        }

        /// <summary>
        /// Gets the last node.
        /// </summary>
        /// <returns></returns>
        public AvlOldNode<TKey, TValue> LastNode()
        {
            var x = Root;
            while (x.Right != null)
                x = x.Right;
            return x;
        }

        public void InsertAtIndex(TKey key, TValue value, long nodeIndex) => InsertByKeyOrIndex(key, Comparer<TKey>.Default, value, nodeIndex);

        public (bool inserted, long index) Insert(TKey key, TValue value) => Insert(key, Comparer<TKey>.Default, value);

        public (bool inserted, long index) Insert(TKey key, IComparer<TKey> comparer, TValue value) => InsertByKeyOrIndex(key, comparer, value, null);

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
			AvlOldNode<TKey, TValue> node = Root;
            long index = node?.LeftCount ?? 0;
			while (node != null)
            {
                node.NodeVisitedDuringChange = true;
                
                int compare = CompareKeyOrIndexToNode(key, comparer, skipDuplicateKeys, nodeIndex, index, node);

                if (compare < 0 || (compare == 0 && nodeIndex != null))
                {
                    AvlOldNode<TKey, TValue> left = node.Left;

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
                    AvlOldNode <TKey, TValue> right = node.Right;

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

        private int CompareKeyOrIndexToNode(TKey key, IComparer<TKey> comparer, bool skipDuplicateKeys, long? desiredNodeIndex, long actualNodeIndex, AvlOldNode<TKey, TValue> node)
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

        #region Balancing

        private void InsertBalance(AvlOldNode<TKey, TValue> node, int balance)
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

				AvlOldNode<TKey, TValue> parent = node.Parent;

				if (parent != null)
				{
					balance = parent.Left == node ? 1 : -1;
				}

				node = parent;
			}
		}

		private AvlOldNode<TKey, TValue> RotateLeft(AvlOldNode<TKey, TValue> node)
		{
			AvlOldNode<TKey, TValue> right = node.Right;
			AvlOldNode<TKey, TValue> rightLeft = right.Left;
			AvlOldNode<TKey, TValue> parent = node.Parent;
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

		private AvlOldNode<TKey, TValue> RotateRight(AvlOldNode<TKey, TValue> node)
		{
			AvlOldNode<TKey, TValue> left = node.Left;
			AvlOldNode<TKey, TValue> leftRight = left.Right;
			AvlOldNode<TKey, TValue> parent = node.Parent;
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

		private AvlOldNode<TKey, TValue> RotateLeftRight(AvlOldNode<TKey, TValue> node)
		{
			AvlOldNode<TKey, TValue> left = node.Left;
			AvlOldNode<TKey, TValue> leftRight = left.Right;
			AvlOldNode<TKey, TValue> parent = node.Parent;
			AvlOldNode<TKey, TValue> leftRightRight = leftRight.Right;
			AvlOldNode<TKey, TValue> leftRightLeft = leftRight.Left;
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

		private AvlOldNode<TKey, TValue> RotateRightLeft(AvlOldNode<TKey, TValue> node)
		{
			AvlOldNode<TKey, TValue> right = node.Right;
			AvlOldNode<TKey, TValue> rightLeft = right.Left;
			AvlOldNode<TKey, TValue> parent = node.Parent;
			AvlOldNode<TKey, TValue> rightLeftLeft = rightLeft.Left;
			AvlOldNode<TKey, TValue> rightLeftRight = rightLeft.Right;
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

        /// <summary>
        /// Tries to remove the first node matching the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True if the item was contained in the tree, in which case it is removed</returns>
        public bool Remove(TKey key) => Remove(key, Comparer<TKey>.Default, null);

        /// <summary>
        /// Tries to remove the first node matching the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="comparer"></param>
        /// <returns>True if the item was contained in the tree, in which case it is removed</returns>
        public bool Remove(TKey key, IComparer<TKey> comparer) => Remove(key, comparer, null);

        /// <summary>
        /// Removes the node at the specified index.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>True if the item was contained in the tree, in which case it is removed</returns>
        public bool RemoveAt(long nodeIndex) => Remove(default, Comparer<TKey>.Default, nodeIndex);

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
			AvlOldNode<TKey, TValue> node = Root;

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
					AvlOldNode<TKey, TValue> left = node.Left;
					AvlOldNode<TKey, TValue> right = node.Right;

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
								AvlOldNode<TKey, TValue> parent = node.Parent;

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
						AvlOldNode<TKey, TValue> successor = right;
					    successor.NodeVisitedDuringChange = true;

						if (successor.Left == null)
						{
							AvlOldNode<TKey, TValue> parent = node.Parent;

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

							AvlOldNode<TKey, TValue> parent = node.Parent;
							AvlOldNode<TKey, TValue> successorParent = successor.Parent;
							AvlOldNode<TKey, TValue> successorRight = successor.Right;
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

		private void DeleteBalance(AvlOldNode<TKey, TValue> node, int balance)
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

				AvlOldNode<TKey, TValue> parent = node.Parent;

				if (parent != null)
				{
					balance = parent.Left == node ? -1 : 1;
				}

				node = parent;
			}
		}

		private static void Replace(AvlOldNode<TKey, TValue> target, AvlOldNode<TKey, TValue> source)
		{
			AvlOldNode<TKey, TValue> left = source.Left;
			AvlOldNode<TKey, TValue> right = source.Right;

			target.Balance = source.Balance;
            target.Key = (TKey)source.Key.CloneLazinator();
			target.Value = (TValue) source.Value.CloneLazinator();
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

        public void Clear()
        {
            Root = null;
        }


        public ILazinatorSplittable SplitOff()
        {
            if (Root.LeftCount == 0 || Root.RightCount == 0)
                return new AvlOldTree<TKey, TValue>() { AllowDuplicates = AllowDuplicates };
            // Create two separate trees, each of them balanced
            var leftNode = Root.Left;
            var rightNode = Root.Right;
            var originalRoot = Root;
            // Now, add the original root's item to the portion of the tree that we are keeping. That will ensure that the tree stays balanced.
            if (leftNode.Count > rightNode.Count)
            {
                Root = rightNode; // Count will automatically adjust
                // We add by index not by key in part because we don't know if a special comparer is used. If we change this, we may need to add a Comparer parameter or alternatively use a custom comparer that forces us to the left-most or right-most node.
                InsertAtIndex(originalRoot.Key, originalRoot.Value, 0);
                leftNode.Parent = null;
                return new AvlOldTree<TKey, TValue>() { AllowDuplicates = AllowDuplicates, Root = leftNode };
            }
            else
            {
                Root = leftNode;
                InsertAtIndex(originalRoot.Key, originalRoot.Value, Root.Count);
                rightNode.Parent = null;
                return new AvlOldTree<TKey, TValue>() { AllowDuplicates = AllowDuplicates, Root = rightNode };
            }
        }

        /// <summary>
        /// Enumerates the nodes of the tree, skipping a specified number of nodes.
        /// </summary>
        /// <param name="skip">The number of nodes to skip</param>
        /// <returns></returns>
        public IEnumerable<AvlOldNode<TKey, TValue>> AsEnumerable(long skip = 0)
        {
            var enumerator = GetEnumerator(skip);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        /// <summary>
        /// Enumerates the keys of the tree, skipping a specified number.
        /// </summary>
        /// <param name="skip">The number of nodes to skip</param>
        /// <returns></returns>
        public IEnumerable<TKey> KeysEnumerable(long skip = 0)
        {
            var enumerator = GetKeyEnumerator(skip);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        /// <summary>
        /// Enumerates the values of the tree, skipping a specified number.
        /// </summary>
        /// <param name="skip">The number of nodes to skip</param>
        /// <returns></returns>
        public IEnumerable<TValue> ValuesEnumerable(long skip = 0)
        {
            var enumerator = GetValueEnumerator(skip);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }


        /// <summary>
        /// Enumerates the keys and values of the tree, skipping a specified number.
        /// </summary>
        /// <param name="skip">The number of nodes to skip</param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<TKey, TValue>> KeyValuePairs(long skip = 0)
        {
            var enumerator = GetKeyValuePairEnumerator(skip);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        /// <summary>
        /// Enumerates the keys and values of the tree, skipping a specified number.
        /// </summary>
        /// <param name="skip">The number of nodes to skip</param>
        /// <returns></returns>
        public IEnumerable<LazinatorKeyValue<TKey, TValue>> LazinatorKeyValues(long skip = 0)
        {
            var enumerator = GetLazinatorKeyValueEnumerator(skip);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

        /// <summary>
        /// Enumerates the nodes of the tree, starting with the lowest key
        /// </summary>
        /// <returns></returns>
        public IEnumerator<AvlOldNode<TKey, TValue>> GetEnumerator()
        {
            var enumerator = new AvlNodeEnumerator<TKey, TValue>(this);
            return enumerator;
        }

        /// <summary>
        /// Enumerates the nodes of the tree, starting at the node with the lowest key
        /// </summary>
        /// <param name="skip">Number of nodes to skip before enumeration</param>
        /// <returns></returns>
        public IEnumerator<AvlOldNode<TKey, TValue>> GetEnumerator(long skip)
        {
            var enumerator = new AvlNodeEnumerator<TKey, TValue>(this, skip);
            return enumerator;
        }

        /// <summary>
        /// Gets the node enumerator. The generic method should usually be used instead.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

        /// <summary>
        /// Enumerates the values in the tree
        /// </summary>
        /// <param name="skip">Number of nodes to skip before enumeration</param>
        /// <returns></returns>
        public IEnumerator<TValue> GetValueEnumerator(long skip = 0)
        {
            return new TransformEnumerator<AvlOldNode<TKey, TValue>, TValue>(new AvlNodeEnumerator<TKey, TValue>(this, skip), x => x.Value);
        }

        /// <summary>
        /// Enumerates the keys in the tree
        /// </summary>
        /// <param name="skip">Number of nodes to skip before enumeration</param>
        /// <returns></returns>
        public IEnumerator<TKey> GetKeyEnumerator(long skip = 0)
        {
            return new TransformEnumerator<AvlOldNode<TKey, TValue>, TKey>(new AvlNodeEnumerator<TKey, TValue>(this, skip), x => x.Key);
        }

        /// <summary>
        /// Enumerates the key/value pairs in the tree as LazinatorKeyValue.
        /// </summary>
        /// <param name="skip">Number of nodes to skip before enumeration</param>
        /// <returns></returns>
        public IEnumerator<LazinatorKeyValue<TKey, TValue>> GetLazinatorKeyValueEnumerator(long skip = 0)
        {
            return new TransformEnumerator<AvlOldNode<TKey, TValue>, LazinatorKeyValue<TKey, TValue>>(new AvlNodeEnumerator<TKey, TValue>(this, skip), x => new LazinatorKeyValue<TKey, TValue>(x.Key, x.Value));
        }

        /// <summary>
        /// Enumerates the key/value pairs in the tree as KeyValuePair.
        /// </summary>
        /// <param name="skip">Number of nodes to skip before enumeration</param>
        /// <returns></returns>
        /// 
        public IEnumerator<KeyValuePair<TKey, TValue>> GetKeyValuePairEnumerator(long skip = 0)
        {
            return new TransformEnumerator<AvlOldNode<TKey, TValue>, KeyValuePair<TKey, TValue>>(new AvlNodeEnumerator<TKey, TValue>(this, skip), x => new KeyValuePair<TKey, TValue>(x.Key, x.Value));
        }

        IEnumerator<TKey> ILazinatorKeyable<TKey, TValue>.GetKeyEnumerator(long skip)
        {
            return GetKeyEnumerator(skip);
        }

        IEnumerator<TValue> ILazinatorKeyable<TKey, TValue>.GetValueEnumerator(long skip)
        {
            return GetValueEnumerator(skip);
        }

        public bool ContainsKeyValue(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKeyValue(TKey key, IComparer<TKey> comparer, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key, IComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        public TValue ValueAtKey(TKey key, IComparer<TKey> comparer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}