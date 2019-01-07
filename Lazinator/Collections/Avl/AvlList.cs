﻿using Lazinator.Collections.Factories;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lazinator.Collections.Avl
{
    public partial class AvlList<T> : IAvlList<T>, IList<T>, ILazinatorListable<T> where T : ILazinator
    {
        public AvlList()
        {
            UnderlyingTree = new AvlIndexableTree<T>();
        }

        public AvlList(AvlIndexableTree<T> underlyingTree)
        {
            UnderlyingTree = underlyingTree;
        }

        public T this[int index]
        {
            get => GetAt(index);
            set => SetAt(index, value);
        }

        public long Count => UnderlyingTree.LongCount;

        public bool IsReadOnly => false;

        int ICollection<T>.Count => (int) Count;

        public void Add(T item)
        {
            InsertAt(Count, item);
        }

        public void Clear()
        {
            UnderlyingTree = new AvlIndexableTree<T>();
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
            InsertAt((long) index, item);
        }

        #region ILazinatorListable 

        public long LongCount => Count;

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
            foreach (var item in UnderlyingTree.AsEnumerable(reverse, skip))
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

        public virtual ILazinatorSplittable SplitOff()
        {
            AvlList<T> partSplitOff = new AvlList<T>((AvlIndexableTree<T>) UnderlyingTree.SplitOff());
            return partSplitOff;
        }

        #endregion

    }
}
