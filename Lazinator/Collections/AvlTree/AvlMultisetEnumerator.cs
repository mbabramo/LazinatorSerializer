using System.Collections;
using System.Collections.Generic;
using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.AvlTree
{
    public sealed class AvlMultisetEnumerator<TKey> : IEnumerator<TKey> where TKey : ILazinator, new()
    {
        private AvlNodeKeyEnumerator<LazinatorTuple<TKey, LazinatorWrapperInt>> UnderlyingEnumerator;

        public AvlMultisetEnumerator(AvlNodeKeyEnumerator<LazinatorTuple<TKey, LazinatorWrapperInt>> underlyingEnumerator)
        {
            UnderlyingEnumerator = underlyingEnumerator;
        }

        public TKey Current => UnderlyingEnumerator.Current.Item1;

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