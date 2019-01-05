using Lazinator.Collections.Avl;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlSortedDictionaryFactory<TKey, TValue> : IAvlSortedDictionaryFactory<TKey, TValue>, ILazinatorOrderedKeyableFactory<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILazinatorOrderedKeyableFactory<TKey, TValue> OrderedKeyableFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ILazinatorOrderedKeyable<TKey, TValue> Create()
        {
            return new AvlSortedDictionary<TKey, TValue>(OrderedKeyableFactory);
        }
    }
}