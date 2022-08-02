using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;

namespace Lazinator.Collections.Enumerators
{
    /// <summary>
    /// An enumerator of values, constructed from an underlying enumerator of key-value pairs
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
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
