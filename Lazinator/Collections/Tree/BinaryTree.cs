﻿using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using Lazinator.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections.Tree
{
    /// <summary>
    /// A binary tree. Because it stores a value that need not implement IComparable, and because it is not countable,
    /// direct users of this class must specify a custom comparer when searching, inserting or removing. Subclasses add
    /// functionality for balancing, for accessing items by index, and for adding items that implement IComparable without a custom comparer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BinaryTree<T> : IBinaryTree<T>, IValueContainer<T>, IMultivalueContainer<T>, IEnumerable<T> where T : ILazinator
    {
        public BinaryTree(bool allowDuplicates, bool unbalanced)
        {
            AllowDuplicates = allowDuplicates;
            Unbalanced = unbalanced;
        }

        public virtual IValueContainer<T> CreateNewWithSameSettings()
        {
            return new BinaryTree<T>()
            {
                AllowDuplicates = AllowDuplicates
            };
        }

        public bool Any()
        {
            return Root != null;
        }

        public T Last()
        {
            if (!Any())
                throw new Exception("The tree is empty.");
            return LastNode().Value;
        }

        public T LastOrDefault()
        {
            if (Any())
                return LastNode().Value;
            return default(T);
        }

        public T First()
        {
            if (!Any())
                throw new Exception("The tree is empty.");
            return FirstNode().Value;
        }

        public T FirstOrDefault()
        {
            if (Any())
                return FirstNode().Value;
            return default(T);
        }

        public IContainerLocation FirstLocation() => FirstNode();
        public IContainerLocation LastLocation() => LastNode();
        public virtual T GetAt(IContainerLocation location) => ((BinaryNode<T>)location).Value;
        public virtual void SetAt(IContainerLocation location, T value) => ((BinaryNode<T>)location).Value = value;

        public long Count(T item, IComparer<T> comparer)
        {
            var node = GetMatchingNode(item, MultivalueLocationOptions.First, comparer);
            if (node == null)
                return 0;
            long count = 0;
            while (node != null)
            {
                count++;
                node = node.GetNextNode();
                if (node != null && !node.Value.Equals(item))
                    node = null;
            }
            return count;
        }

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

        public (IContainerLocation location, bool found) FindContainerLocation(T value, IComparer<T> comparer) => FindContainerLocation(value, MultivalueLocationOptions.Any, comparer);

        public (IContainerLocation location, bool found) FindContainerLocation(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer) => GetMatchingOrNextNode(value, whichOne, comparer);

        protected internal (BinaryNode<T> node, bool found) GetMatchingOrNextNode(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer) => GetMatchingOrNextNode(whichOne, node => CompareValueToNode(value, node, whichOne, comparer));

        protected internal (BinaryNode<T> node, bool found) GetMatchingOrNextNode(MultivalueLocationOptions whichOne, Func<BinaryNode<T>, int> comparisonFunc)
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
                if (whichOne == MultivalueLocationOptions.InsertBeforeFirst)
                    compare = -1;
                else if (whichOne == MultivalueLocationOptions.InsertAfterLast)
                    compare = 1;
                else if (whichOne == MultivalueLocationOptions.First)
                {
                    var previousNode = node.GetPreviousNode();
                    if (previousNode != null && comparer.Compare(value, previousNode.Value) == 0)
                        compare = -1;
                }
                else if (whichOne == MultivalueLocationOptions.Last)
                {
                    var nextNode = node.GetNextNode();
                    if (nextNode != null && comparer.Compare(value, nextNode.Value) == 0)
                        compare = 1;
                }
            }

            return compare;
        }

        public bool Contains(T item, IComparer<T> comparer)
        {
            var matchingNode = GetMatchingNode(item, MultivalueLocationOptions.Any, comparer);
            return matchingNode != null;
        }

        public bool GetValue(T item, IComparer<T> comparer, out T match) => GetValue(item, MultivalueLocationOptions.Any, comparer, out match);

        /// <summary>
        /// Returns a matching item. This is useful if the comparer considers only part of the information in the item.
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <param name="comparer">The comparer with which to conduct the search</param>
        /// <param name="match">The matching item, or a default value</param>
        /// <returns>True if a matching item was found</returns>
        public bool GetValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, out T match)
        {
            var matchingNode = GetMatchingNode(item, whichOne, comparer);
            if (matchingNode != null)
            {
                match = matchingNode.Value;
                return true;
            }
            match = default;
            return false;
        }

        public virtual void Clear()
        {
            Root = null;
        }

        /// <summary>
        /// Gets the last node (or none if empty).
        /// </summary>
        /// <returns></returns>
        protected internal BinaryNode<T> FirstNode()
        {
            var x = Root;
            while (x?.Left != null)
                x = x.Left;
            return x;
        }

        /// <summary>
        /// Gets the last node (or none if empty)
        /// </summary>
        /// <returns></returns>
        protected internal BinaryNode<T> LastNode()
        {
            var x = Root;
            while (x?.Right != null)
                x = x.Right;
            return x;
        }

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, IComparer<T> comparer) => InsertOrReplace(item, MultivalueLocationOptions.Any, comparer);

        public virtual (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            CheckAllowDuplicates(whichOne);
            return InsertOrReplace(item, node => CompareValueToNode(item, node, whichOne, comparer));
        }

        protected internal void CheckAllowDuplicates(MultivalueLocationOptions whichOne)
        {
            if (!AllowDuplicates && whichOne != MultivalueLocationOptions.Any)
                throw new Exception("Allowing potential duplicates is forbidden. Use MultivalueLocationOptions.Any");
        }

        protected (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, Func<BinaryNode<T>, int> comparisonFunc) => InsertOrReplaceReturningNode(item, comparisonFunc);

        public (BinaryNode<T> node, bool insertedNotReplaced) InsertOrReplaceReturningNode(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            CheckAllowDuplicates(whichOne);
            return InsertOrReplaceReturningNode(item, node => CompareValueToNode(item, node, whichOne, comparer));
        }

        protected virtual (BinaryNode<T> node, bool insertedNotReplaced) InsertOrReplaceReturningNode(T item, Func<BinaryNode<T>, int> comparisonFunc)
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

        public bool TryRemove(T item, IComparer<T> comparer) => TryRemove(item, MultivalueLocationOptions.Any, comparer);

        public bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => TryRemove(whichOne, node => CompareValueToNode(item, node, whichOne, comparer));

        protected bool TryRemove(MultivalueLocationOptions whichOne, Func<BinaryNode<T>, int> comparisonFunc) => TryRemoveReturningNode(whichOne, comparisonFunc) != null;

        public bool TryRemoveAll(T item, IComparer<T> comparer)
        {
            bool found = false;
            bool foundAny = false;
            do
            {
                found = TryRemove(item, comparer);
                if (found)
                    foundAny = true;
            } while (found);
            return foundAny;
        }

        protected BinaryNode<T> TryRemoveReturningNode(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => TryRemoveReturningNode(whichOne, node => CompareValueToNode(item, node, whichOne, comparer));

        protected virtual BinaryNode<T> TryRemoveReturningNode(MultivalueLocationOptions whichOne, Func<BinaryNode<T>, int> comparisonFunc)
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

        protected internal virtual void Replace(BinaryNode<T> target, BinaryNode<T> source)
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


        /// <summary>
        /// Returns the depth identified when taking a route through the tree. To reduce allocations, this will be a route already taken if possible. This does not guarantee the maximum depth, but this can be used as an effective way to determine when the depth is so great that a split is necessary.
        /// </summary>
        /// <returns></returns>
        public int GetApproximateDepth()
        {
            BinaryNode<T> node = Root;
            int depth = 0;
            while (node != null)
            {
                depth++;
                node = node.SomeChild;
            }
            return depth;
        }

        public virtual IValueContainer<T> SplitOff(IComparer<T> comparer)
        {
            if (Root.Left == null || Root.Right == null)
                return CreateNewWithSameSettings();
            // Create two separate trees
            var leftNode = Root.Left;
            var rightNode = Root.Right;
            var originalRoot = Root;
            Root = rightNode;
            Root.Parent = null;
            InsertOrReplace(originalRoot.Value, comparer);
            var newContainer = (BinaryTree<T>)CreateNewWithSameSettings();
            newContainer.Root = (BinaryNode<T>) leftNode;
            newContainer.Root.Parent = null;
            return newContainer;
        }

        private BinaryNodeEnumerator<T> GetBinaryNodeEnumerator(bool reverse, long skip)
        {
            return new BinaryNodeEnumerator<T>(reverse ? LastNode() : FirstNode(), reverse, skip);
        }

        public virtual IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            var enumerator = GetBinaryNodeEnumerator(reverse, skip);
            while (enumerator.MoveNext())
                yield return enumerator.Current.Value;
        }

        public IEnumerator<T> GetEnumerator(bool reverse = false, long skip = 0)
        {
            return new TransformEnumerator<BinaryNode<T>, T>(GetBinaryNodeEnumerator(reverse, skip), x => x.Value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(false, 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
