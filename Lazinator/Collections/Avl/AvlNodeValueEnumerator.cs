using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    public struct AvlNodeValueEnumerator<TValue> : IEnumerator<TValue> where TValue : ILazinator
    {
        private AvlNodeEnumerator<Placeholder, TValue> UnderlyingEnumerator;

        public AvlNodeValueEnumerator(AvlNodeEnumerator<Placeholder, TValue> underlyingEnumerator)
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
