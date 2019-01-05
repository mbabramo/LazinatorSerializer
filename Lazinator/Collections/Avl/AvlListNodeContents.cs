using Lazinator.Collections.Factories;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Collections.Avl
{
    public partial class AvlListNodeContents<TKey, TValue> : IAvlListNodeContents<TKey, TValue>
        where TKey : ILazinator, IComparable<TKey>
        where TValue : ILazinator
    {
        private AvlNode<LazinatorKeyValue<TKey, TValue>, AvlListNodeContents<TKey, TValue>> _CorrespondingNode;

        private ILazinatorSortableFactory<LazinatorKeyValue<TKey, TValue>> SortableFactory;

        public AvlNode<LazinatorKeyValue<TKey, TValue>, AvlListNodeContents<TKey, TValue>> ParentNode => _CorrespondingNode.Parent;
        public AvlListNodeContents<TKey, TValue> ParentContents
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
        public long NodeIndex => _CorrespondingNode.Index;
        public long ItemsIndex
        {
            get
            {
                if (ParentContents == null)
                    return LeftItemsCount;
                if (_CorrespondingNode.IsLeftNode)
                    return ParentContents.ItemsIndex - (RightItemsCount + 1);
                else if (_CorrespondingNode.IsRightNode)
                    return ParentContents.ItemsIndex + LeftItemsCount + 1;
                throw new Exception("Malformed AvlTree.");
            }
        }

        public long TotalItemsCount => LeftItemsCount + SelfItemsCount + RightItemsCount;

        public void SetCorrespondingNode(AvlNode<LazinatorKeyValue<TKey, TValue>, AvlListNodeContents<TKey, TValue>> correspondingNode)
        {
            _CorrespondingNode = correspondingNode;
        }

        public AvlListNodeContents(LazinatorKeyValue<TKey, TValue> firstItem, ILazinatorSortableFactory<LazinatorKeyValue<TKey, TValue>> sortableFactory)
        {
            Items = sortableFactory.CreateSortable();
            SortableFactory = sortableFactory;
            Insert(firstItem);
        }

        public AvlListNodeContents(ILazinatorSortable<LazinatorKeyValue<TKey, TValue>> items, ILazinatorSortableFactory<LazinatorKeyValue<TKey, TValue>> sortableFactory)
        {
            Items = items;
            SortableFactory = sortableFactory;
        }

        public AvlListNodeContents(IEnumerable<LazinatorKeyValue<TKey, TValue>> items, ILazinatorSortableFactory<LazinatorKeyValue<TKey, TValue>> sortableFactory, IComparer<LazinatorKeyValue<TKey, TValue>> comparer = null)
        {
            Items = sortableFactory.CreateSortable();
            SortableFactory = sortableFactory;
            foreach (var item in items)
                Items.InsertSorted(item, comparer);
            SelfItemsCount = Items.Count;
        }

        public (long location, bool rejectedAsDuplicate) Insert(LazinatorKeyValue<TKey, TValue> keyAndValue)
        {
            var result = Items.InsertSorted(keyAndValue);
            SelfItemsCount = Items.Count;
            return result;
        }

        /// <summary>
        /// Inserts at a particular index. The caller is responsible for ensuring that this retains sort order.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="keyAndValue"></param>
        public void InsertAt(int index, LazinatorKeyValue<TKey, TValue> keyAndValue)
        {
            Items.Insert(index, keyAndValue);
        }

        /// <summary>
        /// Sets the value at a particular index. The caller is responsible for ensuring that this retains sort order.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="keyAndValue"></param>
        public void Set(int index, TValue value)
        {
            var keyValue = Items[index];
            keyValue.Value = value;
            Items[index] = keyValue;
            if (index == SelfItemsCount - 1)
                UpdateNodeKey();
        }

        public bool Contains(LazinatorKeyValue<TKey, TValue> keyAndValue)
        {
            var result = Items.FindSorted(keyAndValue);
            return result.exists;
        }

        public (long location, bool exists) Find(LazinatorKeyValue<TKey, TValue> keyAndValue)
        {
            var result = Items.FindSorted(keyAndValue);
            return result;
        }

        protected internal static IComparer<LazinatorKeyValue<TKey, TValue>> KeyOnlyComparer = LazinatorKeyValue<TKey, TValue>.GetKeyOnlyComparer();
        protected internal static IComparer<LazinatorKeyValue<TKey, TValue>> KeyValueComparer = LazinatorKeyValue<TKey, TValue>.GetKeyValueComparer(Comparer<TKey>.Default, Comparer<TValue>.Default);

        /// <summary>
        /// Find the first item containing the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (long location, bool exists) Find(TKey key)
        {
            var result = Items.FindSorted(new LazinatorKeyValue<TKey, TValue>(key, default), KeyOnlyComparer);
            // DEBUG: Should be unnecessary, since it's implemented in Items.FindSorted
            //if (result.exists)
            //{
            //    bool matches = true;
            //    do
            //    { // make sure we have the first key match
            //        result.location--;
            //        matches = Items[(int) result.location].Key.Equals(key);
            //        if (!matches)
            //            result.location++;
            //    }
            //    while (matches && result.location > 0);
            //}
            return result;
        }

        public (long priorLocation, bool existed) Remove(LazinatorKeyValue<TKey, TValue> keyAndValue) => Remove(keyAndValue, KeyValueComparer);

        public (long priorLocation, bool existed) Remove(LazinatorKeyValue<TKey, TValue> keyAndValue, IComparer<LazinatorKeyValue<TKey, TValue>> comparer)
        {
            (long priorLocation, bool existed) result = Items.RemoveSorted(keyAndValue, comparer);
            SelfItemsCount = Items.Count;
            if (result.priorLocation == SelfItemsCount && SelfItemsCount > 0)
                UpdateNodeKey();
            return result;
        }

        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
            SelfItemsCount = Items.Count;
            if (index == SelfItemsCount && SelfItemsCount > 0)
                UpdateNodeKey();
        }

        public LazinatorKeyValue<TKey, TValue>? GetLastItem()
        {
            int itemsCount = (int) SelfItemsCount;
            if (itemsCount == 0)
                return null;
            return Items[itemsCount - 1].CloneNoBuffer();
        }

        public (AvlListNodeContents<TKey, TValue> node, long nodeIndex) SplitOff()
        {
            var splitOffItems = Items.SplitOff();
            SelfItemsCount = Items.Count;
            AvlListNodeContents<TKey, TValue> node = new AvlListNodeContents<TKey, TValue>((ILazinatorSortable<LazinatorKeyValue<TKey, TValue>>) splitOffItems, SortableFactory);
            return (node, NodeIndex); // new node will be at current node's index. This will result in this node's NodeIndex increasing
        }

        /// <summary>
        /// Updates the node's key when the last item changes. Note that we will only do this in a way that maintains the order of the overall AvlListNodeTree. When there are no items, the node is removed from the tree entirely.
        /// </summary>
        private void UpdateNodeKey()
        {
            _CorrespondingNode.Key = GetLastItem().Value;
        }
    }
}