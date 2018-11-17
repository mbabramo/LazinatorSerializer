using Lazinator.Collections.Avl;
using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Collections
{
    public partial class LazinatorQueue<T> : IEnumerable<T>, ILazinatorQueue<T> where T : ILazinator
    {
        private AvlList<T> UnderlyingList;

        public LazinatorQueue()
        {
            UnderlyingList = new AvlList<T>();
        }

        public void Enqueue(T item)
        {
            UnderlyingList.Add(item);
        }

        public int Count => UnderlyingList.Count;

        public T Dequeue()
        {
            if (!UnderlyingList.Any())
                throw new Exception("Nothing to dequeue.");
            T item = UnderlyingList[0];
            UnderlyingList.RemoveAt(0);
            return item;
        }

        public T Peek()
        {
            if (Count == 0)
                throw new Exception("No item to dequeue.");
            T item = UnderlyingList[0];
            return item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)UnderlyingList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)UnderlyingList).GetEnumerator();
        }
    }
}
