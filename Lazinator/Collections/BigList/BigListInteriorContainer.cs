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

        protected internal override T Get(long index)
        {
            throw new NotImplementedException();
        }

        protected internal override void Set(long index, T value)
        {
        }

        protected internal override void Insert(long index, T value)
        {
        }

        protected internal override void RemoveAt(long index)
        {
        }

        protected internal override void ChangeCount(long increment, int? childIndex = null) : base(increment, childIndex)
        {
            Counts[(int)childIndex] += increment;
        }
    }
}
