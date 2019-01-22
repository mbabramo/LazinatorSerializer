using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Collections.Tree;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ValueTree
{
    public partial class AvlIndexableTree<T> : AvlTree<T>, IAvlIndexableTree<T>, IIndexableValueContainer<T>, IIndexableMultivalueContainer<T> where T : ILazinator
    {
        public AvlCountedNode<T> AvlIndexableRoot => (AvlCountedNode<T>)Root;

        public long LongCount => (Root as AvlCountedNode<T>)?.LongCount ?? 0;

        #region Construction

        public AvlIndexableTree(bool allowDuplicates, bool unbalanced) : base(allowDuplicates, unbalanced)
        {
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlIndexableTree<T>(AllowDuplicates, Unbalanced);
        }

        protected override BinaryNode<T> CreateNode(T value, BinaryNode<T> parent = null)
        {
            return new AvlCountedNode<T>()
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
            var node = (AvlCountedNode<T>) ((BinaryTreeLocation<T>)result.location).BinaryNode;
            if (node == null)
                return (new IndexLocation(LongCount, LongCount), false);
            return (new IndexLocation(node.Index, LongCount), result.found);
        }

        protected int CompareIndices(long desiredNodeIndex, AvlCountedNode<T> node, MultivalueLocationOptions whichOne)
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

        private Func<BinaryNode<T>, int> CompareIndexToNodesIndex(long index, MultivalueLocationOptions whichOne)
        {
            return node => CompareIndices(index, (AvlCountedNode<T>) node, whichOne);
        }
        
        public (long index, bool exists) FindIndex(T target, IComparer<T> comparer) => FindIndex(target, MultivalueLocationOptions.Any, comparer);
        public (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var result = GetMatchingOrNextNode(target, whichOne, comparer);
            var node = ((AvlCountedNode<T>)result.node);
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

        internal AvlNode<T> GetNodeAtIndex(long index)
        {
            ConfirmInRangeOrThrow(index);
            var node = GetMatchingNode(MultivalueLocationOptions.Any, CompareIndexToNodesIndex(index, MultivalueLocationOptions.Any));
            return (AvlNode<T>) node;
        }

        public T GetAtIndex(long index)
        {
            AvlNode<T> node = GetNodeAtIndex(index);
            return node.Value;
        }

        public void SetAtIndex(long index, T value)
        {
            AvlNode<T> node = GetNodeAtIndex(index);
            node.Value = value;
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
            var node = ((AvlCountedNode<T>)result.node);
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
            if (AvlIndexableRoot.LeftCount == 0 || AvlIndexableRoot.RightCount == 0)
                return CreateNewWithSameSettings();
            // Create two separate trees, each of them balanced
            var leftNode = AvlIndexableRoot.LeftCountedNode;
            var rightNode = AvlIndexableRoot.RightCountedNode;
            var originalRoot = Root;
            // Now, add the original root's item to the portion of the tree that we are keeping. That will ensure that the tree stays balanced.
            // We will always split off the left, because that allows for more consistency with AvlListTree.
            Root = rightNode; // Count will automatically adjust
            Root.Parent = null;
            // We add by index not by key in part because we don't know if a special comparer is used. If we change this, we may need to add a Comparer parameter or alternatively use a custom comparer that forces us to the left-most or right-most node.
            InsertAtIndex(0, originalRoot.Value);
            var newContainer = (AvlIndexableTree<T>)CreateNewWithSameSettings();
            newContainer.Root = leftNode;
            newContainer.Root.Parent = null;
            newContainer.AvlIndexableRoot.ResetIndicesFollowingTreeSplit();
            AvlIndexableRoot.ResetIndicesFollowingTreeSplit();
            return newContainer;
        }

        #endregion

        #region Enumeration

        public override IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            var enumerator = new AvlNodeEnumerator<T>(this, reverse, skip);
            while (enumerator.MoveNext())
                yield return enumerator.Current.Value;
        }

        #endregion
        
    }
}
