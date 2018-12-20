using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.BigList
{
    public partial class BigListInteriorNode<T> : BigListNode<T>, IBigListInteriorNode<T> where T : ILazinator
    {
        public BigListInteriorNode(int maxLeafCount, BigListTree<T> correspondingTree) : base(maxLeafCount, correspondingTree)
        {
            Counts = new LazinatorList<WLong>();
        }

        public override bool IsLeaf => true;

        protected internal override void SetAtIndex(long index, T value)
        {
        }

        protected internal override void InsertAtIndex(long index, T value)
        {
        }

        protected internal override void RemoveAtIndex(long index)
        {
        }
    }
}
