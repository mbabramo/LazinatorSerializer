using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.BigList
{
    public abstract partial class BigListContainer<T> : IBigListContainer<T> where T : ILazinator
    {
        [NonSerialized]
        public BigListTree<T> CorrespondingTree;

        public BigListTree<T> ParentTree => CorrespondingTree.BigListParentTree;

        public BigListContainer<T> ParentContainer => ParentTree?.BigListContainer;

        public int IndexInParentContainer => CorrespondingTree.Index;

        public BigListContainer()
        {
        }

        public BigListContainer(int branchingFactor, BigListTree<T> correspondingTree)
        {
            BranchingFactor = branchingFactor;
            CorrespondingTree = correspondingTree;
        }

        public abstract bool IsLeaf
        {
            get;
        }

        protected internal abstract T Get(long index);

        protected internal abstract void Set(long index, T value);

        protected internal abstract void Insert(long index, T value);

        protected internal abstract void RemoveAt(long index);

        protected internal virtual void ChangeCount(long increment, int? childIndex = null)
        {
            Count += increment;
            ParentContainer?.ChangeCount(increment, IndexInParentContainer);
        }
    }
}
