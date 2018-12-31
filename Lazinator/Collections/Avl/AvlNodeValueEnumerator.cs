using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    public class AvlNodeValueEnumerator<TValue> : IEnumerator<TValue> where TValue : ILazinator
    {
        private AvlNodeEnumerator<WByte, TValue> UnderlyingEnumerator;

        public AvlNodeValueEnumerator(AvlNodeEnumerator<WByte, TValue> underlyingEnumerator)
        {
            UnderlyingEnumerator = underlyingEnumerator;
        }

        public TValue Current => UnderlyingEnumerator.Current.Value;

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
