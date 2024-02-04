using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using LazinatorAvlCollections.Avl.BinaryTree;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl.ValueTree
{
    /// <summary>
    /// An intermediate class to construct the Avl aggregated tree, containing the logic for addressing aggregated index ranges
    /// (i.e., where a single index into the tree corresponds to a range of indices).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="N"></typeparam>
    public partial class AvlAggregatedTreeWithNodeType<T, N> : AvlIndexableTreeWithNodeType<T, N>, IAvlAggregatedTreeWithNodeType<T, N>, IAggregatedMultivalueContainer<T>, IMultivalueContainer<T> where T : ILazinator, ICountableContainer where N : class, ILazinator, IAggregatedNode<T, N>, new()
    {
        public long LongAggregatedCount => (Root as N)?.LongAggregatedCount ?? 0;

        #region Construction

        public AvlAggregatedTreeWithNodeType(bool allowDuplicates, bool unbalanced) : base(allowDuplicates, unbalanced, false)
        {
        }

        public override IValueContainer<T> CreateNewWithSameSettings()
        {
            return new AvlAggregatedTreeWithNodeType<T, N>(AllowDuplicates, Unbalanced);
        }

        protected override N CreateNode(T value, N parent = null)
        {
            return new N()
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
            N aggregatedNode = (N)node;
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

        public N GetNodeAtAggregatedIndex(long aggregatedIndex, bool allowAtCount = false)
        {
            ConfirmInAggregatedRangeOrThrow(aggregatedIndex, allowAtCount);
            var node = GetMatchingNode(MultivalueLocationOptions.Any, CompareAggregatedIndexToNodesIndex(aggregatedIndex, MultivalueLocationOptions.Any));
            return (N)node;
        }

        public N GetNodeAtNonaggregatedIndex(long nonaggregatedIndex)
        {
            return (N)GetNodeAtIndex(nonaggregatedIndex);
        }

        protected int CompareAggregatedIndices(long desiredNodeAggregatedIndex, N node, MultivalueLocationOptions whichOne)
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

        private Func<N, int> CompareAggregatedIndexToNodesIndex(long aggregatedIndex, MultivalueLocationOptions whichOne)
        {
            return node => CompareAggregatedIndices(aggregatedIndex, (N)node, whichOne);
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
