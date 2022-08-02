using Lazinator.Buffers;
using Lazinator.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Lazinator.Collections
{
    /// <summary>
    /// A queue of Lazinator objects
    /// </summary>
    /// <typeparam name="T">The type of the Lazinator objects</typeparam>
    public partial class LazinatorQueue<T> : IEnumerable<T>, ILazinatorQueue<T> where T : ILazinator
    {
        public LazinatorQueue()
        {
            UnderlyingList = new LazinatorList<T>();
        }

        public void Enqueue(T item)
        {
            UnderlyingList.Add(item);
        }

        public bool Any() => LongCount > 0;
        public int Count => UnderlyingList.Count;
        public long LongCount => UnderlyingList.LongCount;

        public T Dequeue()
        {
            if (!UnderlyingList.Any())
                throw new Exception("Nothing to dequeue.");
            T item = UnderlyingList.First();
            UnderlyingList.RemoveAtIndex(0);
            return item;
        }

        public T Peek()
        {
            if (Count == 0)
                throw new Exception("No item to dequeue.");
            T item = UnderlyingList.First();
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
