using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
			_current = _right = _root = root;
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

        private bool ConsiderSkip()
        {
            if (_current != null && SkipPending >= _current.Count && SkipPending > 1)
            {
                //Debug.WriteLine($"Skipping {_current.Count} at {_current.Key}, next action is parent");
                SkipPending -= _current.Count;
                _nextAction = NextAction.Parent;
                return true;
            }

            return false;
        }

        public bool MoveNext()
        {
            ProcessPendingSkips();
            if (SkipPending == -1)
                return true;
            if (_current == null)
                return false;
            return MoveNextHelper();
        }

        private bool MoveNextHelper()
        {
            startAgain: // acceptable use of goto to break out of multiple loops
            switch (_nextAction)
		    {
		        case NextAction.Right:
		            //Debug.WriteLine($"Action right at {_current.Key}");
                    _current = _right;

		            while (_current.Left != null)
		            {
		                //Debug.WriteLine($"Moving left at {_current.Key}");
                        _current = _current.Left;
		                //Debug.WriteLine($"Now at {_current.Key}");
                        if (ConsiderSkip())
		                    goto startAgain;
                    }

		            _right = _current.Right;
		            _nextAction = _right != null ? NextAction.Right : NextAction.Parent;

		            //Debug.WriteLine($"Enumerating {_current.Key}, next action is {_nextAction}");

                    return true;

		        case NextAction.Parent:
		            //Debug.WriteLine($"Taking parent action at {_current.Key}");
                    while (_current.Parent != null)
		            {
		                AvlNode<TKey, TValue> previous = _current;

		                _current = _current.Parent;

		                //Debug.WriteLine($"Now at {_current.Key}");

                        if (_current.Left == previous)
		                {
		                    _right = _current.Right;
		                    _nextAction = _right != null ? NextAction.Right : NextAction.Parent;
		                    //Debug.WriteLine($"Enumerating {_current.Key}, next action is {_nextAction}");
                            return true;
		                }
		            }

		            _nextAction = NextAction.End;

		            //Debug.WriteLine($"Next action is {_nextAction}");

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
