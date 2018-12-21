using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.BigList
{
    public partial class BigListInteriorContainer<T> : BigListContainer<T>, IBigListInteriorContainer<T> where T : ILazinator
    {
        public BigListInteriorContainer(int maxLeafCount, BigListTree<T> correspondingTree) : base(maxLeafCount, correspondingTree)
        {
            Counts = new LazinatorList<WLong>();
        }

        public override bool IsLeaf => true;

        enum Border
        {
            Within,
            BorderLeftEdge,
            BorderLeftNode,
            BorderRightNode,
            BorderRightEdge
        }

        private (int childIndex, long numInEarlierChildren) FindChildForItemIndex(long index)
        {
            if (index == 0)
                return (0, 0);
            else if (index == Count)
                return (Counts.Count - 1, Count - Counts[Counts.Count - 1]); // this is just past the last item, so we'll return the last child)
            else if (index > Count || index < 0)
                throw new ArgumentException();
            // 5 5 5 ==> index 0-4 are in childIndex 0, 5-9 are in childIndex 1, 10-14 are in childIndex 2
            long count = 0, previousCount = 0;
            int childIndex = -1;
            do
            {
                previousCount = 0;
                count += Counts[childIndex].WrappedValue;
                childIndex++;
            }
            while (count <= index);
            return (childIndex, previousCount);
        }

        private BigListContainer<T> GetChildContainer(int childIndex)
        {
            if (childIndex < 0 || childIndex >= Counts.Count)
                throw new ArgumentException();
            var childTree = (BigListTree<T>) CorrespondingTree.GetChild(childIndex);
            return childTree.BigListContainer;
        }

        protected internal override T Get(long index)
        {
            if (index >= Count)
                throw new ArgumentException();
            (int childIndex, long numInEarlierChildren) = FindChildForItemIndex(index);
            var childContainer = GetChildContainer(childIndex);
            return childContainer.Get(index - numInEarlierChildren);
        }

        protected internal override void Set(long index, T value)
        {
            if (index >= Count)
                throw new ArgumentException();
            (int childIndex, long numInEarlierChildren) = FindChildForItemIndex(index);
            var childContainer = GetChildContainer(childIndex);
            childContainer.Set(index - numInEarlierChildren, value);
        }

        protected internal override void Insert(long index, T value)
        {
            throw new NotImplementedException();
        }

        protected internal override void RemoveAt(long index)
        {
            throw new NotImplementedException();
        }

        protected internal override void ChangeCount(long increment, int? childIndex = null) : base(increment, childIndex)
        {
            Counts[(int)childIndex] += increment;
        }
    }
}
