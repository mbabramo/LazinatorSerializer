using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.BigList
{
    public partial class BigListLeafContainer<T> : BigListContainer<T>, IBigListLeafContainer<T> where T : ILazinator
    {
        public BigListLeafContainer(int branchingFactor, BigListTree<T> correspondingTree) : base(branchingFactor, correspondingTree)
        {
            Items = new LazinatorList<T>();
            Count = 0;
        }

        public override bool IsLeaf => true;

        protected internal override T Get(long index)
        {
            if (index > Count - 1 || index < 0)
                throw new ArgumentException();
            return Items[(int)index];
        }

        protected internal override void Set(long index, T value)
        {
            if (index > Count - 1 || index < 0)
                throw new ArgumentException();
            Items[(int)index] = value;
        }

        protected internal override void Insert(long index, T value)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            if (Count < BranchingFactor)
            {
                Items.Insert((int)index, value);
                ChangeCount(1);
            }
            else
            {
                // We will embed the existing tree items and the new item within leaves that are children of an interior node that we will create. If we're adding at the beginning or the end, we'll keep all of the existing items together; thus, if we continue to add at an end, we can easily fill up the entire level before we'll need to create yet another level. If we're adding in the middle, then we want to leave enough space for more random additions, so we'll separate these items out
                bool insertAtBeginning = index == 0;
                bool insertAtEnd = index == BranchingFactor;
                bool separateItemsIntoSeparateLeaves = insertAtBeginning || insertAtEnd;
                var interiorContainer = CorrespondingTree.DemoteLeafContainer(separateItemsIntoSeparateLeaves);
                interiorContainer.Insert(index, value);
            }
        }

        protected internal override void RemoveAt(long index)
        {
            if (index > Count - 1 || index < 0)
                throw new ArgumentException();
            Items.RemoveAt((int)index);
            ChangeCount(-1);
        }
    }
}
