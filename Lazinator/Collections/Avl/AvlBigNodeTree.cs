using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlBigNodeTree<TKey, TValue> : /* IEnumerable<AvlNode<TKey, TValue>>, */ IAvlBigNodeTree<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        private IComparer<LazinatorTuple<TKey, TValue>> _comparer;

        public AvlBigNodeTree(IComparer<LazinatorTuple<TKey, TValue>> comparer)
        {
            _comparer = comparer;
            UnderlyingTree = new AvlTree<LazinatorTuple<TKey, TValue>, AvlBigNodeContents<TKey, TValue>>();
        }

        public AvlBigNodeTree()
           : this(Comparer<LazinatorTuple<TKey, TValue>>.Default)
        {

        }

        public void SetComparer(IComparer<LazinatorTuple<TKey, TValue>> comparer)
        {
            // this method can be used to reset the comparer after deserialization
            _comparer = comparer;
        }

        private AvlBigNodeContents<TKey, TValue> GetNodeContents(AvlNode<LazinatorTuple<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> node)
        {
            var v = node.Value;
            v.SetComparer(_comparer);
            v.SetCorrespondingNode(node);
            return v;
        }

        public AvlNode<LazinatorTuple<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> GetNodeByNodeIndex(int nodeIndex)
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

        public (AvlNode<LazinatorTuple<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> node, int indexInNode) GetNodeByItemIndex(long itemIndex)
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

        //public (AvlNode<LazinatorTuple<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> node, int indexInNode) Find(long itemIndex)
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

        public int Count => 0;
    }
    }
