using Lazinator.Collections.Avl;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlTreeFactory<TKey, TValue> : IAvlTreeFactory<TKey, TValue>, ILazinatorOrderedKeyableFactory<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public ILazinatorOrderedKeyable<TKey, TValue> Create()
        {
            return new AvlTree<TKey, TValue>()
            {
                AllowDuplicates = AllowDuplicates
            };
        }
    }
}
