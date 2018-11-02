﻿using Lazinator.Core;
using System;

namespace Lazinator.Collections
{
    public partial class LazinatorQueue<T> : LazinatorList<T>, ILazinatorQueue<T> where T : ILazinator
    {
        debug; // UpdateStoredBuffer is not inheriting properties -- why not? 

        public void Enqueue(T item)
        {
            Add(item);
        }

        public T Dequeue()
        {
            if (Count == 0)
                throw new Exception("No item to dequeue.");
            T item = this[0];
            RemoveAt(0);
            return item;
        }

        public T Peek()
        {
            if (Count == 0)
                throw new Exception("No item to dequeue.");
            T item = this[0];
            return item;
        }
    }
}
