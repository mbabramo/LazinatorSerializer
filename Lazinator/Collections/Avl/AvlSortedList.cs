using Lazinator.Buffers;
using Lazinator.Collections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lazinator.Collections.Avl
{
    public partial class AvlSortedList<T> : IAvlSortedList<T>, IList<T>, ILazinatorSorted<T> where T : ILazinator, IComparable<T>
    {
        public AvlSortedList()
        {
        }

        public AvlSortedList(bool allowDuplicates, ISortedIndexableMultivalueContainerFactory<T> factory)
        {
            UnderlyingTree = factory.CreateSortedIndexableMultivalueContainer();
            AllowDuplicates = allowDuplicates;
        }

        public AvlSortedList(bool allowDuplicates, ISortedIndexableMultivalueContainer<T> underlyingTree)
        {
            UnderlyingTree = underlyingTree;
            AllowDuplicates = allowDuplicates;
        }

        public bool AllowDuplicates
        {
            get => UnderlyingTree.AllowDuplicates;
            set
            {
                if (value != UnderlyingTree.AllowDuplicates)
                    throw new Exception("AllowDuplicates must be same for sorted list and underlying tree.");
            }
        }


        public T this[int index]
        {
            get => GetAt(index);
            set => SetAt(index, value);
        }

        public long LongCount => UnderlyingTree.LongCount;

        public bool IsReadOnly => false;

        int ICollection<T>.Count => (int)LongCount;

        public void Add(T item)
        {
            InsertAt(LongCount, item);
        }

        public void Clear()
        {
            UnderlyingTree.Clear();
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
            if (UnderlyingTree.LongCount > array.Length - arrayIndex + 1)
                throw new ArgumentException();
            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return UnderlyingTree.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return UnderlyingTree.GetEnumerator();
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

        public void InsertAt(long index, T item)
        {
            UnderlyingTree.InsertAt(index, item);
        }

        public void RemoveAt(long index)
        {
            UnderlyingTree.RemoveAt(index);
        }

        public IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            foreach (T item in UnderlyingTree.AsEnumerable(reverse, skip))
                yield return item;
        }

        public T GetAt(long index)
        {
            return UnderlyingTree.GetAt(index);
        }

        public void SetAt(long index, T value)
        {
            UnderlyingTree.SetAt(index, value);
        }


        public bool Any()
        {
            return UnderlyingTree.Any();
        }

        public T First()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return this.GetAt(0); ;
        }

        public T FirstOrDefault()
        {
            if (Any())
                return this.GetAt(0);
            return default(T);
        }

        public T Last()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return this.GetAt(LongCount - 1);
        }

        public T LastOrDefault()
        {
            if (Any())
                return this.GetAt(LongCount - 1);
            return default(T);
        }

        public (long index, bool insertedNotReplaced) InsertGetIndex(T item) => InsertGetIndex(item, Comparer<T>.Default);
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, MultivalueLocationOptions whichOne) => InsertGetIndex(item, whichOne, Comparer<T>.Default);
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, IComparer<T> comparer) => UnderlyingTree.InsertGetIndex(item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any, comparer);
        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => UnderlyingTree.InsertGetIndex(item, whichOne, comparer);

        public bool TryRemove(T item) => TryRemove(item, Comparer<T>.Default);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne) => TryRemove(item, whichOne, Comparer<T>.Default);
        public bool TryRemove(T item, IComparer<T> comparer) => UnderlyingTree.TryRemove(item, comparer);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => UnderlyingTree.TryRemove(item, whichOne, comparer);

        public (long index, bool exists) Find(T target) => UnderlyingTree.Find(target, MultivalueLocationOptions.First, Comparer<T>.Default);
        public (long index, bool exists) Find(T target, MultivalueLocationOptions whichOne) => UnderlyingTree.Find(target, whichOne, Comparer<T>.Default);
        public (long index, bool exists) Find(T target, IComparer<T> comparer) => UnderlyingTree.Find(target, comparer);
        public (long index, bool exists) Find(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer) => UnderlyingTree.Find(target, whichOne, comparer);

    }
}
