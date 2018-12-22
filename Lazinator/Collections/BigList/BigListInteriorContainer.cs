using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.BigList
{
    public partial class BigListInteriorContainer<T> : BigListContainer<T>, IBigListInteriorContainer<T> where T : ILazinator
    {
        public BigListInteriorContainer(int branchingFactor, BigListTree<T> correspondingTree) : base(branchingFactor, correspondingTree)
        {
            ChildContainerCounts = new LazinatorList<WLong>();
        }

        public int NumChildContainers => ChildContainerCounts.Count;

        public override bool IsLeaf => false;

        private (int childIndex, long numInEarlierChildren) ChildContainerForItemIndex(long index)
        {
            if (index == 0)
                return (0, 0);
            else if (index == Count)
                return (NumChildContainers - 1, Count - ChildContainerCounts[NumChildContainers - 1]); // this is just past the last item, so we'll return the last child)
            else if (index > Count || index < 0)
                throw new ArgumentException();
            // Suppose children counts are 5 5 5 ==> index 0-4 are in childIndex 0, 5-9 are in childIndex 1, 10-14 are in childIndex 2
            long count = 0, previousCount = 0;
            int childIndex = -1;
            do
            {
                previousCount = count;
                childIndex++;
                count += ChildContainerCounts[childIndex].WrappedValue;
            }
            while (count <= index);
            return (childIndex, previousCount);
        }

        private BigListTree<T> GetChildTree(int childIndex)
        {
            if (childIndex < 0 || childIndex >= NumChildContainers)
                throw new ArgumentException();
            var childTree = (BigListTree<T>)CorrespondingTree.GetChild(childIndex);
            return childTree;
        }

        private BigListContainer<T> GetChildContainer(int childIndex)
        {
            BigListTree<T> childTree = GetChildTree(childIndex);
            return childTree.BigListContainer;
        }

        protected internal override T Get(long index)
        {
            if (index >= Count)
                throw new ArgumentException();
            (int childIndex, long numInEarlierChildren) = ChildContainerForItemIndex(index);
            var childContainer = GetChildContainer(childIndex);
            return childContainer.Get(index - numInEarlierChildren);
        }

        protected internal override void Set(long index, T value)
        {
            if (index >= Count)
                throw new ArgumentException();
            (int childIndex, long numInEarlierChildren) = ChildContainerForItemIndex(index);
            var childContainer = GetChildContainer(childIndex);
            childContainer.Set(index - numInEarlierChildren, value);
        }

        protected internal override void Insert(long index, T value)
        {
            if (index > Count)
                throw new ArgumentException();
            if (NumChildContainers == 0)
            {
                CorrespondingTree.InsertChildContainer(new BigListLeafContainer<T>(BranchingFactor, null), 0);
            }
            (int childIndex, long numInEarlierChildren) = ChildContainerForItemIndex(index);

            if (ChildContainerCounts[childIndex] == BranchingFactor)
            { // the child is full, so decide where to put this new item -- maybe in an adjacent child, if one is empty
                bool isAtLeftOfChild = index == numInEarlierChildren;
                bool isAtRightOfChild = index == ChildContainerCounts[childIndex].WrappedValue + numInEarlierChildren;
                if (isAtLeftOfChild)
                {
                    if (childIndex > 0 && ChildContainerCounts[childIndex - 1].WrappedValue < BranchingFactor)
                    { // there is room in child to left -- add it as the last item there
                        var adjacentChildContainer = GetChildContainer(childIndex - 1);
                        adjacentChildContainer.Insert(ChildContainerCounts[childIndex - 1].WrappedValue, value);
                        return;
                    }
                    else
                    {
                        // can we add a new child at childIndex?
                        if (NumChildContainers < BranchingFactor)
                        {
                            CorrespondingTree.InsertChildContainer(new BigListLeafContainer<T>(BranchingFactor, null), childIndex);
                            // insertion will occur below
                        }
                    }
                }
                else if (isAtRightOfChild)
                { // isAtRightOfChild is true
                    if (childIndex < BranchingFactor - 1 && NumChildContainers > childIndex + 1 && ChildContainerCounts[childIndex + 1].WrappedValue < BranchingFactor)
                    { // there is room in child to right -- add it there
                        var adjacentChildContainer = GetChildContainer(childIndex + 1);
                        adjacentChildContainer.Insert(0, value);
                        return;
                    }
                    else
                    {
                        // can we add a new child at childIndex + 1?
                        if (NumChildContainers < BranchingFactor)
                        {
                            CorrespondingTree.InsertChildContainer(new BigListLeafContainer<T>(BranchingFactor, null), childIndex + 1);
                            var newContainer = GetChildContainer(childIndex + 1);
                            newContainer.Insert(0, value);
                            return;
                        }
                    }
                }
            }
            // either the child has enough space, or it doesn't but we couldn't find a way to add in an adjacent child, so we add in this child, adding another level if necessary
            var childContainer = GetChildContainer(childIndex);
            childContainer.Insert(index - numInEarlierChildren, value);
        }

        protected internal override void RemoveAt(long index)
        {
            (int childIndex, long numInEarlierChildren) = ChildContainerForItemIndex(index);
            var childContainer = GetChildContainer(childIndex);
            childContainer.RemoveAt(index - numInEarlierChildren);
            if (ChildContainerCounts[childIndex] == 0)
            {
                ChildContainerCounts.RemoveAt(childIndex);
                CorrespondingTree.RemoveChild(childContainer);
            }
        }

        protected internal override void ChangeCount(long increment, int? childIndex = null) 
        {
            base.ChangeCount(increment, childIndex);
            ChildContainerCounts[(int)childIndex] += increment;
        }
    }
}
