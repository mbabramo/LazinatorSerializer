using LazinatorCollections.Interfaces;
using LazinatorCollections.Location;
using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LazinatorCollections.Tree
{
    /// <summary>
    /// A binary tree. Because it stores a value that need not implement IComparable, and because it is not countable,
    /// direct users of this class must specify a custom comparer when searching, inserting or removing. Subclasses add
    /// functionality for balancing, for accessing items by index, and for adding items that implement IComparable without a custom comparer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BinaryTree<T> : IBinaryTree<T>, IValueContainer<T>, IMultivalueContainer<T>, IEnumerable<T> where T : ILazinator
    {
        #region Construction

        public BinaryTree(bool allowDuplicates, bool unbalanced, bool cacheEnds)
        {
            AllowDuplicates = allowDuplicates;
            Unbalanced = unbalanced;
            CacheEnds = cacheEnds;
        }

        public virtual IValueContainer<T> CreateNewWithSameSettings()
        {
            return new BinaryTree<T>()
            {
                AllowDuplicates = AllowDuplicates
            };
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

        #endregion

        #region Access by location

        public BinaryNode<T> GetNodeFromLocation(IContainerLocation location) => ((BinaryTreeLocation<T>)location).BinaryNode;

        public virtual T GetAt(IContainerLocation location) => location.IsAfterContainer ? throw new ArgumentException() : GetNodeFromLocation(location).Value;

        public virtual void SetAt(IContainerLocation location, T value)
        {
            if (location.IsAfterContainer)
                throw new ArgumentException();
            else
            {
                GetNodeFromLocation(location).Value = value;
                ConsiderCacheUpdateAfterChange(location, value);
            }
        }

        protected void ConsiderCacheUpdateAfterChange(BinaryNode<T> node)
        {
            if (!CacheEnds)
                return;
            if (node == null)
                return;
            var location = new BinaryTreeLocation<T>(node);
            ConsiderCacheUpdateAfterChange(location, node.Value);
        }

        protected void ConsiderCacheUpdateAfterChange(IContainerLocation location, T value)
        {
            if (!CacheEnds)
                return;
            if (location.GetPreviousLocation().IsBeforeContainer)
                CachedFirst = value.CloneNoBuffer();
            if (location.GetNextLocation().IsAfterContainer)
                CachedLast = value.CloneNoBuffer();
        }

        protected void ConsiderCacheUpdateAfterRemoval(BinaryNode<T> node)
        {
            if (!CacheEnds)
                return;
            if (node == null)
                return;
            if (Any())
            {
                if (EqualityComparer<T>.Default.Equals(node.Value, CachedFirst))
                    CachedFirst = FirstNode().Value.CloneNoBuffer();
                if (EqualityComparer<T>.Default.Equals(node.Value, CachedLast))
                    CachedLast = LastNode().Value.CloneNoBuffer();
            }
        }

        protected void SetCached()
        {
            if (!CacheEnds)
                return;
            if (Any())
            {
                CachedFirst = FirstNode().Value;
                CachedLast = LastNode().Value;
            }
        }

        #endregion

        #region Size

        public bool Any()
        {
            return Root != null;
        }

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

        #endregion

        #region First and last

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

        public T Last()
        {
            if (!Any())
                throw new Exception("The tree is empty.");
            if (CacheEnds)
                return CachedLast;
            return ((BinaryTreeLocation<T>)LastLocation()).BinaryNode.Value;
        }

        public T LastOrDefault()
        {
            if (Any())
                return Last();
            return default(T);
        }

        public T First()
        {
            if (!Any())
                throw new Exception("The tree is empty.");
            if (CacheEnds)
                return CachedFirst;
            return ((BinaryTreeLocation<T>)FirstLocation()).BinaryNode.Value;
        }

        public T FirstOrDefault()
        {
            if (Any())
                return First();
            return default(T);
        }

        public IContainerLocation FirstLocation() => FirstNode().GetLocation();

        public IContainerLocation LastLocation() => LastNode().GetLocation();

        #endregion

        #region Find by value

        protected BinaryNode<T> GetMatchingNode(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer) => GetMatchingNode(whichOne, node => CompareValueToNode(value, node, whichOne, comparer));

        protected BinaryNode<T> GetMatchingNode(MultivalueLocationOptions whichOne, Func<BinaryNode<T>, int> comparisonFunc)
        {
            var result = GetMatchingOrNextNode(whichOne, comparisonFunc);
            if (result.found)
                return result.node;
            return null;
        }

        public virtual (IContainerLocation location, bool found) FindContainerLocation(T value, IComparer<T> comparer) => FindContainerLocation(value, MultivalueLocationOptions.Any, comparer);

        public virtual (IContainerLocation location, bool found) FindContainerLocation(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var result = GetMatchingOrNextNode(value, whichOne, comparer);
            return (result.node?.GetLocation() ?? new BinaryTreeLocation<T>(null), result.found);
        }

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

        protected internal (BinaryNode<T> node, bool found) GetMatchingOrPreviousNode(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer) => GetMatchingOrPreviousNode(whichOne, node => CompareValueToNode(value, node, whichOne, comparer));

        protected internal (BinaryNode<T> node, bool found) GetMatchingOrPreviousNode(MultivalueLocationOptions whichOne, Func<BinaryNode<T>, int> comparisonFunc)
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
                    {
                        var next = node.GetPreviousNode();
                        return (next, false);
                    }
                    node = node.Left;
                }
                else if (comparison > 0)
                {
                    if (node.Right == null)
                        return (node, false);
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

        #endregion

        #region Path to node

        private static Func<BinaryNode<T>, int> ComparisonToFollowPath(Stack<bool> pathFromRoot)
        {
            return x =>
            {
                if (!pathFromRoot.Any())
                    return 0;
                if (pathFromRoot.Pop())
                    return -1;
                else
                    return 1;

            };
        }

        private static Stack<bool> GetPathToNode(BinaryNode<T> node)
        {
            Stack<bool> pathFromRoot = new Stack<bool>();
            var onPathToNode = node;
            while (onPathToNode.Parent != null)
            {
                pathFromRoot.Push(onPathToNode.Parent.Left == onPathToNode);
                onPathToNode = onPathToNode.Parent;
            }
            return pathFromRoot;
        }

        #endregion

        #region Insertion

        public virtual void InsertAt(IContainerLocation location, T item)
        {
            var node = GetNodeFromLocation(location);
            Func<BinaryNode<T>, int> comparisonFunc;
            if (node == null)
                comparisonFunc = n => 1; // insert after last node
            else
            {
                Stack<bool> pathFromRoot = GetPathToNode(node);
                comparisonFunc = ComparisonToFollowPath(pathFromRoot);
            }
            InsertOrReplaceReturningNode(item, comparisonFunc, true);
        }

        protected void InsertAtStart(T item)
        {
            var result = InsertOrReplaceReturningNode(item, n => -1);
        }

        protected void InsertAtEnd(T item)
        {
            var result = InsertOrReplaceReturningNode(item, n => 1);
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

        protected (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, Func<BinaryNode<T>, int> comparisonFunc)
        {
            var result = InsertOrReplaceReturningNode(item, comparisonFunc);
            return (result.node.GetLocation(), result.insertedNotReplaced);
        }

        public (BinaryNode<T> node, bool insertedNotReplaced) InsertOrReplaceReturningNode(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            CheckAllowDuplicates(whichOne);
            return InsertOrReplaceReturningNode(item, node => CompareValueToNode(item, node, whichOne, comparer));
        }

        protected virtual (BinaryNode<T> node, bool insertedNotReplaced) InsertOrReplaceReturningNode(T item, Func<BinaryNode<T>, int> comparisonFunc, bool alwaysInsert = false)
        {
            BinaryNode<T> node = Root;
            bool alwaysInsertTargetFound = false;
            bool allMovesLeft = true, allMovesRight = true;
            while (node != null)
            {
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
                    BinaryNode<T> left = node.Left;
                    allMovesRight = false;

                    if (left == null)
                    {
                        var childNode = CreateNode(item, node);
                        node.Left = childNode;

                        if (allMovesLeft)
                            ConsiderCacheUpdateAfterChange(node);
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
                    allMovesLeft = false;

                    if (right == null)
                    {
                        var childNode = CreateNode(item, node);
                        node.Right = childNode;

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
                    return (node, false);
                }
            }

            Root = CreateNode(item);

            ConsiderCacheUpdateAfterChange(node);
            return (node, true);
        }

        #endregion

        #region Removal

        public virtual void Clear()
        {
            Root = null;
        }

        public virtual void RemoveAt(IContainerLocation location) => RemoveNode(GetNodeFromLocation(location));

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

            bool allMovesLeft = true, allMovesRight = true;
            while (node != null)
            {
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
                    if (allMovesLeft || allMovesRight)
                        ConsiderCacheUpdateAfterRemoval(node);
                    return node;
                }
            }

            return null;
        }

        public virtual void RemoveNode(BinaryNode<T> node)
        {
            Stack<bool> pathFromRoot = GetPathToNode(node);
            BinaryNode<T> result = TryRemoveReturningNode(MultivalueLocationOptions.Any, ComparisonToFollowPath(pathFromRoot));
            if (result != node)
                throw new Exception("Internal exception on RemoveNode.");
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


        public bool ShouldSplit(long splitThreshold)
        {
            if (this is ICountableContainer countable)
                return countable.LongCount > splitThreshold;
            return GetApproximateDepth() > splitThreshold;
        }

        public bool IsShorterThan(IValueContainer<T> second)
        {
            return GetApproximateDepth() < ((BinaryTree<T>)second).GetApproximateDepth();
        }

        public virtual IValueContainer<T> SplitOff()
        {
            if (Root.Left == null || Root.Right == null)
                return CreateNewWithSameSettings();
            // Create two separate trees
            var leftNode = Root.Left;
            var rightNode = Root.Right;
            var originalRoot = Root;
            Root = rightNode;
            Root.Parent = null;
            InsertAtStart(originalRoot.Value);
            var newContainer = (BinaryTree<T>)CreateNewWithSameSettings();
            newContainer.Root = leftNode;
            newContainer.Root.Parent = null;
            newContainer.SetCached();
            return newContainer;
        }

        #endregion

        #region Enumeration

        private BinaryNodeEnumerator<T> GetBinaryNodeEnumerator(bool reverse, long skip)
        {
            return new BinaryNodeEnumerator<T>(reverse ? LastNode() : FirstNode(), reverse, skip);
        }

        private BinaryNodeEnumerator<T> GetBinaryNodeEnumerator(bool reverse, T startValue, IComparer<T> comparer) 
        {
            return new BinaryNodeEnumerator<T>(reverse ? GetMatchingOrPreviousNode(startValue, MultivalueLocationOptions.Last, comparer).node : GetMatchingOrNextNode(startValue, MultivalueLocationOptions.First, comparer).node, reverse, 0);
        }

        public virtual IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            var enumerator = GetBinaryNodeEnumerator(reverse, skip);
            while (enumerator.MoveNext())
                yield return enumerator.Current.Value;
        }

        public virtual IEnumerable<T> AsEnumerable(bool reverse, T startValue, IComparer<T> comparer)
        {
            var enumerator = GetBinaryNodeEnumerator(reverse, startValue, comparer);
            while (enumerator.MoveNext())
                yield return enumerator.Current.Value;
        }

        public IEnumerator<T> GetEnumerator(bool reverse = false, long skip = 0)
        {
            return new TransformEnumerator<BinaryNode<T>, T>(GetBinaryNodeEnumerator(reverse, skip), x => x.Value);
        }

        public IEnumerator<T> GetEnumerator(bool reverse, T startValue, IComparer<T> comparer)
        {
            return new TransformEnumerator<BinaryNode<T>, T>(GetBinaryNodeEnumerator(reverse, startValue, comparer), x => x.Value);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(false, 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
