using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    public partial class AvlBigNodeContents<TKey, TValue> : IAvlBigNodeContents<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
        public AvlNode<LazinatorTuple<TKey, TValue>, AvlBigNodeContents<TKey, TValue>> CorrespondingNode;

        public AvlBigNodeContents(TKey firstKey, TValue firstValue)
        {
            Keys = new LazinatorList<TKey>();
            Values = new LazinatorList<TValue>();
            Keys.Add(firstKey);
            Values.Add(firstValue);
            SelfCount = 1;
        }

        public void Add(TKey key, TValue value)
        {

        }

        public LazinatorTuple<TKey, TValue> GetLastItem()
        {
            int itemsCount = (int) SelfCount;
            if (itemsCount == 0)
                return null;
            return new LazinatorTuple<TKey, TValue>(Keys[itemsCount - 1].CloneNoBuffer(), Values[itemsCount - 1].CloneNoBuffer());
        }

        /// <summary>
        /// Updates the node's key when the last item changes. Note that we will only do this in a way that maintains the order of the overall AvlBigNodeTree.
        /// </summary>
        private void UpdateNodeKey()
        {
            CorrespondingNode.Key = GetLastItem();
        }
    }
}