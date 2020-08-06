using Lazinator.Buffers;
using Lazinator.Core;
using System;
using System.Collections.Generic;

namespace LazinatorCollections
{
    /// <summary>
    /// A LIFO stack of Lazinator objects
    /// </summary>
    /// <typeparam name="T">The type of the Lazinator objects</typeparam>
    public partial class LazinatorStack<T> : ILazinatorStack<T> where T : ILazinator
    {
        public LazinatorStack()
        {
            UnderlyingList = new LazinatorList<T>();
        }

        public LazinatorStack(ILazinatorListable<T> underlyingList)
        {
            UnderlyingList = underlyingList;
        }

        public bool Any() => LongCount > 0;
        public int Count => UnderlyingList.Count;
        public long LongCount => UnderlyingList.LongCount;

        public void Push(T item)
        {
            UnderlyingList.Add(item);
        }

        public T Pop()
        {
            if (UnderlyingList.LongCount == 0)
                throw new Exception("No item to dequeue.");
            T item = UnderlyingList.GetAtIndex(UnderlyingList.LongCount - 1);
            UnderlyingList.RemoveAtIndex(UnderlyingList.LongCount - 1);
            return item;
        }

        public T Peek()
        {
            if (UnderlyingList.LongCount == 0)
                throw new Exception("No item to dequeue.");
            T item = UnderlyingList.GetAtIndex(UnderlyingList.LongCount - 1);
            return item;
        }
    }
}
