using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    public struct AvlNodeEnumerator<TKey, TValue> : IEnumerator<AvlNode<TKey, TValue>> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
		private AvlNode<TKey, TValue> _current;
        private long _index;
        bool _isFirst;

        public AvlNodeEnumerator(AvlTree<TKey, TValue> tree, long skip = 0)
		{
            _isFirst = true;
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
            if (_isFirst)
                _isFirst = false;
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
