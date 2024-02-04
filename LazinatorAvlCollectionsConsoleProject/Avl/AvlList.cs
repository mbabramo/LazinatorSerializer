using LazinatorAvlCollections.Avl.ValueTree;
using LazinatorAvlCollections.Factories;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl
{
    /// <summary>
    /// An Avl list, where the list is implemented by an underlying balanced Avl tree structure. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class AvlList<T> : IAvlList<T>, IList<T>, ILazinatorListable<T> where T : ILazinator
    {
        public AvlList()
        {
            UnderlyingTree = new AvlIndexableTree<T>(true, false, true);
        }

        public AvlList(ContainerFactory factory)
        {
            UnderlyingTree = (IIndexableValueContainer<T>) factory.CreateValueContainer<T>();
        }

        public AvlList(AvlIndexableTree<T> underlyingTree)
        {
            UnderlyingTree = underlyingTree;
        }

        public T this[int index]
        {
            get => GetAtIndex(index);
            set => SetAtIndex(index, value);
        }

        public long Count => UnderlyingTree.LongCount;

        public bool IsReadOnly => false;

        int ICollection<T>.Count => (int) Count;

        public void Add(T item)
        {
            InsertAtIndex(Count, item);
        }

        public void Clear()
        {
            var replacement = UnderlyingTree.CreateNewWithSameSettings();
            UnderlyingTree = (IIndexableValueContainer<T>) replacement;
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

        public IEnumerator<T> GetEnumerator(bool reverse, T startValue, IComparer<T> comparer) => AsEnumerable(reverse, startValue, comparer).GetEnumerator();

        public IEnumerable<T> AsEnumerable(bool reverse, T startValue, IComparer<T> comparer)
        {
            foreach (T t in UnderlyingTree.AsEnumerable(reverse, startValue, comparer))
                yield return t;
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
            InsertAtIndex((long) index, item);
        }

        #region ILazinatorListable 

        public long LongCount => Count;

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
            foreach (var item in UnderlyingTree.AsEnumerable(reverse, skip))
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

        #endregion

    }
}
