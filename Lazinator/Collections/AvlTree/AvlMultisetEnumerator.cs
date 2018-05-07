using System.Collections;
using System.Collections.Generic;
using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.AvlTree
{
    public sealed class AvlMultisetEnumerator<TKey> : IEnumerator<TKey> where TKey : ILazinator, new()
    {
        private IEnumerator<LazinatorTuple<TKey, LazinatorWrapperInt>> UnderlyingEnumerator;

        public AvlMultisetEnumerator(IEnumerator<LazinatorTuple<TKey, LazinatorWrapperInt>> underlyingEnumerator)
        {
            UnderlyingEnumerator = underlyingEnumerator;
        }

        public TKey Current => UnderlyingEnumerator.Current.Item1;

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