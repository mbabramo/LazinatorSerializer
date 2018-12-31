using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    public class AvlNodeKeyEnumerator<TKey> : IEnumerator<TKey> where TKey : ILazinator, IComparable<TKey>
    {
        private AvlNodeEnumerator<TKey, WByte> UnderlyingEnumerator;

        public AvlNodeKeyEnumerator(AvlNodeEnumerator<TKey, WByte> underlyingEnumerator)
        {
            UnderlyingEnumerator = underlyingEnumerator;
        }

        public TKey Current => UnderlyingEnumerator.Current.Key;

        object IEnumerator.Current => Current;

        public void Skip(int i)
        {
            UnderlyingEnumerator.Skip(i);
        }

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
