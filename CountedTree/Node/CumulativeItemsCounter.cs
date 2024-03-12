using CountedTree.NodeBuffers;
using Lazinator.Core;
using System;

namespace CountedTree.Node
{
    public partial class CumulativeItemsCounter<TKey> : ICumulativeItemsCounter<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        /// <summary>
        /// A function providing the total number of items in a subtree
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Func<int, uint> NumItemsInSubtreeFn { get; set; }

        public CumulativeItemsCounter(Func<int, uint> numItemsInSubtreeFn)
        {
            NumItemsInSubtreeFn = numItemsInSubtreeFn;
        }

        public void CalculateCumulativeItemsInSubtreeIfNecessary(int numChildrenPerInternalNode)
        {
            if (CumulativeItemsInSubtreeAscending == null)
                CalculateCumulativeItemsInSubtree(numChildrenPerInternalNode);
        }

        private void CalculateCumulativeItemsInSubtree(int numChildrenPerInternalNode)
        {
            CumulativeItemsInSubtreeAscending = new uint[numChildrenPerInternalNode];
            CumulativeItemsInSubtreeDescending = new uint[numChildrenPerInternalNode];
            uint cumulativeSubtreeItems = 0;
            for (int i = 0; i < numChildrenPerInternalNode; i++)
            {
                cumulativeSubtreeItems += NumItemsInSubtreeFn(i);
                CumulativeItemsInSubtreeAscending[i] = cumulativeSubtreeItems;
            }
            cumulativeSubtreeItems = 0;
            for (int i = numChildrenPerInternalNode - 1; i >= 0; i--)
            {
                cumulativeSubtreeItems += NumItemsInSubtreeFn(i);
                CumulativeItemsInSubtreeDescending[i] = cumulativeSubtreeItems;
            }
        }

        /// <summary>
        /// The number of items in a subtree (including pending changes). Note that this could be negative as a result of duplicative deletes.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public long NumItemsInSubtreeIncludingBuffer(INodeBufferBaseMethods<TKey> buffer, int index)
        {
            uint itemsInSubtree = NumItemsInSubtreeFn(index);
            int netItemChange = buffer.NetItemChangeAtNodeIndex(index);
            return (itemsInSubtree + netItemChange);
        }

        /// <summary>
        /// The number of items in the subtrees below this index (including pending changes stored in the buffer).
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public long CumulativeItemsInSubtreeBelowIndexIncludingBuffer(INodeBufferBaseMethods<TKey> buffer, int index) => (CumulativeItemsInSubtreeAscending[index] - NumItemsInSubtreeFn(index) + buffer.NetItemChangeBelowNodeIndex(index));

        /// <summary>
        /// The number of items in the subtrees above this index (including pending changes stored in the buffer).
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public long CumulativeItemsInSubtreeAboveIndexIncludingBuffer(INodeBufferBaseMethods<TKey> buffer, int index) => (CumulativeItemsInSubtreeDescending[index] - NumItemsInSubtreeFn(index) + buffer.NetItemChangeAboveNodeIndex(index));

    }
}
