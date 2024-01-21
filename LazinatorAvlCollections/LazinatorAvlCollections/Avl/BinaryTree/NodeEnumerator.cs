using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Core;

namespace LazinatorAvlCollections.Avl.BinaryTree
{
    public struct NodeEnumerator<T, N> : IEnumerator<N> where T : ILazinator where N : class, ILazinator, INode<T, N>, new()
    {
        private N _current;
        private long _index;
        private bool _reverse;
        bool _isFirst;

        public NodeEnumerator(N firstOrLastNode, bool reverse = false, long skip = 0)
        {
            _isFirst = true;
            _reverse = reverse;
            if (firstOrLastNode == null)
            {
                _current = null;
                _index = 0;
            }
            else
            {
                _current = firstOrLastNode;
                _index = 0;
                while (_index < skip)
                {
                    _index++;
                    if (_current != null)
                        _current = reverse ? _current.GetPreviousNode<T, N>() : _current.GetNextNode<T, N>();
                }
            }
        }
        

        public bool MoveNext()
        {
            if (_isFirst)
                _isFirst = false;
            else
                _current = _reverse ? _current?.GetPreviousNode<T, N>() : _current?.GetNextNode<T, N>();
            if (_current == null)
                return false;
            _index++;
            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public N Current
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
