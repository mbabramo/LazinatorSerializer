using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections.Factories;

namespace Lazinator.Collections.Avl
{
    public partial class AvlListNodeTree<TKey, TValue> : IEnumerable<LazinatorKeyValue<TKey, TValue>>, IAvlListNodeTree<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlListNodeTree(int maxItemsPerNode, AvlTreeFactory<LazinatorKeyValue<TKey, TValue>, AvlListNodeContents<TKey, TValue>> factory)
        {
            UnderlyingTree = factory.Create();
            MaxItemsPerNode = maxItemsPerNode;
        }

        public AvlListNodeTree()
        {

        }

        public bool Contains(TKey key)
        {
            var result = GetNodeByKey(key);
            return result.exists;
        }

        public bool Contains(TKey key, IComparer<TKey> comparer, TValue value)
        {
            LazinatorKeyValue<TKey, TValue> keyValue = new LazinatorKeyValue<TKey, TValue>(key, value);
            var result = GetNodeByKeyAndValue(keyValue, comparer);
            return result.exists;
        }

        public void Set(TKey key, TValue value)
        {
            var result = GetNodeByKey(key);
            var contents = GetNodeContents(result.node);
            if (result.exists)
            {
                contents.Set(result.indexInNode, value.CloneNoBuffer());
            }
            else
            {
                LazinatorKeyValue<TKey, TValue> keyValue = new LazinatorKeyValue<TKey, TValue>(key.CloneNoBuffer(), value.CloneNoBuffer());
                contents.InsertAt(result.indexInNode, keyValue);
            }
        }

        public void Insert(TKey key, IComparer<TKey> comparer, TValue value, long? itemIndex = null)
        {
            LazinatorKeyValue<TKey, TValue> keyValue = new LazinatorKeyValue<TKey, TValue>(key, value);
            AvlNode<LazinatorKeyValue<TKey, TValue>, AvlListNodeContents<TKey, TValue>> node;
            int indexInNode;
            if (itemIndex is long itemIndexNonNull)
            {
                var result = GetNodeByItemIndex(itemIndexNonNull);
                if (result.node == null)
                    throw new ArgumentOutOfRangeException();
                node = result.node;
                indexInNode = result.indexInNode;
            }
            else
            {
                var result = GetNodeByKeyAndValue(keyValue, comparer);
                node = result.node;
                indexInNode = result.indexInNode;
            }
            var contents = GetNodeContents(node);
            contents.InsertAt(indexInNode, keyValue);
            if (contents.SelfItemsCount > MaxItemsPerNode)
            {
                var toInsert = contents.SplitOffFirstHalf();
                UnderlyingTree.InsertAtIndex(toInsert.node.GetLastItem().Value, toInsert.node, toInsert.nodeIndex);
            }
        }

        /// <summary>
        /// Deletes the first item with the matching key. Returns true if such an item is found.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        public bool Delete(TKey key, int? itemIndex = null)
        {
            var result = GetNodeByKey(key);
            if (result.exists)
            {
                var contents = GetNodeContents(result.node);
                contents.RemoveAt(result.indexInNode);
                if (contents.SelfItemsCount == 0)
                    UnderlyingTree.Remove(result.node.Key);
                return true;
            }
            return false;
        }

        public bool Remove(TKey key, IComparer<TKey> comparer, TValue value)
        {
            LazinatorKeyValue<TKey, TValue> keyValue = new LazinatorKeyValue<TKey, TValue>(key, value);
            var result = GetNodeByKeyAndValue(keyValue, comparer);
            if (result.exists)
            {
                var contents = GetNodeContents(result.node);
                contents.RemoveAt(result.indexInNode);
                if (contents.SelfItemsCount == 0)
                    UnderlyingTree.Remove(result.node.Key);
                return true;
            }
            return false;
        }

        public AvlListNodeContents<TKey, TValue> GetNodeContents(AvlNode<LazinatorKeyValue<TKey, TValue>, AvlListNodeContents<TKey, TValue>> node)
        {
            var v = node.Value;
            v.SetCorrespondingNode(node);
            return v;
        }

        public AvlNode<LazinatorKeyValue<TKey, TValue>, AvlListNodeContents<TKey, TValue>> GetNodeByNodeIndex(int nodeIndex)
        {
            if (UnderlyingTree.Root == null)
                return null;
            var node = UnderlyingTree.Root;
            long actualIndex = node.LeftCount;
            while (true)
            {
                if (actualIndex == nodeIndex)
                    return node;
                if (actualIndex > nodeIndex)
                {
                    node = node.Right;
                    if (node == null)
                        return null;
                    actualIndex += 1 + node.LeftCount;
                }
                else
                {
                    node = node.Left;
                    if (node == null)
                        return null;
                    actualIndex -= 1 + node.RightCount;
                }
            }
        }

        public (AvlNode<LazinatorKeyValue<TKey, TValue>, AvlListNodeContents<TKey, TValue>> node, int indexInNode) GetNodeByItemIndex(long itemIndex)
        {
            if (UnderlyingTree.Root == null)
                return (null, -1);
            var node = UnderlyingTree.Root;
            var contents = GetNodeContents(node);
            var previousNode = node;
            long actualFirstIndex = contents.LeftItemsCount;
            while (true)
            {
                if (itemIndex >= actualFirstIndex && itemIndex < actualFirstIndex + contents.SelfItemsCount)
                    return (node, (int) (itemIndex - actualFirstIndex));
                if (actualFirstIndex > itemIndex)
                {
                    node = node.Right;
                    if (node == null)
                        return (null, -1);
                    contents = GetNodeContents(node);
                    actualFirstIndex += 1 + contents.LeftItemsCount;
                }
                else
                {
                    node = node.Left;
                    if (node == null)
                        return (null, -1);
                    contents = GetNodeContents(node);
                    actualFirstIndex -= 1 + contents.RightItemsCount;
                }
            }
        }

        public (AvlNode<LazinatorKeyValue<TKey, TValue>, AvlListNodeContents<TKey, TValue>> node, int indexInNode, bool exists) GetNodeByKey(TKey key)
        {
            if (UnderlyingTree.Root == null)
                return (null, -1, false);
            LazinatorKeyValue<TKey, TValue> keyWithDefaultValue = new LazinatorKeyValue<TKey, TValue>(key, default);
            var node = UnderlyingTree.NodeForKey(keyWithDefaultValue, AvlListNodeContents<TKey, TValue>.KeyOnlyComparer);
            var contents = GetNodeContents(node);
            var result = contents.Find(key);
            return (node, result.exists ? (int) result.location : (int) contents.SelfItemsCount, result.exists);
        }

        static IComparer<LazinatorKeyValue<TKey, TValue>> DefaultKeyValueComparer = LazinatorKeyValue<TKey, TValue>.GetKeyValueComparer(Comparer<TKey>.Default, Comparer<TValue>.Default);

        public (AvlNode<LazinatorKeyValue<TKey, TValue>, AvlListNodeContents<TKey, TValue>> node, int indexInNode, bool exists) GetNodeByKeyAndValue(LazinatorKeyValue<TKey, TValue> keyAndValue, IComparer<TKey> comparer)
        {
            if (UnderlyingTree.Root == null)
                return (null, -1, false);
            IComparer<LazinatorKeyValue<TKey, TValue>> keyValueComparer = (comparer == Comparer<TKey>.Default) ? DefaultKeyValueComparer : LazinatorKeyValue<TKey, TValue>.GetKeyValueComparer(comparer, Comparer<TValue>.Default);
            var node = UnderlyingTree.NodeForKey(keyAndValue, keyValueComparer);
            var contents = GetNodeContents(node);
            var result = contents.Find(keyAndValue);
            return (node, result.exists ? (int) result.location : (int) contents.SelfItemsCount, result.exists);
        }

        public IEnumerator<LazinatorKeyValue<TKey, TValue>> GetEnumerator()
        {
            return new AvlListNodeTreeEnumerator<TKey, TValue>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new AvlListNodeTreeEnumerator<TKey, TValue>(this);
        }

        public long ItemsCount
        {
            get
            {
                var root = UnderlyingTree.Root;
                if (root == null)
                    return 0;
                var contents = GetNodeContents(root);
                return contents.TotalItemsCount;
            }
        }
    }
}
