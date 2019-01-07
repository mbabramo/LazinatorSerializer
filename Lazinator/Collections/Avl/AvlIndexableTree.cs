﻿using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tree;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlIndexableTree<T> : AvlTree<T>, IAvlIndexableTree<T>, IIndexableContainer<T>, ILazinatorSplittable where T : ILazinator
    {
        public override IOrderableContainer<T> CreateNewWithSameSettings()
        {
            return new AvlIndexableTree<T>();
        }

        public AvlCountedNode<T> AvlIndexableRoot => (AvlCountedNode<T>)Root;

        protected override BinaryNode<T> CreateNode(T value, BinaryNode<T> parent = null)
        {
            return new AvlCountedNode<T>()
            {
                Value = value,
                SelfCount = 1,
                Parent = parent
            };
        }

        protected int CompareIndices(long desiredNodeIndex, AvlCountedNode<T> node, MultivalueLocationOptions whichOne)
        {
            long actualNodeIndex = node.Index;
            int compare;
            if (desiredNodeIndex == actualNodeIndex)
            {
                compare = 0;
                // The following is needed for insertions. If on an insertion, we return compare = 0, that means we want to replace the item at that location.
                if (whichOne == MultivalueLocationOptions.BeforeFirst)
                    compare = -1;
                else if (whichOne == MultivalueLocationOptions.AfterLast)
                    compare = 1;
            }
            else if (desiredNodeIndex < actualNodeIndex)
                compare = -1;
            else
                compare = 1;
            return compare;
        }

        private Func<BinaryNode<T>, int> CompareIndexToNodesIndex(long index, MultivalueLocationOptions whichOne)
        {
            return node => CompareIndices(index, (AvlCountedNode<T>) node, whichOne);
        }

        public long LongCount => (Root as AvlCountedNode<T>)?.LongCount ?? 0;

        private bool ConfirmInRange(long index, bool allowAtCount = false)
        {
            return index >= 0 && (index < LongCount || (allowAtCount && index == LongCount));
        }
        private void ConfirmInRangeOrThrow(long index, bool allowAtCount = false)
        {
            if (!ConfirmInRange(index, allowAtCount))
                throw new ArgumentException();
        }

        internal AvlNode<T> GetNodeAtIndex(long index)
        {
            ConfirmInRangeOrThrow(index);
            var node = GetMatchingNode(MultivalueLocationOptions.Any, CompareIndexToNodesIndex(index, MultivalueLocationOptions.Any));
            return (AvlNode<T>) node;
        }

        public T GetAt(long index)
        {
            AvlNode<T> node = GetNodeAtIndex(index);
            return node.Value;
        }

        public void SetAt(long index, T value)
        {
            AvlNode<T> node = GetNodeAtIndex(index);
            node.Value = value;
        }

        public void InsertAt(long index, T item)
        {
            ConfirmInRangeOrThrow(index, true);
            TryInsertSorted(item, CompareIndexToNodesIndex(index, MultivalueLocationOptions.BeforeFirst));
        }
        public void RemoveAt(long index)
        {
            ConfirmInRangeOrThrow(index, true);
            TryRemoveSorted(MultivalueLocationOptions.Any, CompareIndexToNodesIndex(index, MultivalueLocationOptions.First));
        }

        public override IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            var enumerator = new AvlNodeEnumerator<T>(this, reverse, skip);
            while (enumerator.MoveNext())
                yield return enumerator.Current.Value;
        }

        public (long index, bool exists) FindSorted(T target, IComparer<T> comparer) => FindSorted(target, MultivalueLocationOptions.Any, comparer);
        public (long index, bool exists) FindSorted(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var result = GetMatchingOrNextNode(target, whichOne, comparer);
            var node = ((AvlCountedNode<T>)result.node);
            if (result.found)
                return (node.Index, true);
            return (node?.Index ?? LongCount, false);
        }
        public (long index, bool insertedNotReplaced) InsertSorted(T item, IComparer<T> comparer) => InsertSorted(item, MultivalueLocationOptions.Any, comparer);
        public (long index, bool insertedNotReplaced) InsertSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var result = TryInsertSortedReturningNode(item, whichOne, comparer);
            var node = ((AvlCountedNode<T>)result.node);
            return (node.Index, result.insertedNotReplaced);
        }
        public (long priorIndex, bool existed) RemoveSorted(T item, IComparer<T> comparer) => RemoveSorted(item, MultivalueLocationOptions.Any, comparer);
        public (long priorIndex, bool existed) RemoveSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var result = TryRemoveSortedReturningNode(item, whichOne, comparer);
            ((AvlCountedNode<T>)Root).DEBUG();
            if (result == null)
                return (default, false);
            var node = ((AvlCountedNode<T>)result);
            return (node.Index, true);
        }

        public ILazinatorSplittable SplitOff()
        {
            if (AvlIndexableRoot.LeftCount == 0 || AvlIndexableRoot.RightCount == 0)
                return new AvlIndexableTree<T>();
            // Create two separate trees, each of them balanced
            var leftNode = AvlIndexableRoot.LeftCountedNode;
            var rightNode = AvlIndexableRoot.RightCountedNode;
            var originalRoot = Root;
            // Now, add the original root's item to the portion of the tree that we are keeping. That will ensure that the tree stays balanced.
            bool splitOffLeft = leftNode.LongCount > rightNode.LongCount;
            if (splitOffLeft)
            {
                Root = rightNode; // Count will automatically adjust
                // We add by index not by key in part because we don't know if a special comparer is used. If we change this, we may need to add a Comparer parameter or alternatively use a custom comparer that forces us to the left-most or right-most node.
                InsertAt(0, originalRoot.Value);
            }
            else
            {
                Root = leftNode;
                InsertAt(AvlIndexableRoot.LongCount, originalRoot.Value);
            }
            var newContainer = (AvlIndexableTree<T>)CreateNewWithSameSettings();
            newContainer.Root = splitOffLeft ? leftNode : rightNode;
            newContainer.Root.Parent = null;
            newContainer.AvlIndexableRoot.ResetIndicesFollowingTreeSplit();
            AvlIndexableRoot.Parent = null;
            AvlIndexableRoot.ResetIndicesFollowingTreeSplit();
            return newContainer;
        }
    }
}
