using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Collections.Avl
{
    public partial class AvlBigNodeContents<TKey, TValue> : IAvlBigNodeContents<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        private AvlNode<LazinatorKeyValue<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> _CorrespondingNode;
        
        public AvlNode<LazinatorKeyValue<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> ParentNode => _CorrespondingNode.Parent;
        public AvlBigNodeContents<TKey, TValue> ParentContents
        {
            get
            {
                var parent = ParentNode;
                if (parent == null)
                    return null;
                var contents = parent.Value;
                contents.SetCorrespondingNode(parent);
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

        public void SetCorrespondingNode(AvlNode<LazinatorKeyValue<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> correspondingNode)
        {
            _CorrespondingNode = correspondingNode;
        }

        public AvlBigNodeContents(LazinatorKeyValue<TKey, TValue> firstItem)
        {
            Items = new SortedLazinatorList<LazinatorKeyValue<TKey, TValue>>() { AllowDuplicates = false };
            Insert(firstItem);
        }

        public AvlBigNodeContents(IEnumerable<LazinatorKeyValue<TKey, TValue>> items, IComparer<LazinatorKeyValue<TKey, TValue>> comparer = null)
        {
            Items = new SortedLazinatorList<LazinatorKeyValue<TKey, TValue>>() { AllowDuplicates = false };
            foreach (var item in items)
                Items.Insert(item, comparer);
            SelfItemsCount = Items.Count;
        }

        public (int location, bool rejectedAsDuplicate) Insert(LazinatorKeyValue<TKey, TValue> keyAndValue)
        {
            var result = Items.Insert(keyAndValue);
            SelfItemsCount = Items.Count;
            return result;
        }

        public bool Contains(LazinatorKeyValue<TKey, TValue> keyAndValue)
        {
            var result = Items.Find(keyAndValue);
            return result.exists;
        }

        public (int location, bool exists) Find(LazinatorKeyValue<TKey, TValue> keyAndValue)
        {
            var result = Items.Find(keyAndValue);
            return result;
        }

        /// <summary>
        /// Find the first item containing the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (int location, bool exists) Find(TKey key)
        {
            var comparer = LazinatorKeyValue<TKey, TValue>.GetKeyOnlyComparer();
            var result = Items.Find(new LazinatorKeyValue<TKey, TValue>(key, default), comparer);
            if (result.exists)
            {
                bool matches = true;
                do
                { // make sure we have the first key match
                    result.location--;
                    matches = Items[result.location].Key.Equals(key);
                    if (!matches)
                        result.location++;
                }
                while (matches && result.location > 0);
            }
            return result;
        }

        public (int priorLocation, bool existed) Remove(LazinatorKeyValue<TKey, TValue> keyAndValue)
        {
            (int priorLocation, bool existed) result = Items.RemoveSorted(keyAndValue);
            SelfItemsCount = Items.Count;
            return result;
        }

        public LazinatorKeyValue<TKey, TValue>? GetLastItem()
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
            _CorrespondingNode.Key = GetLastItem().Value;
        }
    }
}