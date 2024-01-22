using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;
using LazinatorAvlCollections.Avl.BinaryTree;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    public partial class AvlIndexableTreeWithNodeType<T, N> : AvlTreeWithNodeType<T, N>, IAvlIndexableTreeWithNodeType<T, N>, IEnumerable<T> where T : ILazinator where N : class, ILazinator, IIndexableNode<T, N>, new()
    {
        public long LongCount => (Root as N)?.LongCount ?? 0;

        #region Construction

        public AvlIndexableTreeWithNodeType() : base(true, false, true)
        {

        }

        public AvlIndexableTreeWithNodeType(bool allowDuplicates, bool unbalanced, bool cacheEnds) : base(allowDuplicates, unbalanced, cacheEnds)
        {
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlIndexableTreeWithNodeType<T, N>(AllowDuplicates, Unbalanced, CacheEnds);
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

        #region Find

        public override (IContainerLocation location, bool found) FindContainerLocation(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var result = base.FindContainerLocation(value, whichOne, comparer);
            if (!result.found)
                return result;
            // Convert result to index
            var node = (N) ((TreeLocation<T, N>)result.location).Node;
            if (node == null)
                return (new IndexLocation(LongCount, LongCount), false);
            return (new IndexLocation(node.Index, LongCount), result.found);
        }

        protected int CompareIndices(long desiredNodeIndex, N node, MultivalueLocationOptions whichOne)
        {
            long actualNodeIndex = node.Index;
            int compare;
            if (desiredNodeIndex == actualNodeIndex)
            {
                compare = 0;
                // The following is needed for insertions. If on an insertion, we return compare = 0, that means we want to replace the item at that location.
                if (whichOne == MultivalueLocationOptions.InsertBeforeFirst)
                    compare = -1;
                else if (whichOne == MultivalueLocationOptions.InsertAfterLast)
                    compare = 1;
            }
            else if (desiredNodeIndex < actualNodeIndex)
                compare = -1;
            else
                compare = 1;
            return compare;
        }

        private Func<N, int> CompareIndexToNodesIndex(long index, MultivalueLocationOptions whichOne)
        {
            return node => CompareIndices(index, node, whichOne);
        }
        
        public (long index, bool exists) FindIndex(T target, IComparer<T> comparer) => FindIndex(target, MultivalueLocationOptions.Any, comparer);
        public (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var result = GetMatchingOrNextNode(target, whichOne, comparer);
            var node = result.node;
            if (result.found)
                return (node.Index, true);
            return (node?.Index ?? LongCount, false);
        }

        #endregion

        #region Access by index

        private bool ConfirmInRange(long index, bool allowAtCount = false)
        {
            return index >= 0 && (index < LongCount || (allowAtCount && index == LongCount));
        }

        private void ConfirmInRangeOrThrow(long index, bool allowAtCount = false)
        {
            if (!ConfirmInRange(index, allowAtCount))
                throw new ArgumentException();
        }

        internal N GetNodeAtIndex(long index)
        {
            ConfirmInRangeOrThrow(index);
            var node = GetMatchingNode(MultivalueLocationOptions.Any, CompareIndexToNodesIndex(index, MultivalueLocationOptions.Any));
            return node;
        }

        public T GetAtIndex(long index)
        {
            N node = GetNodeAtIndex(index);
            return node.Value;
        }

        public void SetAtIndex(long index, T value)
        {
            N node = GetNodeAtIndex(index);
            node.Value = value;
            if (index == 0)
                CachedFirst = value;
            if (index == LongCount - 1)
                CachedLast = value;
        }

        public override T GetAt(IContainerLocation location)
        {
            if (location is IndexLocation indexLocation)
                return GetAtIndex(indexLocation.Index);
            return base.GetAt(location);
        }

        public override void SetAt(IContainerLocation location, T value)
        {
            if (location is IndexLocation indexLocation)
                SetAtIndex(indexLocation.Index, value);
            else
                base.SetAt(location, value);
        }

        public N ReloadNodeWithUpdatedIndex(N original)
        {
            return (N)GetMatchingNode(MultivalueLocationOptions.Any, ComparisonToFollowCompactPath(GetCompactPathToNode(original)));
        }

        #endregion

        #region Insertion

        public override void InsertAt(IContainerLocation location, T item)
        {
            if (location is IndexLocation indexLocation)
            {
                InsertAtIndex(indexLocation.Index, item);
            }
            else
                base.InsertAt(location, item);
        }

        public void InsertAtIndex(long index, T item)
        {
            ConfirmInRangeOrThrow(index, true);
            InsertOrReplace(item, CompareIndexToNodesIndex(index, MultivalueLocationOptions.InsertBeforeFirst));
        }

        public override (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var result = InsertOrReplaceReturningNode(item, whichOne, comparer);
            var node = (N)result.node;
            node = ReloadNodeWithUpdatedIndex(node);
            return (new IndexLocation(node.Index, LongCount), result.insertedNotReplaced);
        }

        #endregion

        #region Removal

        public override void RemoveAt(IContainerLocation location)
        {
            if (location is IndexLocation indexLocation)
            {
                RemoveAt(indexLocation.Index);
            }
            else
                base.RemoveAt(location);
        }

        public void RemoveAt(long index)
        {
            ConfirmInRangeOrThrow(index, true);
            TryRemove(MultivalueLocationOptions.Any, CompareIndexToNodesIndex(index, MultivalueLocationOptions.First));
        }

        public bool TryRemoveAll(T item) => TryRemoveAll(item, Comparer<T>.Default);

        public override IValueContainer<T> SplitOff()
        {
            if (Unbalanced)
                return base.SplitOff();
            if (Root.LeftCount == 0 || Root.RightCount == 0)
                return CreateNewWithSameSettings();
            // Create two separate trees, each of them balanced
            var leftNode = Root.Left;
            var rightNode = Root.Right;
            var originalRoot = Root;
            // Now, add the original root's item to the portion of the tree that we are keeping. That will ensure that the tree stays balanced.
            // We will always split off the left, because that allows for more consistency with AvlListTree.
            Root = rightNode; // Count will automatically adjust
            Root.Parent = null;
            // We add by index not by key in part because we don't know if a special comparer is used. If we change this, we may need to add a Comparer parameter or alternatively use a custom comparer that forces us to the left-most or right-most node.
            InsertAtIndex(0, originalRoot.Value);
            var newContainer = (AvlIndexableTreeWithNodeType<T, N>)CreateNewWithSameSettings();
            newContainer.Root = leftNode;
            newContainer.Root.Parent = null;
            newContainer.SetCached();
            return newContainer;
        }

        #endregion

        #region Enumeration

        public override IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            if (Root == null || skip >= LongCount)
                yield break;
            var node = GetNodeAtIndex(reverse ? LongCount - 1 - skip : skip);
            var enumerator = new NodeEnumerator<T, N>(node, reverse, 0);
            while (enumerator.MoveNext())
                yield return enumerator.Current.Value;
        }

        #endregion
        
    }
}
