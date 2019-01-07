using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tree;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlIndexableTree<T> : AvlTree<T>, IAvlIndexableTree<T>, IIndexableContainer<T> where T : ILazinator
    {

        protected override BinaryNode<T> CreateNode(T value, BinaryNode<T> parent = null)
        {
            return new AvlCountedNode<T>()
            {
                Value = value,
                Parent = parent
            };
        }

        protected int CompareIndices(long desiredNodeIndex, AvlCountedNode<T> node)
        {
            long actualNodeIndex = node.Index;
            int compare;
            if (desiredNodeIndex == actualNodeIndex)
            {
                compare = 0;
            }
            else if (desiredNodeIndex < actualNodeIndex)
                compare = -1;
            else
                compare = 1;
            return compare;
        }

        private Func<BinaryNode<T>, int> CompareIndexToNodesIndex(long index)
        {
            return node => CompareIndices(index, (AvlCountedNode<T>) node);
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
            var node = GetMatchingNode(MultivalueLocationOptions.Any, CompareIndexToNodesIndex(index));
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
            TryInsertSorted(item, MultivalueLocationOptions.Any, CompareIndexToNodesIndex(index));
        }
        public void RemoveAt(long index)
        {
            ConfirmInRangeOrThrow(index, true);
            TryRemoveSorted(MultivalueLocationOptions.Any, CompareIndexToNodesIndex(index));
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
        public (long index, bool insertionNotReplacement) InsertSorted(T item, IComparer<T> comparer) => InsertSorted(item, MultivalueLocationOptions.Any, comparer);
        public (long index, bool insertionNotReplacement) InsertSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var result = TryInsertSortedReturningNode(item, whichOne, comparer);
            var node = ((AvlCountedNode<T>)result.node);
            return (node.Index, result.insertionNotReplacement);
        }
        public (long priorIndex, bool existed) RemoveSorted(T item, IComparer<T> comparer) => RemoveSorted(item, MultivalueLocationOptions.Any, comparer);
        public (long priorIndex, bool existed) RemoveSorted(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var result = TryRemoveSortedReturningNode(item, whichOne, comparer);
            if (result == null)
                return (default, false);
            var node = ((AvlCountedNode<T>)result);
            return (node.Index, true);
        }
    }
}
