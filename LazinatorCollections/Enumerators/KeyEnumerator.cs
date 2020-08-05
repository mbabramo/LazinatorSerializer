using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;

namespace LazinatorCollections.Enumerators
{
    /// <summary>
    /// An enumerator of keys, constructed from an underlying enumerator of key-value pairs
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    public struct KeyEnumerator<TKey, TValue> : IEnumerator<TKey> where TKey : ILazinator where TValue : ILazinator
    {
        private IEnumerator<KeyValuePair<TKey, TValue>> UnderlyingEnumerator;

        public KeyEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> underlyingEnumerator)
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
