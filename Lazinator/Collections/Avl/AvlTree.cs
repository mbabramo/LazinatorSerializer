using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    public partial class AvlTree<TKey, TValue> : IEnumerable<AvlNode<TKey, TValue>>, IAvlTree<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
		public AvlTree()
		{
		}

        public AvlNode<TKey, TValue> NodeAtIndex(long i)
        {
            ConfirmInRange(i);
            return Skip(i).First();
        }

        private void ConfirmInRange(long i)
        {
            if (i < 0 || i >= Count)
                throw new ArgumentOutOfRangeException();
        }

        public long Count => Root?.Count ?? 0;

        public IEnumerable<AvlNode<TKey, TValue>> Skip(long i)
        {
            var enumerator = new AvlNodeEnumerator<TKey, TValue>(Root);
            enumerator.Skip(i);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }

		public IEnumerator<AvlNode<TKey, TValue>> GetEnumerator()
		{
			var enumerator = new AvlNodeEnumerator<TKey, TValue>(Root);
		    return enumerator;
		}

		public bool Search(TKey key, out TValue value)
		{
			AvlNode<TKey, TValue> node = Root;

			while (node != null)
			{
                int comparison = key.CompareTo(node.Key);
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

        public (AvlNode<TKey, TValue> node, long index, bool found) SearchMatchOrNext(TKey key)
        {
            AvlNode<TKey, TValue> node = Root;
            if (node == null)
                return (null, 0, false);
            long index = node?.LeftCount ?? 0;
            while (true)
            {
                int comparison = key.CompareTo(node.Key);
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
                    if (AllowDuplicateKeys)
                    {
                        bool keyMatches = true;
                        while (keyMatches)
                        {
                            AvlNode<TKey, TValue> previous = node.GetPreviousNode();
                            if (previous == null)
                                keyMatches = false;
                            if (previous != null)
                            {
                                keyMatches = previous.Key.Equals(key);
                                if (keyMatches)
                                {
                                    node = previous;
                                    index--;
                                }
                            }
                        }
                    }
                    return (node, index, true);
                }
            }
        }

        public AvlNode<TKey, TValue> SearchMatchOrNextOrLast(TKey key)
        {
            return SearchMatchOrNext(key).node ?? LastNode();
        }

        public AvlNode<TKey, TValue> LastNode()
        {
            var x = Root;
            while (x.Right != null)
                x = x.Right;
            return x;
        }

        public (bool inserted, long location) Insert(TKey key, TValue value, long? nodeIndex = null)
        {
            var result = InsertHelper(AllowDuplicateKeys, key, value, nodeIndex);
            if (Root != null)
            {
                Root.RecalculateCount();
                //Root.Print("", false);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">The key to insert.</param>
        /// <param name="value">The value to insert</param>
        /// <param name="nodeIndex">If the insertion point is based on an index, the index at which to insert. Null if the insertion point is to be found from the key.</param>
        /// <returns></returns>
        private (bool inserted, long location) InsertHelper(bool skipDuplicateKeys, TKey key, TValue value, long? nodeIndex = null)
		{
            bool DEBUG = false;
            if (DEBUG)
            {
                Root.Print("", false);
            }
			AvlNode<TKey, TValue> node = Root;
            long index = node?.LeftCount ?? 0;
			while (node != null)
            {
                node.NodeVisitedDuringChange = true;
                
                int compare = CompareKeyOrIndexToNode(skipDuplicateKeys, key, node, nodeIndex, index);

                if (compare < 0 || (compare == 0 && nodeIndex != null))
                {
                    AvlNode<TKey, TValue> left = node.Left;

                    if (left == null)
                    {
                        node.Left = new AvlNode<TKey, TValue> { Key = key, Value = value, Parent = node, NodeVisitedDuringChange = true };
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
                    AvlNode <TKey, TValue> right = node.Right;

                    if (right == null)
                    {
                        node.Right = new AvlNode<TKey, TValue> { Key = key, Value = value, Parent = node, NodeVisitedDuringChange = true };

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

            Root = new AvlNode<TKey, TValue> { Key = key, Value = value, NodeVisitedDuringChange = true };

			return (true, 0);
		}

        private int CompareKeyOrIndexToNode(bool skipDuplicateKeys, TKey key, AvlNode<TKey, TValue> node, long? desiredNodeIndex, long actualNodeIndex)
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
                compare = key.CompareTo(node.Key);
            if (compare == 0 && skipDuplicateKeys && desiredNodeIndex == null)
            {
                compare = 1;
            }
            return compare;
        }

        #region Balancing

        private void InsertBalance(AvlNode<TKey, TValue> node, int balance)
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

				AvlNode<TKey, TValue> parent = node.Parent;

				if (parent != null)
				{
					balance = parent.Left == node ? 1 : -1;
				}

				node = parent;
			}
		}

		private AvlNode<TKey, TValue> RotateLeft(AvlNode<TKey, TValue> node)
		{
			AvlNode<TKey, TValue> right = node.Right;
			AvlNode<TKey, TValue> rightLeft = right.Left;
			AvlNode<TKey, TValue> parent = node.Parent;
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

		private AvlNode<TKey, TValue> RotateRight(AvlNode<TKey, TValue> node)
		{
			AvlNode<TKey, TValue> left = node.Left;
			AvlNode<TKey, TValue> leftRight = left.Right;
			AvlNode<TKey, TValue> parent = node.Parent;
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

		private AvlNode<TKey, TValue> RotateLeftRight(AvlNode<TKey, TValue> node)
		{
			AvlNode<TKey, TValue> left = node.Left;
			AvlNode<TKey, TValue> leftRight = left.Right;
			AvlNode<TKey, TValue> parent = node.Parent;
			AvlNode<TKey, TValue> leftRightRight = leftRight.Right;
			AvlNode<TKey, TValue> leftRightLeft = leftRight.Left;
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

		private AvlNode<TKey, TValue> RotateRightLeft(AvlNode<TKey, TValue> node)
		{
			AvlNode<TKey, TValue> right = node.Right;
			AvlNode<TKey, TValue> rightLeft = right.Left;
			AvlNode<TKey, TValue> parent = node.Parent;
			AvlNode<TKey, TValue> rightLeftLeft = rightLeft.Left;
			AvlNode<TKey, TValue> rightLeftRight = rightLeft.Right;
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

        public bool Remove(TKey key, long? nodeIndex = null)
        {
            bool returnVal = DeleteHelper(key, nodeIndex);
            if (Root != null)
            {
                Root.RecalculateCount();
                //Root.Print("", false);
            }
            return returnVal;
        }

		private bool DeleteHelper(TKey key, long? nodeIndex)
		{
			AvlNode<TKey, TValue> node = Root;

            long index = node?.LeftCount ?? 0;
            while (node != null)
			{
			    node.NodeVisitedDuringChange = true;

                int compare = CompareKeyOrIndexToNode(false, key, node, nodeIndex, index);
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
					AvlNode<TKey, TValue> left = node.Left;
					AvlNode<TKey, TValue> right = node.Right;

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
								AvlNode<TKey, TValue> parent = node.Parent;

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
						AvlNode<TKey, TValue> successor = right;
					    successor.NodeVisitedDuringChange = true;

						if (successor.Left == null)
						{
							AvlNode<TKey, TValue> parent = node.Parent;

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

							AvlNode<TKey, TValue> parent = node.Parent;
							AvlNode<TKey, TValue> successorParent = successor.Parent;
							AvlNode<TKey, TValue> successorRight = successor.Right;
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

		private void DeleteBalance(AvlNode<TKey, TValue> node, int balance)
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

				AvlNode<TKey, TValue> parent = node.Parent;

				if (parent != null)
				{
					balance = parent.Left == node ? -1 : 1;
				}

				node = parent;
			}
		}

		private static void Replace(AvlNode<TKey, TValue> target, AvlNode<TKey, TValue> source)
		{
			AvlNode<TKey, TValue> left = source.Left;
			AvlNode<TKey, TValue> right = source.Right;

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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

        #endregion
    }
}
