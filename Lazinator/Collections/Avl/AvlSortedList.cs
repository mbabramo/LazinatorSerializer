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

        public AvlSortedList(bool allowDuplicates, SortedContainerFactory<T> innerContainerFactory)
        {
            UnderlyingTree = (ISortedIndexableMultivalueContainer<T>) innerContainerFactory.CreateValueContainer();
            AllowDuplicates = allowDuplicates;
        }

        public AvlSortedList(bool allowDuplicates, ISortedIndexableMultivalueContainer<T> underlyingTree)
        {
            UnderlyingTree = underlyingTree;
            AllowDuplicates = allowDuplicates;
        }

        public T this[int index]
        {
            get => GetAtIndex(index);
            set => SetAtIndex(index, value);
        }

        public long LongCount => UnderlyingTree.LongCount;

        public bool IsReadOnly => false;

        int ICollection<T>.Count => (int)LongCount;

        public void Add(T item)
        {
            InsertAtIndex(LongCount, item);
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
            RemoveAtIndex((long)index);
        }

        public void Insert(int index, T item)
        {
            InsertAtIndex((long)index, item);
        }

        public void InsertAtIndex(long index, T item)
        {
            UnderlyingTree.InsertAtIndex(index, item);
        }

        public void RemoveAtIndex(long index)
        {
            UnderlyingTree.RemoveAt(index);
        }

        public IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            foreach (T item in UnderlyingTree.AsEnumerable(reverse, skip))
                yield return item;
        }

        public T GetAtIndex(long index)
        {
            return UnderlyingTree.GetAtIndex(index);
        }

        public void SetAtIndex(long index, T value)
        {
            UnderlyingTree.SetAtIndex(index, value);
        }


        public bool Any()
        {
            return UnderlyingTree.Any();
        }

        public T First()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return this.GetAtIndex(0); ;
        }

        public T FirstOrDefault()
        {
            if (Any())
                return this.GetAtIndex(0);
            return default(T);
        }

        public T Last()
        {
            if (!Any())
                throw new Exception("The list is empty.");
            return this.GetAtIndex(LongCount - 1);
        }

        public T LastOrDefault()
        {
            if (Any())
                return this.GetAtIndex(LongCount - 1);
            return default(T);
        }

        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item) => InsertOrReplace(item, Comparer<T>.Default);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne) => InsertOrReplace(item, whichOne, Comparer<T>.Default);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, IComparer<T> comparer) => UnderlyingTree.InsertOrReplace(item, AllowDuplicates ? MultivalueLocationOptions.InsertAfterLast : MultivalueLocationOptions.Any, comparer);
        public (IContainerLocation location, bool insertedNotReplaced) InsertOrReplace(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => UnderlyingTree.InsertOrReplace(item, whichOne, comparer);

        public bool TryRemove(T item) => TryRemove(item, Comparer<T>.Default);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne) => TryRemove(item, whichOne, Comparer<T>.Default);
        public bool TryRemove(T item, IComparer<T> comparer) => UnderlyingTree.TryRemove(item, comparer);
        public bool TryRemove(T item, MultivalueLocationOptions whichOne, IComparer<T> comparer) => UnderlyingTree.TryRemove(item, whichOne, comparer);

        public (long index, bool exists) FindIndex(T target) => UnderlyingTree.FindIndex(target, MultivalueLocationOptions.First, Comparer<T>.Default);
        public (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne) => UnderlyingTree.FindIndex(target, whichOne, Comparer<T>.Default);
        public (long index, bool exists) FindIndex(T target, IComparer<T> comparer) => UnderlyingTree.FindIndex(target, comparer);
        public (long index, bool exists) FindIndex(T target, MultivalueLocationOptions whichOne, IComparer<T> comparer) => UnderlyingTree.FindIndex(target, whichOne, comparer);

    }
}
