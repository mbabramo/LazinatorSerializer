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

        protected internal override bool Insert(long index, T value)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            if (Count < BranchingFactor)
            {
                Items.Insert((int)index, value);
                ChangeCount(1);
                return true;
            }
            else
            {
                // We will embed the existing tree items and the new item within leaves that are children of an interior node that we will create. If we're adding at the beginning or the end, we'll keep all of the existing items together; thus, if we continue to add at an end, we can easily fill up the entire level before we'll need to create yet another level. If we're adding in the middle, then we want to leave enough space for more random additions, so we'll separate these items out
 
                // DEBUG3: Suppose that a subtree is filled up. The question is where to add if the current node is full. If the item would be inserted in the middle of a leaf node, then we add by demoting the leaf node and spreading the items out. If not, we look to the parent, seeing if (1) there are empty slots, in which case a leaf node is added before/after, as needed; or (2) the child has a maximum level less than the highest max level, in which case the child and adjacent children of the same or lower max level are consolidated. If we get to the root level, and we can't take either of these approaches, then we consolidate all the children.
                // STEP1: Keep track of max levels for each child.
                // STEP2: Add a ChooseContainersToConsolidate function, accepting a parameter including the child that must be demoted. This returns the range of containers that should be consolidated and demoted, or null if demotion is not possible. Add a DemoteContainer function, which calls the parent's ChooseContainersToConsolidate, and if that fails, the parent's DemoteContainer function. 
                
                bool insertAtBeginning = index == 0;
                bool insertAtEnd = index == BranchingFactor;
                bool separateItemsIntoSeparateLeaves = !(insertAtBeginning || insertAtEnd);
                if (separateItemsIntoSeparateLeaves || CorrespondingTree.Level == 0)
                {
                    var interiorContainer = CorrespondingTree.DemoteLeafContainer(separateItemsIntoSeparateLeaves);
                    interiorContainer.Insert(index, value);
                    return true;
                }
                else
                {
                    return false; // can't handle this at the leaf level, must be handled further up
                }
            }
        }

        protected internal override void RemoveAt(long index)
        {
            if (index > Count - 1 || index < 0)
                throw new ArgumentException();
            Items.RemoveAt((int)index);
            ChangeCount(-1);
        }

        public override string ToString()
        {
            return $"{Count} items: {String.Join(", ", Items)}";
        }
    }
}
