using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections
{
    public partial class LazinatorLinkedList<T> : ILazinatorLinkedList<T>, IList<T>, ILazinatorListable<T> where T : ILazinator
    {
        LazinatorLinkedListNode<T> _lastAccessedNode = null;
        int? _lastAccessedIndex = null;

        public T this[int index]
        {
            get
            {
                LazinatorLinkedListNode<T> current = GetNodeAt(index);
                return current.Value;
            }
            set
            {
                LazinatorLinkedListNode<T> current = GetNodeAt(index);
                current.Value = value;
            }
        }

        private LazinatorLinkedListNode<T> GetNodeAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentException();
            if (_lastAccessedNode == null || _lastAccessedIndex > index)
            {
                _lastAccessedNode = FirstNode;
                _lastAccessedIndex = 0;
            }
            for (int i = (int)_lastAccessedIndex; i < index; i++)
            {
                _lastAccessedNode = _lastAccessedNode.NextNode;
                _lastAccessedIndex++;
            }
            return _lastAccessedNode;
        }

        private LazinatorLinkedListNode<T> LastNode => GetNodeAt(Count - 1);

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void Clear()
        {
            FirstNode = null;
            Count = 0;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int i = 0;
            foreach (T value in this)
            {
                array[arrayIndex + i++] = value;
            }
        }


        public int IndexOf(T item)
        {
            int i = 0;
            foreach (T value in this)
            {
                if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(item, value))
                    return i;
                else
                    i++;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > Count)
                throw new ArgumentException();
            LazinatorLinkedListNode<T> node = new LazinatorLinkedListNode<T>(item);
            if (index == 0)
            {
                node.NextNode = FirstNode;
                FirstNode = node;
            }
            else
            {
                var previous = GetNodeAt(index - 1);
                var currentlyAtIndex = previous.NextNode;
                previous.NextNode = node;
                node.NextNode = currentlyAtIndex;
            }
            Count++;
            _lastAccessedNode = node;
            _lastAccessedIndex = index;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;
            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index > Count - 1)
                throw new ArgumentException();
            if (index == 0)
            {
                FirstNode = FirstNode.NextNode;
                _lastAccessedIndex = null;
                _lastAccessedNode = null;
            }
            else
            {
                var previous = GetNodeAt(index - 1);
                previous.NextNode = previous.NextNode?.NextNode;
                _lastAccessedIndex = index - 1;
                _lastAccessedNode = previous;
            }
            Count--;
        }

        #region Enumeration

        struct NodeEnumerator : IEnumerator<LazinatorLinkedListNode<T>>
        {
            LazinatorLinkedList<T> List;

            public NodeEnumerator(LazinatorLinkedList<T> list)
            {
                List = list;
                Current = default;
            }

            public LazinatorLinkedListNode<T> Current;

            object IEnumerator.Current => Current;

            LazinatorLinkedListNode<T> IEnumerator<LazinatorLinkedListNode<T>>.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (Current == null)
                    Current = List.FirstNode;
                else
                    Current = Current.NextNode;
                return Current != null;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }

        struct ValueEnumerator : IEnumerator<T>
        {
            LazinatorLinkedList<T> List;

            public ValueEnumerator(LazinatorLinkedList<T> list)
            {
                List = list;
                CurrentNode = default;
            }

            public LazinatorLinkedListNode<T> CurrentNode;

            object IEnumerator.Current => CurrentNode;

            public T Current => CurrentNode.Value;

            public bool MoveNext()
            {
                if (CurrentNode == null)
                    CurrentNode = List.FirstNode;
                else
                    CurrentNode = CurrentNode.NextNode;
                return CurrentNode != null;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ValueEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ValueEnumerator(this);
        }

        #endregion

        #region ILazinatorCountableListableFactory 

        public long LongCount => Count;

        public void InsertAt(long index, T item)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            Insert((int)index, item);
        }

        public void RemoveAt(long index)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            RemoveAt((int)index);
        }


        public IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            if (skip > Count || skip < 0)
                throw new ArgumentException();
            if (reverse)
            {
                // very slow since current implementation is singly linked
                for (int i = Count - 1 - (int)skip; i >= 0; i--)
                {
                    yield return this[i];
                }
            }
            else
            {
                for (int i = (int)skip; i < Count; i++)
                {
                    yield return this[i];
                }
            }
        }

        public T GetAt(long index)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            return this[(int)index];
        }

        public void SetAt(long index, T value)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            this[(int)index] = value;
        }

        public bool Any()
        {
            return Count != 0;
        }

        public T First()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return this[0];
        }

        public T FirstOrDefault()
        {
            if (Any())
                return this[0];
            return default(T);
        }

        public T Last()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return this[Count - 1];
        }

        public T LastOrDefault()
        {
            if (Any())
                return this[Count - 1];
            return default(T);
        }

        public virtual ILazinatorSplittable SplitOff()
        {
            LazinatorLinkedList<T> partSplitOff = new LazinatorLinkedList<T>();
            int numToMove = Count / 2;
            for (int i = 0; i < numToMove; i++)
            {
                partSplitOff.Add(this[0]);
                RemoveAt(0);
            }
            return partSplitOff;
        }

        #endregion
    }
}
