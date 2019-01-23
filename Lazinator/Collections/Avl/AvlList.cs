using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Factories;
using Lazinator.Collections.Interfaces;
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
            UnderlyingTree = new AvlIndexableTree<T>(true, false);
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
            if (UnderlyingTree is ListTree.AvlIndexableListTree<WInt>)
            {
                var DEBUG = (ListTree.AvlIndexableListTree<WInt>)UnderlyingTree;
                var DEBUG2 = DEBUG.UnderlyingTree.ToTreeString();
                System.Diagnostics.Debug.WriteLine($"\nInserting {item}\n{DEBUG2}");
            }
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
