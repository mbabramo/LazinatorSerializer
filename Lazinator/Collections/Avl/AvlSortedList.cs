using Lazinator.Buffers;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lazinator.Collections.Avl
{
    public partial class AvlSortedList<T> : IAvlSortedList<T>, IList<T>, ILazinatorSortable<T> where T : ILazinator, IComparable<T>
    {
        public AvlSortedList()
        {
        }

        public AvlSortedList(bool allowDuplicates)
        {
            UnderlyingTree = new AvlTree<T, Placeholder>();
            UnderlyingTree.AllowDuplicateKeys = allowDuplicates;
        }

        public bool AllowDuplicates
        {
            get
            {
                return UnderlyingTree.AllowDuplicateKeys;
            }
            set
            {
                UnderlyingTree.AllowDuplicateKeys = value;
            }
        }

        public T this[int index]
        {
            get => GetAt(index);
            set => SetAt(index, value);
        }

        public long Count => UnderlyingTree.Count;

        public bool IsReadOnly => false;

        int ICollection<T>.Count => (int)Count;

        public void Add(T item)
        {
            InsertAt(Count, item);
        }

        public void Clear()
        {
            bool allowDuplicates = AllowDuplicates;
            UnderlyingTree = new AvlTree<T, Placeholder>();
            UnderlyingTree.AllowDuplicateKeys = allowDuplicates;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException();
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException();
            if (UnderlyingTree.Count > array.Length - arrayIndex + 1)
                throw new ArgumentException();
            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return UnderlyingTree.GetKeyEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return UnderlyingTree.GetKeyEnumerator();
        }

        public int IndexOf(T item)
        {
            int i = 0;
            foreach (T existingItem in this)
            {
                if (item == null)
                {
                    if (existingItem == null)
                        return i;
                }
                else
                {
                    if (item.Equals(existingItem))
                        return i;
                }
                i++;
            }
            return -1;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;
            UnderlyingTree.RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            RemoveAt((long)index);
        }

        public void Insert(int index, T item)
        {
            InsertAt((long)index, item);
        }

        #region ILazinatorCountableListableFactory 

        public long LongCount => Count;

        public void InsertAt(long index, T item)
        {
            UnderlyingTree.InsertAtIndex(item, new Placeholder(), index);
        }

        public void RemoveAt(long index)
        {
            UnderlyingTree.RemoveAt(index);
        }

        public IEnumerable<T> AsEnumerable(long skip = 0)
        {
            if (skip > Count || skip < 0)
                throw new ArgumentException();
            if (Count == 0)
                yield break;
            foreach (var key in UnderlyingTree.Keys(skip))
                yield return key;
        }

        public T GetAt(long index)
        {
            return UnderlyingTree.KeyAtIndex(index);
        }

        public void SetAt(long index, T value)
        {
            UnderlyingTree.SetKeyAtIndex(index, value);
        }

        public (long location, bool rejectedAsDuplicate) InsertSorted(T item) => InsertSorted(item, Comparer<T>.Default);

        public (long location, bool rejectedAsDuplicate) InsertSorted(T item, IComparer<T> comparer)
        {
            (bool inserted, long location) = UnderlyingTree.Insert(item, comparer, default);
            return (location, !inserted);
        }

        public (long priorLocation, bool existed) RemoveSorted(T item) => RemoveSorted(item, Comparer<T>.Default);

        public (long priorLocation, bool existed) RemoveSorted(T item, IComparer<T> comparer)
        {
            (long location, bool exists) = FindSorted(item, comparer);
            if (exists)
            {
                RemoveAt(location);
                return (location, true);
            }
            return (-1, false);
        }

        // DEBUG -- must implement Comparer usage

        public (long location, bool exists) FindSorted(T target) => FindSorted(target, Comparer<T>.Default);

        public (long location, bool exists) FindSorted(T target, IComparer<T> comparer)
        {
            var result = UnderlyingTree.GetMatchingOrNextNode(target, comparer);
            return (result.index, result.found);
        }

        #endregion

    }
}
