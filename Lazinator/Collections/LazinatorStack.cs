using Lazinator.Core;
using System;

namespace Lazinator.Collections
{
    public partial class LazinatorStack<T> : LazinatorList<T>, ILazinatorStack<T> where T : ILazinator
    {
        public void Push(T item)
        {
            Add(item);
        }

        public T Pop()
        {
            if (Count == 0)
                throw new Exception("No item to dequeue.");
            T item = this[Count - 1];
            RemoveAt(Count - 1);
            return item;
        }

        public T Peek()
        {
            if (Count == 0)
                throw new Exception("No item to dequeue.");
            T item = this[Count - 1];
            return item;
        }
    }
}
