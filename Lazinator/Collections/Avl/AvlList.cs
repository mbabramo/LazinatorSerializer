using Lazinator.Collections.Factories;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lazinator.Collections.Avl
{
    public partial class AvlList<T> : IAvlList<T>, IList<T>, ILazinatorCountableListable<T> where T : ILazinator
    { 
        public AvlList(ILazinatorOrderedKeyableFactory<Placeholder, T> factory = null)
        {
            if (factory == null)
                factory = (ILazinatorOrderedKeyableFactory<Placeholder, T>)new AvlTreeFactory<Placeholder, T>();
            UnderlyingTree = factory.Create();
        }

        public T this[int index]
        {
            get => GetAt(index);
            set => SetAt(index, value);
        }

        public long Count => UnderlyingTree.Count;

        public bool IsReadOnly => false;

        int ICollection<T>.Count => (int) Count;

        public void Add(T item)
        {
            InsertAt(Count, item);
        }

        public void Clear()
        {
            UnderlyingTree = (ILazinatorOrderedKeyable<Placeholder, T>) new AvlTree<Placeholder, T>();
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
            return UnderlyingTree.GetValueEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return UnderlyingTree.GetValueEnumerator();
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

        #region ILazinatorCountableListableFactory 

        public long LongCount => Count;

        public void InsertAt(long index, T item)
        {
            UnderlyingTree.InsertAtIndex(new Placeholder(), item, index);
        }

        public void RemoveAt(long index)
        {
            UnderlyingTree.RemoveAt(index);
        }

        public IEnumerable<T> AsEnumerable(long skip)
        {
            if (skip > Count || skip < 0)
                throw new ArgumentException();
            if (Count == 0)
                yield break;
            var valueEnumerator = UnderlyingTree.GetValueEnumerator(skip);
            while (valueEnumerator.MoveNext())
                yield return valueEnumerator.Current;
        }

        public T GetAt(long index)
        {
            return UnderlyingTree.ValueAtIndex(index);
        }

        public void SetAt(long index, T value)
        {
            UnderlyingTree.SetValueAtIndex(index, value);
        }

        #endregion

    }
}
