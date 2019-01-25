using LazinatorCollections.Interfaces;
using LazinatorCollections.Location;
using LazinatorCollections.Tree;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    public partial class AvlAggregatedTree<T> : AvlIndexableTree<T>, IAvlAggregatedTree<T>, IAggregatedMultivalueContainer<T>, IMultivalueContainer<T> where T : ILazinator, ICountableContainer
    {
        public AvlAggregatedNode<T> AvlAggregatedRoot => (AvlAggregatedNode<T>)Root;
        public AvlAggregatedNode<T> FirstAggregatedNode => (AvlAggregatedNode<T>)FirstNode();
        public AvlAggregatedNode<T> LastAggregatedNode => (AvlAggregatedNode<T>)LastNode();

        public long LongAggregatedCount => (Root as AvlAggregatedNode<T>)?.LongAggregatedCount ?? 0;

        #region Construction

        public AvlAggregatedTree()
        {

        }

        public AvlAggregatedTree(bool allowDuplicates, bool unbalanced) : base(allowDuplicates, unbalanced)
        {
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlAggregatedTree<T>(AllowDuplicates, Unbalanced);
        }

        protected override BinaryNode<T> CreateNode(T value, BinaryNode<T> parent = null)
        {
            return new AvlAggregatedNode<T>()
            {
                Value = value,
                SelfAggregatedCount = value.LongCount,
                Parent = parent
            };
        }

        #endregion

        public (long firstAggregatedIndex, long lastAggregatedIndex) GetAggregatedIndexRange(IContainerLocation locationOfInnerContainer)
        {
            var node = GetNodeFromLocation(locationOfInnerContainer);
            AvlAggregatedNode<T> aggregatedNode = (AvlAggregatedNode<T>)node;
            return (aggregatedNode.FirstAggregatedIndex, aggregatedNode.LastAggregatedIndex);
        }

        public long GetNonaggregatedIndex(long aggregatedIndex)
        {
            var node = GetNodeAtAggregatedIndex(aggregatedIndex);
            return node.Index;
        }

        public (long firstAggregatedIndex, long lastAggregatedIndex) GetAggregatedIndexRange(long aggregatedIndex)
        {
            var node = GetNodeAtAggregatedIndex(aggregatedIndex);
            return (node.FirstAggregatedIndex, node.LastAggregatedIndex);
        }

        public (long nonaggregatedIndex, long firstAggregatedIndex, long lastAggregatedIndex) GetAggregatedIndexInfo(long aggregatedIndex)
        {
            var node = GetNodeAtAggregatedIndex(aggregatedIndex);
            return (node.Index, node.FirstAggregatedIndex, node.LastAggregatedIndex);
        }

        public AvlAggregatedNode<T> GetNodeAtAggregatedIndex(long aggregatedIndex, bool allowAtCount = false)
        {
            ConfirmInAggregatedRangeOrThrow(aggregatedIndex, allowAtCount);
            var node = GetMatchingNode(MultivalueLocationOptions.Any, CompareAggregatedIndexToNodesIndex(aggregatedIndex, MultivalueLocationOptions.Any));
            return (AvlAggregatedNode<T>)node;
        }

        public AvlAggregatedNode<T> GetNodeAtNonaggregatedIndex(long nonaggregatedIndex)
        {
            return (AvlAggregatedNode<T>)GetNodeAtIndex(nonaggregatedIndex);
        }

        protected int CompareAggregatedIndices(long desiredNodeAggregatedIndex, AvlAggregatedNode<T> node, MultivalueLocationOptions whichOne)
        {
            long actualNodeFirstAggregatedIndex = node.FirstAggregatedIndex;
            int compare;
            if (node.ContainsAggregatedIndex(desiredNodeAggregatedIndex))
            {
                compare = 0;
                // The following is needed for insertions. If on an insertion, we return compare = 0, that means we want to replace the item at that location.
                if (whichOne == MultivalueLocationOptions.InsertBeforeFirst)
                    compare = -1;
                else if (whichOne == MultivalueLocationOptions.InsertAfterLast)
                    compare = 1;
            }
            else if (desiredNodeAggregatedIndex < actualNodeFirstAggregatedIndex)
                compare = -1;
            else
                compare = 1;
            return compare;
        }

        private Func<BinaryNode<T>, int> CompareAggregatedIndexToNodesIndex(long aggregatedIndex, MultivalueLocationOptions whichOne)
        {
            return node => CompareAggregatedIndices(aggregatedIndex, (AvlAggregatedNode<T>)node, whichOne);
        }

        private bool ConfirmInAggregatedRange(long aggregatedIndex, bool allowAtCount = false)
        {
            return aggregatedIndex >= 0 && (aggregatedIndex < LongAggregatedCount || (allowAtCount && aggregatedIndex == LongAggregatedCount));
        }

        private void ConfirmInAggregatedRangeOrThrow(long aggregatedIndex, bool allowAtCount = false)
        {
            if (!ConfirmInAggregatedRange(aggregatedIndex, allowAtCount))
                throw new ArgumentException();
        }
    }
}
