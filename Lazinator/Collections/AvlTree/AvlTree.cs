using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Core;
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Collections.Avl
{
	public partial class AvlTree<TKey, TValue> : IEnumerable<AvlNode<TKey, TValue>>, IAvlTree<TKey, TValue> where TKey : ILazinator, new() where TValue : ILazinator, new()
    {
		private IComparer<TKey> _comparer;

		public AvlTree(IComparer<TKey> comparer)
		{
			_comparer = comparer;
		}

		public AvlTree()
		   : this(Comparer<TKey>.Default)
		{

		}

        public void SetComparer(IComparer<TKey> comparer)
        {
            // this method can be used to reset the comparer after deserialization
            _comparer = comparer;
        }

        public IEnumerable<AvlNode<TKey, TValue>> Skip(int i)
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
				if (_comparer.Compare(key, node.Key) < 0)
				{
					node = node.Left;
				}
				else if (_comparer.Compare(key, node.Key) > 0)
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

        public AvlNode<TKey, TValue> SearchMatchOrNext(TKey key)
        {
            AvlNode<TKey, TValue> node = Root;

            while (true)
            {
                if (_comparer.Compare(key, node.Key) < 0)
                {
                    if (node.Left == null)
                        return node;
                    node = node.Left;
                }
                else if (_comparer.Compare(key, node.Key) > 0)
                {
                    if (node.Right == null)
                        return node.GetNextNode();
                    node = node.Right;
                }
                else
                {
                    return node;
                }
            }
        }

        public bool Insert(TKey key, TValue value)
        {
            bool returnVal = InsertHelper(key, value);
            if (Root != null)
            {
                Root.RecalculateCount();
                //Root.Print("", false);
            }
            return returnVal;
        }

        private bool InsertHelper(TKey key, TValue value)
		{
			AvlNode<TKey, TValue> node = Root;

			while (node != null)
			{
			    node.NodeVisitedDuringChange = true;

				int compare = _comparer.Compare(key, node.Key);

				if (compare < 0)
				{
					AvlNode<TKey, TValue> left = node.Left;

					if (left == null)
					{
						node.Left = new AvlNode<TKey, TValue> { Key = key, Value = value, Parent = node, NodeVisitedDuringChange = true };

						InsertBalance(node, 1);

						return true;
					}
					else
					{
						node = left;
					}
				}
				else if (compare > 0)
				{
					AvlNode<TKey, TValue> right = node.Right;

					if (right == null)
					{
						node.Right = new AvlNode<TKey, TValue> { Key = key, Value = value, Parent = node, NodeVisitedDuringChange = true};

						InsertBalance(node, -1);

						return true;
					}
					else
					{
						node = right;
					}
				}
				else
				{
					node.Value = value;

					return false;
				}
			}
			
			Root = new AvlNode<TKey, TValue> { Key = key, Value = value, NodeVisitedDuringChange = true };

			return true;
		}

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
            node.DetachClassFromLazinatorHierarchy();
            right.Left = node;
            rightLeft.DetachClassFromLazinatorHierarchy();
            node.Right = rightLeft;
			node.Parent = right;

			if (rightLeft != null)
			{
				rightLeft.Parent = node;
			}

			if (node == Root)
			{
                right.DetachClassFromLazinatorHierarchy();
				Root = right;
			}
			else if (parent.Right == node)
			{
                right.DetachClassFromLazinatorHierarchy();
				parent.Right = right;
			}
			else
			{
                right.DetachClassFromLazinatorHierarchy();
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
            node.DetachClassFromLazinatorHierarchy();
			left.Right = node;
            leftRight.DetachClassFromLazinatorHierarchy();
			node.Left = leftRight;
			node.Parent = left;

			if (leftRight != null)
			{
				leftRight.Parent = node;
			}

			if (node == Root)
			{
                left.DetachClassFromLazinatorHierarchy();
				Root = left;
			}
			else if (parent.Left == node)
			{
                left.DetachClassFromLazinatorHierarchy();
				parent.Left = left;
			}
			else
			{
                left.DetachClassFromLazinatorHierarchy();
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
            leftRightRight.DetachClassFromLazinatorHierarchy();
			node.Left = leftRightRight;
            leftRightLeft.DetachClassFromLazinatorHierarchy();
			left.Right = leftRightLeft;
            left.DetachClassFromLazinatorHierarchy();
			leftRight.Left = left;
            node.DetachClassFromLazinatorHierarchy();
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
                leftRight.DetachClassFromLazinatorHierarchy();
                Root = leftRight;
			}
			else if (parent.Left == node)
			{
                leftRight.DetachClassFromLazinatorHierarchy();
				parent.Left = leftRight;
			}
			else
			{
                leftRight.DetachClassFromLazinatorHierarchy();
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
            rightLeftLeft.DetachClassFromLazinatorHierarchy();
			node.Right = rightLeftLeft;
            rightLeftRight.DetachClassFromLazinatorHierarchy();
			right.Left = rightLeftRight;
            right.DetachClassFromLazinatorHierarchy();
			rightLeft.Right = right;
            node.DetachClassFromLazinatorHierarchy();
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
                rightLeft.DetachClassFromLazinatorHierarchy();
				Root = rightLeft;
			}
			else if (parent.Right == node)
            {
                rightLeft.DetachClassFromLazinatorHierarchy();
                parent.Right = rightLeft;
			}
			else
			{
                rightLeft.DetachClassFromLazinatorHierarchy();
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

        public bool Delete(TKey key)
        {
            bool returnVal = DeleteHelper(key);
            if (Root != null)
            {
                Root.RecalculateCount();
                //Root.Print("", false);
            }
            return returnVal;
        }

		private bool DeleteHelper(TKey key)
		{
			AvlNode<TKey, TValue> node = Root;

			while (node != null)
			{
			    node.NodeVisitedDuringChange = true;

				if (_comparer.Compare(key, node.Key) < 0)
				{
					node = node.Left;
				}
				else if (_comparer.Compare(key, node.Key) > 0)
				{
					node = node.Right;
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
                            left.DetachClassFromLazinatorHierarchy();
							successor.Left = left;
							successor.Balance = node.Balance;
							left.Parent = successor;

							if (node == Root)
							{
                                successor.DetachClassFromLazinatorHierarchy();
								Root = successor;
							}
							else
							{
								if (parent.Left == node)
								{
                                    successor.DetachClassFromLazinatorHierarchy();
									parent.Left = successor;
								}
								else
                                {
                                    successor.DetachClassFromLazinatorHierarchy();
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
                                successorRight.DetachClassFromLazinatorHierarchy();
								successorParent.Left = successorRight;
							}
							else
                            {
                                successorRight.DetachClassFromLazinatorHierarchy();
                                successorParent.Right = successorRight;
							}

							if (successorRight != null)
							{
								successorRight.Parent = successorParent;
							}

							successor.Parent = parent;
                            left.DetachClassFromLazinatorHierarchy();
							successor.Left = left;
							successor.Balance = node.Balance;
                            right.DetachClassFromLazinatorHierarchy();
							successor.Right = right;
							right.Parent = successor;
							left.Parent = successor;

							if (node == Root)
							{
                                successor.DetachClassFromLazinatorHierarchy();
								Root = successor;
							}
							else
							{
								if (parent.Left == node)
								{
                                    successor.DetachClassFromLazinatorHierarchy();
									parent.Left = successor;
								}
								else
                                {
                                    successor.DetachClassFromLazinatorHierarchy();
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
            left.DetachClassFromLazinatorHierarchy();
			target.Left = left;
            right.DetachClassFromLazinatorHierarchy();
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
	}
}
