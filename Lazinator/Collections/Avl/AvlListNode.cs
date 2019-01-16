using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlListNode<T> : AvlCountedNode<T>, IAvlListNode<T> where T : ILazinator
    {
        public long LeftAggregate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long SelfAggregate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long RightAggregate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
