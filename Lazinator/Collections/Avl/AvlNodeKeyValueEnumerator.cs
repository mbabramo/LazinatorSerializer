using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    public struct AvlNodeLazinatorKeyValueEnumerator<TKey, TValue> : IEnumerator<LazinatorKeyValue<TKey, TValue>> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        private AvlNodeEnumerator<TKey, TValue> UnderlyingEnumerator;

        public AvlNodeLazinatorKeyValueEnumerator(AvlNodeEnumerator<TKey, TValue> underlyingEnumerator)
        {
            UnderlyingEnumerator = underlyingEnumerator;
        }

        public LazinatorKeyValue<TKey, TValue> Current => UnderlyingEnumerator.Current.LazinatorKeyValue;

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
