using Lazinator.Collections.Avl;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlSortedKeyMultivalueTreeFactory<TKey, TValue> : IAvlSortedKeyMultivalueTreeFactory<TKey, TValue>, ISortedKeyMultivalueContainerFactory<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlSortedKeyMultivalueTreeFactory(bool allowDuplicates, bool indexable)
        {
            AllowDuplicates = allowDuplicates;
            Indexable = indexable;
        }

        public ISortedKeyMultivalueContainer<TKey, TValue> Create()
        {
            if (Indexable)
                return new AvlSortedIndexableKeyValueTree<TKey, TValue>(AllowDuplicates);
            else
                return new AvlSortedKeyValueTree<TKey, TValue>(AllowDuplicates);
        }
    }
}
