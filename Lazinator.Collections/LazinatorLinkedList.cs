using Lazinator.Collections.Enumerators;
using Lazinator.Collections.Extensions;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Collections
{
    /// <summary>
    /// A linked list of Lazinator objects. Preferable to a regular LazinatorList only where the number of items is expected to be very small.
    /// The linked list includes methods to insert items in a sorted way according to some comparer, but it does not enforce that the client
    /// always do so. If enforcement of sorting is desired, using LazinatorSortedLinkedList.
    /// There is an option to specify whether the linked list may contain duplicates, if the comparer methods are used.
    /// </summary>
    /// <typeparam name="T">The type of item stored</typeparam>
    public partial class LazinatorLinkedList<T> : ILazinatorLinkedList<T>, IList<T>, ILazinatorListable<T>, IIndexableMultivalueContainer<T>, IMultilevelReporter where T : ILazinator
    {
        LazinatorLinkedListNode<T> _lastAccessedNode = null;
        int? _lastAccessedIndex = null;

        public LazinatorLinkedList(bool allowDuplicates = true)
        {
            AllowDuplicates = allowDuplicates;
        }

        public virtual IValueContainer<T> CreateNewWithSameSettings()
        {
            return new LazinatorLinkedList<T>(AllowDuplicates);
        }

        public override string ToString()
        {
            var firstTen = this.Take(11).ToArray();
            bool moreThanTen = false;
            if (firstTen.Length == 11)
            {
                moreThanTen = true;
                firstTen = this.Take(10).ToArray();
            }
            return $"[{String.Join(", ", firstTen)}{(moreThanTen ? ", ..." : "")}]";
        }

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
                ConsiderMultilevelReport(index);
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

        public virtual bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public bool Contains(T item, IComparer<T> comparer)
        {
            var result = FindContainerLocation(item, comparer);
            return result.found;
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

        public virtual void InsertAt(IContainerLocation location, T item) => InsertAtIndex(location.IsAfterContainer ? Count : (int)((IndexLocation)location).Index, item);

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
            ConsiderMultilevelReport(index);
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;
            RemoveAt(index);
            return true;
        }

        public void RemoveAt(IContainerLocation location) => RemoveAt((int)((IndexLocation)location).Index);

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
            ConsiderMultilevelReport(index);
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

        public IEnumerator<T> GetEnumerator(bool reverse = false, long skip = 0)
        {
            if (reverse == false && skip == 0)
                return new ValueEnumerator(this);
            return new ListableEnumerator<T>(this, reverse, skip);
        }

        public IEnumerator<T> GetEnumerator(bool reverse, T startValue, IComparer<T> comparer) => this.MultivalueAsEnumerable<LazinatorLinkedList<T>, T>(reverse, startValue, comparer).GetEnumerator();

        public IEnumerable<T> AsEnumerable(bool reverse, T startValue, IComparer<T> comparer) => this.MultivalueAsEnumerable<LazinatorLinkedList<T>, T>(reverse, startValue, comparer);

        #endregion

        #region ILazinatorCountableListableFactory 

        public long LongCount => Count;

        public void InsertAtIndex(long index, T item)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            Insert((int)index, item);
        }

        public void RemoveAtIndex(long index)
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

        public T GetAtIndex(long index)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            return this[(int)index];
        }

        public void SetAtIndex(long index, T value)
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

        public IContainerLocation FirstLocation() => new IndexLocation(0, LongCount);
        public IContainerLocation LastLocation() => new IndexLocation(LongCount - 1, LongCount);
        public T GetAt(IContainerLocation location) => GetAtIndex(((IndexLocation)location).Index);
        public void SetAt(IContainerLocation location, T value) => SetAtIndex(((IndexLocation)location).Index, value);

        public bool ShouldSplit(long splitThreshold)
        {
            return Count > splitThreshold;
        }

        public bool IsShorterThan(IValueContainer<T> second)
        {
            return Count < ((LazinatorLinkedList<T>)second).Count;
        }

        public virtual IValueContainer<T> SplitOff()
        {
            LazinatorLinkedList<T> partSplitOff = (LazinatorLinkedList<T>)CreateNewWithSameSettings();
            int numToMove = Count / 2;
            for (int i = 0; i < numToMove; i++)
            {
                partSplitOff.Add(this[0]);
                RemoveAt(0);
            }
            return partSplitOff;
        }

        public bool Unbalanced { get => false; set => throw new NotImplementedException(); }

        public (IContainerLocation location, bool found) FindContainerLocation(T value, MultivalueLocationOptions whichOne, IComparer<T> comparer) => this.MultivalueFindMatchOrNext(AllowDuplicates, value, whichOne, comparer);
        public bool GetValue(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer, out T match) => this.MultivalueGetValue(AllowDuplicates, item, whichOne, comparer, out match);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => this.SortedInsertOrReplace(AllowDuplicates, item, whichOne, comparer);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => this.MultivalueTryRemove(AllowDuplicates, item, whichOne, comparer);
        public bool TryRemoveAll(T item, IComparer<T> comparer) => this.MultivalueTryRemoveAll(AllowDuplicates, item, comparer);
        long IMultivalueContainer<T>.Count(T item, IComparer<T> comparer) => this.MultivalueCount(AllowDuplicates, item, comparer);
        public (IContainerLocation location, bool found) FindContainerLocation(T value, IComparer<T> comparer) => this.MultivalueFindMatchOrNext(AllowDuplicates, value, comparer);
        public bool GetValue(T item, IComparer<T> comparer, out T match) => this.MultivalueGetValue(AllowDuplicates, item, comparer, out match);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, IComparer<T> comparer) => this.SortedInsertOrReplace(AllowDuplicates, item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any, comparer);
        public bool TryRemove(T item, IComparer<T> comparer) => this.MultivalueTryRemove(AllowDuplicates, item, comparer);

        #endregion

        #region IIndexable

        public (long index, bool exists) FindIndex(T target, IComparer<T> comparer) => FindIndex(target, MultivalueLocationOptions.Any, comparer);

        public (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer)
        {
            var result = FindContainerLocation(target, whichOne, comparer);
            return (((IndexLocation)result.location).Index, result.found);

        }

        public void RemoveAt(long index)
        {
            RemoveAt((int)index);
        }

        #endregion
        
        #region IMultilevelReporter

        public IMultilevelReportReceiver MultilevelReporterParent { get; set; }

        protected void ConsiderMultilevelReport(long index)
        {
            if (index == 0)
                ReportFirstChanged();
            if (index >= LongCount - 1)
                ReportLastChanged();
        }

        protected void ReportFirstChanged()
        {
            if (Any())
                MultilevelReporterParent?.EndItemChanged(true, First(), this);
            else
                MultilevelReporterParent?.EndItemRemoved(true, this);
        }

        protected void ReportLastChanged()
        {
            if (Any())
                MultilevelReporterParent?.EndItemChanged(false, Last(), this);
            else
                MultilevelReporterParent?.EndItemRemoved(false, this);
        }

        #endregion

    }
}
