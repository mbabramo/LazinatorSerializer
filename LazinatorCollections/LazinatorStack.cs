using Lazinator.Core;
using System;

namespace LazinatorCollections
{
    public partial class LazinatorStack<T> : ILazinatorStack<T> where T : ILazinator
    {
        public ILazinatorListable<T> UnderlyingList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LazinatorStack()
        {
            UnderlyingList = new LazinatorList<T>();
        }

        public LazinatorStack(ILazinatorListable<T> underlyingList)
        {
            UnderlyingList = underlyingList;
        }

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
            if (UnderlyingList.Count == 0)
                throw new Exception("No item to dequeue.");
            T item = UnderlyingList.GetAtIndex(UnderlyingList.LongCount - 1);
            return item;
        }
    }
}
