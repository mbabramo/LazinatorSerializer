﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.AvlTree
{
    public partial class AvlSet<TKey> : IAvlSet<TKey>, IEnumerable<TKey> where TKey : ILazinator, new()
    {
        public AvlSet(IComparer<TKey> comparer)
        {
            UnderlyingTree = new AvlTree<TKey, LazinatorWrapperByte>(comparer);
        }

        public AvlSet() : base()
        {
            UnderlyingTree = new AvlTree<TKey, LazinatorWrapperByte>();
        }

        public void SetComparer(IComparer<TKey> comparer)
        {
            UnderlyingTree.SetComparer(comparer);
        }

        public bool Contains(TKey key)
        {
            return UnderlyingTree.Search(key, out _);
        }

        public (bool valueFound, TKey valueIfFound) GetMatchOrNext(TKey key)
        {
            var matchOrNext = UnderlyingTree.SearchMatchOrNext(key);
            return (matchOrNext != null, (matchOrNext == null) ? default(TKey) : matchOrNext.Key);
        }

        public bool Insert(TKey key)
        {
            bool notAlreadyPresent = UnderlyingTree.Insert(key, 0);
            if (notAlreadyPresent)
                Count++;
            return notAlreadyPresent;
        }

        public bool Delete(TKey key)
        {
            bool deleted = UnderlyingTree.Delete(key);
            if (deleted)
                Count--;
            return deleted;
        }

        public IEnumerator GetEnumerator()
        {
            return AsKeyEnumerator();
        }

        IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
        {
            return AsKeyEnumerator();
        }

        public IEnumerator<TKey> AsKeyEnumerator()
        {
            var underlyingEnumerator = UnderlyingTree.GetEnumerator() as AvlNodeEnumerator<TKey, LazinatorWrapperByte>;
            return new AvlNodeKeyEnumerator<TKey>(underlyingEnumerator);
        }

        public IEnumerable<TKey> AsEnumerable()
        {
            var iterator = AsKeyEnumerator();
            while (iterator.MoveNext())
                yield return iterator.Current;
        }
    }
}
