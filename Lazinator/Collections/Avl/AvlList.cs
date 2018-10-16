using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lazinator.Collections.Avl
{
    public partial class AvlList<T> : IAvlList<T>, IList<T> where T : ILazinator, new()
    { 
        public AvlList()
        {
            UnderlyingTree = new AvlTree<WByte, T>();
        }

        public T this[int index]
        {
            get => UnderlyingTree[index].Value;
            set => UnderlyingTree[index].Value = value;
        }

        public int Count => UnderlyingTree.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void Clear()
        {
            UnderlyingTree = new AvlTree<WByte, T>();
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
            return new AvlNodeValueEnumerator<T>(UnderlyingTree.GetEnumerator() as AvlNodeEnumerator<WByte, T>);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new AvlNodeValueEnumerator<T>(UnderlyingTree.GetEnumerator() as AvlNodeEnumerator<WByte, T>);
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
            UnderlyingTree.Insert(new WByte(0), item, index);
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;
            UnderlyingTree.Delete(new WByte(0), index);
            return true;
        }

        public void RemoveAt(int index)
        {
            UnderlyingTree.Delete(new WByte(0), index);
        }
    }
}
