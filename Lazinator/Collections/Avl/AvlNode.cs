using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlNode<T> : IAvlNode<T> where T : ILazinator
    {
        [NonSerialized]
        internal bool NodeVisitedDuringChange;


        public T Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Balance { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public AvlNode<T> Left { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public AvlNode<T> Right { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public AvlNode<T> Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public AvlNode<T> GetNextNode()
        {
            throw new NotImplementedException();
        }

        public AvlNode<T> GetPreviousNode()
        {
            throw new NotImplementedException();
        }
    }
}
