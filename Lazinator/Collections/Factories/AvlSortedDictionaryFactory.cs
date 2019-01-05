using Lazinator.Collections.Avl;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlSortedDictionaryFactory<TKey, TValue> : IAvlSortedDictionaryFactory<TKey, TValue>, ILazinatorOrderedKeyableFactory<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public ILazinatorOrderedKeyable<TKey, TValue> Create()
        {
            return new AvlSortedDictionary<TKey, TValue>(AllowDuplicates, OrderedKeyableFactory);
        }
    }
}