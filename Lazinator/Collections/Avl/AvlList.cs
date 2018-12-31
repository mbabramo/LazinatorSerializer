using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lazinator.Collections.Avl
{
    public partial class AvlList<T> : IAvlList<T>, IList<T> where T : ILazinator
    { 
        public AvlList()
        {
            UnderlyingTree = new AvlTree<Placeholder, T>();
        }

        public T this[int index]
        {
            get => UnderlyingTree.NodeAtIndex(index).Value;
            set => UnderlyingTree.NodeAtIndex(index).Value = value;
        }

        public int Count => UnderlyingTree.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void Clear()
        {
            UnderlyingTree = new AvlTree<Placeholder, T>();
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
            return new AvlNodeValueEnumerator<T>(UnderlyingTree.GetEnumerator() as AvlNodeEnumerator<Placeholder, T>);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new AvlNodeValueEnumerator<T>(UnderlyingTree.GetEnumerator() as AvlNodeEnumerator<Placeholder, T>);
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

        public void Insert(int index, T item)
        {
            UnderlyingTree.Insert(new Placeholder(), item, index);
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;
            UnderlyingTree.Delete(new Placeholder(), index);
            return true;
        }

        public void RemoveAt(int index)
        {
            UnderlyingTree.Delete(new Placeholder(), index);
        }
    }
}
