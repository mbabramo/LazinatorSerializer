using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlBigNodeTree<TKey, TValue> : IEnumerable<LazinatorTuple<TKey, TValue>>, IAvlBigNodeTree<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlBigNodeTree()
        {
            UnderlyingTree = new AvlTree<LazinatorKeyValue<TKey, TValue>, AvlBigNodeContents<TKey, TValue>>();
        }

        private AvlBigNodeContents<TKey, TValue> GetNodeContents(AvlNode<LazinatorKeyValue<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> node)
        {
            var v = node.Value;
            v.SetCorrespondingNode(node);
            return v;
        }

        public AvlNode<LazinatorKeyValue<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> GetNodeByNodeIndex(int nodeIndex)
        {
            if (UnderlyingTree.Root == null)
                return null;
            var node = UnderlyingTree.Root;
            int actualIndex = node.LeftCount;
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

        public (AvlNode<LazinatorKeyValue<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> node, int indexInNode) GetNodeByItemIndex(long itemIndex)
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

        public (AvlNode<LazinatorKeyValue<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> node, int indexInNode, bool exists) GetNodeByKey(TKey key)
        {
            if (UnderlyingTree.Root == null)
                return (null, -1, false);
            LazinatorKeyValue<TKey, TValue> keyWithDefaultValue = new LazinatorKeyValue<TKey, TValue>(key, default);
            var node = UnderlyingTree.SearchMatchOrNextOrLast(keyWithDefaultValue);
            var contents = GetNodeContents(node);
            var result = contents.Find(key);
            return (node, result.exists ? result.location : contents.SelfItemsCount, result.exists);
        }

        public (AvlNode<LazinatorKeyValue<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> node, int indexInNode, bool exists) GetNodeByKeyAndValue(LazinatorKeyValue<TKey, TValue> keyAndValue)
        {
            if (UnderlyingTree.Root == null)
                return (null, -1, false);
            var node = UnderlyingTree.SearchMatchOrNextOrLast(keyAndValue);
            var contents = GetNodeContents(node);
            var result = contents.Find(keyAndValue);
            return (node, result.exists ? result.location : contents.SelfItemsCount, result.exists);
        }

        //public (AvlNode<LazinatorKeyValue<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> node, int indexInNode) Find(long itemIndex)
        //{

        //}

        //public AvlNode<TKey, TValue> this[int i]
        //{
        //    get
        //    {
        //        ConfirmInRange(i);
        //        return default; // Skip(i).First();
        //    }
        //}

        private void ConfirmInRange(int i)
        {
            if (i < 0 || i >= Count)
                throw new ArgumentOutOfRangeException();
        }

        public IEnumerator<LazinatorTuple<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int Count => 0;
    }
    }
