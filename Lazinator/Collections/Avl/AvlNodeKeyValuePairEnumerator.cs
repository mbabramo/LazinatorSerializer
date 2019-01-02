using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    public struct AvlNodeKeyValuePairEnumerator<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        private AvlNodeEnumerator<TKey, TValue> UnderlyingEnumerator;

        public AvlNodeKeyValuePairEnumerator(AvlNodeEnumerator<TKey, TValue> underlyingEnumerator)
        {
            UnderlyingEnumerator = underlyingEnumerator;
        }

        public KeyValuePair<TKey, TValue> Current => UnderlyingEnumerator.Current.KeyValuePair;

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

