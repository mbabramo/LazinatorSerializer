using Lazinator.Collections.Avl;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlDictionaryFactory<TKey, TValue> : IAvlDictionaryFactory<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILazinatorOrderedKeyableFactory<WUint, LazinatorTuple<TKey, TValue>> OrderedKeyableFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public AvlDictionary<TKey, TValue> Create()
        {
            return new AvlDictionary<TKey, TValue>(OrderedKeyableFactory);
        }
    }
}