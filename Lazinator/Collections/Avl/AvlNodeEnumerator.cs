using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    public class AvlNodeEnumerator<TKey, TValue> : IEnumerator<AvlNode<TKey, TValue>> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
		private AvlNode<TKey, TValue> _current;
        private long _index;
        bool isFirst = true;

        public AvlNodeEnumerator(AvlTree<TKey, TValue> tree, long skip = 0)
		{
            if (tree?.Root == null || skip >= tree.Count)
            {
                _current = null;
                _index = tree.Count;
            }
            else
            {
                _current = tree.NodeAtIndex(skip);
                _index = skip;
            }
        }

        public bool MoveNext()
        {
            if (isFirst)
                isFirst = false;
            else
                _current = _current?.GetNextNode();
            if (_current == null)
                return false;
            _index++;
            return true;
        }

		public void Reset()
		{
            throw new NotImplementedException();
		}

		public AvlNode<TKey, TValue> Current
		{
			get
			{
			    return _current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}

		public void Dispose()
		{
		}
	}
}
