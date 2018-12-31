using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Collections.Avl
{
    public partial class AvlBigNodeContents<TKey, TValue> : IAvlBigNodeContents<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
        private AvlNode<LazinatorTuple<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> _CorrespondingNode;
        private IComparer<LazinatorTuple<TKey, TValue>> _Comparer;
        
        public AvlNode<LazinatorTuple<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> ParentNode => _CorrespondingNode.Parent;
        public AvlBigNodeContents<TKey, TValue> ParentContents
        {
            get
            {
                var parent = ParentNode;
                if (parent == null)
                    return null;
                var contents = parent.Value;
                contents.SetCorrespondingNode(parent);
                contents.SetComparer(_Comparer);
                return contents;
            }
        }
        public int NodeIndex => _CorrespondingNode.Index;
        public long ItemsIndex
        {
            get
            {
                if (ParentContents == null)
                    return LeftItemsCount;
                if (_CorrespondingNode.IsLeftNode)
                    return ParentContents.ItemsIndex - RightItemsCount - 1;
                else if (_CorrespondingNode.IsRightNode)
                    return ParentContents.ItemsIndex + LeftItemsCount + 1;
                throw new Exception("Malformed AvlTree.");
            }
        }

        public void SetCorrespondingNode(AvlNode<LazinatorTuple<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> correspondingNode)
        {
            _CorrespondingNode = correspondingNode;
        }

        public void SetComparer(IComparer<LazinatorTuple<TKey, TValue>> comparer)
        {
            // this method can be used to reset the comparer after deserialization
            _Comparer = comparer;
        }

        public AvlBigNodeContents(LazinatorTuple<TKey, TValue> firstItem)
        {
            Items = new SortedLazinatorList<LazinatorTuple<TKey, TValue>>() { AllowDuplicates = false };
            Insert(firstItem);
        }

        public AvlBigNodeContents(IEnumerable<LazinatorTuple<TKey, TValue>> items, IComparer<LazinatorTuple<TKey, TValue>> comparer = null)
        {
            Items = new SortedLazinatorList<LazinatorTuple<TKey, TValue>>() { AllowDuplicates = false };
            foreach (var item in items)
                Items.Insert(item, comparer);
            SelfItemsCount = Items.Count;
        }

        public (int location, bool rejectedAsDuplicate) Insert(LazinatorTuple<TKey, TValue> keyAndValue)
        {
            var result = Items.Insert(keyAndValue);
            SelfItemsCount = Items.Count;
            return result;
        }

        public bool Contains(LazinatorTuple<TKey, TValue> keyAndValue)
        {
            var result = Items.Find(keyAndValue, _Comparer);
            return result.exists;
        }

        public (int location, bool exists) Find(LazinatorTuple<TKey, TValue> keyAndValue)
        {
            var result = Items.Find(keyAndValue, _Comparer);
            return result;
        }

        //public (int location, bool exists) Find(TKey key)
        //{

        //}

        public (int priorLocation, bool existed) Remove(LazinatorTuple<TKey, TValue> keyAndValue)
        {
            (int priorLocation, bool existed) result = Items.RemoveSorted(keyAndValue, _Comparer);
            SelfItemsCount = Items.Count;
            return result;
        }

        public LazinatorTuple<TKey, TValue> GetLastItem()
        {
            int itemsCount = (int) SelfItemsCount;
            if (itemsCount == 0)
                return null;
            return Items[itemsCount - 1].CloneNoBuffer();
        }

        public (AvlBigNodeContents<TKey, TValue> node, int nodeIndex) SplitOffFirstHalf()
        {
            int itemsCount = SelfItemsCount;
            if (itemsCount < 2)
                throw new System.Exception("Insufficient number of items to split.");
            int numToRemove = itemsCount / 2;
            var itemsToRemove = Items.Take(numToRemove).Select(x => x.CloneNoBuffer()).ToList();
            for (int i = 0; i < numToRemove; i++)
                Items.RemoveAt(i);
            SelfItemsCount = Items.Count;
            AvlBigNodeContents<TKey, TValue> node = new AvlBigNodeContents<TKey, TValue>(itemsToRemove);
            return (node, NodeIndex); // new node will be at current node's index. This will result in this node's NodeIndex increasing
        }

        /// <summary>
        /// Updates the node's key when the last item changes. Note that we will only do this in a way that maintains the order of the overall AvlBigNodeTree.
        /// </summary>
        private void UpdateNodeKey()
        {
            _CorrespondingNode.Key = GetLastItem();
        }
    }
}