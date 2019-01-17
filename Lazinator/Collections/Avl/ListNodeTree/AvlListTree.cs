using Lazinator.Buffers;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tree;
using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ListNodeTree
{
    public partial class AvlListTree<T> : IAvlListTree<T>, IIndexableContainer<T>, ILazinatorSplittable where T : ILazinator
    {
        public bool Unbalanced { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AllowDuplicates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public long LongCount => throw new NotImplementedException();

        public IEnumerable<T> AsEnumerable(bool reverse = false, long skip = 0)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public IValueContainer<T> CreateNewWithSameSettings()
        {
            throw new NotImplementedException();
        }

        public (long index, bool exists) Find(T target, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public T GetAt(long index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator(bool reverse = false, long skip = 0)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool GetValue(T item, IComparer<T> comparer, out T match)
        {
            throw new NotImplementedException();
        }

        public void InsertAt(long index, T item)
        {
            throw new NotImplementedException();
        }

        public (long index, bool insertedNotReplaced) InsertGetIndex(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(long index)
        {
            throw new NotImplementedException();
        }

        public void SetAt(long index, T value)
        {
            throw new NotImplementedException();
        }

        public ILazinatorSplittable SplitOff()
        {
            throw new NotImplementedException();
        }

        public bool TryInsert(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public bool TryRemove(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

}