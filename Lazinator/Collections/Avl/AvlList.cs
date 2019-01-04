using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lazinator.Collections.Avl
{
    public partial class AvlList<T> : IAvlList<T>, IList<T>, ILazinatorCountableListable<T> where T : ILazinator
    { 
        public AvlList()
        {
            UnderlyingTree2 = (ILazinatorOrderedKeyable<Placeholder, T>) new AvlTree<Placeholder, T>();
        }

        public T this[int index]
        {
            get => GetAt(index);
            set => SetAt(index, value);
        }

        public long Count => UnderlyingTree2.Count;

        public bool IsReadOnly => false;

        int ICollection<T>.Count => (int) Count;

        public void Add(T item)
        {
            InsertAt(Count, item);
        }

        public void Clear()
        {
            UnderlyingTree2 = (ILazinatorOrderedKeyable<Placeholder, T>) new AvlTree<Placeholder, T>();
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
            if (UnderlyingTree2.Count > array.Length - arrayIndex + 1)
                throw new ArgumentException();
            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return UnderlyingTree2.GetValueEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return UnderlyingTree2.GetValueEnumerator();
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
            UnderlyingTree2.RemoveAt(index);
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

        public ILazinatorOrderedKeyable<Placeholder, T> UnderlyingTree22 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ILazinatorOrderedKeyable<Placeholder, T> UnderlyingTree2 { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void InsertAt(long index, T item)
        {
            UnderlyingTree2.InsertAtIndex(new Placeholder(), item, index);
        }

        public void RemoveAt(long index)
        {
            UnderlyingTree2.RemoveAt(index);
        }

        public IEnumerable<T> AsEnumerable(long skip)
        {
            if (skip > Count || skip < 0)
                throw new ArgumentException();
            if (Count == 0)
                yield break;
            var valueEnumerator = UnderlyingTree2.GetValueEnumerator(skip);
            while (valueEnumerator.MoveNext())
                yield return valueEnumerator.Current;
        }

        public T GetAt(long index)
        {
            return UnderlyingTree2.ValueAtIndex(index);
        }

        public void SetAt(long index, T value)
        {
            UnderlyingTree2.SetValueAtIndex(index, value);
        }

        #endregion

    }
}
