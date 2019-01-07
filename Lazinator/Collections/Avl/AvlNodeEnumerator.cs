using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    public struct AvlNodeEnumerator<T> : IEnumerator<AvlNode<T>> where T : ILazinator where T : ILazinator
    {
		private AvlNode<T> _current;
        private long _index;
        bool _isFirst;

        public AvlNodeEnumerator(AvlNode<T> node)
        {
            _isFirst = true;
            if (node == null)
            {
                _current = null;
                _index = 0;
            }
            else
            {
                _current = node;
                _index = 0;
            }
        }

        public AvlNodeEnumerator(AvlIndexableTree<T> tree, long skip = 0)
		{
            _isFirst = true;
            if (tree?.Root == null || skip >= tree.LongCount)
            {
                _current = null;
                _index = tree.LongCount;
            }
            else
            {
                _current = tree.GetNodeAtIndex(skip);
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

		public AvlNode<T> Current
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
