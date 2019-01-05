using Lazinator.Collections.Avl;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public class AvlTreeFactory<TKey, TValue> : IAvlTreeFactory<TKey, TValue>/* DEBUG , ILazinatorOrderedKeyableFactory<TKey, TValue> */ where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public bool AllowDuplicateKeys => false;
        public AvlTree<TKey, TValue> Create()
        {
            return new AvlTree<TKey, TValue>();
        }
    }
}
