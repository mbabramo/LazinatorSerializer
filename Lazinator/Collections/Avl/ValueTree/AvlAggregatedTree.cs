using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tree;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ValueTree
{
    public partial class AvlAggregatedTree<T> : AvlIndexableTree<T>, IAvlAggregatedTree<T> where T : ILazinator, ICountableContainer
    {
        public AvlAggregatedNode<T> AvlAggregatedRoot => (AvlAggregatedNode<T>)Root;

        public long LongAggregatedCount => (Root as AvlAggregatedNode<T>)?.LongAggregatedCount ?? 0;

        protected int CompareAggregatedIndices(long desiredNodeAggregatedIndex, AvlAggregatedNode<T> node, MultivalueLocationOptions whichOne)
        {
            long actualNodeAggregatedIndex = node.AggregatedIndex;
            int compare;
            if (desiredNodeAggregatedIndex == actualNodeAggregatedIndex)
            {
                compare = 0;
                // The following is needed for insertions. If on an insertion, we return compare = 0, that means we want to replace the item at that location.
                if (whichOne == MultivalueLocationOptions.InsertBeforeFirst)
                    compare = -1;
                else if (whichOne == MultivalueLocationOptions.InsertAfterLast)
                    compare = 1;
            }
            else if (desiredNodeAggregatedIndex < actualNodeAggregatedIndex)
                compare = -1;
            else
                compare = 1;
            return compare;
        }

        private Func<BinaryNode<T>, int> CompareAggregatedIndexToNodesIndex(long index, MultivalueLocationOptions whichOne)
        {
            return node => CompareAggregatedIndices(index, (AvlAggregatedNode<T>)node, whichOne);
        }

        private bool ConfirmInAggregatedRange(long index, bool allowAtCount = false)
        {
            return index >= 0 && (index < LongAggregatedCount || (allowAtCount && index == LongAggregatedCount));
        }

        private void ConfirmInAggregatedRangeOrThrow(long index, bool allowAtCount = false)
        {
            if (!ConfirmInAggregatedRange(index, allowAtCount))
                throw new ArgumentException();
        }

        internal AvlNode<T> GetNodeAtAggregatedIndex(long index)
        {
            ConfirmInAggregatedRangeOrThrow(index);
            var node = GetMatchingNode(MultivalueLocationOptions.Any, CompareAggregatedIndexToNodesIndex(index, MultivalueLocationOptions.Any));
            return (AvlNode<T>)node;
        }
    }
}
