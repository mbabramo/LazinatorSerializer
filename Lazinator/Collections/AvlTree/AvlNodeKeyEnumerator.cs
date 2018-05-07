using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.AvlTree
{
    public sealed class AvlNodeKeyEnumerator<TKey> : IEnumerator<TKey> where TKey : ILazinator, new()
    {
        private AvlNodeEnumerator<TKey, LazinatorWrapperByte> UnderlyingEnumerator;

        public AvlNodeKeyEnumerator(AvlNodeEnumerator<TKey, LazinatorWrapperByte> underlyingEnumerator)
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
