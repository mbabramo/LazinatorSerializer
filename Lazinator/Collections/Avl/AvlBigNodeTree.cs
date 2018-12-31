using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
    public partial class AvlBigNodeTree<TKey, TValue> : /* IEnumerable<AvlNode<TKey, TValue>>, */ IAvlBigNodeTree<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
        private IComparer<TKey> _comparer;

        public AvlBigNodeTree(IComparer<TKey> comparer)
        {
            _comparer = comparer;
        }

        public AvlBigNodeTree()
           : this(Comparer<TKey>.Default)
        {

        }

        public void SetComparer(IComparer<TKey> comparer)
        {
            // this method can be used to reset the comparer after deserialization
            _comparer = comparer;
        }

        //public AvlNode<TKey, TValue> this[int i]
        //{
        //    get
        //    {
        //        ConfirmInRange(i);
        //        return default; // Skip(i).First();
        //    }
        //}

        private void ConfirmInRange(int i)
        {
            if (i < 0 || i >= Count)
                throw new ArgumentOutOfRangeException();
        }

        public int Count => 0;
    }
    }
