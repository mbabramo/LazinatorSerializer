using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace LazinatorCollections.Enumerators
{
    public struct ValueEnumerator<TKey, TValue> : IEnumerator<TValue> where TKey : ILazinator where TValue : ILazinator
    {
        private IEnumerator<KeyValuePair<TKey, TValue>> UnderlyingEnumerator;

        public ValueEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> underlyingEnumerator)
        {
            UnderlyingEnumerator = underlyingEnumerator;
        }

        public TValue Current => UnderlyingEnumerator.Current.Value;

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
