using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    public struct AvlBigNodeTreeEnumerator<TKey, TValue> : IEnumerator<LazinatorKeyValue<TKey, TValue>> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        private long OverallIndex;
        private long TreeCount;
        private AvlBigNodeTree<TKey, TValue> Tree;
        AvlNode<LazinatorKeyValue<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> CurrentNode;
        AvlBigNodeContents<TKey, TValue> CurrentNodeContents;
        int IndexInNode;

        public AvlBigNodeTreeEnumerator(AvlBigNodeTree<TKey, TValue> tree)
        {
            Tree = tree;
            TreeCount = Tree.ItemsCount;
            OverallIndex = -1;
            CurrentNode = null;
            CurrentNodeContents = null;
            IndexInNode = -1;
        }

        public void Skip(long i)
        {
            OverallIndex = i;
        }

        /// <summary>
        /// Updates the current information by searching the Tree for the correct node and index, based on the OverallIndex.
        /// </summary>
        private void UpdateCurrentLocation()
        {
            var result = Tree.GetNodeByItemIndex(OverallIndex);
            CurrentNode = result.node;
            CurrentNodeContents = Tree.GetNodeContents(CurrentNode);
            IndexInNode = result.indexInNode;
        }

        private void MoveToNextNode()
        {

        }

        public bool MoveNext()
        {
            OverallIndex++;
            if (OverallIndex >= TreeCount)
                return false;
            IndexInNode++;
            if (CurrentNode == null || OverallIndex >= CurrentNodeContents.SelfItemsCount)
                UpdateCurrentLocation();
            return true;
        }
        

        public LazinatorKeyValue<TKey, TValue> Current
        {
            get
            {
                if (OverallIndex >= TreeCount)
                    throw new Exception("Enumeration past end.");
                return CurrentNodeContents.Items[IndexInNode - 1];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
        }

        public void Reset()
        {
            OverallIndex = -1;
            CurrentNode = null;
            CurrentNodeContents = null;
            IndexInNode = -1;
        }
    }
}
