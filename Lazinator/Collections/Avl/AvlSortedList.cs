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
            // DEBUG -- preferable not to create data in constructors. Change this throughout
            UnderlyingTree = new AvlTree<T, Placeholder>();
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
            UnderlyingTree = new AvlTree<T, Placeholder>();
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
            var underlyingEnumerator = UnderlyingTree.GetEnumerator() as AvlNodeEnumerator<T, Placeholder>;
            return new AvlNodeKeyEnumerator<T>(underlyingEnumerator);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var underlyingEnumerator = UnderlyingTree.GetEnumerator() as AvlNodeEnumerator<T, Placeholder>;
            return new AvlNodeKeyEnumerator<T>(underlyingEnumerator);
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
            UnderlyingTree.Remove(item, index);
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
            UnderlyingTree.Insert(item, new Placeholder(), index);
        }

        public void RemoveAt(long index)
        {
            UnderlyingTree.Remove(default, index);
        }

        public IEnumerable<T> EnumerateFrom(long index)
        {
            if (index > Count || index < 0)
                throw new ArgumentException();
            if (Count == 0)
                yield break;
            foreach (var node in UnderlyingTree.Skip(index))
                yield return node.Key;
        }

        public T GetAt(long index)
        {
            return UnderlyingTree.NodeAtIndex(index).Key;
        }

        public void SetAt(long index, T value)
        {
            UnderlyingTree.NodeAtIndex(index).Key = value;
        }

        public (long location, bool rejectedAsDuplicate) InsertSorted(T item)
        {
            if (AllowDuplicates)
            {
                (bool inserted, long location) = UnderlyingTree.Insert(item, default);
                return (location, !inserted);
            }
            else
            {
                (long location, bool exists) = FindSorted(item);
                if (exists)
                    return (location, true);
                UnderlyingTree.Insert(item, default, location);
                return (location, exists);
            }
        }

        public (long priorLocation, bool existed) RemoveSorted(T item)
        {
            throw new NotImplementedException();
        }

        public (long location, bool exists) FindSorted(T target)
        {
            var result = UnderlyingTree.SearchMatchOrNext(target);
            return (result.index, result.found);
        }

        #endregion

    }
}
