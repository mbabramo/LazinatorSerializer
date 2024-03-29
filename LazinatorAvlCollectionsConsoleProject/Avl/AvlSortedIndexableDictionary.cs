﻿using LazinatorAvlCollections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl
{
    /// <summary>
    /// A sorted Avl dictionary that also allows access to individual items in the tree by index.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public partial class AvlSortedIndexableDictionary<TKey, TValue> : AvlSortedDictionary<TKey, TValue>, IAvlSortedIndexableDictionary<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlSortedIndexableDictionary()
        {
        }

        public AvlSortedIndexableDictionary(bool allowDuplicates, ContainerFactory factory)
        {
            UnderlyingTree = (ISortedIndexableKeyMultivalueContainer<TKey, TValue>)factory.CreateSortedKeyValueContainer<TKey, TValue>();
            AllowDuplicates = allowDuplicates;
        }

        public AvlSortedIndexableDictionary(bool allowDuplicates, ISortedIndexableKeyMultivalueContainer<TKey, TValue> underlyingTree)
        {
            UnderlyingTree = underlyingTree;
            AllowDuplicates = allowDuplicates;
        }

        public ISortedIndexableKeyMultivalueContainer<TKey, TValue> UnderlyingIndexableTree => (ISortedIndexableKeyMultivalueContainer<TKey, TValue>)UnderlyingTree;

        public (TValue valueIfFound, long index, bool found) FindIndex(TKey key, MultivalueLocationOptions whichOne)
        {
            return UnderlyingIndexableTree.FindIndex(key, whichOne);
        }

        public long KeyValuePairsCount => UnderlyingIndexableTree.LongCount;
    }
}
