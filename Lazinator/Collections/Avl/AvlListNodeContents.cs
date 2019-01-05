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
        #region Node access and constructor

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
            SelfItemsCount = Items.LongCount;
        }

        #endregion

        #region Count updating

        public long TotalItemsCount => LeftItemsCount + SelfItemsCount + RightItemsCount;

        public void UpdateSelfItemsCount()
        {
            SelfItemsCount = Items.LongCount;
            ParentContents.UpdateParentCount();
        }

        public void UpdateLeftItemsCount(long updatedValue)
        {
            LeftItemsCount = updatedValue;
            UpdateParentCount();
        }

        public void UpdateRightItemsCount(long relativeChange)
        {
            RightItemsCount += relativeChange;
            UpdateParentCount();
        }

        private void UpdateParentCount()
        {
            var parentContents = ParentContents;
            if (parentContents != null)
            {
                if (_CorrespondingNode.IsLeftNode)
                    parentContents.UpdateLeftItemsCount(TotalItemsCount);
                else
                    parentContents.UpdateRightItemsCount(TotalItemsCount);
            }
        }

        #endregion

        #region Insertions/modifications in own storage

        public (long location, bool rejectedAsDuplicate) Insert(LazinatorKeyValue<TKey, TValue> keyAndValue)
        {
            var result = Items.InsertSorted(keyAndValue);
            if (result.rejectedAsDuplicate == false)
                UpdateSelfItemsCount();
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
            UpdateSelfItemsCount();
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

        public (long priorLocation, bool existed) Remove(LazinatorKeyValue<TKey, TValue> keyAndValue) => Remove(keyAndValue, KeyValueComparer);

        public (long priorLocation, bool existed) Remove(LazinatorKeyValue<TKey, TValue> keyAndValue, IComparer<LazinatorKeyValue<TKey, TValue>> comparer)
        {
            (long priorLocation, bool existed) result = Items.RemoveSorted(keyAndValue, comparer);
            SelfItemsCount = Items.LongCount;
            if (result.priorLocation == SelfItemsCount && SelfItemsCount > 0)
                UpdateNodeKey();
            UpdateSelfItemsCount();
            return result;
        }

        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
            SelfItemsCount = Items.LongCount;
            if (index == SelfItemsCount && SelfItemsCount > 0)
                UpdateNodeKey();
            UpdateSelfItemsCount();
        }

        #endregion

        #region Search within node items

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
            return result;
        }

        #endregion

        #region Changes to node as a whole

        /// <summary>
        /// Updates the node's key when the last item changes. Note that we will only do this in a way that maintains the order of the overall AvlListNodeTree. When there are no items, the node is removed from the tree entirely.
        /// </summary>
        private void UpdateNodeKey()
        {
            _CorrespondingNode.Key = GetLastItem().Value;
        }

        private LazinatorKeyValue<TKey, TValue>? GetLastItem()
        {
            int itemsCount = (int) SelfItemsCount;
            if (itemsCount == 0)
                return null;
            return Items[itemsCount - 1].CloneNoBuffer();
        }

        public (AvlListNodeContents<TKey, TValue> node, long nodeIndex) SplitOff()
        {
            var splitOffItems = Items.SplitOff();
            UpdateSelfItemsCount();
            AvlListNodeContents<TKey, TValue> node = new AvlListNodeContents<TKey, TValue>((ILazinatorSortable<LazinatorKeyValue<TKey, TValue>>) splitOffItems, SortableFactory);
            UpdateNodeKey();
            return (node, NodeIndex); // new node will be at current node's index. This will result in this node's NodeIndex increasing
        }

        #endregion
    }
}