using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
	public sealed class AvlNodeEnumerator<TKey, TValue> : IEnumerator<AvlNode<TKey, TValue>> where TKey : ILazinator, new() where TValue : ILazinator, new()
    {
		private AvlNode<TKey, TValue> _root;
		private NextAction _nextAction;
		private AvlNode<TKey, TValue> _current;
		private AvlNode<TKey, TValue> _right;
        private int SkipPending = 0;

		public AvlNodeEnumerator(AvlNode<TKey, TValue> root)
		{
			_right = _root = root;
			_nextAction = _root == null ? NextAction.End : NextAction.Right;
		}

        public void Skip(int i)
        {
            SkipPending += i;
        }

        private void ProcessPendingSkips()
        {
            if (_nextAction == NextAction.Right)
                ConsiderSkip();

            while (SkipPending > 0)
            {
                bool more = MoveNextHelper();
                if (more)
                    SkipPending--;
                else
                {
                    SkipPending = 0;
                    _current = null;
                }
            }
        }

        private void ConsiderSkip()
        {
            if (_current == null && _right != null)
                _current = _right; // we're at root
            if (SkipPending > 0 && _current?.Left != null)
            {
                // see if we can skip a bunch of nodes
                int leftCount = _current.Left.Count;
                if (leftCount > 0 && SkipPending >= leftCount)
                {
                    SkipPending -= leftCount;
                    _current = _current.Left;
                    _nextAction = NextAction.Parent;
                }
            }
        }

        public bool MoveNext()
        {
            ProcessPendingSkips();
            if (_current == null)
                return false;
            return MoveNextHelper();
        }

        private bool MoveNextHelper()
		{
		    switch (_nextAction)
		    {
		        case NextAction.Right:
                    ConsiderSkip();
                    if (_nextAction == NextAction.Parent)
                        goto parentMove; // do next switch statement

                    _current = _right;

		            while (_current.Left != null)
		            {
                        _current = _current.Left;
		            }

		            _right = _current.Right;
		            _nextAction = _right != null ? NextAction.Right : NextAction.Parent;

		            return true;

		        case NextAction.Parent:
                    parentMove:
		            while (_current.Parent != null)
		            {
		                AvlNode<TKey, TValue> previous = _current;

		                _current = _current.Parent;

		                if (_current.Left == previous)
		                {
		                    _right = _current.Right;
		                    _nextAction = _right != null ? NextAction.Right : NextAction.Parent;

		                    return true;
		                }
		            }

		            _nextAction = NextAction.End;

		            return false;

		        default:
		            return false;
		    }
		}

		public void Reset()
		{
			_right = _root;
			_nextAction = _root == null ? NextAction.End : NextAction.Right;
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

		enum NextAction
		{
			Parent,
			Right,
			End
		}
	}
}
