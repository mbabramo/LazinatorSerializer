﻿using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    public struct AvlNodeKeyEnumerator<TKey, TValue> : IEnumerator<TKey> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        private AvlNodeEnumerator<TKey, TValue> UnderlyingEnumerator;

        public AvlNodeKeyEnumerator(AvlNodeEnumerator<TKey, TValue> underlyingEnumerator)
        {
            UnderlyingEnumerator = underlyingEnumerator;
        }

        public TKey Current => UnderlyingEnumerator.Current.Key;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            UnderlyingEnumerator.Dispose();
        }

        public bool MoveNext()
        {
            return UnderlyingEnumerator.MoveNext();
        }

        public void Reset()
        {
            UnderlyingEnumerator.MoveNext();
        }
    }
}
