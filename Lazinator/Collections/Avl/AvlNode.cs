using Lazinator.Collections.Tree;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlNode<T> : BinaryNode<T>, IAvlNode<T> where T : ILazinator
    {
        [NonSerialized]
        protected bool _NodeVisitedDuringChange;
        internal virtual bool NodeVisitedDuringChange { get; set; }

        public int Balance { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public AvlNode<T> AvlLeft => (AvlNode<T>)Left;
        public AvlNode<T> AvlRight => (AvlNode<T>)Right;
        public AvlNode<T> AvlParent => (AvlNode<T>)Parent;
        protected AvlNode<T> AvlLeftBackingField => (AvlNode<T>)_Left;
        protected AvlNode<T> AvlRightBackingField => (AvlNode<T>)_Right;

    }
}
