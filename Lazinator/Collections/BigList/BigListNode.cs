using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.BigList
{
    public abstract partial class BigListNode<T> : IBigListNode<T> where T : ILazinator
    {
        [NonSerialized]
        public BigListTree<T> CorrespondingTree;

        public BigListNode(int maxLeafCount, BigListTree<T> correspondingTree)
        {
            MaxLeafCount = maxLeafCount;
            CorrespondingTree = correspondingTree;
        }

        public abstract bool IsLeaf
        {
            get;
        }

        protected internal abstract void SetAtIndex(long index, T value);

        protected internal abstract void InsertAtIndex(long index, T value);

        protected internal abstract void RemoveAtIndex(long index);
    }
}
